using System;
using System.Collections.Generic;
using AppKit;

namespace microcosmMac2.Views.DataSources
{
    public class DirDataSource : NSTableViewDataSource
    {
        public List<DirData> names = new List<DirData>();

        public DirDataSource()
        {
        }

        public override nint GetRowCount(NSTableView tableView)
        {
            return names.Count;
        }

    }
}

