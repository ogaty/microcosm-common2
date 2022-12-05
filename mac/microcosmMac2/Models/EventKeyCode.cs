using System;
using System.Collections.Generic;
using System.Diagnostics;
using AppKit;
using microcosmMac2.Common;
using static CoreMedia.CMTime;

namespace microcosmMac2.Models
{
    public class EventKeyCode
    {
        NSEvent e;
        public EventKeyCode(NSEvent ev)
        {
            e = ev;
        }

        public void GetEvent()
        {
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;

            NSEventModifierMask flag = e.ModifierFlags;

            if (flag.HasFlag(NSEventModifierMask.ControlKeyMask))
            {
                if (appDelegate.keyEventCtrl.ContainsKey(e.KeyCode)) ProcEvent(appDelegate.keyEventCtrl[e.KeyCode]);
            }
            else if (flag.HasFlag(NSEventModifierMask.CommandKeyMask))
            {
                if (flag.HasFlag(NSEventModifierMask.ShiftKeyMask))
                {
                    //if (appDelegate.keyEventCmdShift.ContainsKey(e.KeyCode)) ProcEvent(appDelegate.keyEventCmdShift[e.KeyCode]);
                }
                else
                {
                    //if (appDelegate.keyEventCmd.ContainsKey(e.KeyCode)) ProcEvent(appDelegate.keyEventCmd[e.KeyCode]);
                }
            }
            else
            {
                // Functionキーはctrlいらない
                if (appDelegate.keyEvent.ContainsKey(e.KeyCode)) ProcEvent(appDelegate.keyEvent[e.KeyCode]);
            }
        }

        public void ProcEvent(EShortCut shortCut)
        {
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;

            switch (shortCut)
            {
                case EShortCut.Noop:
                    Debug.WriteLine("noop");
                    break;
                case EShortCut.Plus1Day:
                    appDelegate.viewController.TimeSetAny(86400);
                    break;
                case EShortCut.Minus1Day:
                    appDelegate.viewController.TimeSetAny(-86400);
                    break;
                case EShortCut.Plus1Hour:
                    appDelegate.viewController.TimeSetAny(3600);
                    break;
                case EShortCut.Minus1Hour:
                    appDelegate.viewController.TimeSetAny(-3600);
                    break;
                case EShortCut.Plus12Hour:
                    appDelegate.viewController.TimeSetAny(3600 * 12);
                    break;
                case EShortCut.Minus12Hour:
                    appDelegate.viewController.TimeSetAny(-3600 * 12);
                    break;
                case EShortCut.Plus1Minute:
                    appDelegate.viewController.TimeSetAny(60);
                    break;
                case EShortCut.Minus1Minute:
                    appDelegate.viewController.TimeSetAny(-60);
                    break;
                case EShortCut.Plus1Second:
                    appDelegate.viewController.TimeSetAny(1);
                    break;
                case EShortCut.Minus1Second:
                    appDelegate.viewController.TimeSetAny(-1);
                    break;
                case EShortCut.Plus7Day:
                    appDelegate.viewController.TimeSetAny(86400 * 7);
                    break;
                case EShortCut.Minus7Day:
                    appDelegate.viewController.TimeSetAny(-86400 * 7);
                    break;
                case EShortCut.Plus30Day:
                    appDelegate.viewController.TimeSetAny(86400 * 30);
                    break;
                case EShortCut.Minus30Day:
                    appDelegate.viewController.TimeSetAny(-86400 * 30);
                    break;
                case EShortCut.Plus365Day:
                    appDelegate.viewController.TimeSetAny(86400 * 365);
                    break;
                case EShortCut.Minus365Day:
                    appDelegate.viewController.TimeSetAny(-86400 * 365);
                    break;
                case EShortCut.ChagngeSetting0:
                    appDelegate.viewController.SettingIndexChange(0);
                    break;
                case EShortCut.ChagngeSetting1:
                    appDelegate.viewController.SettingIndexChange(1);
                    break;
                case EShortCut.ChagngeSetting2:
                    appDelegate.viewController.SettingIndexChange(2);
                    break;
                case EShortCut.ChagngeSetting3:
                    appDelegate.viewController.SettingIndexChange(3);
                    break;
                case EShortCut.ChagngeSetting4:
                    appDelegate.viewController.SettingIndexChange(4);
                    break;
                case EShortCut.ChagngeSetting5:
                    appDelegate.viewController.SettingIndexChange(5);
                    break;
                case EShortCut.ChagngeSetting6:
                    appDelegate.viewController.SettingIndexChange(6);
                    break;
                case EShortCut.ChagngeSetting7:
                    appDelegate.viewController.SettingIndexChange(7);
                    break;
                case EShortCut.ChagngeSetting8:
                    appDelegate.viewController.SettingIndexChange(8);
                    break;
                case EShortCut.ChagngeSetting9:
                    appDelegate.viewController.SettingIndexChange(9);
                    break;
                case EShortCut.Ring1U1:
                    appDelegate.viewController.Chart1U1();
                    break;
                case EShortCut.Ring1U2:
                    appDelegate.viewController.Chart1U2();
                    break;
                case EShortCut.Ring1E1:
                    appDelegate.viewController.Chart1E1();
                    break;
                case EShortCut.Ring1E2:
                    appDelegate.viewController.Chart1E2();
                    break;
                case EShortCut.Ring1Current:
                    appDelegate.viewController.TimeSetNowCurrentBand();
                    appDelegate.viewController.Chart1U1();
                    break;
                case EShortCut.Ring2UU:
                    appDelegate.viewController.Chart2UU();
                    break;
                case EShortCut.Ring2UE:
                    appDelegate.viewController.Chart2UE();
                    break;
                case EShortCut.Ring3NPT:
                    appDelegate.viewController.Chart3NPT();
                    break;
                case EShortCut.InvisibleAllAspect:
                    appDelegate.viewController.AspectAllOff();
                    break;
                case EShortCut.VisibleAllAspect:
                    appDelegate.viewController.AspectAllOn();
                    break;
                case EShortCut.InVisible11:
                    appDelegate.viewController.AspectOff(0);
                    break;
                case EShortCut.InVisible12:
                    appDelegate.viewController.AspectOff(1);
                    break;
                case EShortCut.InVisible13:
                    appDelegate.viewController.AspectOff(2);
                    break;
                case EShortCut.InVisible22:
                    appDelegate.viewController.AspectOff(3);
                    break;
                case EShortCut.InVisible23:
                    appDelegate.viewController.AspectOff(4);
                    break;
                case EShortCut.InVisible33:
                    appDelegate.viewController.AspectOff(5);
                    break;
                case EShortCut.Visible11:
                    appDelegate.viewController.AspectOn(0);
                    break;
                case EShortCut.Visible12:
                    appDelegate.viewController.AspectOn(1);
                    break;
                case EShortCut.Visible13:
                    appDelegate.viewController.AspectOn(2);
                    break;
                case EShortCut.Visible22:
                    appDelegate.viewController.AspectOn(3);
                    break;
                case EShortCut.Visible23:
                    appDelegate.viewController.AspectOn(4);
                    break;
                case EShortCut.Visible33:
                    appDelegate.viewController.AspectOn(5);
                    break;
            }
        }

