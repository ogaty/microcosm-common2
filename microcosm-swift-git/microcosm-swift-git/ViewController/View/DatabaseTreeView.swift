//
//  DatabaseTreeView.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/05/10.
//

import Cocoa

class DatabaseTreeView: NSOutlineView {

    override func draw(_ dirtyRect: NSRect) {
        super.draw(dirtyRect)

        // Drawing code here.
    }
    
    override func menu(for event: NSEvent) -> NSMenu? {
        return NSMenu(title: "menu")
    }
    
}
