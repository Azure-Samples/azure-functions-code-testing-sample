using Newtonsoft.Json;

namespace Fta.DemoFunc.Api.Models
{
    public class NoteCreatedRequest
    {
        [JsonProperty("title")]
        public string Title { get; set; } = default!;

        [JsonProperty("description")]
        public string Description { get; set; } = default!;
    }
}
