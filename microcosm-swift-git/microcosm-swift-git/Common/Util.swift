//
//  Util.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/04/10.
//

import Foundation

public class Util
{
    static func UserDataBirthStr(userData: UserData) -> String {
        let str = String(format: "%04d/%02d/%02d %02d:%02d:%02d", userData.birth_year, userData.birth_month, userData.birth_day,
                         userData.birth_hour, userData.birth_minute, userData.birth_second)
        return str
    }


    static func AmateruTimeConverter(date: String, time: String, timezone: String) -> Date {
        let formatter: DateFormatter = DateFormatter()
        formatter.timeZone = TimeZone(identifier: timezone)
        formatter.dateFormat = "yyyy-MM-dd HH:mm:ss"
        let x = formatter.date(from: date + " " + time)!
        let y = Date(timeInterval: 60*60*24, since: x)
        
        return y
    }

    static func TimeConverter(content: String, timezone: String) -> Date {
        let formatter: DateFormatter = DateFormatter()
        formatter.timeZone = TimeZone(identifier: timezone)
        formatter.dateFormat = "yyyy/MM/dd HH:mm:ss"
        let x = formatter.date(from: content)!
        let y = Date(timeInterval: 60*60*24, since: x)
        
        return y
    }
    
    /// 回転
    /// - Parameters:
    ///   - x: x座標
    ///   - y: y座標
    ///   - degree: 回転度数
    /// - Returns:
    static func Rotate(x: Double, y: Double, degree: Double) -> Position
    {
        // ホロスコープは180°から始まる
        let newDegree = degree + 180.0

        let rad = (newDegree / 180.0) * Double.pi
        let newX = x * cos(rad) - y * sin(rad)
        let newY = x * sin(rad) + y * cos(rad)
        
        return Position(x: newX, y: newY)
    }
    
    static func GetDispPlanet(setting: SettingData, index: Int) -> Bool
    {
        switch (index) {
        case 0:
            return setting.dispPlanetSun == 1
        case 1:
            return setting.dispPlanetMoon == 1
        case 2:
            return setting.dispPlanetMercury == 1
        case 3:
            return setting.dispPlanetVenus == 1
        case 4:
            return setting.dispPlanetMars == 1
        case 5:
            return setting.dispPlanetJupiter == 1
        case 6:
            return setting.dispPlanetSaturn == 1
        case 7:
            return setting.dispPlanetUranus == 1
        case 8:
            return setting.dispPlanetNeptune == 1
        case 9:
            return setting.dispPlanetPluto == 1
        case EPlanets.DH_TRUENODE.rawValue:
            return setting.dispPlanetDH == 1
        case EPlanets.DH_MEANNODE.rawValue:
            return setting.dispPlanetDH == 1
        case EPlanets.DT_TRUE.rawValue:
            return setting.dispPlanetDT == 1
        case EPlanets.DT_MEAN.rawValue:
            return setting.dispPlanetDT == 1
        case EPlanets.MEAN_LILITH.rawValue:
            return setting.dispPlanetLilith == 1
        case EPlanets.OSCU_LILITH.rawValue:
            return setting.dispPlanetLilith == 1
        case EPlanets.CERES.rawValue:
            return setting.dispPlanetCeres == 1
        case EPlanets.PALLAS.rawValue:
            return setting.dispPlanetPallas == 1
        case EPlanets.JUNO.rawValue:
            return setting.dispPlanetJuno == 1
        case EPlanets.VESTA.rawValue:
            return setting.dispPlanetVesta == 1
        case EPlanets.EARTH.rawValue:
            return setting.dispPlanetEarth == 1


            
            // todo
        default:
            return false
        }
    }
    
    static func GetDispAspectPlanet(setting: SettingData, index: Int) -> Bool
    {
        switch (index) {
        case 0:
            return setting.dispAspectPlanetSun == 1
        case 1:
            return setting.dispAspectPlanetMoon == 1
        case 2:
            return setting.dispAspectPlanetMercury == 1
        case 3:
            return setting.dispAspectPlanetVenus == 1
        case 4:
            return setting.dispAspectPlanetMars == 1
        case 5:
            return setting.dispAspectPlanetJupiter == 1
        case 6:
            return setting.dispAspectPlanetSaturn == 1
        case 7:
            return setting.dispAspectPlanetUranus == 1
        case 8:
            return setting.dispAspectPlanetNeptune == 1
        case 9:
            return setting.dispAspectPlanetPluto == 1
            // todo
        default:
            return false
        }
    }
    
    static func GetNextIngressDegree(degree: Double) -> Double
    {
        let degrees = [0, 30.0, 60.0, 90.0, 120.0, 150.0, 180.0, 210.0, 240.0, 270.0, 300.0, 330.0]
        if degree > 359.5 {
            return 30.0
        }

        for d in degrees
        {
            // 149.999とかになる場合がある
            if degree + 0.4 < d {
                return d
            }
        }
        return 0.0
    }
    
    static func GetPrevIngressDegree(degree: Double) -> Double
    {
        let degrees = [331.0, 301.0, 271.0, 241.0, 211.0, 181.0, 151.0, 121.0, 91.0, 61.0, 31.0]
        if degree < 1 {
            return 330.0
        }

        for d in degrees {
            if degree > d {
                return d - 1.0
            }
        }
        
        return 0.0
    }
}
