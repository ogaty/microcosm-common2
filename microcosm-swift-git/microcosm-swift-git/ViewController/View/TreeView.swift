//
//  TreeView.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/05/10.
//

import Cocoa

class TreeView: NSTextField {
    override func draw(_ dirtyRect: NSRect) {
        super.draw(dirtyRect)

        // Drawing code here.
        
    }
    
    override func mouseUp(with event: NSEvent) {
        print("left up")
        super.mouseUp(with: event)
    }
    
    override func rightMouseUp(with event: NSEvent) {
        print("right up")
        super.rightMouseUp(with: event)
    }

    override func draggingEnded(_ sender: NSDraggingInfo) {
        print("a")
//        guard let pathAlias = sender.draggingPasteboard.propertyList(forType: .fileURL) as? String else {
//            return
//        }
//
//        let url = URL(fileURLWithPath: pathAlias).standardized
//        let fileInfo = FileInfo(url: url)
//        didDrag?(fileInfo)
    }
}
