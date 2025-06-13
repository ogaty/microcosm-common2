//
//  UserData.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/03/30.
//

import Foundation

public struct UserData: Codable {
    var name: String = "現在時刻"
    var lat: Double = 38
    var lng: Double = 139
    var birth_year: Int = 2024
    var birth_month: Int = 1
    var birth_day: Int = 1
    var birth_hour: Int = 12
    var birth_minute: Int = 0
    var birth_second: Int = 0

    // 9とかの数字
    var birth_timezone: Double = 9.0
    // edit時に表示されるindex
    var birth_timezone_index: Int = 38

    var birth_place: String = "東京都"
    var memo: String = ""
    
    var type: String = "NATAL"
    var kana: String = ""
}

public struct UserList: Codable {
    var list: [UserData]
}

public class UserSave {
    static func save(user: UserList, url: URL) {
        let encoder = JSONEncoder()
        encoder.outputFormatting = .prettyPrinted //JSONデータを整形する
        guard let jsonValue = try? encoder.encode(user) else {
            fatalError("JSONエンコードエラー")
        }
         
        do {
            try jsonValue.write(to: url)
        } catch {
            fatalError("JSON書き込みエラー")
        }
        print("saved")

    }
}

extension UserList {
    public mutating func load(url: URL) {
        do {
            let data = try Data(contentsOf: url)
            let jsonObject: [String: Any] = try JSONSerialization.jsonObject(with: data, options: [.mutableContainers, .mutableLeaves]) as! [String: Any]
            
            self.list.removeAll()
            for case let result in jsonObject["list"] as! NSArray {
                var userData = UserData()
                let result0 = result as! NSDictionary
                
                let birthYear = result0["birth_year"] as! Int
                let birthMonth = result0["birth_month"] as! Int
                let birthDay = result0["birth_day"] as! Int
                let birthHour = result0["birth_hour"] as! Int
                let birthMinute = result0["birth_minute"] as! Int
                let birthSecond = result0["birth_second"] as! Int

                userData.name = result0["name"] as! String
                userData.memo = result0["memo"] as! String
                userData.birth_place = result0["birth_place"] as! String
                userData.lat = result0["lat"] as! Double
                userData.lng = result0["lat"] as! Double
                userData.birth_year = birthYear
                userData.birth_month = birthMonth
                userData.birth_day = birthDay
                userData.birth_hour = birthHour
                userData.birth_minute = birthMinute
                userData.birth_second = birthSecond
                userData.birth_timezone = result0["birth_timezone"] as! Double
                userData.birth_timezone_index = result0["birth_timezone_index"] as! Int

                self.list.append(userData)
            }
        } catch {
            print("error userLoad")
        }
    }
}

extension UserData {
    public mutating func setDate(date: MyDate)
    {
        self.birth_year = date.getFullYear()
        self.birth_month = date.getMonth()
        self.birth_day = date.getDay()
        self.birth_hour = date.getHour()
        self.birth_minute = date.getMinute()
        self.birth_second = date.getSecond()
    }
}
