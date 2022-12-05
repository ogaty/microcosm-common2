using System;
using System.Collections.Generic;
using System.Diagnostics;
using AppKit;
using CoreGraphics;
using microcosmMac2.Models;
using SkiaSharp.Views.Mac;

namespace microcosmMac2.Views
{
    public class CanvasView : SKCanvasView
    {
        public CanvasView(CGRect rect) : base(rect)
        {
            Console.WriteLine("canvas render");
        }

        public override void MouseEntered(NSEvent theEvent)
        {
            base.MouseEntered(theEvent);
        }

        public override void MouseMoved(NSEvent theEvent)
        {
            base.MouseMoved(theEvent);
            CGPoint p = theEvent.Window.MouseLocationOutsideOfEventStream;
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;

            foreach (MouseIn m in appDelegate.viewController.mouseInList)
            {
                if (m.rect.Contains((float)p.X, (float)p.Y))
                {
                    appDelegate.viewController.SetExplanationText(m.kind, m.message, m.degree);
                    if (m.kind == 0)
                    {
                        //appDelegate.viewController.SetExplanationText(m.message);
                        //appDelegate.viewController.SetSabianText((int)m.degree);
                        //appDelegate.viewController.SetExplanationText(m.message, (int)m.degree);
                    }
                    else
                    {
//                        appDelegate.viewController.SetExplanationText(m.message + " " + m.degree.ToString());
//                        appDelegate.viewController.SetSabianText(-1);
                        //appDelegate.viewController.SetExplanationText(m.message, -1);
                    }
                }
            }
        }

        NSTrackingArea _trackingArea;

        public override void UpdateTrackingAreas()
        {
            if (_trackingArea != null)
            {
                this.RemoveTrackingArea(_trackingArea);
            }

            _trackingArea = new NSTrackingArea(
                rect: this.Bounds,
                 options: NSTrackingAreaOptions.ActiveAlways |
                          NSTrackingAreaOptions.InVisibleRect |
                          NSTrackingAreaOptions.MouseEnteredAndExited |
                          NSTrackingAreaOptions.MouseMoved,
                owner: this,
                userInfo: null);

            this.AddTrackingArea(_trackingArea);

            base.UpdateTrackingAreas();
        }


    }
}
