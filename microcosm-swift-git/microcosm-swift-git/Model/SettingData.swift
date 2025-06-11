//
//  SettingData.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/02/14.
//

import Foundation

public struct SettingData: Codable {
    var name: String = "新規設定"
    
    var dispPlanetSun: Int = 1
    var dispPlanetMoon: Int = 1
    var dispPlanetMercury: Int = 1
    var dispPlanetVenus: Int = 1
    var dispPlanetMars: Int = 1
    var dispPlanetJupiter: Int = 1
    var dispPlanetSaturn: Int = 1
    var dispPlanetUranus: Int = 1
    var dispPlanetNeptune: Int = 1
    var dispPlanetPluto: Int = 1
    var dispPlanetAsc: Int = 1
    var dispPlanetMc: Int = 1
    var dispPlanetDH: Int = 0
    var dispPlanetDT: Int = 0
    var dispPlanetChiron: Int = 0
    var dispPlanetLilith: Int = 0
    var dispPlanetEarth: Int = 0
    var dispPlanetCeres: Int = 0
    var dispPlanetPallas: Int = 0
    var dispPlanetJuno: Int = 0
    var dispPlanetVesta: Int = 0
    var dispPlanetPof: Int = 0
    var dispPlanetPos: Int = 0

    var dispAspectPlanetSun: Int = 1
    var dispAspectPlanetMoon: Int = 1
    var dispAspectPlanetMercury: Int = 1
    var dispAspectPlanetVenus: Int = 1
    var dispAspectPlanetMars: Int = 1
    var dispAspectPlanetJupiter: Int = 1
    var dispAspectPlanetSaturn: Int = 1
    var dispAspectPlanetUranus: Int = 1
    var dispAspectPlanetNeptune: Int = 1
    var dispAspectPlanetPluto: Int = 1
    var dispAspectPlanetAsc: Int = 1
    var dispAspectPlanetMc: Int = 1
    var dispAspectPlanetDH: Int = 0
    var dispAspectPlanetDT: Int = 0
    var dispAspectPlanetChiron: Int = 0
    var dispAspectPlanetLilith: Int = 0
    var dispAspectPlanetEarth: Int = 0
    var dispAspectPlanetCeres: Int = 0
    var dispAspectPlanetPallas: Int = 0
    var dispAspectPlanetJuno: Int = 0
    var dispAspectPlanetVesta: Int = 0
    var dispAspectPlanetPof: Int = 0
    var dispAspectPlanetPos: Int = 0
    
    var dispConjunction: Int = 1
    var dispOpposition: Int = 1
    var dispTrine: Int = 1
    var dispSquare: Int = 1
    var dispSextile: Int = 1
    var dispInconjunct: Int = 1
    var dispSesquiQuadrate: Int = 1
    var dispSemiSquare: Int = 0
    var dispSemiSextile: Int = 0
    var dispQuintile: Int = 0
    var dispNovile: Int = 0
    // 昔のバージョンで変数名にAspectつけちゃってるけど、まあそのままでも特に支障ないからそのまま
    var dispAspectBiQuintile: Int = 0
    var dispAspectSemiQuintile: Int = 0
    var dispAspectSeptile: Int = 0
    var dispAspectQuindecile: Int = 0
    
    var orbSunMoon: [Float] = [10, 8]
    var orb1st: [Float] = [6, 3]
    var orb2nd: [Float] = [1.5, 1]
    
    var progression: Int = 1
    var houseCalc: Int = 1
    var sameCusps: Bool = false
}

public class SettingSave {
    
