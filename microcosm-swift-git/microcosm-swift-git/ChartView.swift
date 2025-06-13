//
//  Chart.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/02/05.
//

import Cocoa
import swissEphemeris

class ChartView: NSView {
    override func draw(_ dirtyRect: NSRect) {
        super.draw(dirtyRect)

        let delegate = NSApplication.shared.delegate as! AppDelegate

        let context = NSGraphicsContext.current!.cgContext
        context.saveGState()
        
        let diameter: Int = Int(dirtyRect.width) - delegate.chartStyle.margin * 2
        let radius: Int = Int(diameter / 2)
        let innerDiameter: Int = Int(dirtyRect.width) - (delegate.chartStyle.margin * 2) - (delegate.chartStyle.innerRingWidth * 2)
        let innerRadius: Int = Int(innerDiameter / 2)
        let centerDiameter = innerDiameter * delegate.chartStyle.centerRingRatio / 100
        
        context.setFillColor(NSColor.white.cgColor)
        context.fill(CGRect(x: 0, y: 0, width: dirtyRect.width, height: dirtyRect.height))

        // outer ring
        context.setStrokeColor(NSColor.blue.cgColor)
        context.strokeEllipse(in: NSRect(x: delegate.chartStyle.margin, y: delegate.chartStyle.margin, width: diameter, height: diameter))
        
        // inner ring
        context.setStrokeColor(NSColor.blue.cgColor)
        context.strokeEllipse(in: NSRect(x: Int(CGFloat(delegate.chartStyle.margin + delegate.chartStyle.innerRingWidth)), y: Int(CGFloat(delegate.chartStyle.margin + delegate.chartStyle.innerRingWidth)), width: innerDiameter, height: innerDiameter))

        // center ring
        context.setStrokeColor(NSColor.orange.cgColor)
        let c = getCenterRect(rect: dirtyRect, diameter: centerDiameter)
        context.strokeEllipse(in: c)

        
        let config = delegate.config
        let swiss = delegate.swiss
        let calc = AstroCalc(config: config, swiss: swiss)
        let myDate = MyDate()
        
        var houseKind = EHouse.PLACIDUS
        if (delegate.currentSetting.houseCalc == EHouse.PLACIDUS.rawValue) {
            houseKind = EHouse.PLACIDUS
        } else if (delegate.currentSetting.houseCalc == EHouse.KOCH.rawValue) {
            houseKind = EHouse.KOCH
        } else if (delegate.currentSetting.houseCalc == EHouse.CAMPANUS.rawValue) {
            houseKind = EHouse.CAMPANUS
        } else if (delegate.currentSetting.houseCalc == EHouse.EQUAL.rawValue) {
            houseKind = EHouse.EQUAL
        } else if (delegate.currentSetting.houseCalc == EHouse.PORPHYRY.rawValue) {
            houseKind = EHouse.PORPHYRY
        } else if (delegate.currentSetting.houseCalc == EHouse.REGIOMONTANUS.rawValue) {
            houseKind = EHouse.REGIOMONTANUS
        } else if (delegate.currentSetting.houseCalc == EHouse.SOLAR.rawValue) {
            houseKind = EHouse.SOLAR
        } else if (delegate.currentSetting.houseCalc == EHouse.SOLARSIGN.rawValue) {
            houseKind = EHouse.SOLARSIGN
        } else if (delegate.currentSetting.houseCalc == EHouse.ZEROARIES.rawValue) {
            houseKind = EHouse.ZEROARIES
        }
        
        let rett = calc.CuspCalc(d: myDate, timezone: 9, lat: Double(config.defaultLat)!, lng: Double(config.defaultLng)!, houseKind: houseKind)
        

        // zodiac lines
        let path = CGMutablePath()
        context.setStrokeColor(NSColor.black.cgColor)
        context.setLineWidth(1)
        
        for i in 0..<12 {
            var degree: Double = Double(i * 30)
            degree = degree - rett[1]
            let point = Util.Rotate(x: Double(radius), y: 0, degree: degree)
            let outerNewPoint = outerPosition(point: point, radius: radius)
            path.move(to: CGPoint(x: outerNewPoint.x, y: outerNewPoint.y))
            
            let innerPoint = Util.Rotate(x: Double(innerRadius), y: 0, degree: degree)
            let innerNewPoint = innerPosition(point: innerPoint, radius: innerRadius)
            path.addLine(to: CGPoint(x: innerNewPoint.x, y: innerNewPoint.y))
        }
        context.addPath(path)
        context.strokePath()

        // house cusps
        let path2 = CGMutablePath()
        for i in 1..<13 {
            var point = Position(x: 0, y: 0)
            if (delegate.currentSetting.houseCalc == EHouse.ZEROARIES.rawValue) {
                point = Util.Rotate(x: Double(innerRadius), y: 0, degree: Double(i * 30))
            } else {
                point = Util.Rotate(x: Double(innerRadius), y: 0, degree: Double(rett[i] - rett[1]))
            }
            let outerNewPoint = outerPosition(point: point, radius: radius)
            path2.move(to: CGPoint(x: outerNewPoint.x, y: outerNewPoint.y))
            
            var innerPoint = Position(x: 0, y: 0)
            if (delegate.currentSetting.houseCalc == EHouse.ZEROARIES.rawValue) {
                innerPoint = Util.Rotate(x: Double(centerDiameter / 2), y: 0, degree: Double(i * 30))
            } else {
                innerPoint = Util.Rotate(x: Double(centerDiameter / 2), y: 0, degree: Double(rett[i] - rett[1]))
            }

            let innerNewPoint = centerPosition(point: innerPoint, outerRadius: diameter / 2, centerRadius: centerDiameter / 2)
            path2.addLine(to: CGPoint(x: innerNewPoint.x, y: innerNewPoint.y))
        }
        context.setStrokeColor(NSColor.lightGray.cgColor)
        context.setLineDash(phase: 0.3, lengths: [CGFloat(2.8)])
        context.addPath(path2)
        context.strokePath()
        
        let path3 = CGMutablePath()
        let point31 = CGPoint(x: delegate.chartStyle.margin + diameter / 2, y:0)
        path3.move(to: point31)
        let point32 = CGPoint(x: delegate.chartStyle.margin + diameter / 2, y:delegate.chartStyle.margin + diameter)
        path3.addLine(to: point32)
        context.setStrokeColor(NSColor.lightGray.cgColor)
        context.addPath(path3)
        context.strokePath()

        let path4 = CGMutablePath()
        let point41 = CGPoint(x: 0, y:delegate.chartStyle.margin + diameter / 2)
        path4.move(to: point41)
        let point42 = CGPoint(x: delegate.chartStyle.margin + diameter, y: delegate.chartStyle.margin + diameter / 2)
        path4.addLine(to: point42)
        context.setStrokeColor(NSColor.lightGray.cgColor)
        context.addPath(path4)
        context.strokePath()

        // 獣帯に入るサインシンボル
        for i in 0..<12 {
            var degree: Double = Double(i * 30)
            let offset = 15 - rett[1]
            degree = degree + offset
            let str = NSAttributedString(string: CommonData.getSignSymbol(n: i), attributes: [NSAttributedString.Key.font: NSFont(name: "microcosm", size: 24.0)!])
            let pos = Util.Rotate(x: Double(diameter / 2 - 20), y: 0, degree: degree)
            let tmp = Double(diameter / 2) - 10.0
            let tmp2 = Double(diameter / 2) - 10.0
            str.draw(at: NSPoint(x: Double(pos.x) + tmp + Double(delegate.chartStyle.margin), y: Double(pos.y) + tmp2 + Double(delegate.chartStyle.margin)))
        }
        
        // 実際の天体
        let b = Box()
        b.boxInit();
        // 0.5は半径を求めている
        // 10はマージン
        let radius2 = Double(diameter) * 0.5 - 10
        delegate.list1.keys.forEach{ key in
            if (!delegate.list1[key]!.isDisp) {
                return;
            }
            print("absolute_position:")
            print(delegate.list1[key]!.absolute_position)
            let index = (Int)(delegate.list1[key]!.absolute_position / 5);
            b.boxUpdate(initIndex: index);
            
            print("index:")
            print(index * 5)

            let xMargin = Double(delegate.chartStyle.margin)
            let yMargin = Double(delegate.chartStyle.margin)
            let str = NSAttributedString(string: CommonData.getPlanetSymbol2(n: key), attributes: [NSAttributedString.Key.font: NSFont(name: "microcosm", size: 24.0)!])
            let pos = Util.Rotate(x: Double(diameter) * 0.38, y: 0, degree: Double(b.index * 5) - rett[1])
            str.draw(at: NSPoint(
                x: Double(pos.x + radius2 + xMargin),
                y: Double(pos.y) + radius2 + yMargin
            ))
            
            let IntDegree = (Int)(delegate.list1[key]!.absolute_position.truncatingRemainder(dividingBy: 30))
            let strDegree = NSAttributedString(string: (String)(format: "%02d", IntDegree))
            let posDegree = Util.Rotate(x: Double(diameter) * 0.34, y: 0, degree: Double(b.index * 5) - rett[1])
            // yはシンボルのフォントサイズの都合若干下になるので+5する
            strDegree.draw(at: NSPoint(
                x: Double(posDegree.x + radius2 + 2.0 + xMargin),
                y: Double(posDegree.y + radius2 + 5.0 + yMargin)
            ))

            let strSymbol = NSAttributedString(string: CommonData.getSignSymbol(n: (Int)(delegate.list1[key]!.absolute_position / 30)), attributes: [NSAttributedString.Key.font: NSFont(name: "microcosm", size: 24.0)!])

            let posSymbol = Util.Rotate(x: Double(diameter) * 0.30, y: 0, degree: Double(b.index * 5) - rett[1])
            // yはシンボルのフォントサイズの都合若干下になるので+2する
            strSymbol.draw(at: NSPoint(
                x: Double(posSymbol.x + radius2 + 2 + xMargin),
                y: Double(posSymbol.y + radius2 + 2 + yMargin)
            ))

            let subDegree = getSubDegree(value: delegate.list1[key]!.absolute_position)
            let strSubDegree = NSAttributedString(string: (String)(format: "%02d", subDegree))
            let posSubDegree = Util.Rotate(x: Double(diameter) * 0.25, y: 0, degree: Double(b.index * 5) - rett[1])
            // yはシンボルのフォントサイズの都合若干下になるので+5する
            strSubDegree.draw(at: NSPoint(
                x: Double(posSubDegree.x + radius2 + 2 + xMargin),
                y: Double(posSubDegree.y + radius2 + 5 + yMargin)
            ))

        }
        
    
        //NSBitmapImageRep(cgImage: context.makeImage())
        
//        let i = context.makeImage()
        
        context.restoreGState()
    }
    
