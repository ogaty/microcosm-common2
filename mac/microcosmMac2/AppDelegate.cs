using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using AppKit;
using Foundation;
using microcosmMac2.Common;
using microcosmMac2.Config;
using microcosmMac2.Models;
using microcosmMac2.User;
using microcosmMac2.Views;
using SkiaSharp;

namespace microcosmMac2
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : NSApplicationDelegate
	{
        public bool trial = false;
		public ViewController viewController;
		public DatabaseViewController dbViewController;
        public SettingData[] settings;
		public SettingData currentSetting;
		public int settingIndex;
        public ConfigData config;
		public TempSetting tempSetting;
        public SpanType currentSpanType = SpanType.UNIT;

        // old:savedFileはディレクトリ、eventFileはファイル
        // new:savedFileはファイル、eventIndexはファイル内index
		public string dbSavedFile;
        public int dbSavedEventIndex;
        public string dbSavedDir;
        public string dbSavedDirFullPath;

        public UserData udata1;
		public UserData udata2;
		public UserData edata1;
		public UserData edata2;

        public Dictionary<int, EShortCut> keyEvent;
        public Dictionary<int, EShortCut> keyEventCtrl;

        public enum BandKind
        {
            NATAL = 0,
            PROGRESS = 1,
            TRANSIT = 2,
            COMPOSIT = 3
        }

        public int bands = 1;
        public BandKind firstBand;
        public BandKind secondBand;
        public BandKind thirdBand;
        public bool aspect11disp = true;
        public bool aspect12disp = true;
        public bool aspect13disp = true;
        public bool aspect22disp = true;
        public bool aspect23disp = true;
        public bool aspect33disp = true;

        public mainChart currentChart = mainChart.CHART;

        public ShortCut shortCut;

        public AppDelegate ()
		{
		}

		public override void DidFinishLaunching (NSNotification notification)
		{
			// Insert code here to initialize your application
		}

		public override void WillTerminate (NSNotification notification)
		{
            // Insert code here to tear down your application
        }

        public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
        {
            return true;
        }

        partial void CurrentChartOpen(Foundation.NSObject sender)
        {
            viewController.CurrentChart();
        }

        partial void OpenDbFolder(Foundation.NSObject sender)
        {
            System.Diagnostics.Process.Start(Util.root + @"/data");
        }

        partial void OpenAddrCsv(Foundation.NSObject sender)
        {
            if (trial)
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "トライアル版では使用できません。";
                alert.RunModal();
                return;
            }

            System.Diagnostics.Process.Start(Util.root + @"/system/addr.csv");
        }

        partial void OpenSabianCsv(Foundation.NSObject sender)
        {
            if (trial)
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "トライアル版では使用できません。";
                alert.RunModal();
                return;
            }

            System.Diagnostics.Process.Start(Util.root + @"/system/sabian.csv");
        }

        partial void OpenGithub(Foundation.NSObject sender)
        {
            System.Diagnostics.Process.Start("https://github.com/ogaty/microcosm-common2");
        }

        partial void SaveChartImage(Foundation.NSObject sender)
        {
            viewController.SaveChartImage();
        }
        partial void Chart1U1(Foundation.NSObject sender)
        {
            viewController.Chart1U1();
        }
        partial void Chart1U2(Foundation.NSObject sender)
        {
            viewController.Chart1U2();
        }
        partial void Chart1E1(Foundation.NSObject sender)
        {
            viewController.Chart1E1();
        }
        partial void Chart1E2(Foundation.NSObject sender)
        {
            viewController.Chart1E2();
        }

        partial void Chart2UE(Foundation.NSObject sender)
        {
            if (trial)
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "トライアル版では使用できません。";
                alert.RunModal();
                return;
            }
            viewController.Chart2UE();
        }

        partial void Chart2UU(Foundation.NSObject sender)
        {
            if (trial)
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "トライアル版では使用できません。";
                alert.RunModal();
                return;
            }
            viewController.Chart2UU();
        }

        partial void Chart3NPT(Foundation.NSObject sender)
        {
            viewController.Chart3NPT();
        }

        partial void Chart2EE(Foundation.NSObject sender)
        {
            if (trial)
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "トライアル版では使用できません。";
                alert.RunModal();
                return;
            }
            viewController.Chart2EE();
        }

        partial void Chart3NNT(Foundation.NSObject sender)
        {
            if (trial)
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "トライアル版では使用できません。";
                alert.RunModal();
                return;
            }
            viewController.Chart3NNT();
        }

        partial void Chart3NTT(Foundation.NSObject sender)
        {
            if (trial)
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "トライアル版では使用できません。";
                alert.RunModal();
                return;
            }
            viewController.Chart3NTT();
        }

        partial void Chart3NNC(Foundation.NSObject sender)
        {
            if (trial)
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "トライアル版では使用できません。";
                alert.RunModal();
                return;
            }
            viewController.Chart3NNC();
        }

        public NSMenuItem GetAspect11()
        {
            return aspect11;
        }

        public void SetAspect11(NSCellStateValue state)
        {
            aspect11.State = state;
        }

        public NSMenuItem GetAspect12()
        {
            return aspect12;
        }

        public void SetAspect12(NSCellStateValue state)
        {
            aspect12.State = state;
        }

        public NSMenuItem GetAspect13()
        {
            return aspect13;
        }

        public void SetAspect13(NSCellStateValue state)
        {
            aspect13.State = state;
        }

        public NSMenuItem GetAspect22()
        {
            return aspect22;
        }

        public void SetAspect22(NSCellStateValue state)
        {
            aspect22.State = state;
        }

        public NSMenuItem GetAspect23()
        {
            return aspect23;
        }

        public void SetAspect23(NSCellStateValue state)
        {
            aspect23.State = state;
        }

        public NSMenuItem GetAspect33()
        {
            return aspect33;
        }

        public void SetAspect33(NSCellStateValue state)
        {
            aspect33.State = state;
        }

        partial void Aspect11Click(Foundation.NSObject sender)
        {
            viewController.SetAspect11();
        }

        partial void Aspect12Click(Foundation.NSObject sender)
        {
            viewController.SetAspect12();
        }

        partial void Aspect13Click(Foundation.NSObject sender)
        {
            viewController.SetAspect13();
        }

        partial void Aspect22Click(Foundation.NSObject sender)
        {
            viewController.SetAspect22();
        }

        partial void Aspect23Click(Foundation.NSObject sender)
        {
            viewController.SetAspect23();
        }

        partial void Aspect33Click(Foundation.NSObject sender)
        {
            viewController.SetAspect33();
        }

        partial void AllAspectOff(Foundation.NSObject sender)
        {
            viewController.AspectAllOff();
        }

        partial void AllAspectOn(Foundation.NSObject sender)
        {
            viewController.AspectAllOn();
        }

        partial void ShowHelp(Foundation.NSObject sender)
        {
            System.Diagnostics.Process.Start("https://nimb.ws/M3NK1H");
        }

        partial void ShowLicense(Foundation.NSObject sender)
        {
            System.Diagnostics.Process.Start(Util.root + @"/license");
        }

        partial void ShowChart(Foundation.NSObject sender)
        {
            currentChart = mainChart.CHART;
            viewController.ReRender();
        }

        partial void ShowGrid(NSObject sender)
        {
            currentChart = mainChart.GRID;
            viewController.ReRender();
        }
    }
}

