// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using OWLMiddleware.Models;
//
//    var team = Team.FromJson(jsonString);

using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OWLMiddleware.Models
{
    public partial class Team
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("homeLocation")]
        public string HomeLocation { get; set; }

        [JsonProperty("primaryColor")]
        public string PrimaryColor { get; set; }

        [JsonProperty("secondaryColor")]
        public string SecondaryColor { get; set; }

        [JsonProperty("accounts")]
        public Account[] Accounts { get; set; }

        [JsonProperty("abbreviatedName")]
        public string AbbreviatedName { get; set; }

        [JsonProperty("addressCountry")]
        public string AddressCountry { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("players")]
        public Player[] Players { get; set; }

        [JsonProperty("secondaryPhoto")]
        public string SecondaryPhoto { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("placement")]
        public long Placement { get; set; }

        [JsonProperty("advantage")]
        public long Advantage { get; set; }

        [JsonProperty("ranking")]
        public Record Ranking { get; set; }

        [JsonProperty("schedule")]
        public Schedule[] Schedule { get; set; }

        [JsonProperty("aboutUrl")]
        public string AboutUrl { get; set; }
    }
}
