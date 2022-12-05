// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace microcosmMac2.Views
{
	[Register ("EventEditViewController")]
	partial class EventEditViewController
	{
		[Outlet]
		AppKit.NSDatePicker event_date { get; set; }

		[Outlet]
		AppKit.NSTextField event_hour { get; set; }

		[Outlet]
		AppKit.NSTextField event_lat { get; set; }

		[Outlet]
		AppKit.NSTextField event_lng { get; set; }

		[Outlet]
		AppKit.NSTextView event_memo { get; set; }

		[Outlet]
		AppKit.NSTextField event_minute { get; set; }

		[Outlet]
		AppKit.NSTextField event_name { get; set; }

		[Outlet]
		AppKit.NSTextField event_place { get; set; }

		[Outlet]
		AppKit.NSTextField event_second { get; set; }

		[Outlet]
		AppKit.NSPopUpButton event_timezone { get; set; }

		[Outlet]
		AppKit.NSTextField file_name { get; set; }

		[Outlet]
		AppKit.NSTableView LatLngTable { get; set; }

		[Outlet]
		AppKit.NSTextField memo { get; set; }

		[Action ("LatLngTableClicked:")]
		partial void LatLngTableClicked (Foundation.NSObject sender);

		[Action ("SubmitClicked:")]
		partial void SubmitClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (event_date != null) {
				event_date.Dispose ();
				event_date = null;
			}

			if (event_hour != null) {
				event_hour.Dispose ();
				event_hour = null;
			}

			if (event_lat != null) {
				event_lat.Dispose ();
				event_lat = null;
			}

			if (event_lng != null) {
				event_lng.Dispose ();
				event_lng = null;
			}

			if (event_memo != null) {
				event_memo.Dispose ();
				event_memo = null;
			}

			if (event_minute != null) {
				event_minute.Dispose ();
				event_minute = null;
			}

			if (event_name != null) {
				event_name.Dispose ();
				event_name = null;
			}

			if (event_place != null) {
				event_place.Dispose ();
				event_place = null;
			}

			if (event_second != null) {
				event_second.Dispose ();
				event_second = null;
			}

			if (event_timezone != null) {
				event_timezone.Dispose ();
				event_timezone = null;
			}

			if (file_name != null) {
				file_name.Dispose ();
				file_name = null;
			}

			if (LatLngTable != null) {
				LatLngTable.Dispose ();
				LatLngTable = null;
			}

			if (memo != null) {
				memo.Dispose ();
				memo = null;
			}
		}
	}
}
