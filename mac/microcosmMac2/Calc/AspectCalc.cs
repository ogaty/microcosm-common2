using System;
using microcosmMac2.Common;
using microcosmMac2.Config;
using microcosmMac2.Models;
using System.Collections.Generic;
using static Darwin.Message;
using System.Runtime.InteropServices.ComTypes;
using System.Diagnostics;

namespace microcosmMac2.Calc
{
    public class AspectCalc
    {
        public AspectCalc()
        {
        }

        // 同じリストのアスペクトを計算する
        public Dictionary<int, PlanetData> AspectCalcSame(SettingData a_setting, Dictionary<int, PlanetData> list)
        {
            List<PlanetData> calcList = new List<PlanetData>();

            foreach (KeyValuePair<int, PlanetData> pair in list)
            {
                calcList.Add(pair.Value);
            }

            // if (natal-natal)
            for (int i = 0; i < calcList.Count - 1; i++)
            {
                if (!calcList[i].isAspectDisp)
                {
                    continue;
                }
                for (int j = i + 1; j < calcList.Count; j++)
                {
                    if (!calcList[j].isAspectDisp)
                    {
                        continue;
                    }

                    // 90.0 と　300.0では210度ではなく150度にならなければいけない
                    // アスペクトは180°以上にはならない
                    double from;
                    double to;
                    if (calcList[i].absolute_position - calcList[j].absolute_position < 0)
                    {
                        to = calcList[j].absolute_position;
                        from = calcList[i].absolute_position;
                    }
                    else
                    {
                        to = calcList[i].absolute_position;
                        from = calcList[j].absolute_position;
                    }
                    double aspect_degree = to - from;
                    if (aspect_degree > 180)
                    {
                        aspect_degree = 360 + from - to;
                    }

                    foreach (AspectKind kind in Enum.GetValues(typeof(AspectKind)))
                    {
                        if (kind == AspectKind.CONJUNCTION && a_setting.dispAspectConjunction == 0) continue;
                        if (kind == AspectKind.OPPOSITION && a_setting.dispAspectOpposition == 0) continue;
                        if (kind == AspectKind.TRINE && a_setting.dispAspectTrine == 0) continue;
                        if (kind == AspectKind.SQUARE && a_setting.dispAspectSquare == 0) continue;
                        if (kind == AspectKind.SEXTILE && a_setting.dispAspectSextile == 0) continue;
                        if (kind == AspectKind.INCONJUNCT && a_setting.dispAspectInconjunct == 0) continue;
                        if (kind == AspectKind.SESQUIQUADRATE && a_setting.dispAspectSesquiQuadrate == 0) continue;
                        if (kind == AspectKind.SEMISEXTILE && a_setting.dispAspectSemiSextile == 0) continue;
                        if (kind == AspectKind.SEMISQUARE && a_setting.dispAspectSemiSquare == 0) continue;
                        if (kind == AspectKind.QUINTILE && a_setting.dispAspectQuintile == 0) continue;
                        if (kind == AspectKind.BIQUINTILE && a_setting.dispAspectBiQuintile == 0) continue;
                        if (kind == AspectKind.SEMIQINTILE && a_setting.dispAspectSemiQuintile == 0) continue;
                        if (kind == AspectKind.NOVILE && a_setting.dispAspectNovile == 0) continue;
                        if (kind == AspectKind.SEPTILE && a_setting.dispAspectSeptile == 0) continue;
                        if (kind == AspectKind.QUINDECILE && a_setting.dispAspectQuindecile == 0) continue;

                        double degree = getDegree(kind);

                        SoftHard sh = SoftHard.HARD;
                        bool isAspect = false;
                        if (calcList[i].no == CommonData.ZODIAC_SUN || calcList[i].no == CommonData.ZODIAC_MOON)
                        {
                            if (isFirstAspectKind(kind))
                            {
                                IsAspect(aspect_degree, degree, a_setting.orbSunMoon, ref isAspect, ref sh);
                            }
                            else 
                            {
                                // 2種:3種はsun/moon関係なし
                                IsAspect(aspect_degree, degree, a_setting.orb2nd, ref isAspect, ref sh);
                            }
                        }
                        else
                        {
                            if (isFirstAspectKind(kind))
                            {
                                IsAspect(aspect_degree, degree, a_setting.orb1st, ref isAspect, ref sh);
                            }
                            else
                            {
                                IsAspect(aspect_degree, degree, a_setting.orb2nd, ref isAspect, ref sh);
                            }
                        }
                        if (isAspect)
                        {
                            calcList[i].aspects.Add(new AspectInfo()
                            {
                                sourceDegree = calcList[i].absolute_position,
                                targetDegree = calcList[j].absolute_position,
                                aspectKind = kind,
                                softHard = sh,
                                srcPlanetNo = calcList[i].no,
                                targetPlanetNo = calcList[j].no,
                                aspectDegree = aspect_degree
                            });
                            break;
                        }
                    }

                }
            }

            Dictionary<int, PlanetData> ret = new Dictionary<int, PlanetData>();
            for (int i = 0; i < calcList.Count; i++)
            {
                ret[calcList[i].no] = calcList[i];
            }

            return ret;
        }

