using Fta.DemoFunc.Api.Entities;
using Fta.DemoFunc.Api.Interfaces;
using Microsoft.Azure.Cosmos;
using System.Threading;
using System.Threading.Tasks;

namespace Fta.DemoFunc.Api.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly CosmosClient _cosmosClient;
        
        public NoteRepository(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        public async Task<Note> CreateAsync(Note note, CancellationToken ct)
        {
            var databaseResponse = await _cosmosClient.CreateDatabaseIfNotExistsAsync("NoteDatabase", cancellationToken: ct);
            var database = databaseResponse.Database;

            var containerResponse = await database.CreateContainerIfNotExistsAsync("Notes", "/id", cancellationToken: ct);
            var container = containerResponse.Container;

            var itemResponse = await container.CreateItemAsync(note, new PartitionKey(note.Id), cancellationToken: ct);

            return itemResponse.Resource;
        }
    }
}
