//
//  TimeSetDetailViewController.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/12/27.
//

import Cocoa


/// タイムセッターの時刻変更ユニットコントローラー
class TimeSetDetailViewController: NSViewController {

    @IBOutlet weak var unitRadio: NSButton!
    @IBOutlet weak var newMoonRadio: NSButton!
    @IBOutlet weak var fullMoonRadio: NSButton!
    @IBOutlet weak var unit: NSTextField!
    
    @IBOutlet weak var secondRadio: NSButton!
    @IBOutlet weak var minuteRadio: NSButton!
    @IBOutlet weak var hourRadio: NSButton!
    @IBOutlet weak var dayRadio: NSButton!
    @IBOutlet weak var solarReturnRadio: NSButton!
    @IBOutlet weak var ingressRadio: NSButton!
    
    @IBOutlet weak var ingressCombo: NSPopUpButton!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        // Do view setup here.
        let delegate = NSApplication.shared.delegate as! AppDelegate

        if unit.stringValue == "" {
            unit.stringValue = "1"
        }
        
        if ingressRadio.state == NSControl.StateValue.off {
            ingressCombo.removeAllItems()
            ingressCombo.addItem(withTitle: "太陽")
            ingressCombo.addItem(withTitle: "月")
        }
        
        if delegate.currentSpanType == SpanType.UNIT {
            unitRadio.state = NSControl.StateValue.on
            newMoonRadio.state = NSControl.StateValue.off
            fullMoonRadio.state = NSControl.StateValue.off
            solarReturnRadio.state = NSControl.StateValue.off
            ingressRadio.state = NSControl.StateValue.off
            
            if ((delegate.viewController?.timesetterButton.title.contains("Hour")) == true) {
                secondRadio.state = NSControl.StateValue.off
                hourRadio.state = NSControl.StateValue.on
            }
            if ((delegate.viewController?.timesetterButton.title.contains("Minute")) == true) {
                secondRadio.state = NSControl.StateValue.off
                minuteRadio.state = NSControl.StateValue.on
            }
            if ((delegate.viewController?.timesetterButton.title.contains("Second")) == true) {
                secondRadio.state = NSControl.StateValue.on
            }
            if ((delegate.viewController?.timesetterButton.title.contains("Day")) == true) {
                secondRadio.state = NSControl.StateValue.on
                dayRadio.state = NSControl.StateValue.on
            }
        }

        if delegate.currentSpanType == SpanType.NEWMOON {
            unitRadio.state = NSControl.StateValue.off
            newMoonRadio.state = NSControl.StateValue.on
            fullMoonRadio.state = NSControl.StateValue.off
            solarReturnRadio.state = NSControl.StateValue.off
            ingressRadio.state = NSControl.StateValue.off
        }
        if delegate.currentSpanType == SpanType.FULLMOON {
            unitRadio.state = NSControl.StateValue.off
            newMoonRadio.state = NSControl.StateValue.off
            fullMoonRadio.state = NSControl.StateValue.on
            solarReturnRadio.state = NSControl.StateValue.off
            ingressRadio.state = NSControl.StateValue.off
        }
        if delegate.currentSpanType == SpanType.SOLARRETURN {
            unitRadio.state = NSControl.StateValue.off
            newMoonRadio.state = NSControl.StateValue.off
            fullMoonRadio.state = NSControl.StateValue.off
            solarReturnRadio.state = NSControl.StateValue.on
            ingressRadio.state = NSControl.StateValue.off
        }
        if delegate.currentSpanType == SpanType.SOLARINGRESS {
            unitRadio.state = NSControl.StateValue.off
            newMoonRadio.state = NSControl.StateValue.off
            fullMoonRadio.state = NSControl.StateValue.off
            solarReturnRadio.state = NSControl.StateValue.off
            ingressRadio.state = NSControl.StateValue.on
            ingressCombo.selectItem(withTitle: "太陽")
        }
        
        if delegate.currentSpanType == SpanType.MOONINGRESS {
            unitRadio.state = NSControl.StateValue.off
            newMoonRadio.state = NSControl.StateValue.off
            fullMoonRadio.state = NSControl.StateValue.off
            solarReturnRadio.state = NSControl.StateValue.off
            ingressRadio.state = NSControl.StateValue.on
            ingressCombo.selectItem(withTitle: "月")
        }

