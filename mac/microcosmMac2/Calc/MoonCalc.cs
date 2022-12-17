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
                    offset = 1;
                } else
                {
                    offset = 2;
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
                double calcMoon = moonDegree;
                if (isIn(sunDegree, moonDegree, 180, 6))
                {
                    break;
                }

                double orb = GetOrb(sunDegree, moonDegree);
                Debug.WriteLine(orb);
                if (isApply(sunDegree, moonDegree))
                {
                    if (orb > 170)
                    {
                        offset = 1;
                        newday = newday.AddHours(offset);
                    }
                    else
                    {
                        offset = 1;
                        newday = newday.AddDays(offset);
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

            //誤差があまりにも近い(すでに満月)=次の満月を計算

            Debug.WriteLine("絶対差");
            Debug.WriteLine(Math.Abs(sunDegree - moonDegree));

            // 普通に1日ずつ足していっていけばいいのでは？


            // 1.一旦この時点の月の位置を算出
            double newMoonDegree = (sunDegree + 180.0) % 360.0;

            if (sunDegree < 180)
            {
                // この場合moonは必ず180〜359に入る、わけでもない
                // 仮度数が180〜359なので、最終度数はずれるかも
                newMoonDegree = (sunDegree + 180.0);
            }
            else
            {
                // この場合、moonは0〜180になる
                newMoonDegree = (sunDegree + 180.0) % 360.0;
            }


            // 太陽先行の場合、月を追い越させる
            /*
            if (sunDegree > moonDegree)
            {
                double savedSunDegree = sunDegree;
                while (sunDegree > moonDegree)
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
                // 牡羊座に行ってしまった
                // この場合は保存していた場所まで太陽を動かす
                // 動かした時点で月は後ろにいるはず
                if (sunDegree < 2)
                {
                    while (Math.Abs(sunDegree - savedSunDegree) > 2)
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
            }
            */


            // 2.大雑把な位置計算
            //if (newMoonDegree < moonDegree && moonDegree < newMoonDegree + 15)
            //{
                int calc = 0;
                while (true)
                {
                    if (newMoonDegree < moonDegree && moonDegree < newMoonDegree + 15)
                    {
                        break;
                    }
                    if (calc > 100) break;
                    calc++;

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
            //}

            /*
            // 太陽が0になっちゃった(セパレート状態、180以下)
            if (sunDegree < 10 && Math.Abs(sunDegree - moonDegree) < 180)
            {
                while (Math.Abs(sunDegree - moonDegree) < 180)
                {
                    offset = 1;
                    newday = newday.AddDays(offset);
                    Debug.WriteLine("+1d");

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

                // 4.最終結果（行き過ぎているので戻す）
                // この時点で太陽は5度とかなので、確実にwhileはマイナス
                // なのでAbs必須
                while (Math.Abs(sunDegree - moonDegree) > 180)
                {
                    if (sunDegree - moonDegree < -182)
                    {
                        offset = 1;
                        newday = newday.AddHours(offset);
                        Debug.WriteLine("+1h");
                    }
                    else if (sunDegree - moonDegree < -181)
                    {
                        offset = -30;
                        newday = newday.AddMinutes(offset);
                        Debug.WriteLine("-30m");
                    }
                    else
                    {
                        offset = -10;
                        newday = newday.AddMinutes(offset);
                        Debug.WriteLine("-10m");
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
                    Debug.WriteLine(x1[0] - x2[0]);
                }

            }
            else
            {
                // こっちはアプライ状態、180以上
                // 3.180を超えていたら進める
                // この結果175〜179くらいになる
                if (Math.Abs(sunDegree - moonDegree) > 180)
                {
                    while (Math.Abs(sunDegree - moonDegree) > 180)
                    {
                        offset = 1;
                        newday = newday.AddDays(offset);
                        Debug.WriteLine("+1d");

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
                }

                // 4.最終結果（行き過ぎているので戻す）
                while (sunDegree - moonDegree < 180)
                {
                    if (sunDegree - moonDegree < 179)
                    {
                        offset = -1;
                        newday = newday.AddHours(offset);
                        Debug.WriteLine("-1h");
                    }
                    else if (sunDegree - moonDegree < 179)
                    {
                        offset = -30;
                        newday = newday.AddMinutes(offset);
                        Debug.WriteLine("-30m");
                    }
                    else
                    {
                        offset = -10;
                        newday = newday.AddMinutes(offset);
                        Debug.WriteLine("-10m");
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
                    Debug.WriteLine(x1[0] - x2[0]);
                }

            }
            */

            Debug.WriteLine(Math.Abs(sunDegree - moonDegree));
            return newday;

            // 太陽が180以下=月は180〜359
            if (sunDegree < 180)
            {
                if (sunDegree + 180 < moonDegree)
                {
                    while (moonDegree < 180)
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
            }
            else
            {
                //太陽が180以上

                // 足していくだけだから0度またいでも大丈夫のはず
                // ただ0またぐとマイナスになるから1mしか進んでくれない
                while (Math.Abs(sunDegree - moonDegree) < 160)
                {
                    offset = 1;
                    newday = newday.AddDays(offset);
                    Debug.WriteLine("+1d");

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

            }


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
            double a = (sunDegree + 180) % 360;
            if (moonDegree < a) moonDegree += 360;
            
            if (a < moonDegree && moonDegree < sunDegree)
            {
                return true;
            }
            return false;
        }
    }
}

