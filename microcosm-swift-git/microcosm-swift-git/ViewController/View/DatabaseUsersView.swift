//
//  databaseUsersView.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/05/13.
//

import Cocoa

class DatabaseUsersView: NSTableView {
    
    public var parentViewController: DatabaseViewController?

    override func draw(_ dirtyRect: NSRect) {
        super.draw(dirtyRect)

        // Drawing code here.
    }
    
    override func rightMouseUp(with event: NSEvent) {
        let menu0 = NSMenu(title: "menu")
        let menuItem = NSMenuItem(title: "イベント追加",
                                  action: #selector(self.addEvent),
                                  keyEquivalent: "")
        menu0.addItem(menuItem)
        let menuItem1 = NSMenuItem(title: "イベント変更",
                                   action: #selector(self.editEvent),
                                   keyEquivalent: "")
        menu0.addItem(menuItem1)
        
        let menuItem2 = NSMenuItem(title: "イベント削除",
                                   action: #selector(self.deleteEvent(_:)),
                                   keyEquivalent: "")
        menu0.addItem(menuItem2)
        
        guard let event = NSApp.currentEvent else { return }
        NSMenu.popUpContextMenu(menu0, with: event, for: self)
    }
    
    @objc
    func addEvent(_ sender: AnyObject) {
        parentViewController?.addEvent()
    }

    @objc
    func editEvent(_ sender: AnyObject) {
        parentViewController?.editEvent()
    }

    @objc
    func deleteEvent(_ sender: AnyObject) {
        parentViewController?.removeEvent()
    }

}
