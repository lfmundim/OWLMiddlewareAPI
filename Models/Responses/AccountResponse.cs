using Newtonsoft.Json;

namespace OWLMiddleware.Models.Requests
{
    public partial class AccountResponse
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("accountType")]
        public string AccountType { get; set; }
    }
}