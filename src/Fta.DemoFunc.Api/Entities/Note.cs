using Newtonsoft.Json;
using System;

namespace Fta.DemoFunc.Api.Entities
{
    public class Note
    {
        [JsonProperty("id")]
        public string Id { get; set; } = default!;

        [JsonProperty("title")]
        public string Title { get; set; } = default!;

        [JsonProperty("body")]
        public string Body { get; set; } = default!;

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("lastUpdatedOn")]
        public DateTime LastUpdatedOn { get; set; }
    }
}
