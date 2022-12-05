using microcosm.config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.ViewModel
{
    public class SettingWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public SettingWindow settingWindow;

        public SettingWindowViewModel(SettingData[] settings)
        {
            this.SettingDispNameList = new List<SettingDispNameData>();
            ReSet(settings);
        }

        public void ReSet(SettingData[] settings)
        {
            this.SettingDispNameList = new List<SettingDispNameData>();
            for (int i = 0; i < settings.Length; i++)
            {
                SettingDispNameList.Add(new SettingDispNameData(settings[i].dispName));
            }
        }

        public List<SettingDispNameData> _SettingDispNameList;
        public List<SettingDispNameData> SettingDispNameList
        {
            get
            {
                return _SettingDispNameList;
            }
            set
            {
                _SettingDispNameList = value;
                OnPropertyChanged("SettingDispNameList");
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
