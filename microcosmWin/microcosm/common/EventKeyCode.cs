using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace microcosm.common
{
    public class EventKeyCode
    {
        MainWindow mainWindow;

        public EventKeyCode(MainWindow main)
        {
            mainWindow = main;
        }

        public void GetEvent()
        {
        }


        public void ProcEvent(EShortCut shortCut)
        {
            switch (shortCut)
            {
                case EShortCut.Noop:
                    Debug.WriteLine("noop");
                    break;
                case EShortCut.Plus1Day:
                    mainWindow.TimeSetAny(86400);
                    break;
                case EShortCut.Minus1Day:
                    mainWindow.TimeSetAny(-86400);
                    break;
                case EShortCut.Plus1Hour:
                    mainWindow.TimeSetAny(3600);
                    break;
                case EShortCut.Minus1Hour:
                    mainWindow.TimeSetAny(-3600);
                    break;
                case EShortCut.Plus12Hour:
                    mainWindow.TimeSetAny(3600 * 12);
                    break;
                case EShortCut.Minus12Hour:
                    mainWindow.TimeSetAny(-3600 * 12);
                    break;
                case EShortCut.Plus1Minute:
                    mainWindow.TimeSetAny(60);
                    break;
                case EShortCut.Minus1Minute:
                    mainWindow.TimeSetAny(-60);
                    break;
                case EShortCut.Plus1Second:
                    mainWindow.TimeSetAny(1);
                    break;
                case EShortCut.Minus1Second:
                    mainWindow.TimeSetAny(-1);
                    break;
                case EShortCut.Plus7Day:
                    mainWindow.TimeSetAny(86400 * 7);
                    break;
                case EShortCut.Minus7Day:
                    mainWindow.TimeSetAny(-86400 * 7);
                    break;
                case EShortCut.Plus30Day:
                    mainWindow.TimeSetAny(86400 * 30);
                    break;
                case EShortCut.Minus30Day:
                    mainWindow.TimeSetAny(-86400 * 30);
                    break;
                case EShortCut.Plus365Day:
                    mainWindow.TimeSetAny(86400 * 365);
                    break;
                case EShortCut.Minus365Day:
                    mainWindow.TimeSetAny(-86400 * 365);
                    break;
                case EShortCut.ChagngeSetting0:
                    mainWindow.SettingIndexChange(0);
                    break;
                case EShortCut.ChagngeSetting1:
                    mainWindow.SettingIndexChange(1);
                    break;
                case EShortCut.ChagngeSetting2:
                    mainWindow.SettingIndexChange(2);
                    break;
                case EShortCut.ChagngeSetting3:
                    mainWindow.SettingIndexChange(3);
                    break;
                case EShortCut.ChagngeSetting4:
                    mainWindow.SettingIndexChange(4);
                    break;
                case EShortCut.ChagngeSetting5:
                    mainWindow.SettingIndexChange(5);
                    break;
                case EShortCut.ChagngeSetting6:
                    mainWindow.SettingIndexChange(6);
                    break;
                case EShortCut.ChagngeSetting7:
                    mainWindow.SettingIndexChange(7);
                    break;
                case EShortCut.ChagngeSetting8:
                    mainWindow.SettingIndexChange(8);
                    break;
                case EShortCut.ChagngeSetting9:
                    mainWindow.SettingIndexChange(9);
                    break;
                case EShortCut.Ring1U1:
                    mainWindow.Chart1U1();
                    break;
                case EShortCut.Ring1U2:
                    mainWindow.Chart1U2();
                    break;
                case EShortCut.Ring1E1:
                    mainWindow.Chart1E1();
                    break;
                case EShortCut.Ring1E2:
                    mainWindow.Chart1E2();
                    break;
                case EShortCut.Ring1Current:
                    mainWindow.TimeSetNowCurrentBand();
                    mainWindow.Chart1U1();
                    break;
                case EShortCut.Ring2UU:
                    mainWindow.Chart2UU();
                    break;
                case EShortCut.Ring2UE:
                    mainWindow.Chart2UE();
                    break;
                case EShortCut.Ring3NPT:
                    mainWindow.Chart3NPT();
                    break;
                case EShortCut.InvisibleAllAspect:
                    mainWindow.AspectAllOff();
                    break;
                case EShortCut.VisibleAllAspect:
                    mainWindow.AspectAllOn();
                    break;
                case EShortCut.InVisible11:
                    mainWindow.AspectOff(0);
                    break;
                case EShortCut.InVisible12:
                    mainWindow.AspectOff(1);
                    break;
                case EShortCut.InVisible13:
                    mainWindow.AspectOff(2);
                    break;
                case EShortCut.InVisible22:
                    mainWindow.AspectOff(3);
                    break;
                case EShortCut.InVisible23:
                    mainWindow.AspectOff(4);
                    break;
                case EShortCut.InVisible33:
                    mainWindow.AspectOff(5);
                    break;
                case EShortCut.Visible11:
                    mainWindow.AspectOn(0);
                    break;
                case EShortCut.Visible12:
                    mainWindow.AspectOn(1);
                    break;
                case EShortCut.Visible13:
                    mainWindow.AspectOn(2);
                    break;
                case EShortCut.Visible22:
                    mainWindow.AspectOn(3);
                    break;
                case EShortCut.Visible23:
                    mainWindow.AspectOn(4);
                    break;
                case EShortCut.Visible33:
                    mainWindow.AspectOn(5);
                    break;
            }
        }
    }
}
