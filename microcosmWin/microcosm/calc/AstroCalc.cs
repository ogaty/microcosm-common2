using microcosm.config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwissEphNet;
using System.IO;
using microcosm.Planet;
using microcosm.Aspect;
using microcosm.common;
using System.Diagnostics;
using System.Windows.Media;
using microcosm.Db;

namespace microcosm.calc
{
    public class AstroCalc
    {
        //public MainWindow main;
        public ConfigData configData;
        public double year_days = 365.2424;
        public SwissEph s;
        public EclipseCalc? eclipse;
        public SettingData currentSetting;

        public const double SOLAR_YEAR = 365.2424;

        public AstroCalc(ConfigData configData, SettingData setting)
        {
            //this.main = main;
            this.configData = configData;
            this.currentSetting = setting;
            s = new SwissEph();
            // http://www.astro.com/ftp/swisseph/ephe/archive_zip/ からDL
            s.swe_set_ephe_path(configData.ephepath);
            s.OnLoadFile += (sender, ev) => {
                if (File.Exists(ev.FileName))
                    ev.File = new FileStream(ev.FileName, FileMode.Open);
            };
        }


        /// <summary>
        /// 天体の位置を計算する
        /// </summary>
        /// <param name="d">DateTimeオブジェクト</param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="houseKind"></param>
        /// <param name="subIndex">ring番号</param>
        /// <returns></returns>
        public Dictionary<int, PlanetData> PositionCalc(DateTime d, double lat, double lng, EHouseCalc houseKind, int subIndex)
        {
            Dictionary<int, PlanetData> planetdata = new Dictionary<int, PlanetData>(); ;

            // absolute position
            double[] x = { 0, 0, 0, 0, 0, 0 };
            string serr = "";

            int utc_year = 0;
            int utc_month = 0;
            int utc_day = 0;
            int utc_hour = 0;
            int utc_minute = 0;
            double utc_second = 0;

            // [0]:Ephemeris Time [1]:Universal Time
            double[] dret = { 0.0, 0.0 };

            int ii = 0;

            // utcに変換
            s.swe_utc_time_zone(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second, CommonData.GetTimezoneValue(configData.defaultTimezone), ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

            // 10天体ループ
            // 11(DH/TrueNode)、14(Earth)、15(Chiron)もついでに計算
            Enumerable.Range(0, 21).ToList().ForEach(i =>
            {
                int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
                if (configData.centric == ECentric.HELIO_CENTRIC)
                    flag |= SwissEph.SEFLG_HELCTR;

                if (configData.sidereal == Esidereal.SIDEREAL)
                {
                    flag |= SwissEph.SEFLG_SIDEREAL;
                    s.swe_set_sid_mode(SwissEph.SE_SIDM_LAHIRI, 0, 0);
                    // ayanamsa計算
                    double daya = 0.0;
                    double ut = s.swe_get_ayanamsa_ex_ut(dret[1], SwissEph.SEFLG_SWIEPH, out daya, ref serr);

                    // Ephemeris Timeで計算, 結果はxに入る
                    s.swe_calc_ut(dret[1], i, flag, x, ref serr);
                }
                else
                {
                    // Universal Timeで計算, 結果はxに入る
                    s.swe_calc_ut(dret[1], i, flag, x, ref serr);
                }

                //Debug.WriteLine("{0} {1}", i, x[0]);

                PlanetData p = new PlanetData()
                {
                    no = i,
                    absolute_position = x[0],
                    speed = x[3],
                    aspects = new List<AspectInfo>(),
                    secondAspects = new List<AspectInfo>(),
                    thirdAspects = new List<AspectInfo>(),
                    sensitive = false
                };
                if (i < 10 && subIndex >= 0)
                {
                    p.isDisp = currentSetting.GetDispPlanet(i);
                    p.isAspectDisp = currentSetting.GetDispAspectPlanet(i);
                }
                if (configData.centric == ECentric.HELIO_CENTRIC && i == CommonData.ZODIAC_SUN)
                {
                    // ヘリオセントリック太陽
                    p.isDisp = false;
                    p.isAspectDisp = false;
                }

                if (i == CommonData.ZODIAC_DH_MEANNODE)
                {
                    // MEAN NODE
                    p.sensitive = true;
                    if (configData.centric == ECentric.HELIO_CENTRIC)
                    {
                        p.isDisp = false;
                        p.isAspectDisp = false;
                    }
                    else if (configData.nodeCalc == ENodeCalc.MEAN)
                    {
                        p.isDisp = currentSetting.dispPlanetDH == 1;
                        p.isAspectDisp = currentSetting.dispAspectPlanetDH == 1;
                    }
                    else
                    {
                        p.isDisp = false;
                        p.isAspectDisp = false;
                    }
                }
                if (i == CommonData.ZODIAC_DH_TRUENODE && subIndex >= 0)
                {
                    // TRUE NODE ヘッド
                    p.sensitive = true;
                    if (configData.centric == ECentric.HELIO_CENTRIC)
                    {
                        p.isDisp = false;
                        p.isAspectDisp = false;
                    }
                    else if (configData.nodeCalc == ENodeCalc.TRUE)
                    {
                        p.isDisp = currentSetting.dispPlanetDH == 1;
                        p.isAspectDisp = currentSetting.dispAspectPlanetDH == 1;
                    }
                    else
                    {
                        p.isDisp = false;
                        p.isAspectDisp = false;
                    }
                }
                if (i == 12)
                {
                    // mean apogee、要はリリス
                    p.sensitive = true;
                    if (configData.centric == ECentric.HELIO_CENTRIC)
                    {
                        p.isDisp = false;
                        p.isAspectDisp = false;
                    }
                    else if (configData.lilithCalc == ELilithCalc.MEAN)
                    {
                        p.isDisp = currentSetting.dispPlanetLilith == 1;
                        p.isAspectDisp = currentSetting.dispAspectPlanetLilith == 1;
                    }
                    else
                    {
                        p.isDisp = false;
                        p.isAspectDisp = false;
                    }
                }
                if (i == 13 && subIndex >= 0)
                {
                    // oscu apogee、要はリリス
                    p.sensitive = true;
                    if (configData.centric == ECentric.HELIO_CENTRIC)
                    {
                        p.isDisp = false;
                        p.isAspectDisp = false;
                    }
                    else if (configData.lilithCalc == ELilithCalc.OSCU)
                    {
                        p.isDisp = currentSetting.dispPlanetLilith == 1;
                        p.isAspectDisp = currentSetting.dispAspectPlanetLilith == 1;
                    }
                    else
                    {
                        p.isDisp = false;
                        p.isAspectDisp = false;
                    }
                }
                if (i == CommonData.ZODIAC_EARTH && subIndex >= 0)
                {
                    // ヘリオセントリック地球
                    if (configData.centric == ECentric.GEO_CENTRIC)
                    {
                        p.isDisp = false;
                        p.isAspectDisp = false;
                    }
                    else
                    {
                        p.isDisp = currentSetting.GetDispPlanet(i);
                        p.isAspectDisp = currentSetting.GetDispAspectPlanet(i);
                    }
                }
                if (i == CommonData.ZODIAC_CHIRON && subIndex >= 0)
                {
                    // chiron
                    p.isDisp = currentSetting.GetDispPlanet(i);
                    p.isAspectDisp = currentSetting.GetDispAspectPlanet(i);
                }
                if (i == CommonData.ZODIAC_POLUS)
                {
                    p.isDisp = false;
                    p.isAspectDisp = false;
                }
                if (i >= CommonData.ZODIAC_CERES && subIndex >= 0)
                {
                    // chiron
                    p.isDisp = currentSetting.GetDispPlanet(i);
                    p.isAspectDisp = currentSetting.GetDispAspectPlanet(i);
                }
                ii = i;
                planetdata[i] = p;
            });
            PlanetData dtMean = new PlanetData()
            {
                no = CommonData.ZODIAC_DT_MEAN,
                absolute_position = (planetdata[CommonData.ZODIAC_DH_MEANNODE].absolute_position + 180.0) % 360,
                speed = planetdata[CommonData.ZODIAC_DH_MEANNODE].speed,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = true
            };

            if (configData.centric == ECentric.HELIO_CENTRIC)
            {
                dtMean.isDisp = false;
                dtMean.isAspectDisp = false;
            }
            else if (configData.nodeCalc == ENodeCalc.TRUE)
            {
                dtMean.isDisp = false;
                dtMean.isAspectDisp = false;
            }
            else
            {
                dtMean.isDisp = currentSetting.GetDispPlanet(CommonData.ZODIAC_DT_MEAN);
                dtMean.isAspectDisp = currentSetting.GetDispAspectPlanet(CommonData.ZODIAC_DT_MEAN);
            }

            planetdata[CommonData.ZODIAC_DT_MEAN] = dtMean;

            PlanetData dtTrue = new PlanetData()
            {
                no = CommonData.ZODIAC_DT_TRUE,
                absolute_position = (planetdata[CommonData.ZODIAC_DH_TRUENODE].absolute_position + 180.0) % 360,
                speed = planetdata[CommonData.ZODIAC_DH_TRUENODE].speed,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = true
            };

            if (configData.centric == ECentric.HELIO_CENTRIC)
            {
                dtTrue.isDisp = false;
                dtTrue.isAspectDisp = false;
            }
            else if (configData.nodeCalc == ENodeCalc.MEAN)
            {
                dtTrue.isDisp = false;
                dtTrue.isAspectDisp = false;
            }
            else
            {
                dtTrue.isDisp = currentSetting.GetDispPlanet(CommonData.ZODIAC_DT_MEAN);
                dtTrue.isAspectDisp = currentSetting.GetDispAspectPlanet(CommonData.ZODIAC_DT_MEAN);
            }

            planetdata[CommonData.ZODIAC_DT_TRUE] = dtTrue;

            s.swe_close();
            // ハウスを後ろにくっつける
            //double[] houses = CuspCalc(d, timezone, lat, lng, houseKind);
            //planetdata = setHouse(planetdata, houses, main.currentSetting, subIndex);

            return planetdata;
        }

        public PlanetData PositionCalcSingle(int no, double timezone, DateTime d)
        {
            // absolute position
            double[] x = { 0, 0, 0, 0, 0, 0 };
            string serr = "";

            int utc_year = 0;
            int utc_month = 0;
            int utc_day = 0;
            int utc_hour = 0;
            int utc_minute = 0;
            double utc_second = 0;

            // [0]:Ephemeris Time [1]:Universal Time
            double[] dret = { 0.0, 0.0 };

            // utcに変換
            s.swe_utc_time_zone(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second, timezone, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);


            int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
            if (configData.centric == ECentric.HELIO_CENTRIC)
                flag |= SwissEph.SEFLG_HELCTR;
            if (configData.sidereal == Esidereal.SIDEREAL)
            {
                flag |= SwissEph.SEFLG_SIDEREAL;
                s.swe_set_sid_mode(SwissEph.SE_SIDM_LAHIRI, 0, 0);
                // ayanamsa計算
                double daya = 0.0;
                double ut = s.swe_get_ayanamsa_ex_ut(dret[1], SwissEph.SEFLG_SWIEPH, out daya, ref serr);

                // Ephemeris Timeで計算, 結果はxに入る
                s.swe_calc_ut(dret[1], no, flag, x, ref serr);
            }
            else
            {
                // Universal Timeで計算, 結果はxに入る
                s.swe_calc_ut(dret[1], no, flag, x, ref serr);
            }
            s.swe_close();


            PlanetData planetData = new PlanetData();
            planetData.no = no;
            planetData.absolute_position = x[0];

            return planetData;
        }

        /// <summary>
        /// コンポジットチャート
        /// </summary>
        /// <param name="natal1"></param>
        /// <param name="natal2"></param>
        /// <returns></returns>
        public Dictionary<int, PlanetData> PositionCalcComposit(Dictionary<int, PlanetData> natal1, Dictionary<int, PlanetData> natal2)
        {
            Dictionary<int, PlanetData> ret = new Dictionary<int, PlanetData>();
            foreach (KeyValuePair<int, PlanetData> value in natal1)
            {
                if (natal2.ContainsKey(value.Key))
                {
                    double newPosition = (value.Value.absolute_position + natal2[value.Key].absolute_position) / 2;
                    ret.Add(value.Key, new PlanetData()
                    {
                        no = value.Value.no,
                        absolute_position= newPosition,
                        isDisp = value.Value.isDisp,
                        aspects = new List<AspectInfo>(),
                        isAspectDisp = value.Value.isAspectDisp,
                        secondAspects = new List<AspectInfo>(),
                        thirdAspects = new List<AspectInfo>(),
                        sensitive = value.Value.sensitive,
                        speed = value.Value.speed,
                    });
                }
            }

            return ret;
        }

        /// <summary>
        /// unittest用
        /// </summary>
        /// <param name="subIndex">targetNoList決定に使用</param>
        /// <returns></returns>
        public Dictionary<int, PlanetData> setHouse(Dictionary<int, PlanetData> pdata, double[] houses, SettingData setting, int subIndex)
        {
            PlanetData pAsc = new PlanetData()
            {
                absolute_position = houses[1],
                no = CommonData.ZODIAC_ASC,
                sensitive = true,
                speed = 1,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                isDisp = currentSetting.dispPlanetAsc == 1,
                isAspectDisp = currentSetting.dispAspectPlanetAsc == 1,
            };

            pdata[CommonData.ZODIAC_ASC] = pAsc;


            PlanetData pMc = new PlanetData()
            {
                absolute_position = houses[10],
                no = CommonData.ZODIAC_MC,
                sensitive = true,
                speed = 1,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                isDisp = currentSetting.dispPlanetMc == 1,
                isAspectDisp = currentSetting.dispAspectPlanetMc == 1,
            };

            pdata[CommonData.ZODIAC_MC] = pMc;
            return pdata;
        }

        // カスプを計算
        public double[] CuspCalc(DateTime d, double timezone, double lat, double lng, EHouseCalc houseKind)
        {
            int utc_year = 0;
            int utc_month = 0;
            int utc_day = 0;
            int utc_hour = 0;
            int utc_minute = 0;
            double utc_second = 0;
            string serr = "";
            // [0]:Ephemeris Time [1]:Universal Time
            double[] dret = { 0.0, 0.0 };

            SwissEph s = new SwissEph();

            // utcに変換
            s.swe_utc_time_zone(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second, timezone, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

            double[] cusps = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] ascmc = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            if (houseKind == EHouseCalc.PLACIDUS)
            {
                // Placidas
                s.swe_houses(dret[1], lat, lng, 'P', cusps, ascmc);
            }
            else if (houseKind == EHouseCalc.KOCH)
            {
                // Koch
                s.swe_houses(dret[1], lat, lng, 'K', cusps, ascmc);
            }
            else if (houseKind == EHouseCalc.CAMPANUS)
            {
                // Campanus
                s.swe_houses(dret[1], lat, lng, 'C', cusps, ascmc);
            }
            else if (houseKind == EHouseCalc.EQUAL)
            {
                // Equal
                s.swe_houses(dret[1], lat, lng, 'E', cusps, ascmc);
            }
            else
            {
                // Zero Aries
                s.swe_houses(dret[1], lat, lng, 'N', cusps, ascmc);
            }
            s.swe_close();

            return cusps;
        }

        void primaryProgression(DateTime natalTime, DateTime transitTime)
        {
            double angle = (Julian.ToJulianDate(transitTime) - Julian.ToJulianDate(natalTime)) / SOLAR_YEAR;

        }


        /// <summary>
        /// 一度一年法
        /// 一度ずらすだけなので再計算不要
        /// </summary>
        /// <param name="natallist"></param>
        /// <param name="natalTime"></param>
        /// <param name="transitTime"></param>
        /// <returns></returns>
        public Dictionary<int, PlanetData> PrimaryProgressionCalc(Dictionary<int, PlanetData> natallist, DateTime natalTime, DateTime transitTime)
        {
            Dictionary<int, PlanetData> progresslist = new Dictionary<int, PlanetData>();
            double angle = (Julian.ToJulianDate(transitTime) - Julian.ToJulianDate(natalTime)) / SOLAR_YEAR;

            foreach (KeyValuePair<int, PlanetData> pair in natallist)
            {
                PlanetData progressdata = new PlanetData()
                {
                    absolute_position = pair.Value.absolute_position,
                    no = pair.Key,
                    sensitive = pair.Value.sensitive,
                    speed = pair.Value.speed,
                    aspects = new List<AspectInfo>(),
                    secondAspects = new List<AspectInfo>(),
                    thirdAspects = new List<AspectInfo>(),
                    isDisp = pair.Value.isDisp,
                    isAspectDisp = pair.Value.isAspectDisp
                };

                progressdata.absolute_position += angle;
                progressdata.absolute_position %= 360;
                progresslist[pair.Key] = progressdata;
            }

            return progresslist;
        }

        /// <summary>
        /// 一度一年法のハウス、やることはサインと同じ
        /// </summary>
        /// <param name="houseList"></param>
        /// <param name="natalTime"></param>
        /// <param name="transitTime"></param>
        /// <returns></returns>
        public double[] PrimaryProgressionHouseCalc(double[] houseList, DateTime natalTime, DateTime transitTime)
        {
            double[] retHouse = new double[13];
            houseList.CopyTo(retHouse, 0);

            double angle = (Julian.ToJulianDate(transitTime) - Julian.ToJulianDate(natalTime)) / SOLAR_YEAR;

            for (int i = 0; i < houseList.Count(); i++)
            {
                retHouse[i] += angle;
            }

            return retHouse;
        }


        /// <summary>
        /// ソーラーアーク法
        /// 一日一年法で太陽の動きを見てそれをすべてに加算
        /// </summary>
        /// <param name="natallist"></param>
        /// <param name="natalTime"></param>
        /// <param name="transitTime"></param>
        /// <returns></returns>
        public Dictionary<int, PlanetData> SolarArcCalc(Dictionary<int, PlanetData> natallist, DateTime natalTime, DateTime transitTime, double timezone)
        {
            Dictionary<int, PlanetData> progresslist = new Dictionary<int, PlanetData>();

            // 現在の太陽の度数
            double natalDegree = natallist[0].absolute_position;
            // 計算後の太陽の度数
            double calcDegree = getBodyBySecondaryProgression(natalTime, transitTime, 0, timezone);
            // 加算する度数
            double addDegree = calcDegree - natalDegree;

            foreach (KeyValuePair<int, PlanetData> pair in natallist)
            {

                PlanetData progressdata = new PlanetData()
                {
                    absolute_position = pair.Value.absolute_position,
                    no = pair.Key,
                    sensitive = pair.Value.sensitive,
                    speed = pair.Value.speed,
                    aspects = new List<AspectInfo>(),
                    secondAspects = new List<AspectInfo>(),
                    thirdAspects = new List<AspectInfo>(),
                    isDisp = pair.Value.isDisp,
                    isAspectDisp = pair.Value.isAspectDisp
                };

                progressdata.absolute_position += addDegree;
                progressdata.absolute_position %= 360;
                progresslist[pair.Key] = progressdata;
            }

            return progresslist;
        }

        /// <summary>
        /// ソーラーアークのハウス、やることはサインと同じなんだけど引数が違う
        /// </summary>
        /// <param name="absolute_position"></param>
        /// <param name="houseList"></param>
        /// <param name="natalTime"></param>
        /// <param name="transitTime"></param>
        /// <returns></returns>
        public double[] SolarArcHouseCalc(double absolute_position, double[] houseList, DateTime natalTime, DateTime transitTime, double timezone)
        {
            double[] retHouse = new double[13];
            houseList.CopyTo(retHouse, 0);

            // 現在の太陽の度数
            double natalDegree = absolute_position;
            // 計算後の太陽の度数
            double calcDegree = getBodyBySecondaryProgression(natalTime, transitTime, 0, timezone);
            // 加算する度数
            double addDegree = calcDegree - natalDegree;

            for (int i = 0; i < houseList.Count(); i++)
            {
                retHouse[i] += addDegree;
            }

            return retHouse;
        }

        /// <summary>
        /// 一日一年法で日数を再計算
        /// </summary>
        /// <param name="natalTime"></param>
        /// <param name="transitTime"></param>
        /// <param name="planetId"></param>
        /// <returns></returns>
        public double getBodyBySecondaryProgression(DateTime natalTime, DateTime transitTime, int planetId, double timezone)
        {
            double natalJDay = Julian.ToJulianDate(natalTime);

            double dayOffset = (Julian.ToJulianDate(transitTime) - Julian.ToJulianDate(natalTime)) / SOLAR_YEAR;


            DateTime progresTime = natalTime;
            progresTime = progresTime.AddDays(dayOffset);

            //double jd = Julian.ToJulianDate(progresTime);
            //double ET = jd + s.swe_deltat(jd);

            // [0]:Ephemeris Time [1]:Universal Time
            double[] dret = { 0.0, 0.0 };
            // absolute position
            double[] x = { 0, 0, 0, 0, 0, 0 };
            string serr = "";

            int utc_year = 0;
            int utc_month = 0;
            int utc_day = 0;
            int utc_hour = 0;
            int utc_minute = 0;
            double utc_second = 0;

            // utcに変換
            s.swe_utc_time_zone(progresTime.Year, progresTime.Month, progresTime.Day, progresTime.Hour, progresTime.Minute, progresTime.Second, timezone, 
                ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

            int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
            if (configData.centric == ECentric.HELIO_CENTRIC)
                flag |= SwissEph.SEFLG_HELCTR;

            if (configData.sidereal == Esidereal.SIDEREAL)
            {
                flag |= SwissEph.SEFLG_SIDEREAL;
                s.swe_set_sid_mode(SwissEph.SE_SIDM_LAHIRI, 0, 0);
                // ayanamsa計算
                double daya = 0.0;
                double ut = s.swe_get_ayanamsa_ex_ut(dret[1], SwissEph.SEFLG_SWIEPH, out daya, ref serr);

                // Ephemeris Timeで計算, 結果はxに入る
                s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
            }
            else
            {
                // Universal Timeで計算, 結果はxに入る
                s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
            }
            s.swe_close();

            return x[0];
        }

        /// <summary>
        /// 一日一年法
        /// 365で割った結果が日数か度数かの違い
        /// 日数を足してswe_calcやり直す
        /// </summary>
        /// <param name="natallist"></param>
        /// <param name="natalTime"></param>
        /// <param name="transitTime"></param>
        /// <returns></returns>
        public Dictionary<int, PlanetData> SecondaryProgressionCalc(Dictionary<int, PlanetData> natallist, DateTime natalTime, DateTime transitTime, double timezone, double lat, double lng)
        {
            Dictionary<int, PlanetData> progresslist = new Dictionary<int, PlanetData>();
            double dayOffset = (Julian.ToJulianDate(transitTime) - Julian.ToJulianDate(natalTime)) / SOLAR_YEAR;

            // 日数を秒数に変換する
            int seconds = (int)(dayOffset * 86400);

            TimeSpan add = TimeSpan.FromSeconds(seconds);

            DateTime newTime = natalTime + add;

            // absolute position
            double[] x = { 0, 0, 0, 0, 0, 0 };
            string serr = "";

            int utc_year = 0;
            int utc_month = 0;
            int utc_day = 0;
            int utc_hour = 0;
            int utc_minute = 0;
            double utc_second = 0;

            // [0]:Ephemeris Time [1]:Universal Time
            double[] dret = { 0.0, 0.0 };

            // utcに変換
            s.swe_utc_time_zone(newTime.Year, newTime.Month, newTime.Day, newTime.Hour, newTime.Minute, newTime.Second, timezone, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

            foreach (KeyValuePair<int, PlanetData> pair in natallist)
            {
                PlanetData progressdata;
                if (pair.Key == CommonData.ZODIAC_ASC)
                {
                    // swe_calcでは計算不可
                    // houseListから再度入れ直す
                    progressdata = new PlanetData()
                    {
                        absolute_position = 0,
                        no = pair.Key,
                        sensitive = pair.Value.sensitive,
                        speed = pair.Value.speed,
                        aspects = new List<AspectInfo>(),
                        secondAspects = new List<AspectInfo>(),
                        thirdAspects = new List<AspectInfo>(),
                        isDisp = pair.Value.isDisp,
                        isAspectDisp = pair.Value.isAspectDisp
                    };
                }
                else if (pair.Key == CommonData.ZODIAC_MC)
                {
                    // swe_calcでは計算不可
                    // houseListから再度入れ直す
                    progressdata = new PlanetData()
                    {
                        absolute_position = 0,
                        no = pair.Key,
                        sensitive = pair.Value.sensitive,
                        speed = pair.Value.speed,
                        aspects = new List<AspectInfo>(),
                        secondAspects = new List<AspectInfo>(),
                        thirdAspects = new List<AspectInfo>(),
                        isDisp = pair.Value.isDisp,
                        isAspectDisp = pair.Value.isAspectDisp
                    };
                }
                else if (pair.Key == CommonData.ZODIAC_DT_MEAN)
                {
                    // swe_calcでは計算不可
                    // DHから計算
                    progressdata = new PlanetData()
                    {
                        absolute_position = (progresslist[CommonData.ZODIAC_DH_MEANNODE].absolute_position + 180) % 360,
                        no = pair.Key,
                        sensitive = pair.Value.sensitive,
                        speed = pair.Value.speed,
                        aspects = new List<AspectInfo>(),
                        secondAspects = new List<AspectInfo>(),
                        thirdAspects = new List<AspectInfo>(),
                        isDisp = pair.Value.isDisp,
                        isAspectDisp = pair.Value.isAspectDisp
                    };
                }
                else if (pair.Key == CommonData.ZODIAC_DT_TRUE)
                {
                    // swe_calcでは計算不可
                    // DHから計算
                    progressdata = new PlanetData()
                    {
                        absolute_position = (progresslist[CommonData.ZODIAC_DH_TRUENODE].absolute_position + 180) % 360,
                        no = pair.Key,
                        sensitive = pair.Value.sensitive,
                        speed = pair.Value.speed,
                        aspects = new List<AspectInfo>(),
                        secondAspects = new List<AspectInfo>(),
                        thirdAspects = new List<AspectInfo>(),
                        isDisp = pair.Value.isDisp,
                        isAspectDisp = pair.Value.isAspectDisp
                    };
                }
                else
                {
                    int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
                    if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;
                    if (configData.sidereal == Esidereal.SIDEREAL)
                    {
                        flag |= SwissEph.SEFLG_SIDEREAL;
                        s.swe_set_sid_mode(SwissEph.SE_SIDM_LAHIRI, 0, 0);
                        // ayanamsa計算
                        double daya = 0.0;
                        double ut = s.swe_get_ayanamsa_ex_ut(dret[1], SwissEph.SEFLG_SWIEPH, out daya, ref serr);

                        // Ephemeris Timeで計算, 結果はxに入る
                        s.swe_calc_ut(dret[1], pair.Key, flag, x, ref serr);
                        // tropicalからayanamsaをマイナス
                        x[0] = x[0] > daya ? x[0] - daya : x[0] - daya + 360;
                    }
                    else
                    {
                        // Universal Timeで計算, 結果はxに入る
                        s.swe_calc_ut(dret[1], pair.Key, flag, x, ref serr);
                    }
                    s.swe_close();

                    progressdata = new PlanetData()
                    {
                        absolute_position = x[0],
                        no = pair.Key,
                        sensitive = pair.Value.sensitive,
                        speed = pair.Value.speed,
                        aspects = new List<AspectInfo>(),
                        secondAspects = new List<AspectInfo>(),
                        thirdAspects = new List<AspectInfo>(),
                        isDisp = pair.Value.isDisp,
                        isAspectDisp = pair.Value.isAspectDisp
                    };
                }

                progresslist[pair.Key] = progressdata;
            }
            // ハウスを後ろにくっつける
            double[] houses = CuspCalc(newTime, timezone, lat, lng, configData.houseCalc);
            progresslist = setHouse(progresslist, houses, currentSetting, 1);

            return progresslist;
        }

        /// <summary>
        /// 一日一年法のハウス
        /// </summary>
        /// <param name="houseList"></param>
        /// <param name="natallist"></param>
        /// <param name="natalTime"></param>
        /// <param name="transitTime"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="timezone"></param>
        /// <returns></returns>
        public double[] SecondaryProgressionHouseCalc(double[] houseList, Dictionary<int, PlanetData> natallist, DateTime natalTime, DateTime transitTime, double lat, double lng, double timezone)
        {
            List<PlanetData> progresslist = new List<PlanetData>();
            TimeSpan ts = transitTime - natalTime;
            double years = ts.TotalDays / year_days;

            // 日付を秒数に変換する
            int seconds = (int)(years * 86400);

            TimeSpan add = TimeSpan.FromSeconds(seconds);

            DateTime newTime = natalTime + add;


            double[] retHouse = CuspCalc(newTime, timezone, lat, lng, configData.houseCalc);

            return retHouse;
        }

        /// <summary>
        /// 松村潔法とかCPSというやつ
        /// 月、水星、金星、太陽は一日一年法
        /// 火星以降および感受点は一度一年法
        /// </summary>
        /// <param name="natallist"></param>
        /// <param name="natalTime"></param>
        /// <param name="transitTime"></param>
        /// <returns></returns>
        public Dictionary<int, PlanetData> CompositProgressionCalc(Dictionary<int, PlanetData> natallist, DateTime natalTime, DateTime transitTime, double timezone)
        {
            Dictionary<int, PlanetData> progresslist = new Dictionary<int, PlanetData>();
            double dayOffset = (Julian.ToJulianDate(transitTime) - Julian.ToJulianDate(natalTime)) / SOLAR_YEAR;
            double angle = (Julian.ToJulianDate(transitTime) - Julian.ToJulianDate(natalTime)) / SOLAR_YEAR;

            // 日数を秒数に変換する
            int seconds = (int)(dayOffset * 86400);

            TimeSpan add = TimeSpan.FromSeconds(seconds);

            DateTime newTime = natalTime + add;

            // absolute position
            double[] x = { 0, 0, 0, 0, 0, 0 };
            string serr = "";

            int utc_year = 0;
            int utc_month = 0;
            int utc_day = 0;
            int utc_hour = 0;
            int utc_minute = 0;
            double utc_second = 0;

            // [0]:Ephemeris Time [1]:Universal Time
            double[] dret = { 0.0, 0.0 };

            // utcに変換
            s.swe_utc_time_zone(newTime.Year, newTime.Month, newTime.Day, newTime.Hour, newTime.Minute, newTime.Second, timezone, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

            foreach (KeyValuePair<int, PlanetData> pair in natallist)
            {
                PlanetData progressdata;
                if ((pair.Key != CommonData.ZODIAC_MOON) &&
                    (pair.Key != CommonData.ZODIAC_MERCURY) &&
                    (pair.Key != CommonData.ZODIAC_VENUS) &&
                    (pair.Key != CommonData.ZODIAC_SUN))
                {
                    progressdata = new PlanetData()
                    {
                        absolute_position = pair.Value.absolute_position,
                        no = pair.Key,
                        sensitive = pair.Value.sensitive,
                        speed = pair.Value.speed,
                        aspects = new List<AspectInfo>(),
                        secondAspects = new List<AspectInfo>(),
                        thirdAspects = new List<AspectInfo>(),
                        isDisp = pair.Value.isDisp,
                        isAspectDisp = pair.Value.isAspectDisp
                    };
                    progressdata.absolute_position += angle;
                    progressdata.absolute_position %= 360;
                    progresslist[pair.Key] = progressdata;
                    continue;
                }
                int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
                if (configData.centric == ECentric.HELIO_CENTRIC)
                    flag |= SwissEph.SEFLG_HELCTR;
                if (configData.sidereal == Esidereal.SIDEREAL)
                {
                    flag |= SwissEph.SEFLG_SIDEREAL;
                    s.swe_set_sid_mode(SwissEph.SE_SIDM_LAHIRI, 0, 0);
                    // ayanamsa計算
                    double daya = 0.0;
                    double ut = s.swe_get_ayanamsa_ex_ut(dret[1], SwissEph.SEFLG_SWIEPH, out daya, ref serr);

                    // Ephemeris Timeで計算, 結果はxに入る
                    s.swe_calc_ut(dret[1], pair.Key, flag, x, ref serr);
                    // tropicalからayanamsaをマイナス
                    x[0] = x[0] > daya ? x[0] - daya : x[0] - daya + 360;
                }
                else
                {
                    // Universal Timeで計算, 結果はxに入る
                    s.swe_calc_ut(dret[1], pair.Key, flag, x, ref serr);
                }
                s.swe_close();

                progressdata = new PlanetData()
                {
                    absolute_position = x[0],
                    no = pair.Key,
                    sensitive = pair.Value.sensitive,
                    speed = pair.Value.speed,
                    aspects = new List<AspectInfo>(),
                    secondAspects = new List<AspectInfo>(),
                    thirdAspects = new List<AspectInfo>(),
                    isDisp = pair.Value.isDisp,
                    isAspectDisp = pair.Value.isAspectDisp
                };
                progresslist[pair.Key] = progressdata;
            }



            return progresslist;
        }
        public double[] CompositProgressionHouseCalc(double[] houseList, Dictionary<int, PlanetData> natallist, DateTime natalTime, DateTime transitTime, double lat, double lng, double timezone)
        {
            // AMATERU、SG共にSecondaryで計算されてた
            return SecondaryProgressionHouseCalc(houseList, natallist, natalTime, transitTime, lat, lng, timezone);
        }

        /// <summary>
        /// すべての値をヘッドの度数マイナス
        /// </summary>
        /// <param name="houseList"></param>
        /// <param name="planetList"></param>
        /// <returns></returns>
        public double[] DraconicHouseCalc(double[] houseList, Dictionary<int, PlanetData> planetList)
        {
            double targetDegree;
            if (configData.nodeCalc == ENodeCalc.MEAN)
            {
                targetDegree = planetList[CommonData.ZODIAC_DH_MEANNODE].absolute_position;
            }
            else
            {
                targetDegree = planetList[CommonData.ZODIAC_DH_TRUENODE].absolute_position;
            }

            double[] newList = houseList.Select(d => { return d - targetDegree; }).ToArray();
            return newList;
        }

        public EclipseCalc GetEclipseInstance()
        {
            if (eclipse == null)
            {
                eclipse = new EclipseCalc(s, configData);
            }
            return eclipse;
        }

        public Dictionary<int, PlanetData> Progress(Dictionary<int, PlanetData> list1, UserData udata, DateTime transitDate, double timezone, double lat, double lng)
        {
            Dictionary<int, PlanetData> p;
            if (configData.progression == EProgression.SOLAR)
            {
                p = SolarArcCalc(list1, udata.GetBirthDateTime(), transitDate, timezone);
            }
            else if (configData.progression == EProgression.SECONDARY)
            {
                p = SecondaryProgressionCalc(list1, udata.GetBirthDateTime(), transitDate, timezone, lat, lng);
            }
            else if (configData.progression == EProgression.PRIMARY)
            {
                p = PrimaryProgressionCalc(list1, udata.GetBirthDateTime(), transitDate);
            }
            else if (configData.progression == EProgression.CPS)
            {
                p = CompositProgressionCalc(list1, udata.GetBirthDateTime(), transitDate, timezone);
            }
            else
            {
                p = PositionCalc(transitDate, udata.lat, udata.lng, configData.houseCalc, 1);
            }

            return p;
        }

        public Dictionary<int, PlanetData> DraconicPositionCalc(DateTime d, double lat, double lng, EHouseCalc houseKind, int subIndex)
        {
            Dictionary<int, PlanetData> planetList = PositionCalc(d, lat, lng, houseKind, subIndex);

            double targetDegree;
            if (configData.nodeCalc == ENodeCalc.MEAN)
            {
                targetDegree = planetList[CommonData.ZODIAC_DH_MEANNODE].absolute_position;
            }
            else
            {
                targetDegree = planetList[CommonData.ZODIAC_DH_TRUENODE].absolute_position;
            }

            Dictionary<int, PlanetData> newPlanetList = new Dictionary<int, PlanetData>();
            foreach (KeyValuePair<int, PlanetData> pair in planetList)
            {
                PlanetData data = pair.Value;
                data.absolute_position -= targetDegree;
                newPlanetList.Add(pair.Key, data);
            }

            return newPlanetList;
        }
    }
}
