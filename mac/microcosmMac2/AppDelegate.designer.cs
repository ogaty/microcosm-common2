// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace microcosmMac2
{
	partial class AppDelegate
	{
		[Outlet]
		AppKit.NSMenuItem aspect11 { get; set; }

		[Outlet]
		AppKit.NSMenuItem aspect12 { get; set; }

		[Outlet]
		AppKit.NSMenuItem aspect13 { get; set; }

		[Outlet]
		AppKit.NSMenuItem aspect22 { get; set; }

		[Outlet]
		AppKit.NSMenuItem aspect23 { get; set; }

		[Outlet]
		AppKit.NSMenuItem aspect33 { get; set; }

		[Action ("AllAspectOff:")]
		partial void AllAspectOff (Foundation.NSObject sender);

		[Action ("AllAspectOn:")]
		partial void AllAspectOn (Foundation.NSObject sender);

		[Action ("Aspect11Click:")]
		partial void Aspect11Click (Foundation.NSObject sender);

		[Action ("Aspect12Click:")]
		partial void Aspect12Click (Foundation.NSObject sender);

		[Action ("Aspect13Click:")]
		partial void Aspect13Click (Foundation.NSObject sender);

		[Action ("Aspect22Click:")]
		partial void Aspect22Click (Foundation.NSObject sender);

		[Action ("Aspect23Click:")]
		partial void Aspect23Click (Foundation.NSObject sender);

		[Action ("Aspect33Click:")]
		partial void Aspect33Click (Foundation.NSObject sender);

		[Action ("Chart1E1:")]
		partial void Chart1E1 (Foundation.NSObject sender);

		[Action ("Chart1E2:")]
		partial void Chart1E2 (Foundation.NSObject sender);

		[Action ("Chart1U1:")]
		partial void Chart1U1 (Foundation.NSObject sender);

		[Action ("Chart1U2:")]
		partial void Chart1U2 (Foundation.NSObject sender);

		[Action ("Chart2EE:")]
		partial void Chart2EE (Foundation.NSObject sender);

		[Action ("Chart2UE:")]
		partial void Chart2UE (Foundation.NSObject sender);

		[Action ("Chart2UU:")]
		partial void Chart2UU (Foundation.NSObject sender);

		[Action ("Chart3NNC:")]
		partial void Chart3NNC (Foundation.NSObject sender);

		[Action ("Chart3NNT:")]
		partial void Chart3NNT (Foundation.NSObject sender);

		[Action ("Chart3NPT:")]
		partial void Chart3NPT (Foundation.NSObject sender);

		[Action ("Chart3NTT:")]
		partial void Chart3NTT (Foundation.NSObject sender);

		[Action ("CurrentChartOpen:")]
		partial void CurrentChartOpen (Foundation.NSObject sender);

		[Action ("OpenAddrCsv:")]
		partial void OpenAddrCsv (Foundation.NSObject sender);

		[Action ("OpenDbFolder:")]
		partial void OpenDbFolder (Foundation.NSObject sender);

		[Action ("OpenGithub:")]
		partial void OpenGithub (Foundation.NSObject sender);

		[Action ("OpenSabianCsv:")]
		partial void OpenSabianCsv (Foundation.NSObject sender);

		[Action ("SaveChartImage:")]
		partial void SaveChartImage (Foundation.NSObject sender);

		[Action ("Setting3Open:")]
		partial void Setting3Open (Foundation.NSObject sender);

		[Action ("ShowChart:")]
		partial void ShowChart (Foundation.NSObject sender);

		[Action ("ShowGrid:")]
		partial void ShowGrid (Foundation.NSObject sender);

		[Action ("ShowHelp:")]
		partial void ShowHelp (Foundation.NSObject sender);

		[Action ("ShowLicense:")]
		partial void ShowLicense (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (aspect11 != null) {
				aspect11.Dispose ();
				aspect11 = null;
			}

			if (aspect12 != null) {
				aspect12.Dispose ();
				aspect12 = null;
			}

			if (aspect13 != null) {
				aspect13.Dispose ();
				aspect13 = null;
			}

			if (aspect22 != null) {
				aspect22.Dispose ();
				aspect22 = null;
			}

			if (aspect23 != null) {
				aspect23.Dispose ();
				aspect23 = null;
			}

			if (aspect33 != null) {
				aspect33.Dispose ();
				aspect33 = null;
			}
		}
	}
}
