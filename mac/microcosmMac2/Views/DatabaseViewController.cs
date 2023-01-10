using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using microcosmMac2.Views.DataSources;
using microcosmMac2.Common;
using System.IO;
using System.Diagnostics;
using microcosmMac2.Config;
using System.Text.Encodings.Web;
using System.Text.Json;
using microcosmMac2.User;
using System.Text.Unicode;
using WebKit;
using static System.Net.WebRequestMethods;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using microcosmMac2.Models;
using System.Net.Http;

namespace microcosmMac2.Views
{
    public partial class DatabaseViewController : AppKit.NSViewController
    {
        AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;

        public string currentPath;
        #region Constructors

        // Called when created from unmanaged code
        public DatabaseViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public DatabaseViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public DatabaseViewController() : base("DatabaseView", NSBundle.MainBundle)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            appDelegate.dbViewController = this;

            var root = Util.root;

            if (!Directory.Exists(root + "/data"))
            {
                Directory.CreateDirectory(root + "/data");
            }

            var uDataSource = new FilesDataSource();
            filesList.DataSource = uDataSource;
            filesList.Delegate = new FilesDelegate(uDataSource);
            var eDataSource = new EventDataSource();
            UserEventList.DataSource = eDataSource;
            UserEventList.Delegate = new EventDelegate(eDataSource);
            var dDataSource = new DirDataSource();
            DirList.DataSource = dDataSource;
            DirList.Delegate = new DirDataDelegate(dDataSource);

            currentPath = root + "/data";
            DirRefresh();
            FilesRefresh();
            string[] items = new string[] { "Amateru(csv)", "stargazer", "Zet(zbs)", "SolarFire(txt)", "Kepler(txt)" };

            ImportCombo.RemoveAllItems();
            ImportCombo.AddItems(items);

            string n = appDelegate.viewController.udata1.name;
            string d = String.Format("{0}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}({6})",
                appDelegate.viewController.udata1.birth_year,
                appDelegate.viewController.udata1.birth_month,
                appDelegate.viewController.udata1.birth_day,
                appDelegate.viewController.udata1.birth_hour,
                appDelegate.viewController.udata1.birth_minute,
                appDelegate.viewController.udata1.birth_second,
                appDelegate.viewController.udata1.timezone);
            string l = String.Format("{0} {1}", appDelegate.viewController.udata1.lat, appDelegate.viewController.udata1.lng);
            user1Label.StringValue = n + "\n" + d + "\n" + l;

            n = appDelegate.viewController.udata2.name;
            d = String.Format("{0}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}({6})",
                appDelegate.viewController.udata2.birth_year,
                appDelegate.viewController.udata2.birth_month,
                appDelegate.viewController.udata2.birth_day,
                appDelegate.viewController.udata2.birth_hour,
                appDelegate.viewController.udata2.birth_minute,
                appDelegate.viewController.udata2.birth_second,
                appDelegate.viewController.udata2.timezone);
            l = String.Format("{0} {1}", appDelegate.viewController.udata2.lat, appDelegate.viewController.udata2.lng);
            user2Label.StringValue = n + "\n" + d + "\n" + l;

            n = appDelegate.viewController.edata1.name;
            d = String.Format("{0}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}({6})",
                appDelegate.viewController.edata1.birth_year,
                appDelegate.viewController.edata1.birth_month,
                appDelegate.viewController.edata1.birth_day,
                appDelegate.viewController.edata1.birth_hour,
                appDelegate.viewController.edata1.birth_minute,
                appDelegate.viewController.edata1.birth_second,
                appDelegate.viewController.edata1.timezone);
            l = String.Format("{0} {1}", appDelegate.viewController.edata1.lat, appDelegate.viewController.edata1.lng);
            event1Label.StringValue = n + "\n" + d + "\n" + l;

            n = appDelegate.viewController.edata2.name;
            d = String.Format("{0}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}({6})",
                appDelegate.viewController.edata2.birth_year,
                appDelegate.viewController.edata2.birth_month,
                appDelegate.viewController.edata2.birth_day,
                appDelegate.viewController.edata2.birth_hour,
                appDelegate.viewController.edata2.birth_minute,
                appDelegate.viewController.edata2.birth_second,
                appDelegate.viewController.edata2.timezone);
            l = String.Format("{0} {1}", appDelegate.viewController.edata2.lat, appDelegate.viewController.edata2.lng);
            event2Label.StringValue = n + "\n" + d + "\n" + l;

        }

        //strongly typed view accessor
        public new DatabaseView View
        {
            get
            {
                return (DatabaseView)base.View;
            }
        }

        /// <summary>
        /// old:ディレクトリ一覧を表示 new:jsonファイル一覧を表示
        /// </summary>
        public void FilesRefresh()
        {
            var root = Util.root;
            FilesRefresh(root + @"/data");
        }

