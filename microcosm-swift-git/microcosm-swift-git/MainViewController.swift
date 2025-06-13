//
//  ViewController.swift
//  microcosm-swift-git
//
//  Created by 緒形雄二 on 2025/06/11.
//

import Cocoa

import swissEphemeris

class MainViewController: NSViewController {
    public var menu0: NSMenu? = NSMenu()
    public var menu1: NSMenuItem? = NSMenuItem()
    
    public var canvas: ChartView?
    
    public var swiss: swissEphemerisMain = swissEphemerisMain()
    
    // 秒
    public var plusUnit: Int = 60 * 60 * 24
    
    @IBOutlet weak var spanButton: NSButton!
    
    public var list1 = [Int: PlanetData]()
    public var list2 = [Int: PlanetData]()
    public var list3 = [Int: PlanetData]()
    public var houseList1 = [Double]()
    public var houseList2 = [Double]()
    public var houseList3 = [Double]()
    
    public var calcTargetUser: [ETargetUser] = [ETargetUser.EVENT1, ETargetUser.EVENT1, ETargetUser.EVENT1]
    
    @IBOutlet weak var user1Box: NSTextField!
    @IBOutlet weak var user2Box: NSTextField!
    @IBOutlet weak var event1Box: NSTextField!
    @IBOutlet weak var event2Box: NSTextField!
    
    @IBOutlet weak var timeSetterCombo: NSPopUpButton!
    
    @IBOutlet weak var text111: NSTextField!
    
    @IBOutlet weak var text222: NSTextField!
    
    @IBOutlet weak var centerRingSlider: NSSlider!
    
    @IBOutlet weak var settingCombo: NSPopUpButton!
    
    @IBOutlet weak var signTable: NSTableView!
    @IBOutlet weak var houseTable: NSTableView!
    
    @IBOutlet weak var leftSide: NSTextField!
    @IBOutlet weak var bottomSide: NSTextField!
    @IBOutlet weak var rightSide: NSTextField!
    @IBOutlet weak var topSide: NSTextField!
    @IBOutlet weak var fireSide: NSTextField!
    @IBOutlet weak var earthSide: NSTextField!
    @IBOutlet weak var airSide: NSTextField!
    @IBOutlet weak var waterSide: NSTextField!
    @IBOutlet weak var cSide: NSTextField!
    @IBOutlet weak var fSide: NSTextField!
    @IBOutlet weak var mSide: NSTextField!
    
    
    @IBOutlet weak var currentHouseName: NSTextField!
    
    @IBOutlet weak var timesetterButton: NSButton!
    
    // 未使用
    @IBAction func imageSave(_ sender: Any) {
        print("start")
        
        let font = NSFont(name: "microcosm-aspects", size: 16.0)
        
        //let font = NSFont(descriptor: NSFontDescriptor(name: "microcosm", size: 16.0), size: 16.0)
        
        for font in NSFontManager.shared.availableFonts {
            print(font)
        }
        
        print("done")
    }
    
    
    
    
    
    
    @IBOutlet weak var box: NSBox!
    // 未使用
    @IBAction func changeSize(_ sender: Any) {
        
        //        let m = NSEvent.mouseLocation
        
        //        menu0?.popUp(positioning: menuItem, at: NSPoint(x: m.x / 2, y: m.y / 2), in: self.view)
        guard let event = NSApp.currentEvent else { return }
        NSMenu.popUpContextMenu(menu0!, with: event, for: sender as! NSView)
    }
    
