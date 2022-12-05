using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // 左上ユーザー
        public string _userName;
        public string userName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                OnPropertyChanged("userName");
            }
        }
        public string _userBirthStr;
        public string userBirthStr
        {
            get
            {
                return _userBirthStr;
            }
            set
            {
                _userBirthStr = value;
                OnPropertyChanged("userBirthStr");
            }
        }
        public string _userTimezone;
        public string userTimezone
        {
            get
            {
                return _userTimezone;
            }
            set
            {
                _userTimezone = value;
                OnPropertyChanged("userTimezone");
            }
        }
        public string _userBirthPlace;
        public string userBirthPlace
        {
            get
            {
                return _userBirthPlace;
            }
            set
            {
                _userBirthPlace = value;
                OnPropertyChanged("userBirthPlace");
            }
        }
        public string _userLat;
        public string userLat
        {
            get
            {
                return _userLat;
            }
            set
            {
                _userLat = value;
                OnPropertyChanged("userLat");
            }
        }
        public string _userLng;
        public string userLng
        {
            get
            {
                return _userLng;
            }
            set
            {
                _userLng = value;
                OnPropertyChanged("userLng");
            }
        }
        public string _userLatLng;
        public string userLatLng
        {
            get
            {
                return _userLatLng;
            }
            set
            {
                _userLatLng = value;
                OnPropertyChanged("userLatLng");
            }
        }
        public string _user2Name;
        public string user2Name
        {
            get
            {
                return _user2Name;
            }
            set
            {
                _user2Name = value;
                OnPropertyChanged("user2Name");
            }
        }
        public string _user2BirthStr;
        public string user2BirthStr
        {
            get
            {
                return _user2BirthStr;
            }
            set
            {
                _user2BirthStr = value;
                OnPropertyChanged("user2BirthStr");
            }
        }
        public string _user2Timezone;
        public string user2Timezone
        {
            get
            {
                return _user2Timezone;
            }
            set
            {
                _user2Timezone = value;
                OnPropertyChanged("user2Timezone");
            }
        }
        public string _user2BirthPlace;
        public string user2BirthPlace
        {
            get
            {
                return _user2BirthPlace;
            }
            set
            {
                _user2BirthPlace = value;
                OnPropertyChanged("user2BirthPlace");
            }
        }
        public string _user2Lat;
        public string user2Lat
        {
            get
            {
                return _user2Lat;
            }
            set
            {
                _user2Lat = value;
                OnPropertyChanged("user2Lat");
            }
        }
        public string _user2Lng;
        public string user2Lng
        {
            get
            {
                return _user2Lng;
            }
            set
            {
                _user2Lng = value;
                OnPropertyChanged("user2Lng");
            }
        }
        public string _user2LatLng;
        public string user2LatLng
        {
            get
            {
                return _user2LatLng;
            }
            set
            {
                _user2LatLng = value;
                OnPropertyChanged("user2LatLng");
            }
        }
        // 左下現在設定
        public string _targetUser1;
        public string targetUser1
        {
            get
            {
                return _targetUser1;
            }
            set
            {
                _targetUser1 = value;
                OnPropertyChanged("targetUser1");
            }
        }
        public string _targetUser2;
        public string targetUser2
        {
            get
            {
                return _targetUser2;
            }
            set
            {
                _targetUser2 = value;
                OnPropertyChanged("targetUser2");
            }
        }
        public string _targetUser3;
        public string targetUser3
        {
            get
            {
                return _targetUser3;
            }
            set
            {
                _targetUser3 = value;
                OnPropertyChanged("targetUser3");
            }
        }

        public string _centricMode;
        public string centricMode
        {
            get
            {
                return _centricMode;
            }
            set
            {
                _centricMode = value;
                OnPropertyChanged("centricMode");
            }
        }

        public string _siderealStr;
        public string siderealStr
        {
            get
            {
                return _siderealStr;
            }
            set
            {
                _siderealStr = value;
                OnPropertyChanged("siderealStr");
            }
        }

        public string _houseDivide;
        public string houseDivide
        {
            get
            {
                return _houseDivide;
            }
            set
            {
                _houseDivide = value;
                OnPropertyChanged("houseDivide");
            }
        }

        public string _progressionCalc;
        public string progressionCalc
        {
            get
            {
                return _progressionCalc;
            }
            set
            {
                _progressionCalc = value;
                OnPropertyChanged("progressionCalc");
            }
        }

        // 右上イベント
        public string _transitName;
        public string transitName
        {
            get
            {
                return _transitName;
            }
            set
            {
                _transitName = value;
                OnPropertyChanged("transitName");
            }
        }
        public string _transitBirthStr;
        public string transitBirthStr
        {
            get
            {
                return _transitBirthStr;
            }
            set
            {
                _transitBirthStr = value;
                OnPropertyChanged("transitBirthStr");
            }
        }
        public string _transitTimezone;
        public string transitTimezone
        {
            get
            {
                return _transitTimezone;
            }
            set
            {
                _transitTimezone = value;
                OnPropertyChanged("transitTimezone");
            }
        }
        public string _transitBirthPlace;
        public string transitBirthPlace
        {
            get
            {
                return _transitBirthPlace;
            }
            set
            {
                _transitBirthPlace = value;
                OnPropertyChanged("transitBirthPlace");
            }
        }
        public string _transitLat;
        public string transitLat
        {
            get
            {
                return _transitLat;
            }
            set
            {
                _transitLat = value;
                OnPropertyChanged("transitLat");
            }
        }
        public string _transitLng;
        public string transitLng
        {
            get
            {
                return _transitLng;
            }
            set
            {
                _transitLng = value;
                OnPropertyChanged("transitLng");
            }
        }
        public string _transitLatLng;
        public string transitLatLng
        {
            get
            {
                return _transitLatLng;
            }
            set
            {
                _transitLatLng = value;
                OnPropertyChanged("transitLatLng");
            }
        }
        public string _transit2Name;
        public string transit2Name
        {
            get
            {
                return _transit2Name;
            }
            set
            {
                _transit2Name = value;
                OnPropertyChanged("transit2Name");
            }
        }
        public string _transit2BirthStr;
        public string transit2BirthStr
        {
            get
            {
                return _transit2BirthStr;
            }
            set
            {
                _transit2BirthStr = value;
                OnPropertyChanged("transit2BirthStr");
            }
        }
        public string _transit2Timezone;
        public string transit2Timezone
        {
            get
            {
                return _transit2Timezone;
            }
            set
            {
                _transit2Timezone = value;
                OnPropertyChanged("transit2Timezone");
            }
        }
        public string _transit2BirthPlace;
        public string transit2BirthPlace
        {
            get
            {
                return _transit2BirthPlace;
            }
            set
            {
                _transit2BirthPlace = value;
                OnPropertyChanged("transit2BirthPlace");
            }
        }
        public string _transit2Lat;
        public string transit2Lat
        {
            get
            {
                return _transit2Lat;
            }
            set
            {
                _transit2Lat = value;
                OnPropertyChanged("transit2Lat");
            }
        }
        public string _transit2Lng;
        public string transit2Lng
        {
            get
            {
                return _transit2Lng;
            }
            set
            {
                _transit2Lng = value;
                OnPropertyChanged("transit2Lng");
            }
        }
        public string _transit2LatLng;
        public string transit2LatLng
        {
            get
            {
                return _transit2LatLng;
            }
            set
            {
                _transit2LatLng = value;
                OnPropertyChanged("transit2LatLng");
            }
        }
        public string _explanationTxt;
        public string explanationTxt
        {
            get
            {
                return _explanationTxt;
            }
            set
            {
                _explanationTxt = value;
                OnPropertyChanged("explanationTxt");
            }
        }

        public List<string> _changeSettingList { get; set; }
        public List<string> changeSettingList
        {
            get
            {
                return _changeSettingList;
            }
            set
            {
                _changeSettingList = value;
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

        /// <summary>
        /// 右上、左上のボックスに表示させる
        /// </summary>
        public void ReSet(int index, string name, DateTime time, string birthPlace, string latlng, string timezone)
        {
            if (index == 0)
            {
                userName = name;
                userBirthStr = time.ToString("yyyy/MM/dd HH:mm:ss");
                userTimezone = timezone;
                userBirthPlace = birthPlace;
                userLatLng = latlng;
            }
            else if (index == 1)
            {
                user2Name = name;
                user2BirthStr = time.ToString("yyyy/MM/dd HH:mm:ss");
                user2Timezone = timezone;
                user2BirthPlace = birthPlace;
                user2LatLng = latlng;
            }
            else if (index == 2)
            {
                transitName = name;
                transitBirthStr = time.ToString("yyyy/MM/dd HH:mm:ss");
                transitTimezone = timezone;
                transitBirthPlace = birthPlace;
                transitLatLng = latlng;
            }
            else if (index == 3)
            {
                transit2Name = name;
                transit2BirthStr = time.ToString("yyyy/MM/dd HH:mm:ss");
                transit2Timezone = timezone;
                transit2BirthPlace = birthPlace;
                transit2LatLng = latlng;
            }

        }

        // 設定リスト
        public void ReSetChangeSettingList(List<string> list)
        {
            changeSettingList = list;
        }

        private void CtrlHCommand(object sender)
        {
            Debug.WriteLine("H");
        }

        private void CtrlJCommand(object sender)
        {
            Debug.WriteLine("J");
        }

        private void CtrlKCommand(object sender)
        {
            Debug.WriteLine("K");
        }

        private void CtrlLCommand(object sender)
        {
            Debug.WriteLine("L");
        }

        private void CtrlOpenTagCommand(object sender)
        {
            Debug.WriteLine("<");
        }

        private void CtrlCloseTagCommand(object sender)
        {
            Debug.WriteLine(">");
        }

        private void CtrlOpenBracketCommand(object sender)
        {
            Debug.WriteLine("[");
        }

        private void CtrlCloseBracketCommand(object sender)
        {
            Debug.WriteLine("]");
        }
    }
}
