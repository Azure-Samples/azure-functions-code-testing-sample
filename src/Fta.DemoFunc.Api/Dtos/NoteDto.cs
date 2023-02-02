using System;

namespace Fta.DemoFunc.Api.Dtos
{
    public class NoteDto
    {
        public string Id { get; set; } = default!;

        public string Title { get; set; } = default!;

        public string Body { get; set; } = default!;

        public DateTime LastUpdatedOn { get; set; }
    }
}