    override func viewWillAppear() {
        self.view.window?.minSize = NSSize(width: 960, height: 640)
        self.view.window?.aspectRatio = CGSize(width: 3, height: 2)
        //        let x = text222.resignFirstResponder()
        self.view.window?.makeFirstResponder(self)
        
        view.window?.delegate = self
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        // Do any additional setup after loading the view.
        let delegate = NSApplication.shared.delegate as! AppDelegate
        delegate.viewController = self
        
        self.view.window?.delegate = self
        
        // ファイル初期化
        let u = FileInitialize.microcosmDirectory()
        FileInitialize.fileInit(microcosmURL: u)
        
        settingCombo.removeAllItems()
        for i in 0...9 {
            var data:SettingData? = SettingSave.load(url: FileInitialize.microcosmDirectory(), index: i)
            if (data == nil) {
                print(String(i) + " nil")
                data = SettingData()
            }
            delegate.settingData += [data!]
            delegate.dispNames += [DispName(name: data!.name)]
            settingCombo.addItem(withTitle: data!.name)
        }
        settingCombo.selectItem(at: 0)
        delegate.currentSetting = delegate.settingData[0]
        delegate.currentSettingIndex = 0
        
        swiss.set_ephe_path()
        
        // combo
        timeSetterCombo.removeAllItems()
        timeSetterCombo.addItem(withTitle: "User1")
        timeSetterCombo.addItem(withTitle: "User2")
        timeSetterCombo.addItem(withTitle: "Event1")
        timeSetterCombo.addItem(withTitle: "Event2")
        timeSetterCombo.selectItem(at: 2)
        
        let myDate = MyDate()
        delegate.udata1 = myDate.nowUserData(userData: delegate.udata1)
        delegate.udata2 = myDate.nowUserData(userData: delegate.udata2)
        delegate.edata1 = myDate.nowUserData(userData: delegate.edata1)
        delegate.edata2 = myDate.nowUserData(userData: delegate.edata2)
        
        ReCalc()
        ReSetCurrentSettingBox()
        
        signTable.delegate = self
        signTable.dataSource = self
        houseTable.delegate = self
        houseTable.dataSource = self
        
        // userBox
        user1Box.stringValue = delegate.udata1.name + "\n" +
        Util.UserDataBirthStr(userData: delegate.udata1) + "\n" +
        TimeZoneConst.timezones[delegate.udata1.birth_timezone_index] + "\n" +
        delegate.udata1.birth_place + "\n" +
        String(delegate.udata1.lat) + " " + String(delegate.udata1.lng)
        user2Box.stringValue = delegate.udata2.name + "\n" +
        Util.UserDataBirthStr(userData: delegate.udata2) + "\n" +
        TimeZoneConst.timezones[delegate.udata2.birth_timezone_index] + "\n" +
        delegate.udata2.birth_place + "\n" +
        String(delegate.udata2.lat) + " " + String(delegate.udata2.lng)
        event1Box.stringValue = delegate.edata1.name + "\n" +
        Util.UserDataBirthStr(userData: delegate.edata1) + "\n" +
        TimeZoneConst.timezones[delegate.edata1.birth_timezone_index] + "\n" +
        delegate.edata1.birth_place + "\n" +
        String(delegate.edata1.lat) + " " + String(delegate.edata1.lng)
        event2Box.stringValue = delegate.edata2.name + "\n" +
        Util.UserDataBirthStr(userData: delegate.edata2) + "\n" +
        TimeZoneConst.timezones[delegate.edata2.birth_timezone_index] + "\n" +
        delegate.edata2.birth_place + "\n" +
        String(delegate.edata2.lat) + " " + String(delegate.edata2.lng)
        
        // チャート
        canvas = ChartView(frame: NSRect(x: 0, y: 0, width: box.frame.width, height: box.frame.height))
        box.addSubview(canvas!)
        
        //        self.becomeFirstResponder()
        //        self.makeFirstResponder()
        //        self.view.window?.makeFirstResponder(self.windowController?)
        //        let x = text222.resignFirstResponder()
        
        
        let menuItem = NSMenuItem(title: "title",
                                  action: #selector(self.handleMenuItemSelected),
                                  keyEquivalent: "")
        menu0?.addItem(menuItem)
        
        // なぜかメニュー開閉でも呼ばれる
        NotificationCenter.default.addObserver(forName: NSWindow.didResizeNotification, object: nil, queue: OperationQueue.main) {
            (n: Notification) in
            //            if (self.view.window?.frame.size.width == nil) {
            //                return
            //            }
            //
            //            //print(self.view.window?.frame.size.width as Any)
            //            //print(self.view.window?.frame.size.height as Any)
            //            var x = self.view.window?.frame.size.width ?? 0
            //
            //            x = CGFloat((self.view.window?.frame.size.height)! - 70)
            //            self.box.frame.size.width = CGFloat(x)
            //            self.box.frame.size.height = CGFloat(x)
            //            let leftSize = CGFloat((self.view.window?.frame.size.width)! - x - 20)
            //            self.box.frame.origin.x = leftSize
            //            self.box.frame.origin.y = 20
            //
            //
            //            self.canvas = ChartView(frame: NSRect(x: 0, y: 0, width: self.box.frame.width, height: self.box.frame.height))
            //            self.box.subviews.last?.subviews.removeLast()
            //            self.box.addSubview(self.canvas!)
            
        }
        
        setBalance()
    }
    
    override func viewDidAppear() {
        text111.resignFirstResponder()
    }
    
    @objc
    func handleMenuItemSelected(_ sender: AnyObject) {
        print("aa")
    }
    
//    override func rightMouseUp(with event: NSEvent) {
//        print("aaaa")
//    }
//
    override var representedObject: Any? {
        didSet {
            // Update the view, if already loaded.
        }
    }
    
    @IBAction func timeSetterRightClicked(_ sender: Any) {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        var udata = getTargetUser()
        let myDate = MyDate()
        if delegate.currentSpanType == SpanType.UNIT {
            let newUserData = myDate.addUserData(userData: udata, second: plusUnit)
            setUserData(udata: newUserData)
        } else if delegate.currentSpanType == SpanType.NEWMOON {
            let moon = MoonCalc(config: delegate.config, swiss: delegate.swiss)
            myDate.setUserData(u: udata)
            let time = moon.getNewMoon(d: myDate, timezone: udata.birth_timezone)
            udata.setDate(date: time)
            setUserData(udata: udata)
        } else if delegate.currentSpanType == SpanType.FULLMOON {
            let moon = MoonCalc(config: delegate.config, swiss: delegate.swiss)
            myDate.setUserData(u: udata)
            let time = moon.getFullMoon(d: myDate, timezone: udata.birth_timezone)
            udata.setDate(date: time)
            setUserData(udata: udata)
        } else if delegate.currentSpanType == SpanType.SOLARRETURN {
            let eclipse = EclipseCalc()
            myDate.setUserData(u: udata)
            myDate.addDays(day: 360)
            let time = eclipse.GetEclipse(begin: myDate, timezone: udata.birth_timezone, planetId: 0, targetDegree: list1[0]!.absolute_position, isForward: true, config: delegate.config)
            udata.setDate(date: time)
            setUserData(udata: udata)
        } else if delegate.currentSpanType == SpanType.SOLARINGRESS {
            let eclipse = EclipseCalc()
            myDate.setUserData(u: udata)
            let calc = AstroCalc(config: delegate.config, swiss: swiss)
            let currentDegree = calc.PositionCalcSingle(date: myDate, timezone: udata.birth_timezone, planetId: 0)
            let degree = Util.GetNextIngressDegree(degree: currentDegree)
            print(list1[0]!.absolute_position)
            let time = eclipse.GetEclipse(begin: myDate, timezone: udata.birth_timezone, planetId: 0, targetDegree: degree, isForward: true, config: delegate.config)
            udata.setDate(date: time)
            setUserData(udata: udata)
        } else if delegate.currentSpanType == SpanType.MOONINGRESS {
            let eclipse = EclipseCalc()
            myDate.setUserData(u: udata)
            let calc = AstroCalc(config: delegate.config, swiss: swiss)
            let currentDegree = calc.PositionCalcSingle(date: myDate, timezone: udata.birth_timezone, planetId: 1)
            let degree = Util.GetNextIngressDegree(degree: currentDegree)
            let time = eclipse.GetEclipse(begin: myDate, timezone: udata.birth_timezone, planetId: 1, targetDegree: degree, isForward: true, config: delegate.config)
            udata.setDate(date: time)
            setUserData(udata: udata)
        }
        setUserBox()
        ReCalc()
        ReRender()
    }
    
