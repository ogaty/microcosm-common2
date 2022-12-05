using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace microcosm.Db
{
    public class UserEventJson
    {
        [JsonPropertyName("event_name")]
        public string event_name;
        [JsonPropertyName("event_year")]
        public int event_year;
        [JsonPropertyName("event_month")]
        public int event_month;
        [JsonPropertyName("event_day")]
        public int event_day;
        [JsonPropertyName("event_hour")]
        public int event_hour;
        [JsonPropertyName("event_minute")]
        public int event_minute;
        [JsonPropertyName("event_second")]
        public int event_second;
        [JsonPropertyName("event_place")]
        public string event_place;
        [JsonPropertyName("event_lat")]
        public double event_lat;
        [JsonPropertyName("event_lng")]
        public double event_lng;
        [JsonPropertyName("event_timezone")]
        public double event_timezone;
        [JsonPropertyName("event_timezone_str")]
        public string event_timezone_str;
        [JsonPropertyName("event_memo")]
        public string event_memo;

        public UserEventJson()
        {

        }

        public UserEventJson(string event_name, int event_year, int event_month, int event_day,
            int event_hour, int event_minute, int event_second, string event_place,
            double event_lat, double event_lng, string event_timezone_str, double event_timezone, string event_memo)
        {
            this.event_name = event_name;
            this.event_year = event_year;
            this.event_month = event_month;
            this.event_day = event_day;
            this.event_hour = event_hour;
            this.event_minute = event_minute;
            this.event_second = event_second;
            this.event_place = event_place;
            this.event_lat = event_lat;
            this.event_lng = event_lng;
            this.event_timezone_str = event_timezone_str;
            this.event_timezone = event_timezone;
            this.event_memo = event_memo;
        }
    }
}
