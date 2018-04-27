using Newtonsoft.Json;

namespace OWLMiddleware.Models
{
    public class MatchRequest
    {
        [JsonProperty("teamid")]
        public int teamId { get; set; } 
    }
}