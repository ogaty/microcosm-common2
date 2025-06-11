//
//  swissEphemeris.swift
//  swissEphemeris
//
//  Created by 緒形雄二 on 2023/09/10.
//

import Foundation

import d

public class swissEphemerisMain {
    public var pw = getpwuid(getuid())

    
    public init()
    {
        
    }
    
    
    /// epheパスを設定
    public func set_ephe_path()
    {
        let url = URL(fileURLWithPath:Bundle.main.resourcePath!).path
        wrap_set_ephe_path(strdup(url))
    }
    
    public func swe_close()
    {
        wrap_swe_close()
    }
    
    
    /// UTC時刻を求める
    /// - Parameters:
    ///   - year: 年
    ///   - month: 月
    ///   - day: 日
    ///   - hour: 時間
    ///   - minute: 分
    ///   - second: 秒
    ///   - timeZone: タイムゾーン(JSTは+9.0)
    /// - Returns: UTCオブジェクト
    public func utc_time_zone(year: Int32, month: Int32, day: Int32, hour: Int32, minute: Int32, second: Double, timeZone: Double) -> UTC
    {
        var utcYear: Int32 = year
        var utcMonth: Int32 = month
        var utcDay: Int32 = day
        var utcHour: Int32 = hour
        var utcMinute: Int32 = minute
        var utcSecond: Double = second
        wrap_utc_timezone(&utcYear, &utcMonth, &utcDay, &utcHour, &utcMinute, &utcSecond, timeZone)
//        print(utcYear)
//        print(utcHour)
        
        let utc = UTC()
        utc.year = Int(utcYear)
        utc.month = Int(utcMonth)
        utc.day = Int(utcDay)
        utc.hour = Int(utcHour)
        utc.minute = Int(utcMinute)
        utc.second = Double(utcSecond)
        
        return utc
    }
    
    
    /// 時刻をユリウス日に求める
    /// - Parameters:
    ///   - year: 年
    ///   - month: 月
    ///   - day: 日
    ///   - hour: 時
    ///   - minute: 分
    ///   - second: 秒
    /// - Returns: Julianオブジェクト
    public func swe_utc_to_jd(year: Int32, month: Int32, day: Int32, hour: Int32, minute: Int32, second: Double) -> Julian {
        let dret = UnsafeMutablePointer<Double>.allocate(capacity: 6)
        let serr = UnsafeMutablePointer<Int8>.allocate(capacity: 255)
        wrap_swe_utc_to_jd(year, month, day, hour, minute, second, 1, dret, serr)
        let julian = Julian()
        
        julian.dret[0] = dret[0]
        julian.dret[1] = dret[1]
        julian.dret[2] = dret[2]
        julian.dret[3] = dret[3]
        julian.dret[4] = dret[4]
        julian.dret[5] = dret[5]
        julian.err = String(cString: serr)
        return julian
    }
    
    public func calc_ut(jd: Double, planetNo: Int32, flag: Int32) -> Calc
    {
        let x = UnsafeMutablePointer<Double>.allocate(capacity: 6)
        let serr = UnsafeMutablePointer<Int8>.allocate(capacity: 255)

        wrap_swe_calc_ut(jd, planetNo, flag, x, serr);
        let calc = Calc()
        calc.x[0] = x[0]
        calc.x[1] = x[1]
        calc.x[2] = x[2]
        calc.x[3] = x[3]
        calc.x[4] = x[4]
        calc.x[5] = x[5]
        calc.err = String(cString: serr)
        
        return calc
    }
    
    public func swe_houses(jd: Double, lat: Double, lng: Double, houseSystem: String) -> House
    {
        let cusps = UnsafeMutablePointer<Double>.allocate(capacity: 13)
        let ascmc = UnsafeMutablePointer<Double>.allocate(capacity: 10)
        if (houseSystem == "P") {
            wrap_swe_houses(jd, lat, lng, 80, cusps, ascmc)
        } else if (houseSystem == "K") {
            wrap_swe_houses(jd, lat, lng, 75, cusps, ascmc)
        } else if (houseSystem == "C") {
            wrap_swe_houses(jd, lat, lng, 67, cusps, ascmc)
        } else if (houseSystem == "E") {
            wrap_swe_houses(jd, lat, lng, 69, cusps, ascmc)
        } else if (houseSystem == "O") {
            wrap_swe_houses(jd, lat, lng, 68, cusps, ascmc)
        } else if (houseSystem == "R") {
            wrap_swe_houses(jd, lat, lng, 82, cusps, ascmc)
        } else if (houseSystem == "N") {
            wrap_swe_houses(jd, lat, lng, 78, cusps, ascmc)
        } else {
            wrap_swe_houses(jd, lat, lng, 80, cusps, ascmc)
        }
        
        let house = House()
        for i in 1...12 {
            house.cusps[i] = cusps[i]
        }
        for i in 0..<8 {
            house.ascmc[i] = ascmc[i]
        }

        return house
    }
    
    public func swe_set_sid_mode(flag: Int)
    {
        wrap_swe_set_sid_mode(Int32(flag))
    }
    
    public func swe_get_ayanamsa_ex_ut(tjd_ut: Double, iflag: Int) -> Ayanamsa
    {
        let daya = UnsafeMutablePointer<Double>.allocate(capacity: 8)
        let serr = UnsafeMutablePointer<Int8>.allocate(capacity: 255)

        wrap_swe_get_ayanamsa_ex_ut(Int32(tjd_ut), Int32(iflag), daya, serr)
        
        var ayanamsa = Ayanamsa()
        ayanamsa.daya = daya[0]
        ayanamsa.err = String(cString: serr)
        
        return ayanamsa
    }
}

