//
//  ChartRingMaker.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/02/06.
//

import Foundation

import CoreGraphics
import AppKit

// Ring名称
// 一番外側: OuterRing
// その次: InnerRing
// 中心: CenterRing
// 二重円: Ring2
// 三重円: Ring3
// 四重円: Ring4
public class ChartRingMaker {
    static func MakeChart(cgContext: CGContext, fill: Bool, width: Int) -> CGContext? {
        MakeOuterRing(cgContext: cgContext, fill: fill, width: 150, height: 150)
        MakeInnerRing(cgContext: cgContext, width: 50, height: 50)
        MakeCenterRing(cgContext: cgContext)

        return cgContext
    }
    
    static func MakeOuterRing(cgContext: CGContext, fill: Bool, width: Int, height: Int) {
        cgContext.setFillColor(NSColor.white.cgColor)
        cgContext.fill(CGRect(x: 0, y: 0, width: width, height: width))
        cgContext.setStrokeColor(NSColor.red.cgColor)
        cgContext.strokeEllipse(in: CGRect(x: 0, y: 0, width: width, height: width))
        
//        guard let z = cgContext.makeImage() else {
//            print("bbbb")
//            return nil
//        }
    }
    static func MakeInnerRing(cgContext: CGContext, width: Int, height: Int) {
        cgContext.setStrokeColor(NSColor.green.cgColor)
        cgContext.strokeEllipse(in: CGRect(x: 0, y: 0, width: width, height: width))
    }
    static func MakeCenterRing(cgContext: CGContext) {
        
    }
}
