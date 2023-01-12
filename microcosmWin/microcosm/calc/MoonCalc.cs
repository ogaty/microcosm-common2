using microcosm.config;
using SwissEphNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.calc
{
    public class MoonCalc
    {
        public ConfigData configData;
        public SwissEph s;

        public MoonCalc(ConfigData configData, SwissEph s) {
            this.configData = configData;
            this.s = s;
        }

        public bool isApply(double sunDegree, double moonDegree)
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

        public DateTime NewMoonPlus(DateTime date, double timezone)
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

            DateTime newday = date;
            s.swe_utc_time_zone(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, timezone, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

            int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
            s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
            s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);
            double sunDegree = x1[0];
            double moonDegree = x2[0];
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
                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, 9.0, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

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
                    Console.WriteLine(sunDegree);
                    Console.WriteLine(moonDegree);

                    Console.WriteLine("finish. " + orb);
                    break;
                }

                if (isApply(sunDegree, moonDegree))
                {
                    // applyするということは新月手前
                    if (orb > 80)
                    {
                        Console.WriteLine("3day");
                        offset = 3;
                        newday = newday.AddDays(offset);
                    }
                    else if (orb > 20)
                    {
                        Console.WriteLine("1day");
                        offset = 1;
                        newday = newday.AddDays(offset);
                    }
                    else if (orb > 10)
                    {
                        Console.WriteLine("6hour");
                        offset = 6;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 5)
                    {
                        Console.WriteLine("2hour");
                        offset = 2;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 1.5)
                    {
                        Console.WriteLine("1hour");
                        offset = 1;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 0.5)
                    {
                        Console.WriteLine("40min");
                        offset = 40;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb > 0.2)
                    {
                        Console.WriteLine("10min");
                        offset = 10;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb > 0.1)
                    {
                        Console.WriteLine("5min");
                        offset = 5;
                        newday = newday.AddMinutes(offset);
                    }
                    else
                    {
                        Console.WriteLine("1min");
                        offset = 1;
                        newday = newday.AddMinutes(offset);
                    }
                }
                else
                {
                    Console.WriteLine(orb);

                    Console.WriteLine("3day");
                    offset = 3;
                    newday = newday.AddDays(offset);
                }

                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, timezone, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
                s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);

                sunDegree = x1[0];
                moonDegree = x2[0];

                cnt++;
                if (cnt > 100)
                {
                    Console.WriteLine("100ごえ");
                    break;
                }
            }
            return newday;
        }

        public DateTime NewMoonMinus(DateTime date, double timezone)
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

            DateTime newday = date;
            s.swe_utc_time_zone(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, timezone, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

            int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
            s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
            s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);
            double sunDegree = x1[0];
            double moonDegree = x2[0];
            int offset = 0;

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
                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, timezone, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

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
                    Console.WriteLine(sunDegree);
                    Console.WriteLine(moonDegree);

                    Console.WriteLine("finish. " + orb);
                    break;
                }

                if (isApply(sunDegree, moonDegree))
                {
                    // applyするということは新月手前
                    // 前の新月なので、たくさん戻す
                    Console.WriteLine("-3day");
                    offset = -3;
                    newday = newday.AddDays(offset);
                }
                else
                {
                    Console.WriteLine(orb);
                    if (orb > 80)
                    {
                        Console.WriteLine("-3day");
                        offset = -3;
                        newday = newday.AddDays(offset);
                    }
                    else if (orb > 20)
                    {
                        Console.WriteLine("-1day");
                        offset = -1;
                        newday = newday.AddDays(offset);
                    }
                    else if (orb > 10)
                    {
                        Console.WriteLine("-6hour");
                        offset = -6;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 5)
                    {
                        Console.WriteLine("-2hour");
                        offset = -2;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 1.5)
                    {
                        Console.WriteLine("-1hour");
                        offset = -1;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 0.5)
                    {
                        Console.WriteLine("-40min");
                        offset = -40;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb > 0.2)
                    {
                        Console.WriteLine("-10min");
                        offset = -10;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb > 0.1)
                    {
                        Console.WriteLine("-5min");
                        offset = -5;
                        newday = newday.AddMinutes(offset);
                    }
                    else
                    {
                        Console.WriteLine("-1min");
                        offset = -1;
                        newday = newday.AddMinutes(offset);
                    }
                }

                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, timezone, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
                s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);

                sunDegree = x1[0];
                moonDegree = x2[0];

                cnt++;
                if (cnt > 100)
                {
                    Console.WriteLine("100ごえ");
                    break;
                }
            }

            return newday;
        }

        public DateTime FullMoonPlus(DateTime date, double timezone)
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

            DateTime newday = date;
            s.swe_utc_time_zone(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, timezone, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
            Console.WriteLine(serr);

            int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
            s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
            s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);
            double sunDegree = x1[0];
            double moonDegree = x2[0];
            int offset = 0;

            if (isIn(sunDegree, moonDegree, 180, 5))
            {
                if (isApply(sunDegree, moonDegree))
                {
                    offset = 2;
                }
                else
                {
                    offset = 2;
                }

                newday = newday.AddDays(offset);
                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, timezone, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
                s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);

                sunDegree = x1[0];
                moonDegree = x2[0];
            }

            int cnt = 0;
            while (true)
            {
                double orb = GetOrb(sunDegree, moonDegree);
                if (isIn(sunDegree, moonDegree, 180, 0.05))
                {
                    Console.WriteLine("finish. " + orb);
                    break;
                }

                if (isApply(sunDegree, moonDegree))
                {
                    // applyするということは新月に近づくということ
                    // 満月からは離れる
                    Console.WriteLine("+3day");
                    offset = 3;
                    newday = newday.AddDays(offset);
                }
                else
                {
                    Console.WriteLine(orb);
                    if (orb > 179)
                    {
                        Console.WriteLine("+5m");
                        offset = 5;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb > 177)
                    {
                        Console.WriteLine("+20m");
                        offset = 20;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb > 165)
                    {
                        Console.WriteLine("+2h");
                        offset = 2;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 130)
                    {
                        Console.WriteLine("+1day");
                        offset = 1;
                        newday = newday.AddDays(offset);
                    }
                    else
                    {
                        Console.WriteLine("+2day");
                        offset = 2;
                        newday = newday.AddDays(offset);
                    }
                }

                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, timezone, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
                s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);

                sunDegree = x1[0];
                moonDegree = x2[0];

                cnt++;
                if (cnt > 100)
                {
                    Console.WriteLine("100ごえ");
                    break;
                }
            }
            return newday;
        }

        public DateTime FullMoonMinus(DateTime date, double timezone)
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

            DateTime newday = date;
            s.swe_utc_time_zone(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, timezone, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

            int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
            s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
            s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);
            double sunDegree = x1[0];
            double moonDegree = x2[0];
            int offset = 0;

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
                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, timezone, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
                s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);

                sunDegree = x1[0];
                moonDegree = x2[0];
            }

            int cnt = 0;
            while (true)
            {
                double orb = GetOrb(sunDegree, moonDegree);
                Console.WriteLine(sunDegree - moonDegree);

                if (isIn(sunDegree, moonDegree, 180, 0.01))
                {
                    Console.WriteLine(sunDegree);
                    Console.WriteLine(moonDegree);

                    Console.WriteLine("finish. " + orb);
                    break;
                }

                if (isApply(sunDegree, moonDegree))
                {
                    // applyということは満月過ぎ
                    if (orb > 179.9)
                    {
                        Console.WriteLine("-1min");
                        offset = -1;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb > 179)
                    {
                        Console.WriteLine("-10min");
                        offset = -10;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb > 178)
                    {
                        Console.WriteLine("-20min");
                        offset = -20;
                        newday = newday.AddMinutes(offset);
                    }
                    else if (orb > 175)
                    {
                        Console.WriteLine("-1hour");
                        offset = -1;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 170)
                    {
                        Console.WriteLine("-3hour");
                        offset = -1;
                        newday = newday.AddHours(offset);
                    }
                    else if (orb > 130)
                    {
                        Console.WriteLine("-1day");
                        offset = -1;
                        newday = newday.AddDays(offset);
                    }
                    else
                    {
                        Console.WriteLine("-3day:" + orb);
                        offset = -3;
                        newday = newday.AddDays(offset);
                    }

                }
                else
                {
                    // セパレートしている、つまり戻すと新月に近づく

                    Console.WriteLine("-3dayB" + newday.ToString());
                    offset = -3;
                    newday = newday.AddDays(offset);
                }

                s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, timezone, ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
                s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);

                sunDegree = x1[0];
                moonDegree = x2[0];
                Console.WriteLine(String.Format("{0} {1}", sunDegree, moonDegree));

                cnt++;
                if (cnt > 100)
                {
                    Console.WriteLine("100ごえ");
                    break;
                }
            }
            return newday;
        }

    }
}
