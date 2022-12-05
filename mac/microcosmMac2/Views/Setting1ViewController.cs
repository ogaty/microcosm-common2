using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using StoreKit;
using microcosmMac2.Views.DataSources;
using System.Diagnostics;
using microcosmMac2.Config;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using microcosmMac2.Common;

namespace microcosmMac2.Views
{
    public partial class Setting1ViewController : AppKit.NSViewController
    {
        #region Constructors

        // Called when created from unmanaged code
        public Setting1ViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public Setting1ViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public Setting1ViewController() : base("Setting1View", NSBundle.MainBundle)
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

            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;

            // Create the Product Table Data Source and populate it
            var DataSource = new SettingDataSource();
            for (var i = 0; i < appDelegate.settings.Length; i++)
            {
                DataSource.names.Add(new Entity.SettingName(appDelegate.settings[i].dispName));
            }
            savedNameLabel.StringValue = "";
            savedPlanetLabel.StringValue = "";
            SavedAspectPlanetLabel.StringValue = "";
            SavedDispAspectLabel.StringValue = "";
            SavedOrbLabel.StringValue = "";


            dispName.StringValue = appDelegate.settings[0].dispName;

            dispPlanetSun.State = appDelegate.settings[0].dispPlanetSun == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetMoon.State = appDelegate.settings[0].dispPlanetMoon == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetMercury.State = appDelegate.settings[0].dispPlanetMercury == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetVenus.State = appDelegate.settings[0].dispPlanetVenus == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetMars.State = appDelegate.settings[0].dispPlanetMars == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetJupiter.State = appDelegate.settings[0].dispPlanetJupiter == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetSaturn.State = appDelegate.settings[0].dispPlanetSaturn == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetUranus.State = appDelegate.settings[0].dispPlanetUranus == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetNeptune.State = appDelegate.settings[0].dispPlanetNeptune == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetPluto.State = appDelegate.settings[0].dispPlanetPluto == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetAsc.State = appDelegate.settings[0].dispPlanetAsc == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetMc.State = appDelegate.settings[0].dispPlanetMc == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetDH.State = appDelegate.settings[0].dispPlanetDH == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetDT.State = appDelegate.settings[0].dispPlanetDT == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetChiron.State = appDelegate.settings[0].dispPlanetChiron == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetLilith.State = appDelegate.settings[0].dispPlanetLilith == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetEarth.State = appDelegate.settings[0].dispPlanetEarth == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetCeres.State = appDelegate.settings[0].dispPlanetCeres == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetPallas.State = appDelegate.settings[0].dispPlanetPallas == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetJuno.State = appDelegate.settings[0].dispPlanetJuno == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetVesta.State = appDelegate.settings[0].dispPlanetVesta == 1 ? NSCellStateValue.On : NSCellStateValue.Off;

            dispAspectPlanetSun.State = appDelegate.settings[0].dispAspectPlanetSun == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetMoon.State = appDelegate.settings[0].dispAspectPlanetMoon == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetMercury.State = appDelegate.settings[0].dispAspectPlanetMercury == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetVenus.State = appDelegate.settings[0].dispAspectPlanetVenus == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetMars.State = appDelegate.settings[0].dispAspectPlanetMars == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetJupiter.State = appDelegate.settings[0].dispAspectPlanetJupiter == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetSaturn.State = appDelegate.settings[0].dispAspectPlanetSaturn == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetUranus.State = appDelegate.settings[0].dispAspectPlanetUranus == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetNeptune.State = appDelegate.settings[0].dispAspectPlanetNeptune == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetPluto.State = appDelegate.settings[0].dispAspectPlanetPluto == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetAsc.State = appDelegate.settings[0].dispAspectPlanetAsc == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetMc.State = appDelegate.settings[0].dispAspectPlanetMc == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetDH.State = appDelegate.settings[0].dispAspectPlanetDH == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetDT.State = appDelegate.settings[0].dispAspectPlanetDT == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetChiron.State = appDelegate.settings[0].dispAspectPlanetChiron == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetLilith.State = appDelegate.settings[0].dispAspectPlanetLilith == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetEarth.State = appDelegate.settings[0].dispAspectPlanetEarth == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetCeres.State = appDelegate.settings[0].dispAspectPlanetCeres == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetPallas.State = appDelegate.settings[0].dispAspectPlanetPallas == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetJuno.State = appDelegate.settings[0].dispAspectPlanetJuno == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetVesta.State = appDelegate.settings[0].dispAspectPlanetVesta == 1 ? NSCellStateValue.On : NSCellStateValue.Off;

