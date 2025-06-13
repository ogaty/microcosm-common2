//
//  TreeOutlineView.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/03/27.
//

import Cocoa

class TreeOutlineView: NSOutlineView {
    
    override func draw(_ dirtyRect: NSRect) {
        super.draw(dirtyRect)
        
        // Drawing code here.
    }
    
    override func menu(for event: NSEvent) -> NSMenu? {
        let menu = NSMenu(title: "menu")
        menu.delegate = self
        return menu
    }
    
    
    /// 実装コストがかかるので一旦いいや
    /// - Parameter theEvent:
    override func rightMouseUp(with theEvent : NSEvent) {
        guard let node = self.item(atRow: self.selectedRow) as? DatabaseDirectory else {
            return
        }
        
//        print(node.fullPath.path)
        
        if (node.isDirectory()) {
            let menu0 = NSMenu(title: "menu")
            let menuItem = NSMenuItem(title: "ユーザー追加",
                                      action: #selector(self.handleMenuItemSelected),
                                      keyEquivalent: "")
            menu0.addItem(menuItem)
            let menuItem1 = NSMenuItem(title: "名前変更",
                                       action: #selector(self.editDirectoryName),
                                       keyEquivalent: "")
            menu0.addItem(menuItem1)
            
            let menuItem2 = NSMenuItem(title: "ディレクトリ追加",
                                       action: #selector(self.addDirectory(_:)),
                                       keyEquivalent: "")
            menu0.addItem(menuItem2)
            
            let menuItem3 = NSMenuItem(title: "ディレクトリ削除",
                                       action: #selector(self.addDirectory(_:)),
                                       keyEquivalent: "")
            menu0.addItem(menuItem3)
            
            guard let event = NSApp.currentEvent else { return }
            NSMenu.popUpContextMenu(menu0, with: event, for: self)
        } else {
            let menu0 = NSMenu(title: "menu")
            let menuItem = NSMenuItem(title: "ユーザー編集",
                                      action: #selector(self.handleMenuItemSelected),
                                      keyEquivalent: "")
            menu0.addItem(menuItem)

            let menuItem1 = NSMenuItem(title: "ユーザー削除",
                                      action: #selector(self.handleMenuItemSelected),
                                      keyEquivalent: "")
            menu0.addItem(menuItem1)

            guard let event = NSApp.currentEvent else { return }
            NSMenu.popUpContextMenu(menu0, with: event, for: self)
        }
        
        //
        //        print("right mouse")
    }
    
    @objc
    func handleMenuItemSelected(_ sender: AnyObject) {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        let vc = delegate.databaseViewController!.storyboard?.instantiateController(withIdentifier: "user") as! NSViewController
        let myWindow:NSWindow? = NSWindow(contentViewController: vc)
        myWindow?.makeKeyAndOrderFront(self)
        let wc = NSWindowController(window: myWindow)
        wc.showWindow(self)
    }
    
    @objc
    func editDirectoryName(_ sender: AnyObject) {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        let vc = delegate.databaseViewController!.storyboard?.instantiateController(withIdentifier: "user") as! NSViewController
        let myWindow:NSWindow? = NSWindow(contentViewController: vc)
        myWindow?.makeKeyAndOrderFront(self)
        let wc = NSWindowController(window: myWindow)
        wc.showWindow(self)
    }
    
    @objc
    func addDirectory(_ sender: AnyObject) {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        let vc = delegate.databaseViewController!.storyboard?.instantiateController(withIdentifier: "user") as! NSViewController
        let myWindow:NSWindow? = NSWindow(contentViewController: vc)
        myWindow?.makeKeyAndOrderFront(self)
        let wc = NSWindowController(window: myWindow)
        wc.showWindow(self)
    }
}

extension TreeOutlineView: NSMenuDelegate {
    public func menu(_ menu: NSMenu, willHighlight item: NSMenuItem?) {
        print("aaa")
    }
}
