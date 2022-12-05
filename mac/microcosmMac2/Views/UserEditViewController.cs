using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using microcosmMac2.Common;
using microcosmMac2.Views.DataSources;
using System.Globalization;
using System.IO;
using microcosmMac2.User;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using static System.Net.WebRequestMethods;

namespace microcosmMac2.Views
{
    public partial class UserEditViewController : AppKit.NSViewController
    {
        public List<AddrCsv> addrs = new List<AddrCsv>();
        #region Constructors

        // Called when created from unmanaged code
        public UserEditViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public UserEditViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public UserEditViewController() : base("UserEditView", NSBundle.MainBundle)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion

        public override void ViewDidAppear()
        {
            base.ViewDidAppear();
            var root = Util.root;
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            string savedFile = appDelegate.dbSavedFile;

            file_name.StringValue = Path.GetFileNameWithoutExtension(savedFile);

        }

        //strongly typed view accessor
        public new UserEditView View
        {
            get
            {
                return (UserEditView)base.View;
            }
        }

        partial void SubmitClicked(Foundation.NSObject sender)
        {
            if (file_name.StringValue.IndexOf("/") > 0)
            {
                NSAlert alert = new NSAlert();
                alert.MessageText = "そのファイル名は登録できません。";
                alert.RunModal();
                return;
            }

            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            string savedFile = appDelegate.dbSavedFile;

            try
            {
                System.IO.File.Move(savedFile, appDelegate.dbSavedDirFullPath + @"/" + file_name.StringValue + ".json");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                NSAlert alert = new NSAlert();
                alert.MessageText = "エラーが発生しました。";
                alert.RunModal();

                return;
            }


            appDelegate.dbViewController.FilesRefresh(appDelegate.dbSavedDirFullPath);
            DismissController(this);
        }
    }
}
