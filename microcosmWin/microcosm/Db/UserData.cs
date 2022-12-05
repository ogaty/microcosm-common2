using microcosm.config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace microcosm.Db
{
    public class UserData
    {
        [JsonPropertyName("uuid")]
        public string uuid { get; set; }
        [JsonPropertyName("name")]
        public string name { get; set; }
        [JsonPropertyName("furigana")]
        public string furigana { get; set; }
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
        [JsonPropertyName("lat")]
        public double lat { get; set; }
        [JsonPropertyName("lng")]
        public double lng { get; set; }
        [JsonPropertyName("birth_place")]
        public string birth_place { get; set; }
        [JsonPropertyName("memo")]
        public string memo { get; set; }
        [JsonPropertyName("timezone")]
        public double timezone { get; set; }
        [JsonPropertyName("timezone_str")]
        public string timezone_str { get; set; }
        /*
        [XmlArray("eventlist")]
        [XmlArrayItem("event")]
        public List<UserEvent> userevent { get; set; }
        */

        public string filename;

        public string birth_str
        {
            get
            {
                return birth_year.ToString("0000") + "/" + birth_month.ToString("00") + "/" + birth_day.ToString("00") + " " +
                    birth_hour.ToString("00") + ":" + birth_minute.ToString("00") + ":" + birth_second.ToString("00");
            }
        }
        public string birth_str_ymd
        {
            get
            {
                return birth_year.ToString("0000") + "/" + birth_month.ToString("00") + "/" + birth_day.ToString("00") + " ";
            }
        }
        public string birth_str_his
        {
            get
            {
                return birth_hour.ToString("00") + ":" + birth_minute.ToString("00") + ":" + birth_second.ToString("00");
            }
        }


        public string lat_lng
        {
            get
            {
                return lat.ToString("00.000") + "/" + lng.ToString("000.000");
            }
        }

        public UserData()
        {
            this.uuid = Guid.NewGuid().ToString();
            this.name = "現在時刻";
            this.furigana = "";
            this.birth_year = DateTime.Now.Year;
            this.birth_month = DateTime.Now.Month;
            this.birth_day = DateTime.Now.Day;
            this.birth_hour = DateTime.Now.Hour;
            this.birth_minute = DateTime.Now.Minute;
            this.birth_second = DateTime.Now.Second;
            this.birth_place = "東京都千代田区";
            this.lat = 35.685175;
            this.lng = 139.7528;
            this.memo = "";
            this.timezone = 9.0;
            this.timezone_str = "Asia/Tokyo (UTC+09:00)";
        }

        public UserData(ConfigData config)
        {
            this.uuid = Guid.NewGuid().ToString();
            this.name = "現在時刻";
            this.furigana = "";
            this.birth_year = DateTime.Now.Year;
            this.birth_month = DateTime.Now.Month;
            this.birth_day = DateTime.Now.Day;
            this.birth_hour = DateTime.Now.Hour;
            this.birth_minute = DateTime.Now.Minute;
            this.birth_second = DateTime.Now.Second;
            this.birth_place = config.defaultPlace;
            this.lat = config.lat;
            this.lng = config.lng;
            this.memo = "";
            this.timezone = 9.0;
            this.timezone_str = "Asia/Tokyo (UTC+09:00)";
        }

        public UserData(
            string uuid,
            string name,
            string furigana,
            int birth_year,
            int birth_month,
            int birth_day,
            int birth_hour,
            int birth_minute,
            int birth_second,
            double lat,
            double lng,
            string birth_place,
            string memo,
            string timezone_str,
            double timezone
            )
        {
            this.uuid = uuid;
            this.name = name;
            this.furigana = furigana;
            this.birth_year = birth_year;
            this.birth_month = birth_month;
            this.birth_day = birth_day;
            this.birth_hour = birth_hour;
            this.birth_minute = birth_minute;
            this.birth_second = birth_second;
            this.lat = lat;
            this.lng = lng;
            this.birth_place = birth_place;
            this.memo = memo;
            this.timezone = timezone;
            this.timezone_str = timezone_str;
        }

        public void setData(
            string uuid,
            string name,
            string furigana,
            int birth_year,
            int birth_month,
            int birth_day,
            int birth_hour,
            int birth_minute,
            int birth_second,
            double lat,
            double lng,
            string birth_place,
            string memo,
            string timezone_str,
            double timezone
            )
        {
            this.uuid = uuid;
            this.name = name;
            this.furigana = furigana;
            this.birth_year = birth_year;
            this.birth_month = birth_month;
            this.birth_day = birth_day;
            this.birth_hour = birth_hour;
            this.birth_minute = birth_minute;
            this.birth_second = birth_second;
            this.lat = lat;
            this.lng = lng;
            this.birth_place = birth_place;
            this.memo = memo;
            this.timezone = timezone;
            this.timezone_str = timezone_str;
        }

        /*
        public static explicit operator UserEventJson(UserData val)
        {
            return new UserEventJson(val.name, val.birth_year, val.birth_month, val.birth_day,
                val.birth_hour, val.birth_minute, val.birth_second, val.birth_place,
                val.lat, val.lng, val.timezone_str, val.timezone, val.memo);
        }
        */

        public static explicit operator UserEventData(UserData val)
        {
            return new UserEventData(val.name, val.birth_year, val.birth_month, val.birth_day,
                val.birth_hour, val.birth_minute, val.birth_second,
                val.lat, val.lng, val.birth_place, val.memo, val.timezone_str, val.timezone);
        }

        public DateTime GetBirthDateTime()
        {
            return new DateTime(birth_year, birth_month, birth_day, birth_hour, birth_minute, birth_second);
        }

        public void SetDateTime(DateTime time)
        {
            birth_year = time.Year;
            birth_month = time.Month;
            birth_day = time.Day;
            birth_hour = time.Hour;
            birth_minute = time.Minute;
            birth_second = time.Second;
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}
