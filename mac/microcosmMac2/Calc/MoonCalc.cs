using System;
using microcosmMac2.Config;
using microcosmMac2.Models;
using SwissEphNet;
using System.Collections.Generic;
using microcosmMac2.Common;
using System.IO;
using Foundation;
using System.Diagnostics;
using AppKit;
using EventKit;

namespace microcosmMac2.Calc
{
	public class MoonCalc
	{
        public SwissEph s;
        public ConfigData configData;

        public MoonCalc(ConfigData configData)
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

		public DateTime GetNewMoon(DateTime date, double timezone)
		{
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            Dictionary<int, PlanetData> planetList = new Dictionary<int, PlanetData>();

            //          s.swe_set_ephe_path(path);
            int utc_year = 0;
            int utc_month = 0;
            int utc_day = 0;
            int utc_hour = 0;
            int utc_minute = 0;
            double utc_second = 0;
            double[] dret = {0.0, 0.0};
            double[] x = new double[20];
            double[] x1 = new double[20];
            double[] x2 = new double[20];
            string serr = "";

            s.swe_utc_time_zone(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, timezone,
                                ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

            int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
            if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;

            s.swe_calc(dret[1], 0, flag, x1, ref serr);
            double sunDegree = x1[0];
            s.swe_calc(dret[1], 1, flag, x2, ref serr);
            double moonDegree = x2[0];
            DateTime newday = date;
            int offset = 0;

            if (isIn(sunDegree, moonDegree, 0, 5))
            {
                if (isApply(sunDegree, moonDegree))
                {
                    offset = 1;
                }
                else
                {
                    offset = 1;
                }
                newday = newday.AddDays(offset);

                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;
                s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
                s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);


                sunDegree = x1[0];
                moonDegree = x2[0];

            }


            int cnt = 0;
            while (true)
            {
                double orb = GetOrb(sunDegree, moonDegree);
                if (isIn(sunDegree, moonDegree, 0, 0.01))
                {
                    break;
                }

                Debug.WriteLine(orb);
                if (isApply(sunDegree, moonDegree))
                {
                    // applyということは新月手前
                    if (orb > 80)
                    {
                        offset = 3;
                        newday = newday.AddDays(offset);
                    }
                    else if (orb > 20)
                    {
                        offset = 1;
                        newday = newday.AddDays(offset);
                    }
                    else if (orb > 10)
                    {
                        offset = 6;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 5)
                    {
                        offset = 2;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 1.5)
                    {
                        offset = 1;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 0.5)
                    {
                        offset = 40;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb > 0.2)
                    {
                        offset = 10;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb > 0.1)
                    {
                        offset = 5;
                        newday = newday.AddMinutes(offset);
                    }
                    else
                    {
                        offset = 1;
                        newday = newday.AddMinutes(offset);
                    }

                }
                else
                {
                    offset = 3;
                    newday = newday.AddDays(offset);
                }



                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, timezone,
                    ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;
                s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
                s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);
                

                sunDegree = x1[0];
                moonDegree = x2[0];

                Debug.WriteLine(newday.ToString());
                Debug.WriteLine(x1[0]);
                Debug.WriteLine(x2[0]);
                cnt++;
                if (cnt > 100)
                {
                    NSAlert alert = new NSAlert();
                    alert.MessageText = "100ごえ";
                    alert.RunModal();

                    break;
                }
            }

            return newday;
		}

