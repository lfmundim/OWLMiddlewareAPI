using Newtonsoft.Json;
using static OWLMiddleware.Models.Enumerations;

namespace OWLMiddleware.Models.Requests
{
    public class TeamRequest
    {
        [JsonProperty("divisionId")]
        public DivisionIds divisionId { get; set; } 
    }
}