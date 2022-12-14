using System;
using AppKit;
using System.Diagnostics;
using microcosmMac2.Common;
using microcosmMac2.Config;
using SwissEphNet;

namespace microcosmMac2.Calc
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

            if (planetId == CommonData.ZODIAC_SUN)
            {
                if (!isForward)
                {
                    begin = begin.AddDays(-366);
                }
            }
            else if (planetId == CommonData.ZODIAC_MOON)
            {
                if (!isForward)
                {
                    begin = begin.AddDays(-28);
                }
            }
            s.swe_utc_time_zone(begin.Year, begin.Month, begin.Day, begin.Hour, begin.Minute, begin.Second, timezone,
            ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
            int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
            AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            SettingData currentSetting = appDelegate.settings[0];
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
                // 太陽は逆行無いので単純
                // 324.12345
                // 324.12336
                // ぐらいの精度とする(AMATERUと比べると30秒くらいの誤差)
                // 3度差なら3日足す
                offset = targetDegree - x[0];
                if (offset < 0)
                {
                    // 現在280度、対象が270度だった場合は350度のずれ、来年の10日前くらいにざっくりセット
                    offset += 360;
                }
                int days = (int)offset;
                newDay = begin.AddDays(days);
                s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
                calc++;

                // 1度以内のはず
                Debug.WriteLine(targetDegree);
                Debug.WriteLine(x[0]);

                double calcDegree = x[0];

                // 対象度数が0でcalcDegreeは359なのでおかしくなるので
                if (targetDegree < 1)
                {
                    while (calcDegree > 1)
                    {
                        if (calcDegree < 358)
                        {
                            offset = 358 - calcDegree;
                            newDay = newDay.AddDays(offset);
                        }
                        else if (calcDegree < 359.5)
                        {
                            offset = 12;
                            newDay = newDay.AddHours(offset);
                        }
                        else if (calcDegree < 359.7)
                        {
                            // 0.25 = 6時間
                            offset = 6;
                            newDay = newDay.AddHours(offset);
                        }
                        else if (calcDegree < 359.9)
                        {
                            offset = 1;
                            newDay = newDay.AddHours(offset);
                        }
                        else if (calcDegree < 359.95)
                        {
                            // ここからは分単位で
                            offset = 25;
                            newDay = newDay.AddMinutes(offset);
                        }
                        else if (calcDegree < 359.99)
                        {
                            offset = 7;
                            newDay = newDay.AddMinutes(offset);
                        }
                        else
                        {
                            offset = 45;
                            newDay = newDay.AddSeconds(offset);
                        }
                        s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
                        ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                        s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                        s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
                        calc++;

                        Debug.WriteLine(targetDegree);
                        Debug.WriteLine(x[0]);

                        calcDegree = x[0];
                    }
                }
                else
                {
                    while (targetDegree - calcDegree > 0)
                    {
                        if (Math.Abs(targetDegree - calcDegree) > 0.5)
                        {
                            // 0.5 = 12時間ずらす
                            if (targetDegree - calcDegree < 0)
                            {
                                offset = -12;
                            }
                            else
                            {
                                offset = 12;
                            }
                            newDay = newDay.AddHours(offset);

                            s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
            ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                            s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                            s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
                        }
                        else if (Math.Abs(targetDegree - calcDegree) > 0.3)
                        {
                            // 0.25 = 6時間
                            if (targetDegree - calcDegree < 0)
                            {
                                offset = -6;
                            }
                            else
                            {
                                offset = 6;
                            }

                            newDay = newDay.AddHours(offset);
                        }
                        else if (Math.Abs(targetDegree - calcDegree) > 0.1)
                        {
                            // 0.04 = 1時間
                            if (targetDegree - calcDegree < 0)
                            {
                                offset = -1;
                            }
                            else
                            {
                                offset = 1;
                            }

                            newDay = newDay.AddHours(offset);
                        }
                        else if (Math.Abs(targetDegree - calcDegree) > 0.05)
                        {
                            // ここからは分単位で

                            if (targetDegree - calcDegree < 0)
                            {
                                offset = -25;
                            }
                            else
                            {
                                offset = 25;
                            }

                            newDay = newDay.AddMinutes(offset);
                        }
                        else if (Math.Abs(targetDegree - calcDegree) > 0.01)
                        {
                            // ここからは分単位で

                            if (targetDegree - calcDegree < 0)
                            {
                                offset = -7;
                            }
                            else
                            {
                                offset = 7;
                            }

                            newDay = newDay.AddMinutes(offset);
                        }
                        else if (Math.Abs(targetDegree - calcDegree) > 0.003)
                        {
                            // 分
                            if (targetDegree - calcDegree < 0)
                            {
                                offset = -1;
                            }
                            else
                            {
                                offset = 1;
                            }

                            newDay = newDay.AddMinutes(offset);
                        }
                        else if (Math.Abs(targetDegree - calcDegree) > 0.001)
                        {
                            if (targetDegree - calcDegree < 0)
                            {
                                offset = -45;
                            }
                            else
                            {
                                offset = 45;
                            }

                            newDay = newDay.AddSeconds(offset);
                        }
                        else
                        {
                            // 秒、ここまで必要？
                            if (targetDegree - calcDegree < 0)
                            {
                                offset = -17;
                            }
                            else
                            {
                                offset = 17;
                            }

                            newDay = newDay.AddSeconds(offset);
                        }

                        Debug.WriteLine(newDay.ToString());



                        s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
                        ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                        s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                        s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);
                        calc++;

                        Debug.WriteLine(targetDegree);
                        Debug.WriteLine(x[0]);

                        calcDegree = x[0];
                    }

                }
            }
            else if (planetId == CommonData.ZODIAC_MOON)
            {
                // 月も逆行無いので単純
                // 一日に14〜15度進むので一旦15で割った日数進ませる
                offset = targetDegree - x[0];
                if (offset < 0)
                {
                    offset += 360;
                }
                int days = (int)(offset / 15);
                newDay = begin.AddDays(days);
                s.swe_utc_time_zone(newDay.Year, newDay.Month, newDay.Day, newDay.Hour, newDay.Minute, newDay.Second, timezone,
ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
                s.swe_calc_ut(dret[1], planetId, flag, x, ref serr);

                Debug.WriteLine(x[0]);

                double calcDegree = x[0];

                while (Math.Abs(targetDegree - calcDegree) > 0.001)
                {
                    Debug.WriteLine(Math.Abs(targetDegree - calcDegree));

                    if (Math.Abs(targetDegree - calcDegree) > 50)
                    {
                        if (targetDegree - calcDegree < 0)
                        {
                            offset = -72;
                        }
                        else
                        {
                            offset = 72;
                        }
                        newDay = newDay.AddHours(offset);

                    }
                    else if (Math.Abs(targetDegree - calcDegree) > 20)
                    {
                        if (targetDegree - calcDegree < 0)
                        {
                            offset = -24;
                        }
                        else
                        {
                            offset = 24;
                        }
                        newDay = newDay.AddHours(offset);
                    }
                    else if (Math.Abs(targetDegree - calcDegree) > 6)
                    {
                        if (targetDegree - calcDegree < 0)
                        {
                            offset = -12;
                        }
                        else
                        {
                            offset = 12;
                        }
                        newDay = newDay.AddHours(offset);
                    }
                    else if (Math.Abs(targetDegree - calcDegree) > 1)
                    {
                        if (targetDegree - calcDegree < 0)
                        {
                            offset = -2;
                        }
                        else
                        {
                            offset = 2;
                        }
                        newDay = newDay.AddHours(offset);
                    }
                    else if (Math.Abs(targetDegree - calcDegree) > 0.1)
                    {
                        if (targetDegree - calcDegree < 0)
                        {
                            offset = -12;
                        }
                        else
                        {
                            offset = 12;
                        }
                        newDay = newDay.AddMinutes(offset);
                    }
                    else if (Math.Abs(targetDegree - calcDegree) > 0.03)
                    {
                        if (targetDegree - calcDegree < 0)
                        {
                            offset = -2;
                        }
                        else
                        {
                            offset = 2;
                        }
                        newDay = newDay.AddMinutes(offset);
                    }
                    else
                    {
                        if (targetDegree - calcDegree < 0)
                        {
                            offset = -12;
                        }
                        else
                        {
                            offset = 12;
                        }
                        newDay = newDay.AddSeconds(offset);
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
    }
}
