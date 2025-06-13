//
//  DatabaseViewController.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/03/25.
//

import Foundation
import AppKit

class DatabaseViewController: NSViewController {
    public var databaseDirectories: DatabaseDirectory
    public var databaseUsers: [DatabaseUser]
    public var delegate: AppDelegate

    @IBOutlet weak var directories: NSOutlineView!
    
    @IBOutlet weak var memo: NSTextField!
    
    @IBOutlet weak var editButton: NSButton!
    
    @IBOutlet weak var deleteButton: NSButton!

    @IBOutlet weak var usersTable: DatabaseUsersView!
    
    @IBOutlet weak var user1Field: NSTextField!
    @IBOutlet weak var user2Field: NSTextField!
    @IBOutlet weak var event1Field: NSTextField!
    @IBOutlet weak var event2Field: NSTextField!
    
    @IBOutlet weak var importComboBox: NSComboBox!
    
    
    
    required init?(coder: NSCoder) {
        self.databaseDirectories = DatabaseDirectory(name: "data", fullPath: FileInitialize.microcosmDirectory().appendingPathComponent("data"))
        self.databaseUsers = [DatabaseUser]()
        delegate = NSApplication.shared.delegate as! AppDelegate

        super.init(coder:coder)
    }
    
    public override func viewDidLoad() {
        let delegate = NSApplication.shared.delegate as! AppDelegate

        directories.dataSource = self
        directories.delegate = self
        
        directories.expandItem(directories.item(atRow: 0))
        
        
        usersTable.dataSource = self
        usersTable.delegate = self
        usersTable.parentViewController = self
        
        delegate.databaseViewController = self
        
        
        user1Field.stringValue = delegate.udata1.name + "\n"
        + Util.UserDataBirthStr(userData: delegate.udata1) + "\n"
        + delegate.udata1.birth_place
        user2Field.stringValue = delegate.udata2.name + "\n"
        + Util.UserDataBirthStr(userData: delegate.udata2) + "\n"
        + delegate.udata2.birth_place
        event1Field.stringValue = delegate.edata1.name + "\n"
        + Util.UserDataBirthStr(userData: delegate.edata1) + "\n"
        + delegate.edata1.birth_place
        event2Field.stringValue = delegate.edata2.name + "\n"
        + Util.UserDataBirthStr(userData: delegate.edata2) + "\n"
        + delegate.edata2.birth_place
        
        importComboBox.removeAllItems()
        importComboBox.addItem(withObjectValue: "AMATERU(csv)")
        importComboBox.addItem(withObjectValue: "StarGazer")
        importComboBox.addItem(withObjectValue: "Zet(zbs)")
        importComboBox.addItem(withObjectValue: "CSV(csv)")
        importComboBox.selectItem(at: 0)

        editButton.becomeFirstResponder()
        
        directories.menu = NSMenu(title: "title")
    }

    
    /// ここはViewController全体
    /// - Parameter theEvent:
    override func rightMouseUp(with theEvent : NSEvent) {
//        print("right mouse1")
    }
    
        
    /// ディレクトリ追加
    /// - Parameter sender:
    @IBAction func addDirectoryButtonClicked(_ sender: Any) {
        guard let node = directories.item(atRow: directories.selectedRow) as? DatabaseDirectory else {
            return
        }
        let vc = self.storyboard?.instantiateController(withIdentifier: "directoryModal") as! DirectoryEditViewController
        vc.setBasePath(basePath: node.fullPath)
        vc.parentController = self
        let myWindow:NSWindow? = NSWindow(contentViewController: vc)
        myWindow?.makeKeyAndOrderFront(self)
        let wc = NSWindowController(window: myWindow)
        wc.showWindow(self)

    }
    
    
    /// ファイル追加(ユーザー追加)
    /// - Parameter sender:
    @IBAction func addFileButtonClicked(_ sender: Any) {
        var selected = directories.selectedRow
        if (selected == -1) {
            selected = 0
        }
        guard let node = directories.item(atRow: selected) as? DatabaseDirectory else {
            return
        }

        let vc = self.storyboard?.instantiateController(withIdentifier: "user") as! UserEditViewController
        if (node.isDirectory()) {
            vc.baseURL = node.fullPath
        } else {
            vc.baseURL = node.fullPath.deletingLastPathComponent()
        }
        vc.parentController = self
        let myWindow:NSWindow? = NSWindow(contentViewController: vc)
        myWindow?.makeKeyAndOrderFront(self)
        let wc = NSWindowController(window: myWindow)
        wc.showWindow(self)
    }
    
    
    /// ファイル編集(ユーザー編集じゃない)
    /// - Parameter sender:
    @IBAction func editButtonClicked(_ sender: Any) {
        if (directories.selectedRow == 0) {
            return
        }
        guard let node = directories.item(atRow: directories.selectedRow) as? DatabaseDirectory else {
            return
        }
        if (node.isDirectory()) {
            let vc = self.storyboard?.instantiateController(withIdentifier: "directoryModal") as! DirectoryEditViewController
            vc.setBasePath(basePath: node.fullPath.deletingLastPathComponent())
            vc.setOldPath(oldPath: node.fullPath)
            vc.parentController = self

            let myWindow:NSWindow? = NSWindow(contentViewController: vc)
            myWindow?.makeKeyAndOrderFront(self)
            let wc = NSWindowController(window: myWindow)
            wc.showWindow(self)
            vc.setDirectoryname(name: node.name)
        } else {
            let vc = self.storyboard?.instantiateController(withIdentifier: "fileModal") as! UserFileEditViewController
            vc.setBasePath(basePath: node.fullPath.deletingLastPathComponent())
            vc.setOldPath(oldPath: node.fullPath)
            vc.parentController = self
            
            let myWindow:NSWindow? = NSWindow(contentViewController: vc)
            myWindow?.makeKeyAndOrderFront(self)
            let wc = NSWindowController(window: myWindow)
            wc.showWindow(self)
            vc.setFileName(name: node.name)
        }
    }
    
