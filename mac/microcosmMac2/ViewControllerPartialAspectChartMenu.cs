using System;
using AppKit;
using microcosmMac2.Common;

namespace microcosmMac2
{
	partial class ViewController
	{
        public void Chart1U1()
        {
            appDelegate.bands = 1;
            calcTargetUser[0] = ETargetUser.USER1;
            ReCalc();
            ReRender();
        }

        public void Chart1U2()
        {
            appDelegate.bands = 1;
            calcTargetUser[0] = ETargetUser.USER2;
            ReCalc();
            ReRender();
        }

        public void Chart1E1()
        {
            appDelegate.bands = 1;
            calcTargetUser[0] = ETargetUser.EVENT1;
            ReCalc();
            ReRender();
        }

        public void Chart1E2()
        {
            appDelegate.bands = 1;
            calcTargetUser[0] = ETargetUser.EVENT2;
            ReCalc();
            ReRender();
        }

        public void Chart2UU()
        {
            appDelegate.bands = 2;
            appDelegate.secondBand = AppDelegate.BandKind.NATAL;
            calcTargetUser[0] = ETargetUser.USER1;
            calcTargetUser[1] = ETargetUser.USER2;
            ReCalc();
            ReRender();
        }

        public void Chart2UE()
        {
            appDelegate.bands = 2;
            appDelegate.secondBand = AppDelegate.BandKind.TRANSIT;
            calcTargetUser[0] = ETargetUser.USER1;
            calcTargetUser[1] = ETargetUser.EVENT1;
            ReCalc();
            ReRender();
        }

        public void Chart2EE()
        {
            appDelegate.bands = 2;
            appDelegate.secondBand = AppDelegate.BandKind.TRANSIT;
            calcTargetUser[0] = ETargetUser.EVENT1;
            calcTargetUser[1] = ETargetUser.EVENT2;
            ReCalc();
            ReRender();
        }

        public void Chart3NPT()
        {
            appDelegate.bands = 3;
            appDelegate.secondBand = AppDelegate.BandKind.PROGRESS;
            appDelegate.thirdBand = AppDelegate.BandKind.TRANSIT;
            calcTargetUser[0] = ETargetUser.USER1;
            calcTargetUser[1] = ETargetUser.EVENT1;
            calcTargetUser[2] = ETargetUser.EVENT1;
            ReCalc();
            ReRender();
        }

        public void Chart3NNT()
        {
            appDelegate.bands = 3;
            appDelegate.secondBand = AppDelegate.BandKind.NATAL;
            appDelegate.thirdBand = AppDelegate.BandKind.NATAL;
            calcTargetUser[0] = ETargetUser.USER1;
            calcTargetUser[1] = ETargetUser.USER2;
            calcTargetUser[2] = ETargetUser.EVENT1;
            ReCalc();
            ReRender();
        }

        public void Chart3NTT()
        {
            appDelegate.bands = 3;
            appDelegate.secondBand = AppDelegate.BandKind.TRANSIT;
            appDelegate.thirdBand = AppDelegate.BandKind.TRANSIT;
            calcTargetUser[0] = ETargetUser.USER1;
            calcTargetUser[1] = ETargetUser.EVENT1;
            calcTargetUser[2] = ETargetUser.EVENT2;
            ReCalc();
            ReRender();
        }

        public void Chart3NNC()
        {
            appDelegate.bands = 3;
            appDelegate.secondBand = AppDelegate.BandKind.TRANSIT;
            appDelegate.thirdBand = AppDelegate.BandKind.COMPOSIT;
            calcTargetUser[0] = ETargetUser.USER1;
            calcTargetUser[1] = ETargetUser.USER2;
            calcTargetUser[2] = ETargetUser.EVENT2;
            ReCalc();
            ReRender();
        }


        public void SetAspect11()
        {
            NSMenuItem item = appDelegate.GetAspect11();
            if (item.State == NSCellStateValue.On)
            {
                appDelegate.SetAspect11(NSCellStateValue.Off);
                appDelegate.aspect11disp = false;
            }
            else
            {
                appDelegate.SetAspect11(NSCellStateValue.On);
                appDelegate.aspect11disp = true;
            }
            ReRender();
        }

        public void SetAspect12()
        {
            NSMenuItem item = appDelegate.GetAspect12();
            if (item.State == NSCellStateValue.On)
            {
                appDelegate.SetAspect12(NSCellStateValue.Off);
                appDelegate.aspect12disp = false;
            }
            else
            {
                appDelegate.SetAspect12(NSCellStateValue.On);
                appDelegate.aspect12disp = true;
            }
            ReRender();

        }