            dispConjunction.State = appDelegate.settings[0].dispAspectConjunction == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispOpposition.State = appDelegate.settings[0].dispAspectOpposition == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispTrine.State = appDelegate.settings[0].dispAspectTrine == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispSquare.State = appDelegate.settings[0].dispAspectSquare == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispSextile.State = appDelegate.settings[0].dispAspectSextile == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispInconjunct.State = appDelegate.settings[0].dispAspectInconjunct == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispSesquiQuadrate.State = appDelegate.settings[0].dispAspectSesquiQuadrate == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispSemiSquare.State = appDelegate.settings[0].dispAspectSemiSquare == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispSemiSextile.State = appDelegate.settings[0].dispAspectSemiSextile == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispQuintile.State = appDelegate.settings[0].dispAspectQuintile == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispNovile.State = appDelegate.settings[0].dispAspectNovile == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispBiQuintile.State = appDelegate.settings[0].dispAspectBiQuintile == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispSemiQuintile.State = appDelegate.settings[0].dispAspectSemiQuintile == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispSeptile.State = appDelegate.settings[0].dispAspectSeptile == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispQuindecile.State = appDelegate.settings[0].dispAspectQuindecile == 1 ? NSCellStateValue.On : NSCellStateValue.Off;

            orbSunMoonSoft.StringValue = appDelegate.settings[0].orbSunMoon[0].ToString();
            orbSunMoonHard.StringValue = appDelegate.settings[0].orbSunMoon[1].ToString();
            orb1stSoft.StringValue = appDelegate.settings[0].orb1st[0].ToString();
            orb1stHard.StringValue = appDelegate.settings[0].orb1st[1].ToString();
            orb2ndSoft.StringValue = appDelegate.settings[0].orb2nd[0].ToString();
            orb2ndHard.StringValue = appDelegate.settings[0].orb2nd[1].ToString();

            // Populate the Product Table
            SettingListTable.DataSource = DataSource;
            SettingListTable.Delegate = new SettingDataDelegate(DataSource);
            SettingListTable.SelectRow(0, false);

        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        //strongly typed view accessor
        public new Setting1View View
        {
            get
            {
                return (Setting1View)base.View;
            }
        }

