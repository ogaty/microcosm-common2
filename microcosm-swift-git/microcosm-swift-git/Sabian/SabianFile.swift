//
//  SabianFile.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/02/05.
//

import Foundation

import SwiftCSV

public class SabianFile
{
    public func readCsv(u: URL) -> [Sabian]
    {
        var sabians: [Sabian] = []
        
        do {
            let systemURL = u.appendingPathComponent("/system")
            let sabianFile = systemURL
                .appendingPathComponent("/sabian.csv")

            let csv = try CSV<Named>(url: sabianFile)
            try csv.enumerateAsDict { dict in
                if let degree = Double(dict["degree"]!) {
                    sabians.append(Sabian(degree: degree, jp: dict["jp"]!, text: dict["text"]!))
                } else {
                    print("数値に変換できません")
                    return
                }
                
//                if (dict["lat"] != nil) {
//                    print(dict["lat"]!)
//                    print(dict["lng"]!)
//                }
            }
        } catch {
            print("予期せぬエラーが発生しました:Sabian")
            print(error.localizedDescription)
        }
        
        return sabians
    }
}
