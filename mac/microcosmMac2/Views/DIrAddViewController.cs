using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using System.IO;
using System.Diagnostics;
using microcosmMac2.Common;

namespace microcosmMac2.Views
{
    public partial class DIrAddViewController : AppKit.NSViewController
    {
        #region Constructors

        // Called when created from unmanaged code
        public DIrAddViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public DIrAddViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public DIrAddViewController() : base("DIrAddView", NSBundle.MainBundle)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion

        //strongly typed view accessor
        public new DIrAddView View
        {
            get
            {
                return (DIrAddView)base.View;
            }
        }

        partial void SubmitClick(Foundation.NSObject sender)
        {
            if (fileName.StringValue.IndexOf("/") > 0)
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "そのファイル名は登録できません。";
                alert.RunModal();
                return;
            }

            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;

            try
            {
                Directory.CreateDirectory(Util.root + "/data/" + fileName.StringValue);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                NSAlert alert = new NSAlert();
                alert.MessageText = "エラーが発生しました。";
                alert.RunModal();

                return;
            }

            appDelegate.dbViewController.DirRefresh();
            DismissViewController(this);
        }
    }
}