        public void FilesRefresh(string dir)
        {
            FilesDataSource uDataSource = new FilesDataSource();
            EventDataSource eDataSource = new EventDataSource();

            string[] jsonFiles = Directory.GetFiles(dir, "*.json");
            uDataSource.names.Clear();

            // GetFilesで取得できるのはフルパス
            foreach (var jsonFile in jsonFiles)
            {
                //Debug.WriteLine(file);
                uDataSource.names.Add(new FileName()
                {
                    name = System.IO.Path.GetFileName(jsonFile),
                    nameFullPath = jsonFile
                });
            }

            filesList.DataSource = uDataSource;
            filesList.Delegate = new FilesDelegate(uDataSource);
            // ファイルがあれば先頭ファイルを選択
            if (jsonFiles.Length == 0)
            {
                appDelegate.dbSavedFile = "";
                eDataSource.names.Clear();
                UserEventList.Delegate = new EventDelegate(eDataSource);
            }
            else
            {
                appDelegate.dbSavedFile = jsonFiles[0];

                filesList.SelectRow(0, false);
                FilesListSelectionChanged(0);

            }
        }

        /// <summary>
        /// UserAdd、EventAdd等から呼ばれる
        /// eventIndexは0になる
        /// </summary>
        /// <param name="directory"></param>
        public void EventRefresh()
        {
            var root = Util.root;
            var eDataSource = new EventDataSource();

            UserJsonList jsonList = GetJsonList();

            int index = 0;
            foreach (UserJson json in jsonList.list)
            {
                eDataSource.names.Add(new EventData(
                    index.ToString(),
                    json.name,
                    json.birth_year.ToString(),
                    json.birth_month.ToString("00"),
                    json.birth_day.ToString("00"),
                    json.birth_hour.ToString("00"),
                    json.birth_minute.ToString("00"),
                    json.birth_second.ToString("00"),
                    json.birth_timezone.ToString("00.00"),
                    json.birth_place,
                    json.lat.ToString(),
                    json.lng.ToString(),
                    json.memo
                ));
                index++;
            }

            UserEventList.DataSource = eDataSource;
            UserEventList.Delegate = new EventDelegate(eDataSource);
            UserEventList.SelectRow(0, false);
            appDelegate.dbSavedEventIndex = 0;
            memoField.StringValue = "";
        }

        public void DirRefresh()
        {
            DirDataSource dDataSource = new DirDataSource();

            DirectoryInfo dir = new DirectoryInfo(currentPath);
            dDataSource.names.Add(new DirData()
            {
                name = ".",
                nameFullPath = Util.root + @"/data"
            });
            DirectoryInfo[] subDirs = dir.GetDirectories();
            foreach (var d in subDirs)
            {
                dDataSource.names.Add(new DirData()
                {
                    name = d.Name,
                    nameFullPath = d.FullName
                });
            }
            DirList.DataSource = dDataSource;
            DirList.Delegate = new DirDataDelegate(dDataSource);
            appDelegate.dbSavedDir = dDataSource.names[0].name;
            appDelegate.dbSavedDirFullPath = dDataSource.names[0].nameFullPath;

            DirList.SelectRow(0, false);
        }

        /// <summary>
        /// 左側クリック
        /// </summary>
        /// <param name="sender"></param>
        partial void FilesListClicked(Foundation.NSObject sender)
        {
            NSTableView s = (NSTableView)sender;
            Debug.WriteLine(s.ClickedRow);

            if (s.ClickedRow >= 0)
            {
                FilesListSelectionChanged((int)s.ClickedRow);
                EditUserButton.Enabled = true;
                DeleteUserButton.Enabled = true;
                AddEventButton.Enabled = true;
                EditEventButton.Enabled = false;
                DeleteEventButton.Enabled = false;
            }
            else
            {
                EditUserButton.Enabled = false;
                DeleteUserButton.Enabled = false;
                AddEventButton.Enabled = false;
                EditEventButton.Enabled = false;
                DeleteEventButton.Enabled = false;
            }

        }

        /// <summary>
        /// ファイル選択子関数
        /// </summary>
        /// <param name="index"></param>
        public void FilesListSelectionChanged(int index)
        {
            var root = Util.root;

            EventDataSource eDataSource = new EventDataSource();

            FilesDataSource source = (FilesDataSource)filesList.DataSource;
            string target = source.names[index].name;
            string targetFullPath = source.names[index].nameFullPath;

            appDelegate.dbSavedFile = targetFullPath;

            // old:ディレクトリ内のファイル一覧
            // new:json内のデータ一覧
            UserJsonList jsonObj = GetJsonList();
            if (jsonObj == null) return;
            if (jsonObj.list == null) return;

            int ii = 0;
            foreach (UserJson json in jsonObj.list)
            {
                eDataSource.names.Add(new EventData(
                    ii.ToString(),
                    json.name,
                    json.birth_year.ToString(),
                    json.birth_month.ToString("00"),
                    json.birth_day.ToString("00"),
                    json.birth_hour.ToString("00"),
                    json.birth_minute.ToString("00"),
                    json.birth_second.ToString("00"),
                    json.birth_timezone.ToString("00"),
                    json.birth_place,
                    json.lat.ToString(),
                    json.lng.ToString(),
                    json.memo
                ));
                ii++;
            }

            UserEventList.DataSource = eDataSource;
            UserEventList.Delegate = new EventDelegate(eDataSource); ;
            UserEventList.SelectRow(0, false);
            memoField.StringValue = "";
        }

