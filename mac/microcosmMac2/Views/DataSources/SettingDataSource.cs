using System;
using StoreKit;
using System.Collections.Generic;
using microcosmMac2.Views.Entity;
using AppKit;

namespace microcosmMac2.Views.DataSources
{
    public class SettingDataSource : NSTableViewDataSource
    {
        public List<SettingName> names = new List<SettingName>();
        public SettingDataSource()
        {
        }

        public override nint GetRowCount(NSTableView tableView)
        {
            return names.Count;
        }
    }
}

