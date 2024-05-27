using Moq;
using BallastLane.Application.Dtos;
using BallastLane.Application.Interfaces;
using BallastLane.Application.Services;
using BallastLane.Domain.Entities;
using BallastLane.Domain.Interfaces;

namespace BallastLane.Test.Application
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly IEmployeeService _employeeService;

        public EmployeeServiceTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _employeeService = new EmployeeService(_employeeRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllEmployeesAsync_ShouldReturnAllEmployees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { EmployeeId = 1, FirstName = "Jose", LastName = "Garcia", DateOfBirth = new DateTime(1990, 1, 1) },
                new Employee { EmployeeId = 2, FirstName = "Astrid", LastName = "Garcia", DateOfBirth = new DateTime(1992, 2, 2) }
            };
            _employeeRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(employees);

            // Act
            var result = await _employeeService.GetAllEmployeesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ShouldReturnEmployee()
        {
            // Arrange
            var employee = new Employee { EmployeeId = 1, FirstName = "Jose", LastName = "Garcia", DateOfBirth = new DateTime(1990, 1, 1) };
            _employeeRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(employee);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.EmployeeId);
        }

        [Fact]
        public async Task CreateEmployeeAsync_ShouldReturnCreatedEmployee()
        {
            // Arrange
            var employeeDto = new EmployeeDto { FirstName = "Jose", LastName = "Garcia", DateOfBirth = new DateTime(1990, 1, 1) };
            var employee = new Employee { EmployeeId = 1, FirstName = "Jose", LastName = "Garcia", DateOfBirth = new DateTime(1990, 1, 1) };
            _employeeRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Employee>())).ReturnsAsync(employee);

            // Act
            var result = await _employeeService.CreateEmployeeAsync(employeeDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.EmployeeId);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ShouldReturnUpdatedEmployee()
        {
            // Arrange
            var employeeDto = new EmployeeDto { EmployeeId = 1, FirstName = "Jose", LastName = "Garcia", DateOfBirth = new DateTime(1990, 1, 1) };
            var employee = new Employee { EmployeeId = 1, FirstName = "Jose", LastName = "Garcia", DateOfBirth = new DateTime(1990, 1, 1) };
            _employeeRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Employee>())).ReturnsAsync(true);

            // Act
            var result = await _employeeService.UpdateEmployeeAsync(employeeDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ShouldReturnTrue()
        {
            // Arrange
            _employeeRepositoryMock.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _employeeService.DeleteEmployeeAsync(1);

            // Assert
            Assert.True(result);
        }
    }
}
