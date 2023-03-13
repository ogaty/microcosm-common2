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
            cvs.Clear();
            cvs.Scale((float)skiaScale);

            SKPaint lineStyle = new SKPaint();
            lineStyle.Style = SKPaintStyle.Stroke;
            lineStyle.StrokeWidth = 2.5F;

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
                cvs.DrawText(CommonData.getPlanetSymbol2(i - 1), 50 * (i + 1), 100, planetPoint);
                cvs.DrawText(CommonData.getPlanetSymbol2(i - 1), 50, 40 + 50 * (i + 1), planetPoint);
            }

            AspectCalc aspectCalc = new AspectCalc();
            SKPaint aspectPaint = new SKPaint();
            aspectPaint.Style = SKPaintStyle.Fill;
            aspectPaint.Typeface = SKTypeface.FromFile("system/microcosm-aspects.otf");
            aspectPaint.TextSize = 40;
            foreach (int i in Enumerable.Range(0, 10))
            {
                if (!list1[i].isAspectDisp)
                {
                    continue;
                }
                foreach (int j in Enumerable.Range(i + 1, 9 - i))
                {
                    if (!list1[j].isAspectDisp)
                    {
                        continue;
                    }
                    double aspect_degree = list1[j].absolute_position - list1[i].absolute_position;
                    if (aspect_degree > 180)
                    {
                        aspect_degree = 360 + list1[i].absolute_position - list1[j].absolute_position;
                    }

                    foreach (AspectKind kind in Enum.GetValues(typeof(AspectKind)))
                    {
                        if (kind == AspectKind.CONJUNCTION && appDelegate.currentSetting.dispAspectConjunction == 0) continue;
                        if (kind == AspectKind.OPPOSITION && appDelegate.currentSetting.dispAspectOpposition == 0) continue;
                        if (kind == AspectKind.TRINE && appDelegate.currentSetting.dispAspectTrine == 0) continue;
                        if (kind == AspectKind.SQUARE && appDelegate.currentSetting.dispAspectSquare == 0) continue;
                        if (kind == AspectKind.SEXTILE && appDelegate.currentSetting.dispAspectSextile == 0) continue;
                        if (kind == AspectKind.INCONJUNCT && appDelegate.currentSetting.dispAspectInconjunct == 0) continue;
                        if (kind == AspectKind.SESQUIQUADRATE && appDelegate.currentSetting.dispAspectSesquiQuadrate == 0) continue;
                        if (kind == AspectKind.SEMISEXTILE && appDelegate.currentSetting.dispAspectSemiSextile == 0) continue;
                        if (kind == AspectKind.SEMISQUARE && appDelegate.currentSetting.dispAspectSemiSquare == 0) continue;
                        if (kind == AspectKind.QUINTILE && appDelegate.currentSetting.dispAspectQuintile == 0) continue;
                        if (kind == AspectKind.BIQUINTILE && appDelegate.currentSetting.dispAspectBiQuintile == 0) continue;
                        if (kind == AspectKind.SEMIQINTILE && appDelegate.currentSetting.dispAspectSemiQuintile == 0) continue;
                        if (kind == AspectKind.NOVILE && appDelegate.currentSetting.dispAspectNovile == 0) continue;
                        if (kind == AspectKind.SEPTILE && appDelegate.currentSetting.dispAspectSeptile == 0) continue;
                        if (kind == AspectKind.QUINDECILE && appDelegate.currentSetting.dispAspectQuindecile == 0) continue;

                        bool isAspect = false;
                        SoftHard softHard = SoftHard.HARD;
                        double[] orbs = new double[2] { 6.0, 10.0 };
                        double degree = aspectCalc.getDegree(kind);

                        if (list1[i].no == CommonData.ZODIAC_SUN || list1[i].no == CommonData.ZODIAC_MOON)
                        {
                            if (aspectCalc.isFirstAspectKind(kind))
                            {
                                aspectCalc.IsAspect(aspect_degree, degree, appDelegate.currentSetting.orbSunMoon, ref isAspect, ref softHard);
                            }
                            else
                            {
                                // 2種:3種はsun/moon関係なし
                                aspectCalc.IsAspect(aspect_degree, degree, appDelegate.currentSetting.orb2nd, ref isAspect, ref softHard);
                            }
                        }
                        else
                        {
                            if (aspectCalc.isFirstAspectKind(kind))
                            {
                                aspectCalc.IsAspect(aspect_degree, degree, appDelegate.currentSetting.orb1st, ref isAspect, ref softHard);
                            }
                            else
                            {
                                aspectCalc.IsAspect(aspect_degree, degree, appDelegate.currentSetting.orb2nd, ref isAspect, ref softHard);
                            }
                        }



                        if (isAspect)
                        {
                            cvs.DrawText(CommonData.getAspectSymbol2(kind), 50 + 50 * (i + 1) + 10, 40 + 50 * (j + 2), aspectPaint);
                            break;
                        }
                    }

                }
            }
        }
    }
}

