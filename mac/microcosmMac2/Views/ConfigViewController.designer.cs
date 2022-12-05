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
	[Register ("ConfigViewController")]
	partial class ConfigViewController
	{
		[Outlet]
		AppKit.NSButton campanus { get; set; }

		[Outlet]
		AppKit.NSButton cps { get; set; }

		[Outlet]
		AppKit.NSButton deci { get; set; }

		[Outlet]
		AppKit.NSTextField defaultLat { get; set; }

		[Outlet]
		AppKit.NSTextField defaultLng { get; set; }

		[Outlet]
		AppKit.NSTextField defaultPlace { get; set; }

		[Outlet]
		AppKit.NSPopUpButton defaultTimezone { get; set; }

		[Outlet]
		AppKit.NSButton equal { get; set; }

		[Outlet]
		AppKit.NSButton fulldisp { get; set; }

		[Outlet]
		AppKit.NSButton geoCentric { get; set; }

		[Outlet]
		AppKit.NSButton helioCentric { get; set; }

		[Outlet]
		AppKit.NSButton hex { get; set; }

		[Outlet]
		AppKit.NSButton koch { get; set; }

		[Outlet]
		AppKit.NSTableView LatLngTable { get; set; }

		[Outlet]
		AppKit.NSButton meanapogee { get; set; }

		[Outlet]
		AppKit.NSButton meannode { get; set; }

		[Outlet]
		AppKit.NSButton osculatingapogee { get; set; }

		[Outlet]
		AppKit.NSButton placidus { get; set; }

		[Outlet]
		AppKit.NSButton primary { get; set; }

		[Outlet]
		AppKit.NSTextField savedLabel { get; set; }

		[Outlet]
		AppKit.NSButton secondary { get; set; }

		[Outlet]
		AppKit.NSButton sidereal { get; set; }

		[Outlet]
		AppKit.NSButton simpledisp { get; set; }

		[Outlet]
		AppKit.NSButton solararc { get; set; }

		[Outlet]
		AppKit.NSButton tropical { get; set; }

		[Outlet]
		AppKit.NSButton truenode { get; set; }

		[Outlet]
		AppKit.NSButton zeroaries { get; set; }

		[Action ("CampanusClick:")]
		partial void CampanusClick (Foundation.NSObject sender);

		[Action ("CancelClick:")]
		partial void CancelClick (Foundation.NSObject sender);

		[Action ("CpsClick:")]
		partial void CpsClick (Foundation.NSObject sender);

		[Action ("DecimalClick:")]
		partial void DecimalClick (Foundation.NSObject sender);

		[Action ("EqualClick:")]
		partial void EqualClick (Foundation.NSObject sender);

		[Action ("FullDisp:")]
		partial void FullDisp (Foundation.NSObject sender);

		[Action ("GeoCentricClick:")]
		partial void GeoCentricClick (Foundation.NSObject sender);

		[Action ("HelioCentricClick:")]
		partial void HelioCentricClick (Foundation.NSObject sender);

		[Action ("HexClick:")]
		partial void HexClick (Foundation.NSObject sender);

		[Action ("KochClick:")]
		partial void KochClick (Foundation.NSObject sender);

		[Action ("LatLngTableClicked:")]
		partial void LatLngTableClicked (Foundation.NSObject sender);

		[Action ("MeanApogee:")]
		partial void MeanApogee (Foundation.NSObject sender);

		[Action ("MeanApogeeClick:")]
		partial void MeanApogeeClick (Foundation.NSObject sender);

		[Action ("MeanNodeClick:")]
		partial void MeanNodeClick (Foundation.NSObject sender);

		[Action ("OsculatingApogeeClick:")]
		partial void OsculatingApogeeClick (Foundation.NSObject sender);

		[Action ("PlacidusClick:")]
		partial void PlacidusClick (Foundation.NSObject sender);

		[Action ("PrimaryProgressionClick:")]
		partial void PrimaryProgressionClick (Foundation.NSObject sender);

		[Action ("SecondaryProgressionClick:")]
		partial void SecondaryProgressionClick (Foundation.NSObject sender);

		[Action ("SiderealClick:")]
		partial void SiderealClick (Foundation.NSObject sender);

		[Action ("SimpleDispClick:")]
		partial void SimpleDispClick (Foundation.NSObject sender);

		[Action ("SolarArcProgressionClick:")]
		partial void SolarArcProgressionClick (Foundation.NSObject sender);

		[Action ("SubmitClick:")]
		partial void SubmitClick (Foundation.NSObject sender);

		[Action ("TropicalClick:")]
		partial void TropicalClick (Foundation.NSObject sender);

		[Action ("TrueNodeClick:")]
		partial void TrueNodeClick (Foundation.NSObject sender);

		[Action ("ZeroAriesClick:")]
		partial void ZeroAriesClick (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (campanus != null) {
				campanus.Dispose ();
				campanus = null;
			}

			if (cps != null) {
				cps.Dispose ();
				cps = null;
			}

			if (deci != null) {
				deci.Dispose ();
				deci = null;
			}

			if (defaultLat != null) {
				defaultLat.Dispose ();
				defaultLat = null;
			}

			if (defaultLng != null) {
				defaultLng.Dispose ();
				defaultLng = null;
			}

			if (defaultPlace != null) {
				defaultPlace.Dispose ();
				defaultPlace = null;
			}

			if (defaultTimezone != null) {
				defaultTimezone.Dispose ();
				defaultTimezone = null;
			}

			if (equal != null) {
				equal.Dispose ();
				equal = null;
			}

			if (fulldisp != null) {
				fulldisp.Dispose ();
				fulldisp = null;
			}

			if (geoCentric != null) {
				geoCentric.Dispose ();
				geoCentric = null;
			}

			if (helioCentric != null) {
				helioCentric.Dispose ();
				helioCentric = null;
			}

			if (hex != null) {
				hex.Dispose ();
				hex = null;
			}

			if (koch != null) {
				koch.Dispose ();
				koch = null;
			}

			if (LatLngTable != null) {
				LatLngTable.Dispose ();
				LatLngTable = null;
			}

			if (meanapogee != null) {
				meanapogee.Dispose ();
				meanapogee = null;
			}

			if (meannode != null) {
				meannode.Dispose ();
				meannode = null;
			}

			if (osculatingapogee != null) {
				osculatingapogee.Dispose ();
				osculatingapogee = null;
			}

			if (placidus != null) {
				placidus.Dispose ();
				placidus = null;
			}

			if (primary != null) {
				primary.Dispose ();
				primary = null;
			}

			if (savedLabel != null) {
				savedLabel.Dispose ();
				savedLabel = null;
			}

			if (secondary != null) {
				secondary.Dispose ();
				secondary = null;
			}

			if (sidereal != null) {
				sidereal.Dispose ();
				sidereal = null;
			}

			if (simpledisp != null) {
				simpledisp.Dispose ();
				simpledisp = null;
			}

			if (solararc != null) {
				solararc.Dispose ();
				solararc = null;
			}

			if (tropical != null) {
				tropical.Dispose ();
				tropical = null;
			}

			if (truenode != null) {
				truenode.Dispose ();
				truenode = null;
			}

			if (zeroaries != null) {
				zeroaries.Dispose ();
				zeroaries = null;
			}
		}
	}
}
