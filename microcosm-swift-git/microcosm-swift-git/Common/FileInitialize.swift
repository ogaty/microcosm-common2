//
//  FileInitialize.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/02/05.
//

import Foundation

// ファイル初期化クラス
public class FileInitialize
{
    // microcosmディレクトリのURLを返却、無ければ作る
    public static func microcosmDirectory()-> URL
    {
        // /Documents/microcosmのほうが良かったかも？
        // 旧バージョンとの整合性の都合でこのままにする
        let directory = NSHomeDirectory() + "/microcosm"
        let u = URL(fileURLWithPath: directory)
//        let filePath = pathComponent.path
        let fileManager = FileManager.default
        if fileManager.fileExists(atPath: directory) {
            print("Directory AVAILABLE")
        } else {
            print("Directory NOT AVAILABLE")
            do {
                try fileManager.createDirectory(atPath: directory, withIntermediateDirectories: true, attributes: nil)
            } catch {
                print("ERROR")
            }
        }
        
        return u
    }
    
    // 各種csvやjsonを作る
    public static func fileInit(microcosmURL: URL) -> Void {
        do {
            let fileManager = FileManager.default
            let systemURL = microcosmURL.appendingPathComponent("system")

            if !fileManager.fileExists(atPath: systemURL.path) {
                do {
                    try fileManager.createDirectory(atPath: systemURL.path, withIntermediateDirectories: true, attributes: nil)
                } catch {
                    print("ERROR: cannot create directory")
                }
            }
            
            let csvURL = Bundle.main.url(forResource: "addr", withExtension: "csv")
            let addrFile = systemURL
                .appendingPathComponent("/addr.csv")
            if !fileManager.fileExists(atPath: addrFile.path) {
                try fileManager.copyItem(at: csvURL!, to: addrFile)
            }

            let sabianURL = Bundle.main.url(forResource: "sabian", withExtension: "csv")
            let sabianFile = systemURL
                .appendingPathComponent("/sabian.csv")
            if !fileManager.fileExists(atPath: sabianFile.path) {
                try fileManager.copyItem(at: sabianURL!, to: sabianFile)
            }

            let configURL = Bundle.main.url(forResource: "config", withExtension: "json")
            let configFile = systemURL
                .appendingPathComponent("config.json")
            if !fileManager.fileExists(atPath: configFile.path) {
                try fileManager.copyItem(at: configURL!, to: configFile)
            }
            
            for i in 0...9 {
                let settingURL = Bundle.main.url(forResource: "settings" + String(i), withExtension: "json")
                let settingFile = systemURL
                    .appendingPathComponent("settings" + String(i) + ".json")
                if !fileManager.fileExists(atPath: settingFile.path) {
                    try fileManager.copyItem(at: settingURL!, to: settingFile)
                }
            }

            let dataURL = microcosmURL.appendingPathComponent("data")

            if !fileManager.fileExists(atPath: dataURL.path) {
                do {
                    try fileManager.createDirectory(atPath: dataURL.path, withIntermediateDirectories: true, attributes: nil)
                } catch {
                    print("ERROR: cannot create directory")
                }
            }

            print("file init done")
        } catch {
            print("ERROR: cannot copy")
            print(error.localizedDescription)
        }
    }
}