        public void SetAspect13()
        {
            NSMenuItem item = appDelegate.GetAspect13();
            if (item.State == NSCellStateValue.On)
            {
                appDelegate.SetAspect13(NSCellStateValue.Off);
                appDelegate.aspect13disp = false;
            }
            else
            {
                appDelegate.SetAspect13(NSCellStateValue.On);
                appDelegate.aspect13disp = true;
            }
            ReRender();

        }

        public void SetAspect22()
        {
            NSMenuItem item = appDelegate.GetAspect22();
            if (item.State == NSCellStateValue.On)
            {
                appDelegate.SetAspect22(NSCellStateValue.Off);
                appDelegate.aspect22disp = false;
            }
            else
            {
                appDelegate.SetAspect22(NSCellStateValue.On);
                appDelegate.aspect22disp = true;
            }
            ReRender();

        }

        public void SetAspect23()
        {
            NSMenuItem item = appDelegate.GetAspect23();
            if (item.State == NSCellStateValue.On)
            {
                appDelegate.SetAspect23(NSCellStateValue.Off);
                appDelegate.aspect23disp = false;
            }
            else
            {
                appDelegate.SetAspect23(NSCellStateValue.On);
                appDelegate.aspect23disp = true;
            }
            ReRender();

        }

        public void SetAspect33()
        {
            NSMenuItem item = appDelegate.GetAspect33();
            if (item.State == NSCellStateValue.On)
            {
                appDelegate.SetAspect33(NSCellStateValue.Off);
                appDelegate.aspect33disp = false;
            }
            else
            {
                appDelegate.SetAspect33(NSCellStateValue.On);
                appDelegate.aspect33disp = true;
            }
            ReRender();
        }

        public void AspectAllOn()
        {
            appDelegate.SetAspect11(NSCellStateValue.On);
            appDelegate.SetAspect12(NSCellStateValue.On);
            appDelegate.SetAspect13(NSCellStateValue.On);
            appDelegate.SetAspect22(NSCellStateValue.On);
            appDelegate.SetAspect23(NSCellStateValue.On);
            appDelegate.SetAspect33(NSCellStateValue.On);
            appDelegate.aspect11disp = true;
            appDelegate.aspect12disp = true;
            appDelegate.aspect13disp = true;
            appDelegate.aspect22disp = true;
            appDelegate.aspect23disp = true;
            appDelegate.aspect33disp = true;
            ReRender();
        }

        public void AspectOn(int index)
        {
            AspectLineChange(index, NSCellStateValue.On);
            ReRender();
        }

        public void AspectAllOff()
        {
            appDelegate.SetAspect11(NSCellStateValue.Off);
            appDelegate.SetAspect12(NSCellStateValue.Off);
            appDelegate.SetAspect13(NSCellStateValue.Off);
            appDelegate.SetAspect22(NSCellStateValue.Off);
            appDelegate.SetAspect23(NSCellStateValue.Off);
            appDelegate.SetAspect33(NSCellStateValue.Off);
            appDelegate.aspect11disp = false;
            appDelegate.aspect12disp = false;
            appDelegate.aspect13disp = false;
            appDelegate.aspect22disp = false;
            appDelegate.aspect23disp = false;
            appDelegate.aspect33disp = false;
            ReRender();
        }

        public void AspectOff(int index)
        {
            AspectLineChange(index, NSCellStateValue.Off);
            ReRender();
        }

        private void AspectLineChange(int index, NSCellStateValue state)
        {
            if (index == 0)
            {
                appDelegate.SetAspect11(state);
                appDelegate.aspect11disp = false;
            }
            else if (index == 1)
            {
                appDelegate.SetAspect12(state);
                appDelegate.aspect12disp = false;
            }
            else if (index == 2)
            {
                appDelegate.SetAspect13(state);
                appDelegate.aspect13disp = false;
            }
            else if (index == 3)
            {
                appDelegate.SetAspect22(state);
                appDelegate.aspect22disp = false;
            }
            else if (index == 4)
            {
                appDelegate.SetAspect23(state);
                appDelegate.aspect23disp = false;
            }
            else if (index == 5)
            {
                appDelegate.SetAspect33(state);
                appDelegate.aspect33disp = false;
            }
        }
    }
}

