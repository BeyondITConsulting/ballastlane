using BallastLane.Application.Dtos;
using BallastLane.Application.Interfaces;
using BallastLane.Domain.Entities;
using BallastLane.Domain.Interfaces;

namespace BallastLane.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto employeeDto)
        {
            var employee = MapToEmployeeEntity(employeeDto);
            var createdEmployee = await _employeeRepository.AddAsync(employee);
            return MapToEmployeeDto(createdEmployee);
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            return await _employeeRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return MapToEmployeeDtoList(employees);
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return employee != null ? MapToEmployeeDto(employee) : null;
        }

        public async Task<bool> UpdateEmployeeAsync(EmployeeDto employeeDto)
        {
            var employee = MapToEmployeeEntity(employeeDto);
            return await _employeeRepository.UpdateAsync(employee);
        }


        private Employee MapToEmployeeEntity(EmployeeDto employeeDto)
        {
            return new Employee
            {
                EmployeeId = employeeDto.EmployeeId,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                DateOfBirth = employeeDto.DateOfBirth
            };
        }

        private EmployeeDto MapToEmployeeDto(Employee employee)
        {
            return new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                DateOfBirth = employee.DateOfBirth
            };
        }

        private IEnumerable<EmployeeDto> MapToEmployeeDtoList(IEnumerable<Employee> employees)
        {
            var employeeDtos = new List<EmployeeDto>();
            foreach (var employee in employees)
            {
                employeeDtos.Add(MapToEmployeeDto(employee));
            }
            return employeeDtos;
        }
    }
}
