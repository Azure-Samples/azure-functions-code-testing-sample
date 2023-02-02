using Fta.DemoFunc.Api.Entities;
using Fta.DemoFunc.Api.Interfaces;
using Fta.DemoFunc.Api.Settings;
using Microsoft.Azure.Cosmos;
using System.Threading;
using System.Threading.Tasks;

namespace Fta.DemoFunc.Api.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public NoteRepository(CosmosDbSettings cosmosDbSettings)
        {
            _cosmosClient = new CosmosClient(cosmosDbSettings.ConnectionString);
            _container = _cosmosClient.GetContainer("NoteDatabase", "Notes");
        }
        public async Task<Note> CreateAsync(Note note, CancellationToken ct)
        {
            var itemResponse = await _container.CreateItemAsync(note, new PartitionKey(note.Id), cancellationToken: ct);

            return itemResponse.Resource;
        }
    }
}
