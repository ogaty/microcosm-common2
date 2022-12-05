using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using microcosmMac2.User;
using static CoreFoundation.DispatchSource;
using microcosmMac2.Views.DataSources;

namespace microcosmMac2.Views
{
    public partial class TimeSetViewController : AppKit.NSViewController
    {
        #region Constructors

        // Called when created from unmanaged code
        public TimeSetViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public TimeSetViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public TimeSetViewController() : base("TimeSetView", NSBundle.MainBundle)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        public override void ViewDidLoad()
        {
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            string currentTime = appDelegate.viewController.GetTimeSet();
            UserData u = appDelegate.viewController.udata1;
            if (currentTime == "User1")
            {
                u = appDelegate.viewController.udata1;
            }
            else if (currentTime == "User2")
            {
                u = appDelegate.viewController.udata2;
            }
            else if (currentTime == "Event1")
            {
                u = appDelegate.viewController.edata1;
            }
            else if (currentTime == "Event2")
            {
                u = appDelegate.viewController.edata2;
            }
            DateTime date = new DateTime(u.birth_year, u.birth_month, u.birth_day, 12, 0, 0);
            DateTime reference = new DateTime(2001, 1, 1, 0, 0, 0);
            NSDate dd = NSDate.FromTimeIntervalSinceReferenceDate(
                (date - reference).TotalSeconds);
            timebox.DateValue = dd;

            hour.StringValue = u.birth_hour.ToString();
            minute.StringValue = u.birth_minute.ToString();
            second.StringValue = u.birth_second.ToString();
        }

        #endregion

        //strongly typed view accessor
        public new TimeSetView View
        {
            get
            {
                return (TimeSetView)base.View;
            }
        }

        partial void SaveButtonClicked(Foundation.NSObject sender)
        {
            DateTime d = DateTime.Parse(timebox.DateValue.ToString());
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            string currentTime = appDelegate.viewController.GetTimeSet();
            UserData u = appDelegate.viewController.udata1;
            int h = 0;
            if (!Int32.TryParse(hour.StringValue, out h))
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "時刻は数値で指定してください。";
                alert.RunModal();
                return;
            }
            int m = 0;
            if (!Int32.TryParse(minute.StringValue, out m))
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "時刻は数値で指定してください。";
                alert.RunModal();
                return;
            }
            int s = 0;
            if (!Int32.TryParse(second.StringValue, out s))
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "時刻は数値で指定してください。";
                alert.RunModal();
                return;
            }

            if (currentTime == "User1")
            {
                appDelegate.viewController.udata1.birth_year = d.Year;
                appDelegate.viewController.udata1.birth_month = d.Month;
                appDelegate.viewController.udata1.birth_day = d.Day;
                appDelegate.viewController.udata1.birth_hour = h;
                appDelegate.viewController.udata1.birth_minute = m;
                appDelegate.viewController.udata1.birth_second = s;
                appDelegate.viewController.RefreshUserBox(0, appDelegate.viewController.udata1);
            }
            else if (currentTime == "User2")
            {
                appDelegate.viewController.udata2.birth_year = d.Year;
                appDelegate.viewController.udata2.birth_month = d.Month;
                appDelegate.viewController.udata2.birth_day = d.Day;
                appDelegate.viewController.udata2.birth_hour = h;
                appDelegate.viewController.udata2.birth_minute = m;
                appDelegate.viewController.udata2.birth_second = s;
                appDelegate.viewController.RefreshUserBox(1, appDelegate.viewController.udata2);
            }
            else if (currentTime == "Event1")
            {
                appDelegate.viewController.edata1.birth_year = d.Year;
                appDelegate.viewController.edata1.birth_month = d.Month;
                appDelegate.viewController.edata1.birth_day = d.Day;
                appDelegate.viewController.edata1.birth_hour = h;
                appDelegate.viewController.edata1.birth_minute = m;
                appDelegate.viewController.edata1.birth_second = s;
                appDelegate.viewController.RefreshEventBox(0, appDelegate.viewController.edata1);
            }
            else if (currentTime == "Event2")
            {
                appDelegate.viewController.edata2.birth_year = d.Year;
                appDelegate.viewController.edata2.birth_month = d.Month;
                appDelegate.viewController.edata2.birth_day = d.Day;
                appDelegate.viewController.edata2.birth_hour = h;
                appDelegate.viewController.edata2.birth_minute = m;
                appDelegate.viewController.edata2.birth_second = s;
                appDelegate.viewController.RefreshEventBox(1, appDelegate.viewController.edata2);
            }


            appDelegate.viewController.ReCalc();
            appDelegate.viewController.ReRender();

            DismissController(this);
        }
    }
}