        /// <summary>
        /// 単純にファイル消すだけ
        /// </summary>
        /// <param name="sender"></param>
        partial void DeleteUserClicked(Foundation.NSObject sender)
        {
            int index = (int)filesList.SelectedRow;
            FilesDataSource source = (FilesDataSource)filesList.DataSource;
            string target = source.names[index].name;
            string targetFullPath = source.names[index].nameFullPath;


            NSAlert alert = new NSAlert();
            alert.AlertStyle = NSAlertStyle.Warning;

            alert.MessageText = target + "を削除しますか?";
            alert.InformativeText = "確認";

            alert.AddButton("OK");
            alert.AddButton("Cancel");
            alert.BeginSheet(NSApplication.SharedApplication.KeyWindow, (NSModalResponse response) =>
            {
                if ((int)response == 1000)
                {
                    var root = Util.root;

                    System.IO.File.Delete(targetFullPath);
                    FilesRefresh();
                }
            });
        }

        /// <summary>
        /// 右側クリック
        /// </summary>
        /// <param name="sender"></param>
        partial void UserEventListClicked(Foundation.NSObject sender)
        {
            NSTableView s = (NSTableView)sender;
            Debug.WriteLine(s.ClickedRow);

            if (s.ClickedRow >= 0)
            {
                appDelegate.dbSavedEventIndex = (int)s.ClickedRow;

                EventListSelectionChanged((int)s.ClickedRow);
                if (appDelegate.dbSavedFile.Length > 0)
                {
                    EditUserButton.Enabled = true;
                    DeleteUserButton.Enabled = true;
                }
                AddEventButton.Enabled = true;
                EditEventButton.Enabled = true;
                DeleteEventButton.Enabled = true;
                EclipseButton.Enabled = true;

                UserJsonList jsonObj = GetJsonList();
                if (jsonObj == null) return;
                if (jsonObj.list == null) return;

                EventDataSource source = (EventDataSource)UserEventList.DataSource;
                memoField.StringValue = source.names[(int)s.ClickedRow].eventMemo;
            }
            else
            {
                AddEventButton.Enabled = false;
                EditEventButton.Enabled = false;
                DeleteEventButton.Enabled = false;
                EclipseButton.Enabled = false;
            }


        }

        /// <summary>
        /// 旧verとか確かZetもメモ欄出してたよね
        /// </summary>
        /// <param name="index"></param>
        public void EventListSelectionChanged(int index)
        {
            //todo
        }

        /// <summary>
        /// イベント側の削除
        /// indexを抜いたリストを作ってjson保存、リフレッシュ
        /// </summary>
        /// <param name="sender"></param>
        partial void DeleteEventClicked(Foundation.NSObject sender)
        {
            NSAlert alert = new NSAlert();
            alert.AlertStyle = NSAlertStyle.Warning;
            int index = (int)UserEventList.SelectedRow;
            EventDataSource source = (EventDataSource)UserEventList.DataSource;
            string target = source.names[index].eventName;

            alert.MessageText = target + "を削除しますか?";
            alert.InformativeText = "確認";

            alert.AddButton("OK");
            alert.AddButton("Cancel");
            alert.BeginSheet(NSApplication.SharedApplication.KeyWindow, (NSModalResponse response) =>
            {
                if ((int)response == 1000)
                {
                    var root = Util.root;

                    UserJsonList jsonObj = GetJsonList();
                    if (jsonObj == null) return;

                    // datasourceと保存ファイル2つのremoveAt
                    ((EventDataSource)UserEventList.DataSource).names.RemoveAt(appDelegate.dbSavedEventIndex);
                    jsonObj.list.RemoveAt(appDelegate.dbSavedEventIndex);

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
                        using (FileStream fs = new FileStream(appDelegate.dbSavedFile, FileMode.Create))
                        {
                            StreamWriter sw = new StreamWriter(fs);
                            sw.WriteLine(userJson);
                            sw.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        NSAlert alert1 = new NSAlert();
                        alert1.MessageText = "エラーが発生しました。";
                        alert1.RunModal();

                        return;
                    }


                    EventRefresh();
                }
            });

        }

        /// <summary>
        /// ファイル読むだけ
        /// </summary>
        /// <returns></returns>
        private UserJsonList GetJsonList()
        {
            UserJsonList jsonObj = null;
            using (FileStream fs = new FileStream(appDelegate.dbSavedFile, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                string jsonData = sr.ReadToEnd();
                var option = new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, };
                jsonObj = JsonSerializer.Deserialize<UserJsonList>(jsonData, option);
            }
            return jsonObj;
        }

