//
//  AstroCalc.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/02/15.
//

import Foundation
import swissEphemeris

class AstroCalc {
    public var s: swissEphemerisMain
    public var configData: ConfigData
    public var eclipse: EclipseCalc
    public let SOLAR_YEAR: Double = 365.2424
    public var listarray = [PlanetData]()

    init(config: ConfigData, swiss: swissEphemerisMain) {
        configData = config
        s = swiss
        eclipse = EclipseCalc()
    }
    
    public func updateConfig(config: ConfigData)
    {
        configData = config
    }
    
    public func ReCalc(setting: SettingData, date: MyDate) -> [Int: PlanetData]
    {
        listarray.removeAll()
        var p = [Int: PlanetData]()
        
        if (configData.sideReal == ESideReal.DRACONIC)
        {
            p = DraconicPositionCalc(d: date, currentSetting: setting)
        }
        else
        {
            p = positionCalc(d: date, setting: setting);
        }
        
        return p
    }
    
    public func ReCalcProgress(config: ConfigData, setting: SettingData, natalList: [Int: PlanetData],  udata: UserData, transitTime: MyDate, timezone: Double) -> [Int: PlanetData]
    {
        var list: [Int: PlanetData]
        
        if (setting.progression == EProgression.SOLARARC.rawValue)
        {
            let d = MyDate()
            d.setUserData(u: udata)
            list = SolarArcCalc(natallist: natalList, natalTime: d, transitTime: transitTime, timezone: timezone)
        }
        else if (setting.progression == EProgression.SECONDARY.rawValue)
        {
            let d = MyDate()
            d.setUserData(u: udata)
            list = SecondaryProgressionCalc(natallist: natalList, natalTime: d, transitTime: transitTime, timezone: timezone)
        }
        else if (setting.progression == EProgression.PRIMARY.rawValue)
        {
            let d = MyDate()
            d.setUserData(u: udata)
            list = PrimaryProgressionCalc(natallist: natalList, natalTime: d, transitTime: transitTime)
        }
        else if (setting.progression == EProgression.CPS.rawValue)
        {
            let d = MyDate()
            d.setUserData(u: udata)
            list = CompositProgressionCalc(natallist: natalList, natalTime: d, transitTime: transitTime, timezone: timezone)
        }
        else
        {
            let d = MyDate()
            d.setUserData(u: udata)
            list = positionCalc(d: d, setting: setting)
        }

        return list
    }

    
    /// メイン計算
    /// - Parameters:
    ///   - d: 時刻
    ///   - setting: セッティング
    /// - Returns: 天体リスト
    public func positionCalc(d: MyDate, setting: SettingData) -> [Int: PlanetData] {
        // SEFLG_SWIEPH: 2 SEFLG_SPEED: 256
        var flag = 258
        var list = [Int: PlanetData]()
        
        let jd = s.swe_utc_to_jd(year: Int32(d.getUTCFullYear()), month: Int32(d.getUTCMonth()), day: Int32(d.getUTCDay()), hour: Int32(d.getUTCHour()), minute: Int32(d.getMinute()), second: Double(d.getSecond()))
        
        //var subIndex = 0

        for i in 0...21 {
            var calc: Calc
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
                calc = s.calc_ut(jd: ut.daya, planetNo: Int32(i), flag: Int32(flag))
            }
            else
            {
                calc = s.calc_ut(jd: jd.dret[0], planetNo: Int32(i), flag: Int32(flag))
            }

            let p: PlanetData = PlanetData()
            p.no = i
            p.absolute_position = calc.x[0]
            p.speed = calc.x[3]
            p.aspects = [AspectInfo]()
            p.secondAspects = [AspectInfo]()
            p.thirdAspects = [AspectInfo]()
            p.sensitive = false

            if (i < 10)
            {
                p.isDisp = Util.GetDispPlanet(setting: setting, index: i)
                p.isAspectDisp = Util.GetDispAspectPlanet(setting: setting, index: i)
                if (p.isDisp) {
                    listarray.append(p)
                }
            }

            // ヘリオセントリック太陽
            if (configData.centric == ECentric.HELIO_CENTRIC && i == EPlanets.SUN.rawValue)
            {
                p.isDisp = false
                p.isAspectDisp = false
            }

            // MEAN NODE
            if (i == EPlanets.DT_MEAN.rawValue)
            {
                p.sensitive = true
                if (configData.centric == ECentric.HELIO_CENTRIC)
                {
                    p.isDisp = false
                    p.isAspectDisp = false
                }
                else if (configData.nodeCalc == ENodeCalc.MEAN)
                {
                    p.isDisp = setting.dispPlanetDH == 1
                    p.isAspectDisp = setting.dispAspectPlanetDH == 1
                    if (p.isDisp) {
                        listarray.append(p)
                    }
                }
                else
                {
                    p.isDisp = false
                    p.isAspectDisp = false
                }
            }
            
            // TRUE NODE (ヘッド)
            if (i == EPlanets.DH_TRUENODE.rawValue)
            {
                p.sensitive = true
                if (configData.centric == ECentric.HELIO_CENTRIC)
                {
                    p.isDisp = false
                    p.isAspectDisp = false
                }
                else if (configData.nodeCalc == ENodeCalc.TRUE)
                {
                    p.isDisp = setting.dispPlanetDH == 1
                    p.isAspectDisp = setting.dispAspectPlanetDH == 1
                    if (p.isDisp) {
                        listarray.append(p)
                    }
                }
                else
                {
                    p.isDisp = false
                    p.isAspectDisp = false
                }
            }
         
            // mean apogee、要はリリス
            if (i == EPlanets.MEAN_LILITH.rawValue)
            {
                p.sensitive = true
                if (configData.centric == ECentric.HELIO_CENTRIC)
                {
                    p.isDisp = false
                    p.isAspectDisp = false
                }
                else if (configData.lilithCalc == ELilithCalc.MEAN)
                {
                    p.isDisp = setting.dispPlanetLilith == 1
                    p.isAspectDisp = setting.dispAspectPlanetLilith == 1
                    if (p.isDisp) {
                        listarray.append(p)
                    }
                }
                else
                {
                    p.isDisp = false
                    p.isAspectDisp = false
                }
            }
            
            if (i == EPlanets.OSCU_LILITH.rawValue)
            {
                // oscu apogee、要はリリス
                p.sensitive = true;
                if (configData.centric == ECentric.HELIO_CENTRIC)
                {
                    p.isDisp = false;
                    p.isAspectDisp = false;
                }
                else if (configData.lilithCalc == ELilithCalc.OSCU)
                {
                    p.isDisp = setting.dispPlanetLilith == 1
                    p.isAspectDisp = setting.dispAspectPlanetLilith == 1
                    if (p.isDisp) {
                        listarray.append(p)
                    }
                }
                else
                {
                    p.isDisp = false;
                    p.isAspectDisp = false;
                }
            }
            
            if (i == EPlanets.EARTH.rawValue)
            {
                // ヘリオセントリック地球
                if (configData.centric == ECentric.GEO_CENTRIC)
                {
                    p.isDisp = false
                    p.isAspectDisp = false
                }
                else
                {
                    p.isDisp = Util.GetDispPlanet(setting: setting, index: i)
                    p.isAspectDisp = Util.GetDispAspectPlanet(setting: setting, index: i)
                    if (p.isDisp) {
                        listarray.append(p)
                    }
                }
            }

            // chiron
            if (i == EPlanets.CHIRON.rawValue)
            {
                p.isDisp = Util.GetDispPlanet(setting: setting, index: i)
                p.isAspectDisp = Util.GetDispAspectPlanet(setting: setting, index: i)
                if (p.isDisp) {
                    listarray.append(p)
                }
            }
            
            // POLUS(未使用)
            if (i == EPlanets.POLUS.rawValue)
            {
                p.isDisp = false
                p.isAspectDisp = false
            }
            
            list[i] = p
        }

