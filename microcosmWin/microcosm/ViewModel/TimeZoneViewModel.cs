using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.ViewModel
{
    public class TimeZoneViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public TimeZoneViewModel()
        {
            timezone = new List<string>();
        }

        public List<string> _timezone { get; set; }
        public List<string> timezone
        {
            get
            {
                return _timezone;
            }
            set
            {
                _timezone = value;
                OnPropertyChanged("changeSettingList");
            }
        }

        protected void OnPropertyChanged(string propertyname)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }

        }
    }
}
