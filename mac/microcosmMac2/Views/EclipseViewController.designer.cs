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
	[Register ("EclipseViewController")]
	partial class EclipseViewController
	{
		[Outlet]
		AppKit.NSButton futureRadio { get; set; }

		[Outlet]
		AppKit.NSButton pastRadio { get; set; }

		[Outlet]
		AppKit.NSPopUpButton planetList { get; set; }

		[Outlet]
		AppKit.NSTextField targetDate { get; set; }

		[Outlet]
		AppKit.NSTextField targetDegree { get; set; }

		[Action ("futureRadioClick:")]
		partial void futureRadioClick (Foundation.NSObject sender);

		[Action ("pastRadioClick:")]
		partial void pastRadioClick (Foundation.NSObject sender);

		[Action ("planetListSelectionChanged:")]
		partial void planetListSelectionChanged (Foundation.NSObject sender);

		[Action ("SubmitClicked:")]
		partial void SubmitClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (futureRadio != null) {
				futureRadio.Dispose ();
				futureRadio = null;
			}

			if (pastRadio != null) {
				pastRadio.Dispose ();
				pastRadio = null;
			}

			if (planetList != null) {
				planetList.Dispose ();
				planetList = null;
			}

			if (targetDate != null) {
				targetDate.Dispose ();
				targetDate = null;
			}

			if (targetDegree != null) {
				targetDegree.Dispose ();
				targetDegree = null;
			}
		}
	}
}
