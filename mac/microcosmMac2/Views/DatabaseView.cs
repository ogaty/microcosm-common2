using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace microcosmMac2.Views
{
    public partial class DatabaseView : AppKit.NSView
    {
        #region Constructors

        // Called when created from unmanaged code
        public DatabaseView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public DatabaseView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion

    }
}
