using Newtonsoft.Json;
using static OWLMiddleware.Models.Enumerations;

namespace OWLMiddleware.Models.Requests
{
    public class MatchRequest
    {
        [JsonProperty("teamid")]
        public int teamId { get; set; }
        [JsonProperty("blipFormat")]
        public bool blipFormat { get;set; }
    }
}