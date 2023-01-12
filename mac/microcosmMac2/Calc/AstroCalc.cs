using System;
using microcosmMac2.Common;
using microcosmMac2.Config;
using microcosmMac2.Models;
using microcosmMac2.User;
using SwissEphNet;
using System.Collections.Generic;
using System.IO;
using Foundation;
using System.Linq;
using System.Diagnostics;
using SearchKit;
using AppKit;
using System.Globalization;
using static CoreMedia.CMTime;
using static CoreFoundation.DispatchSource;
using EventKit;
using SkiaSharp;

namespace microcosmMac2.Calc
{
    public class AstroCalc
    {
        public SwissEph s;
        public ConfigData configData;
        public EclipseCalc eclipse;
        public const double SOLAR_YEAR = 365.2424;

        public AstroCalc(ConfigData configData)
        {
            var root = Util.root;

            this.configData = configData;
            s = new SwissEph();
            s.OnLoadFile += (sender, e) => {
                var path = Path.Combine(NSBundle.MainBundle.BundlePath, "Contents", "Resources", "ephe");
                var f = e.FileName.Split('\\');
                if (File.Exists(path + "/" + f[1]))
                {
                    e.File = new FileStream(path + "/" + f[1], FileMode.Open, FileAccess.Read, FileShare.Read);
                }
                else
                {
                    Debug.WriteLine("err:" + f[1]);
                }
            };
        }