        public DateTime GetNewMoonMinus(DateTime date, double timezone)
        {
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            Dictionary<int, PlanetData> planetList = new Dictionary<int, PlanetData>();

            //          s.swe_set_ephe_path(path);
            int utc_year = 0;
            int utc_month = 0;
            int utc_day = 0;
            int utc_hour = 0;
            int utc_minute = 0;
            double utc_second = 0;
            double[] dret = { 0.0, 0.0 };
            double[] x = new double[20];
            double[] x1 = new double[20];
            double[] x2 = new double[20];
            string serr = "";

            s.swe_utc_time_zone(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, timezone,
                                ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

            int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
            if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;

            s.swe_calc(dret[1], 0, flag, x1, ref serr);
            double sunDegree = x1[0];
            s.swe_calc(dret[1], 1, flag, x2, ref serr);
            double moonDegree = x2[0];
            DateTime newday = date;
            int offset = 0;

            //誤差があまりにも近い(すでに満月)=次の満月を計算
            if (isIn(sunDegree, moonDegree, 0, 5))
            {
                if (isApply(sunDegree, moonDegree))
                {
                    offset = -1;
                }
                else
                {
                    offset = -1;
                }
                newday = newday.AddDays(offset);

                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;
                s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
                s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);


                sunDegree = x1[0];
                moonDegree = x2[0];

            }


            int cnt = 0;
            while (true)
            {
                double orb = GetOrb(sunDegree, moonDegree);
                if (isIn(sunDegree, moonDegree, 0, 0.01))
                {
                    break;
                }

                Debug.WriteLine(orb);
                if (isApply(sunDegree, moonDegree))
                {
                    // applyということは新月手前
                    // 求めるのは次じゃなくて前の満月なのでたくさん戻す
                    offset = -3;
                    newday = newday.AddDays(offset);
                }
                else
                {
                    if (orb > 80)
                    {
                        offset = -3;
                        newday = newday.AddDays(offset);
                    }
                    else if (orb > 20)
                    {
                        offset = -1;
                        newday = newday.AddDays(offset);
                    }
                    else if (orb > 10)
                    {
                        offset = -6;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 5)
                    {
                        offset = -2;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 1.5)
                    {
                        offset = -1;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 0.5)
                    {
                        offset = -40;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb > 0.2)
                    {
                        offset = -10;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb > 0.1)
                    {
                        offset = -5;
                        newday = newday.AddMinutes(offset);
                    }
                    else
                    {
                        offset = -1;
                        newday = newday.AddMinutes(offset);
                    }

                }

                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;
                s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
                s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);


                sunDegree = x1[0];
                moonDegree = x2[0];

                cnt++;
                if (cnt > 100)
                {
                    NSAlert alert = new NSAlert();
                    alert.MessageText = "100ごえ";
                    alert.RunModal();

                    break;
                }
            }

