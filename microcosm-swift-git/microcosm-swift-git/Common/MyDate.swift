//
//  MyDate.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/05/06.
//

import Foundation

public class MyDate {
    public var date = Date()
    
    init(date: Date = Date()) {
        self.date = date
    }
    
    
    /// setDate(ハイフン区切り)
    /// - Parameter content: YYYY-MM-DD
    public func setAmateruYmd(content: String) {
        setAmateruYmdHis(content: content + " 12:00:00")
    }
    
    
    /// setDate(ハイフン区切り)
    /// - Parameter content: YYYY-MM-DD HH:ii:ss
    public func setAmateruYmdHis(content: String) {
        let formatter: DateFormatter = DateFormatter()
        formatter.timeZone = TimeZone(identifier: "Asia/Tokyo")
        formatter.dateFormat = "yyyy-MM-dd HH:mm:ss"
        self.date = formatter.date(from: content)!
    }
    
    /// setDate(スラッシュ区切り)
    /// - Parameter content: YYYY/MM/DD
    public func setNormalYmd(content: String) -> Void {
        let formatter: DateFormatter = DateFormatter()
        formatter.timeZone = TimeZone(identifier: "Asia/Tokyo")
        formatter.dateFormat = "yyyy/MM/dd"
        self.date = formatter.date(from: content)!
    }
    
    /// setDate(スラッシュ区切り)
    /// - Parameter content: YYYY/MM/DD HH:ii:ss
    public func setNormalYmdHis(content: String) -> Void {
        let formatter: DateFormatter = DateFormatter()
        formatter.timeZone = TimeZone(identifier: "Asia/Tokyo")
        formatter.dateFormat = "yyyy/MM/dd HH:mm:ss"
        self.date = formatter.date(from: content)!
    }
    
    /// setDate(スラッシュ区切り)
    /// - Parameter content: YYYY/MM/DD HH:ii:ss
    public func setUTCYmdHis(content: String) -> Void {
        let formatter: DateFormatter = DateFormatter()
        formatter.timeZone = TimeZone(identifier: "UTC")
        formatter.dateFormat = "yyyy/MM/dd HH:mm:ss"
        self.date = formatter.date(from: content)!
    }
    
    public func getFullYear() -> Int {
        let calendar = Calendar(identifier: .gregorian)
        let dateComponent = calendar.dateComponents(in: TimeZone.current, from: self.date)
        return dateComponent.year ?? 2000
        //        let formatter = DateFormatter()
        //        formatter.timeZone = TimeZone(identifier: "Asia/Tokyo")
        //        formatter.dateFormat = "Y"
        //        formatter.locale = Locale(identifier: "ja_JP")
        //        let year = formatter.string(from: date)
        //
        //        return Int(year) ?? 2000
    }
    
    public func getUTCFullYear() -> Int {
        let calendar = Calendar(identifier: .gregorian)
        let dateComponent = calendar.dateComponents(in: TimeZone(identifier: "GMT")!, from: self.date)
        return dateComponent.year ?? 2000
        //        let formatter = DateFormatter()
        //        formatter.timeZone = TimeZone(identifier: "GMT")
        //        formatter.dateFormat = "Y"
        //        let year = formatter.string(from: date)
        //
        //        return Int(year) ?? 1
    }
    
    public func getMonth() -> Int {
        let calendar = Calendar(identifier: .gregorian)
        let dateComponent = calendar.dateComponents(in: TimeZone.current, from: self.date)
        return dateComponent.month ?? 1
        //        let formatter = DateFormatter()
        //        formatter.timeZone = TimeZone(identifier: "Asia/Tokyo")
        //        formatter.dateFormat = "M"
        //        formatter.locale = Locale(identifier: "ja_JP")
        //        let year = formatter.string(from: date)
        //
        //        return Int(year) ?? 1
    }
    
    public func getUTCMonth() -> Int {
        let formatter = DateFormatter()
        formatter.timeZone = TimeZone(identifier: "GMT")
        formatter.dateFormat = "M"
        let month = formatter.string(from: date)
        
        return Int(month) ?? 1
    }
    
    public func getDay() -> Int {
        let calendar = Calendar(identifier: .gregorian)
        let dateComponent = calendar.dateComponents(in: TimeZone.current, from: self.date)
        return dateComponent.day ?? 1
        //        let formatter = DateFormatter()
        //        formatter.timeZone = TimeZone(identifier: "Asia/Tokyo")
        //        formatter.dateFormat = "d"
        //        let year = formatter.string(from: date)
        //
        //        return Int(year) ?? 1
    }
    
    public func getUTCDay() -> Int {
        let calendar = Calendar(identifier: .gregorian)
        let dateComponent = calendar.dateComponents(in: TimeZone(identifier: "GMT")!, from: self.date)
        return dateComponent.day ?? 1
        
        //        let formatter = DateFormatter()
        //        formatter.timeZone = TimeZone(identifier: "GMT")
        //        formatter.dateFormat = "d"
        //        let year = formatter.string(from: date)
        //
        //        return Int(year) ?? 1
    }
    
