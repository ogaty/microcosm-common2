//
//  MoonCalc.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2025/03/01.
//

import Foundation
import swissEphemeris

class MoonCalc {
    public var s: swissEphemerisMain
    public var configData: ConfigData

    init(config: ConfigData, swiss: swissEphemerisMain) {
        configData = config
        s = swiss
    }

    public func getNewMoon(d: MyDate, timezone: Double) -> MyDate
    {
        // SEFLG_SWIEPH: 2 SEFLG_SPEED: 256
        var flag = 258
        let jd = s.swe_utc_to_jd(year: Int32(d.getUTCFullYear()), month: Int32(d.getUTCMonth()), day: Int32(d.getUTCDay()), hour: Int32(d.getUTCHour()), minute: Int32(d.getMinute()), second: Double(d.getSecond()))

        var calc1: Calc
        var calc2: Calc
        if (configData.centric == ECentric.HELIO_CENTRIC)
        {
            flag |= SweConst.SEFLG_HELCTR.rawValue
        }
        if (configData.sideReal == ESideReal.SIDEREAL)
        {
            flag |= SweConst.SEFLG_SIDEREAL.rawValue
            s.swe_set_sid_mode(flag: SweSideReal.SE_SIDM_LAHIRI.rawValue)
            // ayanamsa計算
            let ut: Ayanamsa = s.swe_get_ayanamsa_ex_ut(tjd_ut: jd.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

            // Ephemeris Timeで計算, 結果はxに入る
            calc1 = s.calc_ut(jd: ut.daya, planetNo: 0, flag: Int32(flag))
            calc2 = s.calc_ut(jd: ut.daya, planetNo: 1, flag: Int32(flag))
        }
        else
        {
            calc1 = s.calc_ut(jd: jd.dret[0], planetNo: 0, flag: Int32(flag))
            calc2 = s.calc_ut(jd: jd.dret[0], planetNo: 1, flag: Int32(flag))
        }

        var sunDegree = calc1.x[0]
        var moonDegree = calc2.x[0]
        let newDay = d
        var offset: Double = 0.0

        // めっちゃ近い場合全く動かないので強制的に動かす
        if isIn(from: sunDegree, to: moonDegree, degree: 0, orb: 5) {
            offset = 1.0
            newDay.addDays(day: offset)

            // サイドリアルはサポート外でいい？
            let jd = s.swe_utc_to_jd(year: Int32(d.getUTCFullYear()), month: Int32(d.getUTCMonth()), day: Int32(d.getUTCDay()), hour: Int32(d.getUTCHour()), minute: Int32(d.getMinute()), second: Double(d.getSecond()))

            calc1 = s.calc_ut(jd: jd.dret[0], planetNo: 0, flag: Int32(flag))
            calc2 = s.calc_ut(jd: jd.dret[0], planetNo: 1, flag: Int32(flag))

            sunDegree = calc1.x[0]
            moonDegree = calc2.x[0]
        }

        var cnt = 0
        while (true) {
            let orb = getOrb(from: sunDegree, to: moonDegree)
            if (isIn(from: sunDegree, to: moonDegree, degree: 0, orb: 0.01)) {
                break
            }
            
            if (isApply(sunDegree: sunDegree, moonDegree: moonDegree)) {
                if orb > 80 {
                    offset = 3.0
                    newDay.addDays(day: offset)
                } else if orb > 20 {
                    offset = 1.0
                    newDay.addDays(day: offset)
                } else if orb > 10 {
                    offset = 6.0
                    newDay.addHours(hour: offset)
                } else if orb > 5 {
                    offset = 2.0
                    newDay.addHours(hour: offset)
                } else if orb > 1.5 {
                    offset = 1.0
                    newDay.addHours(hour: offset)
                } else if orb > 0.5 {
                    offset = 40.0
                    newDay.addMinutes(minute: offset)
                } else if orb > 0.2 {
                    offset = 10.0
                    newDay.addMinutes(minute: offset)
                } else if orb > 0.1 {
                    offset = 5.0
                    newDay.addMinutes(minute: offset)
                } else {
                    offset = 1.0
                    newDay.addMinutes(minute: offset)
                }
            } else {
                // separateということは満月後
                // 求めるのは次の新月なのでapplyになるまでたくさん進める
                offset = 3.0
                newDay.addDays(day: offset)
            }

            let jd = s.swe_utc_to_jd(year: Int32(d.getUTCFullYear()), month: Int32(d.getUTCMonth()), day: Int32(d.getUTCDay()), hour: Int32(d.getUTCHour()), minute: Int32(d.getMinute()), second: Double(d.getSecond()))

            calc1 = s.calc_ut(jd: jd.dret[0], planetNo: 0, flag: Int32(flag))
            calc2 = s.calc_ut(jd: jd.dret[0], planetNo: 1, flag: Int32(flag))

            sunDegree = calc1.x[0]
            moonDegree = calc2.x[0]
            
            cnt = cnt + 1
            if cnt > 100 {
                print("100ごえ")
                break
            }
        }

        return newDay
    }
    
    public func getNewMoonMinus(d: MyDate, timezone: Double) -> MyDate
    {
        // SEFLG_SWIEPH: 2 SEFLG_SPEED: 256
        var flag = 258
        let jd = s.swe_utc_to_jd(year: Int32(d.getUTCFullYear()), month: Int32(d.getUTCMonth()), day: Int32(d.getUTCDay()), hour: Int32(d.getUTCHour()), minute: Int32(d.getMinute()), second: Double(d.getSecond()))

        var calc1: Calc
        var calc2: Calc
        if (configData.centric == ECentric.HELIO_CENTRIC)
        {
            flag |= SweConst.SEFLG_HELCTR.rawValue
        }
        if (configData.sideReal == ESideReal.SIDEREAL)
        {
            flag |= SweConst.SEFLG_SIDEREAL.rawValue
            s.swe_set_sid_mode(flag: SweSideReal.SE_SIDM_LAHIRI.rawValue)
            // ayanamsa計算
            let ut: Ayanamsa = s.swe_get_ayanamsa_ex_ut(tjd_ut: jd.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

            // Ephemeris Timeで計算, 結果はxに入る
            calc1 = s.calc_ut(jd: ut.daya, planetNo: 0, flag: Int32(flag))
            calc2 = s.calc_ut(jd: ut.daya, planetNo: 1, flag: Int32(flag))
        }
        else
        {
            calc1 = s.calc_ut(jd: jd.dret[0], planetNo: 0, flag: Int32(flag))
            calc2 = s.calc_ut(jd: jd.dret[0], planetNo: 1, flag: Int32(flag))
        }

        var sunDegree = calc1.x[0]
        var moonDegree = calc2.x[0]
        let newDay = d
        var offset: Double = 0.0

        // めっちゃ近い場合全く動かないので強制的に動かす
        if isIn(from: sunDegree, to: moonDegree, degree: 0, orb: 5.0) {
            offset = -1.0
            newDay.addDays(day: offset)

            // サイドリアルはサポート外でいい？
            let jd = s.swe_utc_to_jd(year: Int32(d.getUTCFullYear()), month: Int32(d.getUTCMonth()), day: Int32(d.getUTCDay()), hour: Int32(d.getUTCHour()), minute: Int32(d.getMinute()), second: Double(d.getSecond()))

            calc1 = s.calc_ut(jd: jd.dret[0], planetNo: 0, flag: Int32(flag))
            calc2 = s.calc_ut(jd: jd.dret[0], planetNo: 1, flag: Int32(flag))

            sunDegree = calc1.x[0]
            moonDegree = calc2.x[0]
        }

        var cnt = 0
        while (true) {
            let orb = getOrb(from: sunDegree, to: moonDegree)
            if (isIn(from: sunDegree, to: moonDegree, degree: 0, orb: 0.01)) {
                break
            }
            
            if (isApply(sunDegree: sunDegree, moonDegree: moonDegree)) {
                // applyということは新月手前
                // 求めるのは次じゃなくて前の新月なのでseparateになるまでたくさん戻す
                offset = -3
                offset = -3.0
                newDay.addDays(day: offset)
            } else {
                if orb > 80 {
                    offset = -3.0
                    newDay.addDays(day: offset)
                } else if orb > 20 {
                    offset = -1.0
                    newDay.addDays(day: offset)
                } else if orb > 10 {
                    offset = -6.0
                    newDay.addHours(hour: offset)
                } else if orb > 5 {
                    offset = -2.0
                    newDay.addHours(hour: offset)
                } else if orb > 1.5 {
                    offset = -1.0
                    newDay.addHours(hour: offset)
                } else if orb > 0.5 {
                    offset = -40.0
                    newDay.addMinutes(minute: offset)
                } else if orb > 0.2 {
                    offset = -10.0
                    newDay.addMinutes(minute: offset)
                } else if orb > 0.1 {
                    offset = -5.0
                    newDay.addMinutes(minute: offset)
                } else {
                    offset = -1.0
                    newDay.addMinutes(minute: offset)
                }
            }

            let jd = s.swe_utc_to_jd(year: Int32(d.getUTCFullYear()), month: Int32(d.getUTCMonth()), day: Int32(d.getUTCDay()), hour: Int32(d.getUTCHour()), minute: Int32(d.getMinute()), second: Double(d.getSecond()))

            calc1 = s.calc_ut(jd: jd.dret[0], planetNo: 0, flag: Int32(flag))
            calc2 = s.calc_ut(jd: jd.dret[0], planetNo: 1, flag: Int32(flag))

            sunDegree = calc1.x[0]
            moonDegree = calc2.x[0]
            
            cnt = cnt + 1
            if cnt > 100 {
                print("100ごえ")
                break
            }
        }

        return newDay
    }
    

    
    /// 満月計算
    /// - Parameters:
    ///   - d: 現在時刻
    ///   - timezone: timezone
    /// - Returns: MyDate
    public func getFullMoon(d: MyDate, timezone: Double) -> MyDate
    {
        // SEFLG_SWIEPH: 2 SEFLG_SPEED: 256
        var flag = 258
        let jd = s.swe_utc_to_jd(year: Int32(d.getUTCFullYear()), month: Int32(d.getUTCMonth()), day: Int32(d.getUTCDay()), hour: Int32(d.getUTCHour()), minute: Int32(d.getMinute()), second: Double(d.getSecond()))

        var calc1: Calc
        var calc2: Calc
        if (configData.centric == ECentric.HELIO_CENTRIC)
        {
            flag |= SweConst.SEFLG_HELCTR.rawValue
        }
        if (configData.sideReal == ESideReal.SIDEREAL)
        {
            flag |= SweConst.SEFLG_SIDEREAL.rawValue
            s.swe_set_sid_mode(flag: SweSideReal.SE_SIDM_LAHIRI.rawValue)
            // ayanamsa計算
            let ut: Ayanamsa = s.swe_get_ayanamsa_ex_ut(tjd_ut: jd.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

            // Ephemeris Timeで計算, 結果はxに入る
            calc1 = s.calc_ut(jd: ut.daya, planetNo: 0, flag: Int32(flag))
            calc2 = s.calc_ut(jd: ut.daya, planetNo: 1, flag: Int32(flag))
        }
        else
        {
            calc1 = s.calc_ut(jd: jd.dret[0], planetNo: 0, flag: Int32(flag))
            calc2 = s.calc_ut(jd: jd.dret[0], planetNo: 1, flag: Int32(flag))
        }

        var sunDegree = calc1.x[0]
        var moonDegree = calc2.x[0]
        let newDay = d
        var offset: Double = 0.0

        // めっちゃ近い場合全く動かないので強制的に動かす
        if isIn(from: sunDegree, to: moonDegree, degree: 180, orb: 5.0) {
            if isApply(sunDegree: sunDegree, moonDegree: moonDegree) {
                offset = 2.0
            } else {
                offset = 1.0
            }

            newDay.addDays(day: offset)

            // サイドリアルはサポート外でいい？
            let jd = s.swe_utc_to_jd(year: Int32(d.getUTCFullYear()), month: Int32(d.getUTCMonth()), day: Int32(d.getUTCDay()), hour: Int32(d.getUTCHour()), minute: Int32(d.getMinute()), second: Double(d.getSecond()))

            calc1 = s.calc_ut(jd: jd.dret[0], planetNo: 0, flag: Int32(flag))
            calc2 = s.calc_ut(jd: jd.dret[0], planetNo: 1, flag: Int32(flag))

            sunDegree = calc1.x[0]
            moonDegree = calc2.x[0]
        }

        var cnt = 0
        while (true) {
            let orb = getOrb(from: sunDegree, to: moonDegree)
            if (isIn(from: sunDegree, to: moonDegree, degree: 180, orb: 0.05)) {
                break
            }
            
            if isApply(sunDegree: sunDegree, moonDegree: moonDegree) {
                // applyということは満月すぎ
                // 求めるのは前じゃなくて次の満月なのでseparateになるまでたくさん進める
                offset = 3.0
                newDay.addDays(day: offset)
            } else {
                if orb > 179
                {
                    offset = 7
                    newDay.addMinutes(minute: offset)
                }
                else if orb > 177 {
                    offset = 20
                    newDay.addMinutes(minute: offset)
                }
                else if orb > 174 {
                    offset = 1
                    newDay.addHours(hour: offset)
                }
                else if orb > 165 {
                    offset = 3
                    newDay.addHours(hour: offset)
                }
                else if orb > 130 {
                    offset = 1
                    newDay.addDays(day: offset)
                }
                else {
                    offset = 2
                    newDay.addDays(day: offset)
                }
            }

            let jd = s.swe_utc_to_jd(year: Int32(d.getUTCFullYear()), month: Int32(d.getUTCMonth()), day: Int32(d.getUTCDay()), hour: Int32(d.getUTCHour()), minute: Int32(d.getMinute()), second: Double(d.getSecond()))

            calc1 = s.calc_ut(jd: jd.dret[0], planetNo: 0, flag: Int32(flag))
            calc2 = s.calc_ut(jd: jd.dret[0], planetNo: 1, flag: Int32(flag))

            sunDegree = calc1.x[0]
            moonDegree = calc2.x[0]
            
            cnt = cnt + 1
            if cnt > 100 {
                print("100ごえ")
                break
            }
        }

        return newDay
    }
    
    /// 満月計算マイナス
    /// - Parameters:
    ///   - d: 現在時刻
    ///   - timezone: timezone
    /// - Returns: MyDate
    public func getFullMoonMinus(d: MyDate, timezone: Double) -> MyDate
    {
        // SEFLG_SWIEPH: 2 SEFLG_SPEED: 256
        var flag = 258
        let jd = s.swe_utc_to_jd(year: Int32(d.getUTCFullYear()), month: Int32(d.getUTCMonth()), day: Int32(d.getUTCDay()), hour: Int32(d.getUTCHour()), minute: Int32(d.getMinute()), second: Double(d.getSecond()))

        var calc1: Calc
        var calc2: Calc
        if (configData.centric == ECentric.HELIO_CENTRIC)
        {
            flag |= SweConst.SEFLG_HELCTR.rawValue
        }
        if (configData.sideReal == ESideReal.SIDEREAL)
        {
            flag |= SweConst.SEFLG_SIDEREAL.rawValue
            s.swe_set_sid_mode(flag: SweSideReal.SE_SIDM_LAHIRI.rawValue)
            // ayanamsa計算
            let ut: Ayanamsa = s.swe_get_ayanamsa_ex_ut(tjd_ut: jd.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

            // Ephemeris Timeで計算, 結果はxに入る
            calc1 = s.calc_ut(jd: ut.daya, planetNo: 0, flag: Int32(flag))
            calc2 = s.calc_ut(jd: ut.daya, planetNo: 1, flag: Int32(flag))
        }
        else
        {
            calc1 = s.calc_ut(jd: jd.dret[0], planetNo: 0, flag: Int32(flag))
            calc2 = s.calc_ut(jd: jd.dret[0], planetNo: 1, flag: Int32(flag))
        }

        var sunDegree = calc1.x[0]
        var moonDegree = calc2.x[0]
        let newDay = d
        var offset: Double = 0.0

        // めっちゃ近い場合全く動かないので強制的に動かす
        if isIn(from: sunDegree, to: moonDegree, degree: 180, orb: 5) {
            if isApply(sunDegree: sunDegree, moonDegree: moonDegree) {
                offset = -2.0
            } else {
                offset = -1.0
            }
            newDay.addDays(day: offset)

            // サイドリアルはサポート外でいい？
            let jd = s.swe_utc_to_jd(year: Int32(d.getUTCFullYear()), month: Int32(d.getUTCMonth()), day: Int32(d.getUTCDay()), hour: Int32(d.getUTCHour()), minute: Int32(d.getMinute()), second: Double(d.getSecond()))

            calc1 = s.calc_ut(jd: jd.dret[0], planetNo: 0, flag: Int32(flag))
            calc2 = s.calc_ut(jd: jd.dret[0], planetNo: 1, flag: Int32(flag))

            sunDegree = calc1.x[0]
            moonDegree = calc2.x[0]
        }

        var cnt = 0
        while (true) {
            let orb = getOrb(from: sunDegree, to: moonDegree)
            if isIn(from: sunDegree, to: moonDegree, degree: 180, orb: 0.01) {
                break
            }
            
            if isApply(sunDegree: sunDegree, moonDegree: moonDegree) {
                // applyということは満月過ぎ
                if orb > 179.9 {
                    offset = -1.0
                    newDay.addMinutes(minute: offset)
                } else if orb > 179 {
                    offset = -10.0
                    newDay.addMinutes(minute: offset)
                } else if orb > 178 {
                    offset = -20.0
                    newDay.addMinutes(minute: offset)
                } else if orb > 175 {
                    offset = -1.0
                    newDay.addHours(hour: offset)
                } else if orb > 170 {
                    offset = -3.0
                    newDay.addHours(hour: offset)
                } else if orb > 130 {
                    offset = -1.0
                    newDay.addDays(day: offset)
                } else {
                    offset = -3.0
                    newDay.addDays(day: offset)
                }
            } else {
                // separate、満月に近づく
                // 求めるのは前の満月
                offset = -3.0
                newDay.addDays(day: offset)
            }

            let jd = s.swe_utc_to_jd(year: Int32(newDay.getUTCFullYear()), month: Int32(newDay.getUTCMonth()), day: Int32(newDay.getUTCDay()), hour: Int32(newDay.getUTCHour()), minute: Int32(newDay.getMinute()), second: Double(newDay.getSecond()))

            calc1 = s.calc_ut(jd: jd.dret[0], planetNo: 0, flag: Int32(flag))
            calc2 = s.calc_ut(jd: jd.dret[0], planetNo: 1, flag: Int32(flag))

            sunDegree = calc1.x[0]
            moonDegree = calc2.x[0]
            
            
            cnt = cnt + 1
            if cnt > 100 {
                print("100ごえ")
                break
            }
        }

        return newDay
    }
    


//
//            public DateTime GetFullMoonMinus(DateTime date, double timezone)
//            {
//                AppDelegate appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
//                Dictionary<int, PlanetData> planetList = new Dictionary<int, PlanetData>();
//
//                //          s.swe_set_ephe_path(path);
//                int utc_year = 0;
//                int utc_month = 0;
//                int utc_day = 0;
//                int utc_hour = 0;
//                int utc_minute = 0;
//                double utc_second = 0;
//                double[] dret = { 0.0, 0.0 };
//                double[] x = new double[20];
//                double[] x1 = new double[20];
//                double[] x2 = new double[20];
//                string serr = "";
//
//                s.swe_utc_time_zone(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, timezone,
//                                    ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
//                s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
//
//                int flag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
//                if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;
//
//                s.swe_calc(dret[1], 0, flag, x1, ref serr);
//                double sunDegree = x1[0];
//                s.swe_calc(dret[1], 1, flag, x2, ref serr);
//                double moonDegree = x2[0];
//                DateTime newday = date;
//                int offset = 0;
//
//                //誤差があまりにも近い(すでに満月)=次の満月を計算
//                if (isIn(sunDegree, moonDegree, 180, 5))
//                {
//                    if (isApply(sunDegree, moonDegree))
//                    {
//                        offset = -1;
//                    }
//                    else
//                    {
//                        offset = -1;
//                    }
//                    newday = newday.AddDays(offset);
//
//                    s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, timezone,
//    ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
//                    s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
//
//                    if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;
//                    s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
//                    s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);
//
//
//                    sunDegree = x1[0];
//                    moonDegree = x2[0];
//
//                }
//
//
//                int cnt = 0;
//                while (true)
//                {
//                    double orb = GetOrb(sunDegree, moonDegree);
//                    if (isIn(sunDegree, moonDegree, 180, 0.01))
//                    {
//                        break;
//                    }
//
//                    Debug.WriteLine(orb);
//                    if (isApply(sunDegree, moonDegree))
//                    {
//                        // applyということは満月過ぎ
//                        if (orb > 179.9)
//                        {
//                            offset = -1;
//                            newday = newday.AddMinutes(offset);
//                        }
//                        else if (orb > 179)
//                        {
//                            offset = -10;
//                            newday = newday.AddMinutes(offset);
//                        }
//                        else if (orb > 178)
//                        {
//                            offset = -20;
//                            newday = newday.AddMinutes(offset);
//                        }
//                        else if (orb > 175)
//                        {
//                            offset = -1;
//                            newday = newday.AddHours(offset);
//                        }
//                        else if (orb > 170)
//                        {
//                            offset = -3;
//                            newday = newday.AddHours(offset);
//                        }
//                        else if (orb > 130)
//                        {
//                            offset = -1;
//                            newday = newday.AddDays(offset);
//                        }
//                        else
//                        {
//                            offset = -3;
//                            newday = newday.AddDays(offset);
//                        }
//                    }
//                    else
//                    {
//                        offset = -3;
//                        newday = newday.AddDays(offset);
//                    }
//
//                    s.swe_utc_time_zone(newday.Year, newday.Month, newday.Day, newday.Hour, newday.Minute, newday.Second, timezone,
//    ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
//                    s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);
//
//                    if (configData.centric == ECentric.HELIO_CENTRIC) flag |= SwissEph.SEFLG_HELCTR;
//                    s.swe_calc_ut(dret[1], 0, flag, x1, ref serr);
//                    s.swe_calc_ut(dret[1], 1, flag, x2, ref serr);
//
//
//                    sunDegree = x1[0];
//                    moonDegree = x2[0];
//
//                    cnt++;
//                    if (cnt > 100)
//                    {
//                        NSAlert alert = new NSAlert();
//                        alert.MessageText = "100ごえ";
//                        alert.RunModal();
//
//                        break;
//                    }
//                }
//
//                Debug.WriteLine(cnt);
//                Debug.WriteLine(newday);
//                Debug.WriteLine(moonDegree);
//                return newday;
//            }
//
//
    public func isIn(from: Double, to: Double, degree: Double, orb: Double) -> Bool
    {
        var calc = getOrb(from: from, to: to)
        if degree - orb < calc && calc < degree + orb {
            return true
        }
        
        return false
    }
    
    public func getOrb(from: Double, to: Double) -> Double
    {
        var calc = abs(to - from)
        if calc > 180 {
            calc = 360 - calc
        }
        
        return calc
    }

    
    /// 太陽と月のアプライ/セパレート
    /// 常に月のほうが早い、かつ順行しかないので度数だけで判断可能
    /// - Parameters:
    ///   - sunDegree: 太陽度数
    ///   - moonDegree: 月の度数
    /// - Returns: アプライならtrue
    public func isApply(sunDegree: Double, moonDegree: Double) -> Bool
    {
        // 太陽が50度の場合230度以上ならtrue
        // もちろん0〜49度でもtrue
        if sunDegree < 180 {
            var mid = sunDegree + 180
            if moonDegree < sunDegree {
                return true
            }
            if mid < moonDegree {
                return true
            }
            return false
        } else {
            // 太陽が300度の場合301〜359ならfalse(セパレート)
            // 120〜299度の場合はtrue(アプライ)
            // 0〜119度の場合はfalse(セパレート)
            var mid = (sunDegree + 180).truncatingRemainder(dividingBy: 360.0)
            if moonDegree < mid {
                return false
            }
            if sunDegree < moonDegree {
                return false
            }
            return true
        }
    }
}
