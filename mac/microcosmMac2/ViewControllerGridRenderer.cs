using System;
using System.Linq;
using microcosmMac2.Calc;
using microcosmMac2.Common;
using microcosmMac2.Models;
using SkiaSharp;

namespace microcosmMac2
{
    partial class ViewController
    {
        public void mainGridRenderer(SKCanvas cvs)
        {
            SKPaint lineStyle = new SKPaint();
            lineStyle.Style = SKPaintStyle.Stroke;
            lineStyle.StrokeWidth = 4.5F;

            SKPaint planetPoint = new SKPaint();
            planetPoint.Style = SKPaintStyle.Fill;
            planetPoint.Typeface = SKTypeface.FromFile("system/microcosm.otf");
            planetPoint.TextSize = 40;

            foreach (int i in Enumerable.Range(1, 11))
            {
                cvs.DrawLine(50, 50 * i, 600, 50 * i, lineStyle);
                cvs.DrawLine(50 * i, 50, 50 * i, 600, lineStyle);
            }

            cvs.DrawLine(50, 50, 600, 600, lineStyle);

            foreach (int i in Enumerable.Range(1, 10))
            {
                cvs.DrawText(CommonData.getPlanetSymbol2(i - 1), 50 * (i + 1) + 10, 100, planetPoint);
                cvs.DrawText(CommonData.getPlanetSymbol2(i - 1), 50, 50 * (i + 2), planetPoint);
            }

            AspectCalc aspectCalc = new AspectCalc();
            foreach (int i in Enumerable.Range(1, 10))
            {
                foreach (int j in Enumerable.Range(i + 1, 10 - i))
                {
                    double aspect_degree = list1[j].absolute_position - list1[i].absolute_position;
                    if (aspect_degree > 180)
                    {
                        aspect_degree = 360 + list1[i].absolute_position - list1[j].absolute_position;
                    }

                    foreach (AspectKind kind in Enum.GetValues(typeof(AspectKind)))
                    {
                        bool isAspect = false;
                        SoftHard softHard = SoftHard.HARD;
                        double[] orbs = new double[2] { 6.0, 10.0 };
                        double degree = aspectCalc.getDegree(kind);

                        aspectCalc.IsAspect(aspect_degree, degree, orbs, ref isAspect, ref softHard);

                        if (isAspect)
                        {
                            cvs.DrawText(CommonData.getAspectSymbol(kind), 50 * (i + 1) + 10, 50 * (j + 2), planetPoint);
                            break;
                        }
                    }

                }
            }
        }
    }
}