        /// <summary>
        /// 参照渡しでラベルテキスト取得
        /// </summary>
        /// <param name="json"></param>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="latlng"></param>
        private void RefSetLabelText(UserJson json, ref string name, ref string date, ref string latlng)
        {
            string n = json.name;
            string d = String.Format("{0}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}({6})",
                json.birth_year,
                json.birth_month,
                json.birth_day,
                json.birth_hour,
                json.birth_minute,
                json.birth_second,
                json.birth_timezone);
            string l = String.Format("{0} {1}", json.lat, json.lng);

            name = n;
            date = d;
            latlng = l;
        }

        partial void User1Clicked(Foundation.NSObject sender)
        {
            UserJsonList jsonObj = GetJsonList();
            if (jsonObj == null) return;
            if (jsonObj.list == null) return;

            int index = appDelegate.dbSavedEventIndex;

            string n = "";
            string d = "";
            string l = "";
            RefSetLabelText(jsonObj.list[index], ref n, ref d, ref l);
            user1Label.StringValue = n + "\n" + d + "\n" + l;

            appDelegate.udata1.name = jsonObj.list[index].name;
            appDelegate.udata1.birth_year = jsonObj.list[index].birth_year;
            appDelegate.udata1.birth_month = jsonObj.list[index].birth_month;
            appDelegate.udata1.birth_day = jsonObj.list[index].birth_day;
            appDelegate.udata1.birth_hour = jsonObj.list[index].birth_hour;
            appDelegate.udata1.birth_minute = jsonObj.list[index].birth_minute;
            appDelegate.udata1.birth_second = jsonObj.list[index].birth_second;
            appDelegate.udata1.timezone = jsonObj.list[index].birth_timezone;
            appDelegate.udata1.timezone_str = jsonObj.list[index].birth_timezone_str;
            appDelegate.udata1.lat = jsonObj.list[index].lat;
            appDelegate.udata1.lng = jsonObj.list[index].lng;
            appDelegate.udata1.birth_place = jsonObj.list[index].birth_place;
            appDelegate.udata1.memo = jsonObj.list[index].memo;

            appDelegate.viewController.RefreshUserBox(0, new UserData(jsonObj.list[index]));
            appDelegate.viewController.ReCalc();
            appDelegate.viewController.ReRender();
        }

        partial void User2Clicked(Foundation.NSObject sender)
        {
            UserJsonList jsonObj = GetJsonList();
            if (jsonObj == null) return;
            if (jsonObj.list == null) return;

            int index = appDelegate.dbSavedEventIndex;

            string n = "";
            string d = "";
            string l = "";
            RefSetLabelText(jsonObj.list[index], ref n, ref d, ref l);
            user2Label.StringValue = n + "\n" + d + "\n" + l;

            appDelegate.udata2.name = jsonObj.list[index].name;
            appDelegate.udata2.birth_year = jsonObj.list[index].birth_year;
            appDelegate.udata2.birth_month = jsonObj.list[index].birth_month;
            appDelegate.udata2.birth_day = jsonObj.list[index].birth_day;
            appDelegate.udata2.birth_hour = jsonObj.list[index].birth_hour;
            appDelegate.udata2.birth_minute = jsonObj.list[index].birth_minute;
            appDelegate.udata2.birth_second = jsonObj.list[index].birth_second;
            appDelegate.udata2.timezone = jsonObj.list[index].birth_timezone;
            appDelegate.udata2.timezone_str = jsonObj.list[index].birth_timezone_str;
            appDelegate.udata2.lat = jsonObj.list[index].lat;
            appDelegate.udata2.lng = jsonObj.list[index].lng;
            appDelegate.udata2.birth_place = jsonObj.list[index].birth_place;
            appDelegate.udata2.memo = jsonObj.list[index].memo;

            appDelegate.viewController.RefreshUserBox(1, new UserData(jsonObj.list[index]));
            appDelegate.viewController.ReCalc();
            appDelegate.viewController.ReRender();
        }

