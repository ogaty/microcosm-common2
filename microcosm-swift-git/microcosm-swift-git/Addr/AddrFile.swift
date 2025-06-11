//
//  AddrFile.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/02/05.
//

import Foundation
import SwiftCSV

// 住所csv関連
public class AddrFile
{
    public func readCsv(u: URL) -> [Addr]
    {
        var addrs: [Addr] = []
        do {
            let systemURL = u.appendingPathComponent("/system")
            let addrFile = systemURL
                .appendingPathComponent("/addr.csv")

            let csv = try CSV<Named>(url: addrFile)
            try csv.enumerateAsDict { dict in
                if let lat = Double(dict["lat"]!) {
                    if let lng = Double(dict["lng"]!) {
                        addrs.append(Addr(name: dict["name"]!, lat: lat, lng: lng))
                    }else{
                        print("数値に変換できません")
                        return
                    }
                }else{
                    print("数値に変換できません")
                    return
                }
            }
        } catch
        {
            print("予期せぬエラーが発生しました:Addr")
            print(error.localizedDescription)
        }

        return addrs
    }
    
    public func writeCsv()
    {
        
    }
}
