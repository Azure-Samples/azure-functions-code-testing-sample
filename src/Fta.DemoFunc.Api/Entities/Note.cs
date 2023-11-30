using Newtonsoft.Json;

namespace Fta.DemoFunc.Api.Entities
{
    public class Note : AuditableEntity
    {
        [JsonProperty("title")]
        public string Title { get; set; } = default!;

        [JsonProperty("body")]
        public string Body { get; set; } = default!;
    }
}
