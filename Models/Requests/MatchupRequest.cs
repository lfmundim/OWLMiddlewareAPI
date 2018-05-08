using Newtonsoft.Json;

namespace OWLMiddleware.Models.Requests
{
    public partial class MatchupRequest
    {
        [JsonProperty("firstTeam")]
        public int firstTeamId { get; set; }
        [JsonProperty("secondTeam")]
        public int secondTeamId { get; set; }
    }
}