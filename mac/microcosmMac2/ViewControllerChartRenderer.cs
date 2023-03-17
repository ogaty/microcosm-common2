using System;
using microcosmMac2.Common;
using microcosmMac2.Models;
using SkiaSharp;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using microcosmMac2.Config;

namespace microcosmMac2
{
    partial class ViewController
    {
        public void mainChartRenderer(SKCanvas cvs)
        {
            float CenterX = this.canvasWidth / 2;
            float CenterY = this.canvasWidth / 2;
            // 若干マージンを取る
            CenterY += 10;
            float diameter = this.canvasWidth - 60;
            // 獣帯の幅
            float zodiacWidth = 30;


            string[] signs = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l" };
            string[] planets = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "O", "L" };

            //            var surface = e.Surface;
            //            var surfaceWidth = e.Info.Width;
            //            var surfaceHeight = e.Info.Height;
            cvs.Clear();
            cvs.Scale((float)skiaScale);

            SKRect bg = new SKRect(0, 0, CenterX * 2, CenterY * 2);
            SKPaint bgStyle = new SKPaint();
            bgStyle.Color = new SKColor(255, 255, 255, 255);
            cvs.DrawRect(bg, bgStyle);

            //            cvs.Translate(50, 50);
            SKPaint lineStyle = new SKPaint();
            lineStyle.Style = SKPaintStyle.Stroke;

            // outer
            cvs.DrawCircle(CenterX, CenterY, diameter / 2, lineStyle);
            // inner
            cvs.DrawCircle(CenterX, CenterY, diameter / 2 - zodiacWidth, lineStyle);

            //cvs.DrawLine(new SKPoint(CenterX, 0), new SKPoint(CenterX, this.canvasWidth), p);
            //cvs.DrawLine(new SKPoint(0, CenterY), new SKPoint(this.canvasWidth, CenterY), p);

            double offset = 0;
            float centerDiameter = centerDiameterBase;
            // ring描画
            if (appDelegate.bands == 1)
            {
                if (configData.dispPattern2 == EDispPettern.MINI)
                {
                    // 中心円を広げる
                    centerDiameter = centerDiameterBase + 50;
                }
                // center
                cvs.DrawCircle(CenterX, CenterY, centerDiameter, lineStyle);
            }
            else if (appDelegate.bands == 2)
            {
                // center
                // 中心円を少し縮める
                centerDiameter = centerDiameterBase - 20;
                cvs.DrawCircle(CenterX, CenterY, centerDiameter, lineStyle);

                // middle
                offset = (diameter / 2 - zodiacWidth - centerDiameter) / 2;
                cvs.DrawCircle(CenterX, CenterY, (float)(centerDiameter + offset), lineStyle);
            }
            else if (appDelegate.bands == 3)
            {
                // center
                // 中心円を少し縮める
                centerDiameter = centerDiameterBase - 40;
                cvs.DrawCircle(CenterX, CenterY, centerDiameter, lineStyle);

                offset = (diameter / 2 - zodiacWidth - centerDiameter) / 3;
                cvs.DrawCircle(CenterX, CenterY, (float)(centerDiameter + offset), lineStyle);
                cvs.DrawCircle(CenterX, CenterY, (float)(centerDiameter + offset * 2), lineStyle);
            }
            else if (appDelegate.bands == 4)
            {
                // center
                // 中心円を少し縮める
                centerDiameter = centerDiameterBase - 80;
                cvs.DrawCircle(CenterX, CenterY, centerDiameter, lineStyle);

                offset = (diameter - zodiacWidth - centerDiameter) / 4;
                cvs.DrawCircle(CenterX, CenterY, (float)(centerDiameter + offset), lineStyle);
                cvs.DrawCircle(CenterX, CenterY, (float)(centerDiameter + offset * 2), lineStyle);
                cvs.DrawCircle(CenterX, CenterY, (float)(centerDiameter + offset * 3), lineStyle);
            }

