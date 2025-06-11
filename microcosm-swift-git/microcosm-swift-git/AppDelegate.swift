//
//  AppDelegate.swift
//  microcosm-swift-git
//
//  Created by 緒形雄二 on 2025/06/11.
//

import Cocoa
import swissEphemeris

@main
class AppDelegate: NSObject, NSApplicationDelegate {

    public var viewController: MainViewController? = nil
    public var databaseViewController: DatabaseViewController? = nil
    
    public var settingIndex: Int = 6
    public var dispNames: [DispName] = []
    public var settingData: [SettingData] = []
    public var currentSetting: SettingData = SettingData()
    public var config: ConfigData = ConfigData()
    public var chartStyle: ChartStyle = ChartStyle()
    
    public var currentSpanType: SpanType = SpanType.UNIT
    public var currentSpanUnitNum: Int = 1
    
    public var udata1: UserData = UserData()
    public var udata2: UserData = UserData()
    public var edata1: UserData = UserData()
    public var edata2: UserData = UserData()
    
    public var swiss: swissEphemerisMain = swissEphemerisMain()
    
    // innerRingとcenterRingの比率
    public var centerRingRatio = 50
    
    public var list1 = [Int: PlanetData]()
    public var list2 = [Int: PlanetData]()
    public var list3 = [Int: PlanetData]()
    
    // signTableに出すだけ
    // isDispだけ出す
    public var list1array = [PlanetData]()
    public var list2array = [PlanetData]()
    public var list3array = [PlanetData]()
    
    // リング数
    public var bands = 1
    
    // バンド種別
    public var firstBand: EBandKind  = EBandKind.TRANSIT
    public var secondBand: EBandKind = EBandKind.TRANSIT
    public var thirdBand: EBandKind  = EBandKind.TRANSIT


    func applicationDidFinishLaunching(_ aNotification: Notification) {
        // Insert code here to initialize your application
    }

    func applicationWillTerminate(_ aNotification: Notification) {
        // Insert code here to tear down your application
    }

    func applicationSupportsSecureRestorableState(_ app: NSApplication) -> Bool {
        return true
    }
    
    func applicationShouldTerminateAfterLastWindowClosed(_ sender: NSApplication) -> Bool {
        return true
    }


    @IBAction func saveChartToPng(_ sender: Any) {
        self.viewController?.canvas!.makeImage()
    }
    
    @IBAction func openDbFolder(_ sender: Any) {
        var u: URL
        u = FileInitialize.microcosmDirectory().appendingPathComponent("data")
        NSWorkspace.shared.open(u)
    }
    
    @IBAction func openAddrCsv(_ sender: Any) {
        var u: URL
        u = FileInitialize.microcosmDirectory().appendingPathComponent("system").appendingPathComponent("addr.csv")
        NSWorkspace.shared.open(u)
    }
    
    @IBAction func openSabianCsv(_ sender: Any) {
        var u: URL
        u = FileInitialize.microcosmDirectory().appendingPathComponent("system").appendingPathComponent("sabian.csv")
        NSWorkspace.shared.open(u)
    }
    
    @IBAction func chart1U1(_ sender: Any) {
        viewController!.chart1U1()
    }
    
    @IBAction func chart1U2(_ sender: Any) {
        viewController!.chart1U2()
    }
    
    @IBAction func chart1E1(_ sender: Any) {
        viewController!.chart1E1()
    }
    
    @IBAction func chart1E2(_ sender: Any) {
        viewController!.chart1E2()
    }
}

