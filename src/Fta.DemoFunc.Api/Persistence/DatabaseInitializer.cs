using Dapper;
using Fta.DemoFunc.Api.Interfaces;
using System.Threading.Tasks;

namespace Fta.DemoFunc.Api.Persistence
{
    public class DatabaseInitializer
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DatabaseInitializer(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task InitializeAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            await connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Notes (
                    Id UUID PRIMARY KEY, 
                    Title VARCHAR(100) NOT NULL,
                    Body VARCHAR(1000) NOT NULL,
                    CreatedAt DATE NOT NULL,
                    LastUpdatedOn DATE NOT NULL
                )"
            );
        }
    }
}
