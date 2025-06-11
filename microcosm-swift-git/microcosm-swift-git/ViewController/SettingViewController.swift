//
//  SettingViewController.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/02/06.
//

import Cocoa

class SettingViewController: NSViewController {
    public var dispNames: [DispName]
    public var settingData: [SettingData]
    public var oldIndex: Int = 0

    @IBOutlet weak var secondaryProgression: NSButton!
    @IBOutlet weak var primaryProgression: NSButton!
    @IBOutlet weak var solarArcProgression: NSButton!
    @IBOutlet weak var compositProgression: NSButton!
    
    @IBOutlet weak var placidus: NSButton!
    @IBOutlet weak var koch: NSButton!
    @IBOutlet weak var campanus: NSButton!
    @IBOutlet weak var equal: NSButton!
    @IBOutlet weak var zeroAries: NSButton!
    
    @IBOutlet weak var sameCuspsOff: NSButton!
    @IBOutlet weak var sameCuspsOn: NSButton!
    
    @IBOutlet weak var dispNameTable: NSTableView!
    
    @IBOutlet weak var dispName: NSTextField!
    
    @IBOutlet weak var dispPlanetSun: NSButton!
    @IBOutlet weak var dispPlanetMoon: NSButton!
    @IBOutlet weak var dispPlanetMercury: NSButton!
    @IBOutlet weak var dispPlanetVenus: NSButton!
    @IBOutlet weak var dispPlanetMars: NSButton!
    @IBOutlet weak var dispPlanetJupiter: NSButton!
    @IBOutlet weak var dispPlanetSaturn: NSButton!
    @IBOutlet weak var dispPlanetUranus: NSButton!
    @IBOutlet weak var dispPlanetNeptune: NSButton!
    @IBOutlet weak var dispPlanetPluto: NSButton!
    @IBOutlet weak var dispPlanetAsc: NSButton!
    @IBOutlet weak var dispPlanetMc: NSButton!
    @IBOutlet weak var dispPlanetChiron: NSButton!
    @IBOutlet weak var dispPlanetDragonH: NSButton!
    @IBOutlet weak var dispPlanetDragonT: NSButton!
    @IBOutlet weak var dispPlanetLilith: NSButton!
    @IBOutlet weak var dispPlanetEarth: NSButton!
    @IBOutlet weak var dispPlanetCeres: NSButton!
    @IBOutlet weak var dispPlanetPallas: NSButton!
    @IBOutlet weak var dispPlanetJuno: NSButton!
    @IBOutlet weak var dispPlanetVesta: NSButton!
    @IBOutlet weak var dispPlanetPoF: NSButton!
    @IBOutlet weak var dispPlanetPoS: NSButton!
    
    @IBOutlet weak var dispAspectPlanetSun: NSButton!
    @IBOutlet weak var dispAspectPlanetMoon: NSButton!
    @IBOutlet weak var dispAspectPlanetMercury: NSButton!
    @IBOutlet weak var dispAspectPlanetVenus: NSButton!
    @IBOutlet weak var dispAspectPlanetMars: NSButton!
    @IBOutlet weak var dispAspectPlanetJupiter: NSButton!
    @IBOutlet weak var dispAspectPlanetSaturn: NSButton!
    @IBOutlet weak var dispAspectPlanetUranus: NSButton!
    @IBOutlet weak var dispAspectPlanetNeptune: NSButton!
    @IBOutlet weak var dispAspectPlanetPluto: NSButton!
    @IBOutlet weak var dispAspectPlanetAsc: NSButton!
    @IBOutlet weak var dispAspectPlanetMc: NSButton!
    @IBOutlet weak var dispAspectPlanetChiron: NSButton!
    @IBOutlet weak var dspAspectPlanetDH: NSButton!
    @IBOutlet weak var dispAspectPlanetDT: NSButton!
    @IBOutlet weak var dispAspectPlanetLilith: NSButton!
    @IBOutlet weak var dispAspectPlanetEarth: NSButton!
    @IBOutlet weak var dispAspectPlanetCeres: NSButton!
    @IBOutlet weak var dispAspectPlanetPallas: NSButton!
    @IBOutlet weak var dispAspectPlanetJuno: NSButton!
    @IBOutlet weak var dispAspectPlanetVesta: NSButton!
    @IBOutlet weak var dispAspectPlanetPof: NSButton!
    @IBOutlet weak var dispAspectPlanetPos: NSButton!
    
