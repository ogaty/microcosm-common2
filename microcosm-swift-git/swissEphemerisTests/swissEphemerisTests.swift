//
//  swissEphemerisTests.swift
//  swissEphemerisTests
//
//  Created by 緒形雄二 on 2025/06/11.
//

import XCTest
@testable import swissEphemeris

class swissEphemerisTests: XCTestCase {
    
    override func setUpWithError() throws {
        // Put setup code here. This method is called before the invocation of each test method in the class.
    }
    
    override func tearDownWithError() throws {
        // Put teardown code here. This method is called after the invocation of each test method in the class.
    }
    
    func testSetEphePath() throws {
        let swiss = swissEphemerisMain()
        swiss.set_ephe_path()
        // set_ephe_pathは戻り値なし
        XCTAssertTrue(true)
    }
    
    func testUtcToJulian() {
        let swiss = swissEphemerisMain()
        let julian = swiss.swe_utc_to_jd(year: 2024, month: 1, day: 1, hour: 12, minute: 0, second: 0)
        XCTAssertEqual(2460311, Int(julian.dret[0]))
        XCTAssertEqual("", julian.err)
    }
    
    func testUtcTimeZone() {
        let utcYear: Int32 = 2024
        let utcMonth: Int32 = 1
        let utcDay: Int32 = 1
        let utcHour: Int32 = 12
        let utcMinute: Int32 = 0
        let utcSecond: Double = 0
        let timeZone: Double = 9.0
        let swiss = swissEphemerisMain()
        let utc = swiss.utc_time_zone(year: utcYear, month: utcMonth, day: utcDay, hour: utcHour, minute: utcMinute, second: utcSecond, timeZone: timeZone)
        XCTAssertEqual(2024, Int(utc.year))
        XCTAssertEqual(1, Int(utc.month))
        XCTAssertEqual(1, Int(utc.day))
        XCTAssertEqual(3, Int(utc.hour))
        XCTAssertEqual(0, Int(utc.minute))
        XCTAssertEqual(0, Int(utc.second))
    }
    
    func testCalcUT() {
        let swiss = swissEphemerisMain()
        swiss.set_ephe_path()
        
        // 2460219.0: 2023-10-01 21:00:00 JSTの値
        // (UTCだと2023-10-01 12:00:00)
        
        var calc = swiss.calc_ut(jd: 2460219.0, planetNo: 0, flag: 258)
        // 太陽:天秤座8度
        XCTAssertEqual("", calc.err)
        XCTAssertEqual(188, Int(calc.x[0]))
        XCTAssertTrue(calc.x[3] > 0)
        
        calc = swiss.calc_ut(jd: 2460219.0, planetNo: 1, flag: 258)
        // 月:牡牛座6度
        XCTAssertEqual("", calc.err)
        XCTAssertEqual(36, Int(calc.x[0]))
        XCTAssertTrue(calc.x[3] > 0)
        
        calc = swiss.calc_ut(jd: 2460219.0, planetNo: 9, flag: 258)
        // 冥王星:山羊座27度(逆行)
        XCTAssertEqual("", calc.err)
        XCTAssertEqual(297, Int(calc.x[0]))
        XCTAssertTrue(calc.x[3] < 0)
    }
    
    func testHouse() {
        let swiss = swissEphemerisMain()
        swiss.set_ephe_path()
        var house = swiss.swe_houses(jd: 2460219.0, lat: 35.45, lng: 139.61, houseSystem: "P")
        XCTAssertEqual(78, Int(house.cusps[1]))
        XCTAssertEqual(100, Int(house.cusps[2]))
        XCTAssertEqual(122, Int(house.cusps[3]))
        XCTAssertEqual(147, Int(house.cusps[4]))
        
        house = swiss.swe_houses(jd: 2460219.0, lat: 35.45, lng: 139.61, houseSystem: "K")
        XCTAssertEqual(78, Int(house.cusps[1]))
        XCTAssertEqual(102, Int(house.cusps[2]))
        XCTAssertEqual(125, Int(house.cusps[3]))
        XCTAssertEqual(147, Int(house.cusps[4]))
    }
    
    func testPerformanceExample() throws {
        // This is an example of a performance test case.
        self.measure {
            // Put the code you want to measure the time of here.
        }
    }
}
