using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using System.Diagnostics;

namespace microcosmMac2.Views
{
    public partial class DirListView : AppKit.NSTableView
    {
        #region Constructors

        // Called when created from unmanaged code
        public DirListView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        public DirListView()
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public DirListView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion

        public override void MouseDown(NSEvent theEvent)
        {
            base.MouseDown(theEvent);
            // せっかく作ったけどSelectionChangedのほうがUX的にいいや
            // サブフォルダはできないけど
            /*
            if (theEvent.ClickCount == 2)
            {
                Debug.WriteLine("2222");
                AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
                appDelegate.dbViewController.DirClick();

            }
            */
        }
    }
}
