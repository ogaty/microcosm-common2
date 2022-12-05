using System;
using System.Collections.Generic;
using AppKit;

namespace microcosmMac2.Views.DataSources
{
    public class AddrDataSource : NSTableViewDataSource
    {
        public List<Addr> names = new List<Addr>();

        public AddrDataSource()
        {
        }

        public override nint GetRowCount(NSTableView tableView)
        {
            return names.Count;
        }

    }
}

