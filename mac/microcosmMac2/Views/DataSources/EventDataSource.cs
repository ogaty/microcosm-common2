using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using AppKit;

namespace microcosmMac2.Views.DataSources
{
    public class EventDataSource : NSTableViewDataSource
    {
        public List<EventData> names = new List<EventData>();
        public EventDataSource()
        {
        }


        public override nint GetRowCount(NSTableView tableView)
        {
            return names.Count;
        }
    }
}