    @IBAction func removeButtonClicked(_ sender: Any) {
        if (directories.selectedRow == 0) {
            print("cannot")
            return
        }
        
        guard let node = directories.item(atRow: directories.selectedRow) as? DatabaseDirectory else {
            return
        }
        
        let fileManager = FileManager()
        do {
            try fileManager.removeItem(at: node.fullPath)
        } catch {
            print("failed")
        }
        
        databaseDirectories.refresh()
        directories.reloadData()
    }
    
    @IBAction func addEventClicked(_ sender: Any) {
        addEvent()
    }
    
    public func addEvent() {
        if (directories.selectedRow == 0) {
            print("cannot")
            return
        }
        
        guard let node = directories.item(atRow: directories.selectedRow) as? DatabaseDirectory else {
            return
        }

        let vc = self.storyboard?.instantiateController(withIdentifier: "user") as! UserEditViewController
        let myWindow:NSWindow? = NSWindow(contentViewController: vc)
        myWindow?.makeKeyAndOrderFront(self)
        vc.fileName.stringValue = node.getFileNameWithoutExtension()
        vc.baseURL = node.fullPath.deletingLastPathComponent()
        vc.isAppend = true
        vc.parentController = self
        vc.fileName.isEditable = false
        let wc = NSWindowController(window: myWindow)
        wc.showWindow(self)
    }
    
    @IBAction func removeEventClicked(_ sender: Any) {
        removeEvent()
    }
    
    public func removeEvent() {
        let row = databaseUsers[usersTable.selectedRow]
        var userList = UserList(list: [])
        // ファイル読み込み
        let users = DatabaseUser.load(url: row.url)
        // ループ
        for (index, u) in users.enumerated() {
            // indexだけ除く
            if (index != usersTable.selectedRowIndexes.first) {
                userList.list.append(u.userData)
            } else {
                continue
            }
        }
        UserSave.save(user: userList, url: row.url)

        databaseDirectories.refresh()
        directories.reloadData()
        reload(url: row.url)
        usersTable.reloadData()
    }
    
    @IBAction func editEventClicked(_ sender: Any) {
        editEvent()
    }
    
