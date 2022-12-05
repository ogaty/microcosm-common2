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
	[Register ("DIrAddViewController")]
	partial class DIrAddViewController
	{
		[Outlet]
		AppKit.NSTextField fileName { get; set; }

		[Action ("SubmitClick:")]
		partial void SubmitClick (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (fileName != null) {
				fileName.Dispose ();
				fileName = null;
			}
		}
	}
}
