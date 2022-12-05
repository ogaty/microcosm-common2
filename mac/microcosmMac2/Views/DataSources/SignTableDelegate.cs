using System;
using AppKit;
using microcosmMac2.Common;

namespace microcosmMac2.Views.DataSources
{
    public class SignTableDelegate : NSTableViewDelegate
    {
        private SignTableDataSource DataSource;
        private string identifier = "cell";

        public SignTableDelegate(SignTableDataSource datasource)
        {
            this.DataSource = datasource;
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

            SignTableData lists = DataSource.names[(int)row];
            switch (tableColumn.Title)
            {
                case "1":
                    view.StringValue = lists.degree1;
                    break;
                case "2":
                    view.StringValue = lists.degree2;
                    break;
                case "3":
                    view.StringValue = lists.degree3;
                    break;
                default:
                    view.StringValue = lists.sign;
                    break;
            }

            return view;
        }

    }
}