    public func getHour() -> Int {
        let calendar = Calendar(identifier: .gregorian)
        let dateComponent = calendar.dateComponents(in: TimeZone.current, from: self.date)
        return dateComponent.hour ?? 0
        
        //        let formatter = DateFormatter()
        //        formatter.timeZone = TimeZone(identifier: "Asia/Tokyo")
        //        formatter.dateFormat = "H"
        //        let year = formatter.string(from: date)
        //
        //        return Int(year) ?? 1
    }
    
    public func getUTCHour() -> Int {
        let calendar = Calendar(identifier: .gregorian)
        let dateComponent = calendar.dateComponents(in: TimeZone(identifier: "GMT")!, from: self.date)
        return dateComponent.hour ?? 0
        
        //        let formatter = DateFormatter()
        //        formatter.timeZone = TimeZone(identifier: "GMT")
        //        formatter.dateFormat = "H"
        //        let year = formatter.string(from: date)
        //
        //        return Int(year) ?? 1
    }
    
    public func getMinute() -> Int {
        let calendar = Calendar(identifier: .gregorian)
        let dateComponent = calendar.dateComponents(in: TimeZone.current, from: self.date)
        return dateComponent.minute ?? 0
        
        //        let formatter = DateFormatter()
        //        formatter.timeZone = TimeZone(identifier: "Asia/Tokyo")
        //        formatter.dateFormat = "m"
        //        let year = formatter.string(from: date)
        //
        //        return Int(year) ?? 1
    }
    
    public func getSecond() -> Int {
        let calendar = Calendar(identifier: .gregorian)
        let dateComponent = calendar.dateComponents(in: TimeZone.current, from: self.date)
        return dateComponent.second ?? 0
        //
        //
        //        let formatter = DateFormatter()
        //        formatter.timeZone = TimeZone(identifier: "Asia/Tokyo")
        //        formatter.dateFormat = "s"
        //        let year = formatter.string(from: date)
        //
        //        return Int(year) ?? 1
    }
    
    public func getUTCDateObj() -> Date {
        let calendar = Calendar(identifier: .gregorian)
        let dateComponent = calendar.dateComponents(in: TimeZone(identifier: "GMT")!, from: self.date)
        return dateComponent.date!
    }
    
    public func getDateObj() -> Date {
        let calendar = Calendar(identifier: .gregorian)
        let dateComponent = calendar.dateComponents(in: TimeZone.current, from: self.date)
        return dateComponent.date!
    }
    
    public func setYear(year: Int) {
        let calendar = Calendar(identifier: .gregorian)
        self.date = calendar.date(bySetting: .year, value: year, of: self.date)!
    }
    
    public func setMonth(month: Int) {
        let calendar = Calendar(identifier: .gregorian)
        self.date = calendar.date(bySetting: .month, value: month, of: self.date)!
    }
    
    public func setDay(day: Int) {
        let calendar = Calendar(identifier: .gregorian)
        self.date = calendar.date(bySetting: .day, value: day, of: self.date)!
    }
    
    public func setHour(hour: Int) {
        let calendar = Calendar(identifier: .gregorian)
        self.date = calendar.date(bySetting: .hour, value: hour, of: self.date)!
    }
    
    public func setMinute(minute: Int) {
        let calendar = Calendar(identifier: .gregorian)
        self.date = calendar.date(bySetting: .minute, value: minute, of: self.date)!
    }
    
    public func setSecond(second: Int) {
        let calendar = Calendar(identifier: .gregorian)
        self.date = calendar.date(bySetting: .second, value: second, of: self.date)!
    }
    
    public func getFullFormat() -> String {
        let formatter = DateFormatter()
        formatter.timeZone = TimeZone(identifier: "Asia/Tokyo")
        formatter.dateFormat = "yyyy/MM/dd HH:mm:ss"
        let year = formatter.string(from: date)
        return year
    }
    
    public func getAmateruString() -> String {
        let formatter = DateFormatter()
        formatter.timeZone = TimeZone(identifier: "Asia/Tokyo")
        formatter.dateFormat = "yyyyMMddHHmmss"
        let s = "Amateru" + formatter.string(from: date)
        return s
    }
    
    public func getStarGazerString() -> String {
        let formatter = DateFormatter()
        formatter.timeZone = TimeZone(identifier: "Asia/Tokyo")
        formatter.dateFormat = "yyyyMMddHHmmss"
        let s = "StarGazer" + formatter.string(from: date)
        return s
    }
    
    public func getZetString() -> String {
        let formatter = DateFormatter()
        formatter.timeZone = TimeZone(identifier: "Asia/Tokyo")
        formatter.dateFormat = "yyyyMMddHHmmss"
        let s = "Zet" + formatter.string(from: date)
        return s
    }
    
    public func getCsvString() -> String {
        let formatter = DateFormatter()
        formatter.timeZone = TimeZone(identifier: "Asia/Tokyo")
        formatter.dateFormat = "yyyyMMddHHmmss"
        let s = "Csv" + formatter.string(from: date)
        return s
    }
    
