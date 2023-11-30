using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Fta.DemoFunc.Api.Interfaces;
using Npgsql;

namespace Fta.DemoFunc.Api.Persistence
{
    public class NpgsqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public NpgsqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IDbConnection> CreateConnectionAsync(CancellationToken ct = default)
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(ct);

            return connection;
        }
    }
}
