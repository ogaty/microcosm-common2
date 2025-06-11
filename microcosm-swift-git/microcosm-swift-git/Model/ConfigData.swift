//
//  ConfigJson.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/02/05.
//

import Cocoa

struct ConfigData: Codable {
    var ephePath: String = "ephe"
    var centric: ECentric = ECentric.GEO_CENTRIC
    var sideReal: ESideReal = ESideReal.TROPICAL
    var nodeCalc: ENodeCalc = ENodeCalc.TRUE
    var lilithCalc: ELilithCalc = ELilithCalc.OSCU
    
    var defaultPlace: String = "東京都"
    var defaultLat: String = "35.68944"
    var defaultLng: String = "139.69167"
    var defaultTimeZone: String = "Asia/Tokyo (+9:00)"
}

public class ConfigSave {
    static func save(config: ConfigData, url: URL) -> Void {
        let fileURL = url.appendingPathComponent("/system").appendingPathComponent("config.json")

        let encoder = JSONEncoder()
        encoder.outputFormatting = .prettyPrinted //JSONデータを整形する
        guard let jsonValue = try? encoder.encode(config) else {
            fatalError("JSONエンコードエラー")
        }
         
        do {
            try jsonValue.write(to: fileURL)
        } catch {
            fatalError("JSON書き込みエラー")
        }
    }
    
    static func load(url: URL) -> ConfigData? {
        let fileURL = url.appendingPathComponent("/system").appendingPathComponent("config.json")
        do {
            let data = try Data(contentsOf: fileURL)
            let jsonObject: [String: Any] = try JSONSerialization.jsonObject(with: data, options: [.mutableContainers, .mutableLeaves]) as! [String: Any]
            
            // 毎回インスタンス作って良いのか、本来は同じインスタンスに上書きしたほうが良い気もする
            var config = ConfigData()
            if let ephe = jsonObject["ephePath"] {
                config.ephePath = ephe as! String
            }
            if let centric = jsonObject["centric"] {
                if centric as! Int64 == 0 {
                    config.centric = ECentric.GEO_CENTRIC
                } else if centric as! Int64 == 1 {
                    config.centric = ECentric.HELIO_CENTRIC
                }
            }
            if let sideReal = jsonObject["sidereal"] {
                if sideReal as! Int64 == 0 {
                    config.sideReal = ESideReal.TROPICAL
                } else if sideReal as! Int64 == 1 {
                    config.sideReal = ESideReal.SIDEREAL
                } else if sideReal as! Int64 == 2 {
                    config.sideReal = ESideReal.DRACONIC
                }
            }
            if let nodeCalc = jsonObject["nodeCalc"] {
                if nodeCalc as! Int64 == 0 {
                    config.nodeCalc = ENodeCalc.TRUE
                } else if nodeCalc as! Int64 == 1 {
                    config.nodeCalc = ENodeCalc.MEAN
                }
            }
            if let lilithCalc = jsonObject["lilithCalc"] {
                if lilithCalc as! Int64 == 0 {
                    config.lilithCalc = ELilithCalc.OSCU
                } else if lilithCalc as! Int64 == 1 {
                    config.lilithCalc = ELilithCalc.MEAN
                }
            }
            if let timezone = jsonObject["defaultTimeZone"] {
                config.defaultTimeZone = timezone as! String
            }
            if let place = jsonObject["defaultPlace"] {
                config.defaultPlace = place as! String
            }
            if let lat = jsonObject["defaultLat"] {
                config.defaultLat = lat as! String
            }
            if let lng = jsonObject["defaultLng"] {
                config.defaultLng = lng as! String
            }

            return config
        } catch {
            print("error configLoad")
        }
        
        return nil
    }
}