    public func createDateFromYmdHis(year: String, month: String, day: String, hour: String, minute: String, second: String) -> Date {
        let s = year + month + day + hour + minute + second
        let formatter = DateFormatter()
        formatter.dateFormat = "yyyyMMddHHmmss"
        return formatter.date(from: s)!
    }
    
    
    /// UserDataの時刻を+ or - する
    /// - Parameters:
    ///   - userData: userData
    ///   - second: + or -する秒数
    /// - Returns: 変更後のUserData
    public func addUserData(userData: UserData, second: Int) -> UserData {
        let s = String(userData.birth_year) + String(format: "%02d", userData.birth_month) + String(format: "%02d", userData.birth_day) + String(format: "%02d", userData.birth_hour) + String(format: "%02d", userData.birth_minute) + String(format: "%02d", userData.birth_second)
        
        let formatter = DateFormatter()
        formatter.dateFormat = "yyyyMMddHHmmss"
        var date = formatter.date(from: s)!
        date.addTimeInterval(Double(second))
        
        formatter.timeZone = TimeZone(identifier: "Asia/Tokyo")
        formatter.dateFormat = "y"
        let year = formatter.string(from: date)
        formatter.dateFormat = "M"
        let month = formatter.string(from: date)
        formatter.dateFormat = "d"
        let day = formatter.string(from: date)
        formatter.dateFormat = "H"
        let hour = formatter.string(from: date)
        formatter.dateFormat = "m"
        let minute = formatter.string(from: date)
        formatter.dateFormat = "s"
        let second = formatter.string(from: date)
        
        var newUserData = userData
        
        newUserData.birth_year = Int(year)!
        newUserData.birth_month = Int(month)!
        newUserData.birth_day = Int(day)!
        newUserData.birth_hour = Int(hour)!
        newUserData.birth_minute = Int(minute)!
        newUserData.birth_second = Int(second)!
        
        return newUserData
    }
    
    
    /// UserDataに現在時刻を設定する
    /// - Parameter userData: userData
    /// - Returns: newUserData
    public func nowUserData(userData: UserData) -> UserData {
        let date = Date()
        
        let formatter = DateFormatter()
        formatter.timeZone = TimeZone(identifier: "Asia/Tokyo")
        formatter.dateFormat = "y"
        let year = formatter.string(from: date)
        formatter.dateFormat = "M"
        let month = formatter.string(from: date)
        formatter.dateFormat = "d"
        let day = formatter.string(from: date)
        formatter.dateFormat = "H"
        let hour = formatter.string(from: date)
        formatter.dateFormat = "m"
        let minute = formatter.string(from: date)
        formatter.dateFormat = "s"
        let second = formatter.string(from: date)
        
        var newUserData = userData
        
        newUserData.birth_year = Int(year)!
        newUserData.birth_month = Int(month)!
        newUserData.birth_day = Int(day)!
        newUserData.birth_hour = Int(hour)!
        newUserData.birth_minute = Int(minute)!
        newUserData.birth_second = Int(second)!
        
        return newUserData
    }
    
    public func setUserData(u: UserData)
    {
        let calendar = Calendar(identifier: .gregorian)
        let date = calendar.date(from: DateComponents(year: u.birth_year, month: u.birth_month, day: u.birth_day))
        
        self.date = Calendar.current.date(bySettingHour: u.birth_hour, minute: u.birth_minute, second: u.birth_second, of: date!)!
    }
    
    public func addDays(day: Double) {
        let add: Double = day * 24 * 60 * 60
        self.date.addTimeInterval(add)
    }
    
    public func addHours(hour: Double) {
        let add: Double = hour * 60 * 60
        self.date.addTimeInterval(add)
    }
    
    public func addMinutes(minute: Double) {
        let add: Double = minute * 60
        self.date.addTimeInterval(add)
    }
    
    public func addSeconds(second: Double) {
        self.date.addTimeInterval(second)
    }
    
    /// ユリウス日計算、swe_utc_to_jdと一緒
    /// - Returns: JuliusDay
    public func getJulius() -> Double
    {
        var calendar = Calendar(identifier: .gregorian)
        if let timeZone = TimeZone(identifier: "GMT") {
            calendar.timeZone = timeZone
        }
        let date = calendar.date(from: DateComponents(year: 1858, month: 11, day: 17, hour: 0, minute: 0, second: 0))
        let unix = getDateObj().timeIntervalSince(date!)
        let unixdays = unix / 60 / 60 / 24
        // ここまでが修正ユリウス日
        
        return unixdays + 2400000.50000
    }
    
    /// UTCにしてユリウス日計算、swe_utc_to_jdと一緒
    /// - Returns: JuliusDay
    public func getUTCJulius() -> Double
    {
        var calendar = Calendar(identifier: .gregorian)
        if let timeZone = TimeZone(identifier: "GMT") {
            calendar.timeZone = timeZone
        }
        let date = calendar.date(from: DateComponents(year: 1858, month: 11, day: 17, hour: 0, minute: 0, second: 0))
        let unix = getUTCDateObj().timeIntervalSince(date!)
        let unixdays = unix / 60 / 60 / 24
        // ここまでが修正ユリウス日
        
        return unixdays + 2400000.50000
    }
}
