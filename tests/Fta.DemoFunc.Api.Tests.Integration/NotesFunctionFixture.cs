using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using System.IO;
using System;
using System.Threading.Tasks;
using Testcontainers.CosmosDb;
using Xunit;

namespace Fta.DemoFunc.Api.Tests.Integration
{
    public class NotesFunctionFixture : IAsyncLifetime, IDisposable
    {
        private readonly IOutputConsumer _outputConsumer = Consume.RedirectStdoutAndStderrToStream(new MemoryStream(), new MemoryStream());
        private readonly CosmosDbContainer _cosmosDbContainer;
        private readonly NotificationApiServer _notificationApiServer = new();

        public NotesFunctionFixture()
        {
            _cosmosDbContainer = new CosmosDbBuilder()
                .WithImage("mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator:latest")
                .WithName("Cosmos_DB")
                .WithExposedPort(8081)
                .WithPortBinding(8081, 8081)
                .WithEnvironment("AZURE_COSMOS_EMULATOR_PARTITION_COUNT", "1")
                .WithEnvironment("AZURE_COSMOS_EMULATOR_IP_ADDRESS_OVERRIDE", "127.0.0.1")
                .WithEnvironment("AZURE_COSMOS_EMULATOR_ENABLE_DATA_PERSISTENCE", "false")
                .WithOutputConsumer(_outputConsumer)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("Started"))
                .Build();
        }

        public string GetCosmosDbConnectionString() => _cosmosDbContainer.GetConnectionString();
        public string GetNotificationApiServerUrl() => _notificationApiServer.Url;

        public async Task DisposeAsync()
        {
            await _cosmosDbContainer.DisposeAsync();
            _notificationApiServer.Dispose();
        }

        public async Task InitializeAsync()
        {
            _notificationApiServer.Start();
            _notificationApiServer.SetupRequestDetails();
            await _cosmosDbContainer.StartAsync();
        }

        public void Dispose()
        {
            _outputConsumer.Dispose();
        }
    }
}
