using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AppKit;
using CoreGraphics;
using Foundation;
using microcosmMac2.Calc;
using microcosmMac2.Common;
using microcosmMac2.Config;
using microcosmMac2.Models;
using microcosmMac2.User;
using microcosmMac2.Views;
using SkiaSharp;
using SkiaSharp.Views.Mac;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Drawing;
using microcosmMac2.Views.DataSources;
using static System.Net.Mime.MediaTypeNames;
using IOSurface;
using EventKit;
using System.Runtime.Serialization.Formatters;
using System.Globalization;
using Intents;
using ImageIO;
using Image = System.Drawing.Image;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;

namespace microcosmMac2
{
	public partial class ViewController : NSViewController
	{
        NSObject NSWindowDidResizeNotificationObject;
        NSObject NSWindowDidChangeScreenNotificationObject;
        AppDelegate appDelegate;

        public int a = 0;

        public float canvasWidth = 0;
        public float canvasHeight = 0;

        // 中心円
        public float centerDiameterBase = 160;
        // 外側の直径
        public float diameter = 290;
        public float zodiacWidth = 60;


        public ConfigData configData;
        public SettingData[] settings;
        public int settingIndex = 0;

        public UserData udata1 = new UserData();
        public UserData udata2 = new UserData();
        public UserData edata1 = new UserData();
        public UserData edata2 = new UserData();

        public Dictionary<int, PlanetData> list1;
        public Dictionary<int, PlanetData> list2;
        public Dictionary<int, PlanetData> list3;
        public double[] houseList1;
        public double[] houseList2;
        public double[] houseList3;
        public ETargetUser[] calcTargetUser = {ETargetUser.EVENT1, ETargetUser.EVENT1, ETargetUser.EVENT1};

        public AstroCalc calc;

        public int plusUnit = 86400;
        public double skiaScale = 2;

        SKSurface currentSurface;
        List<Sabian> sabians;
        ShortCut shortCut;

        public List<MouseIn> mouseInList;

        public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            // Do any additional setup after loading the view.

            appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            appDelegate.viewController = this;

            NSWindowDidResizeNotificationObject = NSNotificationCenter.DefaultCenter.AddObserver(new NSString("NSWindowDidResizeNotification"), ResizeObserver, null);
            NSWindowDidChangeScreenNotificationObject = NSNotificationCenter.DefaultCenter.AddObserver(new NSString("NSWindowDidChangeScreenNotification"), WindowMoveObserver, null);
            NSNotificationCenter.DefaultCenter.AddObserver(new NSString("NSWindowDidChangeScreenProfileNotification"), WindowMoveObserver, null);

            NSEvent.AddLocalMonitorForEventsMatchingMask(NSEventMask.KeyDown, (NSEvent e) =>
            {
                var controller = NSApplication.SharedApplication.KeyWindow.ContentViewController;
                if (controller == null)
                {
                    return e;
                }
                string className = controller.GetType().Name;
                if (className == "ViewController")
                {
                    KeyDown(e);
                    return null;
                }
                else
                {
                    return e;
                }
            });

            MainInit();
            ReRender();

        }

        public override void ViewDidAppear()
        {
            base.ViewDidAppear();
            /*
            bool x = BecomeFirstResponder();
            NSResponder y = NSApplication.SharedApplication.KeyWindow.FirstResponder;
            var z = NSApplication.SharedApplication.KeyWindow.ContentViewController;
            */
        }

        public override NSObject RepresentedObject {
			get {
				return base.RepresentedObject;
			}
			set {
				base.RepresentedObject = value;
				// Update the view, if already loaded.
			}
        }

        public void ResizeObserver(NSNotification notify)
        {
            var r = this.View.Frame;
            //Debug.WriteLine("{0}:{1}:{2}", notify.Name, r.Height, r.Width);
        }

        public void WindowMoveObserver(NSNotification notify)
        {
            NSWindow window = (NSWindow)notify.Object;
            Debug.WriteLine("Window moved. {0}", window.BackingScaleFactor);
            skiaScale = window.BackingScaleFactor;
            ReRender();
        }

        public void MainInit()
        {
            var root = Util.root;
            var bundle = Path.Combine(NSBundle.MainBundle.BundlePath, "Contents", "Resources", "system");
            var epheBundle = Path.Combine(NSBundle.MainBundle.BundlePath, "Contents", "Resources", "ephe");
            var lisenceBundle = Path.Combine(NSBundle.MainBundle.BundlePath, "Contents", "Resources", "license");

            if (!Directory.Exists(root + "/ephe"))
            {
                Directory.CreateDirectory(root + "/ephe");
            }

            if (!Directory.Exists(root + "/system"))
            {
                Directory.CreateDirectory(root + "/system");
            }

            if (!Directory.Exists(root + "/data"))
            {
                Directory.CreateDirectory(root + "/data");
            }

            if (!Directory.Exists(root + "/license"))
            {
                Directory.CreateDirectory(root + "/license");
            }

            settings = new SettingData[10];
            for (int i = 0; i < 10; i++)
            {
                string settingFileName = root + String.Format("/system/settings{0}.json", i);

                //settings[i] = SettingFromXml.GetSettingFromXml(root + "/system/setting" + i.ToString() + ".csm", i);
                try
                {
                    using (FileStream fs = new FileStream(settingFileName, FileMode.Open))
                    {
                        StreamReader sr = new StreamReader(fs);
                        string jsonData = sr.ReadToEnd();
                        var option = new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, };
                        SettingJson jsonObj = JsonSerializer.Deserialize<SettingJson>(jsonData, option);
                        settings[i] = new SettingData(jsonObj);
                    }
                }
                catch
                {
                    Debug.WriteLine("settingファイル生成");
                    settings[i] = new SettingData(i);
                    SettingJson json = new SettingJson(settings[i]);

                     string settingJson = JsonSerializer.Serialize(json,
                        new JsonSerializerOptions {
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                            WriteIndented = true
                        }
                    );
                    using (FileStream fs = new FileStream(settingFileName, FileMode.Create))
                    {
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine(settingJson);
                        sw.Close();
                    }
                }
            }
            appDelegate.settings = settings;

