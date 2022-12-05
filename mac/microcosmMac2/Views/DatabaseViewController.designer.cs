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
	[Register ("DatabaseViewController")]
	partial class DatabaseViewController
	{
		[Outlet]
		AppKit.NSButton AddDirButton { get; set; }

		[Outlet]
		AppKit.NSButton AddEventButton { get; set; }

		[Outlet]
		AppKit.NSTableColumn Birth { get; set; }

		[Outlet]
		AppKit.NSButton DeleteDirButton { get; set; }

		[Outlet]
		AppKit.NSButton DeleteEventButton { get; set; }

		[Outlet]
		AppKit.NSButton DeleteUserButton { get; set; }

		[Outlet]
		AppKit.NSTableView DirList { get; set; }

		[Outlet]
		AppKit.NSButton EclipseButton { get; set; }

		[Outlet]
		AppKit.NSButton EditDirButton { get; set; }

		[Outlet]
		AppKit.NSButton EditEventButton { get; set; }

		[Outlet]
		AppKit.NSButton EditUserButton { get; set; }

		[Outlet]
		AppKit.NSTextField event1Label { get; set; }

		[Outlet]
		AppKit.NSTextField event2Label { get; set; }

		[Outlet]
		AppKit.NSOutlineView eventList { get; set; }

		[Outlet]
		AppKit.NSTableView filesList { get; set; }

		[Outlet]
		AppKit.NSTableColumn FilesListColumn { get; set; }

		[Outlet]
		AppKit.NSPopUpButton ImportCombo { get; set; }

		[Outlet]
		AppKit.NSTextField memoField { get; set; }

		[Outlet]
		AppKit.NSTableColumn Name { get; set; }

		[Outlet]
		AppKit.NSTextField user1Label { get; set; }

		[Outlet]
		AppKit.NSTextField user2Label { get; set; }

		[Outlet]
		AppKit.NSTableView UserEventList { get; set; }

		[Action ("AddDirClicked:")]
		partial void AddDirClicked (Foundation.NSObject sender);

		[Action ("CloseClicked:")]
		partial void CloseClicked (Foundation.NSObject sender);

		[Action ("DeleteDirClicked:")]
		partial void DeleteDirClicked (Foundation.NSObject sender);

		[Action ("DeleteEventClicked:")]
		partial void DeleteEventClicked (Foundation.NSObject sender);

		[Action ("DeleteUserClicked:")]
		partial void DeleteUserClicked (Foundation.NSObject sender);

		[Action ("DirListClicked:")]
		partial void DirListClicked (Foundation.NSObject sender);

		[Action ("Event1Clicked:")]
		partial void Event1Clicked (Foundation.NSObject sender);

		[Action ("Event2Clicked:")]
		partial void Event2Clicked (Foundation.NSObject sender);

		[Action ("FilesCellClicked:")]
		partial void FilesCellClicked (Foundation.NSObject sender);

		[Action ("FilesListClicked:")]
		partial void FilesListClicked (Foundation.NSObject sender);

		[Action ("ImportClicked:")]
		partial void ImportClicked (Foundation.NSObject sender);

		[Action ("MoveDirClicked:")]
		partial void MoveDirClicked (Foundation.NSObject sender);

		[Action ("User1Clicked:")]
		partial void User1Clicked (Foundation.NSObject sender);

		[Action ("User2Clicked:")]
		partial void User2Clicked (Foundation.NSObject sender);

		[Action ("UserEventListClicked:")]
		partial void UserEventListClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (AddDirButton != null) {
				AddDirButton.Dispose ();
				AddDirButton = null;
			}

			if (AddEventButton != null) {
				AddEventButton.Dispose ();
				AddEventButton = null;
			}

			if (Birth != null) {
				Birth.Dispose ();
				Birth = null;
			}

			if (DeleteDirButton != null) {
				DeleteDirButton.Dispose ();
				DeleteDirButton = null;
			}

			if (DeleteEventButton != null) {
				DeleteEventButton.Dispose ();
				DeleteEventButton = null;
			}

			if (DeleteUserButton != null) {
				DeleteUserButton.Dispose ();
				DeleteUserButton = null;
			}

			if (DirList != null) {
				DirList.Dispose ();
				DirList = null;
			}

			if (EclipseButton != null) {
				EclipseButton.Dispose ();
				EclipseButton = null;
			}

			if (EditDirButton != null) {
				EditDirButton.Dispose ();
				EditDirButton = null;
			}

			if (EditEventButton != null) {
				EditEventButton.Dispose ();
				EditEventButton = null;
			}

			if (EditUserButton != null) {
				EditUserButton.Dispose ();
				EditUserButton = null;
			}

			if (event1Label != null) {
				event1Label.Dispose ();
				event1Label = null;
			}

			if (event2Label != null) {
				event2Label.Dispose ();
				event2Label = null;
			}

			if (eventList != null) {
				eventList.Dispose ();
				eventList = null;
			}

			if (filesList != null) {
				filesList.Dispose ();
				filesList = null;
			}

			if (FilesListColumn != null) {
				FilesListColumn.Dispose ();
				FilesListColumn = null;
			}

			if (memoField != null) {
				memoField.Dispose ();
				memoField = null;
			}

			if (Name != null) {
				Name.Dispose ();
				Name = null;
			}

			if (user1Label != null) {
				user1Label.Dispose ();
				user1Label = null;
			}

			if (user2Label != null) {
				user2Label.Dispose ();
				user2Label = null;
			}

			if (UserEventList != null) {
				UserEventList.Dispose ();
				UserEventList = null;
			}

			if (ImportCombo != null) {
				ImportCombo.Dispose ();
				ImportCombo = null;
			}
		}
	}
}
