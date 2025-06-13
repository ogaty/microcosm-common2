//
//  DirectoryEditViewController.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/03/29.
//

import Cocoa

class DirectoryEditViewController: NSViewController {

    @IBOutlet weak var directoryName: NSTextField!
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
    
    public func setDirectoryname(name: String) {
        directoryName.stringValue = name
    }
    
    @IBAction func SubmitClicked(_ sender: Any) {
        let newPath = (self.basePath?.appendingPathComponent(directoryName.stringValue))!
        let fileManager = FileManager.default
        if (oldPath != nil) {
            do {
                try fileManager.moveItem(at: oldPath!, to: newPath)
            } catch {
                print("ERROR: cannot move")
            }
        } else {
            do {
                try fileManager.createDirectory(at: newPath, withIntermediateDirectories: true, attributes: nil)
            } catch {
                print("ERROR: cannot create directory")
            }
        }
        
        parentController?.databaseDirectories.refresh()
        parentController?.directories.reloadData()


        self.view.window?.close()
    }
}