    @IBOutlet weak var dispConjunction: NSButton!
    @IBOutlet weak var dispOpposition: NSButton!
    @IBOutlet weak var dispTrine: NSButton!
    @IBOutlet weak var dispSquare: NSButton!
    @IBOutlet weak var dispSextile: NSButton!
    @IBOutlet weak var dispInconjunct: NSButton!
    @IBOutlet weak var dispSesquiquadrate: NSButton!
    @IBOutlet weak var dispSemiSquare: NSButton!
    @IBOutlet weak var dispSemiSextile: NSButton!
    @IBOutlet weak var dispNovile: NSButton!
    @IBOutlet weak var dispSeptile: NSButton!
    @IBOutlet weak var dispQuintile: NSButton!
    @IBOutlet weak var dispBiQuintile: NSButton!
    @IBOutlet weak var dispSemiQuintile: NSButton!
    @IBOutlet weak var dispQuindecile: NSButton!
    
    @IBOutlet weak var orbSunMoonSoft: NSTextField!
    @IBOutlet weak var orbSunMoonHard: NSTextField!
    @IBOutlet weak var orb1stSoft: NSTextField!
    @IBOutlet weak var orb1stHard: NSTextField!
    @IBOutlet weak var orb2ndSoft: NSTextField!
    @IBOutlet weak var orb2ndHard: NSTextField!
    
    
    
