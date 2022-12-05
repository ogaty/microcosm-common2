using System;
using System.IO;
using AppKit;

namespace microcosmMac2.Views.DataSources
{
    public class EventDelegate : NSTableViewDelegate
    {
        private EventDataSource DataSource;
        private string identifier = "cell";

        public EventDelegate(EventDataSource dataSource)
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

            EventData lists = DataSource.names[(int)row];
            switch (tableColumn.Title)
            {
                case "index":
                    view.StringValue = lists.index;
                    break;
                case "Name":
                    view.StringValue = lists.eventName;
                    break;
                case "Birth":
                    view.StringValue = lists.eventBirth;
                    break;
                case "Timezone":
                    view.StringValue = lists.eventTimezone;
                    break;
                case "Place":
                    view.StringValue = lists.eventPlace;
                    break;
                case "Lat":
                    view.StringValue = lists.eventLat;
                    break;
                case "Lng":
                    view.StringValue = lists.eventLng;
                    break;
                case "Memo":
                    view.StringValue = lists.eventMemo;
                    break;
            }

            return view;
        }

    }
}

