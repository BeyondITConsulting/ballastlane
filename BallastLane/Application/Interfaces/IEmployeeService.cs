using BallastLane.Application.Dtos;

namespace BallastLane.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto employeeDto);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto> GetEmployeeByIdAsync(int id);
        Task<bool> UpdateEmployeeAsync(EmployeeDto employeeDto);
    }
}