    required init?(coder: NSCoder) {
        dispNames = []
        settingData = []
        for i in 0...9 {
            var data:SettingData? = SettingSave.load(url: FileInitialize.microcosmDirectory(), index: i)
            if (data == nil) {
                print(String(i) + " nil")
                data = SettingData()
            }
            settingData += [data!]
            dispNames += [DispName(name: data!.name)]
        }

        super.init(coder: coder)
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        // Do view setup here.
        
        dispNameTable.dataSource = self
        dispNameTable.delegate = self
        dispNameTable.selectRowIndexes(IndexSet(integer: 0), byExtendingSelection: true)
        
        // 初期表示
        dispName.stringValue = settingData[0].name
        switch(settingData[0].progression) {
        case 1:
            secondaryProgression.state = NSControl.StateValue.on
            primaryProgression.state = NSControl.StateValue.off
            solarArcProgression.state = NSControl.StateValue.off
            compositProgression.state = NSControl.StateValue.off
            break
        case 2:
            secondaryProgression.state = NSControl.StateValue.off
            primaryProgression.state = NSControl.StateValue.on
            solarArcProgression.state = NSControl.StateValue.off
            compositProgression.state = NSControl.StateValue.off
            break
        case 3:
            secondaryProgression.state = NSControl.StateValue.off
            primaryProgression.state = NSControl.StateValue.off
            solarArcProgression.state = NSControl.StateValue.on
            compositProgression.state = NSControl.StateValue.off
            break
        case 4:
            secondaryProgression.state = NSControl.StateValue.off
            primaryProgression.state = NSControl.StateValue.off
            solarArcProgression.state = NSControl.StateValue.off
            compositProgression.state = NSControl.StateValue.on
            break
        default:
            secondaryProgression.state = NSControl.StateValue.on
            primaryProgression.state = NSControl.StateValue.off
            solarArcProgression.state = NSControl.StateValue.off
            compositProgression.state = NSControl.StateValue.off
            break
        }

        switch(settingData[0].progression) {
        case 1:
            placidus.state = NSControl.StateValue.on
            koch.state = NSControl.StateValue.off
            campanus.state = NSControl.StateValue.off
            equal.state = NSControl.StateValue.off
            zeroAries.state = NSControl.StateValue.off
            break
        case 2:
            placidus.state = NSControl.StateValue.off
            koch.state = NSControl.StateValue.on
            campanus.state = NSControl.StateValue.off
            equal.state = NSControl.StateValue.off
            zeroAries.state = NSControl.StateValue.off
            break
        case 3:
            placidus.state = NSControl.StateValue.off
            koch.state = NSControl.StateValue.off
            campanus.state = NSControl.StateValue.on
            equal.state = NSControl.StateValue.off
            zeroAries.state = NSControl.StateValue.off
            break
        case 4:
            placidus.state = NSControl.StateValue.off
            koch.state = NSControl.StateValue.off
            campanus.state = NSControl.StateValue.off
            equal.state = NSControl.StateValue.on
            zeroAries.state = NSControl.StateValue.off
            break
        case 5:
            placidus.state = NSControl.StateValue.off
            koch.state = NSControl.StateValue.off
            campanus.state = NSControl.StateValue.off
            equal.state = NSControl.StateValue.off
            zeroAries.state = NSControl.StateValue.on
            break
        default:
            placidus.state = NSControl.StateValue.on
            koch.state = NSControl.StateValue.off
            campanus.state = NSControl.StateValue.off
            equal.state = NSControl.StateValue.off
            zeroAries.state = NSControl.StateValue.off
        }

        if (settingData[0].sameCusps) {
            sameCuspsOn.state = NSControl.StateValue.on
            sameCuspsOff.state = NSControl.StateValue.off
        } else {
            sameCuspsOn.state = NSControl.StateValue.off
            sameCuspsOff.state = NSControl.StateValue.on
        }
        
        dispPlanetSun.state = settingData[0].dispPlanetSun == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetMoon.state = settingData[0].dispPlanetMoon == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetMercury.state = settingData[0].dispPlanetMercury == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetVenus.state = settingData[0].dispPlanetVenus == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetMars.state = settingData[0].dispPlanetMars == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetJupiter.state = settingData[0].dispPlanetJupiter == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetSaturn.state = settingData[0].dispPlanetSaturn == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetUranus.state = settingData[0].dispPlanetUranus == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetNeptune.state = settingData[0].dispPlanetNeptune == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetPluto.state = settingData[0].dispPlanetPluto == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetAsc.state = settingData[0].dispPlanetAsc == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetMc.state = settingData[0].dispPlanetMc == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetChiron.state = settingData[0].dispPlanetChiron == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetDragonH.state = settingData[0].dispPlanetDH == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetDragonT.state = settingData[0].dispPlanetDT == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetLilith.state = settingData[0].dispPlanetLilith == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetEarth.state = settingData[0].dispPlanetEarth == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetCeres.state = settingData[0].dispPlanetCeres == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetPallas.state = settingData[0].dispPlanetPallas == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetJuno.state = settingData[0].dispPlanetJuno == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetVesta.state = settingData[0].dispPlanetVesta == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetPoF.state = settingData[0].dispPlanetPof == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetPoS.state = settingData[0].dispPlanetPos == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        
        dispAspectPlanetSun.state = settingData[0].dispAspectPlanetSun == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetMoon.state = settingData[0].dispAspectPlanetMoon == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetMercury.state = settingData[0].dispAspectPlanetMercury == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetVenus.state = settingData[0].dispAspectPlanetVenus == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetMars.state = settingData[0].dispAspectPlanetMars == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetJupiter.state = settingData[0].dispAspectPlanetJupiter == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetSaturn.state = settingData[0].dispAspectPlanetSaturn == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetUranus.state = settingData[0].dispAspectPlanetUranus == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetNeptune.state = settingData[0].dispAspectPlanetNeptune == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetPluto.state = settingData[0].dispAspectPlanetPluto == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetAsc.state = settingData[0].dispAspectPlanetAsc == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetMc.state = settingData[0].dispAspectPlanetMc == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetChiron.state = settingData[0].dispAspectPlanetChiron == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dspAspectPlanetDH.state = settingData[0].dispAspectPlanetDH == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetDT.state = settingData[0].dispAspectPlanetDT == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetLilith.state = settingData[0].dispAspectPlanetLilith == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetEarth.state = settingData[0].dispAspectPlanetEarth == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetCeres.state = settingData[0].dispAspectPlanetCeres == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetPallas.state = settingData[0].dispAspectPlanetPallas == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetJuno.state = settingData[0].dispAspectPlanetJuno == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetVesta.state = settingData[0].dispAspectPlanetVesta == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetPof.state = settingData[0].dispAspectPlanetPof == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetPos.state = settingData[0].dispAspectPlanetPos == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        
        
        dispConjunction.state = settingData[0].dispConjunction == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispOpposition.state = settingData[0].dispOpposition == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispTrine.state = settingData[0].dispTrine == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispSquare.state = settingData[0].dispSquare == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispSextile.state = settingData[0].dispSextile == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispInconjunct.state = settingData[0].dispInconjunct == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispSesquiquadrate.state = settingData[0].dispSesquiQuadrate == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispSemiSquare.state = settingData[0].dispSemiSquare == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispSemiSextile.state = settingData[0].dispSemiSextile == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispNovile.state = settingData[0].dispNovile == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispSeptile.state = settingData[0].dispAspectSeptile == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispQuintile.state = settingData[0].dispQuintile == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispBiQuintile.state = settingData[0].dispAspectBiQuintile == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispSemiQuintile.state = settingData[0].dispAspectSemiQuintile == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispQuindecile.state = settingData[0].dispAspectQuindecile == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        
        orbSunMoonSoft.stringValue = String(settingData[0].orbSunMoon[0])
        orbSunMoonHard.stringValue = String(settingData[0].orbSunMoon[1])
        orb1stSoft.stringValue = String(settingData[0].orb1st[0])
        orb1stHard.stringValue = String(settingData[0].orb1st[1])
        orb2ndSoft.stringValue = String(settingData[0].orb2nd[0])
        orb2ndHard.stringValue = String(settingData[0].orb2nd[1])
    }
    
