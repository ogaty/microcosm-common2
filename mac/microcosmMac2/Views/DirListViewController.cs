using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace microcosmMac2.Views
{
    public partial class DirListViewController : AppKit.NSViewController
    {
        #region Constructors

        // Called when created from unmanaged code
        public DirListViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public DirListViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public DirListViewController() : base("DirListView", NSBundle.MainBundle)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion

        //strongly typed view accessor
        public new DirListView View
        {
            get
            {
                return (DirListView)base.View;
            }
        }
    }
}
