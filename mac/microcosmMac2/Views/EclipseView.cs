using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace microcosmMac2.Views
{
    public partial class EclipseView : AppKit.NSView
    {
        #region Constructors

        // Called when created from unmanaged code
        public EclipseView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public EclipseView(NSCoder coder) : base(coder)
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