    @IBAction func timeSetterLeftClicked(_ sender: Any) {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        let myDate = MyDate()
        var udata = getTargetUser()
        if delegate.currentSpanType == SpanType.UNIT {
            let newUserData = myDate.addUserData(userData: udata, second: -1 * plusUnit)
            setUserData(udata: newUserData)
        } else if delegate.currentSpanType == SpanType.NEWMOON {
            let moon = MoonCalc(config: delegate.config, swiss: delegate.swiss)
            myDate.setUserData(u: udata)
            let time = moon.getNewMoonMinus(d: myDate, timezone: udata.birth_timezone)
            udata.setDate(date: time)
            setUserData(udata: udata)
        } else if delegate.currentSpanType == SpanType.FULLMOON {
            let moon = MoonCalc(config: delegate.config, swiss: delegate.swiss)
            myDate.setUserData(u: udata)
            let time = moon.getFullMoonMinus(d: myDate, timezone: udata.birth_timezone)
            udata.setDate(date: time)
            setUserData(udata: udata)
        } else if delegate.currentSpanType == SpanType.SOLARRETURN {
            // todo nextaction
            let eclipse = EclipseCalc()
            myDate.setUserData(u: udata)
            let time = eclipse.GetEclipse(begin: myDate, timezone: udata.birth_timezone, planetId: 0, targetDegree: list1[0]!.absolute_position, isForward: false, config: delegate.config)
            udata.setDate(date: time)
            setUserData(udata: udata)
        } else if delegate.currentSpanType == SpanType.SOLARINGRESS {
            let eclipse = EclipseCalc()
            myDate.setUserData(u: udata)
            let calc = AstroCalc(config: delegate.config, swiss: swiss)
            let currentDegree = calc.PositionCalcSingle(date: myDate, timezone: udata.birth_timezone, planetId: 0)
            let degree = Util.GetPrevIngressDegree(degree: currentDegree)
            print(degree)
            let time = eclipse.GetEclipse(begin: myDate, timezone: udata.birth_timezone, planetId: 0, targetDegree: degree, isForward: false, config: delegate.config)
            udata.setDate(date: time)
            setUserData(udata: udata)
        } else if delegate.currentSpanType == SpanType.MOONINGRESS {
            let eclipse = EclipseCalc()
            myDate.setUserData(u: udata)
            let calc = AstroCalc(config: delegate.config, swiss: swiss)
            let currentDegree = calc.PositionCalcSingle(date: myDate, timezone: udata.birth_timezone, planetId: 1)
            let degree = Util.GetPrevIngressDegree(degree: currentDegree)
            let time = eclipse.GetEclipse(begin: myDate, timezone: udata.birth_timezone, planetId: 1, targetDegree: degree, isForward: false, config: delegate.config)
            udata.setDate(date: time)
            setUserData(udata: udata)
        }
        setUserBox()
        ReCalc()
        ReRender()
    }
    
    
    @IBAction func timeSetterNowClicked(_ sender: Any) {
        let udata = getTargetUser()
        let myDate = MyDate()
        let newUserData = myDate.nowUserData(userData: udata)
        setUserData(udata: newUserData)
        setUserBox()
        ReCalc()
        ReRender()
    }
    
    public func timeSetterSpecificDate(myDate: MyDate) {
        let udata = getTargetUser()
        var newUserData = udata
        
        newUserData.birth_year = myDate.getFullYear()
        newUserData.birth_month = myDate.getMonth()
        newUserData.birth_day = myDate.getDay()
        newUserData.birth_hour = myDate.getHour()
        newUserData.birth_minute = myDate.getMinute()
        newUserData.birth_second = myDate.getSecond()

        setUserData(udata: newUserData)
        setUserBox()
    }
    
    
    public func setUserBox() {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        user1Box.stringValue = delegate.udata1.name + "\n" +
        Util.UserDataBirthStr(userData: delegate.udata1) + "\n" +
        TimeZoneConst.timezones[delegate.udata1.birth_timezone_index] + "\n" +
        delegate.udata1.birth_place + "\n" +
        String(delegate.udata1.lat) + " " + String(delegate.udata1.lng)
        user2Box.stringValue = delegate.udata2.name + "\n" +
        Util.UserDataBirthStr(userData: delegate.udata2) + "\n" +
        TimeZoneConst.timezones[delegate.udata2.birth_timezone_index] + "\n" +
        delegate.udata2.birth_place + "\n" +
        String(delegate.udata2.lat) + " " + String(delegate.udata2.lng)
        event1Box.stringValue = delegate.edata1.name + "\n" +
        Util.UserDataBirthStr(userData: delegate.edata1) + "\n" +
        TimeZoneConst.timezones[delegate.edata1.birth_timezone_index] + "\n" +
        delegate.edata1.birth_place + "\n" +
        String(delegate.edata1.lat) + " " + String(delegate.edata1.lng)
        event2Box.stringValue = delegate.edata2.name + "\n" +
        Util.UserDataBirthStr(userData: delegate.edata2) + "\n" +
        TimeZoneConst.timezones[delegate.edata2.birth_timezone_index] + "\n" +
        delegate.edata2.birth_place + "\n" +
        String(delegate.edata2.lat) + " " + String(delegate.edata2.lng)
    }
    
    public func getTargetUser() -> UserData {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        if (timeSetterCombo.indexOfSelectedItem == 0) {
            return delegate.udata1
        } else if (timeSetterCombo.indexOfSelectedItem == 1) {
            return delegate.udata2
        } else if (timeSetterCombo.indexOfSelectedItem == 2) {
            return delegate.edata1
        }
        return delegate.edata2
    }
    
