using System.Collections.Generic;
using System.Text.Json.Serialization;

// Holds the response details for a ValidationProblem API response.

namespace Shop.Client.Services
{
    public class ProblemDetails
    {
        [JsonPropertyName("status")]
        public int? Status { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("detail")]
        public string Detail { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("errors")]
        public IDictionary<string, string[]> Errors { get; set; }
    }
}