        /// <summary>
        /// 違うリストのアスペクトを計算する
        /// fromListにアスペクトを追加して返却
        /// listKindはpositionを決める
        /// </summary>
        /// <param name="a_setting"></param>
        /// <param name="fromList"></param>
        /// <param name="toList"></param>
        /// <param name="listKind">
        /// 0: from1 to1
        /// 1: from2 to2
        /// 2: from3 to3
        /// 3: from1 to2
        /// 4: from1 to3
        /// 5: from2 to3
        /// 6: from4 to4
        /// 7: from5 to5
        /// 8: from1 to4
        /// 9: from1 to5
        /// 10: from2 to4
        /// 11: from2 to5
        /// 12: from3 to4
        /// 13: from3 to5
        /// 14: from4 to5
        /// </param>
        /// <returns></returns>
        public Dictionary<int, PlanetData> AspectCalcOther(
            SettingData a_setting,
            Dictionary<int, PlanetData> fromList,
            Dictionary<int, PlanetData> toList,
            int listKind)
        {
            List<PlanetData> calcFromList = new List<PlanetData>();

            foreach (KeyValuePair<int, PlanetData> pair in fromList)
            {
                calcFromList.Add(pair.Value);
            }
            List<PlanetData> calcToList = new List<PlanetData>();

            foreach (KeyValuePair<int, PlanetData> pair in toList)
            {
                calcToList.Add(pair.Value);
            }

            for (int i = 0; i < calcFromList.Count - 1; i++)
            {
                if (!calcFromList[i].isAspectDisp)
                {
                    continue;
                }
                for (int j = 0; j < calcToList.Count - 1; j++)
                {
                    if (i == j) continue;
                    if (!calcToList[j].isAspectDisp)
                    {
                        continue;
                    }

                    // 90.0 と　300.0では210度ではなく150度にならなければいけない
                    double aspect_degree = calcFromList[i].absolute_position - calcToList[j].absolute_position;

                    if (aspect_degree > 180)
                    {
                        aspect_degree = calcFromList[i].absolute_position + 360 - calcToList[j].absolute_position;
                    }
                    if (aspect_degree < 0)
                    {
                        aspect_degree = Math.Abs(aspect_degree);
                    }


                    foreach (AspectKind kind in Enum.GetValues(typeof(AspectKind)))
                    {
                        if (kind == AspectKind.CONJUNCTION && a_setting.dispAspectConjunction == 0) continue;
                        if (kind == AspectKind.OPPOSITION && a_setting.dispAspectOpposition == 0) continue;
                        if (kind == AspectKind.TRINE && a_setting.dispAspectTrine == 0) continue;
                        if (kind == AspectKind.SQUARE && a_setting.dispAspectSquare == 0) continue;
                        if (kind == AspectKind.SEXTILE && a_setting.dispAspectSextile == 0) continue;
                        if (kind == AspectKind.INCONJUNCT && a_setting.dispAspectInconjunct == 0) continue;
                        if (kind == AspectKind.SESQUIQUADRATE && a_setting.dispAspectSesquiQuadrate == 0) continue;
                        if (kind == AspectKind.SEMISEXTILE && a_setting.dispAspectSemiSextile == 0) continue;
                        if (kind == AspectKind.SEMISQUARE && a_setting.dispAspectSemiSquare == 0) continue;
                        if (kind == AspectKind.QUINTILE && a_setting.dispAspectQuintile == 0) continue;
                        if (kind == AspectKind.BIQUINTILE && a_setting.dispAspectBiQuintile == 0) continue;
                        if (kind == AspectKind.SEMIQINTILE && a_setting.dispAspectSemiQuintile == 0) continue;
                        if (kind == AspectKind.NOVILE && a_setting.dispAspectNovile == 0) continue;
                        if (kind == AspectKind.SEPTILE && a_setting.dispAspectSeptile == 0) continue;
                        if (kind == AspectKind.QUINDECILE && a_setting.dispAspectQuindecile == 0) continue;

                        double degree = getDegree(kind);
                        SoftHard sh = SoftHard.HARD;
                        bool isAspect = false;

                        if (listKind == 3)
                        {
                            // 1-2
                            if (i == CommonData.ZODIAC_SUN || i == CommonData.ZODIAC_MOON)
                            {
                                if (isFirstAspectKind(kind))
                                {
                                    IsAspect(aspect_degree, degree, a_setting.orbSunMoon, ref isAspect, ref sh);
                                }
                                else
                                {
                                    IsAspect(aspect_degree, degree, a_setting.orb2nd, ref isAspect, ref sh);
                                }
                            }
                            else
                            {

                                if (isFirstAspectKind(kind))
                                {
                                    IsAspect(aspect_degree, degree, a_setting.orb1st, ref isAspect, ref sh);
                                }
                                else
                                {
                                    IsAspect(aspect_degree, degree, a_setting.orb2nd, ref isAspect, ref sh);
                                }
                            }
                            if (isAspect)
                            {
                                calcFromList[i].secondAspects.Add(new AspectInfo()
                                {
                                    sourceDegree = calcFromList[i].absolute_position,
                                    targetDegree = calcToList[j].absolute_position,
                                    aspectKind = kind,
                                    softHard = sh,
                                    targetPlanetNo = calcToList[j].no,
                                    srcPlanetNo = calcFromList[i].no,
                                    aspectDegree = aspect_degree
                                });
                                break;
                            }
                        }
                        else
                        {
                            // 今はこれで
                            // 1-3
                            // 2-3
                            if (i == CommonData.ZODIAC_SUN || i == CommonData.ZODIAC_MOON)
                            {
                                if (isFirstAspectKind(kind))
                                {
                                    IsAspect(aspect_degree, degree, a_setting.orbSunMoon, ref isAspect, ref sh);
                                }
                                else
                                {
                                    IsAspect(aspect_degree, degree, a_setting.orbSunMoon, ref isAspect, ref sh);
                                }
                            }
                            else
                            {
                                if (isFirstAspectKind(kind))
                                {
                                    IsAspect(aspect_degree, degree, a_setting.orb1st, ref isAspect, ref sh);
                                }
                                else
                                {
                                    IsAspect(aspect_degree, degree, a_setting.orb2nd, ref isAspect, ref sh);
                                }
                            }
                            if (isAspect)
                            {
                                calcFromList[i].thirdAspects.Add(new AspectInfo()
                                {
                                    sourceDegree = calcFromList[i].absolute_position,
                                    targetDegree = calcToList[j].absolute_position,
                                    aspectKind = kind,
                                    softHard = sh,
                                    targetPlanetNo = calcToList[j].no,
                                    srcPlanetNo = calcFromList[i].no,
                                    aspectDegree = aspect_degree
                                });
                                break;
                            }
                        }
                    }
                }
            }

            // calcListはList<T>、fromListはDictionary<int, T>
            for (int i = 0; i < calcFromList.Count; i++)
            {
                fromList[calcFromList[i].no] = calcFromList[i];
            }
            return fromList;
        }