    public func setUserData(udata: UserData) {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        if (timeSetterCombo.indexOfSelectedItem == 0) {
            delegate.udata1 = udata
        } else if (timeSetterCombo.indexOfSelectedItem == 1) {
            delegate.udata2 = udata
        } else if (timeSetterCombo.indexOfSelectedItem == 2) {
            delegate.edata1 = udata
        }
        delegate.edata2 = udata
    }
    
    
    @IBAction func centerRingSliderChanged(_ sender: Any) {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        delegate.chartStyle.centerRingRatio = Int(centerRingSlider.intValue)
        
        var x = self.view.window?.frame.size.width ?? 0
        
        x = CGFloat((self.view.window?.frame.size.height)! - 100)
        box.frame.size.width = CGFloat(x)
        box.frame.size.height = CGFloat(x)
        let leftSize = CGFloat((self.view.window?.frame.size.width)! - x - 20)
        box.frame.origin.x = leftSize
        box.frame.origin.y = 60
        
        canvas = ChartView(frame: NSRect(x: 0, y: 0, width: self.box.frame.width, height: self.box.frame.height))
        box.subviews.last?.subviews.removeLast()
        box.addSubview(self.canvas!)
    }
    
    
    /// 計算処理の入口
    public func ReCalc()
    {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        let ring1 = GetTargetUser(ringIndex: 0)
        let ring2 = GetTargetUser(ringIndex: 1)
        let ring3 = GetTargetUser(ringIndex: 2)
        
        let d1 = MyDate()
        d1.setUserData(u: ring1)
        
        let calc = AstroCalc(config: delegate.config, swiss: swiss)
        list1 = calc.ReCalc(setting: delegate.currentSetting, date: d1)
        // delegate側のlist1はこの後でasc/mcが入るからまだだめ
        houseList1 = calc.CuspCalc(d: d1, timezone: ring1.birth_timezone, lat: ring1.lat, lng: ring1.lng, houseKind: EHouse(rawValue: delegate.currentSetting.progression) ?? EHouse.PLACIDUS)
        if (delegate.config.sideReal == ESideReal.DRACONIC) {
            if (delegate.config.nodeCalc == ENodeCalc.TRUE) {
                for (i, _) in houseList1.enumerated() {
                    houseList1[i] = houseList1[i] - list1[EPlanets.DH_TRUENODE.rawValue]!.absolute_position
                    if (houseList1[i] < 0) {
                        houseList1[i] = houseList1[i] + 360
                    }
                }
            } else {
                for (i, _) in houseList1.enumerated() {
                    houseList1[i] = houseList1[i] - list1[EPlanets.DH_MEANNODE.rawValue]!.absolute_position
                    if (houseList1[i] < 0) {
                        houseList1[i] = houseList1[i] + 360
                    }
                }
            }
        }
        let asc = PlanetData()
        asc.no = EPlanets.ASC.rawValue
        asc.absolute_position = houseList1[1]
        asc.isDisp = delegate.currentSetting.dispPlanetAsc == 1
        asc.isAspectDisp = delegate.currentSetting.dispAspectPlanetAsc == 1
        asc.aspects = [AspectInfo]()
        asc.secondAspects = [AspectInfo]()
        asc.thirdAspects = [AspectInfo]()
        asc.sensitive = true
        list1[EPlanets.ASC.rawValue] = asc
        let mc = PlanetData()
        mc.no = EPlanets.MC.rawValue
        mc.absolute_position = houseList1[10]
        mc.isDisp = delegate.currentSetting.dispPlanetMc == 1
        mc.isAspectDisp = delegate.currentSetting.dispAspectPlanetMc == 1
        mc.aspects = [AspectInfo]()
        mc.secondAspects = [AspectInfo]()
        mc.thirdAspects = [AspectInfo]()
        mc.sensitive = true
        list1[EPlanets.MC.rawValue] = mc
        
        let d2 = MyDate()
        d2.setUserData(u: ring2)
        if (delegate.bands < 3)
        {
            // 一重円、二重円の場合ハウスは考慮しない(NPだけのリングは存在しない)
            let d = MyDate()
            d.setUserData(u: ring2)
            delegate.list2 = calc.ReCalc(setting: delegate.currentSetting, date: d)
        } else {
            if (delegate.secondBand == EBandKind.PROGRESS) {
                delegate.list2 = calc.ReCalcProgress(config: delegate.config, setting: delegate.currentSetting, natalList: list1, udata: ring1, transitTime: d2, timezone: ring1.birth_timezone)
            } else {
                delegate.list2 = calc.ReCalc(setting: delegate.currentSetting, date: d2)
            }
        }
        d2.setUserData(u: ring2)
        if (delegate.bands == 3) {
            if (delegate.secondBand == EBandKind.PROGRESS) {
                houseList2 = calc.HouseCalcProgress(config: delegate.config, setting: delegate.currentSetting, houseList: houseList1, natallist: list1, natalTime: d1, transitTime: d2, lat: ring1.lat, lng: ring1.lng, timezone: ring1.birth_timezone)
            } else {
                houseList2 = calc.CuspCalc(d: d2, timezone: ring1.birth_timezone, lat: ring1.lat, lng: ring1.lng, houseKind: EHouse(rawValue: delegate.currentSetting.progression) ?? EHouse.PLACIDUS)
            }
        } else {
            houseList2 = calc.CuspCalc(d: d2, timezone: ring1.birth_timezone, lat: ring2.lat, lng: ring2.lng, houseKind: EHouse(rawValue: delegate.currentSetting.progression) ?? EHouse.PLACIDUS)
        }
        if (delegate.config.sideReal == ESideReal.DRACONIC) {
            if (delegate.config.nodeCalc == ENodeCalc.TRUE) {
                for (i, _) in houseList2.enumerated() {
                    houseList2[i] = houseList2[i] - list2[EPlanets.DH_TRUENODE.rawValue]!.absolute_position
                    if (houseList2[i] < 0) {
                        houseList2[i] = houseList2[i] + 360
                    }
                }
            } else {
                for (i, _) in houseList1.enumerated() {
                    houseList2[i] = houseList2[i] - list2[EPlanets.DH_MEANNODE.rawValue]!.absolute_position
                    if (houseList2[i] < 0) {
                        houseList2[i] = houseList2[i] + 360
                    }
                }
            }
        }
        let asc2 = PlanetData()
        asc2.no = EPlanets.ASC.rawValue
        asc2.absolute_position = houseList2[1]
        asc2.isDisp = delegate.currentSetting.dispPlanetAsc == 1
        asc2.isAspectDisp = delegate.currentSetting.dispAspectPlanetAsc == 1
        asc2.aspects = [AspectInfo]()
        asc2.secondAspects = [AspectInfo]()
        asc2.thirdAspects = [AspectInfo]()
        asc2.sensitive = true
        list2[EPlanets.ASC.rawValue] = asc2
        let mc2 = PlanetData()
        mc2.no = EPlanets.MC.rawValue
        mc2.absolute_position = houseList2[10]
        mc2.isDisp = delegate.currentSetting.dispPlanetMc == 1
        mc2.isAspectDisp = delegate.currentSetting.dispAspectPlanetMc == 1
        mc2.aspects = [AspectInfo]()
        mc2.secondAspects = [AspectInfo]()
        mc2.thirdAspects = [AspectInfo]()
        mc2.sensitive = true
        list2[EPlanets.MC.rawValue] = mc2
        
        let d3 = MyDate()
        d3.setUserData(u: ring3)
        
        if (delegate.thirdBand == EBandKind.COMPOSIT) {
            list3 = calc.PositionCalcComposit(natal1: list1, natal2: list2)
        } else {
            list3 = calc.ReCalc(setting: delegate.currentSetting, date: d3)
        }
        // Progressにすることはない
        houseList3 = calc.CuspCalc(d: d3, timezone: ring1.birth_timezone, lat: ring3.lat, lng: ring3.lng, houseKind: EHouse(rawValue: delegate.currentSetting.progression) ?? EHouse.PLACIDUS)
        if (delegate.config.sideReal == ESideReal.DRACONIC) {
            if (delegate.config.nodeCalc == ENodeCalc.TRUE) {
                for (i, _) in houseList3.enumerated() {
                    houseList3[i] = houseList3[i] - list3[EPlanets.DH_TRUENODE.rawValue]!.absolute_position
                    if (houseList3[i] < 0) {
                        houseList3[i] = houseList3[i] + 360
                    }
                }
            } else {
                for (i, _) in houseList3.enumerated() {
                    houseList3[i] = houseList2[i] - list3[EPlanets.DH_MEANNODE.rawValue]!.absolute_position
                    if (houseList3[i] < 0) {
                        houseList3[i] = houseList3[i] + 360
                    }
                }
            }
        }
        let asc3 = PlanetData()
        asc3.no = EPlanets.ASC.rawValue
        asc3.absolute_position = houseList3[1]
        asc3.isDisp = delegate.currentSetting.dispPlanetAsc == 1
        asc3.isAspectDisp = delegate.currentSetting.dispAspectPlanetAsc == 1
        asc3.aspects = [AspectInfo]()
        asc3.secondAspects = [AspectInfo]()
        asc3.thirdAspects = [AspectInfo]()
        asc3.sensitive = true
        list3[EPlanets.ASC.rawValue] = asc3
        let mc3 = PlanetData()
        mc3.no = EPlanets.MC.rawValue
        mc3.absolute_position = houseList3[10]
        mc3.isDisp = delegate.currentSetting.dispPlanetMc == 1
        mc3.isAspectDisp = delegate.currentSetting.dispAspectPlanetMc == 1
        mc3.aspects = [AspectInfo]()
        mc3.secondAspects = [AspectInfo]()
        mc3.thirdAspects = [AspectInfo]()
        mc3.sensitive = true
        list3[EPlanets.MC.rawValue] = mc3
        
        delegate.list1 = list1
        delegate.list2 = list2
        delegate.list3 = list3
        // list1array〜list3arrayはsignList(左下枠でのみ使う)
        delegate.list1array = list1.values
            .filter{$0.isDisp == true}
            .sorted { $0.no < $1.no }
        delegate.list2array = list1.values
            .filter{$0.isDisp == true}
            .sorted { $0.no < $1.no }
        delegate.list3array = list1.values
            .filter{$0.isDisp == true}
            .sorted { $0.no < $1.no }
        delegate.house1array = houseList1
        delegate.house2array = houseList2
        delegate.house3array = houseList3
        
        let aspectCalc = AspectCalc()
        list1 = aspectCalc.AspectCalcSame(a_setting: delegate.currentSetting, list: list1)
        list1 = aspectCalc.AspectCalcOther(a_setting: delegate.currentSetting, fromList: list1, toList: list2, listKind: 3)
        list1 = aspectCalc.AspectCalcOther(a_setting: delegate.currentSetting, fromList: list1, toList: list3, listKind: 4)
        list2 = aspectCalc.AspectCalcSame(a_setting: delegate.currentSetting, list: list2)
        list2 = aspectCalc.AspectCalcOther(a_setting: delegate.currentSetting, fromList: list2, toList: list3, listKind: 5)
        list3 = aspectCalc.AspectCalcSame(a_setting: delegate.currentSetting, list: list3)
        
        signTable.reloadData()
        houseTable.reloadData()
        
        //
        //        // 左下サインリスト
        //        var DataSource = new SignTableDataSource();
        //        for (int i = 0; i < 10; i++)
        //        {
        //            string degree1;
        //            string degree2;
        //            string degree3;
        //            if (configData.decimalDisp == EDecimalDisp.DEGREE)
        //            {
        //                degree1 = CommonData.getSignTextJp(list1[i].absolute_position);
        //                degree2 = CommonData.getSignTextJp(list2[i].absolute_position);
        //                degree3 = CommonData.getSignTextJp(list3[i].absolute_position);
        //
        //                degree1 += ((int)(list1[i].absolute_position % 30)).ToString() + "° " +
        //                    ((int)CommonData.DecimalToHex(list1[i].absolute_position % 1 * 100)).ToString() + "'";
        //                degree2 += ((int)(list2[i].absolute_position % 30)).ToString() + "° " +
        //                    ((int)CommonData.DecimalToHex(list2[i].absolute_position % 1 * 100)).ToString() + "'";
        //                degree3 += ((int)(list3[i].absolute_position % 30)).ToString() + "° " +
        //                    ((int)CommonData.DecimalToHex(list3[i].absolute_position % 1 * 100)).ToString() + "'";
        //            }
        //            else
        //            {
        //                degree1 = list1[i].absolute_position.ToString();
        //                degree2 = list2[i].absolute_position.ToString();
        //                degree3 = list3[i].absolute_position.ToString();
        //            }
        //            DataSource.names.Add(new SignTableData()
        //            {
        //                sign = CommonData.getPlanetSymbolText(i),
        //                degree1 = degree1,
        //                degree2 = degree2,
        //                degree3 = degree3
        //            }) ;
        //        }
        //        signTable.DataSource = DataSource;
        //        signTable.Delegate = new SignTableDelegate(DataSource);
        //
        //        // 左下ハウスリスト
        //        var HouseDataSource = new SignTableDataSource();
        //        for (int i = 1; i < 13; i++)
        //        {
        //            string degree1;
        //            string degree2;
        //            string degree3;
        //            if (configData.decimalDisp == EDecimalDisp.DEGREE)
        //            {
        //                degree1 = CommonData.getSignTextJp(houseList1[i]);
        //                degree2 = CommonData.getSignTextJp(houseList2[i]);
        //                degree3 = CommonData.getSignTextJp(houseList3[i]);
        //
        //                degree1 += ((int)(houseList1[i] % 30)).ToString("00") + "° " +
        //                    ((int)CommonData.DecimalToHex(houseList1[i] % 1 * 100)).ToString() + "'";
        //                degree2 += ((int)(houseList2[i] % 30)).ToString("00") + "° " +
        //                    ((int)CommonData.DecimalToHex(houseList2[i] % 1 * 100)).ToString() + "'";
        //                degree3 += ((int)(houseList3[i] % 30)).ToString("00") + "° " +
        //                    ((int)CommonData.DecimalToHex(houseList3[i] % 1 * 100)).ToString() + "'";
        //            }
        //            else
        //            {
        //                degree1 = houseList1[i].ToString();
        //                degree2 = houseList2[i].ToString();
        //                degree3 = houseList3[i].ToString();
        //            }
        //            HouseDataSource.names.Add(new SignTableData()
        //            {
        //                sign = i.ToString(),
        //                degree1 = degree1,
        //                degree2 = degree2,
        //                degree3 = degree3
        //            });
        //        }
        //        houseTable.DataSource = HouseDataSource;
        //        houseTable.Delegate = new SignTableDelegate(HouseDataSource);
        
    }
    