            string filename = root + "/system/config.json";
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    StreamReader sr = new StreamReader(fs);
                    string jsonData = sr.ReadToEnd();
                    configData = JsonSerializer.Deserialize<ConfigData>(jsonData);
                }
            }
            catch
            {
                Debug.WriteLine("configファイル生成");
                configData = new ConfigData(root + @"/ephe");

                string configJson = JsonSerializer.Serialize(configData,
                    new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                        WriteIndented = true
                    });
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(configJson);
                    sw.Close();
                }
            }

            appDelegate.config = configData;

            if (!File.Exists(root + "/ephe/semo_18.se1"))
            {
                File.Copy(epheBundle + "/semo_18.se1", root + "/ephe/semo_18.se1");
            }

            if (!File.Exists(root + "/ephe/sepl_18.se1"))
            {
                File.Copy(epheBundle + "/sepl_18.se1", root + "/ephe/sepl_18.se1");
            }

            if (!File.Exists(root + "/ephe/seas_18.se1"))
            {
                File.Copy(epheBundle + "/seas_18.se1", root + "/ephe/seas_18.se1");
            }

            if (!File.Exists(root + "/ephe/seleapsec.txt"))
            {
                File.Copy(epheBundle + "/seleapsec.txt", root + "/ephe/seleapsec.txt");
            }

            if (!File.Exists(root + "/ephe/s136108s.se1"))
            {
                File.Copy(epheBundle + "/s136108s.se1", root + "/ephe/s136108s.se1");
            }

            if (!File.Exists(root + "/ephe/s136199s.se1"))
            {
                File.Copy(epheBundle + "/s136199s.se1", root + "/ephe/s136199s.se1");
            }

            if (!File.Exists(root + "/ephe/s136472s.se1"))
            {
                File.Copy(epheBundle + "/s136472s.se1", root + "/ephe/s136472s.se1");
            }

            if (!File.Exists(root + "/ephe/se90377s.se1"))
            {
                File.Copy(epheBundle + "/se90377s.se1", root + "/ephe/se90377s.se1");
            }

            if (!File.Exists(root + "/ephe/sedeltat.txt"))
            {
                File.Copy(epheBundle + "/sedeltat.txt", root + "/ephe/sedeltat.txt");
            }

            if (!File.Exists(root + "/ephe/swe_deltat.txt"))
            {
                File.Copy(epheBundle + "/swe_deltat.txt", root + "/ephe/swe_deltat.txt");
            }

            /*
             * 差し替えしやすいようにinternal(.app埋め込み)のフォントを使う
            if (!File.Exists(root + "/system/microcosm.otf"))
            {
                File.Copy(bundle + "/microcosm.otf", root + "/system/microcosm.otf");
            }

            if (!File.Exists(root + "/system/microcosm-aspects.otf"))
            {
                File.Copy(bundle + "/microcosm-aspects.otf", root + "/system/microcosm-aspects.otf");
            }
            */

            if (!File.Exists(root + "/system/addr.csv"))
            {
                File.Copy(bundle + "/addr.csv", root + "/system/addr.csv");
            }

            if (!File.Exists(root + "/system/sabian.csv"))
            {
                File.Copy(bundle + "/sabian.csv", root + "/system/sabian.csv");
            }

            sabians = new List<Sabian>();
            using (FileStream fsSabian = new FileStream(root + "/system/sabian.csv", FileMode.Open))
            {
                StreamReader srSabian = new StreamReader(fsSabian);

                using (var csv = new CsvHelper.CsvReader(srSabian, new CultureInfo("ja-JP", false)))
                {
                    var records = csv.GetRecords<Sabian>();

                    foreach (Sabian record in records)
                    {
                        sabians.Add(new Sabian() { degree = record.degree, text = record.text });
                    }
                }

            }

            if (!File.Exists(root + "/license/csvhelper.txt"))
            {
                File.Copy(lisenceBundle + "/csvhelper.txt", root + "/license/csvhelper.txt");
            }

            if (!File.Exists(root + "/license/license.txt"))
            {
                File.Copy(lisenceBundle + "/license.txt", root + "/license/license.txt");
            }

            if (!File.Exists(root + "/license/agpl-3.0_ja.txt"))
            {
                File.Copy(lisenceBundle + "/agpl-3.0_ja.txt", root + "/license/agpl-3.0_ja.txt");
            }

            if (!File.Exists(root + "/license/agpl-3.0.txt"))
            {
                File.Copy(lisenceBundle + "/agpl-3.0.txt", root + "/license/agpl-3.0.txt");
            }

            string[] files = Directory.GetFiles(root + "/data");

            appDelegate.currentSetting = settings[0];
            appDelegate.settingIndex = 0;
            appDelegate.tempSetting = new TempSetting();

            CommonInstance.getInstance().config = configData;
            CommonInstance.getInstance().settings = settings;
            CommonInstance.getInstance().currentSetting = settings[0];
            CommonInstance.getInstance().currentSettingIndex = 0;

            NSScreen screen = NSScreen.MainScreen;
            skiaScale = (float)screen.BackingScaleFactor;


            string shortcutFile = root + "/system/shortcut.json";
            shortCut = null;
            try
            {
                using (FileStream shorcutFs = new FileStream(shortcutFile, FileMode.Open))
                {
                    StreamReader shortcutSr = new StreamReader(shorcutFs);
                    string jsonData = shortcutSr.ReadToEnd();
                    shortCut = JsonSerializer.Deserialize<ShortCut>(jsonData);
                }
            }
            catch
            {
                Debug.WriteLine("shortcutファイル生成");
                shortCut = new ShortCut();

                string shortcutStr = JsonSerializer.Serialize(shortCut,
                    new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                        WriteIndented = true
                    });
                using (FileStream fs = new FileStream(shortcutFile, FileMode.Create))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(shortcutStr);
                    sw.Close();
                }
            }

            appDelegate.shortCut = shortCut;

            // KeyDownで拾い、EventKeyCodeからDelegateへ流れる
            appDelegate.keyEvent = new Dictionary<int, EShortCut>();
            appDelegate.keyEventCtrl = new Dictionary<int, EShortCut>();
            appDelegate.keyEventCtrl.Add(4, shortCut.ctrlH);
            appDelegate.keyEventCtrl.Add(38, shortCut.ctrlJ);
            appDelegate.keyEventCtrl.Add(40, shortCut.ctrlK);
            appDelegate.keyEventCtrl.Add(37, shortCut.ctrlL);
            appDelegate.keyEventCtrl.Add(18, shortCut.ctrl1);
            appDelegate.keyEventCtrl.Add(19, shortCut.ctrl2);
            appDelegate.keyEventCtrl.Add(20, shortCut.ctrl3);
            appDelegate.keyEventCtrl.Add(21, shortCut.ctrl4);
            appDelegate.keyEventCtrl.Add(23, shortCut.ctrl5);
            appDelegate.keyEventCtrl.Add(22, shortCut.ctrl6);
            appDelegate.keyEventCtrl.Add(26, shortCut.ctrl7);
            appDelegate.keyEventCtrl.Add(28, shortCut.ctrl8);
            appDelegate.keyEventCtrl.Add(25, shortCut.ctrl9);
            appDelegate.keyEventCtrl.Add(29, shortCut.ctrl0);
            appDelegate.keyEventCtrl.Add(45, shortCut.ctrlN);
            appDelegate.keyEventCtrl.Add(46, shortCut.ctrlM);
            appDelegate.keyEventCtrl.Add(43, shortCut.ctrlComma);
            appDelegate.keyEventCtrl.Add(47, shortCut.ctrlDot);
            appDelegate.keyEventCtrl.Add(33, shortCut.ctrlOpenBracket);
            appDelegate.keyEventCtrl.Add(30, shortCut.ctrlCloseBracket);
            appDelegate.keyEvent.Add(122, shortCut.F1);
            appDelegate.keyEvent.Add(120, shortCut.F2);
            appDelegate.keyEvent.Add(99, shortCut.F3);
            appDelegate.keyEvent.Add(118, shortCut.F4);
            appDelegate.keyEvent.Add(96, shortCut.F5);
            appDelegate.keyEvent.Add(97, shortCut.F6);
            appDelegate.keyEvent.Add(98, shortCut.F7);
            appDelegate.keyEvent.Add(100, shortCut.F8);
            appDelegate.keyEvent.Add(101, shortCut.F9);
            appDelegate.keyEvent.Add(109, shortCut.F10);

            // ファイル系設定ここまで

            mouseInList = new List<MouseIn>();


            // calc

            calc = new AstroCalc(configData);

            ReCalc();
            BoxInit();

            appDelegate.udata1 = udata1;
            appDelegate.udata2 = udata2;
            appDelegate.edata1 = edata1;
            appDelegate.edata2 = edata2;


            //timesetter
            settingPopupButton.RemoveAllItems();
            foreach (var setting in settings)
            {
                settingPopupButton.AddItem(setting.dispName);
            }

            timesetterChangeButton.RemoveAllItems();
            timesetterChangeButton.AddItem("User1");
            timesetterChangeButton.AddItem("User2");
            timesetterChangeButton.AddItem("Event1");
            timesetterChangeButton.AddItem("Event2");
            timesetterChangeButton.SelectItem("Event1");

            //左下
            RefreshSettingBox();

            UserTabBox.SelectAt((nint)2);

            //myLabel.StringValue = "Hello";
            //var r = this.View.Frame;
            //Debug.WriteLine("{0}:{1}", r.Height, r.Width);


        }

        /// <summary>
        /// 旧Verと合わせる
        /// </summary>
        public void ReCalc()
        {
            UserData ring1 = GetTargetUser(0);
            UserData ring2 = GetTargetUser(1);
            UserData ring3 = GetTargetUser(2);

            list1 = calc.ReCalc(configData, appDelegate.currentSetting, ring1);
            if (appDelegate.bands < 3)
            {
                list2 = calc.ReCalc(configData, appDelegate.currentSetting, ring2);
            }
            else
            {
                if (appDelegate.secondBand == AppDelegate.BandKind.PROGRESS)
                {
                    list2 = calc.ReCalcProgress(configData, appDelegate.currentSetting, list1, ring1, ring2.GetDateTime(), ring1.timezone);
                }
                else
                {
                    list2 = calc.ReCalc(configData, appDelegate.currentSetting, ring2);
                }
            }
            if (appDelegate.thirdBand == AppDelegate.BandKind.COMPOSIT)
            {
                list3 = calc.PositionCalcComposit(list1, list2);
            }
            else
            {
                list3 = calc.ReCalc(configData, appDelegate.currentSetting, ring3);
            }
            houseList1 = calc.CuspCalc(ring1.GetDateTime(), ring1.timezone, ring1.lat, ring1.lng, configData.houseCalc);
            if (configData.sidereal == Esidereal.DRACONIC)
            {
                if (configData.nodeCalc == ENodeCalc.TRUE)
                {
                    houseList1.Select(h =>
                    {
                        h -= list1[CommonData.ZODIAC_DH_TRUENODE].absolute_position;
                        if (h < 0) h += 360;
                        return h;
                    });
                }
                else
                {
                    houseList1.Select(h =>
                    {
                        h -= list1[CommonData.ZODIAC_DH_MEANNODE].absolute_position;
                        if (h < 0) h += 360;
                        return h;
                    });
                }
            }
            list1[CommonData.ZODIAC_ASC] = new PlanetData()
            {
                no = CommonData.ZODIAC_ASC,
                absolute_position = houseList1[1],
                isDisp = appDelegate.currentSetting.dispPlanetAsc == 1,
                isAspectDisp = appDelegate.currentSetting.dispAspectPlanetAsc == 1,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = true
            };
            list1[CommonData.ZODIAC_MC] = new PlanetData()
            {
                no = CommonData.ZODIAC_MC,
                absolute_position = houseList1[10],
                isDisp = appDelegate.currentSetting.dispPlanetMc == 1,
                isAspectDisp = appDelegate.currentSetting.dispAspectPlanetMc == 1,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = true
            };

            if (appDelegate.bands == 3)
            {
                // 一旦はこれでいいけど、後々修正が必要かも
                houseList2 = calc.HouseCalcProgress(configData, appDelegate.currentSetting, houseList1, list1, ring1.GetDateTime(), ring2.GetDateTime(), ring1.lat, ring1.lng, ring1.timezone);
            }
            else
            {
                houseList2 = calc.CuspCalc(ring2.GetDateTime(), ring2.timezone, ring2.lat, ring2.lng, configData.houseCalc);
            }
            if (configData.sidereal == Esidereal.DRACONIC)
            {
                if (configData.nodeCalc == ENodeCalc.TRUE)
                {
                    houseList2.Select(h =>
                    {
                        h -= list2[CommonData.ZODIAC_DH_TRUENODE].absolute_position;
                        if (h < 0) h += 360;
                        return h;
                    });
                }
                else
                {
                    houseList2.Select(h =>
                    {
                        h -= list2[CommonData.ZODIAC_DH_MEANNODE].absolute_position;
                        if (h < 0) h += 360;
                        return h;
                    });
                }
            }
            list2[CommonData.ZODIAC_ASC] = new PlanetData()
            {
                no = CommonData.ZODIAC_ASC,
                absolute_position = houseList2[1],
                isDisp = appDelegate.currentSetting.dispPlanetAsc == 1,
                isAspectDisp = appDelegate.currentSetting.dispAspectPlanetAsc == 1,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = true
            };
            list2[CommonData.ZODIAC_MC] = new PlanetData()
            {
                no = CommonData.ZODIAC_MC,
                absolute_position = houseList2[10],
                isDisp = appDelegate.currentSetting.dispPlanetMc == 1,
                isAspectDisp = appDelegate.currentSetting.dispAspectPlanetMc == 1,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = true
            };


            houseList3 = calc.CuspCalc(ring3.GetDateTime(), ring3.timezone, ring3.lat, ring3.lng, configData.houseCalc);
            if (configData.sidereal == Esidereal.DRACONIC)
            {
                if (configData.nodeCalc == ENodeCalc.TRUE)
                {
                    houseList3.Select(h =>
                    {
                        h -= list3[CommonData.ZODIAC_DH_TRUENODE].absolute_position;
                        if (h < 0) h += 360;
                        return h;
                    });
                }
                else
                {
                    houseList3.Select(h =>
                    {
                        h -= list3[CommonData.ZODIAC_DH_MEANNODE].absolute_position;
                        if (h < 0) h += 360;
                        return h;
                    });
                }
            }

            list3[CommonData.ZODIAC_ASC] = new PlanetData()
            {
                no = CommonData.ZODIAC_ASC,
                absolute_position = houseList3[1],
                isDisp = appDelegate.currentSetting.dispPlanetAsc == 1,
                isAspectDisp = appDelegate.currentSetting.dispAspectPlanetAsc == 1,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = true
            };
            list3[CommonData.ZODIAC_MC] = new PlanetData()
            {
                no = CommonData.ZODIAC_MC,
                absolute_position = houseList3[10],
                isDisp = appDelegate.currentSetting.dispPlanetMc == 1,
                isAspectDisp = appDelegate.currentSetting.dispAspectPlanetMc == 1,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = true
            };

            AspectCalc aspect = new AspectCalc();
            list1 = aspect.AspectCalcSame(appDelegate.currentSetting, list1);
            list1 = aspect.AspectCalcOther(appDelegate.currentSetting, list1, list2, 3);
            list1 = aspect.AspectCalcOther(appDelegate.currentSetting, list1, list3, 4);
            list2 = aspect.AspectCalcSame(appDelegate.currentSetting, list2);
            list2 = aspect.AspectCalcOther(appDelegate.currentSetting, list2, list3, 5);
            list3 = aspect.AspectCalcSame(appDelegate.currentSetting, list3);

            // 左下サインリスト
            var DataSource = new SignTableDataSource();
            for (int i = 0; i < 10; i++)
            {
                string degree1;
                string degree2;
                string degree3;
                if (configData.decimalDisp == EDecimalDisp.DEGREE)
                {
                    degree1 = CommonData.getSignTextJp(list1[i].absolute_position);
                    degree2 = CommonData.getSignTextJp(list2[i].absolute_position);
                    degree3 = CommonData.getSignTextJp(list3[i].absolute_position);

                    degree1 += ((int)(list1[i].absolute_position % 30)).ToString() + "° " +
                        ((int)CommonData.DecimalToHex(list1[i].absolute_position % 1 * 100)).ToString() + "'";
                    degree2 += ((int)(list2[i].absolute_position % 30)).ToString() + "° " +
                        ((int)CommonData.DecimalToHex(list2[i].absolute_position % 1 * 100)).ToString() + "'";
                    degree3 += ((int)(list3[i].absolute_position % 30)).ToString() + "° " +
                        ((int)CommonData.DecimalToHex(list3[i].absolute_position % 1 * 100)).ToString() + "'";
                }
                else
                {
                    degree1 = list1[i].absolute_position.ToString();
                    degree2 = list2[i].absolute_position.ToString();
                    degree3 = list3[i].absolute_position.ToString();
                }
                DataSource.names.Add(new SignTableData()
                {
                    sign = CommonData.getPlanetSymbolText(i),
                    degree1 = degree1,
                    degree2 = degree2,
                    degree3 = degree3
                }) ;
            }
            signTable.DataSource = DataSource;
            signTable.Delegate = new SignTableDelegate(DataSource);

            // 左下ハウスリスト
            var HouseDataSource = new SignTableDataSource();
            for (int i = 1; i < 13; i++)
            {
                string degree1;
                string degree2;
                string degree3;
                if (configData.decimalDisp == EDecimalDisp.DEGREE)
                {
                    degree1 = CommonData.getSignTextJp(houseList1[i]);
                    degree2 = CommonData.getSignTextJp(houseList2[i]);
                    degree3 = CommonData.getSignTextJp(houseList3[i]);

                    degree1 += ((int)(houseList1[i] % 30)).ToString("00") + "° " +
                        ((int)CommonData.DecimalToHex(houseList1[i] % 1 * 100)).ToString() + "'";
                    degree2 += ((int)(houseList2[i] % 30)).ToString("00") + "° " +
                        ((int)CommonData.DecimalToHex(houseList2[i] % 1 * 100)).ToString() + "'";
                    degree3 += ((int)(houseList3[i] % 30)).ToString("00") + "° " +
                        ((int)CommonData.DecimalToHex(houseList3[i] % 1 * 100)).ToString() + "'";
                }
                else
                {
                    degree1 = houseList1[i].ToString();
                    degree2 = houseList2[i].ToString();
                    degree3 = houseList3[i].ToString();
                }
                HouseDataSource.names.Add(new SignTableData()
                {
                    sign = i.ToString(),
                    degree1 = degree1,
                    degree2 = degree2,
                    degree3 = degree3
                });
            }
            houseTable.DataSource = HouseDataSource;
            houseTable.Delegate = new SignTableDelegate(HouseDataSource);

        }

        public void ReRender()
        {
            mouseInList.Clear();
            // subviewを削除して再生成
            // 速度的に心配だけどこうするしか思いつかない
            if (horoscopeCanvas.Subviews.Length > 0)
            {
                horoscopeCanvas.Subviews[0].RemoveFromSuperview();
            }
            this.canvasWidth = (float)horoscopeCanvas.Frame.Width;
            this.canvasHeight = (float)horoscopeCanvas.Frame.Height;
            //this.canvasWidth = 500;
            //this.canvasHeight = 600;
            CanvasView sk = new CanvasView(new CGRect(0, 0, horoscopeCanvas.Frame.Width, horoscopeCanvas.Frame.Width));
            //Debug.WriteLine(horoscopeCanvas.Frame.Width);

            sk.PaintSurface += CanvasPaint;
            horoscopeCanvas.AddSubview(sk);

            SetReport();
            RefreshSettingBox();
        }

        /// <summary>
        /// box更新
        /// </summary>
        public void BoxInit()
        {
            nameLabel.StringValue = "現在時刻";
            datetimeLabel.StringValue = DateTime.Now.ToString("yyyy/MM/dd H:mm:dd");
            latlngLabel.StringValue = String.Format("{0} {1}", configData.lat, configData.lng);
            u1TimeZoneLabel.StringValue = configData.defaultTimezoneStr;
            u1Place.StringValue = configData.defaultPlace;

            u2NameLabel.StringValue = "現在時刻";
            u2DateTimeLabel.StringValue = DateTime.Now.ToString("yyyy/MM/dd H:mm:dd");
            u2LatLngLabel.StringValue = String.Format("{0} {1}", configData.lat, configData.lng);
            u2TimeZone.StringValue = configData.defaultTimezoneStr;
            u2Place.StringValue = configData.defaultPlace;

            eventNameLabel.StringValue = "現在時刻";
            eventTimeLabel.StringValue = DateTime.Now.ToString("yyyy/MM/dd H:mm:dd");
            eventLatLngLabel.StringValue = String.Format("{0} {1}", configData.lat, configData.lng);
            e1TimeZone.StringValue = configData.defaultTimezoneStr;
            e1Place.StringValue = configData.defaultPlace;

            e2NameLabel.StringValue = "現在時刻";
            e2DateTimeLabel.StringValue = DateTime.Now.ToString("yyyy/MM/dd H:mm:dd");
            e2LatLngLabel.StringValue = String.Format("{0} {1}", configData.lat, configData.lng);
            e2TimeZone.StringValue = configData.defaultTimezoneStr;
            e2Place.StringValue = configData.defaultPlace;
        }

        /// <summary>
        /// UserBox更新
        /// </summary>
        /// <param name="index"></param>
        /// <param name="u"></param>
        public void RefreshUserBox(int index, UserData u)
        {
            if (index == 0)
            {
                nameLabel.StringValue = u.name;
                datetimeLabel.StringValue = String.Format("{0}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}",
                    u.birth_year, u.birth_month, u.birth_day,
                    u.birth_hour, u.birth_minute, u.birth_second);
                latlngLabel.StringValue = String.Format("{0} {1}", u.lat, u.lng);
                u1TimeZoneLabel.StringValue = u.timezone_str;
                u1Place.StringValue = u.birth_place;
            }
            else
            {
                u2NameLabel.StringValue = u.name;
                u2DateTimeLabel.StringValue = String.Format("{0}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}",
                    u.birth_year, u.birth_month, u.birth_day,
                    u.birth_hour, u.birth_minute, u.birth_second);
                u2LatLngLabel.StringValue = String.Format("{0} {1}", u.lat, u.lng);
                u2TimeZone.StringValue = u.timezone_str;
                u2Place.StringValue = u.birth_place;
            }
        }

        /// <summary>
        /// EventBox更新
        /// </summary>
        /// <param name="index"></param>
        /// <param name="u"></param>
        public void RefreshEventBox(int index, UserData e)
        {
            if (index == 0)
            {
                eventNameLabel.StringValue = e.name;
                eventTimeLabel.StringValue = String.Format("{0}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}",
                    e.birth_year, e.birth_month, e.birth_day,
                    e.birth_hour, e.birth_minute, e.birth_second);
                eventLatLngLabel.StringValue = String.Format("{0} {1}", e.lat, e.lng);
                e1TimeZone.StringValue = e.timezone_str;
                e1Place.StringValue = e.birth_place;

            }
            else
            {
                e2NameLabel.StringValue = e.name;
                e2DateTimeLabel.StringValue = String.Format("{0}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}",
                    e.birth_year, e.birth_month, e.birth_day,
                    e.birth_hour, e.birth_minute, e.birth_second);
                e2LatLngLabel.StringValue = String.Format("{0} {1}", e.lat, e.lng);
                e2TimeZone.StringValue = e.timezone_str;
                e2Place.StringValue = e.birth_place;
            }
        }

        public void RefreshSettingPopupButton()
        {
            settingPopupButton.RemoveAllItems();
            foreach (var setting in settings)
            {
                settingPopupButton.AddItem(setting.dispName);
            }
        }

        public void RefreshSettingBox()
        {
            List<string> settingFieldList = new List<string>();
            settingField.StringValue = "";
            settingFieldList.Add("ring1:" + calcTargetUser[0].ToString());
            if (appDelegate.bands > 1)
            {
                if (appDelegate.secondBand == AppDelegate.BandKind.PROGRESS)
                {
                    settingFieldList.Add("ring2:PROGRESS");
                }
                else
                {
                    settingFieldList.Add("ring2:" + calcTargetUser[1].ToString());
                }
                if (appDelegate.bands > 2)
                {
                    if (appDelegate.secondBand == AppDelegate.BandKind.COMPOSIT)
                    {
                        settingFieldList.Add("ring3:COMPOSIT");
                    }
                    else
                    {
                        settingFieldList.Add("ring3:" + calcTargetUser[2].ToString());
                    }
                }
            }
            settingFieldList.Add(configData.progression.ToString());
            settingFieldList.Add(CommonData.HouseCalcToString(configData.houseCalc));
            settingFieldList.Add(configData.centric.ToString());
            settingFieldList.Add(configData.sidereal.ToString());

            foreach (string s in settingFieldList)
            {
                settingField.StringValue += s + "\n";
            }
        }

        /// <summary>
        /// 描画部分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CanvasPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            float CenterX = this.canvasWidth / 2;
            float CenterY = this.canvasWidth / 2;
            // 若干マージンを取る
            CenterY += 10;
            float diameter = this.canvasWidth - 60;
            // 獣帯の幅
            float zodiacWidth = 30;


            string[] signs = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l" };
            string[] planets = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "O", "L" };

