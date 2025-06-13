//
//  ConfigViewController.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/02/05.
//

import Cocoa

class ConfigViewController: NSViewController {
    public var addrs: [Addr]
    public var delegate: AppDelegate
    public var u: URL

    
    @IBOutlet weak var timeZoneCombo: NSComboBox!
    @IBOutlet weak var defaultPlace: NSTextField!
    @IBOutlet weak var defaultLat: NSTextField!
    @IBOutlet weak var defaultLng: NSTextField!
    @IBOutlet weak var addrTable: NSTableView!
    
    required init?(coder aDecoder: NSCoder) {
        addrs = []
        delegate = NSApplication.shared.delegate as! AppDelegate
        u = FileInitialize.microcosmDirectory()

        super.init(coder:aDecoder)
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        // Do view setup here.

        // latlng
        let addrFile = AddrFile()
        addrs = addrFile.readCsv(u: u)
        
        addrTable.delegate = self
        addrTable.dataSource = self

        // timezone
        timeZoneCombo.removeAllItems()
        
        for t in TimeZoneConst.timezones {
            timeZoneCombo.addItem(withObjectValue: t)
        }
        timeZoneCombo.selectItem(at: 38)

        // config復帰
        defaultPlace.stringValue = delegate.config.defaultPlace
        defaultLat.stringValue = delegate.config.defaultLat
        defaultLng.stringValue = delegate.config.defaultLng

        // スクリーンチェンジ
        NotificationCenter.default.addObserver(
            forName: NSWindow.didChangeScreenNotification,
            object: nil, queue: nil,
            using: changed)
    }
    
    @IBAction func AddrTableClicked(_ sender: Any) {
        let table = sender as! NSTableView
        let row = table.selectedRow
        defaultPlace.stringValue = String(addrs[row].name)
        defaultLat.stringValue = String(addrs[row].lat)
        defaultLng.stringValue = String(addrs[row].lng)
        
        var config = delegate.config
        config.defaultPlace = String(addrs[row].name)
        config.defaultLat = String(addrs[row].lat)
        config.defaultLng = String(addrs[row].lng)
        ConfigSave.save(config: config, url: u)
    }
}

func changed(notification : Notification) -> Void {
    
//        let object = notification.object as! NSWindow
//        print(object.identifier!.rawValue)
//        print(self.view.identifier!.rawValue)
    
//        print("did2")
}

extension ConfigViewController: NSTableViewDataSource, NSTableViewDelegate
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
        //print("called")
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
                print(unwrapped)
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