    public func ReSetCurrentSettingBox()
    {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        var houseKind = ""
        if (delegate.currentSetting.houseCalc == EHouse.PLACIDUS.rawValue) {
            houseKind = "PLACIDUS"
        } else if (delegate.currentSetting.houseCalc == EHouse.KOCH.rawValue) {
            houseKind = "KOCH"
        } else if (delegate.currentSetting.houseCalc == EHouse.CAMPANUS.rawValue) {
            houseKind = "CAMPANUS"
        } else if (delegate.currentSetting.houseCalc == EHouse.EQUAL.rawValue) {
            houseKind = "EQUAL"
        } else if (delegate.currentSetting.houseCalc == EHouse.PORPHYRY.rawValue) {
            houseKind = "PORPHYRY"
        } else if (delegate.currentSetting.houseCalc == EHouse.REGIOMONTANUS.rawValue) {
            houseKind = "REGIOMONTANUS"
        } else if (delegate.currentSetting.houseCalc == EHouse.SOLAR.rawValue) {
            houseKind = "SOLAR"
        } else if (delegate.currentSetting.houseCalc == EHouse.SOLARSIGN.rawValue) {
            houseKind = "SOLARSIGN"
        } else if (delegate.currentSetting.houseCalc == EHouse.ZEROARIES.rawValue) {
            houseKind = "ZEROARIES"
        }

        currentHouseName.stringValue = houseKind
    }
    
