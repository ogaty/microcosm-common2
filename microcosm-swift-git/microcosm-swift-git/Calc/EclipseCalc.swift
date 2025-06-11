//
//  EclipseCalc.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/02/15.
//

import Foundation

import swissEphemeris
import AppKit

class EclipseCalc {
    
    /// 次の合を調べる
    /// - Parameters:
    ///   - begin: 開始時刻
    ///   - timezone: タイムゾーン
    ///   - planetId: 対象天体
    ///   - targetDegree: 対象度数
    ///   - isForward: beginより後か前か
    ///   - config: config(delegateから取るとunittest書きづらくなるので渡す)
    /// - Returns: 結果時刻
    public func GetEclipse(begin: MyDate, timezone: Double, planetId: Int, targetDegree: Double, isForward: Bool, config: ConfigData) -> MyDate
    {
        let swiss = swissEphemerisMain()

        // 過去方向の場合、一度戻してから前へ進ませる
        if (planetId == EPlanets.SUN.rawValue)
        {
            if (!isForward)
            {
                begin.addDays(day: -366)
            }
        }
        else if (planetId == EPlanets.MOON.rawValue)
        {
            if (!isForward)
            {
                begin.addDays(day: -28)
            }
        }

        var julian = swiss.swe_utc_to_jd(year: Int32(begin.getUTCFullYear()), month: Int32(begin.getUTCMonth()), day: Int32(begin.getUTCDay()), hour: Int32(begin.getUTCHour()), minute: Int32(begin.getMinute()), second: Double(begin.getSecond()))

        // SEFLG_SWIEPH: 2 SEFLG_SPEED: 256
        var flag = 258
        if (config.centric == ECentric.HELIO_CENTRIC)
        {
            flag |= SweConst.SEFLG_HELCTR.rawValue
        }
        if (config.sideReal == ESideReal.SIDEREAL)
        {
            flag |= SweConst.SEFLG_SIDEREAL.rawValue
            swiss.swe_set_sid_mode(flag: SweSideReal.SE_SIDM_LAHIRI.rawValue)
        }

        var calc: Calc
        if (config.sideReal == ESideReal.SIDEREAL)
        {
            // ayanamsa計算
            let ut: Ayanamsa = swiss.swe_get_ayanamsa_ex_ut(tjd_ut: julian.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

            // Ephemeris Timeで計算, 結果はxに入る
            calc = swiss.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
        }
        else
        {
            // Universal Timeで計算, 結果はxに入る
            calc = swiss.calc_ut(jd: julian.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
        }

        var calcCounter = 0
        var offset: Double
        var newDay = begin
        if (planetId == EPlanets.SUN.rawValue)
        {
            // 太陽は逆行無いので単純
            // 324.12345
            // 324.12336
            // ぐらいの精度とする(AMATERUと比べると30秒くらいの誤差)
            // 3度差なら3日足す
            offset = targetDegree - calc.x[0]
            if (offset < 0)
            {
                // 現在280度、対象が270度だった場合は350度のずれ、来年の10日前くらいにざっくりセット
                offset += 360
            }
            var days = Int(offset)
            newDay.addDays(day: Double(days))

            julian = swiss.swe_utc_to_jd(year: Int32(newDay.getUTCFullYear()), month: Int32(newDay.getUTCMonth()), day: Int32(newDay.getUTCDay()), hour: Int32(newDay.getUTCHour()), minute: Int32(newDay.getMinute()), second: Double(newDay.getSecond()))
            calc = swiss.calc_ut(jd: julian.dret[0], planetNo: Int32(planetId), flag: Int32(flag))

            calcCounter = calcCounter + 1

            // 1度以内のはず
//            Debug.WriteLine(targetDegree);
//            Debug.WriteLine(x[0]);

            var calcDegree = calc.x[0]

            // 対象度数が0でcalcDegreeは359なのでおかしくなるので
            if (targetDegree < 1)
            {
                while (calcDegree > 1)
                {
                    if (calcDegree < 358)
                    {
                        offset = 358 - calcDegree;
                        newDay.addDays(day: offset)
                    }
                    else if (calcDegree < 359.5)
                    {
                        offset = 12
                        newDay.addHours(hour: offset)
                    }
                    else if (calcDegree < 359.7)
                    {
                        // 0.25 = 6時間
                        offset = 6
                        newDay.addHours(hour: offset)
                    }
                    else if (calcDegree < 359.9)
                    {
                        offset = 1
                        newDay.addHours(hour: offset)
                    }
                    else if (calcDegree < 359.95)
                    {
                        // ここからは分単位で
                        offset = 25
                        newDay.addMinutes(minute: offset)
                    }
                    else if (calcDegree < 359.99)
                    {
                        offset = 7
                        newDay.addMinutes(minute: offset)
                    }
                    else
                    {
                        offset = 45;
                        newDay.addSeconds(second: offset)
                    }

                    julian = swiss.swe_utc_to_jd(year: Int32(newDay.getUTCFullYear()), month: Int32(newDay.getUTCMonth()), day: Int32(newDay.getUTCDay()), hour: Int32(newDay.getUTCHour()), minute: Int32(newDay.getMinute()), second: Double(newDay.getSecond()))
                    if (config.sideReal == ESideReal.SIDEREAL)
                    {
                        // ayanamsa計算
                        let ut: Ayanamsa = swiss.swe_get_ayanamsa_ex_ut(tjd_ut: julian.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

                        // Ephemeris Timeで計算, 結果はxに入る
                        calc = swiss.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
                    }
                    else
                    {
                        // Universal Timeで計算, 結果はxに入る
                        calc = swiss.calc_ut(jd: julian.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
                    }

                    calcCounter = calcCounter + 1
//                    Debug.WriteLine(targetDegree);
//                    Debug.WriteLine(x[0]);
                    calcDegree = calc.x[0]
                    if (calcCounter > 100)
                    {
                        print("100ごえ")
                        break
                    }
                }
            }
            else // 1度以上離れている
            {
                while (targetDegree - calcDegree > 0)
                {
                    if (abs(targetDegree - calcDegree) > 0.5)
                    {
                        // 0.5 = 12時間ずらす
                        if (targetDegree - calcDegree < 0)
                        {
                            offset = -12
                        }
                        else
                        {
                            offset = 12
                        }
                        newDay.addHours(hour: offset)
                    }
                    else if (abs(targetDegree - calcDegree) > 0.3)
                    {
                        // 0.25 = 6時間
                        if (targetDegree - calcDegree < 0)
                        {
                            offset = -6
                        }
                        else
                        {
                            offset = 6
                        }

                        newDay.addHours(hour: offset)
                    }
                    else if (abs(targetDegree - calcDegree) > 0.1)
                    {
                        // 0.04 = 1時間
                        if (targetDegree - calcDegree < 0)
                        {
                            offset = -1
                        }
                        else
                        {
                            offset = 1
                        }

                        newDay.addHours(hour: offset)
                    }
                    else if (abs(targetDegree - calcDegree) > 0.05)
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

                        newDay.addMinutes(minute: offset)
                    }
                    else if (abs(targetDegree - calcDegree) > 0.01)
                    {
                        // ここからは分単位で

                        if (targetDegree - calcDegree < 0)
                        {
                            offset = -7
                        }
                        else
                        {
                            offset = 7
                        }

                        newDay.addMinutes(minute: offset)
                    }
                    else if (abs(targetDegree - calcDegree) > 0.003)
                    {
                        // 分
                        if (targetDegree - calcDegree < 0)
                        {
                            offset = -1
                        }
                        else
                        {
                            offset = 1
                        }

                        newDay.addMinutes(minute: offset)
                    }
                    else if (abs(targetDegree - calcDegree) > 0.001)
                    {
                        if (targetDegree - calcDegree < 0)
                        {
                            offset = -45
                        }
                        else
                        {
                            offset = 45
                        }

                        newDay.addSeconds(second: offset)
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

                        newDay.addSeconds(second: offset)
                    }

//                    Debug.WriteLine(newDay.ToString());


                    julian = swiss.swe_utc_to_jd(year: Int32(newDay.getUTCFullYear()), month: Int32(newDay.getUTCMonth()), day: Int32(newDay.getUTCDay()), hour: Int32(newDay.getUTCHour()), minute: Int32(newDay.getMinute()), second: Double(newDay.getSecond()))
                    if (config.sideReal == ESideReal.SIDEREAL)
                    {
                        // ayanamsa計算
                        let ut: Ayanamsa = swiss.swe_get_ayanamsa_ex_ut(tjd_ut: julian.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

                        // Ephemeris Timeで計算, 結果はxに入る
                        calc = swiss.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
                    }
                    else
                    {
                        // Universal Timeで計算, 結果はxに入る
                        calc = swiss.calc_ut(jd: julian.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
                    }

                    calcCounter = calcCounter + 1

//                    Debug.WriteLine(targetDegree);
//                    Debug.WriteLine(x[0]);

                    calcDegree = calc.x[0]

                    if (calcCounter > 100)
                    {
                        print("100ごえ")
                        break
                    }

                }

            }
            
            return newDay
        }
        else if (planetId == EPlanets.MOON.rawValue)
        {
            // 月も逆行無いので単純
            var calcDegree = calc.x[0]

            //誤差があまりにも近い=次の角度を計算
            if (isIn(from: targetDegree, to: calcDegree, degree: 0, orb: 0.4))
            {
                if (isApply(sunDegree: targetDegree, moonDegree: calcDegree))
                {
                    offset = 2
                }
                else
                {
                    offset = 1
                }
                newDay.addDays(day: offset)

                var julian = swiss.swe_utc_to_jd(year: Int32(newDay.getUTCFullYear()), month: Int32(newDay.getUTCMonth()), day: Int32(newDay.getUTCDay()), hour: Int32(newDay.getUTCHour()), minute: Int32(newDay.getMinute()), second: Double(newDay.getSecond()))
                if (config.sideReal == ESideReal.SIDEREAL)
                {
                    // ayanamsa計算
                    let ut: Ayanamsa = swiss.swe_get_ayanamsa_ex_ut(tjd_ut: julian.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

                    // Ephemeris Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
                }
                else
                {
                    // Universal Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: julian.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
                }

                calcDegree = calc.x[0]
            }
            
            var calcCounter = 0
            while (true)
            {
                var orb = GetOrb(from: targetDegree, to: calcDegree)
                if (isIn(from: targetDegree, to: calcDegree, degree: 0, orb: 0.005))
                {
                    break
                }
                
                if (isApply(sunDegree: targetDegree, moonDegree: calcDegree))
                {
                    // applyということは満月過ぎ
                    if (orb < 0.2)
                    {
                        offset = 2
                        newDay.addMinutes(minute: offset)
                    }
                    else if (orb < 1)
                    {
                        offset = 15
                        newDay.addMinutes(minute: offset)
                    }
                    else if (orb < 3)
                    {
                        offset = 1
                        newDay.addHours(hour: offset)
                    }
                    else if (orb < 10)
                    {
                        offset = 3
                        newDay.addHours(hour: offset)
                    }
                    else if (orb < 60)
                    {
                        offset = 1
                        newDay.addDays(day: offset)
                    }
                    else
                    {
                        offset = 3
                        newDay.addDays(day: offset)
                    }
                }
                else 
                {
                    offset = 3
                    newDay.addDays(day: offset)
                }

                julian = swiss.swe_utc_to_jd(year: Int32(newDay.getUTCFullYear()), month: Int32(newDay.getUTCMonth()), day: Int32(newDay.getUTCDay()), hour: Int32(newDay.getUTCHour()), minute: Int32(newDay.getMinute()), second: Double(newDay.getSecond()))
                if (config.sideReal == ESideReal.SIDEREAL)
                {
                    // ayanamsa計算
                    let ut: Ayanamsa = swiss.swe_get_ayanamsa_ex_ut(tjd_ut: julian.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

                    // Ephemeris Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
                }
                else
                {
                    // Universal Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: julian.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
                }

                calcCounter = calcCounter + 1
                calcDegree = calc.x[0]
                
                print(calcDegree)
                
                if (calcCounter > 100)
                {
                    print("100ごえ")
                    break
                }
            }

            return newDay
        }
        else if (planetId == EPlanets.MERCURY.rawValue)
        {
//            // 水星はまずは大雑把でいいや
//            // 逆行してすぐ戻る可能性もあるのよね
//            // だからマイナスオフセットはなしとする
            var calcDegree = calc.x[0]
            var offset = targetDegree - calc.x[0]
            
            if (abs(offset) < 2)
            {
                // すでに回帰時刻の場合
                // とりあえず何日か進めておく
                // 過去方向はすでにマイナスしているのでそのまま
                if (isForward)
                {
                    offset += 3
                }
                newDay.addDays(day: offset)

                julian = swiss.swe_utc_to_jd(year: Int32(newDay.getUTCFullYear()), month: Int32(newDay.getUTCMonth()), day: Int32(newDay.getUTCDay()), hour: Int32(newDay.getUTCHour()), minute: Int32(newDay.getMinute()), second: Double(newDay.getSecond()))
                if (config.sideReal == ESideReal.SIDEREAL)
                {
                    // ayanamsa計算
                    let ut: Ayanamsa = swiss.swe_get_ayanamsa_ex_ut(tjd_ut: julian.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

                    // Ephemeris Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
                }
                else
                {
                    // Universal Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: julian.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
                }
            }
            
            // targetが後ろにあるから0度までcurrentを進める
            // 戻しちゃだめ
            while (targetDegree - calcDegree < 0)
            {
                if (calcDegree > 350)
                {
                    offset = 2
                }
                else
                {
                    offset = 20
                }

                newDay.addDays(day: offset)

                julian = swiss.swe_utc_to_jd(year: Int32(newDay.getUTCFullYear()), month: Int32(newDay.getUTCMonth()), day: Int32(newDay.getUTCDay()), hour: Int32(newDay.getUTCHour()), minute: Int32(newDay.getMinute()), second: Double(newDay.getSecond()))
                if (config.sideReal == ESideReal.SIDEREAL)
                {
                    // ayanamsa計算
                    let ut: Ayanamsa = swiss.swe_get_ayanamsa_ex_ut(tjd_ut: julian.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

                    // Ephemeris Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
                }
                else
                {
                    // Universal Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: julian.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
                }

                calcDegree = calc.x[0]
            }
            
            while (abs(targetDegree - calcDegree) > 0.1)
            {
                if (abs(targetDegree - calcDegree) > 30)
                {
                    offset = 240
                    newDay.addDays(day: offset)
                }
                else if (abs(targetDegree - calcDegree) > 1)
                {
                    offset = 24
                    newDay.addDays(day: offset)
                }
                else
                {
                    offset = 1
                    newDay.addDays(day: offset)
                }
                
                julian = swiss.swe_utc_to_jd(year: Int32(newDay.getUTCFullYear()), month: Int32(newDay.getUTCMonth()), day: Int32(newDay.getUTCDay()), hour: Int32(newDay.getUTCHour()), minute: Int32(newDay.getMinute()), second: Double(newDay.getSecond()))
                if (config.sideReal == ESideReal.SIDEREAL)
                {
                    // ayanamsa計算
                    let ut: Ayanamsa = swiss.swe_get_ayanamsa_ex_ut(tjd_ut: julian.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

                    // Ephemeris Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
                }
                else
                {
                    // Universal Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: julian.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
                }

                calcDegree = calc.x[0]
            }
        }
        else if (planetId == EPlanets.VENUS.rawValue)
        {
            // 金星
            var calcDegree = calc.x[0]
            var offset = targetDegree - calc.x[0]

            if (abs(offset) < 2)
            {
                // すでに回帰時刻の場合
                // とりあえず何日か進めておく
                // 過去方向はすでにマイナスしているのでそのまま
                if (isForward)
                {
                    offset += 3
                }
            }

            // targetが後ろにあるから0度までcurrentを進める
            // 戻しちゃだめ
            while (targetDegree - calcDegree < 0)
            {
                if (calcDegree > 350)
                {
                    offset = 2
                }
                else
                {
                    offset = 20
                }
                newDay.addDays(day: offset)

                julian = swiss.swe_utc_to_jd(year: Int32(newDay.getUTCFullYear()), month: Int32(newDay.getUTCMonth()), day: Int32(newDay.getUTCDay()), hour: Int32(newDay.getUTCHour()), minute: Int32(newDay.getMinute()), second: Double(newDay.getSecond()))
                if (config.sideReal == ESideReal.SIDEREAL)
                {
                    // ayanamsa計算
                    let ut: Ayanamsa = swiss.swe_get_ayanamsa_ex_ut(tjd_ut: julian.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

                    // Ephemeris Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
                }
                else
                {
                    // Universal Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: julian.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
                }
                calcDegree = calc.x[0]
            }


            while (targetDegree - calcDegree < 0)
            {
                if (calcDegree > 350)
                {
                    offset = 2
                }
                else
                {
                    offset = 30
                }

                julian = swiss.swe_utc_to_jd(year: Int32(newDay.getUTCFullYear()), month: Int32(newDay.getUTCMonth()), day: Int32(newDay.getUTCDay()), hour: Int32(newDay.getUTCHour()), minute: Int32(newDay.getMinute()), second: Double(newDay.getSecond()))
                if (config.sideReal == ESideReal.SIDEREAL)
                {
                    // ayanamsa計算
                    let ut: Ayanamsa = swiss.swe_get_ayanamsa_ex_ut(tjd_ut: julian.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

                    // Ephemeris Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
                }
                else
                {
                    // Universal Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: julian.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
                }
                
                calcCounter = calcCounter + 1
                calcDegree = calc.x[0]
                
                if (calcCounter > 100)
                {
                    print("100ごえ")
                    break
                }
            }

            while (abs(targetDegree - calcDegree) > 0.1)
            {
                if (abs(targetDegree - calcDegree) > 30)
                {
                    offset = 240
                    newDay.addDays(day: offset)
                }
                else if (abs(targetDegree - calcDegree) > 1)
                {
                    offset = 24
                    newDay.addDays(day: offset)
                }
                else
                {
                    offset = 1
                    newDay.addDays(day: offset)
                }
                
                julian = swiss.swe_utc_to_jd(year: Int32(newDay.getUTCFullYear()), month: Int32(newDay.getUTCMonth()), day: Int32(newDay.getUTCDay()), hour: Int32(newDay.getUTCHour()), minute: Int32(newDay.getMinute()), second: Double(newDay.getSecond()))
                if (config.sideReal == ESideReal.SIDEREAL)
                {
                    // ayanamsa計算
                    let ut: Ayanamsa = swiss.swe_get_ayanamsa_ex_ut(tjd_ut: julian.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

                    // Ephemeris Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
                }
                else
                {
                    // Universal Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: julian.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
                }

                calcDegree = calc.x[0]
                if (calcCounter > 100)
                {
                    print("100ごえ")
                    break
                }
            }
        }
        else if (planetId == EPlanets.MARS.rawValue)
        {
            // 火星
            // 2年以上の周期
            var calcDegree = calc.x[0]
            var offset = targetDegree - calc.x[0]

            if (abs(offset) < 2)
            {
                // すでに回帰時刻の場合
                // とりあえず何日か進めておく
                // 過去方向はすでにマイナスしているのでそのまま
                if (isForward)
                {
                    offset += 700
                }
            }

            // targetが後ろにあるから0度までcurrentを進める
            // 戻しちゃだめ
            while (targetDegree - calcDegree < 0)
            {
                if (calcDegree > 350)
                {
                    offset = 20
                }
                else
                {
                    offset = 100
                }
                newDay.addDays(day: offset)

                julian = swiss.swe_utc_to_jd(year: Int32(newDay.getUTCFullYear()), month: Int32(newDay.getUTCMonth()), day: Int32(newDay.getUTCDay()), hour: Int32(newDay.getUTCHour()), minute: Int32(newDay.getMinute()), second: Double(newDay.getSecond()))
                if (config.sideReal == ESideReal.SIDEREAL)
                {
                    // ayanamsa計算
                    let ut: Ayanamsa = swiss.swe_get_ayanamsa_ex_ut(tjd_ut: julian.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

                    // Ephemeris Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
                }
                else
                {
                    // Universal Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: julian.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
                }
                calcDegree = calc.x[0]
            }

            while (abs(targetDegree - calcDegree) > 1)
            {
                if (abs(targetDegree - calcDegree) > 100)
                {
                    offset = 150
                    newDay.addDays(day: offset)
                }
                else if (abs(targetDegree - calcDegree) > 30)
                {
                    offset = 30
                    newDay.addDays(day: offset)
                }
                else if (abs(targetDegree - calcDegree) > 10)
                {
                    offset = 8
                    newDay.addDays(day: offset)
                }
                else
                {
                    offset = 1
                    newDay.addDays(day: offset)
                }
                
                julian = swiss.swe_utc_to_jd(year: Int32(newDay.getUTCFullYear()), month: Int32(newDay.getUTCMonth()), day: Int32(newDay.getUTCDay()), hour: Int32(newDay.getUTCHour()), minute: Int32(newDay.getMinute()), second: Double(newDay.getSecond()))
                if (config.sideReal == ESideReal.SIDEREAL)
                {
                    // ayanamsa計算
                    let ut: Ayanamsa = swiss.swe_get_ayanamsa_ex_ut(tjd_ut: julian.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

                    // Ephemeris Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
                }
                else
                {
                    // Universal Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: julian.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
                }

                calcDegree = calc.x[0]
                if (calcCounter > 200)
                {
                    print("200ごえ")
                    break
                }
            }
        }
        else if (planetId == EPlanets.JUPITER.rawValue)
        {
            // 木星
            // 12年
            var calcDegree = calc.x[0]
            var offset = targetDegree - calc.x[0]

            if (abs(offset) < 2)
            {
                // すでに回帰時刻の場合
                // とりあえず何日か進めておく
                // 過去方向はすでにマイナスしているのでそのまま
                if (isForward)
                {
                    offset += 365 * 12
                }
            }

            // targetが後ろにあるから0度までcurrentを進める
            // 戻しちゃだめ
            while (targetDegree - calcDegree < 0)
            {
                if (calcDegree > 350)
                {
                    offset = 30
                }
                else
                {
                    offset = 365
                }
                newDay.addDays(day: offset)

                julian = swiss.swe_utc_to_jd(year: Int32(newDay.getUTCFullYear()), month: Int32(newDay.getUTCMonth()), day: Int32(newDay.getUTCDay()), hour: Int32(newDay.getUTCHour()), minute: Int32(newDay.getMinute()), second: Double(newDay.getSecond()))
                if (config.sideReal == ESideReal.SIDEREAL)
                {
                    // ayanamsa計算
                    let ut: Ayanamsa = swiss.swe_get_ayanamsa_ex_ut(tjd_ut: julian.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

                    // Ephemeris Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
                }
                else
                {
                    // Universal Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: julian.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
                }
                calcDegree = calc.x[0]
            }

            while (abs(targetDegree - calcDegree) > 1)
            {
                if (abs(targetDegree - calcDegree) > 100)
                {
                    offset = 365
                    newDay.addDays(day: offset)
                }
                else if (abs(targetDegree - calcDegree) > 30)
                {
                    offset = 100
                    newDay.addDays(day: offset)
                }
                else if (abs(targetDegree - calcDegree) > 10)
                {
                    offset = 20
                    newDay.addDays(day: offset)
                }
                else if (abs(targetDegree - calcDegree) > 2)
                {
                    offset = 5
                    newDay.addDays(day: offset)
                }
                else
                {
                    offset = 1
                    newDay.addDays(day: offset)
                }
                
                julian = swiss.swe_utc_to_jd(year: Int32(newDay.getUTCFullYear()), month: Int32(newDay.getUTCMonth()), day: Int32(newDay.getUTCDay()), hour: Int32(newDay.getUTCHour()), minute: Int32(newDay.getMinute()), second: Double(newDay.getSecond()))
                if (config.sideReal == ESideReal.SIDEREAL)
                {
                    // ayanamsa計算
                    let ut: Ayanamsa = swiss.swe_get_ayanamsa_ex_ut(tjd_ut: julian.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

                    // Ephemeris Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
                }
                else
                {
                    // Universal Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: julian.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
                }

                calcDegree = calc.x[0]
                if (calcCounter > 200)
                {
                    print("200ごえ")
                    break
                }
            }
        }
        else if (planetId == EPlanets.SATURN.rawValue)
        {
            // 土星
            // 28年
            var calcDegree = calc.x[0]
            var offset = targetDegree - calc.x[0]

            if (abs(offset) < 2)
            {
                // すでに回帰時刻の場合
                // とりあえず何日か進めておく
                // 過去方向はすでにマイナスしているのでそのまま
                if (isForward)
                {
                    offset += 365 * 28
                }
            }

            // targetが後ろにあるから0度までcurrentを進める
            // 戻しちゃだめ
            while (targetDegree - calcDegree < 0)
            {
                if (calcDegree > 350)
                {
                    offset = 30
                }
                else
                {
                    offset = 365
                }
                newDay.addDays(day: offset)

                julian = swiss.swe_utc_to_jd(year: Int32(newDay.getUTCFullYear()), month: Int32(newDay.getUTCMonth()), day: Int32(newDay.getUTCDay()), hour: Int32(newDay.getUTCHour()), minute: Int32(newDay.getMinute()), second: Double(newDay.getSecond()))
                if (config.sideReal == ESideReal.SIDEREAL)
                {
                    // ayanamsa計算
                    let ut: Ayanamsa = swiss.swe_get_ayanamsa_ex_ut(tjd_ut: julian.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

                    // Ephemeris Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
                }
                else
                {
                    // Universal Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: julian.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
                }
                calcDegree = calc.x[0]
            }

            while (abs(targetDegree - calcDegree) > 1)
            {
                if (abs(targetDegree - calcDegree) > 100)
                {
                    offset = 365 * 2
                    newDay.addDays(day: offset)
                }
                else if (abs(targetDegree - calcDegree) > 30)
                {
                    offset = 200
                    newDay.addDays(day: offset)
                }
                else if (abs(targetDegree - calcDegree) > 10)
                {
                    offset = 50
                    newDay.addDays(day: offset)
                }
                else if (abs(targetDegree - calcDegree) > 2)
                {
                    offset = 5
                    newDay.addDays(day: offset)
                }
                else
                {
                    offset = 1
                    newDay.addDays(day: offset)
                }
                
                julian = swiss.swe_utc_to_jd(year: Int32(newDay.getUTCFullYear()), month: Int32(newDay.getUTCMonth()), day: Int32(newDay.getUTCDay()), hour: Int32(newDay.getUTCHour()), minute: Int32(newDay.getMinute()), second: Double(newDay.getSecond()))
                if (config.sideReal == ESideReal.SIDEREAL)
                {
                    // ayanamsa計算
                    let ut: Ayanamsa = swiss.swe_get_ayanamsa_ex_ut(tjd_ut: julian.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

                    // Ephemeris Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
                }
                else
                {
                    // Universal Timeで計算, 結果はxに入る
                    calc = swiss.calc_ut(jd: julian.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
                }

                calcDegree = calc.x[0]
                if (calcCounter > 200)
                {
                    print("200ごえ")
                    break
                }
            }
        }

//        Debug.WriteLine(calc);
        return newDay;
    }
    
    

    public func isIn(from: Double, to: Double, degree: Double, orb: Double) -> Bool
    {
        var calc = GetOrb(from: from, to: to);
        if (degree - orb < calc && calc < degree + orb)
        {
            return true
        }

        return false
    }

    public func GetOrb(from: Double, to: Double) -> Double
    {
        var calc = abs(to - from);
        if (calc > 180) {
            calc = 360 - calc
        }

        return calc
    }
    
    public func isApply(sunDegree: Double, moonDegree: Double) -> Bool
    {
        if (sunDegree < 180)
        {
            var mid = sunDegree + 180;
            if (moonDegree < sunDegree)
            {
                return true
            }
            if (mid < moonDegree)
            {
                return true
            }
            return false
        }
        else
        {
            var mid = (sunDegree + 180).truncatingRemainder(dividingBy: 360)
            if (moonDegree < mid)
            {
                return false
            }
            if (sunDegree < moonDegree)
            {
                return false
            }
            return true
        }
    }
}
