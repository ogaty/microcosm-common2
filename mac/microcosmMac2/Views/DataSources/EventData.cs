using System;
using microcosmMac2.User;

namespace microcosmMac2.Views.DataSources
{
    /// <summary>
    /// DBに表示させる用
    /// 
    /// </summary>
    public class EventData
    {
        public string index { get; set; }

        public string eventName { get; set; }

        public string eventBirth { get; set; }

        public string eventTimezone { get; set; }

        public string eventPlace { get; set; }

        public string eventLat { get; set; }

        public string eventLng { get; set; }

        public string eventMemo { get; set; }

        public EventData(UserEvent edata)
        {
            index = "0";
            eventName = edata.event_name;
            eventBirth = String.Format("{0}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}",
                edata.event_year, edata.event_month, edata.event_day,
                edata.event_hour, edata.event_minute, edata.event_second);
            eventTimezone = edata.event_timezone;
            eventPlace = edata.event_place;
            eventLat = edata.event_lat.ToString();
            eventLng = edata.event_lng.ToString();
            eventMemo = edata.event_memo;

        }

        public EventData(string index, string event_name,
            string event_year, string event_month, string event_day,
            string event_hour, string event_minute, string event_second,
            string event_timezone,
            string event_place,
            string event_lat, string event_lng, string event_memo)
        {
            this.index = index;
            eventName = event_name;
            eventBirth = String.Format("{0}/{1:D2}/{2:D2} {3:D2}:{4:D2}:{5:D2}",
                event_year, event_month, event_day,
                event_hour, event_minute, event_second);
            eventTimezone = event_timezone;
            eventPlace = event_place;
            eventLat = event_lat;
            eventLng = event_lng;
            eventMemo = event_memo;

        }
    }
}

