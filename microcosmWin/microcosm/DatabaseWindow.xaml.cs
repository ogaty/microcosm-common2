using microcosm.Db;
using microcosm.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using microcosm.common;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.Win32;
using System.Globalization;
using System.Windows.Documents;
using CsvHelper.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Reflection;
using System.Xml.Serialization;

namespace microcosm
{
    /// <summary>
    /// DatabaseWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DatabaseWindow : Window
    {
        public MainWindow main;
        public DatabaseWindowViewModel vm;
        public DatabaseWindowUserEventListViewModel userEventVM;
        public DatabaseWindowDirViewModel dirVm;

        public UserAddWindow userAddWindow;
        public UserEditWindow userEditWindow;
        public EventAddWindow eventAddWindow;
        public EventEditWindow eventEditWindow;
        public EclipseWindow eclipseWindow;
        public DirAddWindow dirAddWindow;
        public DirEditWindow dirEditWindow;

        public string currentFullPath = "";

        public DatabaseWindow(MainWindow mainWindow)
        {
            this.main = mainWindow;
            InitializeComponent();

            vm = new DatabaseWindowViewModel(this);
            UserList.DataContext = vm;

            dirVm = new DatabaseWindowDirViewModel(this);
            DirList.DataContext = dirVm;

            userEventVM = new DatabaseWindowUserEventListViewModel(this);
            EventList.DataContext = userEventVM;

            user1Name.Text = "現在時刻";
            user2Name.Text = "現在時刻";
            event1Name.Text = "現在時刻";
            event2Name.Text = "現在時刻";

            user1DateTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            user2DateTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            event1DateTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            event2DateTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            user1LatLng.Text = String.Format("{0} {1}", main.configData.lat, main.configData.lng);
            user2LatLng.Text = String.Format("{0} {1}", main.configData.lat, main.configData.lng);
            event1LatLng.Text = String.Format("{0} {1}", main.configData.lat, main.configData.lng);
            event2LatLng.Text = String.Format("{0} {1}", main.configData.lat, main.configData.lng);

            currentFullPath = Util.root() + @"\data";
        }


        private void data1Button_Click(object sender, RoutedEventArgs e)
        {
            UserJsonList userJsonList = GetUserJsonList();
            if (userJsonList == null)
            {
                return;
            }
            if (UserList.SelectedIndex == -1)
            {
                return;
            }
            if (EventList.SelectedIndex == -1)
            {
                return;
            }

            user1Name.Text = userJsonList.list[EventList.SelectedIndex].name;
            user1DateTime.Text = userJsonList.list[EventList.SelectedIndex].GetBirthDateTime().ToString("yyyy/MM/dd HH:mm:ss");
            user1LatLng.Text = String.Format("{0} {1}", userJsonList.list[EventList.SelectedIndex].lat, userJsonList.list[EventList.SelectedIndex].lng);
            main.mainWindowVM.userName = userJsonList.list[EventList.SelectedIndex].name;
            main.mainWindowVM.userBirthStr = userJsonList.list[EventList.SelectedIndex].GetBirthDateTime().ToString("yyyy/MM/dd HH:mm:ss");
            main.mainWindowVM.userLatLng = String.Format("{0} {1}", userJsonList.list[EventList.SelectedIndex].lat, userJsonList.list[EventList.SelectedIndex].lng);
            main.user1data = userJsonList.list[EventList.SelectedIndex];

            main.ReCalc();
            main.ReRender();

        }

        private void data2Button_Click(object sender, RoutedEventArgs e)
        {
            UserJsonList userJsonList = GetUserJsonList();
            if (userJsonList == null)
            {
                return;
            }
            if (UserList.SelectedIndex == -1)
            {
                return;
            }
            if (EventList.SelectedIndex == -1)
            {
                return;
            }

            user2Name.Text = userJsonList.list[EventList.SelectedIndex].name;
            user2DateTime.Text = userJsonList.list[EventList.SelectedIndex].GetBirthDateTime().ToString("yyyy/MM/dd HH:mm:ss");
            user2LatLng.Text = String.Format("{0} {1}", userJsonList.list[EventList.SelectedIndex].lat, userJsonList.list[EventList.SelectedIndex].lng);
            main.mainWindowVM.user2Name = userJsonList.list[EventList.SelectedIndex].name;
            main.mainWindowVM.user2BirthStr = userJsonList.list[EventList.SelectedIndex].GetBirthDateTime().ToString("yyyy/MM/dd HH:mm:ss");
            main.mainWindowVM.user2LatLng = String.Format("{0} {1}", userJsonList.list[EventList.SelectedIndex].lat, userJsonList.list[EventList.SelectedIndex].lng);
            main.user2data = userJsonList.list[EventList.SelectedIndex];

            main.ReCalc();
            main.ReRender();

        }

        private void data3Button_Click(object sender, RoutedEventArgs e)
        {
            UserJsonList userJsonList = GetUserJsonList();
            if (userJsonList == null)
            {
                return;
            }
            if (UserList.SelectedIndex == -1)
            {
                return;
            }
            if (EventList.SelectedIndex == -1)
            {
                return;
            }

            event1Name.Text = userJsonList.list[EventList.SelectedIndex].name;
            event1DateTime.Text = userJsonList.list[EventList.SelectedIndex].GetBirthDateTime().ToString("yyyy/MM/dd HH:mm:ss");
            event1LatLng.Text = String.Format("{0} {1}", userJsonList.list[EventList.SelectedIndex].lat, userJsonList.list[EventList.SelectedIndex].lng);
            main.mainWindowVM.transitName = userJsonList.list[EventList.SelectedIndex].name;
            main.mainWindowVM.transitBirthStr = userJsonList.list[EventList.SelectedIndex].GetBirthDateTime().ToString("yyyy/MM/dd HH:mm:ss");
            main.mainWindowVM.transitLatLng = String.Format("{0} {1}", userJsonList.list[EventList.SelectedIndex].lat, userJsonList.list[EventList.SelectedIndex].lng);
            main.event1data = userJsonList.list[EventList.SelectedIndex];

            main.ReCalc();
            main.ReRender();

        }

        private void data4Button_Click(object sender, RoutedEventArgs e)
        {
            UserJsonList userJsonList = GetUserJsonList();
            if (userJsonList == null)
            {
                return;
            }
            if (UserList.SelectedIndex == -1)
            {
                return;
            }
            if (EventList.SelectedIndex == -1)
            {
                return;
            }

            event2Name.Text = userJsonList.list[EventList.SelectedIndex].name;
            event2DateTime.Text = userJsonList.list[EventList.SelectedIndex].GetBirthDateTime().ToString("yyyy/MM/dd HH:mm:ss");
            event2LatLng.Text = String.Format("{0} {1}", userJsonList.list[EventList.SelectedIndex].lat, userJsonList.list[EventList.SelectedIndex].lng);
            main.mainWindowVM.transit2Name = userJsonList.list[EventList.SelectedIndex].name;
            main.mainWindowVM.transit2BirthStr = userJsonList.list[EventList.SelectedIndex].GetBirthDateTime().ToString("yyyy/MM/dd HH:mm:ss");
            main.mainWindowVM.transit2LatLng = String.Format("{0} {1}", userJsonList.list[EventList.SelectedIndex].lat, userJsonList.list[EventList.SelectedIndex].lng);
            main.event2data = userJsonList.list[EventList.SelectedIndex];

            main.ReCalc();
            main.ReRender();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Collapsed;
        }

        private UserJsonList? GetUserJsonList()
        {
            if (UserList.SelectedItem == null)
            {
                return null;
            }
            UserJsonList? userJsonList;
            string fileName = ((UserFileListData)UserList.SelectedItem).fileNameFullPath;
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                string userJsonData = sr.ReadToEnd();
                userJsonList = JsonSerializer.Deserialize<UserJsonList>(userJsonData);
            }

            return userJsonList;
        }

        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = (ListView)sender;
            if (lv.SelectedIndex == -1)
            {
                userEventVM.Clear();
                return;
            }
            UserFileListData userFileListData = (UserFileListData)lv.SelectedItem;
            string fileName = userFileListData.fileNameFullPath;
            userEventVM.SetUserEvent(fileName);
            Debug.WriteLine(lv.SelectedIndex);
        }

        private void newFile_Click(object sender, RoutedEventArgs e)
        {
            if (userAddWindow == null)
            {
                userAddWindow = new UserAddWindow(main, this);
                userAddWindow.Owner = this;
            }
            userAddWindow.ShowDialog();
        }

        private void editFile_Click(object sender, RoutedEventArgs e)
        {
            if (userEditWindow == null)
            {
                userEditWindow = new UserEditWindow(main, this);
                userEditWindow.Owner = this;
            }
            UserFileListData data = (UserFileListData)UserList.SelectedItem;
            if (data == null) return;
            userEditWindow.SettingDispNameData(data.fileNameFullPath);
            userEditWindow.ShowDialog();
        }

        public void ReRender()
        {
            vm.ReRender();
        }

        public void ReRenderDir()
        {
            dirVm.ReRender();
        }

        public void ReRenderEvent(string dirName)
        {
            userEventVM.ReRender(dirName);
        }

        private void newEventFile_Click(object sender, RoutedEventArgs e)
        {
            if (eventAddWindow == null)
            {
                eventAddWindow = new EventAddWindow(main, this);
                eventAddWindow.Owner = this;
            }
            if (UserList.SelectedIndex == -1)
            {
                return;
            }
            UserFileListData data = (UserFileListData)UserList.SelectedItem;
            eventAddWindow.SetData(data.fileNameFullPath);
            eventAddWindow.ShowDialog();
        }

        private void editEventFile_Click(object sender, RoutedEventArgs e)
        {
            if (eventEditWindow == null)
            {
                eventEditWindow = new EventEditWindow(main, this);
                eventEditWindow.Owner = this;
            }
            if (UserList.SelectedIndex == -1)
            {
                return;
            }
            if (EventList.SelectedIndex == -1)
            {
                return;
            }
            UserFileListData data = (UserFileListData)UserList.SelectedItem;
            UserEventListData edata = (UserEventListData)EventList.SelectedItem;
            eventEditWindow.SetData(data.fileNameFullPath, UserList.SelectedIndex, EventList.SelectedIndex);
            eventEditWindow.ShowDialog();
        }

        private void deleteFile_Click(object sender, RoutedEventArgs e)
        {
            if (UserList.SelectedIndex == -1)
            {
                MessageBox.Show("削除したいファイルを選択してください。");
                return;
            }
            UserFileListData data = (UserFileListData)UserList.SelectedItem;
            MessageBoxResult res = MessageBox.Show(data.fileName + "を削除してよろしいですか？", "確認", MessageBoxButton.OKCancel,
                MessageBoxImage.Question, MessageBoxResult.Cancel);
            if (res == MessageBoxResult.OK)
            {
                File.Delete(data.fileNameFullPath);
                ReRender();
            }
        }

        private void deleteEventFile_Click(object sender, RoutedEventArgs e)
        {
            if (EventList.SelectedIndex == -1)
            {
                MessageBox.Show("削除したいイベントを選択してください。");
                return;
            }
            UserFileListData fileData = (UserFileListData)UserList.SelectedItem;
            UserEventListData data = (UserEventListData)EventList.SelectedItem;
            MessageBoxResult res = MessageBox.Show(data.eventName + "を削除してよろしいですか？", "確認", MessageBoxButton.OKCancel,
                MessageBoxImage.Question, MessageBoxResult.Cancel);
            if (res == MessageBoxResult.OK)
            {
                UserJsonList jsonObj = GetUserJsonList();
                if (jsonObj == null) return;

                // datasourceと保存ファイル2つのremoveAt
                // VM側を消すとindexも消える
                jsonObj.list.RemoveAt(EventList.SelectedIndex);
                userEventVM.userEventList.RemoveAt(EventList.SelectedIndex);

                // newJsonにoldJson(removeAt済)を入れ直す
                UserJsonList jsonList = new UserJsonList();
                jsonList.list = jsonObj.list;

                try
                {
                    string userJson = JsonSerializer.Serialize(jsonList,
                        new JsonSerializerOptions
                        {
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                            WriteIndented = true
                        });
                    // createって上書きされるよね
                    using (FileStream fs = new FileStream(fileData.fileNameFullPath, FileMode.Create))
                    {
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine(userJson);
                        sw.Close();
                    }
                }
                catch (Exception e1)
                {
                    Debug.WriteLine(e1.Message);
                    MessageBox.Show("エラーが発生しました。");

                    return;
                }

                ReRenderEvent(fileData.fileNameFullPath);
            }
        }

        private void eclipseFile_Click(object sender, RoutedEventArgs e)
        {
            if (eclipseWindow == null)
            {
                eclipseWindow = new EclipseWindow(main, this);
                eclipseWindow.Owner = this;
            }
            if (UserList.SelectedIndex == -1)
            {
                return;
            }
            if (EventList.SelectedIndex == -1)
            {
                return;
            }
            UserFileListData data = (UserFileListData)UserList.SelectedItem;
            UserEventListData eventFileData = (UserEventListData)EventList.SelectedItem;
            eclipseWindow.SetData(data.fileNameFullPath, UserList.SelectedIndex, EventList.SelectedIndex);
            eclipseWindow.ShowDialog();
        }

        private void DirList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {


        }

        private void newDir_Click(object sender, RoutedEventArgs e)
        {
            if (dirAddWindow == null)
            {
                dirAddWindow = new DirAddWindow(main, this);
                dirAddWindow.Owner = this;
            }
            dirAddWindow.ShowDialog();
        }

        private void editDir_Click(object sender, RoutedEventArgs e)
        {
            if (dirEditWindow == null)
            {
                dirEditWindow = new DirEditWindow(main, this);
                dirEditWindow.Owner = this;
            }
            dirEditWindow.SetData(((UserFileListData)(DirList.SelectedItem)).fileName, ((UserFileListData)(DirList.SelectedItem)).fileNameFullPath);
            dirEditWindow.ShowDialog();
        }

        private void deleteDir_Click(object sender, RoutedEventArgs e)
        {
            UserFileListData dirData = (UserFileListData)DirList.SelectedItem;
            MessageBoxResult res = MessageBox.Show(dirData.fileName + "を削除してよろしいですか？", "確認", MessageBoxButton.OKCancel,
                MessageBoxImage.Question, MessageBoxResult.Cancel);
            if (res == MessageBoxResult.OK)
            {
                Directory.Delete(dirData.fileNameFullPath, true);

            }
            ReRenderDir();
        }

        private void DirList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DirList.SelectedIndex == -1)
            {
                return;
            }
            if (DirList.SelectedIndex > 0)
            {
                editDir.IsEnabled = true;
                deleteDir.IsEnabled = true;
            }
            else
            {
                editDir.IsEnabled = false;
                deleteDir.IsEnabled = false;
            }
            string? dirNameFullPath = ((UserFileListData)(DirList.SelectedItem)).fileNameFullPath;
            string? dirName = ((UserFileListData)(DirList.SelectedItem)).fileName;
            currentFullPath = dirNameFullPath;
            ReRender();


            /*
            string? dirNameFullPath = ((UserFileListData)(DirList.SelectedItem)).fileNameFullPath;
            string? dirName = ((UserFileListData)(DirList.SelectedItem)).fileName;

            string root = Util.root();
            if (dirName == ".." && currentFullPath != root + @"\data")
            {
                DirectoryInfo dir = new DirectoryInfo(currentFullPath);
                currentFullPath = dir.Parent.FullName;
                ReRenderDir();
                ReRender();
                userEventVM.Clear();
            }
            else
            {
                DirectoryInfo dir = new DirectoryInfo(dirNameFullPath);
                currentFullPath = dir.FullName;
                ReRenderDir();
                ReRender();
                userEventVM.Clear();
            }
            */
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\";
            ofd.Title = "ファイルを選択してください";
            try
            {
                if (importCombo.SelectedIndex == 0)
                {
                    //AMATERU
                    ofd.Filter = "csv File(*.csv;)|*.csv|すべてのファイル|*.*";
                    if (ofd.ShowDialog() == true)
                    {
                        string root = Util.root();

                        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            PrepareHeaderForMatch = args => args.Header.ToUpper(),
                            Delimiter = "\t"
                        };
                        int i = 0;
                        using (var reader = new StreamReader(ofd.FileName, Encoding.GetEncoding("UTF-8")))
                        {
                            using (var csv = new CsvHelper.CsvReader(reader, config))
                            {
                                var records = csv.GetRecords<AmateruCsv>();

                                UserJsonList jsonList = new UserJsonList();
                                jsonList.list = new List<UserData>();
                                foreach (AmateruCsv record in records)
                                {
                                    DateTime date = DateTime.Now;
                                    if (String.IsNullOrEmpty(record.DATE))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        date = DateTime.Parse(record.DATE);
                                    }

                                    string[] time = record.TIME.Split(':');

                                    int hour;
                                    int minute;
                                    int second;
                                    if (time.Length > 2)
                                    {
                                        if (!Int32.TryParse(time[0], out hour))
                                        {
                                            hour = 12;
                                        }
                                        if (!Int32.TryParse(time[1], out minute))
                                        {
                                            minute = 0;
                                        }
                                        if (!Int32.TryParse(time[2], out second))
                                        {
                                            second = 0;
                                        }
                                    }
                                    else
                                    {
                                        hour = 12;
                                        minute = 0;
                                        second = 0;
                                    }

                                    double lat;
                                    if (String.IsNullOrEmpty(record.LATITUDE))
                                    {
                                        lat = main.configData.lat;
                                    }
                                    else if (!Double.TryParse(record.LATITUDE, out lat))
                                    {
                                        lat = main.configData.lat;
                                    }
                                    double lng;
                                    if (String.IsNullOrEmpty(record.LONGITUDE))
                                    {
                                        lng = main.configData.lng;
                                    }
                                    else if (!Double.TryParse(record.LONGITUDE, out lng))
                                    {
                                        lng = main.configData.lng;
                                    }

                                    //todo AMATERUはJSTなのでめんどい
                                    double timezone = 9.0;

                                    string memo = String.Format("kana: {0}, GENDER: {1}, JOB: {2} \n", record.KANA, record.GENDER, record.JOB);

                                    jsonList.list.Add(new UserData()
                                    {
                                        name = record.NAME,
                                        birth_year = date.Year,
                                        birth_month = date.Month,
                                        birth_day = date.Day,
                                        birth_hour = hour,
                                        birth_minute = minute,
                                        birth_second = second,
                                        birth_place = record.PLACENAME,
                                        timezone = 9.0,
                                        timezone_str = "Asia/Tokyo",
                                        lat = lat,
                                        lng = lng,
                                        memo = memo + record.MEMO
                                    });

                                    i++;
                                    if (i >= 200)
                                    {
                                        MessageBox.Show("インポートが200件を超えたため停止しました。");
                                        break;
                                    }
                                }

                                string userJsonStr = JsonSerializer.Serialize(jsonList, new JsonSerializerOptions
                                {
                                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                                    WriteIndented = true,
                                });

                                string file = root + @"\data\AMATERU" + DateTime.Now.ToString("yyyyMMddHHmm") + ".json";
                                using (FileStream fs = new FileStream(file, FileMode.Create))
                                {
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.WriteLine(userJsonStr);
                                    sw.Close();
                                }
                            }
                        }
                        ReRenderDir();
                        ReRender();
                    }
                }
                else if (importCombo.SelectedIndex == 1)
                {
                    //stargazer
                    ofd.Filter = "stargazer File|*";
                    if (ofd.ShowDialog() == true)
                    {
                        // SGはcsvじゃないので200件だけ読んでおく
                        List<string> dataStr = new List<string>();
                        int i = 0;
                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                        using (var reader = new StreamReader(ofd.FileName, System.Text.Encoding.GetEncoding("Shift_JIS")))
                        {
                            while (reader.Peek() >= 0)
                            {
                                string line = reader.ReadLine();
                                dataStr.Add(line);

                                if (i >= 201) break;
                            }
                        }
                        UserJsonList jsonList = new UserJsonList();
                        jsonList.list = new List<UserData>();
                        foreach (string line in dataStr)
                        {
                            //先頭に読む件数あるけど、データ行は,の有無で判断
                            if (line.IndexOf(",") == 0) continue;

                            try
                            {
                                string trimdata = line.Replace("  ", " ");
                                string[] data = trimdata.Split(' ');
                                // data[0] ymd
                                // data[1] his
                                // data[2] lat
                                // data[3] lng
                                // data[4] other

                                int year = int.Parse(data[0].Substring(0, 4));
                                int month = int.Parse(data[0].Substring(4, 2));
                                int day = int.Parse(data[0].Substring(6, 2));

                                int hour = int.Parse(data[1].Substring(0, 2));
                                int minute = int.Parse(data[1].Substring(2, 2));
                                int second = int.Parse(data[1].Substring(4, 2));

                                // stargazerはUTCで記録されるため、+9:00する
                                DateTime d = new DateTime(year, month, day, hour, minute, second);
                                d = d.AddHours(9.0);

                                string[] name = data[4].Split(',');
                                name[0] = name[0].Replace("\"", "");
                                name[1] = name[1].Replace("\"", "");

                                double lat;
                                if (String.IsNullOrEmpty(data[2]))
                                {
                                    lat = main.configData.lat;
                                }
                                else if (!Double.TryParse(data[2], out lat))
                                {
                                    lat = main.configData.lat;
                                }
                                double lng;
                                if (String.IsNullOrEmpty(data[3]))
                                {
                                    lng = main.configData.lng;
                                }
                                else if (!Double.TryParse(data[3], out lng))
                                {
                                    lng = main.configData.lng;
                                }
                                string memo = String.Format("GENDER: {0} \n", name[3]);
                                jsonList.list.Add(new UserData()
                                {
                                    name = name[1],
                                    birth_year = d.Year,
                                    birth_month = d.Month,
                                    birth_day = d.Day,
                                    birth_hour = d.Hour,
                                    birth_minute = d.Minute,
                                    birth_second = d.Second,
                                    birth_place = name[0],
                                    timezone = 9.0,
                                    timezone_str = "Asia/Tokyo",
                                    lat = lat,
                                    lng = lng,
                                    memo = memo + name[2]
                                });
                            }
                            catch (Exception exception)
                            {
                                Debug.WriteLine(exception.Message);
                                continue;
                            }
                        }
                        if (i >= 200)
                        {
                            MessageBox.Show("インポートが200件を超えたため停止しました。");
                        }

                        string userJsonStr = JsonSerializer.Serialize(jsonList, new JsonSerializerOptions
                        {
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                            WriteIndented = true,
                        });

                        string root = Util.root();
                        string file = root + @"\data\StarGazer" + DateTime.Now.ToString("yyyyMMddHHmm") + ".json";
                        using (FileStream fs = new FileStream(file, FileMode.Create))
                        {
                            StreamWriter sw = new StreamWriter(fs);
                            sw.WriteLine(userJsonStr);
                            sw.Close();
                        }
                        ReRenderDir();
                        ReRender();
                    }
                }
                else if (importCombo.SelectedIndex == 2)
                {
                    //zet
                    ofd.Filter = "Zbs File(*.zbs)|*.zbs";
                    if (ofd.ShowDialog() == true)
                    {
                        int success = 0;
                        int err = 0;
                        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            PrepareHeaderForMatch = args => args.Header.ToUpper(),
                            Delimiter = ";"
                        };
                        // 文字コードのせいでcsvHelperがうまく読めないので普通に読む
                        List<string> dataStr = new List<string>();
                        int i = 0;
                        using (var reader = new StreamReader(ofd.FileName, System.Text.Encoding.GetEncoding("UTF-8")))
                        {
                            while (reader.Peek() >= 0)
                            {
                                string line = reader.ReadLine();
                                dataStr.Add(line);

                                if (i >= 201) break;
                            }
                        }
                        UserJsonList jsonList = new UserJsonList();
                        jsonList.list = new List<UserData>();
                        foreach (string line in dataStr)
                        {
                            try
                            {
                                string[] data = line.Split(';');
                                if (data.Length < 8)
                                {
                                    continue;
                                }
                                // data[0] ymd
                                // data[1] his
                                // data[2] lat
                                // data[3] lng
                                // data[4] other

                                string name = data[0];

                                // dd.mm.yyyyじゃなくて一桁だったりする
                                string d = data[1].Trim(' ');
                                string[] dd = d.Split(".");
                                int year = Int32.Parse(dd[2]);
                                int month = Int32.Parse(dd[1]);
                                int day = Int32.Parse(dd[0]);

                                DateTime date = new DateTime(year, month, day);

                                string t = data[2].Trim(' ');
                                string[] his = t.Split(":");

                                int hour = int.Parse(his[0]);
                                int minute = int.Parse(his[1]);
                                int second = 0;

                                double timezeone = 9.0;
                                string timezone_str = "Asia/Tokyo";

                                string place = data[4];
                                double lat = main.configData.lat;
                                double lng = main.configData.lng;


                                string memo = data[7];
                                jsonList.list.Add(new UserData()
                                {
                                    name = name,
                                    birth_year = date.Year,
                                    birth_month = date.Month,
                                    birth_day = date.Day,
                                    birth_hour = hour,
                                    birth_minute = minute,
                                    birth_second = second,
                                    birth_place = place,
                                    timezone = 9.0,
                                    timezone_str = "Asia/Tokyo",
                                    lat = lat,
                                    lng = lng,
                                    memo = memo
                                });
                            }
                            catch (Exception exception)
                            {
                                Debug.WriteLine(exception.Message);
                                continue;
                            }
                        }
                        if (i >= 200)
                        {
                            MessageBox.Show("インポートが200件を超えたため停止しました。");
                        }

                        string userJsonStr = JsonSerializer.Serialize(jsonList, new JsonSerializerOptions
                        {
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                            WriteIndented = true,
                        });

                        string root = Util.root();
                        string file = root + @"\data\Zet" + DateTime.Now.ToString("yyyyMMddHHmm") + ".json";
                        using (FileStream fs = new FileStream(file, FileMode.Create))
                        {
                            StreamWriter sw = new StreamWriter(fs);
                            sw.WriteLine(userJsonStr);
                            sw.Close();
                        }
                        ReRenderDir();
                        ReRender();
                    }
                }

            } catch (Exception ex)
            {
                MessageBox.Show("エラーが発生しました。\n", ex.Message);
            }

        }

        private void EventList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = (ListView)sender;
            if (lv.SelectedIndex == -1)
            {
                return;
            }
            UserEventListData data = (UserEventListData)lv.SelectedItem;
            memo.Text = data.eventMemo;
        }
    }
}
