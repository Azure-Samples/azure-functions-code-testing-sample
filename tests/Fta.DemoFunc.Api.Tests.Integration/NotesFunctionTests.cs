using FluentAssertions;
using Fta.DemoFunc.Api.Contracts.Requests;
using Fta.DemoFunc.Api.Contracts.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Fta.DemoFunc.Api.Tests.Integration
{
    public class NotesFunctionTests : IClassFixture<NotesFunctionFixture>
    {
        private readonly NotesFunctionFixture _notesFunctionFixture;

        public NotesFunctionTests(NotesFunctionFixture notesFunctionFixture)
        {
            _notesFunctionFixture = notesFunctionFixture;
        }

        [Fact]
        public async void Post_ShouldCreateNote_WhenCalledWithValidNoteDetails()
        {
            var host = new HostBuilder()
                .ConfigureWebJobs(builder => builder.UseExternalStartup())
                .ConfigureDefaultTestHost(_notesFunctionFixture)
                .Build();

            using (host)
            {
                await host.StartAsync();

                var sut = host.Services.GetRequiredService<NotesFunction>();

                // Arrange
                var createValidNoteRequest = new CreateNoteRequest
                {
                    Title = "Test note title",
                    Body = "Test note description"
                };

                // Act
                var response = await sut.Post(createValidNoteRequest);

                // Assert
                var createdResult = (CreatedResult)response.Result!;
                var createNoteResponse = createdResult.Value as CreateNoteResponse;
                createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
                createNoteResponse!.Title.Should().Be("Test note title");
                createNoteResponse.Body.Should().Be("Test note description");
            }
        }

        [Fact]
        public async void Post_ShouldReturnBadRequest_WhenCalledWithInvalidNoteDetails()
        {
            var host = new HostBuilder()
                .ConfigureWebJobs(builder => builder.UseExternalStartup())
                .ConfigureDefaultTestHost(_notesFunctionFixture)
                .Build();

            using (host)
            {
                await host.StartAsync();

                var sut = host.Services.GetRequiredService<NotesFunction>();

                // Arrange
                var createInvalidNoteRequest = new CreateNoteRequest();

                // Act
                var response = await sut.Post(createInvalidNoteRequest);

                // Assert
                var badRequestObjectResult = (BadRequestObjectResult)response.Result!;
                badRequestObjectResult.Should().NotBeNull();
                badRequestObjectResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
                badRequestObjectResult.Value.Should().Be("This HTTP triggered NotesFunction executed successfully, but you passed in a bad request model for the note creation process.");
            }
        }
    }
}