        /// <summary>
        /// aspectKindから実際の度数を検索
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public double getDegree(AspectKind kind)
        {
            switch (kind)
            {
                case AspectKind.CONJUNCTION:
                    return 0.0;
                case AspectKind.OPPOSITION:
                    return 180.0;
                case AspectKind.TRINE:
                    return 120.0;
                case AspectKind.SQUARE:
                    return 90.0;
                case AspectKind.SEXTILE:
                    return 60.0;
                case AspectKind.INCONJUNCT:
                    return 150.0;
                case AspectKind.SESQUIQUADRATE:
                    return 135.0;
                case AspectKind.SEMISQUARE:
                    return 45.0;
                case AspectKind.SEMISEXTILE:
                    return 30.0;
                case AspectKind.QUINTILE:
                    return 72.0;
                case AspectKind.BIQUINTILE:
                    return 144.0;
                case AspectKind.SEMIQINTILE:
                    return 36.0;
                case AspectKind.SEPTILE:
                    return 360.0 / 7;
                case AspectKind.NOVILE:
                    return 40.0;
                case AspectKind.QUINDECILE:
                    return 165.0;
                default:
                    break;
            }

            return 0.0;
        }

        /// <summary>
        /// アスペクトしているか、soft/hardを返す
        /// </summary>
        /// <param name="aspectDegree">オーブ度数(90,180,120,...)</param>
        /// <param name="targetDegree">判定度数(94,107,174,...)</param>
        /// <param name="orb">オーブ度数</param>
        /// <param name="isAspect">(refs)アスペクトしていたらtrue</param>
        /// <param name="softHard">(refs)ソフトかハードかを返却</param>
        public void IsAspect(double aspectDegree, double targetDegree, double[] orb, ref bool isAspect, ref SoftHard softHard)
        {
            if (targetDegree < aspectDegree + orb[1] &&
                targetDegree > aspectDegree - orb[1])
            {
                isAspect = true;
                softHard = SoftHard.HARD;
            }
            else if (targetDegree < aspectDegree + orb[0] &&
                targetDegree > aspectDegree - orb[0])
            {
                isAspect = true;
                softHard = SoftHard.SOFT;
            }
        }

        /// <summary>
        /// 第1種ならtrue
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public bool isFirstAspectKind(AspectKind kind)
        {
            if (kind == AspectKind.CONJUNCTION ||
                kind == AspectKind.OPPOSITION ||
                kind == AspectKind.TRINE ||
                kind == AspectKind.SQUARE ||
                kind == AspectKind.SEXTILE) return true;
            return false;
        }
    }
}

