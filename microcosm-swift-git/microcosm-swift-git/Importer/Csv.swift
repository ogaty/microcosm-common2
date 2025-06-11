//
//  Csv.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/05/13.
//

import Foundation

public class Csv {
    public static func read(content: String, defaultLat: String, defaultLng: String) -> UserList
    {
        var userList: UserList = UserList(list: [UserData]())
        let l = content.split(whereSeparator: \.isNewline)
        for x in l {
            let trimData = x.split(separator: ",")
            // data[0] name
            // data[1] y
            // data[2] m
            // data[3] d
            // data[4] H
            // data[5] i
            // data[6] s
            // data[7] place
            // data[8] lat
            // data[9] lng
            
            let name = trimData[0]
            let year = trimData[1]
            let month = trimData[2]
            let day = trimData[3]
            let hour = trimData[4]
            let minute = trimData[5]
            let second = trimData[6]
            let place = trimData[7]
            let lat = trimData[8]
            let lng = trimData[9]
            
            var userData = UserData()
            userData.name = String(name)
            userData.birth_year = Int(year)!
            userData.birth_month = Int(month)!
            userData.birth_day = Int(day)!
            userData.birth_hour = Int(hour)!
            userData.birth_minute = Int(minute)!
            userData.birth_second = Int(second)!
            
            userData.lat = Double(lat)!
            userData.lng = Double(lng)!
            
            userList.list.append(userData)
        }

        return userList
    }
}
