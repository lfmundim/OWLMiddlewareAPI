using Newtonsoft.Json;

namespace OWLMiddleware.Models
{
    public partial class Account
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("accountType")]
        public string AccountType { get; set; }

        [JsonProperty("isPublic")]
        public bool IsPublic { get; set; }
    }
}