        partial void Event1Clicked(Foundation.NSObject sender)
        {
            UserJsonList jsonObj = GetJsonList();
            if (jsonObj == null) return;
            if (jsonObj.list == null) return;

            int index = appDelegate.dbSavedEventIndex;

            string n = "";
            string d = "";
            string l = "";
            RefSetLabelText(jsonObj.list[index], ref n, ref d, ref l);

            event1Label.StringValue = n + "\n" + d + "\n" + l;

            appDelegate.edata1.name = jsonObj.list[index].name;
            appDelegate.edata1.birth_year = jsonObj.list[index].birth_year;
            appDelegate.edata1.birth_month = jsonObj.list[index].birth_month;
            appDelegate.edata1.birth_day = jsonObj.list[index].birth_day;
            appDelegate.edata1.birth_hour = jsonObj.list[index].birth_hour;
            appDelegate.edata1.birth_minute = jsonObj.list[index].birth_minute;
            appDelegate.edata1.birth_second = jsonObj.list[index].birth_second;
            appDelegate.edata1.timezone = jsonObj.list[index].birth_timezone;
            appDelegate.edata1.timezone_str = jsonObj.list[index].birth_timezone_str;
            appDelegate.edata1.lat = jsonObj.list[index].lat;
            appDelegate.edata1.lng = jsonObj.list[index].lng;
            appDelegate.edata1.birth_place = jsonObj.list[index].birth_place;
            appDelegate.edata1.memo = jsonObj.list[index].memo;

            appDelegate.viewController.RefreshEventBox(0, new UserData(jsonObj.list[index]));
            appDelegate.viewController.ReCalc();
            appDelegate.viewController.ReRender();
        }

        partial void Event2Clicked(Foundation.NSObject sender)
        {
            UserJsonList jsonObj = GetJsonList();
            if (jsonObj == null) return;
            if (jsonObj.list == null) return;

            int index = appDelegate.dbSavedEventIndex;

            string n = "";
            string d = "";
            string l = "";
            RefSetLabelText(jsonObj.list[index], ref n, ref d, ref l);

            event2Label.StringValue = n + "\n" + d + "\n" + l;

            appDelegate.edata2.name = jsonObj.list[index].name;
            appDelegate.edata2.birth_year = jsonObj.list[index].birth_year;
            appDelegate.edata2.birth_month = jsonObj.list[index].birth_month;
            appDelegate.edata2.birth_day = jsonObj.list[index].birth_day;
            appDelegate.edata2.birth_hour = jsonObj.list[index].birth_hour;
            appDelegate.edata2.birth_minute = jsonObj.list[index].birth_minute;
            appDelegate.edata2.birth_second = jsonObj.list[index].birth_second;
            appDelegate.edata2.timezone = jsonObj.list[index].birth_timezone;
            appDelegate.edata2.timezone_str = jsonObj.list[index].birth_timezone_str;
            appDelegate.edata2.lat = jsonObj.list[index].lat;
            appDelegate.edata2.lng = jsonObj.list[index].lng;
            appDelegate.edata2.birth_place = jsonObj.list[index].birth_place;
            appDelegate.edata2.memo = jsonObj.list[index].memo;

            appDelegate.viewController.RefreshEventBox(1, new UserData(jsonObj.list[index]));
            appDelegate.viewController.ReCalc();
            appDelegate.viewController.ReRender();

        }

        /// <summary>
        /// NSTableViewを継承しDirViewから呼ばれる
        /// </summary>
        public void DirClick()
        {
            int index = (int)DirList.SelectedRow;
            if (index == -1) return;

        }

        partial void AddDirClicked(Foundation.NSObject sender)
        {

        }

        partial void DirListClicked(Foundation.NSObject sender)
        {
            NSTableView s = (NSTableView)sender;
            Debug.WriteLine(s.ClickedRow);

            if (s.ClickedRow == 0)
            {
                AddDirButton.Enabled = true;
                EditDirButton.Enabled = false;
                DeleteDirButton.Enabled = false;
            }
            else
            {
                AddDirButton.Enabled = true;
                EditDirButton.Enabled = true;
                DeleteDirButton.Enabled = true;
            }
            if (s.ClickedRow >= 0)
            {
                int index = (int)DirList.SelectedRow;
                DirDataSource source = (DirDataSource)DirList.DataSource;
                string target = source.names[index].name;
                string targetFullPath = source.names[index].nameFullPath;

                appDelegate.dbSavedDir = target;
                appDelegate.dbSavedDirFullPath = targetFullPath;
                FilesRefresh(targetFullPath);
            }
        }

        partial void DeleteDirClicked(Foundation.NSObject sender)
        {
            int index = (int)DirList.SelectedRow;
            DirDataSource source = (DirDataSource)DirList.DataSource;
            string target = source.names[index].name;
            string targetFullPath = source.names[index].nameFullPath;

            NSAlert alert = new NSAlert();
            alert.MessageText = target + "を削除しますか?";
            alert.InformativeText = "確認";

            alert.AddButton("OK");
            alert.AddButton("Cancel");
            alert.BeginSheet(NSApplication.SharedApplication.KeyWindow, (NSModalResponse response) =>
            {
                Directory.Delete(targetFullPath, true);
                DirRefresh();
                FilesRefresh();
            });
        }

        partial void CloseClicked(Foundation.NSObject sender)
        {
            DismissViewController(this);
        }