            // house cusps
            // 外側
            Position housePt;
            // 内側
            Position housePtEnd;
            // テキスト
            Position housePtTxt;
            // 軸
            SKPaint lineStyle1 = new SKPaint();
            lineStyle1.StrokeWidth = 2.5F;
            SKColor xxx = SKColors.LightGray;
            lineStyle1.Color = xxx;
            // それ以外
            SKPaint lineStyle2 = new SKPaint();
            lineStyle2.PathEffect = SKPathEffect.CreateDash(new[] { 5F, 2F }, 1.0F);
            lineStyle2.StrokeWidth = 1.5F;
            SKColor yyy = SKColors.LightGray;
            lineStyle2.Color = yyy;
            SKPaint textStyle = new SKPaint();
            textStyle.StrokeWidth = 1.5F;
            SKColor zzz = SKColors.SlateGray;
            textStyle.Color = zzz;

            // house cusps
            for (int i = 1; i <= 12; i++)
            {
                if (appDelegate.bands == 1 || appDelegate.currentSetting.sameCusps)
                {
                    housePt = Util.Rotate(diameter / 2 - zodiacWidth, 0, houseList1[i] - houseList1[1]);
                    housePt.x = housePt.x + CenterX;
                    housePt.y = -1 * housePt.y + CenterY;
                }
                else if (appDelegate.bands == 2 && !appDelegate.currentSetting.sameCusps)
                {
                    housePt = Util.Rotate(diameter / 2 - zodiacWidth - 75, 0, houseList1[i] - houseList1[1]);
                    housePt.x = housePt.x + CenterX;
                    housePt.y = -1 * housePt.y + CenterY;
                }
                else
                {
                    housePt = Util.Rotate(diameter / 2 - zodiacWidth - 115, 0, houseList1[i] - houseList1[1]);
                    housePt.x = housePt.x + CenterX;
                    housePt.y = -1 * housePt.y + CenterY;
                }
                housePtEnd = Util.Rotate(centerDiameter, 0, houseList1[i] - houseList1[1]);
                housePtEnd.x = housePtEnd.x + CenterX;
                housePtEnd.y = -1 * housePtEnd.y + CenterY;
                housePtTxt = Util.Rotate(diameter / 2 + 10, 0, houseList1[i] - houseList1[1]);
                housePtTxt.x = housePtTxt.x + CenterX - 5;
                housePtTxt.y = -1 * housePtTxt.y + CenterY;

                if (i == 1 || i == 4 || i == 7 || i == 10)
                {
                    cvs.DrawLine((float)housePt.x, (float)housePt.y, (float)housePtEnd.x, (float)housePtEnd.y, lineStyle1);
                }
                else
                {
                    cvs.DrawLine((float)housePt.x, (float)housePt.y, (float)housePtEnd.x, (float)housePtEnd.y, lineStyle2);
                }

                cvs.DrawText(((int)houseList1[i] % 30).ToString(), (float)housePtTxt.x, (float)housePtTxt.y, textStyle);
            }
            if (appDelegate.bands > 1 && !appDelegate.currentSetting.sameCusps)
            {
                for (int i = 1; i <= 12; i++)
                {
                    if (appDelegate.bands == 2)
                    {
                        housePt = Util.Rotate(diameter / 2 - zodiacWidth, 0, houseList2[i] - houseList1[1]);
                        housePt.x = housePt.x + CenterX;
                        housePt.y = -1 * housePt.y + CenterY;
                        housePtEnd = Util.Rotate(diameter / 2 - zodiacWidth - 75, 0, houseList2[i] - houseList1[1]);
                        housePtEnd.x = housePtEnd.x + CenterX;
                        housePtEnd.y = -1 * housePtEnd.y + CenterY;
                    }
                    else
                    {
                        housePt = Util.Rotate(diameter / 2 - zodiacWidth - 55, 0, houseList2[i] - houseList1[1]);
                        housePt.x = housePt.x + CenterX;
                        housePt.y = -1 * housePt.y + CenterY;
                        housePtEnd = Util.Rotate(diameter / 2 - zodiacWidth - 115, 0, houseList2[i] - houseList1[1]);
                        housePtEnd.x = housePtEnd.x + CenterX;
                        housePtEnd.y = -1 * housePtEnd.y + CenterY;
                    }
                    if (i == 1 || i == 4 || i == 7 || i == 10)
                    {
                        cvs.DrawLine((float)housePt.x, (float)housePt.y, (float)housePtEnd.x, (float)housePtEnd.y, lineStyle1);
                    }
                    else
                    {
                        cvs.DrawLine((float)housePt.x, (float)housePt.y, (float)housePtEnd.x, (float)housePtEnd.y, lineStyle2);
                    }
                }

            }
            if (appDelegate.bands > 2 && !appDelegate.currentSetting.sameCusps)
            {
                for (int i = 1; i <= 12; i++)
                {
                    housePt = Util.Rotate(diameter / 2 - zodiacWidth, 0, houseList3[i] - houseList1[1]);
                    housePt.x = housePt.x + CenterX;
                    housePt.y = -1 * housePt.y + CenterY;
                    housePtEnd = Util.Rotate(diameter / 2 - zodiacWidth - 55, 0, houseList3[i] - houseList1[1]);
                    housePtEnd.x = housePtEnd.x + CenterX;
                    housePtEnd.y = -1 * housePtEnd.y + CenterY;
                    if (i == 1 || i == 4 || i == 7 || i == 10)
                    {
                        cvs.DrawLine((float)housePt.x, (float)housePt.y, (float)housePtEnd.x, (float)housePtEnd.y, lineStyle1);
                    }
                    else
                    {
                        cvs.DrawLine((float)housePt.x, (float)housePt.y, (float)housePtEnd.x, (float)housePtEnd.y, lineStyle2);
                    }
                }

            }