        unit.stringValue = String(delegate.currentSpanUnitNum)
    }
    
    @IBAction func unitRadioClicked(_ sender: Any) {
        newMoonRadio.state = NSControl.StateValue.off
        fullMoonRadio.state = NSControl.StateValue.off
        solarReturnRadio.state = NSControl.StateValue.off
        ingressRadio.state = NSControl.StateValue.off
    }
    
    @IBAction func newMoonClicked(_ sender: Any) {
        unitRadio.state = NSControl.StateValue.off
        fullMoonRadio.state = NSControl.StateValue.off
        solarReturnRadio.state = NSControl.StateValue.off
        ingressRadio.state = NSControl.StateValue.off
    }
    
    @IBAction func fullMoonClicked(_ sender: Any) {
        unitRadio.state = NSControl.StateValue.off
        newMoonRadio.state = NSControl.StateValue.off
        solarReturnRadio.state = NSControl.StateValue.off
        ingressRadio.state = NSControl.StateValue.off
    }
    
    @IBAction func solarReturnClicked(_ sender: Any) {
        unitRadio.state = NSControl.StateValue.off
        fullMoonRadio.state = NSControl.StateValue.off
        newMoonRadio.state = NSControl.StateValue.off
        ingressRadio.state = NSControl.StateValue.off
    }
    
    @IBAction func ingressClicked(_ sender: Any) {
        unitRadio.state = NSControl.StateValue.off
        fullMoonRadio.state = NSControl.StateValue.off
        newMoonRadio.state = NSControl.StateValue.off
        solarReturnRadio.state = NSControl.StateValue.off
    }
    
    @IBAction func secondRadioClicked(_ sender: Any) {
        minuteRadio.state = NSControl.StateValue.off
        hourRadio.state = NSControl.StateValue.off
        dayRadio.state = NSControl.StateValue.off
    }
    
    @IBAction func minuteRadioClicked(_ sender: Any) {
        secondRadio.state = NSControl.StateValue.off
        hourRadio.state = NSControl.StateValue.off
        dayRadio.state = NSControl.StateValue.off
    }
    
    @IBAction func hourRadioClicked(_ sender: Any) {
        secondRadio.state = NSControl.StateValue.off
        minuteRadio.state = NSControl.StateValue.off
        dayRadio.state = NSControl.StateValue.off
    }
    
    @IBAction func dayRadioClicked(_ sender: Any) {
        secondRadio.state = NSControl.StateValue.off
        minuteRadio.state = NSControl.StateValue.off
        hourRadio.state = NSControl.StateValue.off
    }
    
    @IBAction func submitClicked(_ sender: Any) {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        if unitRadio.state == NSControl.StateValue.on {
            var unitString = ""
            if secondRadio.state == NSControl.StateValue.on {
                unitString = " Second"
            } else if minuteRadio.state == NSControl.StateValue.on {
                unitString = " Minute"
            } else if hourRadio.state == NSControl.StateValue.on {
                unitString = " Hour"
            } else if dayRadio.state == NSControl.StateValue.on {
                unitString = " Day"
            }
            
            if unit.stringValue == "" {
                unit.stringValue = "1"
            }
            
            delegate.viewController?.timesetterButton.title = unit.stringValue + unitString
            delegate.currentSpanType = SpanType.UNIT
            delegate.currentSpanUnitNum = Int(unit.stringValue) ?? 1
        } else if newMoonRadio.state == NSControl.StateValue.on {
            delegate.viewController?.timesetterButton.title = "新月"
            delegate.currentSpanType = SpanType.NEWMOON
        } else if fullMoonRadio.state == NSControl.StateValue.on {
            delegate.viewController?.timesetterButton.title = "満月"
            delegate.currentSpanType = SpanType.FULLMOON
        } else if solarReturnRadio.state == NSControl.StateValue.on {
            delegate.viewController?.timesetterButton.title = "太陽回帰"
            delegate.currentSpanType = SpanType.SOLARRETURN
        } else if ingressRadio.state == NSControl.StateValue.on {
            let unitString = ingressCombo.selectedItem?.title ?? ""
            delegate.viewController?.timesetterButton.title = "Ingress " + unitString
            if unitString == "太陽" {
                delegate.currentSpanType = SpanType.SOLARINGRESS
            } else if unitString == "月" {
                delegate.currentSpanType = SpanType.MOONINGRESS
            }
        }
        dismiss(self)
    }
}