    /// Save
    /// - Parameters:
    ///   - setting: saveするデータ
    ///   - url: ホームURL
    ///   - index: index
    static func save(setting: SettingData, url: URL, index: Int) {
        let fileURL = url.appendingPathComponent("/system").appendingPathComponent("settings" + String(index) + ".json")

        let encoder = JSONEncoder()
        encoder.outputFormatting = .prettyPrinted //JSONデータを整形する
        guard let jsonValue = try? encoder.encode(setting) else {
            fatalError("JSONエンコードエラー")
        }
         
        do {
            try jsonValue.write(to: fileURL)
        } catch {
            fatalError("JSON書き込みエラー")
        }
        print("saved")
    }
    
    
    /// Load
    /// - Parameter url: ホームURL
    /// - Returns: SettingData
    static func load(url: URL, index: Int) -> SettingData? {
        let fileURL = url.appendingPathComponent("/system").appendingPathComponent("settings" + String(index) + ".json")
        do {
            let data = try Data(contentsOf: fileURL)
            let jsonObject: [String: Any] = try JSONSerialization.jsonObject(with: data, options: [.mutableContainers, .mutableLeaves]) as! [String: Any]
            // 毎回インスタンス作って良いのか、本来は同じインスタンスに上書きしたほうが良い気もする
            var setting = SettingData()
            if let name = jsonObject["name"] {
                setting.name = name as! String
            }

            if let dispPlanetSun = jsonObject["dispPlanetSun"] {
                setting.dispPlanetSun = dispPlanetSun as! Int
            }
            if let dispAspectPlanetSun = jsonObject["dispAspectPlanetSun"] {
                setting.dispAspectPlanetSun = dispAspectPlanetSun as! Int
            }


            if let dispPlanetMoon = jsonObject["dispPlanetMoon"] {
                setting.dispPlanetMoon = dispPlanetMoon as! Int
            }
            if let dispAspectPlanetMoon = jsonObject["dispAspectPlanetMoon"] {
                setting.dispAspectPlanetMoon = dispAspectPlanetMoon as! Int
            }

            if let dispPlanetMercury = jsonObject["dispPlanetMercury"] {
                setting.dispPlanetMercury = dispPlanetMercury as! Int
            }
            if let dispAspectPlanetMercury = jsonObject["dispAspectPlanetMercury"] {
                setting.dispAspectPlanetMercury = dispAspectPlanetMercury as! Int
            }

            if let dispPlanetVenus = jsonObject["dispPlanetVenus"] {
                setting.dispPlanetVenus = dispPlanetVenus as! Int
            }
            if let dispAspectPlanetVenus = jsonObject["dispAspectPlanetVenus"] {
                setting.dispAspectPlanetVenus = dispAspectPlanetVenus as! Int
            }

            if let dispPlanetMars = jsonObject["dispPlanetMars"] {
                setting.dispPlanetMars = dispPlanetMars as! Int
            }
            if let dispAspectPlanetMars = jsonObject["dispAspectPlanetMars"] {
                setting.dispAspectPlanetMars = dispAspectPlanetMars as! Int
            }


            if let dispPlanetJupiter = jsonObject["dispPlanetJupiter"] {
                setting.dispPlanetJupiter = dispPlanetJupiter as! Int
            }
            if let dispAspectPlanetJupiter = jsonObject["dispAspectPlanetJupiter"] {
                setting.dispAspectPlanetJupiter = dispAspectPlanetJupiter as! Int
            }


            if let dispPlanetSaturn = jsonObject["dispPlanetSaturn"] {
                setting.dispPlanetSaturn = dispPlanetSaturn as! Int
            }
            if let dispAspectPlanetSaturn = jsonObject["dispAspectPlanetSaturn"] {
                setting.dispAspectPlanetSaturn = dispAspectPlanetSaturn as! Int
            }


            if let dispPlanetUranus = jsonObject["dispPlanetUranus"] {
                setting.dispPlanetUranus = dispPlanetUranus as! Int
            }
            if let dispAspectPlanetUranus = jsonObject["dispAspectPlanetUranus"] {
                setting.dispAspectPlanetUranus = dispAspectPlanetUranus as! Int
            }


            if let dispPlanetNeptune = jsonObject["dispPlanetNeptune"] {
                setting.dispPlanetNeptune = dispPlanetNeptune as! Int
            }
            if let dispAspectPlanetNeptune = jsonObject["dispAspectPlanetNeptune"] {
                setting.dispAspectPlanetNeptune = dispAspectPlanetNeptune as! Int
            }


            if let dispPlanetPluto = jsonObject["dispPlanetPluto"] {
                setting.dispPlanetPluto = dispPlanetPluto as! Int
            }
            if let dispAspectPlanetPluto = jsonObject["dispAspectPlanetPluto"] {
                setting.dispAspectPlanetPluto = dispAspectPlanetPluto as! Int
            }


            if let dispPlanetAsc = jsonObject["dispPlanetAsc"] {
                setting.dispPlanetAsc = dispPlanetAsc as! Int
            }
            if let dispAspectPlanetAsc = jsonObject["dispAspectPlanetAsc"] {
                setting.dispAspectPlanetAsc = dispAspectPlanetAsc as! Int
            }


            if let dispPlanetMc = jsonObject["dispPlanetMc"] {
                setting.dispPlanetMc = dispPlanetMc as! Int
            }
            if let dispAspectPlanetMc = jsonObject["dispAspectPlanetMc"] {
                setting.dispAspectPlanetMc = dispAspectPlanetMc as! Int
            }


            if let dispPlanetChiron = jsonObject["dispPlanetChiron"] {
                setting.dispPlanetChiron = dispPlanetChiron as! Int
            }
            if let dispAspectPlanetChiron = jsonObject["dispAspectPlanetChiron"] {
                setting.dispAspectPlanetChiron = dispAspectPlanetChiron as! Int
            }


            if let dispPlanetDH = jsonObject["dispPlanetDH"] {
                setting.dispPlanetDH = dispPlanetDH as! Int
            }
            if let dispAspectPlanetDH = jsonObject["dispAspectPlanetDH"] {
                setting.dispAspectPlanetDH = dispAspectPlanetDH as! Int
            }


            if let dispPlanetDT = jsonObject["dispPlanetDT"] {
                setting.dispPlanetDT = dispPlanetDT as! Int
            }
            if let dispAspectPlanetDT = jsonObject["dispAspectPlanetDT"] {
                setting.dispAspectPlanetDT = dispAspectPlanetDT as! Int
            }


            if let dispPlanetLilith = jsonObject["dispPlanetLilith"] {
                setting.dispPlanetLilith = dispPlanetLilith as! Int
            }
            if let dispAspectPlanetLilith = jsonObject["dispAspectPlanetLilith"] {
                setting.dispAspectPlanetLilith = dispAspectPlanetLilith as! Int
            }


            if let dispPlanetEarth = jsonObject["dispPlanetEarth"] {
                setting.dispPlanetEarth = dispPlanetEarth as! Int
            }
            if let dispAspectPlanetEarth = jsonObject["dispAspectPlanetEarth"] {
                setting.dispAspectPlanetEarth = dispAspectPlanetEarth as! Int
            }


            if let dispPlanetCeres = jsonObject["dispPlanetCeres"] {
                setting.dispPlanetCeres = dispPlanetCeres as! Int
            }
            if let dispAspectPlanetCeres = jsonObject["dispAspectPlanetCeres"] {
                setting.dispAspectPlanetCeres = dispAspectPlanetCeres as! Int
            }


            if let dispPlanetPallas = jsonObject["dispPlanetPallas"] {
                setting.dispPlanetPallas = dispPlanetPallas as! Int
            }
            if let dispAspectPlanetPallas = jsonObject["dispAspectPlanetPallas"] {
                setting.dispAspectPlanetPallas = dispAspectPlanetPallas as! Int
            }


            if let dispPlanetJuno = jsonObject["dispPlanetJuno"] {
                setting.dispPlanetJuno = dispPlanetJuno as! Int
            }
            if let dispAspectPlanetJuno = jsonObject["dispAspectPlanetJuno"] {
                setting.dispAspectPlanetJuno = dispAspectPlanetJuno as! Int
            }


            if let dispPlanetVesta = jsonObject["dispPlanetVesta"] {
                setting.dispPlanetVesta = dispPlanetVesta as! Int
            }
            if let dispAspectPlanetVesta = jsonObject["dispAspectPlanetVesta"] {
                setting.dispAspectPlanetVesta = dispAspectPlanetVesta as! Int
            }


            if let dispPlanetPof = jsonObject["dispPlanetPof"] {
                setting.dispPlanetPof = dispPlanetPof as! Int
            }
            if let dispAspectPlanetPof = jsonObject["dispAspectPlanetPof"] {
                setting.dispAspectPlanetPof = dispAspectPlanetPof as! Int
            }


            if let dispPlanetPos = jsonObject["dispPlanetPos"] {
                setting.dispPlanetPos = dispPlanetPos as! Int
            }
            if let dispAspectPlanetPos = jsonObject["dispAspectPlanetPos"] {
                setting.dispAspectPlanetPos = dispAspectPlanetPos as! Int
            }

            if let dispConjunction = jsonObject["dispConjunction"] {
                setting.dispConjunction = dispConjunction as! Int
            }
            if let dispOpposition = jsonObject["dispOpposition"] {
                setting.dispOpposition = dispOpposition as! Int
            }
            if let dispTrine = jsonObject["dispTrine"] {
                setting.dispTrine = dispTrine as! Int
            }
            if let dispSquare = jsonObject["dispSquare"] {
                setting.dispSquare = dispSquare as! Int
            }
            if let dispSextile = jsonObject["dispSextile"] {
                setting.dispSextile = dispSextile as! Int
            }
            if let dispInconjunct = jsonObject["dispInconjunct"] {
                setting.dispInconjunct = dispInconjunct as! Int
            }
            if let dispSesquiQuadrate = jsonObject["dispSesquiQuadrate"] {
                setting.dispSesquiQuadrate = dispSesquiQuadrate as! Int
            }
            if let dispSemiSquare = jsonObject["dispSemiSquare"] {
                setting.dispSemiSquare = dispSemiSquare as! Int
            }
            if let dispSemiSextile = jsonObject["dispSemiSextile"] {
                setting.dispSemiSextile = dispSemiSextile as! Int
            }
            if let dispQuintile = jsonObject["dispQuintile"] {
                setting.dispQuintile = dispQuintile as! Int
            }
            if let dispNovile = jsonObject["dispNovile"] {
                setting.dispNovile = dispNovile as! Int
            }
            if let dispAspectBiQuintile = jsonObject["dispAspectBiQuintile"] {
                setting.dispAspectBiQuintile = dispAspectBiQuintile as! Int
            }
            if let dispAspectSemiQuintile = jsonObject["dispAspectSemiQuintile"] {
                setting.dispAspectSemiQuintile = dispAspectSemiQuintile as! Int
            }
            if let dispAspectSeptile = jsonObject["dispAspectSeptile"] {
                setting.dispAspectSeptile = dispAspectSeptile as! Int
            }
            if let dispAspectQuindecile = jsonObject["dispAspectQuindecile"] {
                setting.dispAspectQuindecile = dispAspectQuindecile as! Int
            }
            if let orbSunMoon = jsonObject["orbSunMoon"] {
                setting.orbSunMoon[0] = ((orbSunMoon as! NSArray)[0] as! NSNumber).floatValue
                setting.orbSunMoon[1] = ((orbSunMoon as! NSArray)[1] as! NSNumber).floatValue
            }
            if let orb1st = jsonObject["orb1st"] {
                setting.orb1st[0] = ((orb1st as! NSArray)[0] as! NSNumber).floatValue
                setting.orb1st[1] = ((orb1st as! NSArray)[1] as! NSNumber).floatValue
            }
            if let orb2nd = jsonObject["orb2nd"] {
                setting.orb2nd[0] = ((orb2nd as! NSArray)[0] as! NSNumber).floatValue
                setting.orb2nd[1] = ((orb2nd as! NSArray)[1] as! NSNumber).floatValue
            }
            if let progression = jsonObject["progression"] {
                setting.progression = progression as! Int
            }
            if let houseCalc = jsonObject["houseCalc"] {
                setting.houseCalc = houseCalc as! Int
            }
            if let sameCusps = jsonObject["sameCusps"] {
                setting.sameCusps = sameCusps as! Bool
            }

            
            return setting
        } catch {
            print("error settingLoad")
            print(error.localizedDescription)
        }
        
        return nil
    }
}