            // sign cusps
            Position signPt;
            Position signPtEnd;
            for (int i = 1; i <= 12; i++)
            {
                signPt = Util.Rotate(diameter / 2, 0, 30 * i - houseList1[1]);
                signPt.x = signPt.x + CenterX;
                signPt.y = -1 * signPt.y + CenterY;
                signPtEnd = Util.Rotate(diameter / 2 - zodiacWidth, 0, 30 * i - houseList1[1]);
                signPtEnd.x = signPtEnd.x + CenterX;
                signPtEnd.y = -1 * signPtEnd.y + CenterY;
                cvs.DrawLine((float)signPt.x, (float)signPt.y, (float)signPtEnd.x, (float)signPtEnd.y, lineStyle);
            }

            System.Reflection.Assembly asm =
                System.Reflection.Assembly.GetExecutingAssembly();
            //SKManagedStream stream = new SKManagedStream(asm.GetManifestResourceStream("microcosm.system.AstroDotBasic.ttf"));
            //{
            // ♈〜♓までのシンボル
            var root = Util.root;

            SKPaint planetPaint = new SKPaint();
            planetPaint.Style = SKPaintStyle.Fill;
            planetPaint.Typeface = SKTypeface.FromFile("system/microcosm.otf");
            //planetPaint.Typeface = SKTypeface.FromFile(root + "/system/AstroDotBasic.ttf");
            //planetPaint.Typeface = Typeface(stream);
            planetPaint.TextSize = 28;
            Position signValuePt;
            //SKColor pink = SKColors.Pink;
            //p.Color = pink;
            float fontW;
            float fontH;
            for (int i = 0; i < signs.Length; i++)
            {
                signValuePt = Util.Rotate(diameter / 2 - 15, 0, 15 + 30 * i - houseList1[1]);
                fontW = planetPaint.MeasureText(signs[i]) / 2;
                fontH = (planetPaint.FontMetrics.Descent - planetPaint.FontMetrics.Ascent) / 4;
                signValuePt.x = signValuePt.x + CenterX - fontW;
                signValuePt.y = -1 * signValuePt.y + CenterY + fontH;
                planetPaint.Color = CommonData.getSignColor(30 * i);

                cvs.DrawText(signs[i], (float)signValuePt.x, (float)signValuePt.y, planetPaint);
            }
            planetPaint.Color = SKColors.Black;
            //}


