using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using System.IO;
using microcosmMac2.User;
using microcosmMac2.Views.DataSources;
using System.Text.Encodings.Web;
using System.Text.Json;
using microcosmMac2.Calc;
using microcosmMac2.Common;
using System.Text.Unicode;

namespace microcosmMac2.Views
{
    public partial class EclipseViewController : AppKit.NSViewController
    {
        public string savedFile = "";
        public int savedEventIndex = 0;

        public int event_year = 2000;
        public int event_month = 1;
        public int event_day = 1;
        public int event_hour = 12;
        public int event_minute = 0;
        public int event_second = 0;
        public double timezone = 9.0;

        public int planetId = 0;

        public AstroCalc calc;

        #region Constructors

        // Called when created from unmanaged code
        public EclipseViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public EclipseViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public EclipseViewController() : base("EclipseView", NSBundle.MainBundle)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion

        //strongly typed view accessor
        public new EclipseView View
        {
            get
            {
                return (EclipseView)base.View;
            }
        }

        public override void ViewDidLoad()
        {
            // 精度の問題で一旦太陽月だけ
            string[] items = new string[2];
            items[0] = "sun";
            items[1] = "moon";
            //items[2] = "mercury";
            //items[3] = "venus";
            //items[4] = "mars";
            //items[5] = "jupiter";
            //items[6] = "saturn";
            planetList.RemoveAllItems();
            planetList.AddItems(items);

            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            savedFile = appDelegate.dbSavedFile;
            savedEventIndex = appDelegate.dbSavedEventIndex;

            using (FileStream fs = new FileStream(savedFile, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);

                string jsonData = sr.ReadToEnd();
                var option = new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, };
                UserJsonList jsonObj = JsonSerializer.Deserialize<UserJsonList>(jsonData, option);

                event_year = jsonObj.list[appDelegate.dbSavedEventIndex].birth_year;
                event_month = jsonObj.list[appDelegate.dbSavedEventIndex].birth_month;
                event_day = jsonObj.list[appDelegate.dbSavedEventIndex].birth_day;
                event_hour = jsonObj.list[appDelegate.dbSavedEventIndex].birth_hour;
                event_minute = jsonObj.list[appDelegate.dbSavedEventIndex].birth_minute;
                event_second = jsonObj.list[appDelegate.dbSavedEventIndex].birth_second;
                timezone = jsonObj.list[appDelegate.dbSavedEventIndex].birth_timezone;

                DateTime dateTime = new DateTime(event_year, event_month, event_day, event_hour, event_minute, event_second);

                calc = new AstroCalc(appDelegate.config);
                targetDegree.StringValue = calc.PositionCalcSingle(dateTime, timezone, planetId).ToString("0.00");
                DateTime now = DateTime.Now;
                targetDate.StringValue = String.Format("{0}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            }
        }

        partial void planetListSelectionChanged(Foundation.NSObject sender)
        {
            NSPopUpButton button = (NSPopUpButton)sender;
            targetDegree.StringValue = button.SelectedItem.Title;
            switch (button.SelectedItem.Title)
            {
                case "sun":

                    planetId = CommonData.ZODIAC_SUN;
                    break;
                case "moon":

                    planetId = CommonData.ZODIAC_MOON;
                    break;
                case "mercury":

                    planetId = CommonData.ZODIAC_MERCURY;
                    break;
                case "venus":

                    planetId = CommonData.ZODIAC_VENUS;
                    break;
                case "mars":

                    planetId = CommonData.ZODIAC_MARS;
                    break;
                case "jupiter":

                    planetId = CommonData.ZODIAC_JUPITER;
                    break;
                case "saturn":

                    planetId = CommonData.ZODIAC_SATURN;
                    break;

            }

            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            DateTime dateTime = new DateTime(event_year, event_month, event_day, event_hour, event_minute, event_second);
            targetDegree.StringValue = calc.PositionCalcSingle(dateTime, timezone, planetId).ToString("0.00");
            targetDate.StringValue = String.Format("{0}/{1}/{2} {3}:{4}:{5}", event_year, event_month, event_day, event_hour, event_minute, event_second);

        }

        partial void SubmitClicked(Foundation.NSObject sender)
        {
            bool isFuture = pastRadio.State == NSCellStateValue.On ? true : false;
            DateTime dateTime = DateTime.Now;
            EclipseCalc eclipse = calc.GetEclipseInstance();
            DateTime result  = eclipse.GetEclipse(dateTime, timezone, planetId, Double.Parse(targetDegree.StringValue), isFuture);

            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;

            string planetName = "太陽";
            switch (planetList.SelectedItem.Title)
            {
                case "sun":

                    planetName = "太陽";
                    break;
                case "moon":

                    planetName = "月";
                    break;
                    /*
                case "mercury":

                    planetName = "水星";
                    fileName = "mercuryReturn";
                    break;
                case "venus":

                    planetName = "金星";
                    fileName = "venusReturn";
                    break;
                case "mars":

                    planetName = "火星";
                    fileName = "marsReturn";
                    break;
                case "jupiter":

                    planetName = "木星";
                    fileName = "jupiterReturn";
                    break;
                case "saturn":

                    planetName = "土星";
                    fileName = "saturnReturn";
                    break;
                    */

            }

            // natalのjsonを読んで、そこのイベントに追記
            UserJsonList jsonObj;
            using (FileStream fs = new FileStream(savedFile, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);

                string jsonData = sr.ReadToEnd();
                var option = new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, };
                jsonObj = JsonSerializer.Deserialize<UserJsonList>(jsonData, option);

                UserJson udata = new UserJson()
                {
                    name = planetName + "回帰",
                    birth_year = result.Year,
                    birth_month = result.Month,
                    birth_day = result.Day,
                    birth_hour = result.Hour,
                    birth_minute = result.Minute,
                    birth_second = result.Second,
                    birth_timezone = jsonObj.list[savedEventIndex].birth_timezone,
                    birth_timezone_str = jsonObj.list[savedEventIndex].birth_timezone_str,
                    birth_place = jsonObj.list[savedEventIndex].birth_place,
                    lat = jsonObj.list[savedEventIndex].lat,
                    lng = jsonObj.list[savedEventIndex].lng,
                    memo = ""
                };

                jsonObj.list.Add(udata);
            }
            // 一旦FileStreamを閉じないと書けないはず

            string userJson = JsonSerializer.Serialize(jsonObj,
            new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            });
            using (FileStream fs2 = new FileStream(savedFile, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs2);
                sw.WriteLine(userJson);
                sw.Close();
            }


            appDelegate.dbViewController.EventRefresh();


            DismissController(this);
        }

        partial void futureRadioClick(Foundation.NSObject sender)
        {
            pastRadio.State = NSCellStateValue.Off;
        }

        partial void pastRadioClick(Foundation.NSObject sender)
        {
            futureRadio.State = NSCellStateValue.Off;
        }
    }
}