        public void DebugKeyCode(int keyCode)
        {
            NSEventModifierMask flag = e.ModifierFlags;

            if (e.KeyCode == 49) Debug.WriteLine("space");
            else if (e.KeyCode == 43) Debug.WriteLine("<");
            else if (e.KeyCode == 47) Debug.WriteLine(">");
            else if (e.KeyCode == 30) Debug.WriteLine("[");
            else if (e.KeyCode == 42) Debug.WriteLine("]");
            else if (e.KeyCode == 27) Debug.WriteLine("-");
            else if (e.KeyCode == 18) Debug.WriteLine("1");
            else if (e.KeyCode == 19) Debug.WriteLine("2");
            else if (e.KeyCode == 20) Debug.WriteLine("3");
            else if (e.KeyCode == 21) Debug.WriteLine("4");
            else if (e.KeyCode == 23) Debug.WriteLine("5");
            else if (e.KeyCode == 22) Debug.WriteLine("6");
            else if (e.KeyCode == 26) Debug.WriteLine("7");
            else if (e.KeyCode == 28) Debug.WriteLine("8");
            else if (e.KeyCode == 25) Debug.WriteLine("9");
            else if (e.KeyCode == 29) Debug.WriteLine("0");
            else if (e.KeyCode == 4) Debug.WriteLine("H");
            else if (e.KeyCode == 38) Debug.WriteLine("J");
            else if (e.KeyCode == 40) Debug.WriteLine("K");
            else if (e.KeyCode == 37) Debug.WriteLine("L");

            // ctrlキー優先
            if (flag.HasFlag(NSEventModifierMask.ControlKeyMask))
            {
                Debug.WriteLine("with controle");
            }
            else if (flag.HasFlag(NSEventModifierMask.CommandKeyMask))
            {
                if (flag.HasFlag(NSEventModifierMask.ShiftKeyMask))
                {
                    Debug.WriteLine("with shift+command");
                }
                else
                {
                    Debug.WriteLine("with command");
                }
            }

        }
    }
}