    /// 使用中のユーザー情報
    /// - Parameter ringIndex: 0〜2
    /// - Returns: UserData
    public func GetTargetUser(ringIndex: Int) -> UserData
    {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        var targetUser: UserData = delegate.udata1
        
        if (calcTargetUser[ringIndex] == ETargetUser.USER1) {
            targetUser = delegate.udata1
        }
        else if (calcTargetUser[ringIndex] == ETargetUser.USER2) {
            targetUser = delegate.udata2
        }
        else if (calcTargetUser[ringIndex] == ETargetUser.EVENT1) {
            targetUser = delegate.edata1
        }
        else if (calcTargetUser[ringIndex] == ETargetUser.EVENT2) {
            targetUser = delegate.edata2
        }
        
        return targetUser;
    }
    
    
    /// ReCalcしないで再描画
    public func ReRender() {
        canvas = ChartView(frame: NSRect(x: 0, y: 0, width: self.box.frame.width, height: self.box.frame.height))
        box.subviews.last?.subviews.removeLast()
        box.addSubview(self.canvas!)
        setBalance()
    }
    
    @IBAction func settingComboChanged(_ sender: Any) {
        let x = sender as! NSPopUpButton
        let delegate = NSApplication.shared.delegate as! AppDelegate
        delegate.currentSetting = delegate.settingData[x.indexOfSelectedItem]
        delegate.currentSettingIndex = x.indexOfSelectedItem
        ReSetCurrentSettingBox()
        ReCalc()
        ReRender()
    }
    
