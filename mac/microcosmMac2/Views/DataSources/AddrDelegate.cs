using System;
using AppKit;

namespace microcosmMac2.Views.DataSources
{
    public class AddrDelegate : NSTableViewDelegate
    {
        private AddrDataSource DataSource;
        private string identifier = "cell";

        public AddrDelegate(AddrDataSource dataSource)
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

            Addr lists = DataSource.names[(int)row];
            switch (tableColumn.Title)
            {
                case "地名":
                    view.StringValue = lists.name;
                    break;
                case "緯度":
                    view.StringValue = lists.lat;
                    break;
                case "経度":
                    view.StringValue = lists.lng;
                    break;
            }

            return view;
        }

    }
}

