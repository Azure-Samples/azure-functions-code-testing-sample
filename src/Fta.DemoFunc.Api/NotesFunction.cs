using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Fta.DemoFunc.Api.Interfaces;
using System;
using System.Web.Http;
using Fta.DemoFunc.Api.Contracts.Requests;
using System.Threading;
using Fta.DemoFunc.Api.Options;
using Fta.DemoFunc.Api.Contracts.Responses;

namespace Fta.DemoFunc.Api
{
    public class NotesFunction
    {
        private readonly INoteService _noteService;
        private readonly ILoggerAdapter<NotesFunction> _logger;

        public NotesFunction(INoteService noteService, ILoggerAdapter<NotesFunction> logger)
        {
            _noteService = noteService;
            _logger = logger;
        }

        [FunctionName("NotesFunction")]
        public async Task<ActionResult<CreateNoteResponse>> Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "notes")] CreateNoteRequest createNoteRequest, CancellationToken ct = default
        )
        {
            _logger.LogInformation("C# HTTP trigger NotesFunction processed a request.");

            try
            {
                var newNoteDto = await _noteService.CreateNoteAsync(new CreateNoteOptions
                {
                    Body = createNoteRequest.Body,
                    Title = createNoteRequest.Title
                }, ct);

                if (newNoteDto is null)
                {
                    return new BadRequestObjectResult("This HTTP triggered NotesFunction executed successfully, but you passed in a bad request model for the note creation process.");
                }

                return new CreatedResult("/notes/" + newNoteDto.Id, new CreateNoteResponse
                {
                    Id = newNoteDto.Id,
                    Title = newNoteDto.Title,
                    Body = newNoteDto.Body,
                    LastUpdatedOn = newNoteDto.LastUpdatedOn
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in {nameof(NotesFunction)} -> {nameof(Post)} method.");

                return new InternalServerErrorResult();
            }
        }
    }
}
