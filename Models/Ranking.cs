// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using OWLMiddleware.Models;
//
//    var ranking = Ranking.FromJson(jsonString);
using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OWLMiddleware.Models
{

    public partial class Ranking
    {
        [JsonProperty("content")]
        public Content[] Content { get; set; }

        [JsonProperty("totalMatches")]
        public long TotalMatches { get; set; }

        [JsonProperty("matchesConcluded")]
        public long MatchesConcluded { get; set; }

        [JsonProperty("playoffCutoff")]
        public long PlayoffCutoff { get; set; }
    }

    public partial class Content
    {
        [JsonProperty("competitor")]
        public Team Competitor { get; set; }

        [JsonProperty("placement")]
        public long Placement { get; set; }

        [JsonProperty("advantage")]
        public long Advantage { get; set; }

        [JsonProperty("records")]
        public Record[] Records { get; set; }
    }

    public partial class Division
    {
        [JsonProperty("competitor")]
        public DivisionClass Competitor { get; set; }

        [JsonProperty("division")]
        public DivisionClass DivisionDivision { get; set; }
    }

    public partial class DivisionClass
    {
        [JsonProperty("id")]
        public long Id { get; set; }
    }
}