        // DragonTailはswissEphemerisから導けないので手動で追加
        let dtMean = PlanetData()
        dtMean.no = EPlanets.DT_MEAN.rawValue  // 101
        let mn = list[EPlanets.DH_MEANNODE.rawValue]
        dtMean.absolute_position = (mn!.absolute_position + 180.0).remainder(dividingBy: 360.0)
        dtMean.speed = mn!.speed
        dtMean.aspects = [AspectInfo]()
        dtMean.secondAspects = [AspectInfo]()
        dtMean.thirdAspects = [AspectInfo]()
        dtMean.sensitive = true
        if (configData.centric == ECentric.HELIO_CENTRIC)
        {
            dtMean.isDisp = false
            dtMean.isAspectDisp = false
        }
        else if (configData.nodeCalc == ENodeCalc.TRUE)
        {
            dtMean.isDisp = false
            dtMean.isAspectDisp = false
        }
        else
        {
            dtMean.isDisp = Util.GetDispPlanet(setting: setting, index: EPlanets.DT_MEAN.rawValue)
            dtMean.isAspectDisp = Util.GetDispAspectPlanet(setting: setting, index: EPlanets.DT_MEAN.rawValue)
            if (dtMean.isDisp) {
                listarray.append(dtMean)
            }
        }
        
        list[EPlanets.DT_MEAN.rawValue] = dtMean

        // DragonTailはswissEphemerisから導けないので手動で追加
        let dtTrue = PlanetData()
        dtTrue.no = EPlanets.DT_TRUE.rawValue
        let tn = list[EPlanets.DH_TRUENODE.rawValue]
        dtTrue.absolute_position = (tn!.absolute_position + 180.0).remainder(dividingBy: 360.0)
        dtTrue.speed = tn!.speed
        dtTrue.aspects = [AspectInfo]()
        dtTrue.secondAspects = [AspectInfo]()
        dtTrue.thirdAspects = [AspectInfo]()
        dtTrue.sensitive = true