    public func editEvent() {
        if (usersTable.selectedRow == -1) {
            print("cannot")
            return
        }
        
        let row = databaseUsers[usersTable.selectedRow]

        let vc = self.storyboard?.instantiateController(withIdentifier: "user") as! UserEditViewController
        let myWindow:NSWindow? = NSWindow(contentViewController: vc)
        myWindow?.makeKeyAndOrderFront(self)
        vc.parentController = self
        vc.fileName.isEditable = false
        vc.submitButton.title = "更新"
        vc.fileURL = row.url
        vc.isEdit = true
        vc.editIndex = usersTable.selectedRow
        
        vc.fileName.stringValue = NSString(string: row.url.lastPathComponent).deletingPathExtension
        vc.name.stringValue = row.userData.name
        
        let formatter: DateFormatter = DateFormatter()
        formatter.timeZone = TimeZone(identifier: "Asia/Tokyo")
        formatter.dateFormat = "yyyyMMdd"
        
        let dateStr = String(format: "%04d", row.userData.birth_year) + String(format: "%02d", row.userData.birth_month) + String(format: "%02d", row.userData.birth_day)

        vc.dateCombo.dateValue = formatter.date(from: dateStr)!
        vc.hour.stringValue = String(row.userData.birth_hour)
        vc.minute.stringValue = String(row.userData.birth_minute)
        vc.second.stringValue = String(row.userData.birth_second)
        vc.lat.stringValue = String(row.userData.lat)
        vc.lng.stringValue = String(row.userData.lng)
        vc.place.stringValue = String(row.userData.birth_place)
        
        vc.memo.stringValue = row.userData.memo
        
        let wc = NSWindowController(window: myWindow)
        wc.showWindow(self)
    }
    
    @IBAction func directorySelectionChanged(_ sender: Any) {
        // このイベントはselectionChangedというかClicked
        // なのでselectionにかかわらず発火
    }
    
    @IBAction func userTableSelectionChanged(_ sender: Any) {
        // このイベントはselectionChangedというかClicked
        // なのでselectionにかかわらず発火
    }
    
    @IBAction func user1Clicked(_ sender: Any) {
        let index = usersTable.selectedRow
        delegate.udata1 = databaseUsers[index].userData
        user1Field.stringValue = databaseUsers[index].userData.name + "\n"
            + Util.UserDataBirthStr(userData: databaseUsers[index].userData) + "\n"
        + databaseUsers[index].userData.birth_place
        
        delegate.viewController?.setUserBox()
    }
    
    @IBAction func user2Clicked(_ sender: Any) {
        let index = usersTable.selectedRow
        delegate.udata2 = databaseUsers[index].userData
        user2Field.stringValue = databaseUsers[index].userData.name + "\n"
            + Util.UserDataBirthStr(userData: databaseUsers[index].userData) + "\n"
        + databaseUsers[index].userData.birth_place
        
        delegate.viewController?.setUserBox()
    }
    
    @IBAction func event1Clicked(_ sender: Any) {
        let index = usersTable.selectedRow
        delegate.edata1 = databaseUsers[index].userData
        event1Field.stringValue = databaseUsers[index].userData.name + "\n"
            + Util.UserDataBirthStr(userData: databaseUsers[index].userData) + "\n"
        + databaseUsers[index].userData.birth_place
        
        delegate.viewController?.setUserBox()
    }
    
    @IBAction func event2Clicked(_ sender: Any) {
        let index = usersTable.selectedRow
        delegate.edata2 = databaseUsers[index].userData
        event2Field.stringValue = databaseUsers[index].userData.name + "\n"
            + Util.UserDataBirthStr(userData: databaseUsers[index].userData) + "\n"
        + databaseUsers[index].userData.birth_place
        
        delegate.viewController?.setUserBox()
    }
    
    
    @IBAction func importClicked(_ sender: Any) {
        let index = importComboBox.indexOfSelectedItem
        if (index < 0) {
            return
        }
        print(index)
        
        let content = openFileDialog()
        let myDate = MyDate()
        
        if (content.count == 0) {
            return
        }

        if (index == 0)
        {
            //AMATERU
            let dataList = Amateru.readCsv(content: content)
            let currentDateStr = myDate.getAmateruString()
            let baseURL = FileInitialize.microcosmDirectory().appendingPathComponent("data")
            let fileURL = baseURL.appendingPathComponent(currentDateStr + ".json")
            UserSave.save(user: dataList, url: fileURL)

            databaseDirectories.refresh()
            directories.reloadData()
        }
        else if (index == 1)
        {
            // StarGazer
            let delegate = NSApplication.shared.delegate as! AppDelegate
            let dataList = StarGazer.read(content: content, defaultLat: delegate.config.defaultLat, defaultLng: delegate.config.defaultLng)
            let currentDateStr = myDate.getStarGazerString()
            let baseURL = FileInitialize.microcosmDirectory().appendingPathComponent("data")
            let fileURL = baseURL.appendingPathComponent(currentDateStr + ".json")
            UserSave.save(user: dataList, url: fileURL)

            databaseDirectories.refresh()
            directories.reloadData()
        }
        else if (index == 2)
        {
            //zet
            let delegate = NSApplication.shared.delegate as! AppDelegate
            let dataList = Zet.read(content: content, defaultLat: delegate.config.defaultLat, defaultLng: delegate.config.defaultLng)
            let currentDateStr = myDate.getZetString()
            let baseURL = FileInitialize.microcosmDirectory().appendingPathComponent("data")
            let fileURL = baseURL.appendingPathComponent(currentDateStr + ".json")
            UserSave.save(user: dataList, url: fileURL)

            databaseDirectories.refresh()
            directories.reloadData()
        }
        else if (index == 3)
        {
            // csv
            let delegate = NSApplication.shared.delegate as! AppDelegate
            let dataList = Csv.read(content: content, defaultLat: delegate.config.defaultLat, defaultLng: delegate.config.defaultLng)
            let currentDateStr = myDate.getCsvString()
            let baseURL = FileInitialize.microcosmDirectory().appendingPathComponent("data")
            let fileURL = baseURL.appendingPathComponent(currentDateStr + ".json")
            UserSave.save(user: dataList, url: fileURL)

            databaseDirectories.refresh()
            directories.reloadData()
        }
    }
    
