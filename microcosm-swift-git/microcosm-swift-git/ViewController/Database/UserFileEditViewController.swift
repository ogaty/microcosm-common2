//
//  UserFileEditViewController.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/03/29.
//

import Cocoa

class UserFileEditViewController: NSViewController {

    @IBOutlet weak var fileName: NSTextField!
    var basePath: URL? = URL(string: "")
    var oldPath: URL? = URL(string: "")
    public var parentController: DatabaseViewController?
    
    override func viewDidLoad() {
        super.viewDidLoad()
        // Do view setup here.
    }
    
    public func setBasePath(basePath: URL) {
        self.basePath = basePath
    }
    
    public func setOldPath(oldPath: URL) {
        self.oldPath = oldPath
    }
    
    public func setFileName(name: String) {
        if let dotIndex = name.lastIndex(of: ".") {
            let nameWithoutExtension = String(name[..<dotIndex])
            fileName.stringValue = nameWithoutExtension
        }
    }
    
    @IBAction func SubmitClicked(_ sender: Any) {
        let newPath = (self.basePath?.appendingPathComponent(fileName.stringValue + ".json"))!
        let fileManager = FileManager.default
        if (oldPath != nil) {
            do {
                try fileManager.moveItem(at: oldPath!, to: newPath)
            } catch {
                print("ERROR: cannot move")
            }
        } else {
            
        }
        
        parentController?.databaseDirectories.refresh()
        parentController?.directories.reloadData()

        self.view.window?.close()
    }
}
