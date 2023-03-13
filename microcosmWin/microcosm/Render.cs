using microcosm.Aspect;
using microcosm.common;
using microcosm.config;
using microcosm.Planet;
using microcosm.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace microcosm
{
    public class Render
    {
        public MainWindow mainWindow;
        public ConfigData configData;
        public RingCanvasViewModel rcanvasVM;
        public Canvas ringCanvas;
        public StackPanel ringStack;
        public TempSetting tempSettings;
        public SettingData currentSetting;

        public Dictionary<int, PlanetData> list1;
        public Dictionary<int, PlanetData> list2;
        public Dictionary<int, PlanetData> list3;

        public double[] houseList1;
        public double[] houseList2;
        public double[] houseList3;

        public Render(ConfigData configData, RingCanvasViewModel rcanvasVM, Canvas ringCanvas, StackPanel ringStack, TempSetting tempSettings)
        {
            this.configData = configData;
            this.rcanvasVM = rcanvasVM;
            this.ringCanvas = ringCanvas;
            this.ringStack = ringStack;
            this.tempSettings = tempSettings;
        }

        public void SetPlanetList(Dictionary<int, PlanetData> list1, Dictionary<int, PlanetData> list2, Dictionary<int, PlanetData> list3)
        {
            this.list1 = list1;
            this.list2 = list2;
            this.list3 = list3;
        }

        public void SetHouseList(double[] houseList1, double[] houseList2, double[] houseList3)
        {
            this.houseList1 = houseList1;
            this.houseList2 = houseList2;
            this.houseList3 = houseList3;
        }

        public void SetCurrentSetting(SettingData currentSetting)
        {
            this.currentSetting = currentSetting;
        }


        public void AllClear(RingCanvasViewModel rcanvas, Canvas ringCanvas)
        {
            rcanvas.natalSunTxt = "";
            rcanvas.natalSunDegreeTxt = "";
            rcanvas.natalSunSignTxt = "";
            rcanvas.natalSunMinuteTxt = "";
            rcanvas.natalSunRetrogradeTxt = "";
            rcanvas.natalEarthTxt = "";
            rcanvas.natalEarthDegreeTxt = "";
            rcanvas.natalEarthSignTxt = "";
            rcanvas.natalEarthMinuteTxt = "";
            rcanvas.natalEarthRetrogradeTxt = "";
            // 最終的に
            ringCanvas.Children.Clear();
        }


        /// <summary>
        /// レンダリングメイン
        /// disp変更の場合はこれだけ呼ぶ
        /// </summary>
        public void ReRender()
        {
#if DEBUG
            DateTime startDt = DateTime.Now;
#endif
            AllClear(rcanvasVM, ringCanvas);
            rcanvasVM.innerLeft = configData.zodiacWidth / 2;
            rcanvasVM.innerTop = configData.zodiacWidth / 2;
            if (ringCanvas.ActualWidth > ringStack.ActualHeight)
            {
                if (tempSettings.bands > 1)
                {
                    tempSettings.zodiacCenter = (int)(ringStack.ActualHeight * 0.9 / 2);
                }
                else
                {
                    if (configData.dispPattern == EDispPetern.MINI)
                    {
                        tempSettings.zodiacCenter = (int)(ringStack.ActualHeight * 0.6);

                    }
                    else
                    {
                        tempSettings.zodiacCenter = (int)(ringStack.ActualHeight * 0.5);

                    }
                }

                rcanvasVM.outerWidth = ringStack.ActualHeight;
                rcanvasVM.outerHeight = ringStack.ActualHeight;
                rcanvasVM.innerWidth = ringStack.ActualHeight - configData.zodiacWidth;
                rcanvasVM.innerHeight = ringStack.ActualHeight - configData.zodiacWidth;
                rcanvasVM.centerLeft = ringStack.ActualHeight / 2 - tempSettings.zodiacCenter / 2;
                rcanvasVM.centerTop = ringStack.ActualHeight / 2 - tempSettings.zodiacCenter / 2;
            }
            else
            {
                if (tempSettings.bands > 1)
                {
                    tempSettings.zodiacCenter = (int)(ringCanvas.ActualWidth * 0.9 / 2);
                }
                else
                {
                    if (configData.dispPattern == EDispPetern.MINI)
                    {
                        tempSettings.zodiacCenter = (int)(ringStack.ActualWidth * 0.6);

                    }
                    else
                    {
                        tempSettings.zodiacCenter = (int)(ringCanvas.ActualWidth * 0.5);
                    }
                }

                rcanvasVM.outerWidth = ringCanvas.ActualWidth;
                rcanvasVM.outerHeight = ringCanvas.ActualWidth;
                rcanvasVM.innerWidth = ringCanvas.ActualWidth - configData.zodiacWidth;
                rcanvasVM.innerHeight = ringCanvas.ActualWidth - configData.zodiacWidth;
                rcanvasVM.centerLeft = ringCanvas.ActualWidth / 2 - tempSettings.zodiacCenter / 2;
                rcanvasVM.centerTop = ringCanvas.ActualWidth / 2 - tempSettings.zodiacCenter / 2;
            }


            // Console.WriteLine(ringCanvas.ActualWidth.ToString() + "," + ringStack.ActualHeight.ToString());

            circleRender();
            houseCuspRender(houseList1, houseList2, houseList3);
            signCuspRender(houseList1[1]);
            zodiacRender(houseList1[1]);

            // 並べ替え
            // こうしないと冥王星10度、月11度となったとき月のほうが先に表示されてしまう
            List<PlanetData> newList1 = new List<PlanetData>();
            foreach (var keys in list1.Keys)
            {
                newList1.Add(list1[keys]);
            }

            List<PlanetData> newList2 = new List<PlanetData>();
            foreach (var keys in list2.Keys)
            {
                newList2.Add(list2[keys]);
            }

            List<PlanetData> newList3 = new List<PlanetData>();
            foreach (var keys in list3.Keys)
            {
                newList3.Add(list3[keys]);
            }
            newList1.Sort((a, b) => (int)(a.absolute_position * 100 - b.absolute_position * 100));
            newList2.Sort((a, b) => (int)(a.absolute_position * 100 - b.absolute_position * 100));
            newList3.Sort((a, b) => (int)(a.absolute_position * 100 - b.absolute_position * 100));
            planetRender(houseList1[1], newList1, newList2, newList3);
            planetLine(houseList1[1], newList1, newList2, newList3);

            // アスペクト描画がおかしくなるので戻しておく
            newList1.Sort((a, b) => (int)(a.no - b.no));
            newList2.Sort((a, b) => (int)(a.no - b.no));
            newList3.Sort((a, b) => (int)(a.no - b.no));
            aspectsRendering(houseList1[1], list1, list2, list3);

        }

        /// <summary>
        /// 円レンダリング
        /// </summary>
        private void circleRender()
        {
            // 獣帯外側
            // Ellipseは左上の座標
            // Macで使ってるSkiaSharpは中心座標
            Ellipse outerEllipse = new Ellipse()
            {
                StrokeThickness = 3,
                Margin = new Thickness(15, 15, 15, 15),
                Stroke = System.Windows.SystemColors.WindowTextBrush
            };
            if (ringCanvas.ActualWidth > ringStack.ActualHeight)
            {
                // 横長(Heightのほうが短い)
                outerEllipse.Width = ringStack.ActualHeight - 30;
                outerEllipse.Height = ringStack.ActualHeight - 30;

            }
            else
            {
                // 縦長(Widthのほうが短い)
                outerEllipse.Width = ringStack.ActualWidth - 30;
                outerEllipse.Height = ringStack.ActualWidth - 30;
            }
            ringCanvas.Children.Add(outerEllipse);

            // 獣帯内側
            Ellipse innerEllipse = new Ellipse()
            {
                StrokeThickness = 3,
                Margin = new Thickness(45, 45, 45, 45),
                Stroke = System.Windows.SystemColors.WindowTextBrush
            };
            //Ellipse.Widthは直径
            if (ringCanvas.ActualWidth > ringStack.ActualHeight)
            {
                // 横長(Heightのほうが短い)
                innerEllipse.Width = ringStack.ActualHeight - 90;
                innerEllipse.Height = ringStack.ActualHeight - 90;

            }
            else
            {
                // 縦長(Widthのほうが短い)
                innerEllipse.Width = ringCanvas.ActualWidth - 90;
                innerEllipse.Height = ringCanvas.ActualWidth - 90;
            }
            ringCanvas.Children.Add(innerEllipse);

            // 中心
            int marginSize;
            if (ringCanvas.ActualWidth > ringStack.ActualHeight)
            {
                // 横長
                marginSize = (int)((ringStack.ActualHeight - tempSettings.zodiacCenter) / 2);
            }
            else
            {
                // 縦長
                marginSize = (int)((ringCanvas.ActualWidth - tempSettings.zodiacCenter) / 2);
            }
            Ellipse centerEllipse = new Ellipse()
            {
                StrokeThickness = 3,
                Stroke = System.Windows.SystemColors.WindowTextBrush,
                Width = tempSettings.zodiacCenter,
                Height = tempSettings.zodiacCenter
            };
            centerEllipse.Margin = new Thickness(marginSize, marginSize, marginSize, marginSize);
            ringCanvas.Children.Add(centerEllipse);

            // 二重円
            if (tempSettings.bands == 2)
            {
                int margin2Size;
                Ellipse ring2Ellipse = new Ellipse()
                {
                    StrokeThickness = 1,
                    Stroke = System.Windows.Media.Brushes.Gray,
                };
                if (ringCanvas.ActualWidth > ringStack.ActualHeight)
                {
                    // 横長
                    margin2Size = (int)(ringStack.ActualHeight - 90 - tempSettings.zodiacCenter) / 4 + 45;
                    ring2Ellipse.Width = (int)(ringStack.ActualHeight + tempSettings.zodiacCenter - 90) / 2;
                    ring2Ellipse.Height = (int)(ringStack.ActualHeight + tempSettings.zodiacCenter - 90) / 2;
                }
                else
                {
                    // 縦長
                    margin2Size = (int)(ringStack.ActualWidth - 90 - tempSettings.zodiacCenter) / 4 + 45;
                    ring2Ellipse.Width = (int)(ringStack.ActualWidth + tempSettings.zodiacCenter - 90) / 2;
                    ring2Ellipse.Height = (int)(ringStack.ActualWidth + tempSettings.zodiacCenter - 90) / 2;
                }
                ring2Ellipse.Margin = new Thickness(margin2Size, margin2Size, margin2Size, margin2Size);
                ringCanvas.Children.Add(ring2Ellipse);
            }

            // 三重円
            if (tempSettings.bands == 3)
            {
                int margin2Size;
                Ellipse ring2Ellipse = new Ellipse()
                {
                    StrokeThickness = 1,
                    Stroke = System.Windows.Media.Brushes.Gray,
                };
                if (ringCanvas.ActualWidth > ringStack.ActualHeight)
                {
                    // 横長
                    margin2Size = (int)(ringStack.ActualHeight - 90 - tempSettings.zodiacCenter) / 3 + 45;
                    ring2Ellipse.Width = (int)(ringStack.ActualHeight - 90 - tempSettings.zodiacCenter) / 3 + tempSettings.zodiacCenter;
                    ring2Ellipse.Height = (int)(ringStack.ActualHeight - 90 - tempSettings.zodiacCenter) / 3 + tempSettings.zodiacCenter;
                }
                else
                {
                    // 縦長
                    margin2Size = (int)(ringStack.ActualWidth - 90 - tempSettings.zodiacCenter) / 3 + 45;
                    ring2Ellipse.Width = (int)(ringStack.ActualWidth - 90 - tempSettings.zodiacCenter) / 3 + tempSettings.zodiacCenter;
                    ring2Ellipse.Height = (int)(ringStack.ActualWidth - 90 - tempSettings.zodiacCenter) / 3 + tempSettings.zodiacCenter;
                }
                ring2Ellipse.Margin = new Thickness(margin2Size, margin2Size, margin2Size, margin2Size);
                ringCanvas.Children.Add(ring2Ellipse);

                int margin3Size;
                Ellipse ring3Ellipse = new Ellipse()
                {
                    StrokeThickness = 1,
                    Stroke = System.Windows.Media.Brushes.Gray,
                };
                if (ringCanvas.ActualWidth > ringStack.ActualHeight)
                {
                    // 横長
                    margin3Size = (int)(ringStack.ActualHeight - 90 - tempSettings.zodiacCenter) / 6 + 45;
                    ring3Ellipse.Width = (int)(ringStack.ActualHeight - 90 - tempSettings.zodiacCenter) * 2 / 3 + tempSettings.zodiacCenter;
                    ring3Ellipse.Height = (int)(ringStack.ActualHeight - 90 - tempSettings.zodiacCenter) * 2 / 3 + tempSettings.zodiacCenter;
                }
                else
                {
                    // 縦長
                    margin3Size = (int)(ringStack.ActualWidth - 90 - tempSettings.zodiacCenter) / 6 + 45;
                    ring3Ellipse.Width = (int)(ringStack.ActualWidth - 90 - tempSettings.zodiacCenter) * 2 / 3 + tempSettings.zodiacCenter;
                    ring3Ellipse.Height = (int)(ringStack.ActualWidth - 90 - tempSettings.zodiacCenter) * 2 / 3 + tempSettings.zodiacCenter;
                }
                ring3Ellipse.Margin = new Thickness(margin3Size, margin3Size, margin3Size, margin3Size);
                ringCanvas.Children.Add(ring3Ellipse);

            }

        }

        /// <summary>
        /// ハウスカスプレンダリング
        /// </summary>
        /// <param name="natalcusp"></param>
        /// <param name="cusp2"></param>
        /// <param name="cusp3"></param>
        private void houseCuspRender(double[] natalcusp,
            double[] cusp2,
            double[] cusp3
            )
        {
            //内側がstart, 外側がend
            double startX = tempSettings.zodiacCenter / 2;
            double endX;
            if (ringStack.ActualHeight < ringStack.ActualWidth)
            {
                endX = (ringStack.ActualHeight - 90) / 2;
            }
            else
            {
                endX = (ringCanvas.ActualWidth - 90) / 2;
            }

            double startY = 0;
            double endY = 0;
            List<PointF[]> pList = new List<PointF[]>();
            List<PointF[]> pListSecond = new List<PointF[]>();
            List<PointF[]> pListThird = new List<PointF[]>();

            // 最適化は後で
            if (tempSettings.bands == 1)
            {
                Enumerable.Range(1, 12).ToList().ForEach(i =>
                {
                    double degree = natalcusp[i] - natalcusp[1];

                    PointF newStart = CommonData.rotate(startX, startY, degree);
                    newStart.X += (float)rcanvasVM.outerWidth / 2;
                    // Formの座標は下がプラス、数学では上がマイナス
                    newStart.Y = newStart.Y * -1;
                    newStart.Y += (float)rcanvasVM.outerHeight / 2;

                    PointF newEnd = CommonData.rotate(endX, endY, degree);
                    newEnd.X += (float)rcanvasVM.outerWidth / 2;
                    // Formの座標は下がプラス、数学では上がマイナス
                    newEnd.Y = newEnd.Y * -1;
                    newEnd.Y += (float)rcanvasVM.outerHeight / 2;

                    PointF[] pointList = new PointF[2];
                    pointList[0] = newStart;
                    pointList[1] = newEnd;
                    pList.Add(pointList);

                });
            }
            else if (tempSettings.bands == 2)
            {
                // end座標が変わる
                Enumerable.Range(1, 12).ToList().ForEach(i =>
                {
                    double degree = natalcusp[i] - natalcusp[1];

                    PointF newStart = CommonData.rotate(startX, startY, degree);
                    newStart.X += (float)rcanvasVM.outerWidth / 2;
                    // Formの座標は下がプラス、数学では上がマイナス
                    newStart.Y = newStart.Y * -1;
                    newStart.Y += (float)rcanvasVM.outerHeight / 2;

                    if (!currentSetting.sameCusps)
                    {
                        endX = (rcanvasVM.outerWidth - 90) / 2;
                    }
                    PointF newEnd = CommonData.rotate(endX, endY, degree);
                    newEnd.X += (float)rcanvasVM.outerWidth / 2;
                    // Formの座標は下がプラス、数学では上がマイナス
                    newEnd.Y = newEnd.Y * -1;
                    newEnd.Y += (float)rcanvasVM.outerHeight / 2;

                    PointF[] pointList = new PointF[2];
                    pointList[0] = newStart;
                    pointList[1] = newEnd;
                    pList.Add(pointList);

                });
                // start座標が変わる
                Enumerable.Range(1, 12).ToList().ForEach(i =>
                {
                    double degree = cusp2[i] - natalcusp[1];

                    startX = (rcanvasVM.outerWidth - 90) / 2;
                    PointF newStart = CommonData.rotate(startX, startY, degree);
                    newStart.X += (float)rcanvasVM.outerWidth / 2;
                    // Formの座標は下がプラス、数学では上がマイナス
                    newStart.Y = newStart.Y * -1;
                    newStart.Y += (float)rcanvasVM.outerHeight / 2;

                    PointF newEnd = CommonData.rotate(endX, endY, degree);
                    newEnd.X += (float)rcanvasVM.outerWidth / 2;
                    // Formの座標は下がプラス、数学では上がマイナス
                    newEnd.Y = newEnd.Y * -1;
                    newEnd.Y += (float)rcanvasVM.outerHeight / 2;

                    PointF[] pointList = new PointF[2];
                    pointList[0] = newStart;
                    pointList[1] = newEnd;
                    pListSecond.Add(pointList);

                });
            }
            if (tempSettings.bands == 3)
            {
                // end座標が変わる
                Enumerable.Range(1, 12).ToList().ForEach(i =>
                {
                    double degree = natalcusp[i] - natalcusp[1];

                    PointF newStart = CommonData.rotate(startX, startY, degree);
                    newStart.X += (float)rcanvasVM.outerWidth / 2;
                    // Formの座標は下がプラス、数学では上がマイナス
                    newStart.Y = newStart.Y * -1;
                    newStart.Y += (float)rcanvasVM.outerHeight / 2;

                    if (!currentSetting.sameCusps)
                    {
                        endX = (rcanvasVM.outerWidth + 2 * tempSettings.zodiacCenter - 90) / 6;
                    }
                    PointF newEnd = CommonData.rotate(endX, endY, degree);
                    newEnd.X += (float)rcanvasVM.outerWidth / 2;
                    // Formの座標は下がプラス、数学では上がマイナス
                    newEnd.Y = newEnd.Y * -1;
                    newEnd.Y += (float)rcanvasVM.outerHeight / 2;

                    PointF[] pointList = new PointF[2];
                    pointList[0] = newStart;
                    pointList[1] = newEnd;
                    pList.Add(pointList);

                });
                // start座標、end座標が変わる
                Enumerable.Range(1, 12).ToList().ForEach(i =>
                {
                    double degree = cusp2[i] - natalcusp[1];

                    startX = (rcanvasVM.outerWidth + 2 * tempSettings.zodiacCenter - 90) / 6;
                    PointF newStart = CommonData.rotate(startX, startY, degree);
                    newStart.X += (float)rcanvasVM.outerWidth / 2;
                    // Formの座標は下がプラス、数学では上がマイナス
                    newStart.Y = newStart.Y * -1;
                    newStart.Y += (float)rcanvasVM.outerHeight / 2;

                    endX = (2 * rcanvasVM.outerWidth + tempSettings.zodiacCenter - 180) / 6;
                    PointF newEnd = CommonData.rotate(endX, endY, degree);
                    newEnd.X += (float)rcanvasVM.outerWidth / 2;
                    // Formの座標は下がプラス、数学では上がマイナス
                    newEnd.Y = newEnd.Y * -1;
                    newEnd.Y += (float)rcanvasVM.outerHeight / 2;

                    PointF[] pointList = new PointF[2];
                    pointList[0] = newStart;
                    pointList[1] = newEnd;
                    pListSecond.Add(pointList);

                });
                Enumerable.Range(1, 12).ToList().ForEach(i =>
                {
                    double degree = cusp3[i] - natalcusp[1];

                    startX = (2 * rcanvasVM.outerWidth + tempSettings.zodiacCenter - 180) / 6;
                    PointF newStart = CommonData.rotate(startX, startY, degree);
                    newStart.X += (float)rcanvasVM.outerWidth / 2;
                    // Formの座標は下がプラス、数学では上がマイナス
                    newStart.Y = newStart.Y * -1;
                    newStart.Y += (float)rcanvasVM.outerHeight / 2;

                    if (ringCanvas.ActualWidth > ringStack.ActualHeight)
                    {
                        // 横長(Heightのほうが短い)
                        endX = (ringStack.ActualHeight - 90) / 2;

                    }
                    else
                    {
                        // 縦長(Widthのほうが短い)
                        endX = (ringCanvas.ActualWidth - 90) / 2;
                    }
                    PointF newEnd = CommonData.rotate(endX, endY, degree);
                    newEnd.X += (float)rcanvasVM.outerWidth / 2;
                    // Formの座標は下がプラス、数学では上がマイナス
                    newEnd.Y = newEnd.Y * -1;
                    newEnd.Y += (float)rcanvasVM.outerHeight / 2;

                    PointF[] pointList = new PointF[2];
                    pointList[0] = newStart;
                    pointList[1] = newEnd;
                    pListThird.Add(pointList);

                });
            }

            Enumerable.Range(0, 12).ToList().ForEach(i =>
            {
                Line l = new Line();
                l.X1 = pList[i][0].X;
                l.Y1 = pList[i][0].Y;
                l.X2 = pList[i][1].X;
                l.Y2 = pList[i][1].Y;
                if (i % 3 == 0)
                {
                    l.Stroke = System.Windows.Media.Brushes.Gray;
                }
                else
                {
                    l.Stroke = System.Windows.Media.Brushes.LightGray;
                    l.StrokeDashArray = new DoubleCollection();
                    l.StrokeDashArray.Add(4.0);
                    l.StrokeDashArray.Add(4.0);
                }
                l.StrokeThickness = 2.0;
                l.Tag = new Explanation()
                {
                    before = (i + 1).ToString() + "ハウス　",
                    sign = CommonData.getSignTextJp(natalcusp[i + 1]),
                    degree = natalcusp[i + 1] % 30
                };
                l.MouseEnter += houseCuspMouseEnter;
                if (i == 0)
                {
                    l.ToolTip = "ASC " + CommonData.getSignTextJp(natalcusp[i + 1]) + (natalcusp[i + 1] % 30).ToString("0") + "度";
                }
                else if (i == 3)
                {
                    l.ToolTip = "IC " + CommonData.getSignTextJp(natalcusp[i + 1]) + (natalcusp[i + 1] % 30).ToString("0") + "度";
                }
                else if (i == 6)
                {
                    l.ToolTip = "DSC " + CommonData.getSignTextJp(natalcusp[i + 1]) + (natalcusp[i + 1] % 30).ToString("0") + "度";
                }
                else if (i == 9)
                {
                    l.ToolTip = "MC " + CommonData.getSignTextJp(natalcusp[i + 1]) + (natalcusp[i + 1] % 30).ToString("0") + "度";
                }
                else
                {
                    l.ToolTip = (i + 1).ToString() + "ハウスカスプ " + CommonData.getSignTextJp(natalcusp[i + 1]) + (natalcusp[i + 1] % 30).ToString("0") + "度";
                }
                ringCanvas.Children.Add(l);
            });
            if (tempSettings.bands >= 2 && !currentSetting.sameCusps)
            {
                Enumerable.Range(0, 12).ToList().ForEach(i =>
                {
                    Line l = new Line();
                    l.X1 = pListSecond[i][0].X;
                    l.Y1 = pListSecond[i][0].Y;
                    l.X2 = pListSecond[i][1].X;
                    l.Y2 = pListSecond[i][1].Y;
                    if (i % 3 == 0)
                    {
                        l.Stroke = System.Windows.Media.Brushes.Gray;
                    }
                    else
                    {
                        l.Stroke = System.Windows.Media.Brushes.LightGray;
                        l.StrokeDashArray = new DoubleCollection();
                        l.StrokeDashArray.Add(4.0);
                        l.StrokeDashArray.Add(4.0);
                    }
                    l.StrokeThickness = 2.0;
                    l.Tag = new Explanation()
                    {
                        before = (i + 1).ToString() + "ハウス　",
                        sign = CommonData.getSignTextJp(natalcusp[i + 1]),
                        degree = CommonData.DecimalToHex(natalcusp[i + 1] % 30)
                    };
                    l.MouseEnter += houseCuspMouseEnter;
                    if (i == 0)
                    {
                        l.ToolTip = "ASC " + CommonData.getSignTextJp(natalcusp[i + 1]) + (natalcusp[i + 1] % 30).ToString("0") + "度";
                    }
                    else if (i == 3)
                    {
                        l.ToolTip = "IC " + CommonData.getSignTextJp(natalcusp[i + 1]) + (natalcusp[i + 1] % 30).ToString("0") + "度";
                    }
                    else if (i == 6)
                    {
                        l.ToolTip = "DSC " + CommonData.getSignTextJp(natalcusp[i + 1]) + (natalcusp[i + 1] % 30).ToString("0") + "度";
                    }
                    else if (i == 9)
                    {
                        l.ToolTip = "MC " + CommonData.getSignTextJp(natalcusp[i + 1]) + (natalcusp[i + 1] % 30).ToString("0") + "度";
                    }
                    else
                    {
                        l.ToolTip = (i + 1).ToString() + "ハウスカスプ " + CommonData.getSignTextJp(natalcusp[i + 1]) + (natalcusp[i + 1] % 30).ToString("0") + "度";
                    }
                    ringCanvas.Children.Add(l);
                });
            }
            if (tempSettings.bands >= 3 && !currentSetting.sameCusps)
            {
                Enumerable.Range(0, 12).ToList().ForEach(i =>
                {
                    Line l = new Line();
                    l.X1 = pListThird[i][0].X;
                    l.Y1 = pListThird[i][0].Y;
                    l.X2 = pListThird[i][1].X;
                    l.Y2 = pListThird[i][1].Y;
                    if (i % 3 == 0)
                    {
                        l.Stroke = System.Windows.Media.Brushes.Gray;
                    }
                    else
                    {
                        l.Stroke = System.Windows.Media.Brushes.LightGray;
                        l.StrokeDashArray = new DoubleCollection();
                        l.StrokeDashArray.Add(4.0);
                        l.StrokeDashArray.Add(4.0);
                    }
                    l.StrokeThickness = 2.0;
                    l.Tag = new Explanation()
                    {
                        before = (i + 1).ToString() + "ハウス　",
                        sign = CommonData.getSignTextJp(natalcusp[i + 1]),
                        degree = CommonData.DecimalToHex(natalcusp[i + 1] % 30)
                    };
                    l.MouseEnter += houseCuspMouseEnter;
                    if (i == 0)
                    {
                        l.ToolTip = "ASC " + CommonData.getSignTextJp(natalcusp[i + 1]) + (natalcusp[i + 1] % 30).ToString("0") + "度";
                    }
                    else if (i == 3)
                    {
                        l.ToolTip = "IC " + CommonData.getSignTextJp(natalcusp[i + 1]) + (natalcusp[i + 1] % 30).ToString("0") + "度";
                    }
                    else if (i == 6)
                    {
                        l.ToolTip = "DSC " + CommonData.getSignTextJp(natalcusp[i + 1]) + (natalcusp[i + 1] % 30).ToString("0") + "度";
                    }
                    else if (i == 9)
                    {
                        l.ToolTip = "MC " + CommonData.getSignTextJp(natalcusp[i + 1]) + (natalcusp[i + 1] % 30).ToString("0") + "度";
                    }
                    else
                    {
                        l.ToolTip = (i + 1).ToString() + "ハウスカスプ " + CommonData.getSignTextJp(natalcusp[i + 1]) + (natalcusp[i + 1] % 30).ToString("0") + "度";
                    }
                    ringCanvas.Children.Add(l);
                });
            }
        }

        /// <summary>
        /// サインカスプレンダリング
        /// </summary>
        /// <param name="startdegree"></param>
        private void signCuspRender(double startdegree)
        {
            // 内側がstart、外側がend
            // margin + thickness * 3だけ実際のリングの幅と差がある
            double startX = (rcanvasVM.innerWidth - 29) / 2;
            double endX = (rcanvasVM.outerWidth - 29) / 2;

            double startY = 0;
            double endY = 0;
            List<PointF[]> pList = new List<PointF[]>();

            Enumerable.Range(1, 12).ToList().ForEach(i =>
            {
                double degree = (30.0 * i) - startdegree;

                PointF newStart = CommonData.rotate(startX, startY, degree);
                newStart.X += (float)(rcanvasVM.outerWidth) / 2;
                // Formの座標は下がプラス、数学では上がマイナス
                newStart.Y = newStart.Y * -1;
                newStart.Y += (float)(rcanvasVM.outerHeight) / 2;

                PointF newEnd = CommonData.rotate(endX, endY, degree);
                newEnd.X += (float)(rcanvasVM.outerWidth) / 2;
                // Formの座標は下がプラス、数学では上がマイナス
                newEnd.Y = newEnd.Y * -1;
                newEnd.Y += (float)(rcanvasVM.outerHeight) / 2;

                PointF[] pointList = new PointF[2];
                pointList[0] = newStart;
                pointList[1] = newEnd;
                pList.Add(pointList);
            });

            Enumerable.Range(0, 12).ToList().ForEach(i =>
            {
                Line l = new Line();
                l.X1 = pList[i][0].X;
                l.Y1 = pList[i][0].Y;
                l.X2 = pList[i][1].X;
                l.Y2 = pList[i][1].Y;
                l.Stroke = System.Windows.Media.Brushes.Black;
                l.StrokeThickness = 1.0;
                ringCanvas.Children.Add(l);
            });
        }

        /// <summary>
        /// zodiac文字列描画
        /// 獣帯に乗る文字ね
        /// </summary>
        /// <param name="startdegree"></param>
        private void zodiacRender(double startdegree)
        {
            List<PointF> pList = new List<PointF>();
            Enumerable.Range(0, 12).ToList().ForEach(i =>
            {
                PointF point = CommonData.rotate(rcanvasVM.outerWidth / 2 - 31, 0, (30 * (i + 1)) - startdegree - 15.0);
                point.X += (float)rcanvasVM.outerWidth / 2 - 12;
                //                point.X -= (float)rcanvas.outerWidth - (float)rcanvas.innerWidth;
                point.Y *= -1;
                point.Y += (float)rcanvasVM.outerHeight / 2 - 12;
                //                point.Y -= (float)rcanvas.outerHeight - (float)rcanvas.innerHeight;
                pList.Add(point);
            });

            Enumerable.Range(0, 12).ToList().ForEach(i =>
            {
                Label zodiacLabel = new Label();
                zodiacLabel.Content = CommonData.getSignSymbol(i);
                zodiacLabel.FontFamily = new System.Windows.Media.FontFamily("file:///" + Util.root() + @"\system\microcosm.otf#microcosm");
                zodiacLabel.FontSize = 24;
                zodiacLabel.Margin = new Thickness(pList[i].X, pList[i].Y - 8, 0, 0);
                zodiacLabel.Foreground = CommonData.getSignColor(i * 30);
                zodiacLabel.ToolTip = CommonData.getSignTextJp(i * 30) + " (ルーラー：" + CommonData.getSignRulersSymbol(i) + ")";
                ringCanvas.Children.Add(zodiacLabel);
            });
        }

        int[] box = new int[72];
        /// <summary>
        /// 天体の表示
        /// </summary>
        /// <param name="startdegree"></param>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <param name="list3"></param>
        private void planetRender(double startdegree,
            List<PlanetData> list1,
            List<PlanetData> list2,
            List<PlanetData> list3
            )
        {
            List<bool> dispList = new List<bool>();
            List<PlanetDisplay> pDisplayList = new List<PlanetDisplay>();

            if (tempSettings.bands == 1)
            {
                // 一重円
                band1(pDisplayList, startdegree, list1);
            }
            else if (tempSettings.bands == 2)
            {
                // 二重円
                band2(pDisplayList, startdegree, list1, list2);
            }
            else if (tempSettings.bands == 3)
            {
                // 三重円
                band3(pDisplayList, startdegree, list1, list2, list3);
            }

        }


        /// <summary>
        /// 一重円
        /// </summary>
        /// <param name="pDisplayList"></param>
        /// <param name="startdegree"></param>
        /// <param name="list1"></param>
        private void band1(List<PlanetDisplay> pDisplayList, double startdegree, List<PlanetData> list1)
        {
            List<bool> dispList = new List<bool>();

            boxReset();
            list1.ForEach(planet =>
            {
                //Debug.WriteLine("{0} {1}", planet.no, planet.isDisp.ToString());
                // 天体表示させない
                if (!planet.isDisp)
                {
                    return;
                }
                if (planet.no == 10000)
                {
                    // 一重円のASC
                    return;
                }
                if (planet.no == 10001)
                {
                    // 一重円のMC
                    return;
                }

                // planet degree symbol minuteの順
                PointF point;
                PointF pointdegree;
                PointF pointsymbol;
                PointF pointminute;
                PointF pointretrograde;
                // 重ならないようにずらしを入れる
                // 1サインに6度単位5個までデータが入る
                int absolute_position = getNewAbsPosition(planet);
                int index = boxSet(absolute_position);

                if (ringCanvas.ActualWidth < 520)
                {
                    // 幅が小さい場合は天体と度数だけしか出さない
                    point = CommonData.rotate(rcanvasVM.outerWidth / 3 - 20, 0, 5 * index - startdegree + 3);
                    pointdegree = CommonData.rotate(rcanvasVM.outerWidth / 3 - 40, 0, 5 * index - startdegree + 3);
                }
                else
                {
                    point = CommonData.rotate(rcanvasVM.outerWidth / 3 + 40, 0, 5 * index - startdegree + 3);
                    pointdegree = CommonData.rotate(rcanvasVM.outerWidth / 3 + 20, 0, 5 * index - startdegree + 3);
                }

                pointsymbol = CommonData.rotate(rcanvasVM.outerWidth / 3, 0, 5 * index - startdegree + 3);
                pointminute = CommonData.rotate(rcanvasVM.outerWidth / 3 - 20, 0, 5 * index - startdegree + 3);
                pointretrograde = CommonData.rotate(rcanvasVM.outerWidth / 3 - 40, 0, 5 * index - startdegree + 3);
                point = getNewPoint(point);

                pointdegree.X += (float)rcanvasVM.outerWidth / 2;
                pointdegree.X -= 8;
                pointsymbol.X += (float)rcanvasVM.outerWidth / 2;
                pointsymbol.X -= 8;
                pointminute.X += (float)rcanvasVM.outerWidth / 2;
                pointminute.X -= 8;
                pointretrograde.X += (float)rcanvasVM.outerWidth / 2;
                pointretrograde.X -= 8;

                pointdegree.Y *= -1;
                pointdegree.Y += (float)rcanvasVM.outerHeight / 2;
                pointdegree = PointUtil.pointMoveUnder(pointdegree, 15);
                pointsymbol.Y *= -1;
                pointsymbol.Y += (float)rcanvasVM.outerHeight / 2;
                pointsymbol = PointUtil.pointMoveUnder(pointsymbol, 15);
                pointminute.Y *= -1;
                pointminute.Y += (float)rcanvasVM.outerHeight / 2;
                pointminute = PointUtil.pointMoveUnder(pointminute, 15);
                pointretrograde.Y *= -1;
                pointretrograde.Y += (float)rcanvasVM.outerHeight / 2;
                pointretrograde = PointUtil.pointMoveUnder(pointretrograde, 15);

                dispList.Add(planet.isDisp);
                bool retrograde = planet.speed < 0;

                // 説明文章
                Explanation exp = new Explanation()
                {
                    planet = CommonData.getPlanetTextOnMouse(planet.no),
                    degree = planet.absolute_position % 30,
                    sign = CommonData.getSignTextJp(planet.absolute_position),
                    retrograde = retrograde,
                    planetNo = planet.no
                };

                string degreeTxt = "";
                // degreeはdecimalでもhexでも変わらない
                degreeTxt = ((int)(planet.absolute_position % 30)).ToString("00");

                string minuteTxt = "";
                if (configData.decimalDisp == EDecimalDisp.DECIMAL)
                {
                    // 小数点2桁を出す (% 1 * 100)
                    minuteTxt = (planet.absolute_position % 1 * 100).ToString("00");
                }
                else
                {
                    minuteTxt = CommonData.DecimalToHex(planet.absolute_position % 1 * 100).ToString("00") + "'";
                }
                PlanetDisplay display = new PlanetDisplay()
                {
                    planetNo = planet.no,
                    isDisp = planet.isDisp,
                    explanation = exp,
                    planetPt = point,
                    planetTxt = CommonData.getPlanetSymbol2(planet.no),
                    planetColor = CommonData.getPlanetColor(planet.no),
                    degreePt = pointdegree,
                    degreeTxt = degreeTxt,
                    symbolPt = pointsymbol,
                    symbolTxt = CommonData.getSignText(planet.absolute_position),
                    minutePt = pointminute,
                    // 小数点以下切り捨て 59.9->59
                    minuteTxt = minuteTxt,
                    retrogradePt = pointretrograde,
                    retrogradeTxt = CommonData.getRetrograde(planet.speed),
                    symbolColor = CommonData.getSignColor(planet.absolute_position)
                };
                pDisplayList.Add(display);

            });

            pDisplayList.ForEach(displayData =>
            {
                if (!displayData.isDisp)
                {
                    return;
                }
                if (ringCanvas.ActualWidth < 520 || configData.dispPattern == EDispPetern.MINI)
                {
                    // 天体と度数のみ
                    SetOnlySignDegree(displayData);
                }
                else
                {
                    SetSign(displayData);
                }

            });
        }

        /// <summary>
        /// 二重円
        /// </summary>
        /// <param name="pDisplayList"></param>
        /// <param name="startdegree"></param>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        private void band2(List<PlanetDisplay> pDisplayList, double startdegree, List<PlanetData> list1, List<PlanetData> list2)
        {
            List<bool> dispList = new List<bool>();
            PointF point;
            PointF pointDegree;
            boxReset();
            // 二重円1st
            list1.ForEach(planet =>
            {
                // 天体表示させない
                if (Util.isNoDisp(planet)) return;


                int absolute_position = getNewAbsPosition(planet);
                int index = boxSet(absolute_position);

                point = CommonData.rotate(rcanvasVM.outerWidth / 4 + 20, 0, 5 * index - startdegree);
                point = getNewPoint(point);
                pointDegree = CommonData.rotate((rcanvasVM.outerWidth / 4), 0, 5 * index - startdegree);
                pointDegree.Y -= 8;
                pointDegree = getNewPoint(pointDegree);

                dispList.Add(planet.isDisp);

                string degreeTxt = Util.getPlanetDegreeTxt(planet);
                PlanetDisplay display = createPlanetDisplay(planet, getExp(planet), point, degreeTxt, pointDegree);
                pDisplayList.Add(display);
            });
            boxReset();

            // 二重円2nd
            list2.ForEach(planet =>
            {
                // 天体表示させない
                if (Util.isNoDisp(planet)) return;

                int absolute_position = getNewAbsPosition(planet);
                int index = boxSet(absolute_position);

                point = CommonData.rotate(rcanvasVM.outerWidth * 0.38, 0, 5 * index - startdegree);
                point = getNewPoint(point);
                pointDegree = CommonData.rotate((rcanvasVM.outerWidth / 3) + 15, 0, 5 * index - startdegree);
                pointDegree.X += 3;
                pointDegree.Y -= 8;

                pointDegree = getNewPoint(pointDegree);

                dispList.Add(planet.isDisp);

                string degreeTxt = Util.getPlanetDegreeTxt(planet);
                PlanetDisplay display = createPlanetDisplay(planet, getExp(planet), point, degreeTxt, pointDegree);
                pDisplayList.Add(display);

            });

            pDisplayList.ForEach(displayData =>
            {
                if (!displayData.isDisp)
                {
                    return;
                }
                SetOnlySignDegree(displayData);
            });

        }

        /// <summary>
        /// 三重円、分けてはみたけどあまり変わらない
        /// </summary>
        /// <param name="pDisplayList"></param>
        /// <param name="startdegree"></param>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <param name="list3"></param>
        private void band3(List<PlanetDisplay> pDisplayList, double startdegree,  List<PlanetData> list1, List<PlanetData> list2, List<PlanetData> list3)
        {
            List<bool> dispList = new List<bool>();

            boxReset();
            list1.ForEach(planet =>
            {
                // 天体表示させない
                if (Util.isNoDisp(planet)) return;

                PointF point;
                PointF pointDegree;
                int absolute_position = getNewAbsPosition(planet);
                int index = boxSet(absolute_position);

                point = CommonData.rotate(rcanvasVM.outerWidth / 4 + 15, 0, 5 * index - startdegree);
                point = getNewPoint(point);
                pointDegree = CommonData.rotate(rcanvasVM.outerWidth / 4, 0, 5 * index - startdegree);
                pointDegree = getNewPoint(pointDegree);
                pointDegree.X += 4;
                pointDegree.Y += 8;
                string degreeTxt = Util.getPlanetDegreeTxt(planet);
                dispList.Add(planet.isDisp);

                Explanation exp = getExp(planet);
                PlanetDisplay display = createPlanetDisplay(planet, exp, point, degreeTxt, pointDegree);
                pDisplayList.Add(display);
            });

            boxReset();

            list2.ForEach(planet =>
            {
                // 天体表示させない
                if (Util.isNoDisp(planet)) return;

                if (planet.sensitive)
                {
                    //return;
                }

                PointF point;
                PointF pointDegree;
                int absolute_position = getNewAbsPosition(planet);
                int index = boxSet(absolute_position);

                point = CommonData.rotate(rcanvasVM.outerWidth / 4 + 55, 0, 5 * index - startdegree);
                point = getNewPoint(point);
                pointDegree = CommonData.rotate((rcanvasVM.outerWidth / 4) + 40, 0, 5 * index - startdegree );
                pointDegree = getNewPoint(pointDegree);
                pointDegree.X += 4;
                pointDegree.Y += 8;
                dispList.Add(planet.isDisp);
                string degreeTxt = Util.getPlanetDegreeTxt(planet);

                PlanetDisplay display = createPlanetDisplay(planet, getExp(planet), point, degreeTxt, pointDegree);
                pDisplayList.Add(display);

            });


            boxReset();

            list3.ForEach(planet =>
            {
                // 天体表示させない
                if (Util.isNoDisp(planet)) return;

                // これ何で入れたんだっけ
                if (planet.sensitive)
                {
                    //return;
                }


                PointF point;
                PointF pointDegree;
                int absolute_position = getNewAbsPosition(planet);
                int index = boxSet(absolute_position);

                point = CommonData.rotate(rcanvasVM.outerWidth / 3 + 45, 0, 5 * index - startdegree);
                point = getNewPoint(point);
                pointDegree = CommonData.rotate((rcanvasVM.outerWidth / 3) + 30, 0, 5 * index - startdegree );
                pointDegree = getNewPoint(pointDegree);
                pointDegree.X += 4;
                pointDegree.Y += 8;

                dispList.Add(planet.isDisp);
                string degreeTxt = Util.getPlanetDegreeTxt(planet);

                PlanetDisplay display = createPlanetDisplay(planet, getExp(planet), point, degreeTxt, pointDegree);
                pDisplayList.Add(display);

            });

            pDisplayList.ForEach(displayData =>
            {
                if (!displayData.isDisp)
                {
                    return;
                }
                SetOnlySignDegree(displayData);
            });

        }

        /// <summary>
        /// 天体から中心円への線
        /// </summary>
        /// <param name="startdegree"></param>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <param name="list3"></param>
        private void planetLine(double startdegree,
            List<PlanetData> list1,
            List<PlanetData> list2,
            List<PlanetData> list3
            )
        {
            List<bool> dispList = new List<bool>();
            List<PlanetDisplay> pDisplayList = new List<PlanetDisplay>();

            if (configData.dispPattern == EDispPetern.MINI)
            {
                //return;
            }

            if (tempSettings.bands == 1)
            {
                int[] box = new int[72];
                list1.ForEach(planet =>
                {
                    if (planet.isDisp == false)
                    {
                        return;
                    }
                    if (planet.no == 10000)
                    {
                        return;
                    }
                    if (planet.no == 10001)
                    {
                        return;
                    }
                    //Debug.WriteLine("{0} {1}", planet.no, planet.absolute_position);
                    // 重ならないようにずらしを入れる
                    // 1サインに6度単位5個までデータが入る
                    int index = (int)(planet.absolute_position / 5);
                    if (box[index] == 1)
                    {
                        while (box[index] == 1)
                        {
                            index++;
                            if (index == 72)
                            {
                                index = 0;
                            }
                        }
                        box[index] = 1;
                    }
                    else
                    {
                        box[index] = 1;
                    }

                    PointF pointPlanet;
                    PointF pointRing;
                    if (configData.dispPattern == EDispPetern.MINI)
                    {
                        pointPlanet = CommonData.rotate(rcanvasVM.outerWidth / 3, 0, 5 * index - startdegree + 3);
                    }
                    else
                    {
                        pointPlanet = CommonData.rotate(rcanvasVM.outerWidth / 3 - 45, 0, 5 * index - startdegree + 3);
                    }
                    pointRing = CommonData.rotate(tempSettings.zodiacCenter / 2, 0, planet.absolute_position - startdegree);

                    pointPlanet.X += (float)(rcanvasVM.outerWidth / 2);
                    pointPlanet.Y *= -1;
                    pointPlanet.Y += (float)(rcanvasVM.outerHeight / 2);
                    pointRing.X += (float)(rcanvasVM.outerWidth / 2);
                    pointRing.Y *= -1;
                    pointRing.Y += (float)(rcanvasVM.outerHeight / 2);

                    Line l = new Line();
                    l.X1 = pointPlanet.X;
                    l.Y1 = pointPlanet.Y;
                    l.X2 = pointRing.X;
                    l.Y2 = pointRing.Y;
                    l.Stroke = System.Windows.Media.Brushes.Gray;
                    l.StrokeThickness = 1.0;
                    ringCanvas.Children.Add(l);
                });
            }
        }
        private int boxSet(double absolute_position)
        {
            // 同じ位置に重なってしまうのでずらしを入れる
            // 1サインに6度単位5個までデータが入る
            int index = (int)(absolute_position / 5) % 72;
            if (box[index] == 1)
            {
                while (box[index] == 1)
                {
                    index++;
                    index = boxMax(index);
                }
                box[index] = 1;
            }
            else
            {
                box[index] = 1;
            }
            return index;
        }

        private void boxReset()
        {
            for (int i = 0; i < 72; i++)
            {
                box[i] = 0;
            }
        }

        private int boxMax(int index)
        {
            if (index == 72)
            {
                return 0;
            }
            return index;
        }



        private void houseCuspMouseEnter(object sender, System.EventArgs e)
        {
            Line l = (Line)sender;
            Explanation data = (Explanation)l.Tag;
            int degree = (int)(data.degree % 30);
            string minuteTxt = "";
            if (configData.decimalDisp == EDecimalDisp.DECIMAL)
            {
                // 小数点2桁を出す (% 1 * 100)
                minuteTxt = (data.degree % 1 * 100).ToString("00");
            }
            else
            {
                minuteTxt = CommonData.DecimalToHex(data.degree % 1 * 100).ToString("00") + "'";
            }
            mainWindow.mainWindowVM.explanationTxt = data.before + data.sign + " " + degree.ToString() + "." + minuteTxt;
        }

        private void planetMouseEnter(object sender, System.EventArgs e)
        {
            Label l;
            TextBlock t;
            Explanation data;
            if (sender is Label)
            {
                l = (Label)sender;
                data = (Explanation)l.Tag;
            }
            else if (sender is TextBlock)
            {
                t = (TextBlock)sender;
                data = (Explanation)t.Tag;
            }
            else
            {
                return;
            }
            string retro;
            if (data.retrograde)
            {
                retro = "(逆行)";
            }
            else
            {
                retro = "";
            }
            int degree = (int)(data.degree % 30);
            string minuteTxt = "";
            if (configData.decimalDisp == EDecimalDisp.DECIMAL)
            {
                // 小数点2桁を出す (% 1 * 100)
                minuteTxt = (data.degree % 1 * 100).ToString("00");
            }
            else
            {
                minuteTxt = CommonData.DecimalToHex(data.degree % 1 * 100).ToString("00") + "'";
            }
            mainWindow.mainWindowVM.explanationTxt = data.planet + retro + " " + data.sign + " " + degree.ToString() + "." + minuteTxt + " " + mainWindow.sabians[degree];
        }
        private void aspectMouseEnter(object sender, System.EventArgs e)
        {
            Line l = (Line)sender;
            AspectInfo info = (AspectInfo)l.Tag;
            mainWindow.mainWindowVM.explanationTxt = CommonData.getPlanetText(info.srcPlanetNo) + "-" +
                CommonData.getPlanetText(info.targetPlanetNo) + " " +
                info.aspectKind.ToString() + " " + info.aspectDegree.ToString("0.00") + "°";
        }
        public void explanationClear(object sender, System.EventArgs e)
        {
//            mainWindowVM.explanationTxt = "";
        }

        /// <summary>
        /// 絶対度数がマイナスなら補正する
        /// いるのかこれ
        /// </summary>
        /// <param name="planet"></param>
        /// <returns></returns>
        private int getNewAbsPosition(PlanetData planet)
        {
            int absolute_position = 0;
            if (planet.absolute_position < 0)
            {
                absolute_position = (int)planet.absolute_position + 360;
                if (absolute_position == 360)
                {
                    absolute_position = 0;
                }
            }
            else
            {
                absolute_position = (int)planet.absolute_position;
            }

            return absolute_position;
        }


        private PointF getNewPoint(PointF point)
        {
            point.X += (float)rcanvasVM.outerWidth / 2;
            point.X -= 8;

            point.Y *= -1;
            point.Y += (float)rcanvasVM.outerHeight / 2;
            point.Y -= 18;
            return point;
        }

        private Explanation getExp(PlanetData planet)
        {
            bool retrograde;
            if (planet.speed < 0)
            {
                retrograde = true;
            }
            else
            {
                retrograde = false;
            }

            return new Explanation()
            {
                degree = planet.absolute_position % 30,
                sign = CommonData.getSignTextJp(planet.absolute_position),
                planetNo = planet.no,
                planet = CommonData.getPlanetText(planet.no),
                retrograde = retrograde,

            };
        }

        /// <summary>
        /// PlanetDisplayインスタンスを作成
        /// </summary>
        /// <param name="planet"></param>
        /// <param name="exp"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private PlanetDisplay createPlanetDisplay(PlanetData planet, Explanation explanation, PointF point, string degreeTxt, PointF degreePt)
        {
            return new PlanetDisplay()
            {
                planetNo = planet.no,
                isDisp = planet.isDisp,
                explanation = explanation,
                planetPt = point,
                planetTxt = CommonData.getPlanetSymbol2(planet.no),
                planetColor = CommonData.getPlanetColor(planet.no),
                degreePt = degreePt,
                degreeTxt = degreeTxt
            };
        }

        /// <summary>
        /// 天体すべて表示
        /// </summary>
        /// <param name="displayData"></param>
        public void SetSign(PlanetDisplay displayData)
        {
            Label txtLbl = new Label();
            txtLbl.Content = displayData.planetTxt;
            txtLbl.Margin = new Thickness(displayData.planetPt.X, displayData.planetPt.Y, 0, 0);
            txtLbl.Foreground = displayData.planetColor;
            txtLbl.Tag = displayData.explanation;
            txtLbl.FontSize = 24;
            txtLbl.FontFamily = new System.Windows.Media.FontFamily("file:///" + Util.root() + @"\system\microcosm.otf#microcosm");
            txtLbl.MouseEnter += planetMouseEnter;
            ringCanvas.Children.Add(txtLbl);

            Label degreeLbl = new Label();
            degreeLbl.Content = displayData.degreeTxt;
            degreeLbl.Margin = new Thickness(displayData.degreePt.X, displayData.degreePt.Y, 0, 0);
            degreeLbl.Tag = displayData.explanation;
            degreeLbl.MouseEnter += planetMouseEnter;
            /*
            if (config.color29 == Config.Color29.CHANGE &&
                (
                    displayData.degreeTxt == "29°" ||
                    displayData.degreeTxt == "00°" ||
                    displayData.degreeTxt == "29" ||
                    displayData.degreeTxt == "00"
                    )
                )
            {
                degreeLbl.Foreground = new SolidColorBrush(Colors.Red);
                degreeLbl.FontWeight = FontWeights.Bold;
            }
            */
            ringCanvas.Children.Add(degreeLbl);

            Label signLbl = new Label();
            signLbl.FontFamily = new System.Windows.Media.FontFamily("file:///" + Util.root() + @"\system\microcosm.otf#microcosm");
            signLbl.Content = displayData.symbolTxt;
            signLbl.Margin = new Thickness(displayData.symbolPt.X, displayData.symbolPt.Y - 8, 0, 0);
            signLbl.Foreground = displayData.symbolColor;
            signLbl.FontSize = 24;
            signLbl.Tag = displayData.explanation;
            signLbl.MouseEnter += planetMouseEnter;
            ringCanvas.Children.Add(signLbl);

            Label minuteLbl = new Label();
            minuteLbl.Content = displayData.minuteTxt;
            minuteLbl.Margin = new Thickness(displayData.minutePt.X, displayData.minutePt.Y, 0, 0);
            minuteLbl.Tag = displayData.explanation;
            minuteLbl.MouseEnter += planetMouseEnter;
            ringCanvas.Children.Add(minuteLbl);

            Label retrogradeLbel = new Label();
            retrogradeLbel.Content = displayData.retrogradeTxt;
            retrogradeLbel.Margin = new Thickness(displayData.retrogradePt.X, displayData.retrogradePt.Y, 0, 0);
            retrogradeLbel.Tag = displayData.explanation;
            retrogradeLbel.MouseEnter += planetMouseEnter;

            /*
            ContextMenu context = new ContextMenu();
            MenuItem onlyAspectItem = new MenuItem { Header = "この天体のアスペクトだけ表示" };
            onlyAspectItem.Click += OnlyAspect_Click;
            context.Items.Add(onlyAspectItem);
            MenuItem fullAspectItem = new MenuItem { Header = "全ての天体のアスペクトだけ表示" };
            fullAspectItem.Click += FullAspect_Click;
            context.Items.Add(fullAspectItem);

            txtLbl.ContextMenu = context;
            */

            ringCanvas.Children.Add(retrogradeLbel);
        }

        /// <summary>
        /// 天体だけ表示
        /// </summary>
        /// <param name="displayData"></param>
        public void SetOnlySign(PlanetDisplay displayData)
        {
            Label txtLbl = new Label();
            txtLbl.Content = displayData.planetTxt;
            txtLbl.Margin = new Thickness(displayData.planetPt.X, displayData.planetPt.Y, 0, 0);
            txtLbl.Foreground = displayData.planetColor;
            txtLbl.Tag = displayData.explanation;
            txtLbl.FontSize = 24;
            txtLbl.FontFamily = new System.Windows.Media.FontFamily("file:///" + Util.root() + @"\system\microcosm.otf#microcosm");
            txtLbl.MouseEnter += planetMouseEnter;
            /*
            ContextMenu context = new ContextMenu();
            MenuItem newItem = new MenuItem { Header = "この天体のアスペクトだけ表示" };
            newItem.Click += OnlyAspect_Click;
            context.Items.Add(newItem);
            MenuItem fullAspectItem = new MenuItem { Header = "全ての天体のアスペクトだけ表示" };
            fullAspectItem.Click += FullAspect_Click;
            context.Items.Add(fullAspectItem);

            txtLbl.ContextMenu = context;
            */
            ringCanvas.Children.Add(txtLbl);
        }

        /// <summary>
        /// 天体と度数を表示
        /// </summary>
        /// <param name="displayData"></param>
        public void SetOnlySignDegree(PlanetDisplay displayData)
        {
            Label txtLbl = new Label();
            txtLbl.Content = displayData.planetTxt;
            txtLbl.Margin = new Thickness(displayData.planetPt.X, displayData.planetPt.Y, 0, 0);
            txtLbl.Foreground = displayData.planetColor;
            txtLbl.Tag = displayData.explanation;
            txtLbl.FontSize = 24;
            txtLbl.MouseEnter += planetMouseEnter;
            txtLbl.FontFamily = new System.Windows.Media.FontFamily("file:///" + Util.root() + @"\system\microcosm.otf#microcosm");
            ringCanvas.Children.Add(txtLbl);

            TextBlock degreeLbl = new TextBlock();
            degreeLbl.Text = displayData.degreeTxt;
            if (displayData.retrogradeTxt != null && displayData.retrogradeTxt != "")
            {
                degreeLbl.TextDecorations = TextDecorations.Underline;
            }
            /*
            if (config.color29 == Config.Color29.CHANGE &&
                    (
                    displayData.degreeTxt == "29°" ||
                    displayData.degreeTxt == "00°" ||
                    displayData.degreeTxt == "29" ||
                    displayData.degreeTxt == "00"
                    )
                )
            {
                degreeLbl.Foreground = new SolidColorBrush(Colors.Red);
                degreeLbl.FontWeight = FontWeights.Bold;
            }
            */

            degreeLbl.Margin = new Thickness(displayData.degreePt.X, displayData.degreePt.Y, 0, 0);
            degreeLbl.Tag = displayData.explanation;
            degreeLbl.MouseEnter += planetMouseEnter;

            /*
            ContextMenu context = new ContextMenu();
            MenuItem newItem = new MenuItem { Header = "この天体のアスペクトだけ表示" };
            newItem.Click += OnlyAspect_Click;
            context.Items.Add(newItem);
            MenuItem fullAspectItem = new MenuItem { Header = "全ての天体のアスペクトだけ表示" };
            fullAspectItem.Click += FullAspect_Click;
            context.Items.Add(fullAspectItem);

            txtLbl.ContextMenu = context;
            */

            ringCanvas.Children.Add(degreeLbl);
        }

        public enum EUseAspectList
        {
            Aspect = 1,
            SecondAspect = 2,
            ThirdAspect = 3,
        }
        // アスペクト表示
        public void aspectsRendering(
                double startDegree,
                Dictionary<int, PlanetData> list1,
                Dictionary<int, PlanetData> list2,
                Dictionary<int, PlanetData> list3
            )
        {
            if (mainWindow.aspect11disp)
            {
                aspectRender(startDegree, list1, 1, 1, EUseAspectList.Aspect);
            }
            if (mainWindow.tempSettings.bands > 1)
            {
                if (mainWindow.aspect22disp)
                {
                    aspectRender(startDegree, list2, 2, 2, EUseAspectList.Aspect);
                }
                if (mainWindow.aspect12disp)
                {
                    aspectRender(startDegree, list1, 1, 2, EUseAspectList.SecondAspect);
                }
            }
            if (mainWindow.tempSettings.bands > 2)
            {
                if (mainWindow.aspect13disp)
                {
                    aspectRender(startDegree, list1, 1, 3, EUseAspectList.ThirdAspect);
                }
                if (mainWindow.aspect23disp)
                {
                    aspectRender(startDegree, list2, 2, 3, EUseAspectList.ThirdAspect);
                }
                if (mainWindow.aspect33disp)
                {
                    aspectRender(startDegree, list3, 3, 3, EUseAspectList.Aspect);
                }
            }
        }

        /// <summary>
        /// aspect描画本体
        /// </summary>
        /// <param name="startDegree"></param>
        /// <param name="list"></param>
        /// <param name="startPosition">startのring番号</param>
        /// <param name="endPosition">endのring番号</param>
        /// <param name="useAspectList"></param>
        private void aspectRender(double startDegree, Dictionary<int, PlanetData> list,
            int startPosition, int endPosition,
            EUseAspectList useAspectList)
        {
            if (list == null)
            {
                return;
            }
            double startRingX = tempSettings.zodiacCenter / 2;
            double endRingX = tempSettings.zodiacCenter / 2;
            if (mainWindow.tempSettings.bands == 1)
            {
                // 一重円
                startRingX = tempSettings.zodiacCenter / 2;
                endRingX = tempSettings.zodiacCenter / 2;
            }
            else if (mainWindow.tempSettings.bands == 2)
            {
                // 二重円
                if (startPosition == 1)
                {
                    // 内側
                    startRingX = tempSettings.zodiacCenter / 2;
                }
                else
                {
                    // 外側
                    if (ringCanvas.ActualWidth > ringStack.ActualHeight)
                    {
                        // 横長
                        startRingX = (ringStack.ActualHeight + tempSettings.zodiacCenter - 90) / 4;
                    }
                    else
                    {
                        // 縦長
                        startRingX = (ringStack.ActualWidth + tempSettings.zodiacCenter - 90) / 4;
                    }
                }
                if (endPosition == 1)
                {
                    // 内側
                    endRingX = tempSettings.zodiacCenter / 2;
                }
                else
                {
                    // 外側
                    if (ringCanvas.ActualWidth > ringStack.ActualHeight)
                    {
                        // 横長
                        endRingX = (ringStack.ActualHeight + tempSettings.zodiacCenter - 90) / 4;
                    }
                    else
                    {
                        // 縦長
                        endRingX = (ringStack.ActualWidth + tempSettings.zodiacCenter - 90) / 4;
                    }
                }
            }
            else if (mainWindow.tempSettings.bands == 3)
            {
                // 三重円
                if (startPosition == 1)
                {
                    // 1
                    startRingX = tempSettings.zodiacCenter / 2;
                }
                else if (startPosition == 2)
                {
                    // 2
                    if (ringCanvas.ActualWidth > ringStack.ActualHeight)
                    {
                        // 横長
                        startRingX = (ringStack.ActualHeight + 2 * tempSettings.zodiacCenter - 90) / 6;
                    }
                    else
                    {
                        // 縦長
                        startRingX = (ringStack.ActualWidth + 2 * tempSettings.zodiacCenter - 90) / 6;
                    }
                }
                else
                {
                    // 3
                    if (ringCanvas.ActualWidth > ringStack.ActualHeight)
                    {
                        // 横長
                        startRingX = (2 * ringStack.ActualHeight + tempSettings.zodiacCenter - 180) / 6;
                    }
                    else
                    {
                        // 縦長
                        startRingX = (2 * ringStack.ActualWidth + tempSettings.zodiacCenter - 180) / 6;
                    }
                }
                if (endPosition == 1)
                {
                    // 1
                    endRingX = tempSettings.zodiacCenter / 2;
                }
                else if (endPosition == 2)
                {
                    // 2
                    if (ringCanvas.ActualWidth > ringStack.ActualHeight)
                    {
                        // 横長
                        endRingX = (ringStack.ActualHeight + 2 * tempSettings.zodiacCenter - 90) / 6;
                    }
                    else
                    {
                        // 縦長
                        endRingX = (ringStack.ActualWidth + 2 * tempSettings.zodiacCenter - 90) / 6;
                    }
                }
                else
                {
                    // 3
                    if (ringCanvas.ActualWidth > ringStack.ActualHeight)
                    {
                        // 横長
                        endRingX = (2 * ringStack.ActualHeight + tempSettings.zodiacCenter - 180) / 6;
                    }
                    else
                    {
                        // 縦長
                        endRingX = (2 * ringStack.ActualWidth + tempSettings.zodiacCenter - 180) / 6;
                    }
                }

            }

            List<PlanetData> newList = new List<PlanetData>();

            foreach (KeyValuePair<int, PlanetData> pair in list)
            {
                newList.Add(pair.Value);
            }

            for (int i = 0; i < newList.Count; i++)
            {
                if (!newList[i].isDisp)
                {
                    // 表示対象外
                    continue;
                }
                if (!newList[i].isAspectDisp)
                {
                    // 表示対象外
                    continue;
                }
                PointF startPoint;
                startPoint = CommonData.rotate(startRingX, 0, newList[i].absolute_position - startDegree);
                startPoint.X += (float)((rcanvasVM.outerWidth) / 2);
                startPoint.Y *= -1;
                startPoint.Y += (float)((rcanvasVM.outerHeight) / 2);
                if (useAspectList == EUseAspectList.Aspect)
                {
                    aspectListRender(startDegree, list, newList[i].aspects, startPoint, endRingX, endPosition);
                }
                else if (useAspectList == EUseAspectList.SecondAspect)
                {
                    aspectListRender(startDegree, list, newList[i].secondAspects, startPoint, endRingX, endPosition);
                }
                else if (useAspectList == EUseAspectList.ThirdAspect)
                {
                    aspectListRender(startDegree, list, newList[i].thirdAspects, startPoint, endRingX, endPosition);
                }
            }
        }

        /// <summary>
        /// アスペクト表示サブ関数
        /// </summary>
        /// <param name="startDegree"></param>
        /// <param name="list"></param>
        /// <param name="aspects"></param>
        /// <param name="startPoint"></param>
        /// <param name="endRingX"></param>
        /// <param name="endPosition"></param>
        private void aspectListRender(double startDegree, Dictionary<int, PlanetData> list, List<AspectInfo> aspects, PointF startPoint, double endRingX, int endPosition)
        {
            for (int j = 0; j < aspects.Count; j++)
            {
                if (!list[aspects[j].targetPlanetNo].isDisp)
                {
                    continue;
                }
                if (!list[aspects[j].targetPlanetNo].isAspectDisp)
                {
                    continue;
                }
                PointF endPoint;

                endPoint = CommonData.rotate(endRingX, 0, aspects[j].targetDegree - startDegree);
                endPoint.X += (float)((rcanvasVM.outerWidth) / 2);
                endPoint.Y *= -1;
                endPoint.Y += (float)((rcanvasVM.outerHeight) / 2);

                Line aspectLine = new Line()
                {
                    X1 = startPoint.X,
                    Y1 = startPoint.Y,
                    X2 = endPoint.X,
                    Y2 = endPoint.Y
                };
                if (aspects[j].softHard == SoftHard.SOFT)
                {
                    aspectLine.StrokeDashArray = new DoubleCollection();
                    aspectLine.StrokeDashArray.Add(4.0);
                    aspectLine.StrokeDashArray.Add(4.0);
                }
                TextBlock aspectLbl = new TextBlock();
                aspectLbl.FontFamily = new System.Windows.Media.FontFamily("file:///" + Util.root() + @"\system\microcosm-aspects.otf#microcosm-aspects");
                aspectLbl.FontSize = 20;
                aspectLbl.HorizontalAlignment = HorizontalAlignment.Left;
                aspectLbl.TextAlignment = TextAlignment.Left;
                aspectLbl.VerticalAlignment = VerticalAlignment.Top;
                aspectLbl.Margin = new Thickness(Math.Abs(startPoint.X + endPoint.X) / 2 - 5, Math.Abs(endPoint.Y + startPoint.Y) / 2 - 8, 0, 0);
                if (aspects[j].aspectKind == Aspect.AspectKind.CONJUNCTION)
                {
                    aspectLine.Stroke = System.Windows.Media.Brushes.Black;
                    // 描画しない
                }
                else if (aspects[j].aspectKind == Aspect.AspectKind.OPPOSITION)
                {
                    aspectLine.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                    //                    aspectLine.Stroke = System.Windows.Media.Brushes.Red;
                    aspectLbl.Foreground = System.Windows.Media.Brushes.IndianRed;
                    aspectLbl.Text = "B";
                }
                else if (aspects[j].aspectKind == Aspect.AspectKind.TRINE)
                {
                    aspectLine.Stroke = System.Windows.Media.Brushes.Orange;
                    aspectLbl.Foreground = System.Windows.Media.Brushes.Orange;
                    aspectLbl.Text = "C";
                }
                else if (aspects[j].aspectKind == Aspect.AspectKind.SQUARE)
                {
                    aspectLine.Stroke = System.Windows.Media.Brushes.Purple;
                    aspectLbl.Foreground = System.Windows.Media.Brushes.Purple;
                    aspectLbl.Text = "D";

                }
                else if (aspects[j].aspectKind == Aspect.AspectKind.SEXTILE)
                {
                    aspectLine.Stroke = System.Windows.Media.Brushes.Green;
                    aspectLbl.Foreground = System.Windows.Media.Brushes.Green;
                    aspectLbl.Text = "E";
                }
                else if (aspects[j].aspectKind == Aspect.AspectKind.SEMISQUARE)
                {
                    aspectLine.Stroke = System.Windows.Media.Brushes.LemonChiffon;
                    aspectLbl.Foreground = System.Windows.Media.Brushes.LemonChiffon;
                    aspectLbl.Text = "G";
                }
                else if (aspects[j].aspectKind == Aspect.AspectKind.SEMISEXTILE)
                {
                    aspectLine.Stroke = System.Windows.Media.Brushes.LemonChiffon;
                    aspectLbl.Foreground = System.Windows.Media.Brushes.LemonChiffon;
                    aspectLbl.Text = "F";
                }
                else if (aspects[j].aspectKind == Aspect.AspectKind.INCONJUNCT)
                {
                    aspectLine.Stroke = System.Windows.Media.Brushes.Aqua;
                    aspectLbl.Foreground = System.Windows.Media.Brushes.Aqua;
                    aspectLbl.Text = "H";
                }
                else if (aspects[j].aspectKind == Aspect.AspectKind.SESQUIQUADRATE)
                {
                    aspectLine.Stroke = System.Windows.Media.Brushes.Gray;
                    aspectLbl.Foreground = System.Windows.Media.Brushes.Gray;
                    aspectLbl.Text = "I";
                }
                else if (aspects[j].aspectKind == Aspect.AspectKind.QUINTILE)
                {
                    aspectLine.Stroke = System.Windows.Media.Brushes.Gray;
                    aspectLbl.Foreground = System.Windows.Media.Brushes.Gray;
                    aspectLbl.Text = "J";
                }
                else if (aspects[j].aspectKind == Aspect.AspectKind.BIQUINTILE)
                {
                    aspectLine.Stroke = System.Windows.Media.Brushes.Gray;
                    aspectLbl.Foreground = System.Windows.Media.Brushes.Gray;
                    aspectLbl.Text = "K";
                }
                else if (aspects[j].aspectKind == Aspect.AspectKind.SEMIQINTILE)
                {
                    aspectLine.Stroke = System.Windows.Media.Brushes.Gray;
                    aspectLbl.Foreground = System.Windows.Media.Brushes.Gray;
                    aspectLbl.Text = "L";
                }
                else if (aspects[j].aspectKind == Aspect.AspectKind.NOVILE)
                {
                    aspectLine.Stroke = System.Windows.Media.Brushes.Gray;
                    aspectLbl.Foreground = System.Windows.Media.Brushes.Gray;
                    aspectLbl.Text = "M";
                }
                else if (aspects[j].aspectKind == Aspect.AspectKind.SEPTILE)
                {
                    aspectLine.Stroke = System.Windows.Media.Brushes.Gray;
                    aspectLbl.Foreground = System.Windows.Media.Brushes.Gray;
                    aspectLbl.Text = "N";
                }
                else if (aspects[j].aspectKind == Aspect.AspectKind.QUINDECILE)
                {
                    aspectLine.Stroke = System.Windows.Media.Brushes.Gray;
                    aspectLbl.Foreground = System.Windows.Media.Brushes.Gray;
                    aspectLbl.Text = "O";
                }
                else
                {
                    aspectLine.Stroke = System.Windows.Media.Brushes.Black;
                    aspectLbl.Foreground = System.Windows.Media.Brushes.Black;
                    aspectLbl.Text = "";
                }
                aspectLbl.Tag = aspects[j];
                aspectLine.MouseEnter += aspectMouseEnter;
                aspectLine.Tag = aspects[j];
                ringCanvas.Children.Add(aspectLine);
                ringCanvas.Children.Add(aspectLbl);

            }

        }
    }
}
