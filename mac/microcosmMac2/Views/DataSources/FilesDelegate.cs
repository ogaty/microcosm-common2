using System;
using System.IO;
using AppKit;
using Foundation;

namespace microcosmMac2.Views.DataSources
{
    public class FilesDelegate : NSTableViewDelegate
    {
        private FilesDataSource DataSource;
        private string identifier = "cell";

        public FilesDelegate(FilesDataSource dataSource)
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
            view.StringValue = Path.GetFileName( DataSource.names[(int)row].name );

            return view;
        }

    }
}

