using DotNet.Testcontainers.Builders;
using System.Threading.Tasks;
using Xunit;
using Testcontainers.PostgreSql;

namespace Fta.DemoFunc.Api.Tests.Integration
{
    public class NotesFunctionFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgreSqlContainer =
            new PostgreSqlBuilder()
                .WithImage("postgres:latest")
                .WithEnvironment("POSTGRES_USER", "postgres")
                .WithEnvironment("POSTGRES_PASSWORD", "postgres")
                .WithEnvironment("POSTGRES_DB", "notesdb")
                .WithPortBinding(5432, 5432)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
                .Build();
        private readonly NotificationApiServer _notificationApiServer = new();

        public string GetPostgresDbConnectionString() => _postgreSqlContainer.GetConnectionString();
        public string GetNotificationApiServerUrl() => _notificationApiServer.Url;

        public async Task DisposeAsync()
        {
            await _postgreSqlContainer.DisposeAsync();
            _notificationApiServer.Dispose();
        }

        public async Task InitializeAsync()
        {
            _notificationApiServer.Start();
            _notificationApiServer.SetupRequestDetails();
            await _postgreSqlContainer.StartAsync();
        }
    }
}
