using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using microcosmMac2.Views.DataSources;
using System.IO;
using microcosmMac2.Common;
using static System.Net.WebRequestMethods;
using microcosmMac2.User;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using microcosmMac2.Config;
using System.Text.Unicode;
using System.Diagnostics;
using static CoreFoundation.DispatchSource;

namespace microcosmMac2.Views
{
    public partial class UserAddViewController : AppKit.NSViewController
    {
        AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;

        public List<AddrCsv> addrs = new List<AddrCsv>();
        #region Constructors

        // Called when created from unmanaged code
        public UserAddViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public UserAddViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public UserAddViewController() : base("UserAddView", NSBundle.MainBundle)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion

        /// <summary>
        /// 新規データなのでJson読み込みは無い
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var root = Util.root;
            string[] items = CommonData.GetTimeTables();

            event_timezone.RemoveAllItems();
            event_timezone.AddItems(items);
            event_timezone.SelectItem(appDelegate.config.defaultTimezoneStr);

            var DataSource = new AddrDataSource();

            string csvFileName = root + "/system/addr.csv";

            using (FileStream fs = new FileStream(csvFileName, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);

                using (var csv = new CsvHelper.CsvReader(sr, new CultureInfo("ja-JP", false)))
                {
                    var records = csv.GetRecords<AddrCsv>();

                    foreach (AddrCsv record in records)
                    {
                        DataSource.names.Add(new Addr() {
                            name = record.name,
                            lat = record.lat,
                            lng = record.lng
                        });
                        addrs.Add(new AddrCsv()
                        {
                            name = record.name,
                            lat = record.lat,
                            lng = record.lng
                        });
                    }
                }

            }

            file_name.StringValue = "" +
                DateTime.Now.Year.ToString("00") +
                DateTime.Now.Month.ToString("00") +
                DateTime.Now.Day.ToString("00") +
                DateTime.Now.Hour.ToString("00") +
                DateTime.Now.Minute.ToString("00") +
                DateTime.Now.Second.ToString("00");

            event_name.StringValue = "ユーザー名";
            event_hour.StringValue = "12";
            event_minute.StringValue = "0";
            event_second.StringValue = "0";
            event_place.StringValue = appDelegate.config.defaultPlace;
            event_lat.StringValue = appDelegate.config.lat.ToString();
            event_lng.StringValue = appDelegate.config.lng.ToString();
            int index = 0;
            foreach (NSMenuItem item in event_timezone.Items())
            {
                if (item.Title == "Asia/Tokyo (UTC+09:00)")
                {
                    event_timezone.SelectItem(index);
                }
                index++;
            }


            LatLngTable.DataSource = DataSource;
            LatLngTable.Delegate = new AddrDelegate(DataSource);
            LatLngTable.SelectRow(0, false);
        }

        //strongly typed view accessor
        public new UserAddView View
        {
            get
            {
                return (UserAddView)base.View;
            }
        }

        partial void SubmitClick(Foundation.NSObject sender)
        {
            if (file_name.StringValue.IndexOf("/") > 0)
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "そのファイル名は登録できません。";
                alert.RunModal();
                return;
            }

            int birth_hour = 0;
            if (!Int32.TryParse(event_hour.StringValue, out birth_hour))
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "時刻は数値で指定してください。";
                alert.RunModal();
                return;
            }
            int birth_minute = 0;
            if (!Int32.TryParse(event_minute.StringValue, out birth_minute))
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "時刻は数値で指定してください。";
                alert.RunModal();
                return;
            }
            int birth_second = 0;
            if (!Int32.TryParse(event_second.StringValue, out birth_second))
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "時刻は数値で指定してください。";
                alert.RunModal();
                return;
            }
            double lat = 0;
            if (!Double.TryParse(event_lat.StringValue, out lat))
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "緯度経度は数値で指定してください。";
                alert.RunModal();
                return;
            }
            double lng = 0;
            if (!Double.TryParse(event_lng.StringValue, out lng))
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "緯度経度は数値で指定してください。";
                alert.RunModal();
                return;
            }


            DateTime d = DateTime.Parse(event_date.DateValue.ToString());

            UserJson udata = new UserJson()
            {
                name = event_name.StringValue,
                birth_year = d.Year,
                birth_month = d.Month,
                birth_day = d.Day,
                birth_hour = birth_hour,
                birth_minute = birth_minute,
                birth_second = birth_second,
                birth_timezone = CommonData.GetTimezoneValue(event_timezone.SelectedItem.Title),
                birth_timezone_str = event_timezone.SelectedItem.Title,
                birth_place = event_place.StringValue,
                lat = lat,
                lng = lng,
                memo = event_memo.String
            };
            UserJsonList userJsonList = new UserJsonList();
            userJsonList.list = new List<UserJson>();
            userJsonList.list.Add(udata);

            // old:ディレクトリ+ファイル作成
            // new:ファイル作成
            string fileName = appDelegate.dbSavedDirFullPath + "/" + file_name.StringValue + ".json";
            string userJson = JsonSerializer.Serialize(userJsonList,
            new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            });
            try
            {
                //すでにあったら上書きされるはず
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(userJson);
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                NSAlert alert = new NSAlert();
                alert.MessageText = "エラーが発生しました。";
                alert.RunModal();

                return;
            }

            appDelegate.dbViewController.FilesRefresh(appDelegate.dbSavedDirFullPath);
            DismissController(this);
        }

        partial void LatLngTableClicked(Foundation.NSObject sender)
        {
            NSTableView s = (NSTableView)sender;
            Debug.WriteLine(s.ClickedRow);
            if (-1 == s.ClickedRow) return;

            event_place.StringValue = addrs[(int)s.ClickedRow].name;
            event_lat.StringValue = addrs[(int)s.ClickedRow].lat.ToString();
            event_lng.StringValue = addrs[(int)s.ClickedRow].lng.ToString();
        }
    }
}
