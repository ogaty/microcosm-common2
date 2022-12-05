using microcosm.common;
using microcosm.Db;
using microcosm.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace microcosm
{
    /// <summary>
    /// EventEditWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class EventEditWindow : Window
    {
        private MainWindow mainWindow;
        private DatabaseWindow databaseWindow;
        private LatLngCsvViewModel vm;
        private TimeZoneViewModel timezoneVM;
        private string oldFile;
        private int userIndex;
        private int eventIndex;

        public EventEditWindow()
        {
            InitializeComponent();
        }

        public EventEditWindow(MainWindow main, DatabaseWindow dbWindow)
        {
            this.mainWindow = main;
            databaseWindow = dbWindow;
            InitializeComponent();

            vm = new LatLngCsvViewModel();
            latlngList.DataContext = vm;
            timezoneVM = new TimeZoneViewModel();
            string[] timezones = CommonData.GetTimeTables();
            foreach (string timezone in timezones)
            {
                timezoneVM.timezone.Add(timezone);
            }
            this.DataContext = timezoneVM;
            birth_timezone.SelectedItem = "Asia/Tokyo (+9:00)";

        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            string item = (string)birth_timezone.SelectedItem;

            UserJsonList userJsonList = null;
            using (FileStream fs = new FileStream(oldFile, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                string userJsonData = sr.ReadToEnd();
                userJsonList = JsonSerializer.Deserialize<UserJsonList>(userJsonData);
            }

            int birth_hour = 0;
            if (!Int32.TryParse(hour.Text, out birth_hour))
            {
                MessageBox.Show("時刻は数値で指定してください。");
                return;
            }
            int birth_minute = 0;
            if (!Int32.TryParse(minute.Text, out birth_minute))
            {
                MessageBox.Show("時刻は数値で指定してください。");
                return;
            }
            int birth_second = 0;
            if (!Int32.TryParse(second.Text, out birth_second))
            {
                MessageBox.Show("時刻は数値で指定してください。");
                return;
            }
            double event_lat = 0;
            if (!Double.TryParse(lat.Text, out event_lat))
            {
                MessageBox.Show("時刻は数値で指定してください。");
                return;
            }
            double event_lng = 0;
            if (!Double.TryParse(lng.Text, out event_lng))
            {
                MessageBox.Show("時刻は数値で指定してください。");
                return;
            }
            UserData userJson = new UserData()
            {
                name = name.Text,
                birth_year = event_date.SelectedDate.Value.Year,
                birth_month = event_date.SelectedDate.Value.Month,
                birth_day = event_date.SelectedDate.Value.Day,
                birth_hour = birth_hour,
                birth_minute = birth_minute,
                birth_second = birth_second,
                birth_place = place.Text,
                memo = memo.Text,
                timezone = CommonData.GetTimezoneValue(item),
                timezone_str = item,
                lat = event_lat,
                lng = event_lng
            };
            userJsonList.list[eventIndex] = userJson;

            string userJsonStr = JsonSerializer.Serialize(userJsonList, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true,
            });


            string root = Util.root();
            if (File.Exists(oldFile))
            {
                File.Delete(oldFile);
            }
            using (FileStream fs = new FileStream(oldFile, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(userJsonStr);
                sw.Close();
            }

            databaseWindow.ReRender();
            databaseWindow.UserList.SelectedIndex = userIndex;
            this.Visibility = Visibility.Hidden;
        }

        private void latlngList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = (ListView)sender;
            LatLng ll = (LatLng)lv.SelectedItem;
            place.Text = ll.name;
            lat.Text = ll.lat.ToString();
            lng.Text = ll.lng.ToString();
        }


        public void SetData(string fileName, int userIndex, int eventIndex)
        {
            UserJsonList jsonList = null;

            oldFile = fileName;
            this.userIndex = userIndex;
            this.eventIndex = eventIndex;

            using (FileStream fs = new FileStream(oldFile, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                string jsonData = sr.ReadToEnd();
                jsonList = JsonSerializer.Deserialize<UserJsonList>(jsonData);
            }

            name.Text = jsonList.list[eventIndex].name;

            event_date.SelectedDate = jsonList.list[eventIndex].GetBirthDateTime();
            hour.Text = jsonList.list[eventIndex].birth_hour.ToString();
            minute.Text = jsonList.list[eventIndex].birth_minute.ToString();
            second.Text = jsonList.list[eventIndex].birth_second.ToString();

            lat.Text = jsonList.list[eventIndex].lat.ToString();
            lng.Text = jsonList.list[eventIndex].lng.ToString();
            place.Text = jsonList.list[eventIndex].birth_place;

            memo.Text = jsonList.list[eventIndex].memo;

            int index2 = 0;
            foreach (LatLng item in latlngList.Items)
            {
                if (item.name == jsonList.list[eventIndex].timezone_str)
                {
                    latlngList.SelectedIndex = index2;
                }
                index2++;
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Collapsed;
        }
    }
}