    public func setBalance()
    {
        var down = 0
        var right = 0
        var up = 0
        var left = 0
        
        var newList = [Double](repeating: 0.0, count: 13)
        
        let signList1: [Int: PlanetData] = list1
        
        for i in 1..<13 {
            newList[i] = houseList1[i] - houseList1[1]
            if (newList[i] < 0)
            {
                newList[i] = newList[i] + 360
            }
        }
        
        var target: Double
        
        for i in 1..<11 {
            target = signList1[i]!.absolute_position - houseList1[1]
            if (target < 0)
            {
                target = target + 360;
            }
            if (
                (newList[1] <= target && target < newList[2])
            )
            {
                down = down + 1
                left = left + 1
            }
            else if (
                (newList[2] <= target && target < newList[3])
            )
            {
                down = down + 1
                left = left + 1
            }
            else if (
                (newList[3] <= target && target < newList[4])
            )
            {
                down = down + 1
                left = left + 1
            }
            else if (
                (newList[4] <= target && target < newList[5])
            )
            {
                down = down + 1
                right = right + 1
            }
            else if (
                (newList[5] <= target && target < newList[6])
            )
            {
                down = down + 1
                right = right + 1
            }
            else if (
                (newList[6] <= target && target < newList[7])
            )
            {
                down = down + 1
                right = right + 1
            }
            else if (
                (newList[7] <= target && target < newList[8])
            )
            {
                up = up + 1
                right = right + 1
            }
            else if (
                (newList[8] <= target && target < newList[9])
            )
            {
                up = up + 1
                right = right + 1
            }
            else if (
                (newList[9] <= target && target < newList[10])
            )
            {
                up = up + 1
                right = right + 1
            }
            else if (
                (newList[10] <= target && target < newList[11])
            )
            {
                up = up + 1
                left = left + 1
            }
            else if (
                (newList[11] <= target && target < newList[12])
            )
            {
                up = up + 1
                left = left + 1
            }
            else
            {
                up = up + 1
                left = left + 1
            }
        }
        
        bottomSide.stringValue = String(down)
        rightSide.stringValue = String(right)
        topSide.stringValue = String(up)
        leftSide.stringValue = String(left)
        
        var fireVal = 0;
        var earthVal = 0;
        var airVal = 0;
        var waterVal = 0;
        var cardinal = 0;
        var fixe = 0;
        var mutable = 0;
        
        for i in 1..<11 {
            print(i)
            print(signList1[i]!.absolute_position)
            if (
                (0.0 <= signList1[i]!.absolute_position && signList1[i]!.absolute_position < 30.0) ||
                (120.0 <= signList1[i]!.absolute_position && signList1[i]!.absolute_position < 150.0) ||
                (240.0 <= signList1[i]!.absolute_position && signList1[i]!.absolute_position < 270.0)
            )
            {
                fireVal = fireVal + 1
            }
            else if (
                (30.0 <= signList1[i]!.absolute_position && signList1[i]!.absolute_position < 60.0) ||
                (150.0 <= signList1[i]!.absolute_position && signList1[i]!.absolute_position < 180.0) ||
                (270.0 <= signList1[i]!.absolute_position && signList1[i]!.absolute_position < 300.0)
            )
            {
                earthVal = earthVal + 1
            }
            else if (
                (60.0 <= signList1[i]!.absolute_position && signList1[i]!.absolute_position < 90.0) ||
                (180.0 <= signList1[i]!.absolute_position && signList1[i]!.absolute_position < 210.0) ||
                (300.0 <= signList1[i]!.absolute_position && signList1[i]!.absolute_position < 330.0)
            )
            {
                airVal = airVal + 1
            }
            else
            {
                waterVal = waterVal + 1
            }
            if (
                (0.0 <= signList1[i]!.absolute_position && signList1[i]!.absolute_position < 30.0) ||
                (90.0 <= signList1[i]!.absolute_position && signList1[i]!.absolute_position < 120.0) ||
                (180.0 <= signList1[i]!.absolute_position && signList1[i]!.absolute_position < 210.0) ||
                (270.0 <= signList1[i]!.absolute_position && signList1[i]!.absolute_position < 300.0)
            )
            {
                cardinal = cardinal + 1
            }
            else if (
                (30.0 <= signList1[i]!.absolute_position && signList1[i]!.absolute_position < 60.0) ||
                (120.0 <= signList1[i]!.absolute_position && signList1[i]!.absolute_position < 150.0) ||
                (210.0 <= signList1[i]!.absolute_position && signList1[i]!.absolute_position < 240.0) ||
                (300.0 <= signList1[i]!.absolute_position && signList1[i]!.absolute_position < 330.0)
            )
            {
                fixe = fixe + 1
            }
            else
            {
                mutable = mutable + 1
            }
        }
        
        fireSide.stringValue = String(fireVal)
        earthSide.stringValue = String(earthVal)
        airSide.stringValue = String(airVal)
        waterSide.stringValue = String(waterVal)
        cSide.stringValue = String(cardinal)
        fSide.stringValue = String(fixe)
        mSide.stringValue = String(mutable)
    }
    
    
    public func chart1U1()
    {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        delegate.bands = 1
        calcTargetUser[0] = ETargetUser.USER1
        ReCalc()
        ReRender()
    }
    
    public func chart1U2()
    {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        delegate.bands = 1
        calcTargetUser[0] = ETargetUser.USER2
        ReCalc()
        ReRender()
    }
    
    public func chart1E1()
    {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        delegate.bands = 1
        calcTargetUser[0] = ETargetUser.EVENT1
        ReCalc()
        ReRender()
    }
    
    public func chart1E2()
    {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        delegate.bands = 1
        calcTargetUser[0] = ETargetUser.EVENT2
        ReCalc()
        ReRender()
    }
}

extension MainViewController: NSWindowDelegate {
    public func windowDidResize(_ notification: Notification) {
        print("windowDidResize")
        var x = self.view.window?.frame.size.width ?? 0
        
        x = CGFloat((self.view.window?.frame.size.height)! - 100)
        box.frame.size.width = CGFloat(x)
        box.frame.size.height = CGFloat(x)
        let leftSize = CGFloat((self.view.window?.frame.size.width)! - x - 20)
        box.frame.origin.x = leftSize
        box.frame.origin.y = 60


        canvas = ChartView(frame: NSRect(x: 0, y: 0, width: self.box.frame.width, height: self.box.frame.height))
        box.subviews.last?.subviews.removeLast()
        box.addSubview(self.canvas!)

    }
}

