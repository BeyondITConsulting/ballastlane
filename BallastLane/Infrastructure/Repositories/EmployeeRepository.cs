using System.Data.SqlClient;
using System.Threading.Tasks;
using BallastLane.Domain.Entities;
using BallastLane.Domain.Interfaces;
using BallastLane.Infrastructure.Data;

namespace BallastLane.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public EmployeeRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Employees (FirstName, LastName, DateOfBirth) OUTPUT INSERTED.EmployeeId VALUES (@FirstName, @LastName, @DateOfBirth)";
                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);

                employee.EmployeeId = (int)await command.ExecuteScalarAsync();
                return employee;
            }
        }

        public async Task<bool> UpdateAsync(Employee employee)
        {
            var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE Employees SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth WHERE EmployeeId = @EmployeeId";
                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                command.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId";
                command.Parameters.AddWithValue("@EmployeeId", id);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Employees WHERE EmployeeId = @EmployeeId";
                command.Parameters.AddWithValue("@EmployeeId", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Employee
                        {
                            EmployeeId = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            DateOfBirth = reader.GetDateTime(3)
                        };
                    }
                }
            }
            return null;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var employees = new List<Employee>();

            var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Employees";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        employees.Add(new Employee
                        {
                            EmployeeId = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            DateOfBirth = reader.GetDateTime(3)
                        });
                    }
                }
            }

            return employees;
        }
    }
}
