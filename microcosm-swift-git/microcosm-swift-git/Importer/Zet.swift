//
//  Zet.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/05/07.
//

import Foundation

public class Zet {
    public static func read(content: String, defaultLat: String, defaultLng: String) -> UserList
    {
        var userList: UserList = UserList(list: [UserData]())
        let l = content.split(whereSeparator: \.isNewline)
        for x in l {
            let trimData = x.split(separator: ";")
            if (trimData.count < 8) {
                continue;
            }
            // data[0] ymd
            // data[1] his
            // data[2] lat
            // data[3] lng
            // data[4] other
            let name = trimData[0]
            // dd.mm.yyyyじゃなくて一桁だったりする
            let d = trimData[1].trimmingCharacters(in: .whitespaces)
            let ymd = d.split(separator: ".")
            let year = ymd[2]
            let month = ymd[1]
            let day = ymd[0]
            
            let t = trimData[2].trimmingCharacters(in: .whitespaces)
            let his = t.split(separator: ":")
            let hour = his[0]
            let minute = his[1]
            let second = 0
            
            var userData = UserData()
            userData.name = String(name)
            userData.birth_year = Int(year)!
            userData.birth_month = Int(month)!
            userData.birth_day = Int(day)!
            userData.birth_hour = Int(hour)!
            userData.birth_minute = Int(minute)!
            userData.birth_second = second
            
            userData.lat = Double(defaultLat)!
            userData.lng = Double(defaultLng)!
            
            userData.memo = String(trimData[7])
            
            userList.list.append(userData)
        }

        return userList
    }
}
