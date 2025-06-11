//
//  DatabaseDirectory.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/03/25.
//

import Foundation

class DatabaseDirectory: NSObject {
    public var fileURLs = [URL]()

    var name: String
    var fullPath: URL
    var children: [DatabaseDirectory]
    
    init(name: String, fullPath: URL, children: [DatabaseDirectory] = []) {
        self.name = name
        self.fullPath = fullPath
        self.children = children
    }
    
    func isDirectory() -> Bool {
        return fullPath.hasDirectoryPath
    }
    
    func isLeaf() -> Bool {
        return fullPath.hasDirectoryPath
    }
    
    func getBasePath() -> URL? {
        return fullPath.baseURL
    }
    
    func getFileNameWithoutExtension() -> String {
        return NSString(string: fullPath.lastPathComponent).deletingPathExtension
    }
    
    func childCount() -> Int {
        fileURLs.removeAll()
        children.removeAll()
        appendURL(at: fullPath)
        return children.count
    }
    
    // ディレクトリツリーをリフレッシュ
    func refresh() {
        appendURL(at: FileInitialize.microcosmDirectory().appendingPathComponent("data"))
        fileURLs.sort { $0.path < $1.path }
        //print(fileURLs)
    }
    
    func appendURL(at: URL) {
        if (!at.hasDirectoryPath) {
            return
        }
        do {
            let tempDirContentsUrls = try FileManager.default.contentsOfDirectory(at: at, includingPropertiesForKeys: nil).sorted { $0.path < $1.path }

            tempDirContentsUrls.forEach {
                url in
                fileURLs.append(url)
                children.append(DatabaseDirectory(name: url.lastPathComponent, fullPath: url))
//                appendURL(at: url)
            }
        } catch {
            print(error.localizedDescription)
        }
    }
}
