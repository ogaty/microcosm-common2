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
	[Register ("SpanViewController")]
	partial class SpanViewController
	{
		[Outlet]
		AppKit.NSButton radioDays { get; set; }

		[Outlet]
		AppKit.NSButton radioHours { get; set; }

		[Outlet]
		AppKit.NSButton radioMinutes { get; set; }

		[Outlet]
		AppKit.NSButton radioSeconds { get; set; }

		[Outlet]
		AppKit.NSPopUpButton spanCombo { get; set; }

		[Outlet]
		AppKit.NSTextField unit { get; set; }

		[Action ("radioDaysClicked:")]
		partial void radioDaysClicked (Foundation.NSObject sender);

		[Action ("radioHoursClicked:")]
		partial void radioHoursClicked (Foundation.NSObject sender);

		[Action ("radioMinutesClicked:")]
		partial void radioMinutesClicked (Foundation.NSObject sender);

		[Action ("radioSecondsClicked:")]
		partial void radioSecondsClicked (Foundation.NSObject sender);

		[Action ("SaveButtonClicked:")]
		partial void SaveButtonClicked (Foundation.NSObject sender);

		[Action ("spanComboChanged:")]
		partial void spanComboChanged (Foundation.NSObject sender);

		[Action ("spanComboChenged:")]
		partial void spanComboChenged (Foundation.NSObject sender);

		[Action ("spanPopupChanged:")]
		partial void spanPopupChanged (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (radioDays != null) {
				radioDays.Dispose ();
				radioDays = null;
			}

			if (radioHours != null) {
				radioHours.Dispose ();
				radioHours = null;
			}

			if (radioMinutes != null) {
				radioMinutes.Dispose ();
				radioMinutes = null;
			}

			if (radioSeconds != null) {
				radioSeconds.Dispose ();
				radioSeconds = null;
			}

			if (spanCombo != null) {
				spanCombo.Dispose ();
				spanCombo = null;
			}

			if (unit != null) {
				unit.Dispose ();
				unit = null;
			}
		}
	}
}
