using FluentAssertions;
using Fta.DemoFunc.Api.Contracts.Requests;
using Fta.DemoFunc.Api.Contracts.Responses;
using Fta.DemoFunc.Api.Entities;
using Fta.DemoFunc.Api.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;

namespace Fta.DemoFunc.Api.Tests.Integration
{
    [Collection(IntegrationTestsCollection.Name)]
    public class NotesFunctionTests : IClassFixture<TestStartup>, IAsyncLifetime
    {
        private readonly NotesFunction? _sut;
        private readonly TestsInitializer _testsInitializer;
        private readonly CosmosClient _cosmosClient;
        private Container? _container;
        private string? _noteId;

        public NotesFunctionTests(TestsInitializer testsInitializer)
        {
            _testsInitializer = testsInitializer;

            var cosmosDbSettings = testsInitializer.ServiceProvider.GetService<CosmosDbSettings>();
            _cosmosClient = new CosmosClient(cosmosDbSettings!.ConnectionString);
            _sut = _testsInitializer.ServiceProvider.GetService<NotesFunction>();
        }

        public async Task InitializeAsync()
        {
            var databaseResponse = await _cosmosClient.CreateDatabaseIfNotExistsAsync("NoteDatabase");
            var database = databaseResponse.Database;

            var containerResponse = await database.CreateContainerIfNotExistsAsync("Notes", "/id");
            _container = containerResponse.Container;
        }

        [Fact]
        public async void Post_ShouldCreateNote_WhenCalledWithValidNoteDetails()
        {
            // Arrange
            var createValidNoteRequest = new CreateNoteRequest
            {
                Title = "Note Title from Integration Test",
                Body = "Note Body from Integration Test"
            };

            // Act
            var response = await _sut!.Post(createValidNoteRequest);
            var createdResult = (CreatedResult)response.Result;
            var createNoteResponse = createdResult.Value as CreateNoteResponse;
            _noteId = createNoteResponse!.Id;

            // Assert
            createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
            createNoteResponse.Title.Should().Be("Note Title from Integration Test");
            createNoteResponse.Body.Should().Be("Note Body from Integration Test");
        }

        [Fact]
        public async void Post_ShouldReturnBadRequest_WhenCalledWithInvalidNoteDetails()
        {
            // Arrange
            var createInvalidNoteRequest = new CreateNoteRequest();

            // Act
            var response = await _sut!.Post(createInvalidNoteRequest);
            var badRequestObjectResult = (BadRequestObjectResult)response.Result;

            // Assert
            badRequestObjectResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            badRequestObjectResult.Value.Should().Be("This HTTP triggered NotesFunction executed successfully, but you passed in a bad request model for the note creation process.");
        }

        public async Task DisposeAsync()
        {
            if (!string.IsNullOrEmpty(_noteId))
            {
                await _container!.DeleteItemAsync<Note>(_noteId, new PartitionKey(_noteId));
            }
        }
    }
}
