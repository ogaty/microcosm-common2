using System;
using System.Collections.Generic;
using microcosmMac2.Common;

namespace microcosmMac2.User
{
    public class UserData
    {
        public string uuid { get; set; }
        public string name { get; set; }
        public string furigana { get; set; }
        public int birth_year { get; set; }
        public int birth_month { get; set; }
        public int birth_day { get; set; }
        public int birth_hour { get; set; }
        public int birth_minute { get; set; }
        public int birth_second { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public string birth_place { get; set; }
        public string memo { get; set; }
        public double timezone { get; set; }
        public string timezone_str { get; set; }
        //    public List<UserEvent> userevent { get; set; }

        public UserData()
        {
            this.uuid = Guid.NewGuid().ToString();
            DateTime time = DateTime.Now;
            this.name = "現在時刻";
            this.furigana = "";
            this.birth_year = time.Year;
            this.birth_month = time.Month;
            this.birth_day = time.Day;
            this.birth_hour = time.Hour;
            this.birth_minute = time.Minute;
            this.birth_second = time.Second;
            this.lat = CommonData.defaultLat;
            this.lng = CommonData.defaultLng;
            this.birth_place = CommonData.defaultPlace;
            this.memo = "";
            this.timezone = 9.0;
            this.timezone_str = "Asia/Tokyo (+9:00)";
        }

        public UserData(
            string name,
            string furigana,
            DateTime birth,
            double lat,
            double lng,
            string birth_place,
            string memo,
            string timezone_str,
            double timezone
        )
        {
            this.uuid = Guid.NewGuid().ToString();
            this.name = name;
            this.furigana = furigana;
            this.birth_year = birth.Year;
            this.birth_month = birth.Month;
            this.birth_day = birth.Day;
            this.birth_hour = birth.Hour;
            this.birth_minute = birth.Minute;
            this.birth_second = birth.Second;
            this.lat = lat;
            this.lng = lng;
            this.birth_place = birth_place;
            this.memo = memo;
            this.timezone_str = timezone_str;
            this.timezone = timezone;
        }

        public UserData(UserJson json)
        {
            this.uuid = json.uuid;
            this.name = json.name;
            this.furigana = "";
            this.birth_year = json.birth_year;
            this.birth_month = json.birth_month;
            this.birth_day = json.birth_day;
            this.birth_hour = json.birth_hour;
            this.birth_minute = json.birth_minute;
            this.birth_second = json.birth_second;
            this.lat = json.lat;
            this.lng = json.lng;
            this.birth_place = json.birth_place;
            this.memo = json.memo;
            this.timezone_str = json.birth_timezone_str;
            this.timezone = json.birth_timezone;
        }

        public DateTime GetDateTime()
        {
            return new DateTime(this.birth_year, this.birth_month, this.birth_day, this.birth_hour, this.birth_minute, this.birth_second);
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
    }
}

