using microcosm.common;
using microcosm.config;
using SwissEphNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.calc
{
    public class EclipseCalc
    {
        public SwissEph s;
        public ConfigData configData;
        public EclipseCalc(SwissEph s, ConfigData configData)
        {
            this.s = s;
            this.configData = configData;
        }

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

        /// <summary>
        /// 回帰計算
        /// 
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="timezone"></param>
        /// <param name="planetId"></param>
        /// <param name="targetDegree"></param>
        /// <param name="isForward"></param>
        /// <returns></returns>
        public DateTime GetEclipse(DateTime begin, double timezone, int planetId, double targetDegree, bool isForward)
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

            // 逆方向(!isForward)ならx日戻してから順行計算させる
            if (planetId == CommonData.ZODIAC_MOON)
            {
                if (!isForward)
                {
                    begin = begin.AddDays(-28);
                }
            }
            else if (planetId == CommonData.ZODIAC_MERCURY)
            {
                if (!isForward)
                {
                    begin = begin.AddDays(-80);
                }
            }
            else if (planetId == CommonData.ZODIAC_VENUS)
            {
                if (!isForward)
                {
                    begin = begin.AddDays(-120);
                }
            }
            else if (planetId == CommonData.ZODIAC_MARS)
            {
                if (!isForward)
                {
                    begin = begin.AddDays(-400);
                }
            }
            else if (planetId == CommonData.ZODIAC_JUPITER)
            {
                if (!isForward)
                {
                    begin = begin.AddYears(-12);
                }
            }
            else if (planetId == CommonData.ZODIAC_SATURN)
            {
                if (!isForward)
                {
                    begin = begin.AddYears(-28);
                }
            }
            s.swe_utc_time_zone(begin.Year, begin.Month, begin.Day, begin.Hour, begin.Minute, begin.Second, timezone,
            ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
            int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;

            //SettingData currentSetting = main.currentSetting;
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

            int calc = 0;
            double offset;
            DateTime newDay = DateTime.Now;
            if (planetId == CommonData.ZODIAC_SUN)
            {
                newDay = isForward ? GetEclipseSunForward(begin, timezone, planetId, targetDegree) : GetEclipseSunBack(begin, timezone, planetId, targetDegree);
            }
            else if (planetId == CommonData.ZODIAC_MOON)
            {
                // 月も逆行無いので単純
                // 月も逆行無いので単純
                double calcDegree = x[0];
                newDay = begin;

                //誤差があまりにも近い=次の角度を計算
                if (isIn(targetDegree, calcDegree, 0, 0.4))
                {
                    if (isApply(targetDegree, calcDegree))
                    {
                        offset = 2;
                    }
                    else
                    {
                        offset = 1;
                    }
                    newDay = newDay.AddDays(offset);
                    s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
                    ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                    s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                    if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;
                    s.swe_calc_ut(dret[1], 1, flag, x, ref serr);
                    calcDegree = x[0];
                }

                int cnt = 0;
                while (true)
                {
                    double orb = GetOrb(targetDegree, calcDegree);
                    if (isIn(targetDegree, calcDegree, 0, 0.01))
                    {
                        break;
                    }

                    Debug.WriteLine(orb);
                    if (isApply(targetDegree, calcDegree))
                    {
                        // applyということは満月過ぎ
                        if (orb < 0.2)
                        {
                            offset = 2;
                            newDay = newDay.AddMinutes(offset);
                        }
                        else if (orb < 1)
                        {
                            offset = 15;
                            newDay = newDay.AddMinutes(offset);
                        }
                        else if (orb < 3)
                        {
                            offset = 1;
                            newDay = newDay.AddHours(offset);
                        }
                        else if (orb < 10)
                        {
                            offset = 3;
                            newDay = newDay.AddHours(offset);
                        }
                        else if (orb < 60)
                        {
                            offset = 1;
                            newDay = newDay.AddDays(offset);
                        }
                        else
                        {
                            offset = 3;
                            newDay = newDay.AddDays(offset);
                        }
                    }
                    else
                    {
                        offset = 3;
                        newDay = newDay.AddDays(offset);
                    }

                    s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
    ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                    s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

                    if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;
                    s.swe_calc_ut(dret[1], 1, flag, x, ref serr);


                    calcDegree = x[0];

                    cnt++;
                    if (cnt > 100)
                    {
                        Console.WriteLine("100ごえ");
                        break;
                    }
                }

                Debug.WriteLine(cnt);
                Debug.WriteLine(newDay);
            }
            else if (planetId == CommonData.ZODIAC_MERCURY)
            {
                // 水星はまずは大雑把でいいや
                // 逆行してすぐ戻る可能性もあるのよね
                // だからマイナスオフセットはなしとする
                double calcDegree = x[0];
                offset = targetDegree - x[0];
                if (Math.Abs(offset) < 2)
                {
                    // すでに回帰時刻の場合
                    // とりあえず何日か進めておく
                    // 過去方向はすでにマイナスしているのでそのまま
                    if (isForward)
                    {
                        offset += 3;
                    }
                }

                // targetが後ろにあるから0度までcurrentを進める
                // 戻しちゃだめ
                while (targetDegree - calcDegree < 0)
                {
                    if (calcDegree > 350)
                    {
                        offset = 2;
                    }
                    else
                    {
                        offset = 20;
                    }
                    newDay = newDay.AddDays(offset);
                    s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                    s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                    s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
                    calc++;

                    Debug.WriteLine(targetDegree);
                    Debug.WriteLine(calcDegree);
                    calcDegree = x[0];
                }

                while (Math.Abs(targetDegree - calcDegree) > 0.1)
                {
                    if (Math.Abs(targetDegree - calcDegree) > 30)
                    {
                        offset = 240;
                        newDay = newDay.AddHours(offset);
                    }
                    else if (Math.Abs(targetDegree - calcDegree) > 1)
                    {
                        offset = 24;
                        newDay = newDay.AddHours(offset);
                    }
                    else
                    {
                        offset = 1;
                        newDay = newDay.AddHours(offset);
                    }

                    s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                    s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                    s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
                    calc++;

                    Debug.WriteLine(targetDegree);
                    Debug.WriteLine(calcDegree);
                    calcDegree = x[0];
                }
            }
            else if (planetId == CommonData.ZODIAC_VENUS)
            {
                // 金星
                double calcDegree = x[0];
                offset = targetDegree - x[0];
                if (Math.Abs(offset) < 2)
                {
                    // すでに回帰時刻の場合
                    // とりあえず何日か進めておく
                    // 過去方向はすでにマイナスしているのでそのまま
                    if (isForward)
                    {
                        offset += 3;
                    }
                }

                // targetが後ろにあるから0度までcurrentを進める
                // 戻しちゃだめ
                while (targetDegree - calcDegree < 0)
                {
                    if (calcDegree > 350)
                    {
                        offset = 2;
                    }
                    else
                    {
                        offset = 30;
                    }
                    newDay = newDay.AddDays(offset);
                    s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                    s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                    s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
                    calc++;

                    Debug.WriteLine(targetDegree);
                    Debug.WriteLine(calcDegree);
                    calcDegree = x[0];
                }

                while (Math.Abs(targetDegree - calcDegree) > 0.1)
                {
                    if (Math.Abs(targetDegree - calcDegree) > 30)
                    {
                        offset = 240;
                        newDay = newDay.AddHours(offset);
                    }
                    else if (Math.Abs(targetDegree - calcDegree) > 1)
                    {
                        offset = 24;
                        newDay = newDay.AddHours(offset);
                    }
                    else
                    {
                        offset = 1;
                        newDay = newDay.AddHours(offset);
                    }

                    s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                    s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                    s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
                    calc++;

                    Debug.WriteLine(targetDegree);
                    Debug.WriteLine(calcDegree);
                    calcDegree = x[0];
                }
            }
            else if (planetId == CommonData.ZODIAC_MARS)
            {
                // 火星
                // 2年以上の周期
                double calcDegree = x[0];
                offset = targetDegree - x[0];
                if (Math.Abs(offset) < 2)
                {
                    // すでに回帰時刻の場合
                    // とりあえず何日か進めておく
                    // 過去方向はすでにマイナスしているのでそのまま
                    if (isForward)
                    {
                        offset += 700;
                        newDay = newDay.AddDays(offset);
                        s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
    ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                        s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                        s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
                        calc++;
                    }
                }

                // targetが後ろにあるから0度までcurrentを進める
                // 戻しちゃだめ
                while (targetDegree - calcDegree < 0)
                {
                    if (calcDegree > 350)
                    {
                        offset = 20;
                    }
                    else
                    {
                        offset = 100;
                    }
                    newDay = newDay.AddDays(offset);
                    s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                    s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                    s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
                    calc++;

                    Debug.WriteLine(targetDegree);
                    Debug.WriteLine(calcDegree);
                    calcDegree = x[0];
                }

                while (Math.Abs(targetDegree - calcDegree) > 1)
                {
                    if (Math.Abs(targetDegree - calcDegree) > 100)
                    {
                        offset = 150;
                        newDay = newDay.AddDays(offset);
                    }
                    else if (Math.Abs(targetDegree - calcDegree) > 30)
                    {
                        offset = 30;
                        newDay = newDay.AddDays(offset);
                    }
                    else if (Math.Abs(targetDegree - calcDegree) > 10)
                    {
                        offset = 8;
                        newDay = newDay.AddDays(offset);
                    }
                    else
                    {
                        offset = 1;
                        newDay = newDay.AddDays(offset);
                    }

                    s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                    s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                    s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
                    calc++;

                    Debug.WriteLine(targetDegree);
                    Debug.WriteLine(calcDegree);
                    calcDegree = x[0];

                    if (calc > 200)
                    {
                        break;
                    }
                }
            }
            else if (planetId == CommonData.ZODIAC_JUPITER)
            {
                // 木星
                // 12年
                double calcDegree = x[0];
                offset = targetDegree - x[0];
                if (Math.Abs(offset) < 2)
                {
                    // すでに回帰時刻の場合
                    // とりあえず何日か進めておく
                    // 過去方向はすでにマイナスしているのでそのまま
                    if (isForward)
                    {
                        offset += 365 * 12;
                        newDay = newDay.AddDays(offset);
                        s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
    ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                        s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                        s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
                        calc++;
                    }
                }

                // targetが後ろにあるから0度までcurrentを進める
                // 戻しちゃだめ
                while (targetDegree - calcDegree < 0)
                {
                    if (calcDegree > 350)
                    {
                        offset = 30;
                    }
                    else
                    {
                        offset = 365;
                    }
                    newDay = newDay.AddDays(offset);
                    s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                    s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                    s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
                    calc++;

                    Debug.WriteLine(targetDegree);
                    Debug.WriteLine(calcDegree);
                    calcDegree = x[0];
                }

                while (Math.Abs(targetDegree - calcDegree) > 1)
                {
                    if (Math.Abs(targetDegree - calcDegree) > 100)
                    {
                        offset = 365;
                        newDay = newDay.AddDays(offset);
                    }
                    else if (Math.Abs(targetDegree - calcDegree) > 30)
                    {
                        offset = 100;
                        newDay = newDay.AddDays(offset);
                    }
                    else if (Math.Abs(targetDegree - calcDegree) > 10)
                    {
                        offset = 20;
                        newDay = newDay.AddDays(offset);
                    }
                    else if (Math.Abs(targetDegree - calcDegree) > 2)
                    {
                        offset = 5;
                        newDay = newDay.AddDays(offset);
                    }
                    else
                    {
                        offset = 1;
                        newDay = newDay.AddDays(offset);
                    }

                    s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                    s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                    s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
                    calc++;

                    Debug.WriteLine(targetDegree);
                    Debug.WriteLine(calcDegree);
                    calcDegree = x[0];

                    if (calc > 200)
                    {
                        break;
                    }
                }
            }
            else if (planetId == CommonData.ZODIAC_SATURN)
            {
                // 土星
                // 28年
                double calcDegree = x[0];
                offset = targetDegree - x[0];
                if (Math.Abs(offset) < 2)
                {
                    // すでに回帰時刻の場合
                    // とりあえず何日か進めておく
                    // 過去方向はすでにマイナスしているのでそのまま
                    if (isForward)
                    {
                        offset += 365 * 28;
                        newDay = newDay.AddDays(offset);
                        s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
    ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                        s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                        s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
                        calc++;
                    }
                }

                // targetが後ろにあるから0度までcurrentを進める
                // 戻しちゃだめ
                while (targetDegree - calcDegree < 0)
                {
                    if (calcDegree > 350)
                    {
                        offset = 30;
                    }
                    else
                    {
                        offset = 365;
                    }
                    newDay = newDay.AddDays(offset);
                    s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                    s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                    s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
                    calc++;

                    Debug.WriteLine(targetDegree);
                    Debug.WriteLine(calcDegree);
                    calcDegree = x[0];
                }

                while (Math.Abs(targetDegree - calcDegree) > 0.5)
                {
                    if (Math.Abs(targetDegree - calcDegree) > 100)
                    {
                        offset = 365 * 2;
                        newDay = newDay.AddDays(offset);
                    }
                    else if (Math.Abs(targetDegree - calcDegree) > 30)
                    {
                        offset = 200;
                        newDay = newDay.AddDays(offset);
                    }
                    else if (Math.Abs(targetDegree - calcDegree) > 10)
                    {
                        offset = 50;
                        newDay = newDay.AddDays(offset);
                    }
                    else if (Math.Abs(targetDegree - calcDegree) > 2)
                    {
                        offset = 5;
                        newDay = newDay.AddDays(offset);
                    }
                    else
                    {
                        offset = 1;
                        newDay = newDay.AddDays(offset);
                    }

                    s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                    s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                    s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
                    calc++;

                    Debug.WriteLine(targetDegree);
                    Debug.WriteLine(calcDegree);
                    calcDegree = x[0];

                    if (calc > 200)
                    {
                        break;
                    }
                }
            }
            Debug.WriteLine(calc);
            return newDay;
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

            if (isIn(targetDegree, sunDegree, 0, 2))
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

            return newday;
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
