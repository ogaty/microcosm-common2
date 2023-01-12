using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using ScreenCaptureKit;
using microcosmMac2.Config;
using System.IO;
using System.Text.Json;
using microcosmMac2.Common;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using microcosmMac2.Views.DataSources;
using System.Globalization;
using System.Diagnostics;

namespace microcosmMac2.Views
{
    public partial class ConfigViewController : AppKit.NSViewController
    {
        public List<AddrCsv> addrs = new List<AddrCsv>();

        #region Constructors

        // Called when created from unmanaged code
        public ConfigViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public ConfigViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public ConfigViewController() : base("ConfigView", NSBundle.MainBundle)
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

            savedLabel.StringValue = "";

            string[] items = CommonData.GetTimeTables();

            defaultTimezone.RemoveAllItems();
            defaultTimezone.AddItems(items);
            defaultTimezone.SelectItem(appDelegate.config.defaultTimezoneStr);
            var DataSource = new AddrDataSource();

            var root = Util.root;
            string fileName = root + "/system/addr.csv";

            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);

                using (var csv = new CsvHelper.CsvReader(sr, new CultureInfo("ja-JP", false)))
                {
                    var records = csv.GetRecords<AddrCsv>();

                    foreach (AddrCsv record in records)
                    {
                        DataSource.names.Add(new Addr()
                        {
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

            if (appDelegate.config.centric == ECentric.GEO_CENTRIC)
            {
                geoCentric.State = NSCellStateValue.On;
                helioCentric.State = NSCellStateValue.Off;
            }
            else
            {
                geoCentric.State = NSCellStateValue.Off;
                helioCentric.State = NSCellStateValue.On;
            }

            if (appDelegate.config.sidereal == Esidereal.SIDEREAL)
            {
                sidereal.State = NSCellStateValue.On;
                tropical.State = NSCellStateValue.Off;
                Draconic.State = NSCellStateValue.Off;
            }
            else if (appDelegate.config.sidereal == Esidereal.TROPICAL)
            {
                sidereal.State = NSCellStateValue.Off;
                tropical.State = NSCellStateValue.On;
                Draconic.State = NSCellStateValue.Off;
            }
            else
            {
                sidereal.State = NSCellStateValue.Off;
                tropical.State = NSCellStateValue.Off;
                Draconic.State = NSCellStateValue.On;
            }


            if (appDelegate.config.decimalDisp == EDecimalDisp.DECIMAL)
            {
                deci.State = NSCellStateValue.On;
                hex.State = NSCellStateValue.Off;
            }
            else
            {
                deci.State = NSCellStateValue.Off;
                hex.State = NSCellStateValue.On;
            }

            if (appDelegate.config.dispPattern2 == EDispPettern.FULL)
            {
                fulldisp.State = NSCellStateValue.On;
                simpledisp.State = NSCellStateValue.Off;
            }
            else
            {
                fulldisp.State = NSCellStateValue.Off;
                simpledisp.State = NSCellStateValue.On;
            }

            if (appDelegate.config.nodeCalc == ENodeCalc.TRUE)
            {
                truenode.State = NSCellStateValue.On;
                meannode.State = NSCellStateValue.Off;
            }
            else
            {
                truenode.State = NSCellStateValue.Off;
                meannode.State = NSCellStateValue.On;
            }

            if (appDelegate.config.lilithCalc == ELilithCalc.OSCU)
            {
                osculatingapogee.State = NSCellStateValue.On;
                meanapogee.State = NSCellStateValue.Off;
            }
            else
            {
                osculatingapogee.State = NSCellStateValue.Off;
                meanapogee.State = NSCellStateValue.On;
            }



            defaultLat.StringValue = appDelegate.config.lat.ToString();
            defaultLng.StringValue = appDelegate.config.lng.ToString();
            defaultPlace.StringValue = appDelegate.config.defaultPlace;

            LatLngTable.DataSource = DataSource;
            LatLngTable.Delegate = new AddrDelegate(DataSource);
            LatLngTable.SelectRow(0, false);

            int index = 0;
            bool set = false;
            Debug.WriteLine(appDelegate.config.defaultTimezoneStr);
            foreach (NSMenuItem item in defaultTimezone.Items())
            {
                if (item.Title == appDelegate.config.defaultTimezoneStr)
                {
                    defaultTimezone.SelectItem(index);
                    set = true;
                }
                index++;
            }
            if (!set)
            {
                defaultTimezone.SelectItem("Asia/Tokyo (+9:00)");
            }

        }

        public override void ViewDidAppear()
        {
            // After a window is displayed, get the handle to the new window.
        }

        #endregion

        //strongly typed view accessor
        public new ConfigView View
        {
            get
            {
                return (ConfigView)base.View;
            }
        }

        partial void GeoCentricClick(Foundation.NSObject sender)
        {
            NSButton btn = (NSButton)sender;
            if (btn.State == NSCellStateValue.On)
            {
                helioCentric.State = NSCellStateValue.Off;
            }
        }

        partial void HelioCentricClick(Foundation.NSObject sender)
        {
            NSButton btn = (NSButton)sender;
            if (btn.State == NSCellStateValue.On)
            {
                geoCentric.State = NSCellStateValue.Off;
            }
        }

        partial void SiderealClick(Foundation.NSObject sender)
        {
            NSButton btn = (NSButton)sender;
            if (btn.State == NSCellStateValue.On)
            {
                tropical.State = NSCellStateValue.Off;
                Draconic.State = NSCellStateValue.Off;
            }
        }

        partial void TropicalClick(Foundation.NSObject sender)
        {
            NSButton btn = (NSButton)sender;
            if (btn.State == NSCellStateValue.On)
            {
                sidereal.State = NSCellStateValue.Off;
                Draconic.State = NSCellStateValue.Off;
            }
        }

        partial void DraconicClick(Foundation.NSObject sender)
        {
            NSButton btn = (NSButton)sender;
            if (btn.State == NSCellStateValue.On)
            {
                sidereal.State = NSCellStateValue.Off;
                tropical.State = NSCellStateValue.Off;
            }
        }

        partial void TrueNodeClick(Foundation.NSObject sender)
        {
            NSButton btn = (NSButton)sender;
            if (btn.State == NSCellStateValue.On)
            {
                meannode.State = NSCellStateValue.Off;
            }
        }

        partial void MeanNodeClick(Foundation.NSObject sender)
        {
            NSButton btn = (NSButton)sender;
            if (btn.State == NSCellStateValue.On)
            {
                truenode.State = NSCellStateValue.Off;
            }
        }

        partial void OsculatingApogeeClick(Foundation.NSObject sender)
        {
            NSButton btn = (NSButton)sender;
            if (btn.State == NSCellStateValue.On)
            {
                meanapogee.State = NSCellStateValue.Off;
            }
        }

        partial void MeanApogeeClick(Foundation.NSObject sender)
        {
            NSButton btn = (NSButton)sender;
            if (btn.State == NSCellStateValue.On)
            {
                osculatingapogee.State = NSCellStateValue.Off;
            }
        }

        partial void HexClick(Foundation.NSObject sender)
        {
            NSButton btn = (NSButton)sender;
            if (btn.State == NSCellStateValue.On)
            {
                deci.State = NSCellStateValue.Off;
            }
        }

        partial void DecimalClick(Foundation.NSObject sender)
        {
            NSButton btn = (NSButton)sender;
            if (btn.State == NSCellStateValue.On)
            {
                hex.State = NSCellStateValue.Off;
            }
        }

        partial void FullDisp(Foundation.NSObject sender)
        {
            NSButton btn = (NSButton)sender;
            if (btn.State == NSCellStateValue.On)
            {
                simpledisp.State = NSCellStateValue.Off;
            }
        }

        partial void SimpleDispClick(Foundation.NSObject sender)
        {
            NSButton btn = (NSButton)sender;
            if (btn.State == NSCellStateValue.On)
            {
                fulldisp.State = NSCellStateValue.Off;
            }
        }

        private void ConfigSave()
        {
            string filename = Util.root + "/system/config.json";
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;

            string configJson = JsonSerializer.Serialize(appDelegate.config,
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

        partial void SubmitClick(Foundation.NSObject sender)
        {
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;

            if (geoCentric.State == NSCellStateValue.On)
            {
                appDelegate.config.centric = Config.ECentric.GEO_CENTRIC;
            }
            else
            {
                appDelegate.config.centric = Config.ECentric.HELIO_CENTRIC;
            }

            if (sidereal.State == NSCellStateValue.On)
            {
                appDelegate.config.sidereal = Config.Esidereal.SIDEREAL;
            }
            else if (tropical.State == NSCellStateValue.On)
            {
                appDelegate.config.sidereal = Config.Esidereal.TROPICAL;
            }
            else
            {
                appDelegate.config.sidereal = Config.Esidereal.DRACONIC;
            }

            if (truenode.State == NSCellStateValue.On)
            {
                appDelegate.config.nodeCalc = Config.ENodeCalc.TRUE;
            }
            else
            {
                appDelegate.config.nodeCalc = Config.ENodeCalc.MEAN;
            }

            if (osculatingapogee.State == NSCellStateValue.On)
            {
                appDelegate.config.lilithCalc = Config.ELilithCalc.OSCU;
            }
            else
            {
                appDelegate.config.lilithCalc = Config.ELilithCalc.MEAN;
            }

            if (deci.State == NSCellStateValue.On)
            {
                appDelegate.config.decimalDisp = Config.EDecimalDisp.DECIMAL;
            }
            else
            {
                appDelegate.config.decimalDisp = Config.EDecimalDisp.DEGREE;
            }

            if (fulldisp.State == NSCellStateValue.On)
            {
                appDelegate.config.dispPattern2 = Config.EDispPettern.FULL;
            }
            else
            {
                appDelegate.config.dispPattern2 = Config.EDispPettern.MINI;
            }

            appDelegate.config.defaultPlace = defaultPlace.StringValue;
            appDelegate.config.lat = Double.Parse(defaultLat.StringValue);
            appDelegate.config.lng = Double.Parse(defaultLng.StringValue);
            if (defaultTimezone.SelectedItem == null)
            {
                appDelegate.config.defaultTimezoneStr = "Asia/Tokyo (+9:00)";
            }
            else
            {
                appDelegate.config.defaultTimezoneStr = defaultTimezone.SelectedItem.Title;
            }
            appDelegate.config.defaultTimezone = CommonData.GetTimezoneValue(appDelegate.config.defaultTimezoneStr);

            ConfigSave();
            appDelegate.viewController.calc.updateConfig(appDelegate.config);
            savedLabel.StringValue = "保存しました";
            appDelegate.viewController.ReCalc();
            appDelegate.viewController.ReRender();
            DismissController(this);
        }

        partial void LatLngTableClicked(Foundation.NSObject sender)
        {
            NSTableView s = (NSTableView)sender;
            Debug.WriteLine(s.ClickedRow);

            defaultPlace.StringValue = addrs[(int)s.ClickedRow].name;
            defaultLat.StringValue = addrs[(int)s.ClickedRow].lat.ToString();
            defaultLng.StringValue = addrs[(int)s.ClickedRow].lat.ToString();
        }

    }
}
