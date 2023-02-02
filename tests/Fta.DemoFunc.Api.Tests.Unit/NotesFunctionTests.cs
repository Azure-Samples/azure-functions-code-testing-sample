using Fta.DemoFunc.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using FluentAssertions;
using NSubstitute.ReturnsExtensions;
using System.Web.Http;
using NSubstitute.ExceptionExtensions;
using Fta.DemoFunc.Api.Options;
using Fta.DemoFunc.Api.Dtos;
using Fta.DemoFunc.Api.Contracts.Requests;
using Fta.DemoFunc.Api.Contracts.Responses;

namespace Fta.DemoFunc.Api.Tests.Unit
{
    public class NotesFunctionTests
    {
        private readonly string _newNoteId = Guid.NewGuid().ToString();
        private readonly DateTime _newNoteLastUpdatedOn = DateTime.UtcNow;
        private readonly string _newNoteTitle = "mock title";
        private readonly string _newNoteBody = "mock body";
        private readonly NotesFunction _sut;
        private readonly ILoggerAdapter<NotesFunction> _logger = Substitute.For<ILoggerAdapter<NotesFunction>>();
        private readonly INoteService _noteService = Substitute.For<INoteService>();
        
        public NotesFunctionTests()
        {
            _sut = new NotesFunction(_noteService, _logger);
        }

        [Fact]
        public async Task Post_ShouldReturnOkObjectResultWithCreatedNoteDetails_WhenCalledWithValidNoteDetails()
        {
            // Arrange
            var expectedResult = new CreateNoteResponse
            {
                Id = _newNoteId,
                LastUpdatedOn = _newNoteLastUpdatedOn,
                Title = _newNoteTitle,
                Body = _newNoteBody
            };

            _noteService.CreateNoteAsync(Arg.Any<CreateNoteOptions>(), Arg.Any<CancellationToken>()).Returns(new NoteDto
            {
                Id = _newNoteId,
                Body = _newNoteBody,
                Title = _newNoteTitle,
                LastUpdatedOn = _newNoteLastUpdatedOn
            });

            // Act
            var response = await _sut.Post(new CreateNoteRequest
            {
                Title = _newNoteTitle,
                Body = _newNoteBody
            });

            // Assert
            _logger.Received(1).LogInformation("C# HTTP trigger NotesFunction processed a request.");
            var result = response.Result as CreatedResult;
            result!.StatusCode.Should().Be(StatusCodes.Status201Created);
            result!.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequestObjectResultWithRespectiveErrorMessage_WhenCalledWithInvalidNoteDetails()
        {
            // Arrange
            _noteService.CreateNoteAsync(Arg.Any<CreateNoteOptions>(), Arg.Any<CancellationToken>()).ReturnsNull();

            // Act
            var response = await _sut.Post(new CreateNoteRequest
            {
                Title = _newNoteTitle,
                Body = _newNoteBody
            });

            // Assert
            _logger.Received(1).LogInformation("C# HTTP trigger NotesFunction processed a request.");
            var result = response.Result as BadRequestObjectResult;
            result!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result!.Value.Should().BeEquivalentTo("This HTTP triggered NotesFunction executed successfully, but you passed in a bad request model for the note creation process.");
        }

        [Fact]
        public async Task Post_ShouldLogTheExceptionAndReturnInternalServerError_WhenNoteServiceThrowsAnException()
        {
            // Arrange
            _noteService.CreateNoteAsync(Arg.Any<CreateNoteOptions>(), Arg.Any<CancellationToken>()).ThrowsAsync<Exception>();

            // Act
            var response = await _sut.Post(new CreateNoteRequest
            {
                Title = _newNoteTitle,
                Body = _newNoteBody
            });

            // Assert
            _logger.Received(1).LogInformation("C# HTTP trigger NotesFunction processed a request.");
            _logger.Received(1).LogError(Arg.Any<Exception>(), $"Exception in {nameof(NotesFunction)} -> {typeof(NotesFunction)!.GetMethod("Post")!.Name} method.");
            var result = response.Result as InternalServerErrorResult;
            result!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}