        /// <summary>
        /// ファイル保存
        /// </summary>
        /// <param name="index"></param>
        /// <param name="setting"></param>
        private void SettingSave(int index, SettingData setting)
        {
            var root = Util.root;

            string settingFileName = root + String.Format("/system/settings{0}.json", index);

            SettingJson settingJson = new SettingJson(setting);

            string settingJsonStr = JsonSerializer.Serialize(settingJson,
               new JsonSerializerOptions
               {
                   Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                   WriteIndented = true
               }
           );
            using (FileStream fs = new FileStream(settingFileName, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(settingJsonStr);
                sw.Close();
            }

            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            appDelegate.viewController.RefreshSettingPopupButton();
        }

        private void SetMenuTitle(string title)
        {
        }

        /// <summary>
        /// 保存クリック
        /// </summary>
        /// <param name="sender"></param>
        partial void SaveDispNameClicked(Foundation.NSObject sender)
        {
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            int index = ((int)SettingListTable.SelectedRow);
            if (index == -1)
            {
                index = 0;
            }

            appDelegate.settings[index].dispName = dispName.StringValue;

            var DataSource = new SettingDataSource();
            for (var i = 0; i < appDelegate.settings.Length; i++)
            {
                DataSource.names.Add(new Entity.SettingName(appDelegate.settings[i].dispName));
            }

            SettingListTable.DataSource = DataSource;
            SettingListTable.Delegate = new SettingDataDelegate(DataSource);
            SettingListTable.SelectRow(index, true);

            SettingSave(index, appDelegate.settings[index]);
            savedNameLabel.StringValue = "保存しました";
            if (appDelegate.settingIndex == index)
            {
                appDelegate.viewController.ReCalc();
                appDelegate.viewController.ReRender();
            }

        }

        partial void SaveAspectKind(Foundation.NSObject sender)
        {
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            int index = ((int)SettingListTable.SelectedRow);
            if (index == -1)
            {
                index = 0;
            }

            appDelegate.settings[index].dispAspectConjunction = dispConjunction.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectOpposition = dispOpposition.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectTrine = dispTrine.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectSquare = dispSquare.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectSextile = dispSextile.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectInconjunct = dispInconjunct.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectSesquiQuadrate = dispSesquiQuadrate.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectSemiSquare = dispSemiSquare.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectSemiSextile = dispSemiSextile.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectQuintile = dispQuintile.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectNovile = dispNovile.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectBiQuintile = dispBiQuintile.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectSemiQuintile = dispSemiQuintile.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectSeptile = dispSeptile.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectQuindecile = dispQuindecile.State == NSCellStateValue.On ? 1 : 0;

            SettingSave(index, appDelegate.settings[index]);
            SavedDispAspectLabel.StringValue = "保存しました";
            if (appDelegate.settingIndex == index) { 
                appDelegate.viewController.ReCalc();
                appDelegate.viewController.ReRender();
            }

        }

        /// <summary>
        /// 保存クリック
        /// </summary>
        /// <param name="sender"></param>
        partial void SaveOrbClicked(Foundation.NSObject sender)
        {
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            int index = ((int)SettingListTable.SelectedRow);
            if (index == -1)
            {
                index = 0;
            }

            appDelegate.settings[index].orbSunMoon[0] = Double.Parse(orbSunMoonSoft.StringValue);
            appDelegate.settings[index].orbSunMoon[1] = Double.Parse(orbSunMoonHard.StringValue);

            appDelegate.settings[index].orb1st[0] = Double.Parse(orb1stSoft.StringValue);
            appDelegate.settings[index].orb1st[1] = Double.Parse(orb1stHard.StringValue);

            appDelegate.settings[index].orb2nd[0] = Double.Parse(orb2ndSoft.StringValue);
            appDelegate.settings[index].orb2nd[1] = Double.Parse(orb2ndHard.StringValue);

            SettingSave(index, appDelegate.settings[index]);
            SavedOrbLabel.StringValue = "保存しました";
            if (appDelegate.settingIndex == index)
            {
                appDelegate.viewController.ReCalc();
                appDelegate.viewController.ReRender();
            }

        }

        partial void SettingTableCellClicked(Foundation.NSObject sender)
        {
        }

        /// <summary>
        /// テーブル選択(SelectionChanged)
        /// </summary>
        /// <param name="sender"></param>
        partial void SettingListTableClicked(Foundation.NSObject sender)
        {
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            NSTableView s = (NSTableView)sender;
            Debug.WriteLine(s.ClickedRow);
            dispName.StringValue = appDelegate.settings[s.ClickedRow].dispName;

            dispPlanetSun.State = appDelegate.settings[s.ClickedRow].dispPlanetSun == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetMoon.State = appDelegate.settings[s.ClickedRow].dispPlanetMoon == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetMercury.State = appDelegate.settings[s.ClickedRow].dispPlanetMercury == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetVenus.State = appDelegate.settings[s.ClickedRow].dispPlanetVenus == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetMars.State = appDelegate.settings[s.ClickedRow].dispPlanetMars == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetJupiter.State = appDelegate.settings[s.ClickedRow].dispPlanetJupiter == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetSaturn.State = appDelegate.settings[s.ClickedRow].dispPlanetSaturn == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetUranus.State = appDelegate.settings[s.ClickedRow].dispPlanetUranus == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetNeptune.State = appDelegate.settings[s.ClickedRow].dispPlanetNeptune == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetPluto.State = appDelegate.settings[s.ClickedRow].dispPlanetPluto == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetAsc.State = appDelegate.settings[s.ClickedRow].dispPlanetAsc == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetMc.State = appDelegate.settings[s.ClickedRow].dispPlanetMc == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetDH.State = appDelegate.settings[s.ClickedRow].dispPlanetDH == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetDT.State = appDelegate.settings[s.ClickedRow].dispPlanetDT == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetChiron.State = appDelegate.settings[s.ClickedRow].dispPlanetChiron == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetLilith.State = appDelegate.settings[s.ClickedRow].dispPlanetLilith == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetEarth.State = appDelegate.settings[s.ClickedRow].dispPlanetEarth == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetCeres.State = appDelegate.settings[s.ClickedRow].dispPlanetCeres == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetPallas.State = appDelegate.settings[s.ClickedRow].dispPlanetPallas == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetJuno.State = appDelegate.settings[s.ClickedRow].dispPlanetJuno == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispPlanetVesta.State = appDelegate.settings[s.ClickedRow].dispPlanetVesta == 1 ? NSCellStateValue.On : NSCellStateValue.Off;

            dispAspectPlanetSun.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetSun == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetMoon.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetMoon == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetMercury.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetMercury == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetVenus.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetVenus == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetMars.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetMars == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetJupiter.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetJupiter == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetSaturn.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetSaturn == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetUranus.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetUranus == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetNeptune.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetNeptune == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetPluto.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetPluto == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetAsc.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetAsc == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetMc.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetMc == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetDH.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetDH == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetDT.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetDT == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetChiron.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetChiron == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetLilith.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetLilith == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetEarth.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetEarth == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetCeres.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetCeres == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetPallas.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetPallas == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetJuno.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetJuno == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispAspectPlanetVesta.State = appDelegate.settings[s.ClickedRow].dispAspectPlanetVesta == 1 ? NSCellStateValue.On : NSCellStateValue.Off;


            dispConjunction.State = appDelegate.settings[s.ClickedRow].dispAspectConjunction == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispOpposition.State = appDelegate.settings[s.ClickedRow].dispAspectOpposition == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispTrine.State = appDelegate.settings[s.ClickedRow].dispAspectTrine == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispSquare.State = appDelegate.settings[s.ClickedRow].dispAspectSquare == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispSextile.State = appDelegate.settings[s.ClickedRow].dispAspectSextile == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispInconjunct.State = appDelegate.settings[s.ClickedRow].dispAspectInconjunct == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispSesquiQuadrate.State = appDelegate.settings[s.ClickedRow].dispAspectSesquiQuadrate == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispSemiSquare.State = appDelegate.settings[s.ClickedRow].dispAspectSemiSquare == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispSemiSextile.State = appDelegate.settings[s.ClickedRow].dispAspectSemiSextile == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispQuintile.State = appDelegate.settings[s.ClickedRow].dispAspectQuintile == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispNovile.State = appDelegate.settings[s.ClickedRow].dispAspectNovile == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispBiQuintile.State = appDelegate.settings[s.ClickedRow].dispAspectBiQuintile == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispSeptile.State = appDelegate.settings[s.ClickedRow].dispAspectSeptile == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispSemiQuintile.State = appDelegate.settings[s.ClickedRow].dispAspectSemiQuintile == 1 ? NSCellStateValue.On : NSCellStateValue.Off;
            dispQuindecile.State = appDelegate.settings[s.ClickedRow].dispAspectQuindecile == 1 ? NSCellStateValue.On : NSCellStateValue.Off;

            orbSunMoonSoft.StringValue = appDelegate.settings[s.ClickedRow].orbSunMoon[0].ToString();
            orbSunMoonHard.StringValue = appDelegate.settings[s.ClickedRow].orbSunMoon[1].ToString();
            orb1stSoft.StringValue = appDelegate.settings[s.ClickedRow].orb1st[0].ToString();
            orb1stHard.StringValue = appDelegate.settings[s.ClickedRow].orb1st[1].ToString();
            orb2ndSoft.StringValue = appDelegate.settings[s.ClickedRow].orb2nd[0].ToString();
            orb2ndHard.StringValue = appDelegate.settings[s.ClickedRow].orb2nd[1].ToString();
        }

        partial void SaveDispPlanet(Foundation.NSObject sender)
        {
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            int index = ((int)SettingListTable.SelectedRow);
            if (index == -1)
            {
                index = 0;
            }

            appDelegate.settings[index].dispPlanetSun = dispPlanetSun.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetMoon = dispPlanetMoon.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetMercury = dispPlanetMercury.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetVenus = dispPlanetVenus.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetMars = dispPlanetMars.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetJupiter = dispPlanetJupiter.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetSaturn = dispPlanetSaturn.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetUranus = dispPlanetUranus.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetNeptune = dispPlanetNeptune.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetPluto = dispPlanetPluto.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetAsc = dispPlanetAsc.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetMc = dispPlanetMc.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetDH = dispPlanetDH.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetDT = dispPlanetDT.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetChiron = dispPlanetChiron.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetLilith = dispPlanetLilith.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetEarth = dispPlanetEarth.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetCeres = dispPlanetCeres.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetPallas = dispPlanetPallas.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetJuno = dispPlanetJuno.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispPlanetVesta = dispPlanetVesta.State == NSCellStateValue.On ? 1 : 0;

            SettingSave(index, appDelegate.settings[index]);
            savedPlanetLabel.StringValue = "保存しました";

            if (appDelegate.settingIndex == index)
            {
                appDelegate.viewController.ReCalc();
                appDelegate.viewController.ReRender();
            }

        }

        partial void SaveDispAspectPlanet(Foundation.NSObject sender)
        {
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            int index = ((int)SettingListTable.SelectedRow);
            if (index == -1)
            {
                index = 0;
            }

            appDelegate.settings[index].dispAspectPlanetSun = dispAspectPlanetSun.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetMoon = dispAspectPlanetMoon.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetMercury = dispAspectPlanetMercury.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetVenus = dispAspectPlanetVenus.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetMars = dispAspectPlanetMars.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetJupiter = dispAspectPlanetJupiter.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetSaturn = dispAspectPlanetSaturn.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetUranus = dispAspectPlanetUranus.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetNeptune = dispAspectPlanetNeptune.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetPluto = dispAspectPlanetPluto.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetAsc = dispAspectPlanetAsc.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetMc = dispAspectPlanetMc.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetDH = dispAspectPlanetDH.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetDT = dispAspectPlanetDT.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetChiron = dispAspectPlanetChiron.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetLilith = dispAspectPlanetLilith.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetEarth = dispAspectPlanetEarth.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetCeres = dispAspectPlanetCeres.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetPallas = dispAspectPlanetPallas.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetJuno = dispAspectPlanetJuno.State == NSCellStateValue.On ? 1 : 0;
            appDelegate.settings[index].dispAspectPlanetVesta = dispAspectPlanetVesta.State == NSCellStateValue.On ? 1 : 0;

            SettingSave(index, appDelegate.settings[index]);
            SavedAspectPlanetLabel.StringValue = "保存しました";
            if (appDelegate.settingIndex == index)
            {
                appDelegate.viewController.ReCalc();
                appDelegate.viewController.ReRender();
            }

        }
    }
}
