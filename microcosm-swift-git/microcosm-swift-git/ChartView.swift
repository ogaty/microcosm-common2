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
        
        let rett = calc.CuspCalc(d: myDate, timezone: 9, lat: Double(config.defaultLat)!, lng: Double(config.defaultLng)!, houseKind: EHouse.PLACIDUS)
        

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
            let point = Util.Rotate(x: Double(innerRadius), y: 0, degree: Double(rett[i] - rett[1]))
            let outerNewPoint = outerPosition(point: point, radius: radius)
            path2.move(to: CGPoint(x: outerNewPoint.x, y: outerNewPoint.y))
            
            let innerPoint = Util.Rotate(x: Double(centerDiameter / 2), y: 0, degree: Double(rett[i] - rett[1]))
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

        for i in 0..<12 {
            var degree: Double = Double(i * 30)
            var offset = 15 - rett[1]
            degree = degree + offset
            let str = NSAttributedString(string: CommonData.getSignSymbol(n: i), attributes: [NSAttributedString.Key.font: NSFont(name: "microcosm", size: 24.0)!])
            let pos = Util.Rotate(x: Double(diameter / 2 - 20), y: 0, degree: degree)
            let tmp = Double(diameter / 2) - 10.0
            let tmp2 = Double(diameter / 2) - 10.0
            str.draw(at: NSPoint(x: Double(pos.x) + tmp + Double(delegate.chartStyle.margin), y: Double(pos.y) + tmp2 + Double(delegate.chartStyle.margin)))
        }
        
        delegate.list1.keys.forEach{ key in
            if (!delegate.list1[key]!.isDisp) {
                return;
            }
            let str = NSAttributedString(string: CommonData.getPlanetSymbol2(n: key), attributes: [NSAttributedString.Key.font: NSFont(name: "microcosm", size: 24.0)!])
            let pos = Util.Rotate(x: Double(diameter) * 0.35, y: 0, degree: delegate.list1[key]!.absolute_position - rett[1])
            let tmp = Double(diameter) * 0.5 - 10
            str.draw(at: NSPoint(x: Double(pos.x) + tmp + Double(delegate.chartStyle.margin), y: Double(pos.y) + tmp + Double(delegate.chartStyle.margin)))
        }

        
        
    
        //NSBitmapImageRep(cgImage: context.makeImage())
        
//        let i = context.makeImage()
        
        context.restoreGState()
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
