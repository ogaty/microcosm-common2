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
	[Register ("TimeSetViewController")]
	partial class TimeSetViewController
	{
		[Outlet]
		AppKit.NSTextField hour { get; set; }

		[Outlet]
		AppKit.NSTextField minute { get; set; }

		[Outlet]
		AppKit.NSTextField second { get; set; }

		[Outlet]
		AppKit.NSDatePicker timebox { get; set; }

		[Action ("SaveButtonClicked:")]
		partial void SaveButtonClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (timebox != null) {
				timebox.Dispose ();
				timebox = null;
			}

			if (hour != null) {
				hour.Dispose ();
				hour = null;
			}

			if (minute != null) {
				minute.Dispose ();
				minute = null;
			}

			if (second != null) {
				second.Dispose ();
				second = null;
			}
		}
	}
}
