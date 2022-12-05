using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using microcosmMac2.Common;
using microcosmMac2.Models;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace microcosmMac2.Views
{
    public partial class ShortcutViewController : AppKit.NSViewController
    {
        #region Constructors

        // Called when created from unmanaged code
        public ShortcutViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public ShortcutViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public ShortcutViewController() : base("ShortcutView", NSBundle.MainBundle)
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

            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;

            Dictionary<EShortCut, string> shortCutList = Util.createShortCut();

            ctrlHShortCut.RemoveAllItems();
            ctrlJShortCut.RemoveAllItems();
            ctrlKShortCut.RemoveAllItems();
            ctrlLShorCut.RemoveAllItems();
            ctrlNShortCut.RemoveAllItems();
            ctrlMShortCut.RemoveAllItems();
            ctrlCommaShortCut.RemoveAllItems();
            ctrlDotShortCut.RemoveAllItems();
            ctrlOpenBracketShortCut.RemoveAllItems();
            ctrlCloseBracketShortCut.RemoveAllItems();
            ctrl0ShortCut.RemoveAllItems();
            ctrl1ShortCut.RemoveAllItems();
            ctrl2ShortCut.RemoveAllItems();
            ctrl3ShortCut.RemoveAllItems();
            ctrl4ShortCut.RemoveAllItems();
            ctrl5ShortCut.RemoveAllItems();
            ctrl6ShortCut.RemoveAllItems();
            ctrl7ShortCut.RemoveAllItems();
            ctrl8ShortCut.RemoveAllItems();
            ctrl9ShortCut.RemoveAllItems();
            F1ShorCut.RemoveAllItems();
            F2ShortCut.RemoveAllItems();
            F3ShortCut.RemoveAllItems();
            F4ShortCut.RemoveAllItems();
            F5ShortCut.RemoveAllItems();
            F6ShortCut.RemoveAllItems();
            F7ShortCut.RemoveAllItems();
            F8ShortCut.RemoveAllItems();
            F9ShortCut.RemoveAllItems();
            F10ShortCut.RemoveAllItems();
            foreach (KeyValuePair<EShortCut, string> s in shortCutList)
            {
                ctrlHShortCut.AddItem(s.Value);
                ctrlJShortCut.AddItem(s.Value);
                ctrlKShortCut.AddItem(s.Value);
                ctrlLShorCut.AddItem(s.Value);
                ctrlNShortCut.AddItem(s.Value);
                ctrlMShortCut.AddItem(s.Value);
                ctrlCommaShortCut.AddItem(s.Value);
                ctrlDotShortCut.AddItem(s.Value);
                ctrlOpenBracketShortCut.AddItem(s.Value);
                ctrlCloseBracketShortCut.AddItem(s.Value);
                ctrl0ShortCut.AddItem(s.Value);
                ctrl1ShortCut.AddItem(s.Value);
                ctrl2ShortCut.AddItem(s.Value);
                ctrl3ShortCut.AddItem(s.Value);
                ctrl4ShortCut.AddItem(s.Value);
                ctrl5ShortCut.AddItem(s.Value);
                ctrl6ShortCut.AddItem(s.Value);
                ctrl7ShortCut.AddItem(s.Value);
                ctrl8ShortCut.AddItem(s.Value);
                ctrl9ShortCut.AddItem(s.Value);
                F1ShorCut.AddItem(s.Value);
                F2ShortCut.AddItem(s.Value);
                F3ShortCut.AddItem(s.Value);
                F4ShortCut.AddItem(s.Value);
                F5ShortCut.AddItem(s.Value);
                F6ShortCut.AddItem(s.Value);
                F7ShortCut.AddItem(s.Value);
                F8ShortCut.AddItem(s.Value);
                F9ShortCut.AddItem(s.Value);
                F10ShortCut.AddItem(s.Value);
            }


            ctrlHShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrlH]);
            ctrlJShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrlJ]);
            ctrlKShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrlK]);
            ctrlLShorCut.SelectItem(shortCutList[appDelegate.shortCut.ctrlL]);
            ctrlNShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrlN]);
            ctrlMShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrlM]);
            ctrlCommaShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrlComma]);
            ctrlDotShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrlDot]);
            ctrlOpenBracketShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrlOpenBracket]);
            ctrlCloseBracketShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrlCloseBracket]);
            ctrl0ShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrl0]);
            ctrl1ShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrl1]);
            ctrl2ShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrl2]);
            ctrl3ShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrl3]);
            ctrl4ShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrl4]);
            ctrl5ShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrl5]);
            ctrl6ShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrl6]);
            ctrl7ShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrl7]);
            ctrl8ShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrl8]);
            ctrl9ShortCut.SelectItem(shortCutList[appDelegate.shortCut.ctrl9]);
            F1ShorCut.SelectItem(shortCutList[appDelegate.shortCut.F1]);
            F2ShortCut.SelectItem(shortCutList[appDelegate.shortCut.F2]);
            F3ShortCut.SelectItem(shortCutList[appDelegate.shortCut.F3]);
            F4ShortCut.SelectItem(shortCutList[appDelegate.shortCut.F4]);
            F5ShortCut.SelectItem(shortCutList[appDelegate.shortCut.F5]);
            F6ShortCut.SelectItem(shortCutList[appDelegate.shortCut.F6]);
            F7ShortCut.SelectItem(shortCutList[appDelegate.shortCut.F7]);
            F8ShortCut.SelectItem(shortCutList[appDelegate.shortCut.F8]);
            F9ShortCut.SelectItem(shortCutList[appDelegate.shortCut.F9]);
            F10ShortCut.SelectItem(shortCutList[appDelegate.shortCut.F10]);
        }

        //strongly typed view accessor
        public new ShortcutView View
        {
            get
            {
                return (ShortcutView)base.View;
            }
        }

        partial void SubmitClick(Foundation.NSObject sender)
        {
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            appDelegate.shortCut.ctrlH = Util.ShortCutStringToEnum(ctrlHShortCut.SelectedItem.Title);
            appDelegate.shortCut.ctrlJ = Util.ShortCutStringToEnum(ctrlJShortCut.SelectedItem.Title);
            appDelegate.shortCut.ctrlK = Util.ShortCutStringToEnum(ctrlKShortCut.SelectedItem.Title);
            appDelegate.shortCut.ctrlL = Util.ShortCutStringToEnum(ctrlLShorCut.SelectedItem.Title);
            appDelegate.shortCut.ctrlN = Util.ShortCutStringToEnum(ctrlNShortCut.SelectedItem.Title);
            appDelegate.shortCut.ctrlM = Util.ShortCutStringToEnum(ctrlMShortCut.SelectedItem.Title);
            appDelegate.shortCut.ctrlComma = Util.ShortCutStringToEnum(ctrlCommaShortCut.SelectedItem.Title);
            appDelegate.shortCut.ctrlDot = Util.ShortCutStringToEnum(ctrlDotShortCut.SelectedItem.Title);
            appDelegate.shortCut.ctrlOpenBracket = Util.ShortCutStringToEnum(ctrlOpenBracketShortCut.SelectedItem.Title);
            appDelegate.shortCut.ctrlCloseBracket = Util.ShortCutStringToEnum(ctrlCloseBracketShortCut.SelectedItem.Title);
            appDelegate.shortCut.ctrl0 = Util.ShortCutStringToEnum(ctrl0ShortCut.SelectedItem.Title);
            appDelegate.shortCut.ctrl1 = Util.ShortCutStringToEnum(ctrl1ShortCut.SelectedItem.Title);
            appDelegate.shortCut.ctrl2 = Util.ShortCutStringToEnum(ctrl2ShortCut.SelectedItem.Title);
            appDelegate.shortCut.ctrl3 = Util.ShortCutStringToEnum(ctrl3ShortCut.SelectedItem.Title);
            appDelegate.shortCut.ctrl4 = Util.ShortCutStringToEnum(ctrl4ShortCut.SelectedItem.Title);
            appDelegate.shortCut.ctrl5 = Util.ShortCutStringToEnum(ctrl5ShortCut.SelectedItem.Title);
            appDelegate.shortCut.ctrl6 = Util.ShortCutStringToEnum(ctrl6ShortCut.SelectedItem.Title);
            appDelegate.shortCut.ctrl7 = Util.ShortCutStringToEnum(ctrl7ShortCut.SelectedItem.Title);
            appDelegate.shortCut.ctrl8 = Util.ShortCutStringToEnum(ctrl8ShortCut.SelectedItem.Title);
            appDelegate.shortCut.ctrl9 = Util.ShortCutStringToEnum(ctrl9ShortCut.SelectedItem.Title);
            appDelegate.shortCut.F1 = Util.ShortCutStringToEnum(F1ShorCut.SelectedItem.Title);
            appDelegate.shortCut.F2 = Util.ShortCutStringToEnum(F2ShortCut.SelectedItem.Title);
            appDelegate.shortCut.F3 = Util.ShortCutStringToEnum(F3ShortCut.SelectedItem.Title);
            appDelegate.shortCut.F4 = Util.ShortCutStringToEnum(F4ShortCut.SelectedItem.Title);
            appDelegate.shortCut.F5 = Util.ShortCutStringToEnum(F5ShortCut.SelectedItem.Title);
            appDelegate.shortCut.F6 = Util.ShortCutStringToEnum(F6ShortCut.SelectedItem.Title);
            appDelegate.shortCut.F7 = Util.ShortCutStringToEnum(F7ShortCut.SelectedItem.Title);
            appDelegate.shortCut.F8 = Util.ShortCutStringToEnum(F8ShortCut.SelectedItem.Title);
            appDelegate.shortCut.F9 = Util.ShortCutStringToEnum(F9ShortCut.SelectedItem.Title);
            appDelegate.shortCut.F10 = Util.ShortCutStringToEnum(F10ShortCut.SelectedItem.Title);

            appDelegate.keyEventCtrl[4] = appDelegate.shortCut.ctrlH;
            appDelegate.keyEventCtrl[38] = appDelegate.shortCut.ctrlJ;
            appDelegate.keyEventCtrl[40] = appDelegate.shortCut.ctrlK;
            appDelegate.keyEventCtrl[37] = appDelegate.shortCut.ctrlL;
            appDelegate.keyEventCtrl[45] = appDelegate.shortCut.ctrlN;
            appDelegate.keyEventCtrl[46] = appDelegate.shortCut.ctrlM;
            appDelegate.keyEventCtrl[43] = appDelegate.shortCut.ctrlComma;
            appDelegate.keyEventCtrl[47] = appDelegate.shortCut.ctrlDot;
            appDelegate.keyEventCtrl[33] = appDelegate.shortCut.ctrlOpenBracket;
            appDelegate.keyEventCtrl[30] = appDelegate.shortCut.ctrlCloseBracket;
            appDelegate.keyEventCtrl[29] = appDelegate.shortCut.ctrl0;
            appDelegate.keyEventCtrl[18] = appDelegate.shortCut.ctrl1;
            appDelegate.keyEventCtrl[19] = appDelegate.shortCut.ctrl2;
            appDelegate.keyEventCtrl[20] = appDelegate.shortCut.ctrl3;
            appDelegate.keyEventCtrl[21] = appDelegate.shortCut.ctrl4;
            appDelegate.keyEventCtrl[23] = appDelegate.shortCut.ctrl5;
            appDelegate.keyEventCtrl[22] = appDelegate.shortCut.ctrl6;
            appDelegate.keyEventCtrl[26] = appDelegate.shortCut.ctrl7;
            appDelegate.keyEventCtrl[28] = appDelegate.shortCut.ctrl8;
            appDelegate.keyEventCtrl[25] = appDelegate.shortCut.ctrl9;
            appDelegate.keyEvent[122] = appDelegate.shortCut.F1;
            appDelegate.keyEvent[120] = appDelegate.shortCut.F2;
            appDelegate.keyEvent[99] = appDelegate.shortCut.F3;
            appDelegate.keyEvent[118] = appDelegate.shortCut.F4;
            appDelegate.keyEvent[96] = appDelegate.shortCut.F5;
            appDelegate.keyEvent[97] = appDelegate.shortCut.F6;
            appDelegate.keyEvent[98] = appDelegate.shortCut.F7;
            appDelegate.keyEvent[100] = appDelegate.shortCut.F8;
            appDelegate.keyEvent[101] = appDelegate.shortCut.F9;
            appDelegate.keyEvent[109] = appDelegate.shortCut.F10;

            string filename = Util.root + "/system/shortcut.json";

            string shortCutJson = JsonSerializer.Serialize(appDelegate.shortCut,
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true
                });
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(shortCutJson);
                sw.Close();
            }

            DismissController(this);
        }
    }
}
