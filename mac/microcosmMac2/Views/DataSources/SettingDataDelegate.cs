using System;
using AppKit;

namespace microcosmMac2.Views.DataSources
{
    public class SettingDataDelegate : NSTableViewDelegate
    {
        private const string CellIdentifier = "ProdCell";

        private SettingDataSource DataSource;

        public SettingDataDelegate(SettingDataSource datasource)
        {
            this.DataSource = datasource;
        }

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            NSTextField view = (NSTextField)tableView.MakeView(CellIdentifier, this);
            if (view == null)
            {
                view = new NSTextField();
                view.Identifier = CellIdentifier;
                view.BackgroundColor = NSColor.Clear;
                view.Bordered = false;
                view.Selectable = false;
                view.Editable = false;
            }

            // Setup view based on the column selected
            view.StringValue = DataSource.names[(int)row].Title;

            return view;
        }
    }
}

