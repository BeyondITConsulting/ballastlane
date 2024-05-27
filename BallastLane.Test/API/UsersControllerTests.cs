using Microsoft.AspNetCore.Mvc;
using Moq;
using BallastLane.Application.Interfaces;
using BallastLane.Domain.Entities;
using BallastLane.Application.Dtos;
using BallastLane.Api.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace BallastLane.Tests.Api
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UsersController _usersController;
        private readonly IConfiguration _configuration;

        public UsersControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();

            var inMemorySettings = new Dictionary<string, string> {
                {"Jwt:Secret", "this is my custom Secret key for authentication"},
                {"Jwt:Issuer", "your_issuer_here"},
                {"Jwt:Audience", "your_audience_here"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _usersController = new UsersController(_userServiceMock.Object, _configuration);
        }

        [Fact]
        public async Task Register_ValidDto_ReturnsCreated()
        {
            // Arrange
            var userDto = new UserDto { Username = "testuser", Password = "password123" };
            var user = new User { Id = 1, Username = userDto.Username };

            _userServiceMock.Setup(s => s.CreateUserAsync(It.IsAny<User>())).ReturnsAsync(user);

            // Act
            var result = await _usersController.Register(userDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var model = Assert.IsType<User>(createdResult.Value);
            Assert.Equal(user.Id, model.Id);
        }

        [Fact]
        public async Task Register_InvalidDto_ReturnsBadRequest()
        {
            // Arrange
            _usersController.ModelState.AddModelError("Username", "Username is required.");
            var userDto = new UserDto();

            // Act
            var result = await _usersController.Register(userDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "testuser", Password = "password123" };
            var user = new User { Id = 1, Username = loginDto.Username, Password = loginDto.Password };

            _userServiceMock.Setup(s => s.AuthenticateUserAsync(loginDto.Username, loginDto.Password))
                .ReturnsAsync(user);

            // Act
            var result = await _usersController.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var token = Assert.IsType<string>(okResult.Value);
            Assert.False(string.IsNullOrEmpty(token));



            // Validate the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true
            }, out SecurityToken validatedToken);

            Assert.NotNull(validatedToken);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "testuser", Password = "wrongpassword" };

            _userServiceMock.Setup(s => s.AuthenticateUserAsync(loginDto.Username, loginDto.Password)).ReturnsAsync((User)null);

            // Act
            var result = await _usersController.Login(loginDto);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task GetUserById_ExistingUser_ReturnsOk()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, Username = "testuser" };

            _userServiceMock.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _usersController.GetUserById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<User>(okResult.Value);
            Assert.Equal(userId, model.Id);
        }

        [Fact]
        public async Task GetUserById_NonExistingUser_ReturnsNotFound()
        {
            // Arrange
            var userId = 1;

            _userServiceMock.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _usersController.GetUserById(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