            // 天体そのもの
            #region PlanetRenderer
            int[] box = new int[72];
            int planetOffset = 0;
            IOrderedEnumerable<KeyValuePair<int, PlanetData>> sortPlanetData = list1.OrderBy(pair => pair.Value.absolute_position);
            foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData)
            {
                PlanetData planet = pData.Value;
                //Debug.WriteLine("planet: " + planet.no + "(" + CommonData.getPlanetSymbolText(planet.no) + ")");
                //Debug.WriteLine("isDisp: " + planet.isDisp);
                //Debug.WriteLine("display: " + planet.absolute_position);

                if (!planet.isDisp)
                {
                    continue;
                }
                if (planet.no == 10000 || planet.no == 10001)
                {
                    // 一重円のASC、MC
                    continue;
                }

                int index = (int)(planet.absolute_position / 5);

                BoxInit(ref box, ref index);

                // 天体そのもの
                planetOffset = GetPlanetOffset(1);
                DrawPlanetText(index, houseList1[1], planet, planetPaint, cvs, planetOffset);

                // 天体から中心への線
                if (appDelegate.bands == 1)
                {
                    Position startPt;
                    Position endPt;

                    startPt = Util.Rotate(diameter / 2 - (planetOffset + 95), 0, 5 * index - houseList1[1]);
                    startPt.x = startPt.x + CenterX;
                    startPt.y = -1 * startPt.y + CenterY;

                    endPt = Util.Rotate(centerDiameter, 0, planet.absolute_position - houseList1[1]);
                    endPt.x = endPt.x + CenterX;
                    endPt.y = -1 * endPt.y + CenterY;

                    cvs.DrawLine((float)startPt.x, (float)startPt.y, (float)endPt.x, (float)endPt.y, lineStyle);
                }
            }




