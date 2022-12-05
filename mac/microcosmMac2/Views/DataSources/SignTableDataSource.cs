using System;
using System.Collections.Generic;
using AppKit;

namespace microcosmMac2.Views.DataSources
{
    public class SignTableDataSource : NSTableViewDataSource
    {
        public List<SignTableData> names = new List<SignTableData>();

        public SignTableDataSource()
        {
        }

        public override nint GetRowCount(NSTableView tableView)
        {
            return names.Count;
        }
    }
}

