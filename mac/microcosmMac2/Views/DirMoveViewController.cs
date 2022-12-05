using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using microcosmMac2.Common;
using microcosmMac2.Views.DataSources;
using System.Diagnostics;
using System.IO;

namespace microcosmMac2.Views
{
    public partial class DirMoveViewController : AppKit.NSViewController
    {
        #region Constructors

        // Called when created from unmanaged code
        public DirMoveViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public DirMoveViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public DirMoveViewController() : base("DirMoveView", NSBundle.MainBundle)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var root = Util.root;
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            string dbSavedDir = appDelegate.dbSavedDir;

            fileName.StringValue = dbSavedDir;
        }

        //strongly typed view accessor
        public new DirMoveView View
        {
            get
            {
                return (DirMoveView)base.View;
            }
        }

        partial void SubmitClicked(Foundation.NSObject sender)
        {
            if (fileName.StringValue.IndexOf("/") > 0)
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "そのファイル名は登録できません。";
                alert.RunModal();
                return;
            }

            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            string savedFile = appDelegate.dbSavedDirFullPath;

            try
            {
                System.IO.Directory.Move(savedFile, Util.root + "/data/" + fileName.StringValue);
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
            DismissController(this);

        }
    }
}
