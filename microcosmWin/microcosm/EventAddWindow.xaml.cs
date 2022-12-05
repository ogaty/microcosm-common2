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
    /// EventAddWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class EventAddWindow : Window
    {
        public MainWindow mainWindow;
        public DatabaseWindow databaseWindow;
        public LatLngCsvViewModel vm;
        public TimeZoneViewModel timezoneVM;
        public string savedFile;

        public EventAddWindow()
        {
            InitializeComponent();
        }

        public EventAddWindow(MainWindow main, DatabaseWindow dbWindow)
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


            name.Text = "新規データ";

            event_date.SelectedDate = new DateTime(2000, 1, 1);
            hour.Text = "12";
            minute.Text = "0";
            second.Text = "0";

            lat.Text = mainWindow.configData.lat.ToString();
            lng.Text = mainWindow.configData.lng.ToString();
            place.Text = mainWindow.configData.defaultPlace;
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            string item = (string)birth_timezone.SelectedItem;

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
            //old読み込み、listにadd、ファイル上書き
            UserJsonList userJsonList = null;
            using (FileStream fs = new FileStream(savedFile, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                string userJsonData = sr.ReadToEnd();
                userJsonList = JsonSerializer.Deserialize<UserJsonList>(userJsonData);
            }

            userJsonList.list.Add(userJson);
            string userJsonStr = JsonSerializer.Serialize(userJsonList, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true,
            });

            string root = Util.root();


            using (FileStream fs = new FileStream(savedFile, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(userJsonStr);
                sw.Close();
            }

            databaseWindow.ReRenderEvent(savedFile);
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

        public void SetData(string fileName)
        {
            savedFile = fileName;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Collapsed;
        }
    }
}