        /// <summary>
        /// planet position calcurate.
        /// </summary>
        /// <param name="timezone">Timezone. JST=9.0</param>
        public Dictionary<int, PlanetData> PositionCalc(DateTime date, double timezone, SettingData currentSetting)
        {
            Dictionary<int, PlanetData> planetList = new Dictionary<int, PlanetData> ();

            //          s.swe_set_ephe_path(path);
            int utc_year = 0;
            int utc_month = 0;
            int utc_day = 0;
            int utc_hour = 0;
            int utc_minute = 0;
            double utc_second = 0;
            double[] dret = { 0.0, 0.0 };
            double[] x = { 0, 0, 0, 0, 0, 0 };
            string serr = "";

            s.swe_utc_time_zone(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, timezone,
                                ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

            int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
            if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;
            int subIndex = 0;

            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;

            Enumerable.Range(0, 21).ToList().ForEach(i =>
            {
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

                Debug.WriteLine("{0} {1}", i, x[0]);

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
                //ii = i;
                /*
                int save_number = planet_number;
                if (planet_number == 100377)
                {
                    save_number = 90377;
                }
                if (planet_number == 146108)
                {
                    save_number = 136108;
                }
                if (planet_number == 146199)
                {
                    save_number = 136199;
                }
                if (planet_number == 146472)
                {
                    save_number = 136472;
                }
                */
                planetList[i] = p;
            });

            PlanetData dtMean = new PlanetData()
            {
                no = CommonData.ZODIAC_DT_MEAN,
                absolute_position = (planetList[CommonData.ZODIAC_DH_MEANNODE].absolute_position + 180.0) % 360,
                speed = planetList[CommonData.ZODIAC_DH_MEANNODE].speed,
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

            planetList[CommonData.ZODIAC_DT_MEAN] = dtMean;

            PlanetData dtTrue = new PlanetData()
            {
                no = CommonData.ZODIAC_DT_TRUE,
                absolute_position = (planetList[CommonData.ZODIAC_DH_TRUENODE].absolute_position + 180.0) % 360,
                speed = planetList[CommonData.ZODIAC_DH_TRUENODE].speed,
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
                dtTrue.isDisp = currentSetting.GetDispPlanet(CommonData.ZODIAC_DT_TRUE);
                dtTrue.isAspectDisp = currentSetting.GetDispAspectPlanet(CommonData.ZODIAC_DT_TRUE);
            }

            planetList[CommonData.ZODIAC_DT_TRUE] = dtTrue;

            s.swe_close();
            return planetList;
        }

        /// <summary>
        /// 位置だけ計算、dispFlagとか関係なし
        /// </summary>
        /// <param name="date"></param>
        /// <param name="timezone"></param>
        /// <param name="planetId"></param>
        /// <returns></returns>
        public double PositionCalcSingle(DateTime date, double timezone, int planetId)
        {
            int utc_year = 0;
            int utc_month = 0;
            int utc_day = 0;
            int utc_hour = 0;
            int utc_minute = 0;
            double utc_second = 0;
            double[] dret = { 0.0, 0.0 };
            double[] x = { 0, 0, 0, 0, 0, 0 };
            string serr = "";

            s.swe_utc_time_zone(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, timezone,
                                ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

            int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;

            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;

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

            return x[0];
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
                        absolute_position = newPosition,
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
        /// ハウス計算
        /// </summary>
        /// <returns>The calculate.</returns>
        /// <param name="d">時刻</param>
        /// <param name="lat">緯度</param>
        /// <param name="lng">経度</param>
        /// <param name="houseKind">ハウス種別
        /// 0:Placidus
        /// 1:Koch
        /// 2:Campanus
        /// 
        /// </param>
        /// 
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
            else if (houseKind == EHouseCalc.PORPHYRY)
            {
                // Porphyrious
                s.swe_houses(dret[1], lat, lng, 'O', cusps, ascmc);
            }
            else if (houseKind == EHouseCalc.REGIOMONTANUS)
            {
                // Porphyrious
                s.swe_houses(dret[1], lat, lng, 'R', cusps, ascmc);
            }
            else if (houseKind == EHouseCalc.EQUAL)
            {
                // Equal
                s.swe_houses(dret[1], lat, lng, 'E', cusps, ascmc);
            }
            else if (houseKind == EHouseCalc.ZEROARIES)
            {
                // ZeroAries
                s.swe_houses(dret[1], lat, lng, 'N', cusps, ascmc);
            }
            else if (houseKind == EHouseCalc.SOLAR)
            {
                // Solar
                // 太陽の度数をASCとして30度
            }
            else if (houseKind == EHouseCalc.SOLARSIGN)
            {
                // SolarSign
                // 太陽のサインの0度をASCとして30度
            }
            s.swe_close();

            return cusps;
        }

        /// <summary>
        /// 再計算
        /// </summary>
        /// <returns>The calculate.</returns>
        /// <param name="config">Config.</param>
        /// <param name="setting">Setting.</param>
        /// <param name="udata">UserData.</param>
        public Dictionary<int, PlanetData> ReCalc(ConfigData config, SettingData setting, UserData udata)
        {
            Dictionary<int, PlanetData> p;
            if (config.sidereal == Esidereal.DRACONIC)
            {
                p = DraconicPositionCalc(udata.GetDateTime(), udata.timezone, setting);
            }
            else
            {
                p = PositionCalc(udata.GetDateTime(), udata.timezone, setting);
            }
            //double[] cusps = CuspCalc(udata.GetDateTime(), udata.timezone, udata.lat, udata.lng, config.houseCalc);
            //Calculation calculate = new Calculation(p, cusps);

            return p;
        }

        /// <summary>
        /// プログレス用再計算
        /// </summary>
        /// <returns>The calculate progress.</returns>
        /// <param name="config">Config.</param>
        /// <param name="setting">Setting.</param>
        /// <param name="udata">Udata.</param>
        public Dictionary<int, PlanetData> ReCalcProgress(ConfigData config, SettingData setting, Dictionary<int, PlanetData> natalList, UserData udata, DateTime transitTime, double timezone)
        {
            Dictionary<int, PlanetData> p = null;
            if (config.progression == EProgression.SOLARARC)
            {
                p = SolarArcCalc(natalList, udata.GetDateTime(), transitTime, timezone);

            }
            else if (config.progression == EProgression.SECONDARY)
            {
                p = SecondaryProgressionCalc(natalList, udata.GetDateTime(), transitTime, timezone);

            }
            else if (config.progression == EProgression.PRIMARY)
            {
                p = PrimaryProgressionCalc(natalList, udata.GetDateTime(), transitTime);

            }
            else if (config.progression == EProgression.CPS)
            {
                p = CompositProgressionCalc(natalList, udata.GetDateTime(), transitTime, timezone);

            }
            else
            {
                p = PositionCalc(udata.GetDateTime(), udata.timezone, setting);
            }

            //double[] cusps = CuspCalc(udata.GetDateTime(), udata.timezone, udata.lat, udata.lng, config.houseCalc);
            //Calculation calculate = new Calculation(p, cusps);

            return p;
        }


        /*
        private void search(GregorianCalendar begin_gcal,
                       int hit_count,
                       int body_id,
                       double lon,
                       boolean backward,
                       List<GregorianCalendar> resultList)
        {
            SwissEph eph = Ephemeris.getInstance().getSwissEph();
            TimeZone timeZone = begin_gcal.getTimeZone();
            double jday = JDay.get(begin_gcal);
            System.out.println("calendar = " + String.format("%tF %tT", begin_gcal, begin_gcal));
            System.out.println("start jday = " + jday);
            System.out.println("target lon = " + lon);
            System.out.println("body   id  = " + body_id);
            System.out.println("backward = " + backward);
            int flags = SweConst.SEFLG_SWIEPH | SweConst.SEFLG_TRANSIT_LONGITUDE;
            TransitCalculator tcal = new TCPlanet(eph, body_id, flags, lon);
            while (hit_count > 0)
            {
                double deltaT = SweDate.getDeltaT(jday);
                double ET = jday + deltaT;
                double transitTime = eph.getTransitET(tcal, ET, backward) - deltaT;
                GregorianCalendar gcal2 = JDay.getCalendar(transitTime, timeZone);
                resultList.add(gcal2);
                jday = transitTime;
                hit_count--;
            }
        }
        */

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
            double calcDegree = getBodyBySecondaryProgression(natalTime, transitTime, timezone, 0);
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
            double calcDegree = getBodyBySecondaryProgression(natalTime, transitTime, timezone, 0);
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
        public double getBodyBySecondaryProgression(DateTime natalTime, DateTime transitTime, double timezone, int planetId)
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
        public Dictionary<int, PlanetData> SecondaryProgressionCalc(Dictionary<int, PlanetData> natallist, DateTime natalTime, DateTime transitTime, double timezone)
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

                PlanetData progressdata = null;
                if (pair.Key == CommonData.ZODIAC_DT_TRUE)
                {
                    // DHより先にDTの計算来たら落ちる
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
                else if (pair.Key == CommonData.ZODIAC_DT_MEAN)
                {
                    // DHより先にDTの計算来たら落ちる
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
                else
                {
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
            double years = ts.TotalDays / SOLAR_YEAR;

            // 日付を秒数に変換する
            int seconds = (int)(years * 86400);

            TimeSpan add = TimeSpan.FromSeconds(seconds);

            DateTime newTime = natalTime + add;


            //todo
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

                // ここからセカンダリー
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

                if (pair.Key == CommonData.ZODIAC_DT_TRUE)
                {
                    // DHより先にDTの計算来たら落ちる
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
                else if (pair.Key == CommonData.ZODIAC_DT_MEAN)
                {
                    // DHより先にDTの計算来たら落ちる
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
                else
                {
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



            return progresslist;
        }
        public double[] CompositProgressionHouseCalc(double[] houseList, Dictionary<int, PlanetData> natallist, DateTime natalTime, DateTime transitTime, double lat, double lng, double timezone)
        {
            // AMATERU、SG共にSecondaryで計算されてた
            return SecondaryProgressionHouseCalc(houseList, natallist, natalTime, transitTime, lat, lng, timezone);
        }

        public double[] HouseCalcProgress(ConfigData config, SettingData setting, double[] houseList, Dictionary<int, PlanetData> natallist, DateTime natalTime, DateTime transitTime, double lat, double lng, double timezone)
        {
            double[] house;
            if (config.progression == EProgression.SOLARARC)
            {
                house = SolarArcHouseCalc(natallist[0].absolute_position, houseList, natalTime, transitTime, timezone);

            }
            else if (config.progression == EProgression.SECONDARY)
            {
                house = SecondaryProgressionHouseCalc(houseList, natallist, natalTime, transitTime, lat, lng, timezone);

            }
            else if (config.progression == EProgression.PRIMARY)
            {
                house = PrimaryProgressionHouseCalc(houseList, natalTime, transitTime);

            }
            else if (config.progression == EProgression.CPS)
            {
                house = CompositProgressionHouseCalc(houseList, natallist, natalTime, transitTime, lat, lng, timezone);

            }
            else
            {
                house = CuspCalc(natalTime, timezone, lat, lng, config.houseCalc);
            }

            return house;
        }

        public EclipseCalc GetEclipseInstance()
        {
            if (eclipse == null)
            {
                eclipse = new EclipseCalc(s, configData);
            }
            return eclipse;
        }

        /// <summary>
        /// draconic
        /// </summary>
        /// <param name="d"></param>
        /// <param name="timezone"></param>
        /// <param name="currentSetting"></param>
        /// <returns></returns>
        public Dictionary<int, PlanetData> DraconicPositionCalc(DateTime d, double timezone, SettingData currentSetting)
        {
            Dictionary<int, PlanetData> planetList = PositionCalc(d, timezone, currentSetting);

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
                if (data.absolute_position < 0)
                {
                    data.absolute_position += 360;
                }
                newPlanetList.Add(pair.Key, data);
            }

            return newPlanetList;
        }
    }
}

