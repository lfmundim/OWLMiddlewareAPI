using Newtonsoft.Json;

namespace OWLMiddleware.Models.Requests
{
    public class MatchRequest
    {
        [JsonProperty("teamid")]
        public int teamId { get; set; } 
    }
}