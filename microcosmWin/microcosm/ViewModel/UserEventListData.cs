using microcosm.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.ViewModel
{
    public class UserEventListData
    {
        public int index { get; set; }

        public string uuid { get; set; }

        public string fileName { get; set; }

        public string fileNameFullPath { get; set; }

        public string eventName { get; set; }

        public string eventBirth { get; set; }

        public string eventTimezone { get; set; }

        public string eventPlace { get; set; }

        public string eventLat { get; set; }

        public string eventLng { get; set; }

        public string eventMemo { get; set; }


 
        public UserEventListData(
            int index,
            string event_name, 
            string event_year, string event_month, string event_day,
            string event_hour, string event_minute, string event_second,
            string event_timezone,
            string event_place,
            string event_lat, string event_lng, string event_memo,
            string file, string fileFullPath)
        {
            this.index = index;
            eventName = event_name;
            eventBirth = String.Format("{0}/{1}/{2} {3}:{4}:{5}",
                event_year, event_month, event_day,
                event_hour, event_minute, event_second);
            eventTimezone = event_timezone;
            eventPlace = event_place;
            eventLat = event_lat;
            eventLng = event_lng;
            eventMemo = event_memo.Replace("\n", " ");
            this.fileName = file;
            this.fileNameFullPath = fileFullPath;

        }
    }
}
