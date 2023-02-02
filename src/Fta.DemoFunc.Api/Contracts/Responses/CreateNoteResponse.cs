using System;

namespace Fta.DemoFunc.Api.Contracts.Responses
{
    public class CreateNoteResponse
    {
        public string Id { get; set; } = default!;

        public string Title { get; set; } = default!;

        public string Body { get; set; } = default!;

        public DateTime LastUpdatedOn { get; set; }
    }
}