    func openFileDialog() -> String {
        let openPanel = NSOpenPanel()
        
//        openPanel.allowedContentTypes = [.text]
        
        let modalResponse = openPanel.runModal()
        
        if modalResponse == .OK {
            let url = openPanel.url
            do {
                let fileContents = try String(contentsOf: url!, encoding: .utf8)
                return fileContents
            } catch {
                let alert = NSAlert()
                alert.icon = NSImage(named: "NSCaution")
                alert.alertStyle = .warning
                alert.messageText = "ERROR"
                alert.informativeText = "ファイルの読み込みに失敗しました。エンコードを修正してください。"
                alert.addButton(withTitle: "OK")
                _ = alert.runModal()
                return ""
            }
        }
        
        return ""
    }
    
    public func reload(url: URL) {
        databaseUsers.removeAll()
        let users = DatabaseUser.load(url: url)
        for user in users {
            databaseUsers.append(user)
        }
        usersTable.reloadData()
    }
}

extension DatabaseViewController: NSOutlineViewDataSource, NSOutlineViewDelegate, NSTableViewDataSource, NSTableViewDelegate, NSMenuDelegate {
    public func menu(_ menu: NSMenu, willHighlight item: NSMenuItem?) {
//        print("aaa")
    }
    
    public func outlineView(
        _ outlineView: NSOutlineView,
        child index: Int,
        ofItem item: Any?
    ) -> Any {
        if (item == nil) {
            if (index == 0) {
                return self.databaseDirectories
            }
        }
        
        let directory = item as! DatabaseDirectory
        return directory.children[index]
    }
    
//    public func outlineView(
//        _ outlineView: NSOutlineView,
//        setObjectValue object: Any?,
//        for tableColumn: NSTableColumn?,
//        byItem item: Any?
//    ) {
//        print("aaaa")
//    }
    
    public func outlineView(
        _ outlineView: NSOutlineView,
        isItemExpandable item: Any
    ) -> Bool {
        let directory = item as! DatabaseDirectory
        return directory.childCount() > 0
    }
    
    // 子供の数
    public func outlineView(
        _ outlineView: NSOutlineView,
        numberOfChildrenOfItem item: Any?
    ) -> Int {
        if (item == nil) {
            return 1
        }
        let directory = item as! DatabaseDirectory
        return directory.childCount()
    }
    
//    public func outlineView(
//        _ outlineView: NSOutlineView,
//        typeSelectStringFor tableColumn: NSTableColumn?,
//        item: Any
//    ) -> String? {
//        return "aaa"
//    }
    
