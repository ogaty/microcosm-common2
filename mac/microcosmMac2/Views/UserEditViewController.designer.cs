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
	[Register ("UserEditViewController")]
	partial class UserEditViewController
	{
		[Outlet]
		AppKit.NSDatePickerCell event_date { get; set; }

		[Outlet]
		AppKit.NSTextField event_name { get; set; }

		[Outlet]
		AppKit.NSPopUpButton event_timezone { get; set; }

		[Outlet]
		AppKit.NSTextField file_name { get; set; }

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

			if (event_name != null) {
				event_name.Dispose ();
				event_name = null;
			}

			if (event_timezone != null) {
				event_timezone.Dispose ();
				event_timezone = null;
			}

			if (file_name != null) {
				file_name.Dispose ();
				file_name = null;
			}
		}
	}
}
