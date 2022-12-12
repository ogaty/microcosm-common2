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

            if (appDelegate.currentSpanType == Common.SpanType.UNIT)
            {
                UnitRadio.State = NSCellStateValue.On;
                NewMoonRadio.State = NSCellStateValue.Off;
                FullMoonRadio.State = NSCellStateValue.Off;
                SolarReturnRadio.State = NSCellStateValue.Off;
                Ingress.State = NSCellStateValue.Off;
            }
            else if (appDelegate.currentSpanType == Common.SpanType.NEWMOON)
            {
                NewMoonRadio.State = NSCellStateValue.On;
                UnitRadio.State = NSCellStateValue.Off;
                FullMoonRadio.State = NSCellStateValue.Off;
                SolarReturnRadio.State = NSCellStateValue.Off;
                Ingress.State = NSCellStateValue.Off;

            }
            else if (appDelegate.currentSpanType == Common.SpanType.FULLMOON)
            {
                FullMoonRadio.State = NSCellStateValue.On;
                UnitRadio.State = NSCellStateValue.Off;
                NewMoonRadio.State = NSCellStateValue.Off;
                SolarReturnRadio.State = NSCellStateValue.Off;
                Ingress.State = NSCellStateValue.Off;
            }
            else if (appDelegate.currentSpanType == Common.SpanType.SOLARRETURN)
            {
                SolarReturnRadio.State = NSCellStateValue.On;
                UnitRadio.State = NSCellStateValue.Off;
                NewMoonRadio.State = NSCellStateValue.Off;
                FullMoonRadio.State = NSCellStateValue.Off;
                Ingress.State = NSCellStateValue.Off;
            }
            else if (appDelegate.currentSpanType == Common.SpanType.SOLARINGRESS ||
                appDelegate.currentSpanType == Common.SpanType.MOONINGRESS
                )
            {
                Ingress.State = NSCellStateValue.On;
                UnitRadio.State = NSCellStateValue.Off;
                NewMoonRadio.State = NSCellStateValue.Off;
                FullMoonRadio.State = NSCellStateValue.Off;
                SolarReturnRadio.State = NSCellStateValue.Off;
            }
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

            string[] ingressPlanetList = { "sun", "moon" };
            IngressPlanet.RemoveAllItems();
            IngressPlanet.AddItems(ingressPlanetList);
            if (appDelegate.currentSpanType == Common.SpanType.SOLARINGRESS)
            {
                IngressPlanet.SelectItem("sun");
            }
            else if (appDelegate.currentSpanType == Common.SpanType.MOONINGRESS)
            {
                IngressPlanet.SelectItem("moon");
            }
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

            if (UnitRadio.State == NSCellStateValue.On)
            {
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
                appDelegate.currentSpanType = Common.SpanType.UNIT;
            }
            else if (NewMoonRadio.State == NSCellStateValue.On)
            {
                appDelegate.viewController.SetSpanButton("NewMoon");
                appDelegate.currentSpanType = Common.SpanType.NEWMOON;
            }
            else if (FullMoonRadio.State == NSCellStateValue.On)
            {
                appDelegate.viewController.SetSpanButton("FullMoon");
                appDelegate.currentSpanType = Common.SpanType.FULLMOON;
            }
            else if (SolarReturnRadio.State == NSCellStateValue.On)
            {
                appDelegate.viewController.SetSpanButton("Solar");
                appDelegate.currentSpanType = Common.SpanType.SOLARRETURN;
            }
            else if (Ingress.State == NSCellStateValue.On)
            {
                if (IngressPlanet.SelectedItem.Title == "sun")
                {
                    appDelegate.viewController.SetSpanButton("Sun Ing.");
                    appDelegate.currentSpanType = Common.SpanType.SOLARINGRESS;
                }
                else if (IngressPlanet.SelectedItem.Title == "moon")
                {
                    appDelegate.viewController.SetSpanButton("Moon Ing.");
                    appDelegate.currentSpanType = Common.SpanType.MOONINGRESS;
                }
            }

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

        partial void FullMoonRadioClicked(Foundation.NSObject sender)
        {
            SolarReturnRadio.State = NSCellStateValue.Off;
            Ingress.State = NSCellStateValue.Off;
            NewMoonRadio.State = NSCellStateValue.Off;
            UnitRadio.State = NSCellStateValue.Off;
        }

        partial void NewMoonRadioClicked(Foundation.NSObject sender)
        {
            SolarReturnRadio.State = NSCellStateValue.Off;
            Ingress.State = NSCellStateValue.Off;
            FullMoonRadio.State = NSCellStateValue.Off;
            UnitRadio.State = NSCellStateValue.Off;
        }

        partial void IngressClicked(Foundation.NSObject sender)
        {
            SolarReturnRadio.State = NSCellStateValue.Off;
            NewMoonRadio.State = NSCellStateValue.Off;
            FullMoonRadio.State = NSCellStateValue.Off;
            UnitRadio.State = NSCellStateValue.Off;
        }

        partial void SolarReturnRadioClicked(Foundation.NSObject sender)
        {
            Ingress.State = NSCellStateValue.Off;
            NewMoonRadio.State = NSCellStateValue.Off;
            FullMoonRadio.State = NSCellStateValue.Off;
            UnitRadio.State = NSCellStateValue.Off;
        }

        partial void UnitRadioClicked(Foundation.NSObject sender)
        {
            SolarReturnRadio.State = NSCellStateValue.Off;
            Ingress.State = NSCellStateValue.Off;
            NewMoonRadio.State = NSCellStateValue.Off;
            FullMoonRadio.State = NSCellStateValue.Off;
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
