using System.Data.SqlClient;
using System.Data;

namespace BallastLane.Domain.Interfaces
{
    public interface IDbConnectionWrapper
    {
        Task OpenAsync();
        SqlCommand CreateCommand();
        IDbConnection Connection { get; }
    }
}
