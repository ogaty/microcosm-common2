//
//  UrlUtil.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/05/13.
//

import Foundation

public class UrlUtil {
    public static func isDirectory(url: URL) -> Bool {
        return url.hasDirectoryPath
    }
    
    public static func getFileName(url: URL) -> String {
        if (url.hasDirectoryPath) {
            return ""
        }
        
        return url.lastPathComponent
    }
}