    // ツリーを開くかどうか？
    public func outlineView(
        _ outlineView: NSOutlineView,
        shouldExpandItem item: Any
    ) -> Bool {
        let directory = item as! DatabaseDirectory
        return directory.childCount() > 0
    }

    
    /// viewを返す
    /// - Parameters:
    ///   - outlineView:
    ///   - tableColumn:
    ///   - item:
    /// - Returns:
    public func outlineView(
        _ outlineView: NSOutlineView,
        viewFor tableColumn: NSTableColumn?,
        item: Any
    ) -> NSView? {
//        if item is String {
//            let buttonView = NSView()
//            return view
//        }
        let directory = item as! DatabaseDirectory
        var suffix = ""
        if (directory.isDirectory()) {
            suffix = "/"
        }
        let view = TreeView(labelWithString: directory.name + suffix);
        view.identifier = NSUserInterfaceItemIdentifier(rawValue: directory.name)
        return view
    }
    
    
    /// 選択変更
    /// - Parameter notification:
    public func outlineViewSelectionDidChange(_ notification: Notification) {
        guard let node = directories.item(atRow: directories.selectedRow) as? DatabaseDirectory else {
            return
        }
        
        let users = DatabaseUser.load(url: node.fullPath)
        
        if (directories.selectedRow == 0) {
            editButton.isEnabled = false
            deleteButton.isEnabled = false
        } else {
            editButton.isEnabled = true
            deleteButton.isEnabled = true
        }
        
        databaseUsers.removeAll()
        if (!node.isDirectory()) {
            for user in users {
                databaseUsers.append(user)
            }
            usersTable.reloadData()
        }
        if(users.count > 0) {
            memo.stringValue = users[0].memo
        }
        
        usersTable.selectRowIndexes(NSIndexSet(index: 0) as IndexSet, byExtendingSelection: false)
    }
    
    // 行数を返す
    public func numberOfRows(in tableView: NSTableView) -> Int
    {
        return databaseUsers.count
    }

    
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
                if (unwrapped == "name") {
                    view = NSTextField(labelWithString: databaseUsers[row].name)
                    view?.identifier = NSUserInterfaceItemIdentifier(rawValue: unwrapped + String(row))
                    
                } else if (unwrapped == "birthStr") {
                    view = NSTextField(labelWithString: (String)(databaseUsers[row].birthStr))
                    view?.identifier = NSUserInterfaceItemIdentifier(rawValue: unwrapped + String(row))
                } else if (unwrapped == "place") {
                    view = NSTextField(labelWithString: (String)(databaseUsers[row].place))
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

    
    /// 選択変更
    /// - Parameter notification:
    public func tableViewSelectionDidChange(_ notification: Notification)
    {
        let index = usersTable.selectedRow
        memo.stringValue = databaseUsers[index].memo
    }
    
    public func outlineView(
        _ outlineView: NSOutlineView,
        didDrag tableColumn: NSTableColumn
    ) {
        print("aa")
    }
    
//    public func outlineView(
//        _ outlineView: NSOutlineView,
//        acceptDrop info: any NSDraggingInfo,
//        item: Any?,
//        childIndex index: Int
//    ) -> Bool {
//        let directory = item as! DatabaseDirectory
//        if (directory.isDirectory()) {
//            return true
//        }
//
//        return false
//    }
//    
//    // drag中
//    public func outlineView(
//        _ outlineView: NSOutlineView,
//        draggingSession session: NSDraggingSession,
//        willBeginAt screenPoint: NSPoint,
//        forItems draggedItems: [Any]
//    ) {
//    }
//
//    // drag開始
//    public func outlineView(
//        _ outlineView: NSOutlineView,
//        pasteboardWriterForItem item: Any
//    ) -> (any NSPasteboardWriting)? {
//        let from = item as! DatabaseDirectory
//        let pbItem:NSPasteboardItem = NSPasteboardItem()
//        pbItem.setString(from.fullPath.path, forType: .URL)
//        
//        print("drag start")
//        return pbItem
//    }
//    
//    public func outlineView(
//        _ outlineView: NSOutlineView,
//        validateDrop info: any NSDraggingInfo,
//        proposedItem item: Any?,
//        proposedChildIndex index: Int
//    ) -> NSDragOperation {
//        print("drop")
//        return .move
//    }
//    
//    public func outlineView(
//        _ outlineView: NSOutlineView,
//        updateDraggingItemsForDrag draggingInfo: any NSDraggingInfo
//    ) {
//        print("bbbb")
//    }
//    
//    public func outlineView(
//        _ outlineView: NSOutlineView,
//        draggingSession session: NSDraggingSession,
//        endedAt screenPoint: NSPoint,
//        operation: NSDragOperation
//    ) {
//        print("xxxx")
//    }
}
