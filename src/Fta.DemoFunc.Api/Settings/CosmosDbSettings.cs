using Fta.DemoFunc.Api.Interfaces;

namespace Fta.DemoFunc.Api.Settings
{
    public class CosmosDbSettings : ICosmosDbSettings
    {
        public const string CosmosDbSectionKey = "CosmosDb";

        public string ConnectionString { get; set; } = default!;
    }
}
