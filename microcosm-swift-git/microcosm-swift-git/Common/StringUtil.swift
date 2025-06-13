//
//  StringUtil.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/05/07.
//

import Foundation

public class StringUtil {
    
    
    /// 20240304から2024を取得
    /// - Parameter content: 20240304
    /// - Returns: 2024
    public static func getYearFromYmd(content: String) -> String {
        return String(content[content.startIndex...content.index(content.startIndex, offsetBy: 3)])
    }
    
    /// 20240304から03を取得
    /// - Parameter content: 20240304
    /// - Returns: 03
    public static func getMonthFromYmd(content: String) -> String {
        return String(content[content.index(content.startIndex, offsetBy: 4)...content.index(content.startIndex, offsetBy: 5)])
    }

    /// 20240304から04を取得
    /// - Parameter content: 20240304
    /// - Returns: 04
    public static func getDayFromYmd(content: String) -> String {
        return String(content[content.index(content.startIndex, offsetBy: 6)...content.index(content.startIndex, offsetBy: 7)])
    }

    /// 030405から03を取得
    /// - Parameter content: 030405
    /// - Returns: 03
    public static func getHourFromYmd(content: String) -> String {
        return String(content[content.index(content.startIndex, offsetBy: 0)...content.index(content.startIndex, offsetBy: 1)])
    }

    /// 030405から04を取得
    /// - Parameter content: 030405
    /// - Returns: 04
    public static func getMinuteFromYmd(content: String) -> String {
        return String(content[content.index(content.startIndex, offsetBy: 2)...content.index(content.startIndex, offsetBy: 3)])
    }

    /// 030405から05を取得
    /// - Parameter content: 030405
    /// - Returns: 05
    public static func getSecondFromYmd(content: String) -> String {
        return String(content[content.index(content.startIndex, offsetBy: 4)...content.index(content.startIndex, offsetBy: 5)])
    }
}
