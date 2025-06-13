//
//  DatabaseUser.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/03/30.
//

import Foundation

struct databaseVal: Codable {
    var name: String
    var birth_year: Int
    var birth_month: Int
    var birth_day: Int
    var birth_hour: Int
    var birth_minute: Int
    var birth_second: Int
}
struct databaseObj: Codable {
    var list: [databaseVal]
    
}

class DatabaseUser: NSObject {
    var name: String
    var birthStr: String
    var memo: String
    var place: String
    var userData: UserData
    var url: URL
    
    init(name: String, birthStr: String, memo: String, url: URL) {
        self.name = name
        self.birthStr = birthStr
        self.memo = ""
        self.place = ""
        self.url = url
        self.userData = UserData()
    }
    
    static func load(url: URL) -> [DatabaseUser] {
        var users: [DatabaseUser] = [DatabaseUser]()
        do {
            let data = try Data(contentsOf: url)
            let jsonObject: [String: Any] = try JSONSerialization.jsonObject(with: data, options: [.mutableContainers, .mutableLeaves]) as! [String: Any]
            
            for case let result in jsonObject["list"] as! NSArray {
                let user = DatabaseUser(name: "", birthStr: "", memo: "", url: url)
                let result0 = result as! NSDictionary
                user.name = result0["name"] as! String

                let birthYear = result0["birth_year"] as! Int
                let birthMonth = result0["birth_month"] as! Int
                let birthDay = result0["birth_day"] as! Int
                let birthHour = result0["birth_hour"] as! Int
                let birthMinute = result0["birth_minute"] as! Int
                let birthSecond = result0["birth_second"] as! Int

                user.birthStr = String(format: "%04d/%02d/%02d %02d:%02d:%02d", birthYear, birthMonth, birthDay, birthHour, birthMinute, birthSecond)

                user.memo = result0["memo"] as! String
                user.place = result0["birth_place"] as! String
                user.userData.name = user.name
                user.userData.memo = user.memo
                user.userData.birth_place = result0["birth_place"] as! String
                user.userData.lat = result0["lat"] as! Double
                user.userData.lng = result0["lat"] as! Double
                user.userData.birth_year = birthYear
                user.userData.birth_month = birthMonth
                user.userData.birth_day = birthDay
                user.userData.birth_hour = birthHour
                user.userData.birth_minute = birthMinute
                user.userData.birth_second = birthSecond
                user.userData.birth_timezone = result0["birth_timezone"] as! Double
                user.userData.birth_timezone_index = result0["birth_timezone_index"] as! Int
                users.append(user)
            }
            
            return users
        } catch {
            print("error userLoad")
        }
        
        return users
    }
}
