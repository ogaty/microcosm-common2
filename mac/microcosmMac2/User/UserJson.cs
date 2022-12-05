using System;
using System.Text.Json.Serialization;

namespace microcosmMac2.User
{
    public class UserJson
    {
        [JsonPropertyName("uuid")]
        public string uuid { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("birth_year")]
        public int birth_year { get; set; }

        [JsonPropertyName("birth_month")]
        public int birth_month { get; set; }

        [JsonPropertyName("birth_day")]
        public int birth_day { get; set; }

        [JsonPropertyName("birth_hour")]
        public int birth_hour { get; set; }

        [JsonPropertyName("birth_minute")]
        public int birth_minute { get; set; }

        [JsonPropertyName("birth_second")]
        public int birth_second { get; set; }

        [JsonPropertyName("birth_place")]
        public string birth_place { get; set; }

        [JsonPropertyName("birth_timezone")]
        public double birth_timezone { get; set; }

        [JsonPropertyName("birth_timezone_str")]
        public string birth_timezone_str { get; set; }

        [JsonPropertyName("lat")]
        public double lat { get; set; }

        [JsonPropertyName("lng")]
        public double lng { get; set; }

        [JsonPropertyName("memo")]
        public string memo { get; set; }

        public UserJson()
        {

        }
    }
}

