namespace Fta.DemoFunc.Api.Contracts.Requests
{
    public class CreateNoteRequest
    {
        public string Title { get; init; } = default!;

        public string Body { get; init; } = default!;
    }
}
