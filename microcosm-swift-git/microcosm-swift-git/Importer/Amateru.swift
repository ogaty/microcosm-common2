//
//  Amateru.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/05/06.
//

import Foundation
import SwiftCSV

public class Amateru {
    
    public static func readCsv(content: String) -> UserList
    {
        var data: UserList = UserList(list: [UserData]())
        
        do {
            let csv = try CSV<Enumerated>(string: content, delimiter: .tab)
            try csv.enumerateAsDict { dict in
                var x = UserData()
                x.type = dict["TYPE"] ?? "NATAL"
                x.name = dict["NAME"] ?? "no name"
                x.kana = dict["KANA"] ?? ""
                x.birth_place = dict["PLACENAME"] ?? "東京都"
                var gender = dict["GENDER"] ?? "1"
                if (gender == "1") {
                    gender = "性別:男 "
                } else if (gender == "2") {
                    gender = "性別:女 "
                } else {
                    gender = "性別:不明 "
                }
                let jobMemo = dict["JOB_MEMO"] ?? ""
                let memo = dict["NOTE"] ?? ""
                x.memo = gender + jobMemo + " " + memo
                
                let date = dict["DATE"] ?? "2024-01-01"
                let time = dict["TIME"] ?? "12:00:00"
                let timezoneIndex = TimeZoneConst.TimeZoneSearch(content: dict["TIMEZONE"] ?? "JST")
                if (timezoneIndex == -1) {
                    x.birth_timezone_index = 38
                    x.birth_timezone = 9.0
                } else {
                    x.birth_timezone_index = timezoneIndex
                    x.birth_timezone = TimeZoneConst.timezone_time[timezoneIndex]
                }

                let myDate = MyDate()
                myDate.setAmateruYmdHis(content: date + " " + time)
                
                x.birth_year = myDate.getFullYear()
                x.birth_month = myDate.getMonth()
                x.birth_day = myDate.getDay()
                x.birth_hour = myDate.getHour()
                x.birth_minute = myDate.getMinute()
                x.birth_second = myDate.getSecond()
                
                data.list.append(x)
            }
        } catch
        {
            print("予期せぬエラーが発生しました:Addr")
            print(error.localizedDescription)
        }
        
        return data
    }
}
