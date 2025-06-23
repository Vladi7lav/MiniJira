using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace MiniJira.Infrastructure
{
    public static class DatabaseConnectionHelper
    {
        private const string ConnectionString = "Host=localhost;Database=mini_jira;Username=postgres;Password=qwe123";
        
        public static async Task<IDbConnection> GetOpenConnection(CancellationToken cancellationToken)
        {
            var connection = new NpgsqlConnection(ConnectionString);
            await connection.OpenAsync(cancellationToken);
            return connection;
        }
    }
}