using System;
using System.Collections.Generic;
using AppKit;
using microcosmMac2.Views.Entity;

namespace microcosmMac2.Views.DataSources
{
    public class FilesDataSource : NSTableViewDataSource
    {
        public List<FileName> names = new List<FileName>();

        public FilesDataSource()
        {
        }

        public override nint GetRowCount(NSTableView tableView)
        {
            return names.Count;
        }

    }
}

