//
//  StarGazer.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/05/07.
//

import Foundation

public class StarGazer {
    public static func read(content: String, defaultLat: String, defaultLng: String) -> UserList
    {
        var userList: UserList = UserList(list: [UserData]())
        let l = content.split(whereSeparator: \.isNewline)
        
        for x in l {
            if !x.contains(",") {
                continue
            }

            var u = UserData()
            let trimData = x.replacingOccurrences(of: "　", with: " ")
            let data = trimData.split(separator: " ")
            // data[0] ymd
            // data[1] his
            // data[2] lat
            // data[3] lng
            // data[4] other
            let ymd = String(data[0])
            let year = StringUtil.getYearFromYmd(content: ymd)
            let month = StringUtil.getMonthFromYmd(content: ymd)
            let day = StringUtil.getDayFromYmd(content: ymd)
            let hour = StringUtil.getHourFromYmd(content: ymd)
            let minute = StringUtil.getMinuteFromYmd(content: ymd)
            let second = StringUtil.getSecondFromYmd(content: ymd)
            
            u.birth_year = Int(year)!
            u.birth_month = Int(month)!
            u.birth_day = Int(day)!
            u.birth_hour = Int(hour)!
            u.birth_minute = Int(minute)!
            u.birth_second = Int(second)!
            
            var lat = ""
            if data[2] == "" {
                lat = defaultLat
            } else {
                lat = String(data[2])
            }
            u.lat = Double(lat)!
            
            var lng = ""
            if data[3] == "" {
                lng = defaultLng
            } else {
                lng = String(data[3])
            }
            u.lng = Double(lng)!

            let names = data[4].split(separator: ",")
            var names0 = String(names[0])
            names0 = names0.replacingOccurrences(of: "\"", with:  "")
            u.birth_place = names0
            var names1 = String(names[1])
            names1 = names1.replacingOccurrences(of: "\"", with:  "")
            u.name = names1
            
            var memo = ""
            memo = String(names[3]) + " " + String(names[2])
            u.memo = memo
            userList.list.append(u)
        }

        return userList
    }
}
