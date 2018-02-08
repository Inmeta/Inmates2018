// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using ProcessMessage;
//
//    var data = MoodResponse.FromJson(jsonString);

namespace ProcessMessage
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class MoodResponse
    {
        [JsonProperty("documents")]
        public List<Document> Documents { get; set; }

        [JsonProperty("errors")]
        public List<object> Errors { get; set; }
    }

    public partial class Document
    {
        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class MoodResponse
    {
        public static MoodResponse FromJson(string json) => JsonConvert.DeserializeObject<MoodResponse>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this MoodResponse self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
