using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace microcosmMac2.Views
{
    public partial class SpanViewController : AppKit.NSViewController
    {
        #region Constructors

        // Called when created from unmanaged code
        public SpanViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public SpanViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public SpanViewController() : base("SpanView", NSBundle.MainBundle)
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
            unit.StringValue = appDelegate.viewController.plusUnit.ToString();

            if (appDelegate.viewController.plusUnit >= 86400)
            {
                int tmp = (int)(appDelegate.viewController.plusUnit / 86400);
                unit.StringValue = tmp.ToString();
                radioSeconds.State = NSCellStateValue.Off;
                radioMinutes.State = NSCellStateValue.Off;
                radioHours.State = NSCellStateValue.Off;
                radioDays.State = NSCellStateValue.On;
            }
            else if (appDelegate.viewController.plusUnit >= 3600)
            {
                int tmp = (int)(appDelegate.viewController.plusUnit / 3600);
                unit.StringValue = tmp.ToString();
                radioSeconds.State = NSCellStateValue.Off;
                radioMinutes.State = NSCellStateValue.Off;
                radioHours.State = NSCellStateValue.On;
                radioDays.State = NSCellStateValue.Off;
            }
            else if (appDelegate.viewController.plusUnit >= 60)
            {
                int tmp = (int)(appDelegate.viewController.plusUnit / 60);
                unit.StringValue = tmp.ToString();
                radioSeconds.State = NSCellStateValue.Off;
                radioHours.State = NSCellStateValue.On;
                radioDays.State = NSCellStateValue.Off;
                radioMinutes.State = NSCellStateValue.On;
            }
            else
            {
                unit.StringValue = appDelegate.viewController.plusUnit.ToString();
                radioSeconds.State = NSCellStateValue.On;
                radioMinutes.State = NSCellStateValue.Off;
                radioHours.State = NSCellStateValue.Off;
                radioDays.State = NSCellStateValue.Off;
            }

            string[] items = { "", "7 days", "30 days", "365 days" };
            spanCombo.RemoveAllItems();
            spanCombo.AddItems(items);
            spanCombo.SelectItem(appDelegate.config.defaultTimezoneStr);

        }

        #endregion

        //strongly typed view accessor
        public new SpanView View
        {
            get
            {
                return (SpanView)base.View;
            }
        }

        partial void SaveButtonClicked(Foundation.NSObject sender)
        {
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;

            int unitInt = 0;
            if (!Int32.TryParse(unit.StringValue, out unitInt))
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "数値で指定してください。";
                alert.RunModal();
                return;
            }

            string unitDisplay = unitInt.ToString();
            string display = "";
            int plusUnit = 0;

            if (radioSeconds.State == NSCellStateValue.On)
            {
                display = " Seconds";
                plusUnit = unitInt;
            }
            if (radioMinutes.State == NSCellStateValue.On)
            {
                display = " Minutes";
                plusUnit = unitInt * 60;
            }
            if (radioHours.State == NSCellStateValue.On)
            {
                display = " Hours";
                plusUnit = unitInt * 3600;
            }
            if (radioDays.State == NSCellStateValue.On)
            {
                display = " Days";
                plusUnit = unitInt * 86400;
            }
            appDelegate.viewController.SetSpanButton(unitDisplay + display);
            appDelegate.viewController.plusUnit = plusUnit;

            DismissController(this);
        }

        partial void radioDaysClicked(Foundation.NSObject sender)
        {
            radioSeconds.State = NSCellStateValue.Off;
            radioMinutes.State = NSCellStateValue.Off;
            radioHours.State = NSCellStateValue.Off;
        }

        partial void radioHoursClicked(Foundation.NSObject sender)
        {
            radioSeconds.State = NSCellStateValue.Off;
            radioMinutes.State = NSCellStateValue.Off;
            radioDays.State = NSCellStateValue.Off;
        }

        partial void radioMinutesClicked(Foundation.NSObject sender)
        {
            radioSeconds.State = NSCellStateValue.Off;
            radioDays.State = NSCellStateValue.Off;
            radioHours.State = NSCellStateValue.Off;
        }

        partial void radioSecondsClicked(Foundation.NSObject sender)
        {
            radioMinutes.State = NSCellStateValue.Off;
            radioHours.State = NSCellStateValue.Off;
            radioDays.State = NSCellStateValue.Off;
        }

        partial void spanPopupChanged(Foundation.NSObject sender)
        {
            NSPopUpButton combo = (NSPopUpButton)sender;
            string title = combo.SelectedItem.Title;
            switch (title)
            {
                case "":
                    break;
                case "7 days":
                    unit.StringValue = "7";
                    radioSeconds.State = NSCellStateValue.Off;
                    radioMinutes.State = NSCellStateValue.Off;
                    radioDays.State = NSCellStateValue.On;
                    radioHours.State = NSCellStateValue.Off;

                    break;
                case "30 days":
                    unit.StringValue = "30";
                    radioSeconds.State = NSCellStateValue.Off;
                    radioMinutes.State = NSCellStateValue.Off;
                    radioDays.State = NSCellStateValue.On;
                    radioHours.State = NSCellStateValue.Off;

                    break;
                case "365 days":
                    unit.StringValue = "365";
                    radioSeconds.State = NSCellStateValue.Off;
                    radioMinutes.State = NSCellStateValue.Off;
                    radioDays.State = NSCellStateValue.On;
                    radioHours.State = NSCellStateValue.Off;

                    break;
            }
        }
    }
}