            Debug.WriteLine(cnt);
            Debug.WriteLine(newday);
            Debug.WriteLine(moonDegree);
            return newday;
        }


        public DateTime GetFullMoon(DateTime date, double timezone)
        {
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            Dictionary<int, PlanetData> planetList = new Dictionary<int, PlanetData>();

            //          s.swe_set_ephe_path(path);
            int utc_year = 0;
            int utc_month = 0;
            int utc_day = 0;
            int utc_hour = 0;
            int utc_minute = 0;
            double utc_second = 0;
            double[] dret = { 0.0, 0.0 };
            double[] x = new double[20];
            double[] x1 = new double[20];
            double[] x2 = new double[20];
            string serr = "";

            s.swe_utc_time_zone(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, timezone,
                                ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

            int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
            if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;

            s.swe_calc(dret[1], 0, flag, x1, ref serr);
            double sunDegree = x1[0];
            s.swe_calc(dret[1], 1, flag, x2, ref serr);
            double moonDegree = x2[0];
            DateTime newday = date;
            int offset = 0;

            //誤差があまりにも近い(すでに満月)=次の満月を計算
            if (isIn(sunDegree, moonDegree, 180, 10))
            {
                if (isApply(sunDegree, moonDegree))
                {
                    offset = 2;
                } else
                {
                    offset = 1;
                }
                newday = newday.AddDays(offset);

                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;
                s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
                s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);


                sunDegree = x1[0];
                moonDegree = x2[0];

            }
            int cnt = 0;
            while(true)
            {
                double calcDegree = sunDegree + 180;
                double orb = GetOrb(sunDegree, moonDegree);
                if (isIn(sunDegree, moonDegree, 180, 0.05))
                {
                    break;
                }

                Debug.WriteLine(orb);
                if (isApply(sunDegree, moonDegree))
                {
                    // applyということは新月に近づくということ
                    // 満月からは離れる
                    Debug.WriteLine("+3day");
                    offset = 3;
                    newday = newday.AddDays(offset);
                }
                else
                {
                    if (orb > 179)
                    {
                        offset = 7;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb > 177)
                    {
                        offset = 20;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb > 165)
                    {
                        offset = 2;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 130)
                    {
                        offset = 1;
                        newday = newday.AddDays(offset);
                    }
                    else
                    {
                        offset = 2;
                        newday = newday.AddDays(offset);
                    }
                }

                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;
                s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
                s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);


                sunDegree = x1[0];
                moonDegree = x2[0];

                cnt++;
                if (cnt > 100)
                {
                    NSAlert alert = new NSAlert();
                    alert.MessageText = "100ごえ";
                    alert.RunModal();

                    break;
                }
            }

            Debug.WriteLine(cnt);
            Debug.WriteLine(newday);
            return newday;
        }

        public DateTime GetFullMoonMinus(DateTime date, double timezone)
        {
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            Dictionary<int, PlanetData> planetList = new Dictionary<int, PlanetData>();

            //          s.swe_set_ephe_path(path);
            int utc_year = 0;
            int utc_month = 0;
            int utc_day = 0;
            int utc_hour = 0;
            int utc_minute = 0;
            double utc_second = 0;
            double[] dret = { 0.0, 0.0 };
            double[] x = new double[20];
            double[] x1 = new double[20];
            double[] x2 = new double[20];
            string serr = "";

            s.swe_utc_time_zone(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, timezone,
                                ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

            int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
            if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;

            s.swe_calc(dret[1], 0, flag, x1, ref serr);
            double sunDegree = x1[0];
            s.swe_calc(dret[1], 1, flag, x2, ref serr);
            double moonDegree = x2[0];
            DateTime newday = date;
            int offset = 0;

            //誤差があまりにも近い(すでに満月)=次の満月を計算
            if (isIn(sunDegree, moonDegree, 180, 5))
            {
                if (isApply(sunDegree, moonDegree))
                {
                    offset = -1;
                }
                else
                {
                    offset = -1;
                }
                newday = newday.AddDays(offset);

                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;
                s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
                s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);


                sunDegree = x1[0];
                moonDegree = x2[0];

            }


            int cnt = 0;
            while (true)
            {
                double orb = GetOrb(sunDegree, moonDegree);
                if (isIn(sunDegree, moonDegree, 180, 0.01))
                {
                    break;
                }

                Debug.WriteLine(orb);
                if (isApply(sunDegree, moonDegree))
                {
                    // applyということは満月過ぎ
                    if (orb > 179.9)
                    {
                        offset = -1;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb > 179)
                    {
                        offset = -10;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb > 178)
                    {
                        offset = -20;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb > 175)
                    {
                        offset = -1;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 170)
                    {
                        offset = -3;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 130)
                    {
                        offset = -1;
                        newday = newday.AddDays(offset);
                    }
                    else 
                    {
                        offset = -3;
                        newday = newday.AddDays(offset);
                    }
                }
                else
                {
                    offset = -3;
                    newday = newday.AddDays(offset);
                }

                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;
                s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
                s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);


                sunDegree = x1[0];
                moonDegree = x2[0];

                cnt++;
                if (cnt > 100)
                {
                    NSAlert alert = new NSAlert();
                    alert.MessageText = "100ごえ";
                    alert.RunModal();

                    break;
                }
            }

            Debug.WriteLine(cnt);
            Debug.WriteLine(newday);
            Debug.WriteLine(moonDegree);
            return newday;
        }


        public bool isIn(double from, double to, double degree, double orb)
        {
            double calc = GetOrb(from, to);
            if (degree - orb < calc && calc < degree + orb)
            {
                return true;
            }

            return false;
        }

        public double GetOrb(double from, double to)
        {
            double calc = Math.Abs(to - from);
            if (calc > 180) calc = 360 - calc;

            return calc;
        }

        public bool isApply(double sunDegree, double moonDegree)
        {
            if (sunDegree < 180)
            {
                double mid = sunDegree + 180;
                if (moonDegree < sunDegree)
                {
                    return true;
                }
                if (mid < moonDegree)
                {
                    return true;
                }
                return false;
            }
            else
            {
                double mid = (sunDegree + 180) % 360;
                if (moonDegree < mid)
                {
                    return false;
                }
                if (sunDegree < moonDegree)
                {
                    return false;
                }
                return true;
            }

            return false;
        }
    }
}

