//
//  UserEditViewController.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/03/29.
//

import Cocoa

class UserEditViewController: NSViewController {

    @IBOutlet weak var fileName: NSTextField!
    
    @IBOutlet weak var name: NSTextField!
    
    @IBOutlet weak var dateCombo: NSDatePicker!
    
    @IBOutlet weak var hour: NSTextField!
    
    @IBOutlet weak var minute: NSTextField!
    
    @IBOutlet weak var second: NSTextField!
    
    @IBOutlet weak var timeZoneCombo: NSComboBox!
    
    @IBOutlet weak var memo: NSTextField!
    
    @IBOutlet weak var place: NSTextField!
    
    @IBOutlet weak var lat: NSTextField!
    
    @IBOutlet weak var lng: NSTextField!
    
    @IBOutlet weak var addrTable: NSTableView!
    
    public var addrs: [Addr]
    public var delegate: AppDelegate
    public var baseURL: URL
    public var parentController: DatabaseViewController?
    
    public var isAppend: Bool = false
    public var isEdit: Bool = false
    public var editIndex: Int = 0
    public var fileURL: URL = URL(fileURLWithPath: "")

    @IBOutlet weak var submitButton: NSButton!
    
    required init?(coder aDecoder: NSCoder) {
        delegate = NSApplication.shared.delegate as! AppDelegate
        addrs = []
        baseURL = URL(fileURLWithPath: "")

        super.init(coder:aDecoder)
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        // Do view setup here.
        
        
        let currentDate = Date()
        let dateFormatter = DateFormatter()
        dateFormatter.dateFormat = "yyyyMMddHHmmss"
        fileName.stringValue = dateFormatter.string(from: currentDate)
        
        dateFormatter.dateFormat = "H"
        hour.stringValue = dateFormatter.string(from: currentDate)
        dateFormatter.dateFormat = "m"
        minute.stringValue = dateFormatter.string(from: currentDate)
        dateFormatter.dateFormat = "s"
        second.stringValue = dateFormatter.string(from: currentDate)

        name.stringValue = "ユーザー名"
        
        place.stringValue = delegate.config.defaultPlace
        lat.stringValue = delegate.config.defaultLat
        lng.stringValue = delegate.config.defaultLng
        
        // latlng
        let addrFile = AddrFile()
        addrs = addrFile.readCsv(u: FileInitialize.microcosmDirectory())
        
        addrTable.delegate = self
        addrTable.dataSource = self

        // timezone
        timeZoneCombo.removeAllItems()
        
        for t in TimeZoneConst.timezones {
            timeZoneCombo.addItem(withObjectValue: t)
        }
        timeZoneCombo.selectItem(at: 38)

        memo.stringValue = "メモ"
        
    }
    
    @IBAction func addrTableSelected(_ sender: Any) {
        let table = sender as! NSTableView
        let row = table.selectedRow
        place.stringValue = String(addrs[row].name)
        lat.stringValue = String(addrs[row].lat)
        lng.stringValue = String(addrs[row].lng)
    }
    
    @IBAction func submitClicked(_ sender: Any) {
        var user = UserData()
        
        user.name = name.stringValue
        user.lat = Double(lat.stringValue)!
        user.lng = Double(lng.stringValue)!
        user.birth_place = place.stringValue
        user.memo = memo.stringValue
        let d = dateCombo.dateValue
        let dateFormatter = DateFormatter()
        dateFormatter.dateFormat = "yyyy"
        user.birth_year = Int(dateFormatter.string(from: d))!
        dateFormatter.dateFormat = "M"
        user.birth_month = Int(dateFormatter.string(from: d))!
        dateFormatter.dateFormat = "d"
        user.birth_day = Int(dateFormatter.string(from: d))!
        user.birth_hour = Int(hour.stringValue) ?? 12
        user.birth_minute = Int(minute.stringValue) ?? 0
        user.birth_second = Int(second.stringValue) ?? 0
        user.birth_timezone_index = timeZoneCombo.indexOfSelectedItem
        user.birth_timezone = TimeZoneConst.timezone_time[timeZoneCombo.indexOfSelectedItem]

        if (isEdit == false) {
            fileURL = baseURL.appendingPathComponent(fileName.stringValue + ".json")
            if (isAppend == true) {
                var userList = UserList(list: [])
                let users = DatabaseUser.load(url: fileURL)
                for (_, u) in users.enumerated() {
                    userList.list.append(u.userData)
                }
                userList.list.append(user)
                UserSave.save(user: userList, url: fileURL)
            } else {
                let userList = UserList(list: [user])
                UserSave.save(user: userList, url: fileURL)
            }
        } else {
            var userList = UserList(list: [])
            // ファイル読み込み
            let users = DatabaseUser.load(url: fileURL)
            // ループ
            for (index, u) in users.enumerated() {
                // indexだけ更新
                if (index != editIndex) {
                    userList.list.append(u.userData)
                } else {
                    userList.list.append(user)
                }
            }
            UserSave.save(user: userList, url: fileURL)
        }
        
        parentController?.databaseDirectories.refresh()
        parentController?.directories.reloadData()
        parentController?.reload(url: fileURL)
        parentController?.usersTable.reloadData()

        self.view.window?.close()
    }
    
}

extension UserEditViewController: NSTableViewDataSource, NSTableViewDelegate
{
    // 行数を返す
    public func numberOfRows(in tableView: NSTableView) -> Int
    {
        return addrs.count
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
            if let unwrapped = tableColumn?.identifier.rawValue {
                if (unwrapped == "place") {
                    view = NSTextField(labelWithString: addrs[row].name);
                    view?.identifier = NSUserInterfaceItemIdentifier(rawValue: unwrapped + String(row))
                } else if (unwrapped == "AutomaticTableColumnIdentifier.1") {
                    view = NSTextField(labelWithString: (String)(addrs[row].lat));
                    view?.identifier = NSUserInterfaceItemIdentifier(rawValue: unwrapped + String(row))
                } else {
                    view = NSTextField(labelWithString: (String)(addrs[row].lng));
                    view?.identifier = NSUserInterfaceItemIdentifier(rawValue: unwrapped + String(row))
                }
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

    public func tableView(
        _ tableView: NSTableView,
        didAdd rowView: NSTableRowView,
        forRow row: Int
    )
    {
        //print("didAdd")
    }

    public func tableView(
        _ tableView: NSTableView,
        didRemove rowView: NSTableRowView,
        forRow row: Int
    )
    {
        //print("didRemove")
    }

    public func tableView(
        _ tableView: NSTableView,
        heightOfRow row: Int
    ) -> CGFloat
    {
        return 20.0
    }

    public func tableView(
        _ tableView: NSTableView,
        sizeToFitWidthOfColumn column: Int
    ) -> CGFloat
    {
        return 50.0
    }

}
