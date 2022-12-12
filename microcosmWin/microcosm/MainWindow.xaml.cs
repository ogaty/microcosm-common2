using microcosm.Aspect;
using microcosm.calc;
using microcosm.common;
using microcosm.config;
using microcosm.Db;
using microcosm.Planet;
using microcosm.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace microcosm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool trial = false;

        public BaseData baseData;
        public SettingData[] settings = new SettingData[10];
        public SettingData currentSetting;
        public ConfigData configData;
        public TempSetting tempSettings;
        public List<string> sabians;

        public UserData user1data;
        public UserData user2data;
        public UserData event1data;
        public UserData event2data;

        // ringにどのデータを使うか
        // 0:user1 1:user2 2:event1 3:event2
        public ETargetUser[] calcTargetUser = { ETargetUser.EVENT1, ETargetUser.EVENT1, ETargetUser.EVENT1 };


        public Dictionary<int, PlanetData> list1;
        public Dictionary<int, PlanetData> list2;
        public Dictionary<int, PlanetData> list3;

        public double[] houseList1;
        public double[] houseList2;
        public double[] houseList3;

        public AstroCalc calc;
        public Render render;

        public MainWindowViewModel mainWindowVM;
        public RingCanvasViewModel rcanvas;
        public PlanetListViewModel planetListVM;
        public HouseListViewModel houseListVM;
        public ReportViewModel reportVM;

        public SpanWindow spanWindow;
        public TimeSetWindow timeSetWindow;
        public DatabaseWindow databaseWindow;
        public SettingWindow settingWindow;
        public ConfigWindow configWindow;
        public VersionWindow versionWindow;
        public ShortCutWindow shortcutWindow;

        public Dictionary<int, EShortCut> keyEvent;
        public Dictionary<int, EShortCut> keyEventCtrl;
        public EventKeyCode keyCode;
        public ShortCut shortCutData;

        public bool aspect11disp = true;
        public bool aspect12disp = true;
        public bool aspect13disp = true;
        public bool aspect22disp = true;
        public bool aspect23disp = true;
        public bool aspect33disp = true;

        public SpanType currentSpanType = SpanType.UNIT;

        public int plusUnit = 86400;

        public MainWindow(BaseData baseData)
        {
            InitializeComponent();
            this.baseData = baseData;

            // 最終的にはすべてbaseDataに落とし込みたい
            // List<BaseData>にすればtabWindowも可能になるはず
            this.configData = baseData.configData;
            this.settings = baseData.settings;
            this.currentSetting = baseData.currentSetting;
            this.tempSettings = baseData.tempSetting;
            this.sabians = baseData.sabians;

            this.user1data = baseData.user1data;
            this.user2data = baseData.user2data;
            this.event1data = baseData.event1data;
            this.event2data = baseData.event2data;
            this.list1 = baseData.list1;
            this.list2 = baseData.list2;
            this.list3 = baseData.list3;
            this.houseList1 = baseData.houseList1;
            this.houseList2 = baseData.houseList2;
            this.houseList3 = baseData.houseList3;
            this.calc = baseData.calc;

            //DataCalc();
            SetViewModel();
            ShortCutInit();
        }

        public void SetData(BaseData baseData)
        {
            this.baseData = baseData;
        }

        private void DataInit()
        {
        }

        public void DataCalc()
        {
            //calc = new AstroCalc(configData, currentSetting);
        }

        public void SetViewModel()
        {
            rcanvas = new RingCanvasViewModel(configData);
            user1data = new UserData(configData);
            user2data = new UserData(configData);
            event1data = new UserData(configData);
            event2data = new UserData(configData);
            UserBinding ub = new UserBinding(user1data);
            TransitBinding tb = new TransitBinding(user1data);
            mainWindowVM = new MainWindowViewModel()
            {
                userName = user1data.name,
                userBirthStr = ub.birthStr,
                userTimezone = user1data.timezone_str,
                userBirthPlace = user1data.birth_place,
                userLatLng = String.Format("{0:f3} {1:f3}", user1data.lat, user1data.lng),

                user2Name = user1data.name,
                user2BirthStr = ub.birthStr,
                user2Timezone = user1data.timezone_str,
                user2BirthPlace = user1data.birth_place,
                user2LatLng = String.Format("{0:f3} {1:f3}", user1data.lat, user1data.lng),

                transitName = "現在時刻",
                transitBirthStr = tb.birthStr,
                transitTimezone = event1data.timezone_str,
                transitBirthPlace = event1data.birth_place,
                transitLatLng = String.Format("{0:f3} {1:f3}", event1data.lat, event1data.lng),

                transit2Name = "現在時刻",
                transit2BirthStr = tb.birthStr,
                transit2Timezone = event1data.timezone_str,
                transit2BirthPlace = event1data.birth_place,
                transit2LatLng = String.Format("{0:f3} {1:f3}", event1data.lat, event1data.lng),
            };
            List<string> list = new List<string>();
            foreach (var item in settings)
            {
                list.Add(item.dispName);
            }

            mainWindowVM.ReSetChangeSettingList(list);
            changeSettingList.SelectedIndex = 0;
            // 左上、右上表示
            this.DataContext = mainWindowVM;


            mainWindowVM.explanationTxt = "Welcome.";

            keyCode = new EventKeyCode(this);

            //UserEventData edata = CommonData.udata2event(targetUser);
            //List<UserData> listEventData = new List<UserData>();
            //listEventData.Add(user1data);
            //listEventData.Add(user1data);
            //listEventData.Add(event1data);

            // 一番最初のReCalc
            //ReCalc(listEventData);

            DateTime now = DateTime.Now;
            using (StreamWriter sw = new StreamWriter(Util.root() + @"\log.txt", true, Encoding.UTF8))
            {
                sw.WriteLine("recalc done." + now.ToString() + " " + now.Millisecond);
            }

            //左下のviewmodel設定
            //listはsplashで計算済み
            planetListVM = new PlanetListViewModel(this, list1, list2, list3);
            planetList.DataContext = planetListVM;

            houseListVM = new HouseListViewModel(configData.decimalDisp, houseList1, houseList2, houseList3);
            cuspList.DataContext = houseListVM;

            //explanation.DataContext = mainWindowVM;

            reportVM = new ReportViewModel(list1, houseList1);
            houseDown.DataContext = reportVM;
            houseRight.DataContext = reportVM;
            houseUp.DataContext = reportVM;
            houseLeft.DataContext = reportVM;
            signFire.DataContext = reportVM;
            signEarth.DataContext = reportVM;
            signAir.DataContext = reportVM;
            signWater.DataContext = reportVM;
            signCardinal.DataContext = reportVM;
            signFixed.DataContext = reportVM;
            signMutable.DataContext = reportVM;
            /*
            houseAngular.DataContext = reportVM;
            houseSuccedent.DataContext = reportVM;
            houseCadent.DataContext = reportVM;
            */

            RefreshSettingBox();

            render = new Render(configData, rcanvas, ringCanvas, ringStack, tempSettings);
            using (StreamWriter sw = new StreamWriter(Util.root() + @"\log.txt", true, Encoding.UTF8))
            {
                sw.WriteLine("render done." + now.ToString() + " " + now.Millisecond);
            }
            render.mainWindow = this;
        }

        public void ShortCutInit()
        {
            keyEvent = new Dictionary<int, EShortCut>();
            keyEventCtrl = new Dictionary<int, EShortCut>();

            string root = Util.root();
            string systemDirName = root + @"\system";

            string fileName = systemDirName + @"\shortcut.json";
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    StreamReader sr = new StreamReader(fs);
                    string jsonData = sr.ReadToEnd();
                    shortCutData = JsonSerializer.Deserialize<ShortCut>(jsonData);
                }
            }
            catch
            {
                shortCutData = new ShortCut();

                string shortCutJson = JsonSerializer.Serialize(shortCutData, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true,
                });
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(shortCutJson);
                    sw.Close();
                }
            }

            keyEventCtrl.Add(4, shortCutData.ctrlH);
            keyEventCtrl.Add(38, shortCutData.ctrlJ);
            keyEventCtrl.Add(40, shortCutData.ctrlK);
            keyEventCtrl.Add(37, shortCutData.ctrlL);
            keyEventCtrl.Add(45, shortCutData.ctrlN);
            keyEventCtrl.Add(46, shortCutData.ctrlM);
            keyEventCtrl.Add(43, shortCutData.ctrlComma);
            keyEventCtrl.Add(47, shortCutData.ctrlDot);
            keyEventCtrl.Add(33, shortCutData.ctrlOpenBracket);
            keyEventCtrl.Add(30, shortCutData.ctrlCloseBracket);
            keyEventCtrl.Add(18, shortCutData.ctrl1);
            keyEventCtrl.Add(19, shortCutData.ctrl2);
            keyEventCtrl.Add(20, shortCutData.ctrl3);
            keyEventCtrl.Add(21, shortCutData.ctrl4);
            keyEventCtrl.Add(23, shortCutData.ctrl5);
            keyEventCtrl.Add(22, shortCutData.ctrl6);
            keyEventCtrl.Add(26, shortCutData.ctrl7);
            keyEventCtrl.Add(28, shortCutData.ctrl8);
            keyEventCtrl.Add(25, shortCutData.ctrl9);
            keyEventCtrl.Add(29, shortCutData.ctrl0);
            keyEvent.Add(122, shortCutData.F1);
            keyEvent.Add(120, shortCutData.F2);
            keyEvent.Add(99, shortCutData.F3);
            keyEvent.Add(118, shortCutData.F4);
            keyEvent.Add(96, shortCutData.F5);
            keyEvent.Add(97, shortCutData.F6);
            keyEvent.Add(98, shortCutData.F7);
            keyEvent.Add(100, shortCutData.F8);
            keyEvent.Add(101, shortCutData.F9);
            keyEvent.Add(109, shortCutData.F10);

            var ctrlH = new RoutedUICommand("ctrlH", "ctrlH", typeof(MainWindow));
            var ctrlJ = new RoutedUICommand("ctrlJ", "ctrlJ", typeof(MainWindow));
            var ctrlK = new RoutedUICommand("ctrlK", "ctrlK", typeof(MainWindow));
            var ctrlL = new RoutedUICommand("ctrlL", "ctrlL", typeof(MainWindow));
            var ctrlN = new RoutedUICommand("ctrlN", "ctrlN", typeof(MainWindow));
            var ctrlM = new RoutedUICommand("ctrlM", "ctrlM", typeof(MainWindow));
            var ctrlComma = new RoutedUICommand("ctrlComma", "ctrlComma", typeof(MainWindow));
            var ctrlDot = new RoutedUICommand("ctrlDot", "ctrlDot", typeof(MainWindow));
            var ctrlOpenBracket = new RoutedUICommand("ctrlOpenBracket", "ctrlOpenBracket", typeof(MainWindow));
            var ctrlCloseBracket = new RoutedUICommand("ctrlCloseBracket", "ctrlCloseBracket", typeof(MainWindow));
            var ctrl0 = new RoutedUICommand("ctrl0", "ctrl0", typeof(MainWindow));
            var ctrl1 = new RoutedUICommand("ctrl1", "ctrl1", typeof(MainWindow));
            var ctrl2 = new RoutedUICommand("ctrl2", "ctrl2", typeof(MainWindow));
            var ctrl3 = new RoutedUICommand("ctrl3", "ctrl3", typeof(MainWindow));
            var ctrl4 = new RoutedUICommand("ctrl4", "ctrl4", typeof(MainWindow));
            var ctrl5 = new RoutedUICommand("ctrl5", "ctrl5", typeof(MainWindow));
            var ctrl6 = new RoutedUICommand("ctrl6", "ctrl6", typeof(MainWindow));
            var ctrl7 = new RoutedUICommand("ctrl7", "ctrl7", typeof(MainWindow));
            var ctrl8 = new RoutedUICommand("ctrl8", "ctrl8", typeof(MainWindow));
            var ctrl9 = new RoutedUICommand("ctrl9", "ctrl9", typeof(MainWindow));
            var f1 = new RoutedUICommand("F1", "F1", typeof(MainWindow));
            var f2 = new RoutedUICommand("F2", "F2", typeof(MainWindow));
            var f3 = new RoutedUICommand("F3", "F3", typeof(MainWindow));
            var f4 = new RoutedUICommand("F4", "F4", typeof(MainWindow));
            var f5 = new RoutedUICommand("F5", "F5", typeof(MainWindow));
            var f6 = new RoutedUICommand("F6", "F6", typeof(MainWindow));
            var f7 = new RoutedUICommand("F7", "F7", typeof(MainWindow));
            var f8 = new RoutedUICommand("F8", "F8", typeof(MainWindow));
            var f9 = new RoutedUICommand("F9", "F9", typeof(MainWindow));
            var f10 = new RoutedUICommand("F10", "F10", typeof(MainWindow));

            CommandBindings.Add(new CommandBinding(ctrlH, CtrlH));
            CommandBindings.Add(new CommandBinding(ctrlJ, CtrlJ));
            CommandBindings.Add(new CommandBinding(ctrlK, CtrlK));
            CommandBindings.Add(new CommandBinding(ctrlL, CtrlL));
            CommandBindings.Add(new CommandBinding(ctrlN, CtrlN));
            CommandBindings.Add(new CommandBinding(ctrlM, CtrlM));
            CommandBindings.Add(new CommandBinding(ctrlComma, CtrlCommma));
            CommandBindings.Add(new CommandBinding(ctrlDot, CtrlDot));
            CommandBindings.Add(new CommandBinding(ctrlOpenBracket, CtrlOpenBracket));
            CommandBindings.Add(new CommandBinding(ctrlCloseBracket, CtrlCloseBracket));
            CommandBindings.Add(new CommandBinding(ctrl0, Ctrl0));
            CommandBindings.Add(new CommandBinding(ctrl1, Ctrl1));
            CommandBindings.Add(new CommandBinding(ctrl2, Ctrl2));
            CommandBindings.Add(new CommandBinding(ctrl3, Ctrl3));
            CommandBindings.Add(new CommandBinding(ctrl4, Ctrl4));
            CommandBindings.Add(new CommandBinding(ctrl5, Ctrl5));
            CommandBindings.Add(new CommandBinding(ctrl6, Ctrl6));
            CommandBindings.Add(new CommandBinding(ctrl7, Ctrl7));
            CommandBindings.Add(new CommandBinding(ctrl8, Ctrl8));
            CommandBindings.Add(new CommandBinding(ctrl9, Ctrl9));
            CommandBindings.Add(new CommandBinding(f1, F1));
            CommandBindings.Add(new CommandBinding(f2, F2));
            CommandBindings.Add(new CommandBinding(f3, F3));
            CommandBindings.Add(new CommandBinding(f4, F4));
            CommandBindings.Add(new CommandBinding(f5, F5));
            CommandBindings.Add(new CommandBinding(f6, F6));
            CommandBindings.Add(new CommandBinding(f7, F7));
            CommandBindings.Add(new CommandBinding(f8, F8));
            CommandBindings.Add(new CommandBinding(f9, F9));
            CommandBindings.Add(new CommandBinding(f10, F10));


            KeyGesture h = new KeyGesture(Key.H, ModifierKeys.Control);
            KeyGesture j = new KeyGesture(Key.J, ModifierKeys.Control);
            KeyGesture k = new KeyGesture(Key.K, ModifierKeys.Control);
            KeyGesture l = new KeyGesture(Key.L, ModifierKeys.Control);
            KeyGesture n = new KeyGesture(Key.N, ModifierKeys.Control);
            KeyGesture m = new KeyGesture(Key.M, ModifierKeys.Control);
            KeyGesture commma = new KeyGesture(Key.OemComma, ModifierKeys.Control);
            KeyGesture dot = new KeyGesture(Key.OemPeriod, ModifierKeys.Control);
            KeyGesture openBracket = new KeyGesture(Key.OemOpenBrackets, ModifierKeys.Control);
            KeyGesture closeBracket = new KeyGesture(Key.OemCloseBrackets, ModifierKeys.Control);
            KeyGesture d0 = new KeyGesture(Key.D0, ModifierKeys.Control);
            KeyGesture d1 = new KeyGesture(Key.D1, ModifierKeys.Control);
            KeyGesture d2 = new KeyGesture(Key.D2, ModifierKeys.Control);
            KeyGesture d3 = new KeyGesture(Key.D3, ModifierKeys.Control);
            KeyGesture d4 = new KeyGesture(Key.D4, ModifierKeys.Control);
            KeyGesture d5 = new KeyGesture(Key.D5, ModifierKeys.Control);
            KeyGesture d6 = new KeyGesture(Key.D6, ModifierKeys.Control);
            KeyGesture d7 = new KeyGesture(Key.D7, ModifierKeys.Control);
            KeyGesture d8 = new KeyGesture(Key.D8, ModifierKeys.Control);
            KeyGesture d9 = new KeyGesture(Key.D9, ModifierKeys.Control);
            KeyGesture f1k = new KeyGesture(Key.F1);
            KeyGesture f2k = new KeyGesture(Key.F2);
            KeyGesture f3k = new KeyGesture(Key.F3);
            KeyGesture f4k = new KeyGesture(Key.F4);
            KeyGesture f5k = new KeyGesture(Key.F5);
            KeyGesture f6k = new KeyGesture(Key.F6);
            KeyGesture f7k = new KeyGesture(Key.F7);
            KeyGesture f8k = new KeyGesture(Key.F8);
            KeyGesture f9k = new KeyGesture(Key.F9);
            KeyGesture f10k = new KeyGesture(Key.F10);

            InputBindings.Add(new KeyBinding(ctrlH, h));
            InputBindings.Add(new KeyBinding(ctrlJ, j));
            InputBindings.Add(new KeyBinding(ctrlK, k));
            InputBindings.Add(new KeyBinding(ctrlL, l));
            InputBindings.Add(new KeyBinding(ctrlN, n));
            InputBindings.Add(new KeyBinding(ctrlM, m));
            InputBindings.Add(new KeyBinding(ctrlComma, commma));
            InputBindings.Add(new KeyBinding(ctrlDot, dot));
            InputBindings.Add(new KeyBinding(ctrlOpenBracket, openBracket));
            InputBindings.Add(new KeyBinding(ctrlCloseBracket, closeBracket));
            InputBindings.Add(new KeyBinding(ctrl0, d0));
            InputBindings.Add(new KeyBinding(ctrl1, d1));
            InputBindings.Add(new KeyBinding(ctrl2, d2));
            InputBindings.Add(new KeyBinding(ctrl3, d3));
            InputBindings.Add(new KeyBinding(ctrl4, d4));
            InputBindings.Add(new KeyBinding(ctrl5, d5));
            InputBindings.Add(new KeyBinding(ctrl6, d6));
            InputBindings.Add(new KeyBinding(ctrl7, d7));
            InputBindings.Add(new KeyBinding(ctrl8, d8));
            InputBindings.Add(new KeyBinding(ctrl9, d9));
            InputBindings.Add(new KeyBinding(f1, f1k));
            InputBindings.Add(new KeyBinding(f2, f2k));
            InputBindings.Add(new KeyBinding(f3, f3k));
            InputBindings.Add(new KeyBinding(f4, f4k));
            InputBindings.Add(new KeyBinding(f5, f5k));
            InputBindings.Add(new KeyBinding(f6, f6k));
            InputBindings.Add(new KeyBinding(f7, f7k));
            InputBindings.Add(new KeyBinding(f8, f8k));
            InputBindings.Add(new KeyBinding(f9, f9k));
            InputBindings.Add(new KeyBinding(f10, f10k));

        }

        /// <summary>
        /// 再計算
        /// </summary>
        public void ReCalc()
        {
#if DEBUG
            DateTime startDt = DateTime.Now;
#endif

            List<UserData> listEventData = new List<UserData>();
            listEventData.Add(GetTargetUser(0));
            listEventData.Add(GetTargetUser(1));
            listEventData.Add(GetTargetUser(2));
            ReCalc(listEventData);
#if DEBUG
            DateTime endDt = DateTime.Now;
            TimeSpan ts = endDt - startDt; // 時間の差分を取得
            Console.WriteLine("Recalc " + ts.TotalSeconds + " sec"); // 経過時間（秒）
#endif

        }


        /// <summary>
        /// list1, list2, list3を再計算
        /// 一重円でも3つ入れること
        /// 初期バージョンではNPTだけにしようか
        /// </summary>
        /// <param name="listEventData"></param>
        public void ReCalc(
                List<UserData> listEventData
            )
        {
            if (listEventData.Count < 3)
            {
                Debug.WriteLine("count error");
                throw new Exception("count error");
            }

            calc.configData = configData;
            calc.currentSetting = currentSetting;
            // ここから先udata,edataは使わない
            // list1Data～list3Dataを使う
            UserData list1UserData = listEventData[0];
            UserData list2UserData = listEventData[1];
            UserData list3UserData = listEventData[2];

            //ring1計算
            if (tempSettings.firstBand == TempSetting.BandKind.PROGRESS)
            {
                // プログレスの場合
                // ありえないのでnatalで
                list1 = calc.PositionCalc(list1UserData.GetBirthDateTime(),
                    list1UserData.lat, list1UserData.lng, configData.houseCalc, 0);
                houseList1 = calc.CuspCalc(list1UserData.GetBirthDateTime(),
                    list1UserData.timezone, list1UserData.lat, list1UserData.lng, configData.houseCalc);
            }
            else
            {
                // natal or transit
                if (configData.sidereal == Esidereal.DRACONIC)
                {
                    list1 = calc.DraconicPositionCalc(list1UserData.GetBirthDateTime(), list1UserData.lat, list1UserData.lng, configData.houseCalc, 0);
                }
                else
                {
                    list1 = calc.PositionCalc(list1UserData.GetBirthDateTime(),
                        list1UserData.lat, list1UserData.lng, configData.houseCalc, 0);
                }
                houseList1 = calc.CuspCalc(list1UserData.GetBirthDateTime(),
                    list1UserData.timezone, list1UserData.lat, list1UserData.lng, configData.houseCalc);
                if (configData.sidereal == Esidereal.DRACONIC)
                {
                    if (configData.nodeCalc == ENodeCalc.TRUE)
                    {
                        houseList1.Select(h => {
                            h -= list1[CommonData.ZODIAC_DH_TRUENODE].absolute_position;
                            if (h < 0) h += 360;
                            return h;
                        });
                    }
                    else
                    {
                        houseList1.Select(h => {
                            h -= list1[CommonData.ZODIAC_DH_MEANNODE].absolute_position;
                            if (h < 0) h += 360;
                            return h;
                        });
                    }
                }
                list1[CommonData.ZODIAC_ASC] = new PlanetData
                {
                    no = CommonData.ZODIAC_ASC,
                    absolute_position = houseList1[1],
                    speed = 0,
                    aspects = new List<AspectInfo>(),
                    secondAspects = new List<AspectInfo>(),
                    thirdAspects = new List<AspectInfo>(),
                    sensitive = true,
                    isDisp = currentSetting.dispPlanetAsc == 1,
                    isAspectDisp = currentSetting.dispAspectPlanetAsc == 1
                };
                list1[CommonData.ZODIAC_MC] = new PlanetData
                {
                    no = CommonData.ZODIAC_MC,
                    absolute_position = houseList1[10],
                    speed = 0,
                    aspects = new List<AspectInfo>(),
                    secondAspects = new List<AspectInfo>(),
                    thirdAspects = new List<AspectInfo>(),
                    sensitive = true,
                    isDisp = currentSetting.dispPlanetMc == 1,
                    isAspectDisp = currentSetting.dispAspectPlanetMc == 1
                };
            }
            if (tempSettings.secondBand == TempSetting.BandKind.PROGRESS)
            {
                // progresはlist1とlist3の時刻で固定
                // progresではDraconicを無効化させる
                list2 = calc.Progress(list1, list1UserData, list3UserData.GetBirthDateTime(), list1UserData.timezone, list1UserData.lat, list1UserData.lng);
                if (configData.progression == EProgression.SECONDARY)
                {
                    houseList2 = calc.SecondaryProgressionHouseCalc(houseList1, list1, list1UserData.GetBirthDateTime(), list3UserData.GetBirthDateTime(), list1UserData.lat, list1UserData.lng, list1UserData.timezone);
                }
                else if (configData.progression == EProgression.PRIMARY)
                {
                    houseList2 = calc.PrimaryProgressionHouseCalc(houseList1, list1UserData.GetBirthDateTime(), list3UserData.GetBirthDateTime());
                }
                else if (configData.progression == EProgression.SOLAR)
                {
                    houseList2 = calc.SolarArcHouseCalc(list1[0].absolute_position, houseList1, list1UserData.GetBirthDateTime(), list3UserData.GetBirthDateTime(), list1UserData.timezone);
                }
                else if (configData.progression == EProgression.CPS)
                {
                    houseList2 = calc.CompositProgressionHouseCalc( houseList1, list1, list1UserData.GetBirthDateTime(), list3UserData.GetBirthDateTime(), list1UserData.lat, list1UserData.lng, list1UserData.timezone);
                }
                else
                {
                    houseList2 = calc.CuspCalc(list1UserData.GetBirthDateTime(),
                            list1UserData.timezone, list3UserData.lat, list3UserData.lng, configData.houseCalc);
                }

                list2[CommonData.ZODIAC_ASC] = new PlanetData
                {
                    no = CommonData.ZODIAC_ASC,
                    absolute_position = houseList2[1],
                    speed = 0,
                    aspects = new List<AspectInfo>(),
                    secondAspects = new List<AspectInfo>(),
                    thirdAspects = new List<AspectInfo>(),
                    sensitive = true
                };
                list2[CommonData.ZODIAC_MC] = new PlanetData
                {
                    no = CommonData.ZODIAC_MC,
                    absolute_position = houseList2[10],
                    speed = 0,
                    aspects = new List<AspectInfo>(),
                    secondAspects = new List<AspectInfo>(),
                    thirdAspects = new List<AspectInfo>(),
                    sensitive = true
                };
            }
            else
            {
                // natal or transit
                if (configData.sidereal == Esidereal.DRACONIC)
                {
                    list2 = calc.DraconicPositionCalc(list2UserData.GetBirthDateTime(), list2UserData.lat, list2UserData.lng, configData.houseCalc, 0);
                }
                else
                {
                    list2 = calc.PositionCalc(list2UserData.GetBirthDateTime(),
                        list2UserData.lat, list2UserData.lng, configData.houseCalc, 1);
                }

                houseList2 = calc.CuspCalc(list1UserData.GetBirthDateTime(),
                        list3UserData.timezone, list3UserData.lat, list3UserData.lng, configData.houseCalc);
                if (configData.sidereal == Esidereal.DRACONIC)
                {
                    if (configData.nodeCalc == ENodeCalc.TRUE)
                    {
                        houseList2.Select(h => {
                            h -= list2[CommonData.ZODIAC_DH_TRUENODE].absolute_position;
                            if (h < 0) h += 360;
                            return h;
                        });
                    }
                    else
                    {
                        houseList2.Select(h => {
                            h -= list2[CommonData.ZODIAC_DH_MEANNODE].absolute_position;
                            if (h < 0) h += 360;
                            return h;
                        });
                    }
                }
                list2[CommonData.ZODIAC_ASC] = new PlanetData
                {
                    no = CommonData.ZODIAC_ASC,
                    absolute_position = houseList2[1],
                    speed = 0,
                    aspects = new List<AspectInfo>(),
                    secondAspects = new List<AspectInfo>(),
                    thirdAspects = new List<AspectInfo>(),
                    sensitive = true
                };
                list2[CommonData.ZODIAC_MC] = new PlanetData
                {
                    no = CommonData.ZODIAC_MC,
                    absolute_position = houseList2[10],
                    speed = 0,
                    aspects = new List<AspectInfo>(),
                    secondAspects = new List<AspectInfo>(),
                    thirdAspects = new List<AspectInfo>(),
                    sensitive = true
                };
            }

            if (tempSettings.thirdBand == TempSetting.BandKind.PROGRESS)
            {
                list3 = calc.Progress(list1, list1UserData, list3UserData.GetBirthDateTime(), list1UserData.timezone, list1UserData.lat, list1UserData.lng);
                houseList3 = calc.CuspCalc(list3UserData.GetBirthDateTime(),
                            list3UserData.timezone, list1UserData.lat, list1UserData.lng, configData.houseCalc);
                list3[CommonData.ZODIAC_ASC] = new PlanetData
                {
                    no = CommonData.ZODIAC_ASC,
                    absolute_position = houseList3[1],
                    speed = 0,
                    aspects = new List<AspectInfo>(),
                    secondAspects = new List<AspectInfo>(),
                    thirdAspects = new List<AspectInfo>(),
                    sensitive = true
                };
                list3[CommonData.ZODIAC_MC] = new PlanetData
                {
                    no = CommonData.ZODIAC_MC,
                    absolute_position = houseList3[10],
                    speed = 0,
                    aspects = new List<AspectInfo>(),
                    secondAspects = new List<AspectInfo>(),
                    thirdAspects = new List<AspectInfo>(),
                    sensitive = true
                };

            }
            if (tempSettings.thirdBand == TempSetting.BandKind.COMPOSIT)
            {
                list3 = calc.PositionCalcComposit(list1, list2);
            }
            else
            {
                // natal or transit
                if (configData.sidereal == Esidereal.DRACONIC)
                {
                    list3 = calc.DraconicPositionCalc(list3UserData.GetBirthDateTime(), list3UserData.lat, list3UserData.lng, configData.houseCalc, 0);
                }
                else
                {
                    list3 = calc.PositionCalc(list3UserData.GetBirthDateTime(),
                        list3UserData.lat, list3UserData.lng, configData.houseCalc, 2);
                }

                houseList3 = calc.CuspCalc(list3UserData.GetBirthDateTime(),
                            list3UserData.timezone, list1UserData.lat, list1UserData.lng, configData.houseCalc);
                if (configData.sidereal == Esidereal.DRACONIC)
                {
                    if (configData.nodeCalc == ENodeCalc.TRUE)
                    {
                        houseList3.Select(h => {
                            h -= list3[CommonData.ZODIAC_DH_TRUENODE].absolute_position;
                            if (h < 0) h += 360;
                            return h;
                        });
                    }
                    else
                    {
                        houseList3.Select(h => {
                            h -= list3[CommonData.ZODIAC_DH_MEANNODE].absolute_position;
                            if (h < 0) h += 360;
                            return h;
                        });
                    }
                }
                list3[CommonData.ZODIAC_ASC] = new PlanetData
                {
                    no = CommonData.ZODIAC_ASC,
                    absolute_position = houseList3[1],
                    speed = 0,
                    aspects = new List<AspectInfo>(),
                    secondAspects = new List<AspectInfo>(),
                    thirdAspects = new List<AspectInfo>(),
                    sensitive = true
                };
                list3[CommonData.ZODIAC_MC] = new PlanetData
                {
                    no = CommonData.ZODIAC_MC,
                    absolute_position = houseList3[10],
                    speed = 0,
                    aspects = new List<AspectInfo>(),
                    secondAspects = new List<AspectInfo>(),
                    thirdAspects = new List<AspectInfo>(),
                    sensitive = true
                };
            }


            AspectCalc aspect = new AspectCalc();
            list1 = aspect.AspectCalcSame(currentSetting, list1);
            list1 = aspect.AspectCalcOther(currentSetting, list1, list2, 3);
            list1 = aspect.AspectCalcOther(currentSetting, list1, list3, 4);
            //list1 = aspect.AspectCalcOther(currentSetting, list1, list4, 9);
            list2 = aspect.AspectCalcSame(currentSetting, list2);
            list2 = aspect.AspectCalcOther(currentSetting, list2, list3, 5);
            //            list2 = aspect.AspectCalcOther(currentSetting, list2, list4, 11);
            list3 = aspect.AspectCalcSame(currentSetting, list3);
            //            list3 = aspect.AspectCalcOther(currentSetting, list3, list4, 13);




        }

        public void Chart1U1()
        {
            tempSettings.bands = 1;
            tempSettings.firstBand = TempSetting.BandKind.NATAL;
            calcTargetUser[0] = ETargetUser.USER1;
            ReCalc();
            ReRender();
        }

        private void SingleRing_Click(object sender, RoutedEventArgs e)
        {
            Chart1U1();
        }

        public void Chart1E1()
        {
            tempSettings.bands = 1;
            tempSettings.firstBand = TempSetting.BandKind.NATAL;
            calcTargetUser[0] = ETargetUser.EVENT1;
            ReCalc();
            ReRender();
        }

        private void SingleRingEvent_Click(object sender, RoutedEventArgs e)
        {
            Chart1E1();
        }

        public void Chart3NPT()
        {
            tempSettings.bands = 3;
            tempSettings.secondBand = TempSetting.BandKind.PROGRESS;
            tempSettings.thirdBand = TempSetting.BandKind.TRANSIT;
            calcTargetUser[0] = ETargetUser.USER1;
            calcTargetUser[1] = ETargetUser.EVENT1; // 無効化される
            calcTargetUser[2] = ETargetUser.EVENT1;
            ReCalc();
            ReRender();
        }

        private void TripleRing_Click(object sender, RoutedEventArgs e)
        {
            Chart3NPT();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ReRender();
        }

        public void ReRender()
        {
            RefreshSettingBox();

            render.SetCurrentSetting(currentSetting);
            render.SetPlanetList(list1, list2, list3);
            planetListVM.ReRender(list1, list2, list3);
            render.SetHouseList(houseList1, houseList2, houseList3);
            houseListVM.ReRender(configData.decimalDisp, houseList1, houseList2, houseList3);
            reportVM.ReCalcReport(list1, houseList1);
            render.ReRender();
        }

        public void RefreshSettingBox()
        {
            mainWindowVM.targetUser1 = calcTargetUser[0].ToString();
            mainWindowVM.targetUser2 = calcTargetUser[1].ToString();
            mainWindowVM.targetUser3 = calcTargetUser[2].ToString();


            if (configData.progression == EProgression.SOLAR)
            {
                mainWindowVM.progressionCalc = "ソーラーアーク法";
            }
            else if (configData.progression == EProgression.SECONDARY)
            {
                mainWindowVM.progressionCalc = "一日一年法";
            }
            else if (configData.progression == EProgression.PRIMARY)
            {
                mainWindowVM.progressionCalc = "一度一年法";
            }
            else
            {
                mainWindowVM.progressionCalc = "CPS";
            }

            if (configData.sidereal == Esidereal.SIDEREAL)
            {
                mainWindowVM.siderealStr = "SIDEREAL";
            }
            else
            {
                mainWindowVM.siderealStr = "TROPICAL";
            }


            if (configData.houseCalc == EHouseCalc.CAMPANUS)
            {
                mainWindowVM.houseDivide = "CAMPANUS";
            }
            else if (configData.houseCalc == EHouseCalc.EQUAL)
            {
                mainWindowVM.houseDivide = "EQUAL";
            }
            else if (configData.houseCalc == EHouseCalc.KOCH)
            {
                mainWindowVM.houseDivide = "KOCH";
            }
            else if (configData.houseCalc == EHouseCalc.PLACIDUS)
            {
                mainWindowVM.houseDivide = "PLACIDUS";
            }
            else if (configData.houseCalc == EHouseCalc.ZEROARIES)
            {
                mainWindowVM.houseDivide = "Zero Aries";
            }
            if (configData.centric == ECentric.GEO_CENTRIC)
            {
                mainWindowVM.centricMode = "GeoCentric";
            }
            else
            {
                mainWindowVM.centricMode = "HelioCentric";
            }



        }

        private void ChangeSpanButton_Click(object sender, RoutedEventArgs e)
        {
            if (spanWindow == null)
            {
                spanWindow = new SpanWindow(this);
                spanWindow.Owner = this;
            }
            spanWindow.ShowDialog();

        }

        private void TimeSet_Click(object sender, RoutedEventArgs e)
        {
            if (timeSetWindow == null)
            {
                timeSetWindow = new TimeSetWindow(this);
                timeSetWindow.Owner = this;
            }
            timeSetWindow.ShowDialog();
        }

        private void DatabaseOpen_Click(object sender, RoutedEventArgs e)
        {
            if (databaseWindow == null)
            {
                databaseWindow = new DatabaseWindow(this);
            }
            databaseWindow.ShowDialog();
        }

        private void CommonSetting_Click(object sender, RoutedEventArgs e)
        {
            if (configWindow == null)
            {
                configWindow = new ConfigWindow(this);
            }
            configWindow.ShowDialog();
        }

        private void IndividualSetting_Click(object sender, RoutedEventArgs e)
        {
            if (settingWindow == null)
            {
                settingWindow = new SettingWindow(this);
            }
            settingWindow.ShowDialog();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (spanWindow != null)
            {
                spanWindow.Close();
            }
            if (timeSetWindow != null)
            {
                timeSetWindow.Close();
            }
            if (databaseWindow != null)
            {
                databaseWindow.Close();
            }
            if (settingWindow != null)
            {
                settingWindow.Close();
            }

            Application.Current.Shutdown();
        }

        /// <summary>
        /// どのデータを使うか(u1/u2/e1/e2)
        /// </summary>
        /// <param name="index">ringIndex</param>
        /// <returns></returns>
        public UserData GetTargetUser(int ringIndex)
        {
            UserData targetUser;
            if (calcTargetUser[ringIndex] == ETargetUser.USER1)
            {
                targetUser = user1data;
            }
            else if (calcTargetUser[ringIndex] == ETargetUser.USER2)
            {
                targetUser = user2data;
            }
            else if (calcTargetUser[ringIndex] == ETargetUser.EVENT1)
            {
                targetUser = event1data;
            }
            else
            {
                targetUser = event2data;
            }

            return targetUser;
        }


        public void SettingIndexChange(int index)
        {
            currentSetting = settings[index];
            changeSettingList.SelectedIndex = index;
            ReCalc();
            ReRender();
        }

        private void changeSettingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingIndexChange(changeSettingList.SelectedIndex);
        }

        private void Github_Click(object sender, RoutedEventArgs e)
        {
            var proc = new System.Diagnostics.Process();

            proc.StartInfo.FileName = "https://github.com/ogaty/microcosm-common2";
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {

            if (versionWindow == null)
            {
                versionWindow = new VersionWindow(this);
            }
            versionWindow.ShowDialog();
        }

        public void RefreshUserBox(int index, UserData udata)
        {
        }

        /// <summary>
        /// 1重なら表示されている円、3重なら外側をNowにする
        /// </summary>
        public void TimeSetNowCurrentBand()
        {
            ETargetUser currentTime = ETargetUser.USER1;

            if (tempSettings.bands == 1)
            {
                currentTime = calcTargetUser[0];
            }
            else if (tempSettings.bands == 2)
            {
                currentTime = calcTargetUser[1];
            }
            else if (tempSettings.bands == 3)
            {
                currentTime = calcTargetUser[2];
            }
            DateTime now = DateTime.Now;
            if (currentTime == ETargetUser.USER1)
            {
                user1data.SetDateTime(now);
                ReCalc();
                mainWindowVM.ReSet(0, user1data.name, user1data.GetBirthDateTime(), user1data.birth_place, String.Format("{0:f3} {1:f3}", user1data.lat, user1data.lng), user1data.timezone_str);
            }
            else if (currentTime == ETargetUser.USER2)
            {
                user2data.SetDateTime(now);
                ReCalc();
                mainWindowVM.ReSet(1, user2data.name, user2data.GetBirthDateTime(), user2data.birth_place, String.Format("{0:f3} {1:f3}", user2data.lat, user2data.lng), user2data.timezone_str);
            }
            else if (currentTime == ETargetUser.EVENT1)
            {
                event1data.SetDateTime(now);
                ReCalc();
                mainWindowVM.ReSet(2, event1data.name, event1data.GetBirthDateTime(), event1data.birth_place, String.Format("{0:f3} {1:f3}", event1data.lat, event1data.lng), event1data.timezone_str);
            }
            else if (currentTime == ETargetUser.EVENT2)
            {
                event2data.SetDateTime(now);
                ReCalc();
                mainWindowVM.ReSet(3, event2data.name, event2data.GetBirthDateTime(), event2data.birth_place, String.Format("{0:f3} {1:f3}", event2data.lat, event2data.lng), event2data.timezone_str);
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

            if (tempSettings.bands == 1)
            {
                currentTime = calcTargetUser[0];
            }
            else if (tempSettings.bands == 2)
            {
                currentTime = calcTargetUser[1];
            }
            else if (tempSettings.bands == 3)
            {
                currentTime = calcTargetUser[2];
            }
            DateTime now = user1data.GetBirthDateTime();
            if (currentTime == ETargetUser.USER1)
            {
                now = user1data.GetBirthDateTime().AddSeconds(seconds);
                user1data.SetDateTime(now);
                ReCalc();
                mainWindowVM.ReSet(0, user1data.name, user1data.GetBirthDateTime(), user1data.birth_place, String.Format("{0:f3} {1:f3}", user1data.lat, user1data.lng), user1data.timezone_str);
            }
            else if (currentTime == ETargetUser.USER2)
            {
                now = user2data.GetBirthDateTime().AddSeconds(seconds);
                user2data.SetDateTime(now);
                ReCalc();
                mainWindowVM.ReSet(1, user2data.name, user2data.GetBirthDateTime(), user2data.birth_place, String.Format("{0:f3} {1:f3}", user2data.lat, user2data.lng), user2data.timezone_str);
            }
            else if (currentTime == ETargetUser.EVENT1)
            {
                now = event1data.GetBirthDateTime().AddSeconds(seconds);
                event1data.SetDateTime(now);
                ReCalc();
                mainWindowVM.ReSet(2, event1data.name, event1data.GetBirthDateTime(), event1data.birth_place, String.Format("{0:f3} {1:f3}", event1data.lat, event1data.lng), event1data.timezone_str);
            }
            else if (currentTime == ETargetUser.EVENT2)
            {
                now = event2data.GetBirthDateTime().AddSeconds(seconds);
                event2data.SetDateTime(now);
                ReCalc();
                mainWindowVM.ReSet(3, event2data.name, event2data.GetBirthDateTime(), event2data.birth_place, String.Format("{0:f3} {1:f3}", event2data.lat, event2data.lng), event2data.timezone_str);
            }
            ReRender();

        }

        private void TimeNow_Click(object sender, RoutedEventArgs e)
        {
            UserData target = user1data;
            if (NT.SelectedIndex == 0)
            {
                target = user1data;
                target.SetDateTime(DateTime.Now);
                ReCalc();
                mainWindowVM.ReSet(0, target.name, target.GetBirthDateTime(), target.birth_place, String.Format("{0:f3} {1:f3}", target.lat, target.lng), target.timezone_str);
            }
            else if (NT.SelectedIndex == 1)
            {
                target = user2data;
                target.SetDateTime(DateTime.Now);
                ReCalc();
                mainWindowVM.ReSet(1, target.name, target.GetBirthDateTime(), target.birth_place, String.Format("{0:f3} {1:f3}", target.lat, target.lng), target.timezone_str);
            }
            else if (NT.SelectedIndex == 2)
            {
                target = event1data;
                target.SetDateTime(DateTime.Now);
                ReCalc();
                mainWindowVM.ReSet(2, target.name, target.GetBirthDateTime(), target.birth_place, String.Format("{0:f3} {1:f3}", target.lat, target.lng), target.timezone_str);
            }
            else if (NT.SelectedIndex == 3)
            {
                target = event2data;
                target.SetDateTime(DateTime.Now);
                ReCalc();
                mainWindowVM.ReSet(3, target.name, target.GetBirthDateTime(), target.birth_place, String.Format("{0:f3} {1:f3}", target.lat, target.lng), target.timezone_str);
            }
            ReRender();
        }

        private void TimeMinus_Click(object sender, RoutedEventArgs e)
        {
            DateTime now = user1data.GetBirthDateTime();
            if (NT.SelectedIndex == 0)
            {
                now = user1data.GetBirthDateTime().AddSeconds(-1 * plusUnit);
                user1data.SetDateTime(now);
                ReCalc();
                mainWindowVM.ReSet(0, user1data.name, user1data.GetBirthDateTime(), user1data.birth_place, String.Format("{0:f3} {1:f3}", user1data.lat, user1data.lng), user1data.timezone_str);
            }
            else if (NT.SelectedIndex == 1)
            {
                now = user2data.GetBirthDateTime().AddSeconds(-1 * plusUnit);
                user2data.SetDateTime(now);
                ReCalc();
                mainWindowVM.ReSet(1, user2data.name, user2data.GetBirthDateTime(), user2data.birth_place, String.Format("{0:f3} {1:f3}", user2data.lat, user2data.lng), user2data.timezone_str);
            }
            else if (NT.SelectedIndex == 2)
            {
                now = event1data.GetBirthDateTime().AddSeconds(-1 * plusUnit);
                event1data.SetDateTime(now);
                ReCalc();
                mainWindowVM.ReSet(2, event1data.name, event1data.GetBirthDateTime(), event1data.birth_place, String.Format("{0:f3} {1:f3}", event1data.lat, event1data.lng), event1data.timezone_str);
            }
            else if (NT.SelectedIndex == 3)
            {
                now = event2data.GetBirthDateTime().AddSeconds(-1 * plusUnit);
                event2data.SetDateTime(now);
                ReCalc();
                mainWindowVM.ReSet(3, event2data.name, event2data.GetBirthDateTime(), event2data.birth_place, String.Format("{0:f3} {1:f3}", event2data.lat, event2data.lng), event2data.timezone_str);
            }
            ReRender();
        }

        private void TimePlus_Click(object sender, RoutedEventArgs e)
        {
            DateTime now = user1data.GetBirthDateTime();
            if (NT.SelectedIndex == 0)
            {
                now = user1data.GetBirthDateTime().AddSeconds(plusUnit);
                user1data.SetDateTime(now);
                ReCalc();
                mainWindowVM.ReSet(0, user1data.name, user1data.GetBirthDateTime(), user1data.birth_place, String.Format("{0:f3} {1:f3}", user1data.lat, user1data.lng), user1data.timezone_str);
            }
            else if (NT.SelectedIndex == 1)
            {
                now = user2data.GetBirthDateTime().AddSeconds(plusUnit);
                user2data.SetDateTime(now);
                ReCalc();
                mainWindowVM.ReSet(1, user2data.name, user2data.GetBirthDateTime(), user2data.birth_place, String.Format("{0:f3} {1:f3}", user2data.lat, user2data.lng), user2data.timezone_str);
            }
            else if (NT.SelectedIndex == 2)
            {
                now = event1data.GetBirthDateTime().AddSeconds(plusUnit);
                event1data.SetDateTime(now);
                ReCalc();
                mainWindowVM.ReSet(2, event1data.name, event1data.GetBirthDateTime(), event1data.birth_place, String.Format("{0:f3} {1:f3}", event1data.lat, event1data.lng), event1data.timezone_str);
            }
            else if (NT.SelectedIndex == 3)
            {
                now = event2data.GetBirthDateTime().AddSeconds(plusUnit);
                event2data.SetDateTime(now);
                ReCalc();
                mainWindowVM.ReSet(3, event2data.name, event2data.GetBirthDateTime(), event2data.birth_place, String.Format("{0:f3} {1:f3}", event2data.lat, event2data.lng), event2data.timezone_str);
            }
            ReRender();
        }

        private void DatabaseDir_Open(object sender, RoutedEventArgs e)
        {
            var proc = new System.Diagnostics.Process();

            proc.StartInfo.FileName = Util.root() + @"\data";
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }

        private void Addr_Open(object sender, RoutedEventArgs e)
        {
            if (trial)
            {
                MessageBox.Show("トライアル版では使用できません。");
                return;
            }
            var proc = new System.Diagnostics.Process();

            proc.StartInfo.FileName = Util.root() + @"\system\addr.csv";
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }

        public void Chart2UU()
        {
            tempSettings.bands = 2;
            tempSettings.secondBand = TempSetting.BandKind.NATAL;
            calcTargetUser[0] = ETargetUser.USER1;
            calcTargetUser[1] = ETargetUser.USER2;
            ReCalc();
            ReRender();
        }

        public void Chart2UE()
        {
            tempSettings.bands = 2;
            tempSettings.secondBand = TempSetting.BandKind.TRANSIT;
            calcTargetUser[0] = ETargetUser.USER1;
            calcTargetUser[1] = ETargetUser.EVENT1;
            ReCalc();
            ReRender();
        }

        public void Chart2EE()
        {
            tempSettings.bands = 2;
            tempSettings.secondBand = TempSetting.BandKind.TRANSIT;
            calcTargetUser[0] = ETargetUser.EVENT1;
            calcTargetUser[1] = ETargetUser.EVENT2;
            ReCalc();
            ReRender();
        }

        private void DoubleNN_Click(object sender, RoutedEventArgs e)
        {
            if (trial)
            {
                MessageBox.Show("トライアル版では使用できません。");
                return;
            }
            Chart2UU();
        }

        private void DoubleNT_Click(object sender, RoutedEventArgs e)
        {
            if (trial)
            {
                MessageBox.Show("トライアル版では使用できません。");
                return;
            }
            Chart2UE();
        }

        public void Chart1U2()
        {
            if (trial)
            {
                MessageBox.Show("トライアル版では使用できません。");
                return;
            }
            tempSettings.bands = 1;
            tempSettings.firstBand = TempSetting.BandKind.NATAL;
            calcTargetUser[0] = ETargetUser.USER2;
            ReCalc();
            ReRender();

        }
        private void SingleRingUser2_Click(object sender, RoutedEventArgs e)
        {
            Chart1U2();
        }


        public void Chart1E2()
        {
            if (trial)
            {
                MessageBox.Show("トライアル版では使用できません。");
                return;
            }
            tempSettings.bands = 1;
            tempSettings.firstBand = TempSetting.BandKind.NATAL;
            calcTargetUser[0] = ETargetUser.EVENT2;
            ReCalc();
            ReRender();

        }

        private void SingleRingEvent2_Click(object sender, RoutedEventArgs e)
        {
            Chart1E2();
        }

        private void aspect11_Click(object sender, RoutedEventArgs e)
        {
            // winはIsCheckedを勝手に切り替えてくれる
            // のでこの関数に来た時点でひっくり返っている
            // macは切り替えてくれない
            if (aspect11.IsChecked == true)
            {
                aspect11disp = true;
            }
            else
            {
                aspect11disp = false;
            }
            ReRender();
        }

        private void aspect12_Click(object sender, RoutedEventArgs e)
        {
            if (aspect12.IsChecked == true)
            {
                aspect12disp = true;
            }
            else
            {
                aspect12disp = false;
            }
            ReRender();
        }

        private void aspect13_Click(object sender, RoutedEventArgs e)
        {
            if (aspect13.IsChecked == true)
            {
                aspect13disp = true;
            }
            else
            {
                aspect13disp = false;
            }
            ReRender();
        }

        private void aspect22_Click(object sender, RoutedEventArgs e)
        {
            if (aspect22.IsChecked == true)
            {
                aspect22disp = true;
            }
            else
            {
                aspect22disp = false;
            }
            ReRender();
        }

        private void aspect23_Click(object sender, RoutedEventArgs e)
        {
            if (aspect23.IsChecked == true)
            {
                aspect23disp = true;
            }
            else
            {
                aspect23disp = false;
            }
            ReRender();
        }

        private void aspect33_Click(object sender, RoutedEventArgs e)
        {
            if (aspect33.IsChecked == true)
            {
                aspect33disp = true;
            }
            else
            {
                aspect33disp = false;
            }
            ReRender();
        }

        public void AspectAllOff()
        {
            aspect11.IsChecked = false;
            aspect12.IsChecked = false;
            aspect13.IsChecked = false;
            aspect22.IsChecked = false;
            aspect23.IsChecked = false;
            aspect33.IsChecked = false;
            aspect11disp = false;
            aspect12disp = false;
            aspect13disp = false;
            aspect22disp = false;
            aspect23disp = false;
            aspect33disp = false;

            ReRender();
        }

        public void AspectAllOn()
        {
            aspect11.IsChecked = true;
            aspect12.IsChecked = true;
            aspect13.IsChecked = true;
            aspect22.IsChecked = true;
            aspect23.IsChecked = true;
            aspect33.IsChecked = true;
            aspect11disp = true;
            aspect12disp = true;
            aspect13disp = true;
            aspect22disp = true;
            aspect23disp = true;
            aspect33disp = true;

            ReRender();
        }

        public void AspectOff(int index)
        {
            if (index == 0)
            {
                aspect11.IsChecked = false;
                aspect11disp = false;
            }
            else if (index == 1)
            {
                aspect12.IsChecked = false;
                aspect12disp = false;
            }
            else if (index == 2)
            {
                aspect13.IsChecked = false;
                aspect13disp = false;
            }
            else if (index == 3)
            {
                aspect22.IsChecked = false;
                aspect22disp = false;
            }
            else if (index == 4)
            {
                aspect23.IsChecked = false;
                aspect23disp = false;
            }
            else if (index == 5)
            {
                aspect33.IsChecked = false;
                aspect33disp = false;
            }
            ReRender();
        }

        public void AspectOn(int index)
        {
            if (index == 0)
            {
                aspect11.IsChecked = true;
                aspect11disp = true;
            }
            else if (index == 1)
            {
                aspect12.IsChecked = true;
                aspect12disp = true;
            }
            else if (index == 2)
            {
                aspect13.IsChecked = true;
                aspect13disp = true;
            }
            else if (index == 3)
            {
                aspect22.IsChecked = true;
                aspect22disp = true;
            }
            else if (index == 4)
            {
                aspect23.IsChecked = true;
                aspect23disp = true;
            }
            else if (index == 5)
            {
                aspect33.IsChecked = true;
                aspect33disp = true;
            }
            ReRender();
        }
        private void allAspectOff_Click(object sender, RoutedEventArgs e)
        {
            AspectAllOff();
        }

        private void allAspectOn_Click(object sender, RoutedEventArgs e)
        {
            AspectAllOn();
        }

        private void CtrlH(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(4)) keyCode.ProcEvent(keyEventCtrl[4]);
        }

        private void CtrlJ(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(38)) keyCode.ProcEvent(keyEventCtrl[38]);
        }

        private void CtrlK(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(40)) keyCode.ProcEvent(keyEventCtrl[40]);
        }

        private void CtrlL(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(37)) keyCode.ProcEvent(keyEventCtrl[37]);
        }

        private void CtrlN(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(45)) keyCode.ProcEvent(keyEventCtrl[45]);
        }

        private void CtrlM(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(46)) keyCode.ProcEvent(keyEventCtrl[46]);
        }

        private void CtrlY(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(16)) keyCode.ProcEvent(keyEventCtrl[16]);
        }

        private void CtrlU(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(32)) keyCode.ProcEvent(keyEventCtrl[32]);
        }

        private void CtrlI(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(34)) keyCode.ProcEvent(keyEventCtrl[34]);
        }

        private void CtrlO(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(31)) keyCode.ProcEvent(keyEventCtrl[31]);
        }

        private void CtrlP(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(35)) keyCode.ProcEvent(keyEventCtrl[35]);
        }

        private void CtrlCommma(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(43)) keyCode.ProcEvent(keyEventCtrl[43]);
        }

        private void CtrlDot(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(47)) keyCode.ProcEvent(keyEventCtrl[47]);
        }

        private void CtrlOpenBracket(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(33)) keyCode.ProcEvent(keyEventCtrl[33]);
        }

        private void CtrlCloseBracket(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(30)) keyCode.ProcEvent(keyEventCtrl[30]);
        }

        private void Ctrl0(object sender, ExecutedRoutedEventArgs e)
        {
            Debug.WriteLine("0");
            if (keyEventCtrl.ContainsKey(29)) keyCode.ProcEvent(keyEventCtrl[29]);
        }
        private void Ctrl1(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(18)) keyCode.ProcEvent(keyEventCtrl[18]);
        }
        private void Ctrl2(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(19)) keyCode.ProcEvent(keyEventCtrl[19]);
        }
        private void Ctrl3(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(20)) keyCode.ProcEvent(keyEventCtrl[20]);
        }
        private void Ctrl4(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(21)) keyCode.ProcEvent(keyEventCtrl[21]);
        }
        private void Ctrl5(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(23)) keyCode.ProcEvent(keyEventCtrl[23]);
        }
        private void Ctrl6(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(22)) keyCode.ProcEvent(keyEventCtrl[22]);
        }
        private void Ctrl7(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(26)) keyCode.ProcEvent(keyEventCtrl[26]);
        }
        private void Ctrl8(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(28)) keyCode.ProcEvent(keyEventCtrl[28]);
        }
        private void Ctrl9(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEventCtrl.ContainsKey(25)) keyCode.ProcEvent(keyEventCtrl[25]);
        }
        private void F10(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEvent.ContainsKey(109)) keyCode.ProcEvent(keyEvent[109]);
        }
        private void F1(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEvent.ContainsKey(122)) keyCode.ProcEvent(keyEvent[122]);
        }
        private void F2(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEvent.ContainsKey(120)) keyCode.ProcEvent(keyEvent[120]);
        }
        private void F3(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEvent.ContainsKey(99)) keyCode.ProcEvent(keyEvent[99]);
        }
        private void F4(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEvent.ContainsKey(118)) keyCode.ProcEvent(keyEvent[118]);
        }
        private void F5(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEvent.ContainsKey(96)) keyCode.ProcEvent(keyEvent[96]);
        }
        private void F6(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEvent.ContainsKey(97)) keyCode.ProcEvent(keyEvent[97]);
        }
        private void F7(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEvent.ContainsKey(98)) keyCode.ProcEvent(keyEvent[98]);
        }
        private void F8(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEvent.ContainsKey(100)) keyCode.ProcEvent(keyEvent[100]);
        }
        private void F9(object sender, ExecutedRoutedEventArgs e)
        {
            if (keyEvent.ContainsKey(101)) keyCode.ProcEvent(keyEvent[101]);
        }

        private void ShortCut_Click(object sender, RoutedEventArgs e)
        {
            if (shortcutWindow == null)
            {
                shortcutWindow = new ShortCutWindow(this);
            }
            shortcutWindow.ShowDialog();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            using (StreamWriter sw = new StreamWriter(Util.root() + @"\log.txt", false, Encoding.UTF8))
            {
                sw.WriteLine("Init." + now.ToString() + " " + now.Millisecond);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (trial)
            {
                MessageBox.Show("トライアル版では使用できません。");
                return;
            }

            //DrawingVisual dv = new DrawingVisual();
            //DrawingContext dc = dv.RenderOpen();

            // 画像用キャンバスを複製
            Canvas cnvs = ringCanvas;
            cnvs.Background = System.Windows.Media.Brushes.White;
            cnvs.LayoutTransform = null;

            //いらないよね
            //System.Windows.Size oldSize = new System.Windows.Size(ringCanvas.ActualWidth, ringCanvas.ActualHeight);


            System.Windows.Size renderSize = new System.Windows.Size(cnvs.ActualWidth, ringStack.ActualHeight);
            //cnvs.Measure(renderSize);
            // 背景色塗りつぶし
            cnvs.Arrange(new Rect(renderSize));
            RenderTargetBitmap render = new RenderTargetBitmap((Int32)renderSize.Width, (Int32)renderSize.Height, 96, 96, PixelFormats.Default);
            render.Render(cnvs);


            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "horoscope.png";
            sfd.Filter = "pngファイル(*.png)|*.png|jpegファイル(*.jpg)|*.jpg|すべてのファイル(*.*)|*.*";
            sfd.Title = "画像ファイル名を選択してください";

            sfd.ShowDialog();
            if (sfd.FileName != "")
            {
                // 画像生成
                if (sfd.FileName.EndsWith(".jpg"))
                {
                    var enc = new JpegBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(render));

                    string fileName = sfd.FileName;
                    using (FileStream fs = new FileStream(fileName, FileMode.Create))
                    {
                        enc.Save(fs);
                        fs.Close();
                    }
                }
                else
                {
                    var enc = new PngBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(render));

                    string pngFile = sfd.FileName;
                    using (FileStream fs = new FileStream(pngFile, FileMode.Create))
                    {
                        enc.Save(fs);
                        fs.Close();
                    }

                }
            }

            // これいる？
            // ringCanvas.Measure(oldSize);
            // ringCanvas.Arrange(new Rect(oldSize));
        }

        private void test100_Click(object sender, RoutedEventArgs e)
        {
            string fileName = Util.root() + @"\data\aaa.json";

            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                string jsonData = sr.ReadToEnd();
                var option = new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                TestJson jsonObj = JsonSerializer.Deserialize<TestJson>(jsonData, option);

                Debug.WriteLine("done");
            }
        }

        private void Sabian_Open(object sender, RoutedEventArgs e)
        {
            if (trial)
            {
                MessageBox.Show("トライアル版では使用できません。");
                return;
            }
            var proc = new System.Diagnostics.Process();

            proc.StartInfo.FileName = Util.root() + @"\system\sabian.csv";
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }

        private void DoubleTT_Click(object sender, RoutedEventArgs e)
        {
            if (trial)
            {
                MessageBox.Show("トライアル版では使用できません。");
                return;
            }
            Chart2EE();
        }

        public void Chart3NTT()
        {
            tempSettings.bands = 3;
            tempSettings.secondBand = TempSetting.BandKind.TRANSIT;
            tempSettings.thirdBand = TempSetting.BandKind.TRANSIT;
            calcTargetUser[0] = ETargetUser.USER1;
            calcTargetUser[1] = ETargetUser.EVENT1;
            calcTargetUser[2] = ETargetUser.EVENT2;
            ReCalc();
            ReRender();
        }

        public void Chart3NNT()
        {
            tempSettings.bands = 3;
            tempSettings.secondBand = TempSetting.BandKind.NATAL;
            tempSettings.thirdBand = TempSetting.BandKind.TRANSIT;
            calcTargetUser[0] = ETargetUser.USER1;
            calcTargetUser[1] = ETargetUser.USER2;
            calcTargetUser[2] = ETargetUser.EVENT1;
            ReCalc();
            ReRender();
        }

        private void TripleRingNTT_Click(object sender, RoutedEventArgs e)
        {
            if (trial)
            {
                MessageBox.Show("トライアル版では使用できません。");
                return;
            }
            Chart3NTT();
        }

        private void TripleRingNNT_Click(object sender, RoutedEventArgs e)
        {
            if (trial)
            {
                MessageBox.Show("トライアル版では使用できません。");
                return;
            }
            Chart3NNT();
        }

        public void Chart3NNC()
        {
            tempSettings.bands = 3;
            tempSettings.secondBand = TempSetting.BandKind.NATAL;
            tempSettings.thirdBand = TempSetting.BandKind.COMPOSIT;
            calcTargetUser[0] = ETargetUser.USER1;
            calcTargetUser[1] = ETargetUser.USER2;
            calcTargetUser[2] = ETargetUser.EVENT1; // 無効化される
            ReCalc();
            ReRender();
        }

        private void TripleRingNNC_Click(object sender, RoutedEventArgs e)
        {
            if (trial)
            {
                MessageBox.Show("トライアル版では使用できません。");
                return;
            }
            Chart3NNC();
        }

        private void CurrentChart_Click(object sender, RoutedEventArgs e)
        {
            UserData target = event1data;
            target.SetDateTime(DateTime.Now);
            ReCalc();
            mainWindowVM.ReSet(2, target.name, target.GetBirthDateTime(), target.birth_place, String.Format("{0:f3} {1:f3}", target.lat, target.lng), target.timezone_str);

            Chart1E1();
        }

        private void License_Click(object sender, RoutedEventArgs e)
        {
            var proc = new System.Diagnostics.Process();

            proc.StartInfo.FileName = Util.root() + @"\license";
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            var proc = new System.Diagnostics.Process();

            proc.StartInfo.FileName = "https://nimb.ws/M3NK1H";
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }
    }
}
