//
//  TimeSetViewController.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/05/07.
//

import Cocoa


/// timeSetterの現在時刻設定コントローラー
class TimeSetViewController: NSViewController {

    @IBOutlet weak var datePicker: NSDatePicker!
    
    @IBOutlet weak var hour: NSTextField!
    
    @IBOutlet weak var minute: NSTextField!
    
    @IBOutlet weak var second: NSTextField!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        // Do view setup here.
        let delegate = NSApplication.shared.delegate as! AppDelegate
        let udata = delegate.viewController!.getTargetUser()
        let myDate = MyDate()
        myDate.setUserData(u: udata)

        datePicker.dateValue = myDate.date
        hour.stringValue = String(myDate.getHour())
        minute.stringValue = String(myDate.getMinute())
        second.stringValue = String(myDate.getSecond())
    }
    
    @IBAction func SubmitClicked(_ sender: Any) {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        let myDate = MyDate(date: datePicker.dateValue)
        if let h = Int(hour.stringValue) {
            myDate.setHour(hour: h)
        } else {
            myDate.setHour(hour: 12)
        }
        if let m = Int(minute.stringValue) {
            myDate.setMinute(minute: m)
        } else {
            myDate.setMinute(minute: 0)
        }
        if let s = Int(second.stringValue) {
            myDate.setSecond(second: s)
        } else {
            myDate.setSecond(second: 0)
        }
        
        delegate.viewController!.timeSetterSpecificDate(myDate: myDate)

        dismiss(self)
    }
}