        partial void ImportClicked(Foundation.NSObject sender)
        {
            try
            {
                NSOpenPanel ofd = new NSOpenPanel();
                ofd.Title = "ファイルを選択してください";
                if (ImportCombo.SelectedItem.Title == "Amateru(csv)")
                {
                    //AMATERU
                    ofd.AllowedFileTypes = new string[] { "csv" };
                    if (ofd.RunModal() == 1)
                    {
                        string root = Util.root;

                        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            PrepareHeaderForMatch = args => args.Header.ToUpper(),
                            Delimiter = "\t"
                        };
                        int i = 0;
                        using (var reader = new StreamReader(ofd.Url.ToString().Replace("file://", "").Replace("%20", " "), Encoding.GetEncoding("UTF-8")))
                        {
                            using (var csv = new CsvHelper.CsvReader(reader, config))
                            {
                                var records = csv.GetRecords<AmateruCsv>();

                                UserJsonList jsonList = new UserJsonList();
                                jsonList.list = new List<UserJson>();
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
                                        lat = appDelegate.config.lat;
                                    }
                                    else if (!Double.TryParse(record.LATITUDE, out lat))
                                    {
                                        lat = appDelegate.config.lat;
                                    }
                                    double lng;
                                    if (String.IsNullOrEmpty(record.LONGITUDE))
                                    {
                                        lng = appDelegate.config.lng;
                                    }
                                    else if (!Double.TryParse(record.LONGITUDE, out lng))
                                    {
                                        lng = appDelegate.config.lng;
                                    }

                                    //todo AMATERUはJSTなのでめんどい
                                    double timezone = 9.0;

                                    string memo = String.Format("kana: {0}, GENDER: {1}, JOB: {2} \n", record.KANA, record.GENDER, record.JOB);

                                    jsonList.list.Add(new UserJson()
                                    {
                                        name = record.NAME,
                                        birth_year = date.Year,
                                        birth_month = date.Month,
                                        birth_day = date.Day,
                                        birth_hour = hour,
                                        birth_minute = minute,
                                        birth_second = second,
                                        birth_place = record.PLACENAME,
                                        birth_timezone = 9.0,
                                        birth_timezone_str = "Asia/Tokyo",
                                        lat = lat,
                                        lng = lng,
                                        memo = memo + record.MEMO
                                    });

                                    i++;
                                    if (i >= 200)
                                    {
                                        NSAlert alert = new NSAlert();
                                        alert.MessageText = "インポートが200件を超えたため停止しました。";
                                        alert.RunModal();
                                        break;
                                    }
                                }

                                string userJsonStr = JsonSerializer.Serialize(jsonList, new JsonSerializerOptions
                                {
                                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                                    WriteIndented = true,
                                });

                                string file = root + @"/data/AMATERU" + DateTime.Now.ToString("yyyyMMddHHmm") + ".json";
                                using (FileStream fs = new FileStream(file, FileMode.Create))
                                {
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.WriteLine(userJsonStr);
                                    sw.Close();
                                }
                            }
                        }
                        DirRefresh();
                        FilesRefresh();
                    }
                }
                else if (ImportCombo.SelectedItem.Title == "stargazer")
                {
                    NSAlert alert2 = new NSAlert();
                    alert2.MessageText = "Ｓtargazerのファイルを読み込む場合、まず外部ソフトでutf-8エンコードに変換してください。";
                    alert2.RunModal();

                    //stargazer
                    if (ofd.RunModal() == 1)
                    {
                        // SGはcsvじゃないので200件だけ読んでおく
                        List<string> dataStr = new List<string>();
                        int i = 0;
                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                        using (var reader = new StreamReader(ofd.Url.ToString().Replace("file://", "").Replace("%20", " "), System.Text.Encoding.GetEncoding("UTF-8")))
                        {
                            while (reader.Peek() >= 0)
                            {
                                string line = reader.ReadLine();
                                dataStr.Add(line);

                                if (i >= 201) break;
                            }
                        }
                        UserJsonList jsonList = new UserJsonList();
                        jsonList.list = new List<UserJson>();
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
                                    lat = appDelegate.config.lat;
                                }
                                else if (!Double.TryParse(data[2], out lat))
                                {
                                    lat = appDelegate.config.lat;
                                }
                                double lng;
                                if (String.IsNullOrEmpty(data[3]))
                                {
                                    lng = appDelegate.config.lng;
                                }
                                else if (!Double.TryParse(data[3], out lng))
                                {
                                    lng = appDelegate.config.lng;
                                }
                                string memo = String.Format("GENDER: {0} \n", name[3]);
                                jsonList.list.Add(new UserJson()
                                {
                                    name = name[1],
                                    birth_year = d.Year,
                                    birth_month = d.Month,
                                    birth_day = d.Day,
                                    birth_hour = d.Hour,
                                    birth_minute = d.Minute,
                                    birth_second = d.Second,
                                    birth_place = name[0],
                                    birth_timezone = 9.0,
                                    birth_timezone_str = "Asia/Tokyo",
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
                            NSAlert alert = new NSAlert();
                            alert.MessageText = "インポートが200件を超えたため停止しました。";
                            alert.RunModal();
                        }

                        string userJsonStr = JsonSerializer.Serialize(jsonList, new JsonSerializerOptions
                        {
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                            WriteIndented = true,
                        });

                        string root = Util.root;
                        string file = root + @"/data/StarGazer" + DateTime.Now.ToString("yyyyMMddHHmm") + ".json";
                        using (FileStream fs = new FileStream(file, FileMode.Create))
                        {
                            StreamWriter sw = new StreamWriter(fs);
                            sw.WriteLine(userJsonStr);
                            sw.Close();
                        }
                        DirRefresh();
                        FilesRefresh();
                    }
                }
                else if (ImportCombo.SelectedItem.Title == "Zet(zbs)")
                {
                    //zet
                    ofd.AllowedFileTypes = new string[] { "zbs" };
                    if (ofd.RunModal() == 1)
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
                        using (var reader = new StreamReader(ofd.Url.ToString().Replace("file://", "").Replace("%20", " "), System.Text.Encoding.GetEncoding("UTF-8")))
                        {
                            while (reader.Peek() >= 0)
                            {
                                string line = reader.ReadLine();
                                dataStr.Add(line);

                                if (i >= 201) break;
                            }
                        }
                        UserJsonList jsonList = new UserJsonList();
                        jsonList.list = new List<UserJson>();
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
                                double lat = appDelegate.config.lat;
                                double lng = appDelegate.config.lng;


                                string memo = data[7];
                                jsonList.list.Add(new UserJson()
                                {
                                    name = name,
                                    birth_year = date.Year,
                                    birth_month = date.Month,
                                    birth_day = date.Day,
                                    birth_hour = hour,
                                    birth_minute = minute,
                                    birth_second = second,
                                    birth_place = place,
                                    birth_timezone = 9.0,
                                    birth_timezone_str = "Asia/Tokyo",
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
                            NSAlert alert = new NSAlert();
                            alert.MessageText = "インポートが200件を超えたため停止しました。";
                            alert.RunModal();
                        }

                        string userJsonStr = JsonSerializer.Serialize(jsonList, new JsonSerializerOptions
                        {
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                            WriteIndented = true,
                        });

                        string root = Util.root;
                        string file = root + @"/data/Zet" + DateTime.Now.ToString("yyyyMMddHHmm") + ".json";
                        using (FileStream fs = new FileStream(file, FileMode.Create))
                        {
                            StreamWriter sw = new StreamWriter(fs);
                            sw.WriteLine(userJsonStr);
                            sw.Close();
                        }
                        DirRefresh();
                        FilesRefresh();
                    }
                }
                else if (ImportCombo.SelectedItem.Title == "SolarFire(txt)")
                {
                    NSAlert alert = new NSAlert();
                    alert.MessageText = "SolarFireファイルを読み込む場合、\n名前,年,月,日,時,分,秒,場所,緯度,経度\nの順に並べたcsvでエクスポートしてください。";
                    alert.RunModal();
                    //solar fire
                    ofd.AllowedFileTypes = new string[] { "txt" };

                    if (ofd.RunModal() == 1)
                    {
                        string root = Util.root;

                        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            Delimiter = ",",
                            HasHeaderRecord = false
                        };
                        int i = 0;
                        using (var reader = new StreamReader(ofd.Url.ToString().Replace("file://", "").Replace("%20", " "), System.Text.Encoding.GetEncoding("UTF-8")))
                        {
                            using (var csv = new CsvHelper.CsvReader(reader, config))
                            {
                                var records = csv.GetRecords<SolarFireCsv>();

                                UserJsonList jsonList = new UserJsonList();
                                jsonList.list = new List<UserJson>();
                                foreach (SolarFireCsv record in records)
                                {
                                    DateTime date = new DateTime(
                                        record.YEAR,
                                        record.MONTH,
                                        record.DAY,
                                        record.HOUR,
                                        record.MINUTE,
                                        record.SECOND
                                        );


                                    jsonList.list.Add(new UserJson()
                                    {
                                        name = record.NAME,
                                        birth_year = record.YEAR,
                                        birth_month = record.MONTH,
                                        birth_day = record.DAY,
                                        birth_hour = record.HOUR,
                                        birth_minute = record.MINUTE,
                                        birth_second = record.SECOND,
                                        birth_place = record.PLACENAME,
                                        birth_timezone = 9.0,
                                        birth_timezone_str = "Asia/Tokyo",
                                        lat = record.LATITUDE,
                                        lng = record.LONGITUDE,
                                        memo = ""
                                    });

                                    i++;
                                    if (i >= 200)
                                    {
                                        alert.MessageText = "インポートが200件を超えたため停止しました。";
                                        alert.RunModal();
                                        break;
                                    }
                                }

                                string userJsonStr = JsonSerializer.Serialize(jsonList, new JsonSerializerOptions
                                {
                                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                                    WriteIndented = true,
                                });

                                string file = root + @"/data/SolarFire" + DateTime.Now.ToString("yyyyMMddHHmm") + ".json";
                                using (FileStream fs = new FileStream(file, FileMode.Create))
                                {
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.WriteLine(userJsonStr);
                                    sw.Close();
                                }
                            }
                        }
                        DirRefresh();
                        FilesRefresh();
                    }
                }
                else if (ImportCombo.SelectedItem.Title == "Kepler(txt)")
                {
                    NSAlert alert = new NSAlert();
                    alert.MessageText = "Keplerファイルを読み込む場合、Add Quotes and Commasオプションでエクスポートしてください。";
                    alert.RunModal();
                    //kepler
                    ofd.AllowedFileTypes = new string[] { "txt" };
                    if (ofd.RunModal() == 1)
                    {
                        string root = Util.root;

                        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            Delimiter = ",",
                            HasHeaderRecord = false
                        };
                        int i = 0;
                        using (var reader = new StreamReader(ofd.Url.ToString().Replace("file://", "").Replace("%20", " "), System.Text.Encoding.GetEncoding("UTF-8")))
                        {
                            using (var csv = new CsvHelper.CsvReader(reader, config))
                            {
                                var records = csv.GetRecords<KeplerCsv>();

                                UserJsonList jsonList = new UserJsonList();
                                jsonList.list = new List<UserJson>();
                                foreach (KeplerCsv record in records)
                                {
                                    int month = int.Parse(record.DATE.Substring(0, 2));
                                    int day = int.Parse(record.DATE.Substring(2, 2));
                                    int year = int.Parse(record.DATE.Substring(4, 4));

                                    int hour = int.Parse(record.TIME.Substring(0, 2));
                                    int minute = int.Parse(record.TIME.Substring(2, 2));
                                    int second = int.Parse(record.TIME.Substring(4, 2));

                                    string[] lat;
                                    if (record.LATITUDE.IndexOf("N") > 0)
                                    {
                                        lat = record.LATITUDE.Split("N");
                                    }
                                    else
                                    {
                                        lat = record.LATITUDE.Split("S");
                                    }
                                    int latMin = Int32.Parse(lat[1].Substring(0, 2));
                                    int latSec = Int32.Parse(lat[1].Substring(2, 2));
                                    string[] lon;
                                    if (record.LONGITUDE.IndexOf("E") > 0)
                                    {
                                        lon = record.LONGITUDE.Split("E");
                                    }
                                    else
                                    {
                                        lon = record.LONGITUDE.Split("W");
                                    }
                                    int lonMin = Int32.Parse(lon[1].Substring(0, 2));
                                    int lonSec = Int32.Parse(lon[1].Substring(2, 2));


                                    jsonList.list.Add(new UserJson()
                                    {
                                        name = record.NAME,
                                        birth_year = year,
                                        birth_month = month,
                                        birth_day = day,
                                        birth_hour = hour,
                                        birth_minute = minute,
                                        birth_second = second,
                                        birth_place = record.PLACENAME,
                                        birth_timezone = 9.0,
                                        birth_timezone_str = "Asia/Tokyo",
                                        lat = Double.Parse(lat[0]) + latMin / 60 + latSec / 3600,
                                        lng = Double.Parse(lon[0]) + lonMin / 60 + lonSec / 3600,
                                        memo = ""
                                    });

                                    i++;
                                    if (i >= 200)
                                    {
                                        alert.MessageText = "インポートが200件を超えたため停止しました。";
                                        alert.RunModal();
                                        break;
                                    }
                                }

                                string userJsonStr = JsonSerializer.Serialize(jsonList, new JsonSerializerOptions
                                {
                                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                                    WriteIndented = true,
                                });

                                string file = root + @"/data/Kepler" + DateTime.Now.ToString("yyyyMMddHHmm") + ".json";
                                using (FileStream fs = new FileStream(file, FileMode.Create))
                                {
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.WriteLine(userJsonStr);
                                    sw.Close();
                                }
                            }
                        }
                        DirRefresh();
                        FilesRefresh();
                    }
                }
            }
            catch (FormatException ex)
            {
                Debug.WriteLine(ex.Message);
                NSAlert alert2 = new NSAlert();
                alert2.MessageText = "フォーマットが不正です。\n" + ex.Message;
                alert2.RunModal();
            }
            catch (CsvHelper.MissingFieldException ex)
            {
                Debug.WriteLine(ex.Message);
                NSAlert alert2 = new NSAlert();
                alert2.MessageText = "フォーマットが不正です。\n" + ex.Message;
                alert2.RunModal();
            }
            catch (CsvHelper.ParserException ex)
            {
                Debug.WriteLine(ex.Message);
                NSAlert alert2 = new NSAlert();
                alert2.MessageText = "フォーマットが不正です。\n" + ex.Message;
                alert2.RunModal();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                NSAlert alert2 = new NSAlert();
                alert2.MessageText = "エラーが発生しました。\n" + ex.Message;
                alert2.RunModal();
            }
        }
    }
}
