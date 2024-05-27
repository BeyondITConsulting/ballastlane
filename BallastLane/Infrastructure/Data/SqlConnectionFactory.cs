using BallastLane.Domain.Interfaces;
using System.Data.SqlClient;

namespace BallastLane.Infrastructure.Data
{
    public class SqlConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnectionWrapper CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            return new DbConnectionWrapper(connection);
        }

    }

}
