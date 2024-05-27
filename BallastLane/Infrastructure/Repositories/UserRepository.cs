using BallastLane.Domain.Entities;
using BallastLane.Domain.Interfaces;
using BallastLane.Infrastructure.Data;
using System.Data.SqlClient;

namespace BallastLane.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public UserRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User> AddAsync(User user)
        {
            var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Users (Username, PasswordHash) OUTPUT INSERTED.Id VALUES (@Username, @Password)";
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password); // In a real-world application, ensure passwords are hashed

                user.Id = (int)await command.ExecuteScalarAsync();
                return user;
            }
        }

        public async Task<User> GetByUsernameAndPasswordAsync(string username, string password)
        {
            var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Users WHERE Username = @Username AND PasswordHash = @Password";
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password); // In a real-world application, ensure passwords are hashed

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new User
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Password = reader.GetString(2),
                        };
                    }
                }
            }
            return null;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Users WHERE UserId = @Id";
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new User
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Password = reader.GetString(2),
                        };
                    }
                }
            }
            return null;
        }
    }
}