    func getSubDegree(value: Double) -> Int {
        let tmp = value.truncatingRemainder(dividingBy: 1) * 100
        let tmp2 = CommonData.DecimalToHex(deci: tmp)
        return (Int)(tmp2)
    }
    
    func outerPosition(point: Position, radius: Int) -> Position {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        let x = point.x + Double(radius + delegate.chartStyle.margin)
        let y = point.y + Double(radius + delegate.chartStyle.margin)
        
        let newPosition = Position(x: x, y: y)
        return newPosition
    }

    func innerPosition(point: Position, radius: Int) -> Position {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        let x = point.x + Double(radius + delegate.chartStyle.margin + delegate.chartStyle.innerRingWidth)
        let y = point.y + Double(radius + delegate.chartStyle.margin + delegate.chartStyle.innerRingWidth)
        
        let newPosition = Position(x: x, y: y)
        return newPosition
    }
    
    // outerの半径
    func centerPosition(point: Position, outerRadius: Int, centerRadius: Int) -> Position {
        let delegate = NSApplication.shared.delegate as! AppDelegate
        let x = point.x + Double(outerRadius + delegate.chartStyle.margin)
        let y = point.y + Double(outerRadius + delegate.chartStyle.margin)
        
        let newPosition = Position(x: x, y: y)
        return newPosition
    }
    
    func getCenterRect(rect: NSRect, diameter: Int) -> NSRect {
        let centerX = (Int(rect.width) / 2) - (diameter / 2)
        let centerY = (Int(rect.height) / 2) - (diameter / 2)
        let r = NSRect(x: centerX, y: centerY, width: diameter, height: diameter)
        
        return r
    }


}

extension NSView {
    func makeImage() -> Void {
        guard let rep = bitmapImageRepForCachingDisplay(in: bounds) else { return }
        cacheDisplay(in: bounds, to: rep)
        guard let pngData = rep.representation(using: .png, properties: [:]) else { return }

        let savePanel = NSSavePanel()
        savePanel.canCreateDirectories = true
        savePanel.showsTagField = false
        savePanel.nameFieldStringValue = "horoscope.png"
        savePanel.level = .modalPanel
        
        savePanel.begin {
            if $0 == .OK {
                do {
                    try pngData.write(to: savePanel.url!)
                } catch {
                    print(error)
                }
            }
        }
    }
}
