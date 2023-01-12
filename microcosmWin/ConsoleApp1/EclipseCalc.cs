using SwissEphNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class EclipseCalc
    {
        public SwissEph s;

        public bool isApply(double sunDegree, double moonDegree, double degree)
        {
            if (sunDegree < 180)
            {
                //100度の場合、0～100と280～360がアプライ
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
                // 250度の場合、70度～250度がアプライ
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
        }

        public EclipseCalc()
        {
            SwissEph s = new SwissEph();
            // http://www.astro.com/ftp/swisseph/ephe/archive_zip/ からDL
            s.swe_set_ephe_path("ephe");
            s.OnLoadFile += (sender, ev) => {
                if (File.Exists(ev.FileName))
                {
                    ev.File = new FileStream(ev.FileName, FileMode.Open);
                }
                else
                {
                    Console.WriteLine("no file:" + ev.FileName);
                }
            };
            this.s = s;
        }

        public double GetOrb(double from, double to)
        {
            double calc = Math.Abs(to - from);
            if (calc > 180) calc = 360 - calc;
            return calc;
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

        public DateTime GetEclipse(DateTime begin, double timezone, int planetId, double targetDegree, bool isForward)
        {
            switch (planetId)
            {
                case 0:
                    return isForward ? GetEclipseSunForward(begin, timezone, planetId, targetDegree) : GetEclipseSunBack(begin, timezone, planetId, targetDegree);
                default:
                    return DateTime.Now;
            }
        }

        public DateTime GetEclipseSunForward(DateTime begin, double timezone, int planetId, double targetDegree)
        {
            // absolute position
            double[] x1 = { 0, 0, 0, 0, 0, 0 };
            double[] x2 = { 0, 0, 0, 0, 0, 0 };
            string serr = "";

            int utc_year = 0;
            int utc_month = 0;
            int utc_day = 0;
            int utc_hour = 0;
            int utc_minute = 0;
            double utc_second = 0;

            // [0]:Ephemeris Time [1]:Universal Time
            double[] dret = { 0.0, 0.0 };

            DateTime newday = begin;
            s.swe_utc_time_zone(begin.Year, begin.Month, begin.Day, begin.Hour, begin.Minute, begin.Second, timezone, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

            int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
            s.swe_calc_ut(dret[1], planetId, flag, x1, ref serr);
            double sunDegree = x1[0];
            int offset = 0;

            if (isIn(sunDegree, targetDegree, 0, 2))
            {
                if (isApply(sunDegree, targetDegree, 180))
                {
                    offset = 1;
                }
                else
                {
                    offset = 1;
                }

                newday = newday.AddDays(offset);
                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, 9.0, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);

                sunDegree = x1[0];
            }

            int cnt = 0;
            while (true)
            {
                double orb = GetOrb(sunDegree, targetDegree);
                if (isIn(sunDegree, targetDegree, 0, 0.01))
                {
                    Console.WriteLine(sunDegree);
                    Console.WriteLine(targetDegree);

                    Console.WriteLine("finish. " + orb);
                    break;
                }

                Console.WriteLine(orb);
                if (isApply(targetDegree, sunDegree, 180))
                {
                    // applyするということは
                    if (orb > 80)
                    {
                        Console.WriteLine("25day");
                        offset = 25;
                        newday = newday.AddDays(offset);
                    }
                    else if (orb > 20)
                    {
                        Console.WriteLine("9day");
                        offset = 9;
                        newday = newday.AddDays(offset);
                    }
                    else if (orb > 10)
                    {
                        Console.WriteLine("4d");
                        offset = 4;
                        newday = newday.AddDays(offset);
                    }
                    else if (orb > 5)
                    {
                        Console.WriteLine("2d");
                        offset = 2;
                        newday = newday.AddDays(offset);
                    }
                    else if (orb > 1.5)
                    {
                        Console.WriteLine("1d");
                        offset = 1;
                        newday = newday.AddDays(offset);
                    }
                    else if (orb > 0.5)
                    {
                        Console.WriteLine("6hour");
                        offset = 6;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 0.2)
                    {
                        Console.WriteLine("1h");
                        offset = 1;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 0.1)
                    {
                        Console.WriteLine("10min");
                        offset = 10;
                        newday = newday.AddMinutes(offset);
                    }
                    else
                    {
                        Console.WriteLine("7min");
                        offset = 7;
                        newday = newday.AddMinutes(offset);
                    }
                }
                else
                {
                    // targetに対しての太陽なのでMoonとは太陽の扱いが逆
                    Console.WriteLine("120day");
                    offset = 120;
                    newday = newday.AddDays(offset);
                }

                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, 9.0, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);

                sunDegree = x1[0];

                cnt++;
                if (cnt > 100)
                {
                    Console.WriteLine("100ごえ");
                    break;
                }
            }

            Console.WriteLine(newday.ToString());
            return newday;
        }

        public DateTime GetEclipseSunBack(DateTime begin, double timezone, int planetId, double targetDegree)
        {
            // absolute position
            double[] x1 = { 0, 0, 0, 0, 0, 0 };
            double[] x2 = { 0, 0, 0, 0, 0, 0 };
            string serr = "";

            int utc_year = 0;
            int utc_month = 0;
            int utc_day = 0;
            int utc_hour = 0;
            int utc_minute = 0;
            double utc_second = 0;

            // [0]:Ephemeris Time [1]:Universal Time
            double[] dret = { 0.0, 0.0 };

            DateTime newday = begin;
            s.swe_utc_time_zone(begin.Year, begin.Month, begin.Day, begin.Hour, begin.Minute, begin.Second, timezone, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

            int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
            s.swe_calc_ut(dret[1], planetId, flag, x1, ref serr);
            double sunDegree = x1[0];
            int offset = 0;

            if (isIn(sunDegree, targetDegree, 0, 2))
            {
                if (isApply(sunDegree, targetDegree, 180))
                {
                    offset = -1;
                }
                else
                {
                    offset = -1;
                }

                newday = newday.AddDays(offset);
                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, 9.0, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);

                sunDegree = x1[0];
            }

            int cnt = 0;
            while (true)
            {
                double orb = GetOrb(sunDegree, targetDegree);
                if (isIn(sunDegree, targetDegree, 0, 0.01))
                {
                    Console.WriteLine(sunDegree);
                    Console.WriteLine(targetDegree);

                    Console.WriteLine("finish. " + orb);
                    break;
                }

                Console.WriteLine(orb);
                if (isApply(targetDegree, sunDegree, 180))
                {
                    // applyするということはtarget手前
                    Console.WriteLine("120day");
                    offset = -120;
                    newday = newday.AddDays(offset);
                }
                else
                {
                    if (orb < 0.1)
                    {
                        Console.WriteLine("-20m");
                        offset = -20;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb < 0.5)
                    {
                        Console.WriteLine("-1h");
                        offset = -1;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb < 1)
                    {
                        Console.WriteLine("-6h");
                        offset = -6;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb < 4)
                    {
                        Console.WriteLine("-1day");
                        offset = -1;
                        newday = newday.AddDays(offset);
                    }
                    else if (orb < 10)
                    {
                        Console.WriteLine("-4d");
                        offset = -4;
                        newday = newday.AddDays(offset);
                    }
                    else if (orb < 20)
                    {
                        Console.WriteLine("-8d");
                        offset = -8;
                        newday = newday.AddDays(offset);
                    }
                    else if (orb < 50)
                    {
                        Console.WriteLine("-15d");
                        offset = -15;
                        newday = newday.AddDays(offset);
                    }
                    else if (orb < 100)
                    {
                        Console.WriteLine("-20d");
                        offset = -20;
                        newday = newday.AddDays(offset);
                    }
                    else
                    {
                        Console.WriteLine("-30d");
                        offset = -30;
                        newday = newday.AddDays(offset);
                    }
                }

                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, 9.0, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);

                sunDegree = x1[0];

                cnt++;
                if (cnt > 100)
                {
                    Console.WriteLine("100ごえ");
                    break;
                }
            }

            Console.WriteLine(newday.ToString());
            return newday;
        }
    }
}
