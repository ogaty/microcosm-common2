using System;
using AppKit;

namespace microcosmMac2.Views.DataSources
{
    public class DirDataDelegate : NSTableViewDelegate
    {
        private DirDataSource DataSource;
        private string identifier = "cell";

        public DirDataDelegate(DirDataSource dataSource)
        {
            this.DataSource = dataSource;
        }

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            NSTextField view = (NSTextField)tableView.MakeView(identifier, this);
            if (view == null)
            {
                view = new NSTextField();
                view.Identifier = identifier;
                view.BackgroundColor = NSColor.Clear;
                view.Bordered = false;
                view.Selectable = false;
                view.Editable = false;
            }

            // Setup view based on the column selected

            DirData lists = DataSource.names[(int)row];
            view.StringValue = lists.name;

            return view;
        }

    }
}

