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

            // (Earth-planet-sun)
            // アスペクトの角度とは違う
            // 新月：180度
            // 満月：0度
            s.swe_calc(dret[1], 0, flag, x1, ref serr);
            double sunDegree = x1[0];
            s.swe_calc(dret[1], 1, flag, x2, ref serr);
            double moonDegree = x2[0];
            DateTime newday = date;
            int offset = 0;

            if (sunDegree - moonDegree < 1)
            {
                offset = 1;
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
            if (sunDegree - moonDegree < 0)
            {
                // この時点でマイナス(月が先行)の場合
                while (sunDegree - moonDegree < 0)
                {
                    offset = 1;
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
            }
            // 足していくだけだから0度またいでも大丈夫のはず
            // ただ0またぐとマイナスになるから1mしか進んでくれない
            while (Math.Abs(sunDegree - moonDegree) > 0.01)
            {
                if (sunDegree - moonDegree < 0)
                {
                    offset = 1;
                    newday = newday.AddDays(offset);
                    Debug.WriteLine("+1d");
                } else
                {
                    if (sunDegree - moonDegree > 24)
                    {
                        offset = 1;
                        newday = newday.AddDays(offset);
                        Debug.WriteLine("+1d");
                    }
                    else if (sunDegree - moonDegree > 12)
                    {
                        offset = 12;
                        newday = newday.AddHours(offset);
                        Debug.WriteLine("+12h");
                    }
                    else if (sunDegree - moonDegree > 6)
                    {
                        offset = 1;
                        newday = newday.AddHours(offset);
                        Debug.WriteLine("+1h");
                    }
                    else if (sunDegree - moonDegree > 1)
                    {
                        offset = 30;
                        newday = newday.AddMinutes(offset);
                        Debug.WriteLine("+30m");
                    }
                    else if (sunDegree - moonDegree > 0.1)
                    {
                        offset = 5;
                        newday = newday.AddMinutes(offset);
                        Debug.WriteLine("+5m");
                    }
                    else
                    {
                        offset = 1;
                        newday = newday.AddMinutes(offset);
                        Debug.WriteLine("+1m");
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

                Debug.WriteLine(newday.ToString());
                Debug.WriteLine(x1[0]);
                Debug.WriteLine(x2[0]);
            }

            return newday;
		}
	}
}