        if (configData.centric == ECentric.HELIO_CENTRIC)
        {
            dtTrue.isDisp = false;
            dtTrue.isAspectDisp = false;
        }
        else if (configData.nodeCalc == ENodeCalc.MEAN)
        {
            dtTrue.isDisp = false;
            dtTrue.isAspectDisp = false;
        }
        else
        {
            dtTrue.isDisp = Util.GetDispPlanet(setting: setting, index: EPlanets.DT_TRUE.rawValue)
            dtTrue.isAspectDisp = Util.GetDispAspectPlanet(setting: setting, index: EPlanets.DT_TRUE.rawValue)
            if (dtTrue.isDisp) {
                listarray.append(dtTrue)
            }
        }
        
        list[EPlanets.DT_TRUE.rawValue] = dtTrue

        return list
    }
    
    public func PositionCalcSingle(date: MyDate, timezone: Double, planetId: Int) -> Double
    {
        // SEFLG_SWIEPH: 2 SEFLG_SPEED: 256
        var flag = 258
        
        let jd = s.swe_utc_to_jd(year: Int32(date.getUTCFullYear()), month: Int32(date.getUTCMonth()), day: Int32(date.getUTCDay()), hour: Int32(date.getUTCHour()), minute: Int32(date.getMinute()), second: Double(date.getSecond()))

        var calc: Calc
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
            calc = s.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
        }
        else
        {
            calc = s.calc_ut(jd: jd.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
        }

        return calc.x[0]
    }

    public func PositionCalcComposit(natal1: [Int: PlanetData], natal2: [Int: PlanetData]) -> [Int: PlanetData]
    {
        var list: [Int: PlanetData] = [:]

        natal1.keys.forEach { key in
            if natal2.contains(where: {
                $0.key == key
            }) {
                let newPosition = natal1[key]!.absolute_position + natal2[key]!.absolute_position / 2
                
                let progressData = PlanetData()
                progressData.absolute_position = newPosition
                progressData.no = key
                progressData.sensitive = natal1[key]?.sensitive ?? false
                progressData.aspects = []
                progressData.secondAspects = []
                progressData.thirdAspects = []
                progressData.isDisp = natal1[key]?.isDisp ?? false
                progressData.isAspectDisp = natal1[key]?.isAspectDisp ?? false
                
                list[key] = progressData
            }
        }

        return list
    }

    public func DraconicPositionCalc(d: MyDate, currentSetting: SettingData) -> [Int: PlanetData] {
        let list = positionCalc(d: d, setting: currentSetting)
        var targetDegree: Double
        if (configData.nodeCalc == ENodeCalc.MEAN)
        {
            targetDegree = list[EPlanets.DH_MEANNODE.rawValue]!.absolute_position
        }
        else
        {
            targetDegree = list[EPlanets.DH_TRUENODE.rawValue]!.absolute_position
        }

        var newPlanetList = [Int: PlanetData]()
        
        list.keys.forEach { key in
            let data = list[key]
            if (data != nil) {
                data!.absolute_position -= targetDegree
                if (data!.absolute_position < 0)
                {
                    data!.absolute_position += 360
                }
                newPlanetList[key] = data
            }
        }
        
        return newPlanetList
    }

    
    public func CuspCalc(d: MyDate, timezone: Double, lat: Double, lng: Double, houseKind: EHouse) -> [Double] {
        let swiss = swissEphemerisMain()
        
        let julian = swiss.swe_utc_to_jd(year: Int32(d.getUTCFullYear()), month: Int32(d.getUTCMonth()), day: Int32(d.getUTCDay()), hour: Int32(d.getUTCHour()), minute: Int32(d.getMinute()), second: Double(d.getSecond()))
        var cusps = swiss.swe_houses(jd: julian.dret[1], lat: lat, lng: lng, houseSystem: "P")
        
        if (houseKind == EHouse.PLACIDUS)
        {
            // Placidas
            cusps = swiss.swe_houses(jd: julian.dret[1], lat: lat, lng: lng, houseSystem: "P")
        }
        else if (houseKind == EHouse.KOCH)
        {
            // Koch
            cusps = swiss.swe_houses(jd: julian.dret[1], lat: lat, lng: lng, houseSystem: "K")
        }
        else if (houseKind == EHouse.CAMPANUS)
        {
            // Campanus
            cusps = swiss.swe_houses(jd: julian.dret[1], lat: lat, lng: lng, houseSystem: "C")
        }
        else if (houseKind == EHouse.PORPHYRY)
        {
            // Porphyrious
            cusps = swiss.swe_houses(jd: julian.dret[1], lat: lat, lng: lng, houseSystem: "O")
        }
        else if (houseKind == EHouse.REGIOMONTANUS)
        {
            // Porphyrious
            cusps = swiss.swe_houses(jd: julian.dret[1], lat: lat, lng: lng, houseSystem: "R")
        }
        else if (houseKind == EHouse.EQUAL)
        {
            // Equal
            cusps = swiss.swe_houses(jd: julian.dret[1], lat: lat, lng: lng, houseSystem: "E")
        }
        else if (houseKind == EHouse.ZEROARIES)
        {
            // ZeroAries
            cusps = swiss.swe_houses(jd: julian.dret[1], lat: lat, lng: lng, houseSystem: "N")
        }
        else if (houseKind == EHouse.SOLAR)
        {
            // Solar
            // 太陽の度数をASCとして30度
        }
        else if (houseKind == EHouse.SOLARSIGN)
        {
            // SolarSign
            // 太陽のサインの0度をASCとして30度
        }
        swiss.swe_close()

        return cusps.cusps
    }
    
    public func HouseCalcProgress(config: ConfigData, setting: SettingData, houseList: [Double], natallist: [Int:PlanetData], natalTime: MyDate, transitTime: MyDate, lat: Double, lng: Double,  timezone: Double) -> [Double]
    {
        var house: [Double] = []
        if (setting.progression == EProgression.SOLARARC.rawValue)
        {
            house = SolarArcHouseCalc(absolute_position: natallist[0]!.absolute_position, houseList: houseList, natalTime: natalTime, transitTime: transitTime, timezone: timezone)
        }
        else if (setting.progression == EProgression.SECONDARY.rawValue)
        {
            if (setting.houseCalc == 0) {
                house = SecondaryProgressionHouseCalc(natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone, houseKind: EHouse.PLACIDUS)
            } else if (setting.houseCalc == 1) {
                house = SecondaryProgressionHouseCalc(natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone, houseKind: EHouse.KOCH)
            } else if (setting.houseCalc == 2) {
                house = SecondaryProgressionHouseCalc(natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone, houseKind: EHouse.CAMPANUS)
            } else if (setting.houseCalc == 3) {
                house = SecondaryProgressionHouseCalc(natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone, houseKind: EHouse.EQUAL)
            } else if (setting.houseCalc == 4) {
                house = SecondaryProgressionHouseCalc(natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone, houseKind: EHouse.PORPHYRY)
            } else if (setting.houseCalc == 5) {
                house = SecondaryProgressionHouseCalc(natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone, houseKind: EHouse.REGIOMONTANUS)
            } else if (setting.houseCalc == 6) {
                house = SecondaryProgressionHouseCalc(natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone, houseKind: EHouse.SOLAR)
            } else if (setting.houseCalc == 7) {
                house = SecondaryProgressionHouseCalc(natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone, houseKind: EHouse.SOLARSIGN)
            } else if (setting.houseCalc == 8) {
                house = SecondaryProgressionHouseCalc(natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone, houseKind: EHouse.ZEROARIES)
            }
        }
        else if (setting.progression == EProgression.PRIMARY.rawValue)
        {
            house = PrimaryProgressionHouseCalc(houseList: houseList, natalTime: natalTime, transitTime: transitTime)
        }
        else if (setting.progression == EProgression.CPS.rawValue)
        {
            if (setting.houseCalc == 0) {
                house = CompositProgressionHouseCalc(houseKind: EHouse.PLACIDUS, houseList: houseList, natallist: natallist, natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone)
            } else if (setting.houseCalc == 1) {
                house = CompositProgressionHouseCalc(houseKind: EHouse.KOCH, houseList: houseList, natallist: natallist, natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone)
            } else if (setting.houseCalc == 2) {
                house = CompositProgressionHouseCalc(houseKind: EHouse.CAMPANUS, houseList: houseList, natallist: natallist, natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone)
            } else if (setting.houseCalc == 3) {
                house = CompositProgressionHouseCalc(houseKind: EHouse.EQUAL, houseList: houseList, natallist: natallist, natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone)
            } else if (setting.houseCalc == 4) {
                house = CompositProgressionHouseCalc(houseKind: EHouse.PORPHYRY, houseList: houseList, natallist: natallist, natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone)
            } else if (setting.houseCalc == 5) {
                house = CompositProgressionHouseCalc(houseKind: EHouse.REGIOMONTANUS, houseList: houseList, natallist: natallist, natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone)
            } else if (setting.houseCalc == 6) {
                house = CompositProgressionHouseCalc(houseKind: EHouse.SOLAR, houseList: houseList, natallist: natallist, natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone)
            } else if (setting.houseCalc == 7) {
                house = CompositProgressionHouseCalc(houseKind: EHouse.SOLARSIGN, houseList: houseList, natallist: natallist, natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone)
            } else if (setting.houseCalc == 8) {
                house = CompositProgressionHouseCalc(houseKind: EHouse.ZEROARIES, houseList: houseList, natallist: natallist, natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone)
            }
        }
        else
        {
            house = CuspCalc(d: natalTime, timezone: timezone, lat: lat, lng: lng, houseKind: EHouse.ZEROARIES)
        }

        return house;
    }
    
    public func SolarArcCalc(natallist: [Int: PlanetData], natalTime: MyDate, transitTime: MyDate, timezone: Double) -> [Int: PlanetData]
    {
        var progresslist: [Int: PlanetData] = [:]

        // 現在の太陽の度数
        let natalDegree: Double = natallist[0]!.absolute_position
        // 計算後の太陽の度数
        let calcDegree: Double = getBodyBySecondaryProgression(natalTime: natalTime, transitTime: transitTime, timezone: timezone, planetId: 0)
        // 加算する度数
        let addDegree: Double = calcDegree - natalDegree;

        natallist.keys.forEach { key in
            let progressData = PlanetData()
            progressData.absolute_position = progresslist[key]?.absolute_position ?? 0
            progressData.no = key
            progressData.sensitive = progresslist[key]?.sensitive ?? false
            progressData.aspects = []
            progressData.secondAspects = []
            progressData.thirdAspects = []
            progressData.isDisp = progresslist[key]?.isDisp ?? false
            progressData.isAspectDisp = progresslist[key]?.isAspectDisp ?? false
            
            progressData.absolute_position = progressData.absolute_position + addDegree
            progressData.absolute_position = progressData.absolute_position.truncatingRemainder(dividingBy: 360)
            progresslist[key] = progressData
        }

        return progresslist;
    }
    
    public func SolarArcHouseCalc(absolute_position: Double, houseList: [Double], natalTime: MyDate,  transitTime: MyDate, timezone: Double) -> [Double]
    {
        var retHouse: [Double] = []
        for i in 0..<13 {
            retHouse[i] = houseList[i]
        }

        // 現在の太陽の度数
        let natalDegree: Double = absolute_position;
        // 計算後の太陽の度数
        let calcDegree = getBodyBySecondaryProgression(natalTime: natalTime, transitTime: transitTime, timezone: timezone, planetId: 0)
        // 加算する度数
        let addDegree = calcDegree - natalDegree

        for i in 0..<13 {
            retHouse[i] += addDegree
        }

        return retHouse
    }
    
    public func SecondaryProgressionCalc(natallist: [Int: PlanetData], natalTime: MyDate, transitTime: MyDate, timezone: Double) -> [Int: PlanetData]
    {
        var progresslist: [Int: PlanetData] = [:]

        let natalJDay = s.swe_utc_to_jd(year: Int32(natalTime.getUTCFullYear()), month: Int32(natalTime.getUTCMonth()), day: Int32(natalTime.getUTCDay()), hour: Int32(natalTime.getUTCHour()), minute: Int32(natalTime.getMinute()), second:  Double(natalTime.getSecond()))
        
        let transitJDay = s.swe_utc_to_jd(year: Int32(transitTime.getUTCFullYear()), month: Int32(transitTime.getUTCMonth()), day: Int32(transitTime.getUTCDay()), hour: Int32(transitTime.getUTCHour()), minute: Int32(transitTime.getMinute()), second:  Double(transitTime.getSecond()))

        let dayOffset: Double = (transitJDay.dret[0] - natalJDay.dret[0]) / SOLAR_YEAR

        // 日数を秒数に変換する
        let seconds = dayOffset * 86400
        natalTime.addSeconds(second: seconds)

        let progressJDay = s.swe_utc_to_jd(year: Int32(natalTime.getUTCFullYear()), month: Int32(natalTime.getUTCMonth()), day: Int32(natalTime.getUTCDay()), hour: Int32(natalTime.getUTCHour()), minute: Int32(natalTime.getMinute()), second:  Double(natalTime.getSecond()))
        
        natallist.keys.forEach { key in
            // SEFLG_SWIEPH: 2 SEFLG_SPEED: 256
            var flag = 258
            var calc: Calc

            if (configData.centric == ECentric.HELIO_CENTRIC)
            {
                flag |= SweConst.SEFLG_HELCTR.rawValue
            }
            if (configData.sideReal == ESideReal.SIDEREAL)
            {
                flag |= SweConst.SEFLG_SIDEREAL.rawValue
                s.swe_set_sid_mode(flag: SweSideReal.SE_SIDM_LAHIRI.rawValue)
                // ayanamsa計算
                let ut: Ayanamsa = s.swe_get_ayanamsa_ex_ut(tjd_ut: progressJDay.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

                // Ephemeris Timeで計算, 結果はxに入る
                calc = s.calc_ut(jd: ut.daya, planetNo: Int32(key), flag: Int32(flag))
            }
            else
            {
                calc = s.calc_ut(jd: progressJDay.dret[0], planetNo: Int32(key), flag: Int32(flag))
            }

            // DragonTailはcalc_utじゃ計算されないのでcalc_utの結果を無視して
            // 事前にcalc_utで計算したDragonHeadの値から計算
            // DHより先にDTの計算来たら落ちる
            if (key == EPlanets.DT_TRUE.rawValue)
            {
                let progressData = PlanetData()
                progressData.absolute_position = natallist[key]?.absolute_position ?? 0
                progressData.no = key
                progressData.sensitive = natallist[key]?.sensitive ?? false
                progressData.aspects = []
                progressData.secondAspects = []
                progressData.thirdAspects = []
                progressData.isDisp = natallist[key]?.isDisp ?? false
                progressData.isAspectDisp = natallist[key]?.isAspectDisp ?? false
                
                progressData.absolute_position = (natallist[EPlanets.DH_TRUENODE.rawValue]!.absolute_position + 180)
                progressData.absolute_position = progressData.absolute_position.truncatingRemainder(dividingBy: 360)
                progresslist[key] = progressData
            } else if (key == EPlanets.DT_MEAN.rawValue)
            {
                let progressData = PlanetData()
                progressData.absolute_position = natallist[key]?.absolute_position ?? 0
                progressData.no = key
                progressData.sensitive = natallist[key]?.sensitive ?? false
                progressData.aspects = []
                progressData.secondAspects = []
                progressData.thirdAspects = []
                progressData.isDisp = natallist[key]?.isDisp ?? false
                progressData.isAspectDisp = natallist[key]?.isAspectDisp ?? false
                
                progressData.absolute_position = (natallist[EPlanets.DH_MEANNODE.rawValue]!.absolute_position + 180)
                progressData.absolute_position = progressData.absolute_position.truncatingRemainder(dividingBy: 360)
                progresslist[key] = progressData
            } else {
                let progressData = PlanetData()
                progressData.absolute_position = natallist[key]?.absolute_position ?? 0
                progressData.no = key
                progressData.sensitive = natallist[key]?.sensitive ?? false
                progressData.aspects = []
                progressData.secondAspects = []
                progressData.thirdAspects = []
                progressData.isDisp = natallist[key]?.isDisp ?? false
                progressData.isAspectDisp = natallist[key]?.isAspectDisp ?? false
                
                progressData.absolute_position = calc.x[0]
                progresslist[key] = progressData
            }
        }

        return progresslist
    }


    public func SecondaryProgressionHouseCalc(natalTime: MyDate, transitTime: MyDate, lat: Double, lng: Double, timezone: Double, houseKind: EHouse) -> [Double] {
        
        let natalJDay = s.swe_utc_to_jd(year: Int32(natalTime.getUTCFullYear()), month: Int32(natalTime.getUTCMonth()), day: Int32(natalTime.getUTCDay()), hour: Int32(natalTime.getUTCHour()), minute: Int32(natalTime.getMinute()), second:  Double(natalTime.getSecond()))
        
        let transitJDay = s.swe_utc_to_jd(year: Int32(transitTime.getUTCFullYear()), month: Int32(transitTime.getUTCMonth()), day: Int32(transitTime.getUTCDay()), hour: Int32(transitTime.getUTCHour()), minute: Int32(transitTime.getMinute()), second:  Double(transitTime.getSecond()))

        let dayOffset: Double = (transitJDay.dret[0] - natalJDay.dret[0]) / SOLAR_YEAR

        // 日数を秒数に変換する
        let seconds = dayOffset * 86400
        natalTime.addSeconds(second: seconds)
        let retHouse = CuspCalc(d: natalTime, timezone: timezone, lat: lat, lng: lng, houseKind: houseKind)
        
        return retHouse
    }
    
    public func getBodyBySecondaryProgression(natalTime: MyDate, transitTime: MyDate, timezone: Double,  planetId: Int) -> Double
    {
        let natalJDay = s.swe_utc_to_jd(year: Int32(natalTime.getUTCFullYear()), month: Int32(natalTime.getUTCMonth()), day: Int32(natalTime.getUTCDay()), hour: Int32(natalTime.getUTCHour()), minute: Int32(natalTime.getMinute()), second:  Double(natalTime.getSecond()))
        
        let transitJDay = s.swe_utc_to_jd(year: Int32(transitTime.getUTCFullYear()), month: Int32(transitTime.getUTCMonth()), day: Int32(transitTime.getUTCDay()), hour: Int32(transitTime.getUTCHour()), minute: Int32(transitTime.getMinute()), second:  Double(transitTime.getSecond()))

        let dayOffset: Double = (transitJDay.dret[0] - natalJDay.dret[0]) / SOLAR_YEAR

        let progresTime = natalTime
        progresTime.addDays(day: dayOffset)

        //double jd = Julian.ToJulianDate(progresTime);
        //double ET = jd + s.swe_deltat(jd);

//        // [0]:Ephemeris Time [1]:Universal Time
//        double[] dret = { 0.0, 0.0 };
//        // absolute position
//        double[] x = { 0, 0, 0, 0, 0, 0 };
//        string serr = "";
//
//        int utc_year = 0;
//        int utc_month = 0;
//        int utc_day = 0;
//        int utc_hour = 0;
//        int utc_minute = 0;
//        double utc_second = 0;
//
//        // utcに変換
//        s.swe_utc_time_zone(progresTime.Year, progresTime.Month, progresTime.Day, progresTime.Hour, progresTime.Minute, progresTime.Second, timezone,
//            ref utc_year, ref utc_month, ref utc_day, ref utc_hour, ref utc_minute, ref utc_second);
//        s.swe_utc_to_jd(utc_year, utc_month, utc_day, utc_hour, utc_minute, utc_second, 1, dret, ref serr);

        let jd = s.swe_utc_to_jd(year: Int32(progresTime.getUTCFullYear()), month: Int32(progresTime.getUTCMonth()), day: Int32(progresTime.getUTCDay()), hour: Int32(progresTime.getUTCHour()), minute: Int32(progresTime.getMinute()), second: Double(progresTime.getSecond()))

        // SEFLG_SWIEPH: 2 SEFLG_SPEED: 256
        var flag = 258
        var calc: Calc

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
            calc = s.calc_ut(jd: ut.daya, planetNo: Int32(planetId), flag: Int32(flag))
        }
        else
        {
            calc = s.calc_ut(jd: jd.dret[0], planetNo: Int32(planetId), flag: Int32(flag))
        }

        s.swe_close()

        return calc.x[0]
    }

    public func PrimaryProgressionCalc(natallist: [Int: PlanetData], natalTime: MyDate, transitTime: MyDate) -> [Int: PlanetData]
    {
        var progresslist: [Int: PlanetData] = [:]
        let natalJDay = s.swe_utc_to_jd(year: Int32(natalTime.getUTCFullYear()), month: Int32(natalTime.getUTCMonth()), day: Int32(natalTime.getUTCDay()), hour: Int32(natalTime.getUTCHour()), minute: Int32(natalTime.getMinute()), second:  Double(natalTime.getSecond()))
        
        let transitJDay = s.swe_utc_to_jd(year: Int32(transitTime.getUTCFullYear()), month: Int32(transitTime.getUTCMonth()), day: Int32(transitTime.getUTCDay()), hour: Int32(transitTime.getUTCHour()), minute: Int32(transitTime.getMinute()), second:  Double(transitTime.getSecond()))

        let dayOffset: Double = (transitJDay.dret[0] - natalJDay.dret[0]) / SOLAR_YEAR

        natallist.keys.forEach { key in
            let progressData = PlanetData()
            progressData.absolute_position = progresslist[key]?.absolute_position ?? 0
            progressData.no = key
            progressData.sensitive = progresslist[key]?.sensitive ?? false
            progressData.aspects = []
            progressData.secondAspects = []
            progressData.thirdAspects = []
            progressData.isDisp = progresslist[key]?.isDisp ?? false
            progressData.isAspectDisp = progresslist[key]?.isAspectDisp ?? false
            
            progressData.absolute_position = progressData.absolute_position + dayOffset
            progressData.absolute_position = progressData.absolute_position.truncatingRemainder(dividingBy: 360)
            progresslist[key] = progressData
        }

        return progresslist
    }
    
    public func PrimaryProgressionHouseCalc(houseList: [Double], natalTime: MyDate, transitTime: MyDate) -> [Double]
    {
        var retHouse: [Double] = []
        for i in 0..<13 {
            retHouse[i] = houseList[i]
        }

        let natalJDay = s.swe_utc_to_jd(year: Int32(natalTime.getUTCFullYear()), month: Int32(natalTime.getUTCMonth()), day: Int32(natalTime.getUTCDay()), hour: Int32(natalTime.getUTCHour()), minute: Int32(natalTime.getMinute()), second:  Double(natalTime.getSecond()))
        
        let transitJDay = s.swe_utc_to_jd(year: Int32(transitTime.getUTCFullYear()), month: Int32(transitTime.getUTCMonth()), day: Int32(transitTime.getUTCDay()), hour: Int32(transitTime.getUTCHour()), minute: Int32(transitTime.getMinute()), second:  Double(transitTime.getSecond()))

        let dayOffset: Double = (transitJDay.dret[0] - natalJDay.dret[0]) / SOLAR_YEAR

        for i in 0..<13 {
            retHouse[i] += dayOffset
        }

        return retHouse
    }

    public func CompositProgressionCalc(natallist: [Int: PlanetData], natalTime: MyDate, transitTime: MyDate, timezone: Double) -> [Int: PlanetData]
    {
        var progresslist: [Int: PlanetData] = [:]
        let natalJDay = s.swe_utc_to_jd(year: Int32(natalTime.getUTCFullYear()), month: Int32(natalTime.getUTCMonth()), day: Int32(natalTime.getUTCDay()), hour: Int32(natalTime.getUTCHour()), minute: Int32(natalTime.getMinute()), second:  Double(natalTime.getSecond()))
        
        let transitJDay = s.swe_utc_to_jd(year: Int32(transitTime.getUTCFullYear()), month: Int32(transitTime.getUTCMonth()), day: Int32(transitTime.getUTCDay()), hour: Int32(transitTime.getUTCHour()), minute: Int32(transitTime.getMinute()), second:  Double(transitTime.getSecond()))

        let dayOffset: Double = (transitJDay.dret[0] - natalJDay.dret[0]) / SOLAR_YEAR

        // 日数を秒数に変換する
        let seconds = dayOffset * 86400
        natalTime.addSeconds(second: seconds)

        let progressJDay = s.swe_utc_to_jd(year: Int32(natalTime.getUTCFullYear()), month: Int32(natalTime.getUTCMonth()), day: Int32(natalTime.getUTCDay()), hour: Int32(natalTime.getUTCHour()), minute: Int32(natalTime.getMinute()), second:  Double(natalTime.getSecond()))

        natallist.keys.forEach { key in
            if ((key != EPlanets.MOON.rawValue) &&
                (key != EPlanets.MERCURY.rawValue) &&
                (key != EPlanets.VENUS.rawValue) &&
                (key != EPlanets.SUN.rawValue))
            {
                let progressData = PlanetData()
                progressData.absolute_position = natallist[key]?.absolute_position ?? 0
                progressData.no = key
                progressData.sensitive = natallist[key]?.sensitive ?? false
                progressData.aspects = []
                progressData.secondAspects = []
                progressData.thirdAspects = []
                progressData.isDisp = natallist[key]?.isDisp ?? false
                progressData.isAspectDisp = natallist[key]?.isAspectDisp ?? false
                
                progressData.absolute_position = progressData.absolute_position + dayOffset
                progressData.absolute_position = progressData.absolute_position.truncatingRemainder(dividingBy: 360)
                progresslist[key] = progressData

                return
            }


            // SEFLG_SWIEPH: 2 SEFLG_SPEED: 256
            var flag = 258
            var calc: Calc

            if (configData.centric == ECentric.HELIO_CENTRIC)
            {
                flag |= SweConst.SEFLG_HELCTR.rawValue
            }
            if (configData.sideReal == ESideReal.SIDEREAL)
            {
                flag |= SweConst.SEFLG_SIDEREAL.rawValue
                s.swe_set_sid_mode(flag: SweSideReal.SE_SIDM_LAHIRI.rawValue)
                // ayanamsa計算
                let ut: Ayanamsa = s.swe_get_ayanamsa_ex_ut(tjd_ut: progressJDay.dret[1], iflag: SweConst.SEFLG_SWIEPH.rawValue)

                // Ephemeris Timeで計算, 結果はxに入る
                calc = s.calc_ut(jd: ut.daya, planetNo: Int32(key), flag: Int32(flag))
            }
            else
            {
                calc = s.calc_ut(jd: progressJDay.dret[0], planetNo: Int32(key), flag: Int32(flag))
            }

            // DragonTailはcalc_utじゃ計算されないのでcalc_utの結果を無視して
            // 事前にcalc_utで計算したDragonHeadの値から計算
            // DHより先にDTの計算来たら落ちる
            if (key == EPlanets.DT_TRUE.rawValue)
            {
                let progressData = PlanetData()
                progressData.absolute_position = natallist[key]?.absolute_position ?? 0
                progressData.no = key
                progressData.sensitive = natallist[key]?.sensitive ?? false
                progressData.aspects = []
                progressData.secondAspects = []
                progressData.thirdAspects = []
                progressData.isDisp = natallist[key]?.isDisp ?? false
                progressData.isAspectDisp = natallist[key]?.isAspectDisp ?? false
                
                progressData.absolute_position = (natallist[EPlanets.DH_TRUENODE.rawValue]!.absolute_position + 180)
                progressData.absolute_position = progressData.absolute_position.truncatingRemainder(dividingBy: 360)
                progresslist[key] = progressData
            } else if (key == EPlanets.DT_MEAN.rawValue)
            {
                let progressData = PlanetData()
                progressData.absolute_position = natallist[key]?.absolute_position ?? 0
                progressData.no = key
                progressData.sensitive = natallist[key]?.sensitive ?? false
                progressData.aspects = []
                progressData.secondAspects = []
                progressData.thirdAspects = []
                progressData.isDisp = natallist[key]?.isDisp ?? false
                progressData.isAspectDisp = natallist[key]?.isAspectDisp ?? false
                
                progressData.absolute_position = (natallist[EPlanets.DH_MEANNODE.rawValue]!.absolute_position + 180)
                progressData.absolute_position = progressData.absolute_position.truncatingRemainder(dividingBy: 360)
                progresslist[key] = progressData
            } else {
                let progressData = PlanetData()
                progressData.absolute_position = natallist[key]?.absolute_position ?? 0
                progressData.no = key
                progressData.sensitive = natallist[key]?.sensitive ?? false
                progressData.aspects = []
                progressData.secondAspects = []
                progressData.thirdAspects = []
                progressData.isDisp = natallist[key]?.isDisp ?? false
                progressData.isAspectDisp = natallist[key]?.isAspectDisp ?? false
                
                progressData.absolute_position = calc.x[0]
                progresslist[key] = progressData
            }
        }

        return progresslist
    }
    
    public func CompositProgressionHouseCalc(houseKind: EHouse, houseList: [Double], natallist: [Int:PlanetData], natalTime: MyDate, transitTime: MyDate, lat: Double, lng: Double, timezone: Double) -> [Double]
    {
        // AMATERU、SG共にSecondaryで計算されてた
        return SecondaryProgressionHouseCalc(natalTime: natalTime, transitTime: transitTime, lat: lat, lng: lng, timezone: timezone, houseKind: houseKind)
    }
}
