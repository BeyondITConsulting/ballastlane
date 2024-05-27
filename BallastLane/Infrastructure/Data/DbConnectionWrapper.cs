using BallastLane.Domain.Interfaces;
using System.Data.SqlClient;
using System.Data;

namespace BallastLane.Infrastructure.Data
{
    public class DbConnectionWrapper : IDbConnectionWrapper
    {
        private readonly SqlConnection _connection;

        public DbConnectionWrapper(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task OpenAsync()
        {
            await _connection.OpenAsync();
        }

        public SqlCommand CreateCommand()
        {
            return _connection.CreateCommand();
        }

        public IDbConnection Connection => _connection;
    }

}