extension MainViewController: NSTableViewDelegate, NSTableViewDataSource {
    // 行数を返す
    public func numberOfRows(in tableView: NSTableView) -> Int
    {
        let delegate = NSApplication.shared.delegate as! AppDelegate

        if let id = tableView.identifier?.rawValue {
            if (id == "signTable") {
                return delegate.list1array.count
            } else if (id == "houseTable") {
                return 12
            }
        }
        
        return 0
    }
    
    public func tableView(
        _ tableView: NSTableView,
        viewFor tableColumn: NSTableColumn?,
        row: Int
    ) -> NSView?
    {
        var viewId = "MyView"
        var tableId = ""
        if let id = tableView.identifier?.rawValue {
            tableId = id
            if (id == "signTable") {
                viewId = "signView"
            } else if (id == "houseTable") {
                viewId = "houseView"
            }
        }
        let delegate = NSApplication.shared.delegate as! AppDelegate

        // findByIdみたいなもの
        var view = tableView.makeView(withIdentifier: NSUserInterfaceItemIdentifier(rawValue: viewId), owner: self)
    
        // nullなら新規viewを作成
        if (view == nil)
        {
            if let unwrapped = tableColumn?.identifier.rawValue {
                if (tableId == "signTable") {
                    if (unwrapped == "signTitleColumn") {
                        let font = NSFont(name: "microcosm", size: 18.0)
                        let text = NSTextField(labelWithString: CommonData.getPlanetSymbol2(n: delegate.list1array[row].no))
                        text.font = font
                        view = text
                    } else {
                        var signSymbol = ""
                        var tmp_absolute_position = 0.0
                        if (unwrapped == "signFirstColumn") {
                            signSymbol = CommonData.getSignSymbolJp(absolute_position: delegate.list1array[row].absolute_position);
                            tmp_absolute_position = delegate.list1array[row].absolute_position
                        } else if (unwrapped == "signSecondColumn") {
                            signSymbol = CommonData.getSignSymbolJp(absolute_position: delegate.list2array[row].absolute_position);
                            tmp_absolute_position = delegate.list2array[row].absolute_position
                        } else if (unwrapped == "signThirdColumn") {
                            signSymbol = CommonData.getSignSymbolJp(absolute_position: delegate.list3array[row].absolute_position);
                            tmp_absolute_position = delegate.list3array[row].absolute_position
                        } else {
                            signSymbol = CommonData.getSignSymbolJp(absolute_position: delegate.list1array[row].absolute_position);
                            tmp_absolute_position = delegate.list1array[row].absolute_position
                        }
                        
                        var tmp_int = (Int)(tmp_absolute_position.truncatingRemainder(dividingBy: 30))
                        var tmp_degree = (String)(format: "%02d", tmp_int)
                        tmp_degree = signSymbol + tmp_degree + "° "
                        tmp_int = (Int)(tmp_absolute_position.truncatingRemainder(dividingBy: 1) * 100)
                        tmp_degree = tmp_degree + (String)(format: "%02d", tmp_int)
                        tmp_degree = tmp_degree + "'"

                        let text = NSTextField(labelWithString: tmp_degree)
                        view = text
                    }
                } else {
                    if (unwrapped == "houseTitleColumn") {
                        view = NSTextField(labelWithString: String(row + 1))
                    } else {
                        var signSymbol = ""
                        var tmp_absolute_position = 0.0
                        if (unwrapped == "houseFirstColumn") {
                            signSymbol = CommonData.getSignSymbolJp(absolute_position: delegate.house1array[row + 1]);
                            tmp_absolute_position = delegate.house1array[row + 1]
                        } else if (unwrapped == "houseSecondColumn") {
                            signSymbol = CommonData.getSignSymbolJp(absolute_position: delegate.house2array[row + 1]);
                            tmp_absolute_position = delegate.house2array[row + 1]
                        } else if (unwrapped == "houseThirdColumn") {
                            signSymbol = CommonData.getSignSymbolJp(absolute_position: delegate.house3array[row + 1]);
                            tmp_absolute_position = delegate.house3array[row + 1]
                        } else {
                            signSymbol = CommonData.getSignSymbolJp(absolute_position: delegate.house1array[row + 1]);
                            tmp_absolute_position = delegate.house1array[row + 1]
                        }
                        var tmp_int = (Int)(tmp_absolute_position.truncatingRemainder(dividingBy: 30))
                        var tmp_degree = (String)(format: "%02d", tmp_int)
                        tmp_degree = signSymbol + tmp_degree + "° "
                        tmp_int = (Int)(tmp_absolute_position.truncatingRemainder(dividingBy: 1) * 100)
                        tmp_degree = tmp_degree + (String)(format: "%02d", tmp_int)
                        tmp_degree = tmp_degree + "'"
                        view = NSTextField(labelWithString: tmp_degree)
                    }
                }
//                if (unwrapped == "name") {
//                    view = NSTextField(labelWithString: "aaa")
//                    view?.identifier = NSUserInterfaceItemIdentifier(rawValue: unwrapped + String(row))
//
//                } else if (unwrapped == "birthStr") {
//                    view = NSTextField(labelWithString: "bbb")
//                    view?.identifier = NSUserInterfaceItemIdentifier(rawValue: unwrapped + String(row))
//                } else if (unwrapped == "place") {
//                    view = NSTextField(labelWithString: "ccc")
//                    view?.identifier = NSUserInterfaceItemIdentifier(rawValue: unwrapped + String(row))
//                }
            } else {
                view = NSTextField(labelWithString: "error");
                view?.identifier = NSUserInterfaceItemIdentifier(rawValue: "error" + String(row))
            }
        }
    
        return view
    }


    // tableRowViewを返す
    public func tableView(
        _ tableView: NSTableView,
        rowViewForRow row: Int
    ) -> NSTableRowView?
    {
        let view = tableView.rowView(atRow: row, makeIfNecessary: true)
        return view
    }

}