    @IBAction func submitDispName(_ sender: Any) {
        let t:NSTextField = sender as! NSTextField
        settingData[oldIndex].name = t.stringValue
        dispNames[oldIndex].name = t.stringValue
        dispNameTable.reloadData()
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func secondaryProgressionClick(_ sender: Any) {
        primaryProgression.state = NSControl.StateValue.off
        solarArcProgression.state = NSControl.StateValue.off
        compositProgression.state = NSControl.StateValue.off
        settingData[oldIndex].progression = 0
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)

    }
    @IBAction func primaryProgressionClick(_ sender: Any) {
        secondaryProgression.state = NSControl.StateValue.off
        solarArcProgression.state = NSControl.StateValue.off
        compositProgression.state = NSControl.StateValue.off
        settingData[oldIndex].progression = 1
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)

    }
    @IBAction func solarArcProgressionClick(_ sender: Any) {
        secondaryProgression.state = NSControl.StateValue.off
        primaryProgression.state = NSControl.StateValue.off
        compositProgression.state = NSControl.StateValue.off
        settingData[oldIndex].progression = 2
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)

    }
    @IBAction func compositProgressionClick(_ sender: Any) {
        secondaryProgression.state = NSControl.StateValue.off
        primaryProgression.state = NSControl.StateValue.off
        solarArcProgression.state = NSControl.StateValue.off
        settingData[oldIndex].progression = 3
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    
    @IBAction func placidusClick(_ sender: Any) {
        koch.state = NSControl.StateValue.off
        equal.state = NSControl.StateValue.off
        campanus.state = NSControl.StateValue.off
        zeroAries.state = NSControl.StateValue.off
        settingData[oldIndex].houseCalc = 0
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func kochClick(_ sender: Any) {
        placidus.state = NSControl.StateValue.off
        equal.state = NSControl.StateValue.off
        campanus.state = NSControl.StateValue.off
        zeroAries.state = NSControl.StateValue.off
        settingData[oldIndex].houseCalc = 1
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func campanusClick(_ sender: Any) {
        placidus.state = NSControl.StateValue.off
        koch.state = NSControl.StateValue.off
        equal.state = NSControl.StateValue.off
        zeroAries.state = NSControl.StateValue.off
        settingData[oldIndex].houseCalc = 2
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func equalClick(_ sender: Any) {
        placidus.state = NSControl.StateValue.off
        koch.state = NSControl.StateValue.off
        campanus.state = NSControl.StateValue.off
        zeroAries.state = NSControl.StateValue.off
        settingData[oldIndex].houseCalc = 3
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func zeroAriesClick(_ sender: Any) {
        placidus.state = NSControl.StateValue.off
        koch.state = NSControl.StateValue.off
        campanus.state = NSControl.StateValue.off
        equal.state = NSControl.StateValue.off
        settingData[oldIndex].houseCalc = 4
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func sameCuspsOnClick(_ sender: Any) {
        sameCuspsOff.state = NSControl.StateValue.off
        settingData[oldIndex].sameCusps = true
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func sameCuspsOffClick(_ sender: Any) {
        sameCuspsOn.state = NSControl.StateValue.off
        settingData[oldIndex].sameCusps = false
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispPlanetSunClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetSun = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispPlanetMoonClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetMoon = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetMercuryClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetMercury = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetVenusClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetVenus = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetMarsClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetMars = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetJupiterClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetJupiter = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetSaturnCLick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetSaturn = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetUranusClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetUranus = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetNeptuneClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetNeptune = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetPlutoClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetPluto = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetAscClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetAsc = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetMcClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetMc = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetChironClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetChiron = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetDragonHClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetDH = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetDragonTClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetDT = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetLilithClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetLilith = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetEarthClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetEarth = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetCeresClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetCeres = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetPallasClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetPallas = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetJunoClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetJuno = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetVestaClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetVesta = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispPlanetPoFClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetPof = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispPlanetPoSClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispPlanetPos = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    @IBAction func dispAspectPlanetSunClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetSun = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetMoonClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetMoon = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetMercuryClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetMercury = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetVenusClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetVenus = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetMarsClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetMars = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetJupiterClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetJupiter = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetSaturnClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetSaturn = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetUranusClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetUranus = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }

    @IBAction func dispAspectPlanetNeptuneClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetNeptune = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetPlutoClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetPluto = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetAscClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetAsc = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
  
    @IBAction func dispAspectPlanetMcClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetMc = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetChironClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetChiron = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetDHClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetDH = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetDTClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetDT = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetLilithClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetLilith = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetEarthClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetEarth = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetCeresClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetCeres = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetPallasClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetPallas = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetJunoClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetJuno = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetVestaClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetVesta = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetPofClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetPof = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispAspectPlanetPosClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectPlanetPos = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispConjunctionClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispConjunction = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispOppositionClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispOpposition = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispTrineClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispTrine = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispSquareClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispSquare = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispSextileClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispSextile = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispInconjunctClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispInconjunct = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispSesquiquadrateClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispSesquiQuadrate = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispSemiSquareClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispSemiSquare = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispSemiSextileClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispSemiSextile = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispNovileClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispNovile = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispSeptileClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectSeptile = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispQuintileClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispQuintile = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispBiQuintileClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectBiQuintile = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispSemiQuintileClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectSemiQuintile = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func dispQuindecileClick(_ sender: Any) {
        let btn:NSButton = sender as! NSButton
        settingData[oldIndex].dispAspectQuindecile = btn.state.rawValue
        SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
    }
    
    @IBAction func orbSunMoonSoftChanged(_ sender: Any) {
        let txt:NSTextField = sender as! NSTextField
        if let val = Float(txt.stringValue) {
            settingData[oldIndex].orbSunMoon[0] = val
            SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
        }
    }
    
    @IBAction func orbSunMoonHardChanged(_ sender: Any) {
        let txt:NSTextField = sender as! NSTextField
        if let val = Float(txt.stringValue) {
            settingData[oldIndex].orbSunMoon[1] = val
            SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
        }
    }
    
    @IBAction func orb1stSoftChanged(_ sender: Any) {
        let txt:NSTextField = sender as! NSTextField
        if let val = Float(txt.stringValue) {
            settingData[oldIndex].orb1st[0] = val
            SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
        }
    }
    
    @IBAction func orb1stHardChanged(_ sender: Any) {
        let txt:NSTextField = sender as! NSTextField
        if let val = Float(txt.stringValue) {
            settingData[oldIndex].orb1st[1] = val
            SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
        }
    }
    
    @IBAction func orb2ndSoftChanged(_ sender: Any) {
        let txt:NSTextField = sender as! NSTextField
        if let val = Float(txt.stringValue) {
            settingData[oldIndex].orb2nd[0] = val
            SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
        }
    }
    
    @IBAction func orb2ndHardChanged(_ sender: Any) {
        let txt:NSTextField = sender as! NSTextField
        if let val = Float(txt.stringValue) {
            settingData[oldIndex].orb2nd[1] = val
            SettingSave.save(setting: settingData[oldIndex], url: FileInitialize.microcosmDirectory(), index: oldIndex)
        }
    }
    
}

extension SettingViewController: NSTableViewDataSource, NSTableViewDelegate
{
    // 行数を返す
    public func numberOfRows(in tableView: NSTableView) -> Int
    {
        return dispNames.count
    }
    
    // たぶんオブジェクトの型を返す
    // NSView返す際にしっかりしてれば問題なく動く
    public func tableView(
      _ tableView: NSTableView,
      objectValueFor tableColumn: NSTableColumn?,
      row: Int
    ) -> Any?
    {
        return nil
    }

    // セットされたら呼ばれる
    public func tableView(
            _ tableView: NSTableView,
            setObjectValue object: Any?,
            for tableColumn: NSTableColumn?,
            row: Int
    )
    {
        print("called")
    }
    
    // 現在のcolumnとrowのviewを返す、だよね
    // tableCellを返す
    public func tableView(
        _ tableView: NSTableView,
        viewFor tableColumn: NSTableColumn?,
        row: Int
    ) -> NSView?
    {
        // findByIdみたいなもの
        var view = tableView.makeView(withIdentifier: NSUserInterfaceItemIdentifier(rawValue: "MyView"), owner: self)
    
        // nullなら新規viewを作成
        if (view == nil)
        {
            if (tableColumn?.identifier.rawValue) != nil {
                view = NSTextField(labelWithString: dispNames[row].name)
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
    
    
    /// 選択が変わった時に呼ばれる
    /// - Parameter notification:
    public func tableViewSelectionDidChange(_ notification: Notification) {
        if (dispNameTable.selectedRow != oldIndex) {
            settingData[oldIndex].name = dispName.stringValue
        }
        
        let index = dispNameTable.selectedRow
        dispName.stringValue = settingData[index].name
        oldIndex = index
        
        switch(settingData[index].progression) {
        case 1:
            secondaryProgression.state = NSControl.StateValue.on
            primaryProgression.state = NSControl.StateValue.off
            solarArcProgression.state = NSControl.StateValue.off
            compositProgression.state = NSControl.StateValue.off
            break
        case 2:
            secondaryProgression.state = NSControl.StateValue.off
            primaryProgression.state = NSControl.StateValue.on
            solarArcProgression.state = NSControl.StateValue.off
            compositProgression.state = NSControl.StateValue.off
            break
        case 3:
            secondaryProgression.state = NSControl.StateValue.off
            primaryProgression.state = NSControl.StateValue.off
            solarArcProgression.state = NSControl.StateValue.on
            compositProgression.state = NSControl.StateValue.off
            break
        case 4:
            secondaryProgression.state = NSControl.StateValue.off
            primaryProgression.state = NSControl.StateValue.off
            solarArcProgression.state = NSControl.StateValue.off
            compositProgression.state = NSControl.StateValue.on
            break
        default:
            secondaryProgression.state = NSControl.StateValue.on
            primaryProgression.state = NSControl.StateValue.off
            solarArcProgression.state = NSControl.StateValue.off
            compositProgression.state = NSControl.StateValue.off
            break
        }

        switch(settingData[index].progression) {
        case 1:
            placidus.state = NSControl.StateValue.on
            koch.state = NSControl.StateValue.off
            campanus.state = NSControl.StateValue.off
            equal.state = NSControl.StateValue.off
            zeroAries.state = NSControl.StateValue.off
            break
        case 2:
            placidus.state = NSControl.StateValue.off
            koch.state = NSControl.StateValue.on
            campanus.state = NSControl.StateValue.off
            equal.state = NSControl.StateValue.off
            zeroAries.state = NSControl.StateValue.off
            break
        case 3:
            placidus.state = NSControl.StateValue.off
            koch.state = NSControl.StateValue.off
            campanus.state = NSControl.StateValue.on
            equal.state = NSControl.StateValue.off
            zeroAries.state = NSControl.StateValue.off
            break
        case 4:
            placidus.state = NSControl.StateValue.off
            koch.state = NSControl.StateValue.off
            campanus.state = NSControl.StateValue.off
            equal.state = NSControl.StateValue.on
            zeroAries.state = NSControl.StateValue.off
            break
        case 5:
            placidus.state = NSControl.StateValue.off
            koch.state = NSControl.StateValue.off
            campanus.state = NSControl.StateValue.off
            equal.state = NSControl.StateValue.off
            zeroAries.state = NSControl.StateValue.on
            break
        default:
            placidus.state = NSControl.StateValue.on
            koch.state = NSControl.StateValue.off
            campanus.state = NSControl.StateValue.off
            equal.state = NSControl.StateValue.off
            zeroAries.state = NSControl.StateValue.off
        }

        if (settingData[index].sameCusps) {
            sameCuspsOn.state = NSControl.StateValue.on
            sameCuspsOff.state = NSControl.StateValue.off
        } else {
            sameCuspsOn.state = NSControl.StateValue.off
            sameCuspsOff.state = NSControl.StateValue.on
        }

        
        dispPlanetSun.state = settingData[index].dispPlanetSun == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetMoon.state = settingData[index].dispPlanetMoon == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetMercury.state = settingData[index].dispPlanetMercury == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetVenus.state = settingData[index].dispPlanetVenus == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetMars.state = settingData[index].dispPlanetMars == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetJupiter.state = settingData[index].dispPlanetJupiter == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetSaturn.state = settingData[index].dispPlanetSaturn == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetUranus.state = settingData[index].dispPlanetUranus == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetNeptune.state = settingData[index].dispPlanetNeptune == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetPluto.state = settingData[index].dispPlanetPluto == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetAsc.state = settingData[index].dispPlanetAsc == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetMc.state = settingData[index].dispPlanetMc == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetChiron.state = settingData[index].dispPlanetChiron == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetDragonH.state = settingData[index].dispPlanetDH == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetDragonT.state = settingData[index].dispPlanetDT == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetLilith.state = settingData[index].dispPlanetLilith == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetEarth.state = settingData[index].dispPlanetEarth == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetCeres.state = settingData[index].dispPlanetCeres == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetPallas.state = settingData[index].dispPlanetPallas == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetJuno.state = settingData[index].dispPlanetJuno == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetVesta.state = settingData[index].dispPlanetVesta == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetPoF.state = settingData[index].dispPlanetPof == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispPlanetPoS.state = settingData[index].dispPlanetPos == 1 ? NSControl.StateValue.on : NSControl.StateValue.off

        dispAspectPlanetSun.state = settingData[index].dispAspectPlanetSun == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetMoon.state = settingData[index].dispAspectPlanetMoon == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetMercury.state = settingData[index].dispAspectPlanetMercury == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetVenus.state = settingData[index].dispAspectPlanetVenus == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetMars.state = settingData[index].dispAspectPlanetMars == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetJupiter.state = settingData[index].dispAspectPlanetJupiter == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetSaturn.state = settingData[index].dispAspectPlanetSaturn == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetUranus.state = settingData[index].dispAspectPlanetUranus == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetNeptune.state = settingData[index].dispAspectPlanetNeptune == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetPluto.state = settingData[index].dispAspectPlanetPluto == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetAsc.state = settingData[index].dispAspectPlanetAsc == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetMc.state = settingData[index].dispAspectPlanetMc == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetChiron.state = settingData[index].dispAspectPlanetChiron == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dspAspectPlanetDH.state = settingData[index].dispAspectPlanetDH == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetDT.state = settingData[index].dispAspectPlanetDT == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetLilith.state = settingData[index].dispAspectPlanetLilith == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetEarth.state = settingData[index].dispAspectPlanetEarth == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetCeres.state = settingData[index].dispAspectPlanetCeres == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetPallas.state = settingData[index].dispAspectPlanetPallas == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetJuno.state = settingData[index].dispAspectPlanetJuno == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetVesta.state = settingData[index].dispAspectPlanetVesta == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetPof.state = settingData[index].dispAspectPlanetPof == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispAspectPlanetPos.state = settingData[index].dispAspectPlanetPos == 1 ? NSControl.StateValue.on : NSControl.StateValue.off

        dispConjunction.state = settingData[index].dispConjunction == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispOpposition.state = settingData[index].dispOpposition == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispTrine.state = settingData[index].dispTrine == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispSquare.state = settingData[index].dispSquare == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispSextile.state = settingData[index].dispSextile == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispInconjunct.state = settingData[index].dispInconjunct == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispSesquiquadrate.state = settingData[index].dispSesquiQuadrate == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispSemiSquare.state = settingData[index].dispSemiSquare == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispSemiSextile.state = settingData[index].dispSemiSextile == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispNovile.state = settingData[index].dispNovile == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispSeptile.state = settingData[index].dispAspectSeptile == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispQuintile.state = settingData[index].dispQuintile == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispBiQuintile.state = settingData[index].dispAspectBiQuintile == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispSemiQuintile.state = settingData[index].dispAspectSemiQuintile == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        dispQuindecile.state = settingData[index].dispAspectQuindecile == 1 ? NSControl.StateValue.on : NSControl.StateValue.off
        
        orbSunMoonSoft.stringValue = String(settingData[index].orbSunMoon[0])
        orbSunMoonHard.stringValue = String(settingData[index].orbSunMoon[1])
        orb1stSoft.stringValue = String(settingData[index].orb1st[0])
        orb1stHard.stringValue = String(settingData[index].orb1st[1])
        orb2ndSoft.stringValue = String(settingData[index].orb2nd[0])
        orb2ndHard.stringValue = String(settingData[index].orb2nd[1])

    }
}