//            var surface = e.Surface;
//            var surfaceWidth = e.Info.Width;
//            var surfaceHeight = e.Info.Height;
            SKCanvas cvs = e.Surface.Canvas;
            currentSurface = e.Surface;
            cvs.Clear();
            cvs.Scale((float)skiaScale);

            SKRect bg = new SKRect(0, 0, CenterX * 2, CenterY * 2);
            SKPaint bgStyle = new SKPaint();
            bgStyle.Color = new SKColor(255, 255, 255, 255);
            cvs.DrawRect(bg, bgStyle);

            //            cvs.Translate(50, 50);
            SKPaint lineStyle = new SKPaint();
            lineStyle.Style = SKPaintStyle.Stroke;

            SKPaint p = new SKPaint();
            p.Style = SKPaintStyle.Fill;


            // outer
            cvs.DrawCircle(CenterX, CenterY, diameter/2, lineStyle);
            // inner
            cvs.DrawCircle(CenterX, CenterY, diameter/2 - zodiacWidth, lineStyle);

            //cvs.DrawLine(new SKPoint(CenterX, 0), new SKPoint(CenterX, this.canvasWidth), p);
            //cvs.DrawLine(new SKPoint(0, CenterY), new SKPoint(this.canvasWidth, CenterY), p);

            double offset = 0;
            float centerDiameter = centerDiameterBase;
            // ring描画
            if (appDelegate.bands == 1)
            {
                if (configData.dispPattern2 == EDispPettern.MINI)
                {
                    // 中心円を広げる
                    centerDiameter = centerDiameterBase + 50;
                }
                // center
                cvs.DrawCircle(CenterX, CenterY, centerDiameter, lineStyle);
            }
            else if (appDelegate.bands == 2)
            {
                // center
                // 中心円を少し縮める
                centerDiameter = centerDiameterBase - 20;
                cvs.DrawCircle(CenterX, CenterY, centerDiameter, lineStyle);

                // middle
                offset = (diameter/2 - zodiacWidth - centerDiameter) / 2;
                cvs.DrawCircle(CenterX, CenterY, (float)(centerDiameter + offset), lineStyle);
            }
            else if (appDelegate.bands == 3)
            {
                // center
                // 中心円を少し縮める
                centerDiameter = centerDiameterBase - 40;
                cvs.DrawCircle(CenterX, CenterY, centerDiameter, lineStyle);

                offset = (diameter/2 - zodiacWidth - centerDiameter) / 3;
                cvs.DrawCircle(CenterX, CenterY, (float)(centerDiameter + offset), lineStyle);
                cvs.DrawCircle(CenterX, CenterY, (float)(centerDiameter + offset * 2), lineStyle);
            }
            else if (appDelegate.bands == 4)
            {
                // center
                // 中心円を少し縮める
                centerDiameter = centerDiameterBase - 80;
                cvs.DrawCircle(CenterX, CenterY, centerDiameter, lineStyle);

                offset = (diameter - zodiacWidth - centerDiameter) / 4;
                cvs.DrawCircle(CenterX, CenterY, (float)(centerDiameter + offset), lineStyle);
                cvs.DrawCircle(CenterX, CenterY, (float)(centerDiameter + offset * 2), lineStyle);
                cvs.DrawCircle(CenterX, CenterY, (float)(centerDiameter + offset * 3), lineStyle);
            }

            // house cusps
            // 外側
            Position housePt;
            // 内側
            Position housePtEnd;
            // テキスト
            Position housePtTxt;
            // 軸
            SKPaint lineStyle1 = new SKPaint();
            lineStyle1.StrokeWidth = 2.5F;
            SKColor xxx = SKColors.LightGray;
            lineStyle1.Color = xxx;
            // それ以外
            SKPaint lineStyle2 = new SKPaint();
            lineStyle2.PathEffect = SKPathEffect.CreateDash(new[] { 5F, 2F }, 1.0F);
            lineStyle2.StrokeWidth = 1.5F;
            SKColor yyy = SKColors.LightGray;
            lineStyle2.Color = yyy;
            SKPaint textStyle = new SKPaint();
            textStyle.StrokeWidth = 1.5F;
            SKColor zzz = SKColors.SlateGray;
            textStyle.Color = zzz;

            // house cusps
            for (int i = 1; i <= 12; i++)
            {
                if (appDelegate.bands == 1)
                {
                    housePt = Util.Rotate(diameter / 2 - zodiacWidth, 0, houseList1[i] - houseList1[1]);
                    housePt.x = housePt.x + CenterX;
                    housePt.y = -1 * housePt.y + CenterY;
                }
                else if (appDelegate.bands == 2)
                {
                    housePt = Util.Rotate(diameter / 2 - zodiacWidth - 75, 0, houseList1[i] - houseList1[1]);
                    housePt.x = housePt.x + CenterX;
                    housePt.y = -1 * housePt.y + CenterY;
                }
                else
                {
                    housePt = Util.Rotate(diameter / 2 - zodiacWidth - 115, 0, houseList1[i] - houseList1[1]);
                    housePt.x = housePt.x + CenterX;
                    housePt.y = -1 * housePt.y + CenterY;
                }
                housePtEnd = Util.Rotate(centerDiameter, 0, houseList1[i] - houseList1[1]);
                housePtEnd.x = housePtEnd.x + CenterX;
                housePtEnd.y = -1 * housePtEnd.y + CenterY;
                housePtTxt = Util.Rotate(diameter / 2 + 10, 0, houseList1[i] - houseList1[1]);
                housePtTxt.x = housePtTxt.x + CenterX - 5;
                housePtTxt.y = -1 * housePtTxt.y + CenterY;

                if (i == 1 || i == 4 || i == 7 || i == 10)
                {
                    cvs.DrawLine((float)housePt.x, (float)housePt.y, (float)housePtEnd.x, (float)housePtEnd.y, lineStyle1);
                }
                else
                {
                    cvs.DrawLine((float)housePt.x, (float)housePt.y, (float)housePtEnd.x, (float)housePtEnd.y, lineStyle2);
                }

                cvs.DrawText(((int)houseList1[i] % 30).ToString(), (float)housePtTxt.x, (float)housePtTxt.y, textStyle);
            }
            if (appDelegate.bands > 1)
            {
                for (int i = 1; i <= 12; i++)
                {
                    if (appDelegate.bands == 2)
                    {
                        housePt = Util.Rotate(diameter / 2 - zodiacWidth, 0, houseList2[i] - houseList1[1]);
                        housePt.x = housePt.x + CenterX;
                        housePt.y = -1 * housePt.y + CenterY;
                        housePtEnd = Util.Rotate(diameter / 2 - zodiacWidth - 75, 0, houseList2[i] - houseList1[1]);
                        housePtEnd.x = housePtEnd.x + CenterX;
                        housePtEnd.y = -1 * housePtEnd.y + CenterY;
                    }
                    else
                    {
                        housePt = Util.Rotate(diameter / 2 - zodiacWidth - 55, 0, houseList2[i] - houseList1[1]);
                        housePt.x = housePt.x + CenterX;
                        housePt.y = -1 * housePt.y + CenterY;
                        housePtEnd = Util.Rotate(diameter / 2 - zodiacWidth - 115, 0, houseList2[i] - houseList1[1]);
                        housePtEnd.x = housePtEnd.x + CenterX;
                        housePtEnd.y = -1 * housePtEnd.y + CenterY;
                    }
                    if (i == 1 || i == 4 || i == 7 || i == 10)
                    {
                        cvs.DrawLine((float)housePt.x, (float)housePt.y, (float)housePtEnd.x, (float)housePtEnd.y, lineStyle1);
                    }
                    else
                    {
                        cvs.DrawLine((float)housePt.x, (float)housePt.y, (float)housePtEnd.x, (float)housePtEnd.y, lineStyle2);
                    }
                }

            }
            if (appDelegate.bands > 2)
            {
                for (int i = 1; i <= 12; i++)
                {
                    housePt = Util.Rotate(diameter / 2 - zodiacWidth, 0, houseList3[i] - houseList1[1]);
                    housePt.x = housePt.x + CenterX;
                    housePt.y = -1 * housePt.y + CenterY;
                    housePtEnd = Util.Rotate(diameter / 2 - zodiacWidth - 55, 0, houseList3[i] - houseList1[1]);
                    housePtEnd.x = housePtEnd.x + CenterX;
                    housePtEnd.y = -1 * housePtEnd.y + CenterY;
                    if (i == 1 || i == 4 || i == 7 || i == 10)
                    {
                        cvs.DrawLine((float)housePt.x, (float)housePt.y, (float)housePtEnd.x, (float)housePtEnd.y, lineStyle1);
                    }
                    else
                    {
                        cvs.DrawLine((float)housePt.x, (float)housePt.y, (float)housePtEnd.x, (float)housePtEnd.y, lineStyle2);
                    }
                }

            }

            // sign cusps
            Position signPt;
            Position signPtEnd;
            for (int i = 1; i <= 12; i++)
            {
                signPt = Util.Rotate(diameter / 2, 0, 30 * i - houseList1[1]);
                signPt.x = signPt.x + CenterX;
                signPt.y = -1 * signPt.y + CenterY;
                signPtEnd = Util.Rotate(diameter / 2 - zodiacWidth, 0, 30 * i - houseList1[1]);
                signPtEnd.x = signPtEnd.x + CenterX;
                signPtEnd.y = -1 * signPtEnd.y + CenterY;
                cvs.DrawLine((float)signPt.x, (float)signPt.y, (float)signPtEnd.x, (float)signPtEnd.y, lineStyle);
            }

            System.Reflection.Assembly asm =
                System.Reflection.Assembly.GetExecutingAssembly();
            //SKManagedStream stream = new SKManagedStream(asm.GetManifestResourceStream("microcosm.system.AstroDotBasic.ttf"));
            //{
            // ♈〜♓までのシンボル
            var root = Util.root;

            p.Typeface = SKTypeface.FromFile("system/microcosm.otf");
            //p.Typeface = SKTypeface.FromFile(root + "/system/AstroDotBasic.ttf");
                //p.Typeface = Typeface(stream);
            p.TextSize = 28;
            Position signValuePt;
            //SKColor pink = SKColors.Pink;
            //p.Color = pink;
            float fontW;
            float fontH;
            for (int i = 0; i < signs.Length; i++)
            {
                signValuePt = Util.Rotate(diameter / 2 - 15, 0, 15 + 30 * i - houseList1[1]);
                fontW = p.MeasureText(signs[i]) / 2;
                fontH = (p.FontMetrics.Descent - p.FontMetrics.Ascent) / 4;
                signValuePt.x = signValuePt.x + CenterX - fontW;
                signValuePt.y = -1 * signValuePt.y + CenterY + fontH;
                p.Color = CommonData.getSignColor(30 * i);

                cvs.DrawText(signs[i], (float)signValuePt.x, (float)signValuePt.y, p);
            }
            p.Color = SKColors.Black;
            //}


            // 天体そのもの
            int[] box = new int[72];
            int planetOffset = 0;
            IOrderedEnumerable<KeyValuePair<int, PlanetData>> sortPlanetData = list1.OrderBy(pair => pair.Value.absolute_position);
            foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData)
            {
                PlanetData planet = pData.Value;
                Debug.WriteLine("planet: " + planet.no + "(" + CommonData.getPlanetSymbolText(planet.no) + ")");
                Debug.WriteLine("isDisp: " + planet.isDisp);
                Debug.WriteLine("display: " + planet.absolute_position);

                if (!planet.isDisp)
                {
                    continue;
                }
                if (planet.no == 10000 || planet.no == 10001)
                {
                    // 一重円のASC、MC
                    continue;
                }

                int index = (int)(planet.absolute_position / 5);

                BoxInit(ref box, ref index);

                // 天体そのもの
                planetOffset = GetPlanetOffset(1);
                DrawPlanetText(index, houseList1[1], planet, p, cvs, planetOffset);

                // 天体から中心への線
                //if (appDelegate.bands == 1)
                //{
                    Position startPt;
                    Position endPt;

                    startPt = Util.Rotate(diameter / 2 - (planetOffset + 95), 0, 5 * index - houseList1[1]);
                    startPt.x = startPt.x + CenterX;
                    startPt.y = -1 * startPt.y + CenterY;

                    endPt = Util.Rotate(centerDiameter, 0, planet.absolute_position - houseList1[1]);
                    endPt.x = endPt.x + CenterX;
                    endPt.y = -1 * endPt.y + CenterY;

                    cvs.DrawLine((float)startPt.x, (float)startPt.y, (float)endPt.x, (float)endPt.y, lineStyle);
                //}
            }
            



            // 二重円の天体
            if (appDelegate.bands > 1)
            {
                int[] box2 = new int[72];
                int planetOffset2 = 0;
                IOrderedEnumerable<KeyValuePair<int, PlanetData>> sortPlanetData2 = list2.OrderBy(pair => pair.Value.absolute_position);
                foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData2)
                {
                    PlanetData planet2 = pData.Value;
                    Debug.WriteLine("planet2: " + planet2.no + "(" + CommonData.getPlanetSymbolText(planet2.no) + ")");
                    Debug.WriteLine("isDisp2: " + planet2.isDisp);
                    Debug.WriteLine("display2: " + planet2.absolute_position);

                    if (!planet2.isDisp)
                    {
                        continue;
                    }

                    int index2 = (int)(planet2.absolute_position / 5);

                    BoxInit(ref box2, ref index2);

                    // 天体そのもの
                    planetOffset2 = GetPlanetOffset(2);
                    DrawPlanetText(index2, houseList1[1], planet2, p, cvs, planetOffset2);
                }
            }

            // 三重円の天体
            if (appDelegate.bands > 2)
            {
                int[] box3 = new int[72];
                int planetOffset3 = 0;
                IOrderedEnumerable<KeyValuePair<int, PlanetData>> sortPlanetData3 = list3.OrderBy(pair => pair.Value.absolute_position);
                foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData3)
                {
                    PlanetData planet3 = pData.Value;
                    Debug.WriteLine("planet3: " + planet3.no + "(" + CommonData.getPlanetSymbolText(planet3.no) + ")");
                    Debug.WriteLine("isDisp3: " + planet3.isDisp);
                    Debug.WriteLine("display3: " + planet3.absolute_position);

                    if (!planet3.isDisp)
                    {
                        continue;
                    }

                    int index3 = (int)(planet3.absolute_position / 5);

                    BoxInit(ref box3, ref index3);

                    // 天体そのもの
                    planetOffset3 = GetPlanetOffset(3);
                    DrawPlanetText(index3, houseList1[1], planet3, p, cvs, planetOffset3);
                }
            }

            // 四重円の天体
            if (appDelegate.bands > 3)
            {
                int[] box4 = new int[72];
                /*
                IOrderedEnumerable<PlanetData> sortPlanetData4 = ringsData[3].planetData.OrderBy(planetTmp => planetTmp.absolute_position);
                foreach (PlanetData planet in sortPlanetData4)
                {
                    if (!CommonInstance.getInstance().currentSetting.GetDispPlanet(planet.no))
                    {
                        continue;
                    }

                    int index = (int)(planet.absolute_position / 5);

                    BoxInit(ref box4, ref index, planet.absolute_position);

                    planetOffset = GetPlanetOffset(4);
                    DrawPlanetText(index, ringsData[3].cusps[1], planet, p, cvs, degreeText, planetOffset, 35, 10, 20, 15, 10);
                }
                */
            }

            // aspects
            Position aspectPt;
            Position aspectPtEnd;
            SKPaint aspectLine = new SKPaint();
            aspectLine.Style = SKPaintStyle.Stroke;
            aspectLine.StrokeWidth = 1.0F;
            SKPaint aspectSymboolText = new SKPaint()
            {
                TextSize = 24,
                Style = SKPaintStyle.Fill
            };
            // aspectsData[0, 0] => natal-natal
            if (appDelegate.aspect11disp)
            {
                foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData)
                {
                    PlanetData planet = pData.Value;
                    Debug.WriteLine("planet: " + planet.no + "(" + CommonData.getPlanetSymbolText(planet.no) + ")");
                    Debug.WriteLine("isDisp: " + planet.isDisp);
                    Debug.WriteLine("display: " + planet.absolute_position);

                    if (!planet.isDisp)
                    {
                        continue;
                    }
                    // isAspectDispは不要

                    foreach (AspectInfo x in planet.aspects)
                    {
                        if (!list1[x.targetPlanetNo].isDisp)
                        {
                            continue;
                        }

                        if (x.softHard == SoftHard.SOFT)
                        {
                            aspectLine.PathEffect = SKPathEffect.CreateDash(new float[] { 1, 4 }, (float)2.0);
                        }
                        else
                        {
                            aspectLine.PathEffect = null;
                        }

                        double calcDegree = x.sourceDegree - houseList1[1];
                        if (calcDegree < 0)
                        {
                            calcDegree += 360;
                        }
                        double calcDegree2 = x.targetDegree - houseList1[1];
                        if (calcDegree2 < 0)
                        {
                            calcDegree2 += 360;
                        }
                        aspectPt = Util.Rotate(centerDiameter, 0, calcDegree);
                        aspectPt.x = aspectPt.x + CenterX;
                        aspectPt.y = -1 * aspectPt.y + CenterY;

                        aspectPtEnd = Util.Rotate(centerDiameter, 0, calcDegree2);
                        aspectPtEnd.x = aspectPtEnd.x + CenterX;
                        aspectPtEnd.y = -1 * aspectPtEnd.y + CenterY;

                        GetAspectLineAndText(x.aspectKind, ref aspectLine, ref aspectSymboolText);
                        DrawAspect(cvs, x, aspectPt, aspectPtEnd, aspectLine, aspectSymboolText);
                    }

                    /*
                    aspectSymbolPt = Util.Rotate(diameter - 160, 0, planet.absolute_position - houseList1[1]);
                    aspectSymbolPt.x = aspectSymbolPt.x + CenterX;
                    aspectSymbolPt.y = -1 * aspectSymbolPt.y + CenterY + 10;
                    */
                }
            }

            // 二重円アスペクト
            // aspectsData[0, 1]
            if (appDelegate.bands > 1 && appDelegate.aspect12disp)
            {
                foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData)
                {
                    PlanetData planet = pData.Value;
                    Debug.WriteLine("planet: " + planet.no + "(" + CommonData.getPlanetSymbolText(planet.no) + ")");
                    Debug.WriteLine("isDisp: " + planet.isDisp);
                    Debug.WriteLine("display: " + planet.absolute_position);

                    if (!planet.isDisp)
                    {
                        continue;
                    }
                    // isAspectDispは不要

                    foreach (AspectInfo x in planet.secondAspects)
                    {
                        if (!list1[x.targetPlanetNo].isDisp)
                        {
                            continue;
                        }
                        if (!list2[x.targetPlanetNo].isDisp)
                        {
                            continue;
                        }

                        if (x.softHard == SoftHard.SOFT)
                        {
                            aspectLine.PathEffect = SKPathEffect.CreateDash(new float[] { 1, 4 }, (float)2.0);
                        }
                        else
                        {
                            aspectLine.PathEffect = null;
                        }

                        // box度数じゃないからちょっとずれるんだよね
                        double calcDegree = x.sourceDegree - houseList1[1];
                        if (calcDegree < 0)
                        {
                            calcDegree += 360;
                        }
                        double calcDegree2 = x.targetDegree - houseList2[1];
                        if (calcDegree2 < 0)
                        {
                            calcDegree2 += 360;
                        }
                        aspectPt = Util.Rotate(centerDiameter, 0, calcDegree);
                        aspectPt.x = aspectPt.x + CenterX;
                        aspectPt.y = -1 * aspectPt.y + CenterY;

                        if (appDelegate.bands == 2)
                        {
                            offset = (diameter / 2 - zodiacWidth - centerDiameter) / 2;
                        }
                        else if (appDelegate.bands == 3)
                        {
                            offset = (diameter / 2 - zodiacWidth - centerDiameter) / 3;
                        }
                        aspectPtEnd = Util.Rotate(centerDiameter + offset, 0, calcDegree2);
                        aspectPtEnd.x = aspectPtEnd.x + CenterX;
                        aspectPtEnd.y = -1 * aspectPtEnd.y + CenterY;

                        GetAspectLineAndText(x.aspectKind, ref aspectLine, ref aspectSymboolText);
                        DrawAspect(cvs, x, aspectPt, aspectPtEnd, aspectLine, aspectSymboolText);
                    }
                }
            }

            // 二重円アスペクト
            // aspectsData[1, 1]
            if (appDelegate.bands > 1 && appDelegate.aspect22disp)
            {
                IOrderedEnumerable<KeyValuePair<int, PlanetData>> sortPlanetData2 = list2.OrderBy(pair => pair.Value.absolute_position);
                foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData2)
                {
                    PlanetData planet = pData.Value;
                    Debug.WriteLine("planet: " + planet.no + "(" + CommonData.getPlanetSymbolText(planet.no) + ")");
                    Debug.WriteLine("isDisp: " + planet.isDisp);
                    Debug.WriteLine("display: " + planet.absolute_position);

                    if (!planet.isDisp)
                    {
                        continue;
                    }
                    // isAspectDispは不要

                    foreach (AspectInfo x in planet.secondAspects)
                    {
                        if (!list2[x.targetPlanetNo].isDisp)
                        {
                            continue;
                        }

                        if (x.softHard == SoftHard.SOFT)
                        {
                            aspectLine.PathEffect = SKPathEffect.CreateDash(new float[] { 1, 4 }, (float)2.0);
                        }
                        else
                        {
                            aspectLine.PathEffect = null;
                        }

                        // box度数じゃないからちょっとずれるんだよね
                        double calcDegree = x.sourceDegree - houseList2[1];
                        if (calcDegree < 0)
                        {
                            calcDegree += 360;
                        }
                        double calcDegree2 = x.targetDegree - houseList2[1];
                        if (calcDegree2 < 0)
                        {
                            calcDegree2 += 360;
                        }

                        if (appDelegate.bands == 2)
                        {
                            offset = (diameter / 2 - zodiacWidth - centerDiameter) / 2;
                        }
                        else if (appDelegate.bands == 3)
                        {
                            offset = (diameter / 2 - zodiacWidth - centerDiameter) / 3;
                        }

                        aspectPt = Util.Rotate(centerDiameter + offset, 0, calcDegree);
                        aspectPt.x = aspectPt.x + CenterX;
                        aspectPt.y = -1 * aspectPt.y + CenterY;

                        aspectPtEnd = Util.Rotate(centerDiameter + offset, 0, calcDegree2);
                        aspectPtEnd.x = aspectPtEnd.x + CenterX;
                        aspectPtEnd.y = -1 * aspectPtEnd.y + CenterY;

                        GetAspectLineAndText(x.aspectKind, ref aspectLine, ref aspectSymboolText);
                        DrawAspect(cvs, x, aspectPt, aspectPtEnd, aspectLine, aspectSymboolText);
                    }
                }
            }

            // 三重円アスペクト
            // aspectsData[0, 2]
            if (appDelegate.bands > 2 && appDelegate.aspect13disp)
            {
                foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData)
                {
                    PlanetData planet = pData.Value;

                    if (!planet.isDisp)
                    {
                        continue;
                    }
                    // isAspectDispは不要

                    foreach (AspectInfo x in planet.thirdAspects)
                    {
                        if (!list1[x.targetPlanetNo].isDisp)
                        {
                            continue;
                        }
                        if (!list3[x.targetPlanetNo].isDisp)
                        {
                            continue;
                        }

                        if (x.softHard == SoftHard.SOFT)
                        {
                            aspectLine.PathEffect = SKPathEffect.CreateDash(new float[] { 1, 4 }, (float)2.0);
                        }
                        else
                        {
                            aspectLine.PathEffect = null;
                        }

                        // box度数じゃないからちょっとずれるんだよね
                        double calcDegree = x.sourceDegree - houseList1[1];
                        if (calcDegree < 0)
                        {
                            calcDegree += 360;
                        }
                        double calcDegree2 = x.targetDegree - houseList1[1];
                        if (calcDegree2 < 0)
                        {
                            calcDegree2 += 360;
                        }
                        aspectPt = Util.Rotate(centerDiameter, 0, calcDegree);
                        aspectPt.x = aspectPt.x + CenterX;
                        aspectPt.y = -1 * aspectPt.y + CenterY;

                        offset = (diameter / 2 - zodiacWidth - centerDiameter) / 3;
                        aspectPtEnd = Util.Rotate(centerDiameter + offset * 2, 0, calcDegree2);
                        aspectPtEnd.x = aspectPtEnd.x + CenterX;
                        aspectPtEnd.y = -1 * aspectPtEnd.y + CenterY;

                        GetAspectLineAndText(x.aspectKind, ref aspectLine, ref aspectSymboolText);
                        DrawAspect(cvs, x, aspectPt, aspectPtEnd, aspectLine, aspectSymboolText);
                    }
                }
            }

            // 三重円アスペクト
            // aspectsData[1, 2]
            if (appDelegate.bands > 2 && appDelegate.aspect23disp)
            {
                IOrderedEnumerable<KeyValuePair<int, PlanetData>> sortPlanetData2 = list2.OrderBy(pair => pair.Value.absolute_position);
                foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData2)
                {
                    PlanetData planet = pData.Value;

                    if (!planet.isDisp)
                    {
                        continue;
                    }
                    // isAspectDispは不要

                    foreach (AspectInfo x in planet.thirdAspects)
                    {
                        if (!list2[x.targetPlanetNo].isDisp)
                        {
                            continue;
                        }
                        if (!list3[x.targetPlanetNo].isDisp)
                        {
                            continue;
                        }

                        if (x.softHard == SoftHard.SOFT)
                        {
                            aspectLine.PathEffect = SKPathEffect.CreateDash(new float[] { 1, 4 }, (float)2.0);
                        }
                        else
                        {
                            aspectLine.PathEffect = null;
                        }

                        // box度数じゃないからちょっとずれるんだよね
                        double calcDegree = x.sourceDegree - houseList1[1];
                        if (calcDegree < 0)
                        {
                            calcDegree += 360;
                        }
                        double calcDegree2 = x.targetDegree - houseList1[1];
                        if (calcDegree2 < 0)
                        {
                            calcDegree2 += 360;
                        }
                        offset = (diameter / 2 - zodiacWidth - centerDiameter) / 3;
                        aspectPt = Util.Rotate(centerDiameter + offset, 0, calcDegree);
                        aspectPt.x = aspectPt.x + CenterX;
                        aspectPt.y = -1 * aspectPt.y + CenterY;

                        aspectPtEnd = Util.Rotate(centerDiameter + offset * 2, 0, calcDegree2);
                        aspectPtEnd.x = aspectPtEnd.x + CenterX;
                        aspectPtEnd.y = -1 * aspectPtEnd.y + CenterY;

                        GetAspectLineAndText(x.aspectKind, ref aspectLine, ref aspectSymboolText);
                        DrawAspect(cvs, x, aspectPt, aspectPtEnd, aspectLine, aspectSymboolText);
                    }
                }
            }

            // 三重円アスペクト
            // aspectsData[2, 2]
            if (appDelegate.bands > 2 && appDelegate.aspect33disp)
            {
                IOrderedEnumerable<KeyValuePair<int, PlanetData>> sortPlanetData2 = list2.OrderBy(pair => pair.Value.absolute_position);
                foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData2)
                {
                    PlanetData planet = pData.Value;

                    if (!planet.isDisp)
                    {
                        continue;
                    }
                    // isAspectDispは不要

                    foreach (AspectInfo x in planet.thirdAspects)
                    {
                        if (!list3[x.targetPlanetNo].isDisp)
                        {
                            continue;
                        }

                        if (x.softHard == SoftHard.SOFT)
                        {
                            aspectLine.PathEffect = SKPathEffect.CreateDash(new float[] { 1, 4 }, (float)2.0);
                        }
                        else
                        {
                            aspectLine.PathEffect = null;
                        }

                        // box度数じゃないからちょっとずれるんだよね
                        double calcDegree = x.sourceDegree - houseList1[1];
                        if (calcDegree < 0)
                        {
                            calcDegree += 360;
                        }
                        double calcDegree2 = x.targetDegree - houseList1[1];
                        if (calcDegree2 < 0)
                        {
                            calcDegree2 += 360;
                        }
                        offset = (diameter / 2 - zodiacWidth - centerDiameter) / 3;
                        aspectPt = Util.Rotate(centerDiameter + offset * 2, 0, calcDegree);
                        aspectPt.x = aspectPt.x + CenterX;
                        aspectPt.y = -1 * aspectPt.y + CenterY;

                        aspectPtEnd = Util.Rotate(centerDiameter + offset * 2, 0, calcDegree2);
                        aspectPtEnd.x = aspectPtEnd.x + CenterX;
                        aspectPtEnd.y = -1 * aspectPtEnd.y + CenterY;

                        GetAspectLineAndText(x.aspectKind, ref aspectLine, ref aspectSymboolText);
                        DrawAspect(cvs, x, aspectPt, aspectPtEnd, aspectLine, aspectSymboolText);
                    }
                }
            }

            cvs.Flush();

        }

        /// <summary>
        /// 色を取得
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="line"></param>
        /// <param name="symbol"></param>
        public void GetAspectLineAndText(AspectKind kind, ref SKPaint line, ref SKPaint symbol)
        {
            if (kind == AspectKind.OPPOSITION)
            {
                line.Color = SKColors.HotPink;
                symbol.Color = SKColors.HotPink;
            }
            else if (kind == AspectKind.TRINE)
            {
                line.Color = SKColors.Orange;
                symbol.Color = SKColors.Orange;
            }
            else if (kind == AspectKind.SQUARE)
            {
                line.Color = SKColors.MediumPurple;
                symbol.Color = SKColors.MediumPurple;
            }
            else if (kind == AspectKind.SEXTILE)
            {
                line.Color = SKColors.Green;
                symbol.Color = SKColors.Green;
            }
            else if (kind == AspectKind.INCONJUNCT)
            {
                line.Color = SKColors.Sienna;
                symbol.Color = SKColors.Sienna;
            }
            else if (kind == AspectKind.SESQUIQUADRATE)
            {
                line.Color = SKColors.Lime;
                symbol.Color = SKColors.Lime;
            }
            else if (kind == AspectKind.SEMISQUARE)
            {
                line.Color = SKColors.Tomato;
                symbol.Color = SKColors.Tomato;
            }
            else if (kind == AspectKind.SEMISEXTILE)
            {
                line.Color = SKColors.DarkTurquoise;
                symbol.Color = SKColors.DarkTurquoise;
            }
        }


        /// <summary>
        /// 天体表示
        /// </summary>
        /// <param name="index">boxIndex</param>
        /// <param name="cusp0">cusps[0]</param>
        /// <param name="planet">Planet.</param>
        /// <param name="p">SKPaintインスタンス</param>
        /// <param name="cvs">SKCanvasインスタンス</param>
        /// <param name="planetOffset">一番外側(獣帯内側)から天体までのオフセット</param>
        public void DrawPlanetText(int index, double cusp0, PlanetData planet, SKPaint p, SKCanvas cvs,
                                   int planetOffset)
        {
            float CenterX = this.canvasWidth / 2;
            float CenterY = this.canvasWidth / 2;


            // 一番外側 - 獣帯
            float diameter = this.canvasWidth - 60;
            // 回転のベース
            double radius = diameter / 2;
            // オフセットは大きくすると内側、小さくすると外側
            // 天体から度数までのオフセット
            int planetDOffset = 20;
            // 度数からサインまでのオフセット
            int planetSOffset = 25;
            // サインから小数点までのオフセット
            int planetCOffset = 25;

            SKPaint degreeText = new SKPaint();

            Position planetPt;
            Position planetPt2;
            Position planetPt3;
            Position planetDegreePt;
            Position planetRetrogradePt;

            // 🌙を描画
            planetPt = Util.Rotate(diameter / 2 - planetOffset, 0, 5 * index - cusp0);
            var fontW = p.MeasureText(CommonData.getPlanetSymbol2(planet.no)) / 2;
            var fontH = (p.FontMetrics.Descent - p.FontMetrics.Ascent) / 2;

            planetPt.x = planetPt.x + CenterX - fontW;
            planetPt.y = -1 * planetPt.y + CenterY + fontH + 5;
            p.Color = CommonData.getPlanetColor(planet.no);
            cvs.DrawText(CommonData.getPlanetSymbol2(planet.no), (float)planetPt.x, (float)planetPt.y, p);

            SKRect rect = new SKRect(
                392 + (int)planetPt.x,
                780 - (int)planetPt.y - 10,
                392 + (int)planetPt.x + 20,
                780 - (int)planetPt.y + 10
                );
            // ここで入れたものはCanvasViewクラスから呼び出す
            mouseInList.Add(new MouseIn() {
                kind = 0,
                message = CommonData.getPlanetSymbolText(planet.no),
                rect = rect,
                degree = planet.absolute_position
            });

            /*
            if (!explanation.ContainsKey(planet.no))
            {
                explanation.Add(planet.no, new MouseInner()
                {
                    start = new Point(392 + (int)planetPt.x, 780 - (int)planetPt.y - 10),
                    end = new Point(392 + (int)planetPt.x + 20, 780 - (int)planetPt.y + 10),
                    message = CommonData.getPlanetSymbolText(planet.no)
                });
            }
            */

            // 天体度数
            planetDegreePt = Util.Rotate(radius - (planetOffset + planetDOffset), 0, 5 * index - cusp0);
            fontW = degreeText.MeasureText(((int)(planet.absolute_position % 30)).ToString("00")) / 2;
            fontH = (degreeText.FontMetrics.Descent - degreeText.FontMetrics.Ascent) / 2;
            planetDegreePt.x = planetDegreePt.x + CenterX - fontW;
            planetDegreePt.y = -1 * planetDegreePt.y + CenterY + fontH + 10;
            degreeText.Color = SKColors.Black;
            degreeText.TextSize = 14;
            degreeText.Style = SKPaintStyle.Fill;
            cvs.DrawText(((int)(planet.absolute_position % 30)).ToString("00"), (float)planetDegreePt.x, (float)planetDegreePt.y, degreeText);

            // ♈を描画
            if (appDelegate.bands == 1 && configData.dispPattern2 == EDispPettern.FULL)
            {
                planetPt2 = Util.Rotate(radius - (planetOffset + planetDOffset) - planetSOffset, 0, 5 * index - cusp0);
                fontW = p.MeasureText(CommonData.getSignSymbol((int)(planet.absolute_position / 30))) / 2;
                fontH = (p.FontMetrics.Descent - p.FontMetrics.Ascent) / 2;

                planetPt2.x = planetPt2.x + CenterX - fontW;
                planetPt2.y = -1 * planetPt2.y + CenterY + fontH + 5;
                cvs.DrawText(CommonData.getSignSymbol((int)(planet.absolute_position / 30)), (float)planetPt2.x, (float)planetPt2.y, p);
            }

            // 小数点
            if (appDelegate.bands == 1 && configData.dispPattern2 == EDispPettern.FULL)
            {
                string minuteTxt = "";
                if (configData.decimalDisp == EDecimalDisp.DECIMAL)
                {
                    // 小数点2桁を出す (% 1 * 100)
                    minuteTxt = (planet.absolute_position % 1 * 100).ToString("00");
                }
                else
                {
                    minuteTxt = CommonData.DecimalToHex(planet.absolute_position % 1 * 100).ToString("00") + "'";
                }

                planetPt3 = Util.Rotate(radius - (planetOffset + planetDOffset + planetSOffset) - planetCOffset, 0, 5 * index - cusp0);
                fontW = degreeText.MeasureText(minuteTxt) / 2;
                fontH = (degreeText.FontMetrics.Descent - degreeText.FontMetrics.Ascent) / 2;
                planetPt3.x = planetPt3.x + CenterX - fontW;
                planetPt3.y = -1 * planetPt3.y + CenterY + fontH + 10;
                cvs.DrawText(minuteTxt, (float)planetPt3.x, (float)planetPt3.y, degreeText);
            }


            // 逆行
            if (appDelegate.bands == 1 && configData.dispPattern2 == EDispPettern.FULL)
            {
                if (planet.speed < 0)
                {
                    planetRetrogradePt = Util.Rotate(radius - (planetOffset + planetDOffset + planetSOffset) - 45, 0, 5 * index - cusp0);
                    fontW = p.MeasureText("R") / 2;
                    fontH = (p.FontMetrics.Descent - p.FontMetrics.Ascent) / 2;
                    planetRetrogradePt.x = planetRetrogradePt.x + CenterX - fontW;
                    planetRetrogradePt.y = -1 * planetRetrogradePt.y + CenterY + fontH + 5;
                    degreeText.Color = SKColors.Black;
                    cvs.DrawText("Z", (float)planetRetrogradePt.x, (float)planetRetrogradePt.y, p);
                }
            }
        }

        public void BoxInit(ref int[] box, ref int index)
        {
            if (box.Length < index || index < 0)
            {
                Debug.WriteLine("BoxInitError:" + index);
                return;
            }
            // 重ならないようにずらしを入れる
            // 1サインに6度単位5個までデータが入る
            if (box[index] == 1)
            {
                while (box[index] == 1)
                {
                    index++;
                    if (index == 72)
                    {
                        index = 0;
                    }
                }
                box[index] = 1;
            }
            else
            {
                box[index] = 1;
            }

        }

        /// <summary>
        /// アスペクトを描画
        /// </summary>
        /// <param name="cvs">Cvs.</param>
        /// <param name="info">Info.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="aspectLine">Aspect line.</param>
        /// <param name="symbol">Symbol.</param>
        public void DrawAspect(SKCanvas cvs, AspectInfo info, Position from, Position to, SKPaint aspectLine, SKPaint symbol)
        {
            symbol.Typeface = SKTypeface.FromFile("system/microcosm-aspects.otf");
            /*
            cvs.DrawLine((float)from.x, (float)from.y, (float)to.x, (float)to.y, aspectLine);
            cvs.DrawText(CommonData.getAspectSymbol(info.aspectKind),
             (float)((from.x + to.x) / 2), (float)((from.y + to.y) / 2), symbol);
            */

            symbol.TextSize = 16;
            SKColor pink = SKColors.Pink;
            symbol.Style = SKPaintStyle.Fill;

            double newX = (from.x + to.x) / 2;
            double newY = (from.y + to.y) / 2;
            cvs.DrawLine((float)from.x, (float)from.y, (float)to.x, (float)to.y, aspectLine);
            cvs.DrawText(CommonData.getAspectSymbol(info.aspectKind),
             (float)(newX), (float)(newY + 5), symbol);

            SKRect rect = new SKRect(
            392 + (int)newX,
            780 - (int)newY - 10,
            392 + (int)newX + 20,
            780 - (int)newY + 10
            );
            // ここで入れたものはCanvasViewから呼び出す
            mouseInList.Add(new MouseIn()
            {
                kind = 1,
                message = info.aspectKind.ToString() + CommonData.getPlanetSymbolText(info.srcPlanetNo) + "-" + CommonData.getPlanetSymbolText(info.targetPlanetNo),
                rect = rect,
                degree = info.aspectDegree
            });

        }

        /// <summary>
        /// 全体のリング数と何番目のリングかでオフセットを計算する
        /// </summary>
        /// <param name="bandIndex">何番目のリングか(> 0)</param>
        /// <returns></returns>
        public int GetPlanetOffset(int bandIndex)
        {
            int[] planetOffset1 = { 50 };
            int[] planetOffset2 = { 130, 50 };
            int[] planetOffset3 = { 160, 110, 50 };
            int[] planetOffset4 = { 270, 200, 130, 50 };
            if (appDelegate.bands == 1)
            {
                return planetOffset1[bandIndex - 1];
            }
            else if (appDelegate.bands == 2)
            {
                return planetOffset2[bandIndex - 1];
            }
            else if (appDelegate.bands == 3)
            {
                return planetOffset3[bandIndex - 1];
            }
            else if (appDelegate.bands == 4)
            {
                return planetOffset4[bandIndex - 1];
            }

            return planetOffset1[0];
        }

        public void SetReport()
        {
            int down = 0;
            int right = 0;
            int up = 0;
            int left = 0;

            double[] newList = new double[13];
            Dictionary<int, PlanetData> signList1 = list1;

            Enumerable.Range(1, 12).ToList().ForEach(i =>
            {
                newList[i] = houseList1[i] - houseList1[1];
                if (newList[i] < 0)
                {
                    newList[i] += 360;
                }
                //                Console.WriteLine(list1[i].ToString());
            });

            double target;
            Enumerable.Range(0, 10).ToList().ForEach(i =>
            {
                target = signList1[i].absolute_position - houseList1[1];
                if (target < 0)
                {
                    target += 360;
                }
                if (
                    (newList[1] <= target && target < newList[2])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target +  ":1");
                    down++;
                    left++;
                }
                else if (
                    (newList[2] <= target && target < newList[3])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":2");
                    down++;
                    left++;
                }
                else if (
                    (newList[3] <= target && target < newList[4])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":3");
                    down++;
                    left++;
                }
                else if (
                    (newList[4] <= target && target < newList[5])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":4");
                    down++;
                    right++;
                }
                else if (
                    (newList[5] <= target && target < newList[6])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":5");
                    down++;
                    right++;
                }
                else if (
                    (newList[6] <= target && target < newList[7])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":6");
                    down++;
                    right++;
                }
                else if (
                    (newList[7] <= target && target < newList[8])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":7");
                    up++;
                    right++;
                }
                else if (
                    (newList[8] <= target && target < newList[9])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":8");
                    up++;
                    right++;
                }
                else if (
                    (newList[9] <= target && target < newList[10])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":9");
                    up++;
                    right++;
                }
                else if (
                    (newList[10] <= target && target < newList[11])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":10");
                    up++;
                    left++;
                }
                else if (
                    (newList[11] <= target && target < newList[12])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":11");
                    up++;
                    left++;
                }
                else
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":12");
                    up++;
                    left++;
                }

            });

            houseBottom.StringValue = down.ToString();
            houseRight.StringValue = right.ToString();
            houseTop.StringValue = up.ToString();
            houseLeft.StringValue = left.ToString();

            int fireVal = 0;
            int earthVal = 0;
            int airVal = 0;
            int waterVal = 0;
            int cardinal = 0;
            int fixe = 0;
            int mutable = 0;

            // 10天体
            Enumerable.Range(0, 10).ToList().ForEach(i =>
            {
                if (
                    (0.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 30.0) ||
                    (120.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 150.0) ||
                    (240.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 270.0)
                )
                {
                    fireVal++;
                }
                else if (
                    (30.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 60.0) ||
                    (150.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 180.0) ||
                    (270.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 300.0)
                )
                {
                    earthVal++;
                }
                else if (
                    (60.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 90.0) ||
                    (180.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 210.0) ||
                    (300.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 330.0)
                )
                {
                    airVal++;
                }
                else
                {
                    waterVal++;
                }
                if (
                    (0.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 30.0) ||
                    (90.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 120.0) ||
                    (180.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 210.0) ||
                    (270.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 300.0)
                )
                {
                    cardinal++;
                }
                else if (
                    (30.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 60.0) ||
                    (120.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 150.0) ||
                    (210.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 240.0) ||
                    (300.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 330.0)
                )
                {
                    fixe++;
                }
                else
                {
                    mutable++;
                }


            });

            fire.StringValue = fireVal.ToString();
            earth.StringValue = earthVal.ToString();
            air.StringValue = airVal.ToString();
            water.StringValue = waterVal.ToString();
            cardinalS.StringValue = cardinal.ToString();
            fixedS.StringValue = fixe.ToString();
            mutableS.StringValue = mutable.ToString();
        }

        public void SetSpanButton(string name)
        {
            spanButton.Title = name;
        }

        partial void debugButtonClicked(Foundation.NSObject sender)
        {
            AstroCalc calc = new AstroCalc(configData);
            EclipseCalc eclipse = calc.GetEclipseInstance();
            eclipse.GetEclipse(DateTime.Now, 9.0, CommonData.ZODIAC_MERCURY, 88.669056188166, true);
        }

        partial void SettingPopupChanged(Foundation.NSObject sender)
        {
            NSPopUpButton button = (NSPopUpButton)sender;
            int index = (int)button.IndexOfSelectedItem;
            SettingIndexChange(index);
        }

        /// <summary>
        /// 設定変更(ショートカットからも呼ばれる)
        /// </summary>
        /// <param name="setting"></param>
        public void SettingIndexChange(int index)
        {
            appDelegate.currentSetting = settings[index];
            appDelegate.settingIndex = index;

            settingPopupButton.SelectItem(appDelegate.currentSetting.dispName);

            ReCalc();
            ReRender();
        }

        public string GetTimeSet()
        {
            return timesetterChangeButton.SelectedItem.Title;
        }

        /// <summary>
        /// ">"ボタン
        /// </summary>
        /// <param name="sender"></param>
        partial void TimeSetRightClicked(Foundation.NSObject sender)
        {
            string currentTime = GetTimeSet();
            DateTime now = udata1.GetDateTime();
            if (appDelegate.currentSpanType == SpanType.UNIT)
            {
                if (currentTime == "User1")
                {
                    now = udata1.GetDateTime().AddSeconds(plusUnit);
                    udata1.SetDateTime(now);
                    ReCalc();
                    RefreshUserBox(0, udata1);
                }
                else if (currentTime == "User2")
                {
                    now = udata2.GetDateTime().AddSeconds(plusUnit);
                    udata2.SetDateTime(now);
                    ReCalc();
                    RefreshUserBox(1, udata2);
                }
                else if (currentTime == "Event1")
                {
                    now = edata1.GetDateTime().AddSeconds(plusUnit);
                    edata1.SetDateTime(now);
                    ReCalc();
                    RefreshEventBox(0, edata1);
                }
                else if (currentTime == "Event2")
                {
                    now = edata2.GetDateTime().AddSeconds(plusUnit);
                    edata2.SetDateTime(now);
                    ReCalc();
                    RefreshEventBox(1, edata2);
                }
            }
            ReRender();
        }

        /// <summary>
        /// "<"ボタン
        /// </summary>
        /// <param name="sender"></param>
        partial void TimeSetLeftClicked(Foundation.NSObject sender)
        {
            string currentTime = GetTimeSet();
            DateTime now = udata1.GetDateTime();
            if (appDelegate.currentSpanType == SpanType.UNIT)
            {
                if (currentTime == "User1")
                {
                    now = udata1.GetDateTime().AddSeconds(-1 * plusUnit);
                    udata1.SetDateTime(now);
                    ReCalc();
                    RefreshUserBox(0, udata1);
                }
                else if (currentTime == "User2")
                {
                    now = udata2.GetDateTime().AddSeconds(-1 * plusUnit);
                    udata2.SetDateTime(now);
                    ReCalc();
                    RefreshUserBox(1, udata2);
                }
                else if (currentTime == "Event1")
                {
                    now = edata1.GetDateTime().AddSeconds(-1 * plusUnit);
                    edata1.SetDateTime(now);
                    ReCalc();
                    RefreshEventBox(0, edata1);
                }
                else if (currentTime == "Event2")
                {
                    now = edata2.GetDateTime().AddSeconds(-1 * plusUnit);
                    edata2.SetDateTime(now);
                    ReCalc();
                    RefreshEventBox(1, edata2);
                }
            }
            ReRender();
        }

        partial void TimeSetNowClicked(Foundation.NSObject sender)
        {
            string currentTime = GetTimeSet();
            if (currentTime == "User1")
            {
                udata1.SetDateTime(DateTime.Now);
                ReCalc();
                RefreshUserBox(0, udata1);
            }
            else if (currentTime == "User2")
            {
                udata2.SetDateTime(DateTime.Now);
                ReCalc();
                RefreshUserBox(1, udata1);
            }
            else if (currentTime == "Event1")
            {
                edata1.SetDateTime(DateTime.Now);
                ReCalc();
                RefreshEventBox(0, edata1);
            }
            else if (currentTime == "Event2")
            {
                edata2.SetDateTime(DateTime.Now);
                ReCalc();
                RefreshEventBox(1, edata2);
            }
            ReRender();
        }

        /// <summary>
        /// 1重なら表示されている円、3重なら外側をNowにする
        /// </summary>
        public void TimeSetNowCurrentBand()
        {
            ETargetUser currentTime = ETargetUser.USER1;

            if (appDelegate.bands == 1)
            {
                currentTime = calcTargetUser[0];
            }
            else if (appDelegate.bands == 2)
            {
                currentTime = calcTargetUser[1];
            }
            else if (appDelegate.bands == 3)
            {
                currentTime = calcTargetUser[2];
            }
            DateTime now = DateTime.Now;
            if (currentTime == ETargetUser.USER1)
            {
                udata1.SetDateTime(now);
                ReCalc();
                RefreshUserBox(0, udata1);
            }
            else if (currentTime == ETargetUser.USER2)
            {
                udata2.SetDateTime(now);
                ReCalc();
                RefreshUserBox(1, udata2);
            }
            else if (currentTime == ETargetUser.EVENT1)
            {
                edata1.SetDateTime(now);
                ReCalc();
                RefreshEventBox(0, edata1);
            }
            else if (currentTime == ETargetUser.EVENT2)
            {
                edata2.SetDateTime(now);
                ReCalc();
                RefreshEventBox(1, edata2);
            }
            ReRender();

        }

        /// <summary>
        /// 1重なら表示されている円、3重なら外側かなぁ
        /// </summary>
        /// <param name="seconds"></param>
        public void TimeSetAny(int seconds)
        {
            ETargetUser currentTime = ETargetUser.USER1;

            if (appDelegate.bands == 1)
            {
                currentTime = calcTargetUser[0];
            }
            else if (appDelegate.bands == 2)
            {
                currentTime = calcTargetUser[1];
            }
            else if (appDelegate.bands == 3)
            {
                currentTime = calcTargetUser[2];
            }
            DateTime now = udata1.GetDateTime();
            if (currentTime == ETargetUser.USER1)
            {
                now = udata1.GetDateTime().AddSeconds(seconds);
                udata1.SetDateTime(now);
                ReCalc();
                RefreshUserBox(0, udata1);
            }
            else if (currentTime == ETargetUser.USER2)
            {
                now = udata2.GetDateTime().AddSeconds(seconds);
                udata2.SetDateTime(now);
                ReCalc();
                RefreshUserBox(1, udata2);
            }
            else if (currentTime == ETargetUser.EVENT1)
            {
                now = edata1.GetDateTime().AddSeconds(seconds);
                edata1.SetDateTime(now);
                ReCalc();
                RefreshEventBox(0, edata1);
            }
            else if (currentTime == ETargetUser.EVENT2)
            {
                now = edata2.GetDateTime().AddSeconds(seconds);
                edata2.SetDateTime(now);
                ReCalc();
                RefreshEventBox(1, edata2);
            }
            ReRender();

        }

        /// <summary>
        /// マウスオーバー
        /// </summary>
        /// <param name="text"></param>
        /// <param name="degree"></param>
        public void SetExplanationText(int kind, string text, double degree)
        {
            if (degree < 0) return;
            if (degree >= 360) return;

            // typo(Explanation)だけどいいやもう
            Explation3.StringValue = "";

            string[] signs = { "牡羊座", "牡牛座", "双子座", "蟹座", "獅子座", "乙女座", "天秤座", "蠍座", "射手座", "山羊座", "水瓶座", "魚座" };

            if (kind == 0)
            {
                // 天体のマウスオーバー、サビアンあり
                int absoluteStart = (int)degree;
                int absoluteEnd = absoluteStart + 1;
                int sabianStart = (int)(absoluteStart % 30);
                int sabianEnd = (int)(absoluteEnd % 30);
                int signKey = (int)(absoluteStart / 30);

                //Explanation2.InsertText(new NSString(text));
                string sabianStartTxt = signs[signKey] + sabianStart.ToString();
                string sabianEndTxt = signs[signKey] + sabianEnd.ToString();
                string sabianString = sabians[sabianStart]?.text;
                Explation3.StringValue = text + " " + String.Format("{0}-{1}({2}-{3})",
                    absoluteStart.ToString(), absoluteEnd.ToString(),
                    sabianStartTxt, sabianEndTxt
                    ) + ":\n" + sabianString;
                /*
                Explanation2.InsertText(new NSString(String.Format("{0}-{1}({2}-{3})",
                    absoluteStart.ToString(), absoluteEnd.ToString(),
                    sabianStartTxt, sabianEndTxt
                    ) + ":\n" + sabianString));
                */

            } else
            {
                // アスペクトのマウスオーバー、サビアンなし
                Explation3.StringValue = text + " " + degree.ToString();
                //Explanation2.InsertText(new NSString(text));

            }


        }

        public void SetSabianText(int degree)
        {
            if (degree < 0) return;
            if (degree >= 360) return;

            int absoluteStart = (int)degree;
            int absoluteEnd = absoluteStart + 1;
            int sabianStart = (int)(absoluteStart % 30);
            int sabianEnd = (int)(absoluteEnd % 30);
            int sabianKey = (int)(absoluteStart / 30);

            string[] signs = { "牡羊座", "牡牛座", "双子座", "蟹座", "獅子座", "乙女座", "天秤座", "蠍座", "射手座", "山羊座", "水瓶座", "魚座" };

            string sabianStartTxt = signs[sabianKey] + sabianStart.ToString();
            string sabianEndTxt = signs[sabianKey] + sabianEnd.ToString();
            string sabianString = sabians[sabianStart]?.text;
            sabianLabel.StringValue = String.Format("{0}-{1}({2}-{3})",
                absoluteStart.ToString(), absoluteEnd.ToString(),
                sabianStartTxt, sabianEndTxt
                ) + ":" + sabianString;


        }

        public void SabianClear()
        {
            sabianLabel.StringValue = "";
        }

        public bool ShowSaveAsSheet { get; set; } = true;
        public void SaveChartImage()
        {
            var rect = new CGRect(horoscopeCanvas.Frame.X, horoscopeCanvas.Frame.Y, horoscopeCanvas.Frame.Width, horoscopeCanvas.Frame.Width);
            var rep = horoscopeCanvas.Subviews[0].BitmapImageRepForCachingDisplayInRect(rect);
            this.View.CacheDisplay(rect, rep);
            NSImage img = new NSImage(horoscopeCanvas.Subviews[0].Bounds.Size);
            img.AddRepresentation(rep);

            //SKImage sImg = new SKImage();

            SKImage simg = MacExtensions.ToSKImage(img);

            var dlg = new NSSavePanel();
            dlg.Title = "horoscope";
            dlg.AllowedFileTypes = new string[] { "png", "jpg" };
            if (dlg.RunModal() == 1)
            {
                var url = dlg.Url;

                if (url != null)
                {
                    var path = url.Path;
                    using (var stream = File.Create(path))
                    {
                        SKData data;
                        if (path.EndsWith(".jpg"))
                        {
                            data = simg.Encode(SKEncodedImageFormat.Jpeg, 100);
                        }
                        else
                        {
                            data = simg.Encode(SKEncodedImageFormat.Png, 100);
                        }
                        data.SaveTo(stream);
                    }
                }
            }


            /*
            using (var image = currentSurface.Snapshot())
            {
                using (var data = image.Encode(SKEncodedImageFormat.Png, 80))
                {
                    using (var stream = File.OpenWrite(Path.Combine(Util.root, "horoscope.png")))
                    {
                        // save the data to a stream
                        data.SaveTo(stream);
                    }
                }
            }
            */
        }

        public void CurrentChart()
        {
            edata1.SetDateTime(DateTime.Now);
            RefreshEventBox(0, edata1);

            // ReCalc/ReRenderはこっちで
            Chart1E1();
        }

        public void Chart1U1()
        {
            appDelegate.bands = 1;
            calcTargetUser[0] = ETargetUser.USER1;
            ReCalc();
            ReRender();
        }

        public void Chart1U2()
        {
            appDelegate.bands = 1;
            calcTargetUser[0] = ETargetUser.USER2;
            ReCalc();
            ReRender();
        }

        public void Chart1E1()
        {
            appDelegate.bands = 1;
            calcTargetUser[0] = ETargetUser.EVENT1;
            ReCalc();
            ReRender();
        }

        public void Chart1E2()
        {
            appDelegate.bands = 1;
            calcTargetUser[0] = ETargetUser.EVENT2;
            ReCalc();
            ReRender();
        }

        public void Chart2UU()
        {
            appDelegate.bands = 2;
            appDelegate.secondBand = AppDelegate.BandKind.NATAL;
            calcTargetUser[0] = ETargetUser.USER1;
            calcTargetUser[1] = ETargetUser.USER2;
            ReCalc();
            ReRender();
        }

        public void Chart2UE()
        {
            appDelegate.bands = 2;
            appDelegate.secondBand = AppDelegate.BandKind.TRANSIT;
            calcTargetUser[0] = ETargetUser.USER1;
            calcTargetUser[1] = ETargetUser.EVENT1;
            ReCalc();
            ReRender();
        }

        public void Chart2EE()
        {
            appDelegate.bands = 2;
            appDelegate.secondBand = AppDelegate.BandKind.TRANSIT;
            calcTargetUser[0] = ETargetUser.EVENT1;
            calcTargetUser[1] = ETargetUser.EVENT2;
            ReCalc();
            ReRender();
        }

        public void Chart3NPT()
        {
            appDelegate.bands = 3;
            appDelegate.secondBand = AppDelegate.BandKind.PROGRESS;
            appDelegate.thirdBand = AppDelegate.BandKind.TRANSIT;
            calcTargetUser[0] = ETargetUser.USER1;
            calcTargetUser[1] = ETargetUser.EVENT1;
            calcTargetUser[2] = ETargetUser.EVENT1;
            ReCalc();
            ReRender();
        }

        public void Chart3NNT()
        {
            appDelegate.bands = 3;
            appDelegate.secondBand = AppDelegate.BandKind.NATAL;
            appDelegate.thirdBand = AppDelegate.BandKind.NATAL;
            calcTargetUser[0] = ETargetUser.USER1;
            calcTargetUser[1] = ETargetUser.USER2;
            calcTargetUser[2] = ETargetUser.EVENT1;
            ReCalc();
            ReRender();
        }

        public void Chart3NTT()
        {
            appDelegate.bands = 3;
            appDelegate.secondBand = AppDelegate.BandKind.TRANSIT;
            appDelegate.thirdBand = AppDelegate.BandKind.TRANSIT;
            calcTargetUser[0] = ETargetUser.USER1;
            calcTargetUser[1] = ETargetUser.EVENT1;
            calcTargetUser[2] = ETargetUser.EVENT2;
            ReCalc();
            ReRender();
        }

        public void Chart3NNC()
        {
            appDelegate.bands = 3;
            appDelegate.secondBand = AppDelegate.BandKind.TRANSIT;
            appDelegate.thirdBand = AppDelegate.BandKind.COMPOSIT;
            calcTargetUser[0] = ETargetUser.USER1;
            calcTargetUser[1] = ETargetUser.USER2;
            calcTargetUser[2] = ETargetUser.EVENT2;
            ReCalc();
            ReRender();
        }

        public void SetAspect11()
        {
            NSMenuItem item = appDelegate.GetAspect11();
            if (item.State == NSCellStateValue.On)
            {
                appDelegate.SetAspect11(NSCellStateValue.Off);
                appDelegate.aspect11disp = false;
            }
            else
            {
                appDelegate.SetAspect11(NSCellStateValue.On);
                appDelegate.aspect11disp = true;
            }
            ReRender();

        }

        public void SetAspect12()
        {
            NSMenuItem item = appDelegate.GetAspect12();
            if (item.State == NSCellStateValue.On)
            {
                appDelegate.SetAspect12(NSCellStateValue.Off);
                appDelegate.aspect12disp = false;
            }
            else
            {
                appDelegate.SetAspect12(NSCellStateValue.On);
                appDelegate.aspect12disp = true;
            }
            ReRender();

        }

        public void SetAspect13()
        {
            NSMenuItem item = appDelegate.GetAspect13();
            if (item.State == NSCellStateValue.On)
            {
                appDelegate.SetAspect13(NSCellStateValue.Off);
                appDelegate.aspect13disp = false;
            }
            else
            {
                appDelegate.SetAspect13(NSCellStateValue.On);
                appDelegate.aspect13disp = true;
            }
            ReRender();

        }

        public void SetAspect22()
        {
            NSMenuItem item = appDelegate.GetAspect22();
            if (item.State == NSCellStateValue.On)
            {
                appDelegate.SetAspect22(NSCellStateValue.Off);
                appDelegate.aspect22disp = false;
            }
            else
            {
                appDelegate.SetAspect22(NSCellStateValue.On);
                appDelegate.aspect22disp = true;
            }
            ReRender();

        }

        public void SetAspect23()
        {
            NSMenuItem item = appDelegate.GetAspect23();
            if (item.State == NSCellStateValue.On)
            {
                appDelegate.SetAspect23(NSCellStateValue.Off);
                appDelegate.aspect23disp = false;
            }
            else
            {
                appDelegate.SetAspect23(NSCellStateValue.On);
                appDelegate.aspect23disp = true;
            }
            ReRender();

        }

        public void SetAspect33()
        {
            NSMenuItem item = appDelegate.GetAspect33();
            if (item.State == NSCellStateValue.On)
            {
                appDelegate.SetAspect33(NSCellStateValue.Off);
                appDelegate.aspect33disp = false;
            }
            else
            {
                appDelegate.SetAspect33(NSCellStateValue.On);
                appDelegate.aspect33disp = true;
            }
            ReRender();
        }

        public void AspectAllOn()
        {
            appDelegate.SetAspect11(NSCellStateValue.On);
            appDelegate.SetAspect12(NSCellStateValue.On);
            appDelegate.SetAspect13(NSCellStateValue.On);
            appDelegate.SetAspect22(NSCellStateValue.On);
            appDelegate.SetAspect23(NSCellStateValue.On);
            appDelegate.SetAspect33(NSCellStateValue.On);
            appDelegate.aspect11disp = true;
            appDelegate.aspect12disp = true;
            appDelegate.aspect13disp = true;
            appDelegate.aspect22disp = true;
            appDelegate.aspect23disp = true;
            appDelegate.aspect33disp = true;
            ReRender();
        }

        public void AspectOn(int index)
        {
            if (index == 0)
            {
                appDelegate.SetAspect11(NSCellStateValue.On);
                appDelegate.aspect11disp = true;
            }
            else if (index == 1)
            {
                appDelegate.SetAspect12(NSCellStateValue.On);
                appDelegate.aspect12disp = true;
            }
            else if (index == 2)
            {
                appDelegate.SetAspect13(NSCellStateValue.On);
                appDelegate.aspect13disp = true;
            }
            else if (index == 3)
            {
                appDelegate.SetAspect22(NSCellStateValue.On);
                appDelegate.aspect22disp = true;
            }
            else if (index == 4)
            {
                appDelegate.SetAspect23(NSCellStateValue.On);
                appDelegate.aspect23disp = true;
            }
            else if (index == 5)
            {
                appDelegate.SetAspect33(NSCellStateValue.On);
                appDelegate.aspect33disp = true;
            }

            ReRender();
        }

        public void AspectAllOff()
        {
            appDelegate.SetAspect11(NSCellStateValue.Off);
            appDelegate.SetAspect12(NSCellStateValue.Off);
            appDelegate.SetAspect13(NSCellStateValue.Off);
            appDelegate.SetAspect22(NSCellStateValue.Off);
            appDelegate.SetAspect23(NSCellStateValue.Off);
            appDelegate.SetAspect33(NSCellStateValue.Off);
            appDelegate.aspect11disp = false;
            appDelegate.aspect12disp = false;
            appDelegate.aspect13disp = false;
            appDelegate.aspect22disp = false;
            appDelegate.aspect23disp = false;
            appDelegate.aspect33disp = false;
            ReRender();
        }

        public void AspectOff(int index)
        {
            if (index == 0)
            {
                appDelegate.SetAspect11(NSCellStateValue.Off);
                appDelegate.aspect11disp = false;
            }
            else if (index == 1)
            {
                appDelegate.SetAspect12(NSCellStateValue.Off);
                appDelegate.aspect12disp = false;
            }
            else if (index == 2)
            {
                appDelegate.SetAspect13(NSCellStateValue.Off);
                appDelegate.aspect13disp = false;
            }
            else if (index == 3)
            {
                appDelegate.SetAspect22(NSCellStateValue.Off);
                appDelegate.aspect22disp = false;
            }
            else if (index == 4)
            {
                appDelegate.SetAspect23(NSCellStateValue.Off);
                appDelegate.aspect23disp = false;
            }
            else if (index == 5)
            {
                appDelegate.SetAspect33(NSCellStateValue.Off);
                appDelegate.aspect33disp = false;
            }

            ReRender();
        }

        /// <summary>
        /// どのデータを使うか(u1/u2/e1/u2)
        /// </summary>
        /// <param name="ringIndex">リング番号</param>
        /// <returns></returns>
        public UserData GetTargetUser(int ringIndex)
        {
            UserData targetUser = udata1;
            if (calcTargetUser[ringIndex] == ETargetUser.USER1) targetUser = udata1;
            else if (calcTargetUser[ringIndex] == ETargetUser.USER2) targetUser = udata2;
            else if (calcTargetUser[ringIndex] == ETargetUser.EVENT1) targetUser = edata1;
            else if (calcTargetUser[ringIndex] == ETargetUser.EVENT2) targetUser = edata2;

            return targetUser;
        }

        public override void KeyDown(NSEvent e)
        {

            var flag = e.ModifierFlags;
            //Debug.WriteLine(e.KeyCode);

            if (
                e.KeyCode != 4 &&
                e.KeyCode != 38 &&
                e.KeyCode != 40 &&
                e.KeyCode != 37 &&
                e.KeyCode != 18 &&
                e.KeyCode != 19 &&
                e.KeyCode != 20 &&
                e.KeyCode != 21 &&
                e.KeyCode != 23 &&
                e.KeyCode != 22 &&
                e.KeyCode != 26 &&
                e.KeyCode != 28 &&
                e.KeyCode != 25 &&
                e.KeyCode != 29 &&
                e.KeyCode != 45 &&
                e.KeyCode != 46 &&
                e.KeyCode != 43 &&
                e.KeyCode != 47 &&
                e.KeyCode != 33 &&
                e.KeyCode != 30 &&
                e.KeyCode != 122 &&
                e.KeyCode != 120 &&
                e.KeyCode != 99 &&
                e.KeyCode != 118 &&
                e.KeyCode != 96 &&
                e.KeyCode != 97 &&
                e.KeyCode != 98 &&
                e.KeyCode != 100 &&
                e.KeyCode != 101 &&
                e.KeyCode != 109 &&
                e.KeyCode != 16 &&
                e.KeyCode != 32 &&
                e.KeyCode != 34 &&
                e.KeyCode != 31 &&
                e.KeyCode != 35
                ) return;

            EventKeyCode code = new EventKeyCode(e);
            code.GetEvent();
        }

        public override bool AcceptsFirstResponder()
        {
            return true;
        }

        public override void ViewDidDisappear()
        {
            base.ViewDidDisappear();
        }

        public override void MouseDragged(NSEvent theEvent)
        {
            base.MouseDragged(theEvent);
            ReRender();
        }


        partial void test100(Foundation.NSObject sender)
        {
        }

    }
}