            // 二重円の天体
            if (appDelegate.bands > 1)
            {
                int[] box2 = new int[72];
                int planetOffset2 = 0;
                IOrderedEnumerable<KeyValuePair<int, PlanetData>> sortPlanetData2 = list2.OrderBy(pair => pair.Value.absolute_position);
                foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData2)
                {
                    PlanetData planet2 = pData.Value;
                    Debug.WriteLine("planet2: " + planet2.no + "(" + CommonData.getPlanetSymbolText(planet2.no) + ")");
                    Debug.WriteLine("isDisp2: " + planet2.isDisp);
                    Debug.WriteLine("display2: " + planet2.absolute_position);

                    if (!planet2.isDisp)
                    {
                        continue;
                    }

                    int index2 = (int)(planet2.absolute_position / 5);

                    BoxInit(ref box2, ref index2);

                    // 天体そのもの
                    planetOffset2 = GetPlanetOffset(2);
                    DrawPlanetText(index2, houseList1[1], planet2, planetPaint, cvs, planetOffset2);
                }
            }

            // 三重円の天体
            if (appDelegate.bands > 2)
            {
                int[] box3 = new int[72];
                int planetOffset3 = 0;
                IOrderedEnumerable<KeyValuePair<int, PlanetData>> sortPlanetData3 = list3.OrderBy(pair => pair.Value.absolute_position);
                foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData3)
                {
                    PlanetData planet3 = pData.Value;
                    Debug.WriteLine("planet3: " + planet3.no + "(" + CommonData.getPlanetSymbolText(planet3.no) + ")");
                    Debug.WriteLine("isDisp3: " + planet3.isDisp);
                    Debug.WriteLine("display3: " + planet3.absolute_position);

                    if (!planet3.isDisp)
                    {
                        continue;
                    }

                    int index3 = (int)(planet3.absolute_position / 5);

                    BoxInit(ref box3, ref index3);

                    // 天体そのもの
                    planetOffset3 = GetPlanetOffset(3);
                    DrawPlanetText(index3, houseList1[1], planet3, planetPaint, cvs, planetOffset3);
                }
            }

            // 四重円の天体
            if (appDelegate.bands > 3)
            {
                int[] box4 = new int[72];
                /*
                IOrderedEnumerable<PlanetData> sortPlanetData4 = ringsData[3].planetData.OrderBy(planetTmp => planetTmp.absolute_position);
                foreach (PlanetData planet in sortPlanetData4)
                {
                    if (!CommonInstance.getInstance().currentSetting.GetDispPlanet(planet.no))
                    {
                        continue;
                    }

                    int index = (int)(planet.absolute_position / 5);

                    BoxInit(ref box4, ref index, planet.absolute_position);

                    planetOffset = GetPlanetOffset(4);
                    DrawPlanetText(index, ringsData[3].cusps[1], planet, p, cvs, degreeText, planetOffset, 35, 10, 20, 15, 10);
                }
                */
            }
            #endregion PlanetRenderer

            // aspects
            Position aspectPt;
            Position aspectPtEnd;
            SKPaint aspectLine = new SKPaint();
            aspectLine.Style = SKPaintStyle.Stroke;
            aspectLine.StrokeWidth = 1.0F;
            SKPaint aspectSymboolText = new SKPaint()
            {
                TextSize = 24,
                Style = SKPaintStyle.Fill
            };
            // aspectsData[0, 0] => natal-natal
            if (appDelegate.aspect11disp)
            {
                foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData)
                {
                    PlanetData planet = pData.Value;
                    Debug.WriteLine("planet: " + planet.no + "(" + CommonData.getPlanetSymbolText(planet.no) + ")");
                    Debug.WriteLine("isDisp: " + planet.isDisp);
                    Debug.WriteLine("display: " + planet.absolute_position);

                    if (!planet.isDisp)
                    {
                        continue;
                    }
                    // isAspectDispは不要

                    foreach (AspectInfo x in planet.aspects)
                    {
                        if (!list1[x.targetPlanetNo].isDisp)
                        {
                            continue;
                        }

                        if (x.softHard == SoftHard.SOFT)
                        {
                            aspectLine.PathEffect = SKPathEffect.CreateDash(new float[] { 1, 4 }, (float)2.0);
                        }
                        else
                        {
                            aspectLine.PathEffect = null;
                        }

                        double calcDegree = x.sourceDegree - houseList1[1];
                        if (calcDegree < 0)
                        {
                            calcDegree += 360;
                        }
                        double calcDegree2 = x.targetDegree - houseList1[1];
                        if (calcDegree2 < 0)
                        {
                            calcDegree2 += 360;
                        }
                        aspectPt = Util.Rotate(centerDiameter, 0, calcDegree);
                        aspectPt.x = aspectPt.x + CenterX;
                        aspectPt.y = -1 * aspectPt.y + CenterY;

                        aspectPtEnd = Util.Rotate(centerDiameter, 0, calcDegree2);
                        aspectPtEnd.x = aspectPtEnd.x + CenterX;
                        aspectPtEnd.y = -1 * aspectPtEnd.y + CenterY;

                        GetAspectLineAndText(x.aspectKind, ref aspectLine, ref aspectSymboolText);
                        DrawAspect(cvs, x, aspectPt, aspectPtEnd, aspectLine, aspectSymboolText);
                    }

                    /*
                    aspectSymbolPt = Util.Rotate(diameter - 160, 0, planet.absolute_position - houseList1[1]);
                    aspectSymbolPt.x = aspectSymbolPt.x + CenterX;
                    aspectSymbolPt.y = -1 * aspectSymbolPt.y + CenterY + 10;
                    */
                }
            }

            // 二重円アスペクト
            // aspectsData[0, 1]
            if (appDelegate.bands > 1 && appDelegate.aspect12disp)
            {
                foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData)
                {
                    PlanetData planet = pData.Value;
                    Debug.WriteLine("planet: " + planet.no + "(" + CommonData.getPlanetSymbolText(planet.no) + ")");
                    Debug.WriteLine("isDisp: " + planet.isDisp);
                    Debug.WriteLine("display: " + planet.absolute_position);

                    if (!planet.isDisp)
                    {
                        continue;
                    }
                    // isAspectDispは不要

                    foreach (AspectInfo x in planet.secondAspects)
                    {
                        if (!list1[x.targetPlanetNo].isDisp)
                        {
                            continue;
                        }
                        if (!list2[x.targetPlanetNo].isDisp)
                        {
                            continue;
                        }

                        if (x.softHard == SoftHard.SOFT)
                        {
                            aspectLine.PathEffect = SKPathEffect.CreateDash(new float[] { 1, 4 }, (float)2.0);
                        }
                        else
                        {
                            aspectLine.PathEffect = null;
                        }

                        // box度数じゃないからちょっとずれるんだよね
                        double calcDegree = x.sourceDegree - houseList1[1];
                        if (calcDegree < 0)
                        {
                            calcDegree += 360;
                        }
                        double calcDegree2 = x.targetDegree - houseList2[1];
                        if (calcDegree2 < 0)
                        {
                            calcDegree2 += 360;
                        }
                        aspectPt = Util.Rotate(centerDiameter, 0, calcDegree);
                        aspectPt.x = aspectPt.x + CenterX;
                        aspectPt.y = -1 * aspectPt.y + CenterY;

                        if (appDelegate.bands == 2)
                        {
                            offset = (diameter / 2 - zodiacWidth - centerDiameter) / 2;
                        }
                        else if (appDelegate.bands == 3)
                        {
                            offset = (diameter / 2 - zodiacWidth - centerDiameter) / 3;
                        }
                        aspectPtEnd = Util.Rotate(centerDiameter + offset, 0, calcDegree2);
                        aspectPtEnd.x = aspectPtEnd.x + CenterX;
                        aspectPtEnd.y = -1 * aspectPtEnd.y + CenterY;

                        GetAspectLineAndText(x.aspectKind, ref aspectLine, ref aspectSymboolText);
                        DrawAspect(cvs, x, aspectPt, aspectPtEnd, aspectLine, aspectSymboolText);
                    }
                }
            }

            // 二重円アスペクト
            // aspectsData[1, 1]
            if (appDelegate.bands > 1 && appDelegate.aspect22disp)
            {
                IOrderedEnumerable<KeyValuePair<int, PlanetData>> sortPlanetData2 = list2.OrderBy(pair => pair.Value.absolute_position);
                foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData2)
                {
                    PlanetData planet = pData.Value;
                    Debug.WriteLine("planet: " + planet.no + "(" + CommonData.getPlanetSymbolText(planet.no) + ")");
                    Debug.WriteLine("isDisp: " + planet.isDisp);
                    Debug.WriteLine("display: " + planet.absolute_position);

                    if (!planet.isDisp)
                    {
                        continue;
                    }
                    // isAspectDispは不要

                    foreach (AspectInfo x in planet.secondAspects)
                    {
                        if (!list2[x.targetPlanetNo].isDisp)
                        {
                            continue;
                        }

                        if (x.softHard == SoftHard.SOFT)
                        {
                            aspectLine.PathEffect = SKPathEffect.CreateDash(new float[] { 1, 4 }, (float)2.0);
                        }
                        else
                        {
                            aspectLine.PathEffect = null;
                        }

                        // box度数じゃないからちょっとずれるんだよね
                        double calcDegree = x.sourceDegree - houseList2[1];
                        if (calcDegree < 0)
                        {
                            calcDegree += 360;
                        }
                        double calcDegree2 = x.targetDegree - houseList2[1];
                        if (calcDegree2 < 0)
                        {
                            calcDegree2 += 360;
                        }

                        if (appDelegate.bands == 2)
                        {
                            offset = (diameter / 2 - zodiacWidth - centerDiameter) / 2;
                        }
                        else if (appDelegate.bands == 3)
                        {
                            offset = (diameter / 2 - zodiacWidth - centerDiameter) / 3;
                        }

                        aspectPt = Util.Rotate(centerDiameter + offset, 0, calcDegree);
                        aspectPt.x = aspectPt.x + CenterX;
                        aspectPt.y = -1 * aspectPt.y + CenterY;

                        aspectPtEnd = Util.Rotate(centerDiameter + offset, 0, calcDegree2);
                        aspectPtEnd.x = aspectPtEnd.x + CenterX;
                        aspectPtEnd.y = -1 * aspectPtEnd.y + CenterY;

                        GetAspectLineAndText(x.aspectKind, ref aspectLine, ref aspectSymboolText);
                        DrawAspect(cvs, x, aspectPt, aspectPtEnd, aspectLine, aspectSymboolText);
                    }
                }
            }

            // 三重円アスペクト
            // aspectsData[0, 2]
            if (appDelegate.bands > 2 && appDelegate.aspect13disp)
            {
                foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData)
                {
                    PlanetData planet = pData.Value;

                    if (!planet.isDisp)
                    {
                        continue;
                    }
                    // isAspectDispは不要

                    foreach (AspectInfo x in planet.thirdAspects)
                    {
                        if (!list1[x.targetPlanetNo].isDisp)
                        {
                            continue;
                        }
                        if (!list3[x.targetPlanetNo].isDisp)
                        {
                            continue;
                        }

                        if (x.softHard == SoftHard.SOFT)
                        {
                            aspectLine.PathEffect = SKPathEffect.CreateDash(new float[] { 1, 4 }, (float)2.0);
                        }
                        else
                        {
                            aspectLine.PathEffect = null;
                        }

                        // box度数じゃないからちょっとずれるんだよね
                        double calcDegree = x.sourceDegree - houseList1[1];
                        if (calcDegree < 0)
                        {
                            calcDegree += 360;
                        }
                        double calcDegree2 = x.targetDegree - houseList1[1];
                        if (calcDegree2 < 0)
                        {
                            calcDegree2 += 360;
                        }
                        aspectPt = Util.Rotate(centerDiameter, 0, calcDegree);
                        aspectPt.x = aspectPt.x + CenterX;
                        aspectPt.y = -1 * aspectPt.y + CenterY;

                        offset = (diameter / 2 - zodiacWidth - centerDiameter) / 3;
                        aspectPtEnd = Util.Rotate(centerDiameter + offset * 2, 0, calcDegree2);
                        aspectPtEnd.x = aspectPtEnd.x + CenterX;
                        aspectPtEnd.y = -1 * aspectPtEnd.y + CenterY;

                        GetAspectLineAndText(x.aspectKind, ref aspectLine, ref aspectSymboolText);
                        DrawAspect(cvs, x, aspectPt, aspectPtEnd, aspectLine, aspectSymboolText);
                    }
                }
            }

            // 三重円アスペクト
            // aspectsData[1, 2]
            if (appDelegate.bands > 2 && appDelegate.aspect23disp)
            {
                IOrderedEnumerable<KeyValuePair<int, PlanetData>> sortPlanetData2 = list2.OrderBy(pair => pair.Value.absolute_position);
                foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData2)
                {
                    PlanetData planet = pData.Value;

                    if (!planet.isDisp)
                    {
                        continue;
                    }
                    // isAspectDispは不要

                    foreach (AspectInfo x in planet.thirdAspects)
                    {
                        if (!list2[x.targetPlanetNo].isDisp)
                        {
                            continue;
                        }
                        if (!list3[x.targetPlanetNo].isDisp)
                        {
                            continue;
                        }

                        if (x.softHard == SoftHard.SOFT)
                        {
                            aspectLine.PathEffect = SKPathEffect.CreateDash(new float[] { 1, 4 }, (float)2.0);
                        }
                        else
                        {
                            aspectLine.PathEffect = null;
                        }

                        // box度数じゃないからちょっとずれるんだよね
                        double calcDegree = x.sourceDegree - houseList1[1];
                        if (calcDegree < 0)
                        {
                            calcDegree += 360;
                        }
                        double calcDegree2 = x.targetDegree - houseList1[1];
                        if (calcDegree2 < 0)
                        {
                            calcDegree2 += 360;
                        }
                        offset = (diameter / 2 - zodiacWidth - centerDiameter) / 3;
                        aspectPt = Util.Rotate(centerDiameter + offset, 0, calcDegree);
                        aspectPt.x = aspectPt.x + CenterX;
                        aspectPt.y = -1 * aspectPt.y + CenterY;

                        aspectPtEnd = Util.Rotate(centerDiameter + offset * 2, 0, calcDegree2);
                        aspectPtEnd.x = aspectPtEnd.x + CenterX;
                        aspectPtEnd.y = -1 * aspectPtEnd.y + CenterY;

                        GetAspectLineAndText(x.aspectKind, ref aspectLine, ref aspectSymboolText);
                        DrawAspect(cvs, x, aspectPt, aspectPtEnd, aspectLine, aspectSymboolText);
                    }
                }
            }

            // 三重円アスペクト
            // aspectsData[2, 2]
            if (appDelegate.bands > 2 && appDelegate.aspect33disp)
            {
                IOrderedEnumerable<KeyValuePair<int, PlanetData>> sortPlanetData2 = list2.OrderBy(pair => pair.Value.absolute_position);
                foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData2)
                {
                    PlanetData planet = pData.Value;

                    if (!planet.isDisp)
                    {
                        continue;
                    }
                    // isAspectDispは不要

                    foreach (AspectInfo x in planet.thirdAspects)
                    {
                        if (!list3[x.targetPlanetNo].isDisp)
                        {
                            continue;
                        }

                        if (x.softHard == SoftHard.SOFT)
                        {
                            aspectLine.PathEffect = SKPathEffect.CreateDash(new float[] { 1, 4 }, (float)2.0);
                        }
                        else
                        {
                            aspectLine.PathEffect = null;
                        }

                        // box度数じゃないからちょっとずれるんだよね
                        double calcDegree = x.sourceDegree - houseList1[1];
                        if (calcDegree < 0)
                        {
                            calcDegree += 360;
                        }
                        double calcDegree2 = x.targetDegree - houseList1[1];
                        if (calcDegree2 < 0)
                        {
                            calcDegree2 += 360;
                        }
                        offset = (diameter / 2 - zodiacWidth - centerDiameter) / 3;
                        aspectPt = Util.Rotate(centerDiameter + offset * 2, 0, calcDegree);
                        aspectPt.x = aspectPt.x + CenterX;
                        aspectPt.y = -1 * aspectPt.y + CenterY;

                        aspectPtEnd = Util.Rotate(centerDiameter + offset * 2, 0, calcDegree2);
                        aspectPtEnd.x = aspectPtEnd.x + CenterX;
                        aspectPtEnd.y = -1 * aspectPtEnd.y + CenterY;

                        GetAspectLineAndText(x.aspectKind, ref aspectLine, ref aspectSymboolText);
                        DrawAspect(cvs, x, aspectPt, aspectPtEnd, aspectLine, aspectSymboolText);
                    }
                }
            }

        }
    }
}

