using BallastLane.Api.Controllers;
using BallastLane.Application.Dtos;
using BallastLane.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BallastLane.Tests.Api
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly EmployeesController _employeeController;

        public EmployeeControllerTests()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _employeeController = new EmployeesController(_employeeServiceMock.Object);
        }

        [Fact]
        public async Task GetAllEmployees_ShouldReturnOkResultWithEmployees()
        {
            // Arrange
            var employees = new List<EmployeeDto>
            {
                new EmployeeDto { EmployeeId = 1, FirstName = "Jose", LastName = "Garcia", DateOfBirth = new DateTime(1990, 1, 1) },
                new EmployeeDto { EmployeeId = 2, FirstName = "Astrid", LastName = "Garcia", DateOfBirth = new DateTime(1992, 2, 2) }
            };
            _employeeServiceMock.Setup(service => service.GetAllEmployeesAsync()).ReturnsAsync(employees);

            // Act
            var result = await _employeeController.GetAllEmployees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<EmployeeDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetEmployeeById_ShouldReturnOkResultWithEmployee()
        {
            // Arrange
            var employee = new EmployeeDto { EmployeeId = 1, FirstName = "Jose", LastName = "Garcia", DateOfBirth = new DateTime(1990, 1, 1) };
            _employeeServiceMock.Setup(service => service.GetEmployeeByIdAsync(1)).ReturnsAsync(employee);

            // Act
            var result = await _employeeController.GetEmployeeById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<EmployeeDto>(okResult.Value);
            Assert.Equal(1, returnValue.EmployeeId);
        }

        [Fact]
        public async Task CreateEmployee_ShouldReturnCreatedAtActionResult()
        {
            // Arrange
            var employeeDto = new EmployeeDto { FirstName = "Jose", LastName = "Garcia", DateOfBirth = new DateTime(1990, 1, 1) };
            var createdEmployeeDto = new EmployeeDto { EmployeeId = 1, FirstName = "Jose", LastName = "Garcia", DateOfBirth = new DateTime(1990, 1, 1) };
            _employeeServiceMock.Setup(service => service.CreateEmployeeAsync(employeeDto)).ReturnsAsync(createdEmployeeDto);

            // Act
            var result = await _employeeController.CreateEmployee(employeeDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<EmployeeDto>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.EmployeeId);
        }

        [Fact]
        public async Task UpdateEmployee_ShouldReturnOkResultWithUpdatedEmployee()
        {
            // Arrange
            var employeeDto = new EmployeeDto { EmployeeId = 1, FirstName = "Jose", LastName = "Garcia", DateOfBirth = new DateTime(1990, 1, 1) };
            var updatedEmployeeDto = new EmployeeDto { EmployeeId = 1, FirstName = "Jose", LastName = "Garcia", DateOfBirth = new DateTime(1990, 1, 1) };
            _employeeServiceMock.Setup(service => service.UpdateEmployeeAsync(employeeDto)).ReturnsAsync(true);

            // Act
            var result = await _employeeController.UpdateEmployee(1, employeeDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<bool>(okResult.Value);
            Assert.True(returnValue);
        }

        [Fact]
        public async Task DeleteEmployee_ShouldReturnNoContentResult()
        {
            // Arrange
            _employeeServiceMock.Setup(service => service.DeleteEmployeeAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _employeeController.DeleteEmployee(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
