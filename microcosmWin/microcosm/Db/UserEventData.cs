using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.Db
{
    public class UserEventData
    {
        public string name { get; set; }
        public int birth_year { get; set; }
        public int birth_month { get; set; }
        public int birth_day { get; set; }
        public int birth_hour { get; set; }
        public int birth_minute { get; set; }
        public int birth_second { get; set; }
        public string birth_place { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public string lat_lng { get; set; }
        public double timezone { get; set; }
        public string timezone_str { get; set; }
        public string memo { get; set; }
        public string fullpath { get; set; }
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

        public UserEventData()
        {

        }

        public UserEventData(string name,
            int year, int month, int day,
            int hour, int minute, int second,
            double lat, double lng, string place,
            string memo, string timezone_str, double timezone)
        {
            this.name = name;
            this.birth_year = year;
            this.birth_month = month;
            this.birth_day = day;
            this.birth_hour = hour;
            this.birth_minute = minute;
            this.birth_second = second;
            this.birth_place = place;
            this.lat = lat;
            this.lng = lng;
            this.memo = memo;
            this.timezone = timezone;
            this.timezone_str = timezone_str;
        }


    }
}
