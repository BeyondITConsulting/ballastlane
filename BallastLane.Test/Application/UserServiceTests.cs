using Moq;
using BallastLane.Domain.Entities;
using BallastLane.Domain.Interfaces;
using BallastLane.Application.Services;

namespace BallastLane.Test.Application
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateUser()
        {
            // Arrange
            var user = new User { Username = "testuser", Password = "password123" };
            _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync(user);

            // Act
            var result = await _userService.CreateUserAsync(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Username, result.Username);
            _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldAuthenticateUser_WhenCredentialsAreValid()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";
            var user = new User { Username = username, Password = password };

            _userRepositoryMock.Setup(r => r.GetByUsernameAndPasswordAsync(username, password)).ReturnsAsync(user);

            // Act
            var result = await _userService.AuthenticateUserAsync(username, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Username, result.Username);
            _userRepositoryMock.Verify(r => r.GetByUsernameAndPasswordAsync(username, password), Times.Once);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";

            _userRepositoryMock.Setup(r => r.GetByUsernameAndPasswordAsync(username, password)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.AuthenticateUserAsync(username, password);

            // Assert
            Assert.Null(result);
            _userRepositoryMock.Verify(r => r.GetByUsernameAndPasswordAsync(username, password), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, Username = "testuser" };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            _userRepositoryMock.Verify(r => r.GetByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.Null(result);
            _userRepositoryMock.Verify(r => r.GetByIdAsync(userId), Times.Once);
        }
    }
}
