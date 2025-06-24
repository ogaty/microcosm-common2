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
        let centerRadius = Int((centerDiameter / 2))
        
        context.setFillColor(NSColor.white.cgColor)
        context.fill(CGRect(x: 0, y: 0, width: dirtyRect.width, height: dirtyRect.height))

        // outer ring
        context.setStrokeColor(NSColor.black.cgColor)
        context.strokeEllipse(in: NSRect(x: delegate.chartStyle.margin, y: delegate.chartStyle.margin, width: diameter, height: diameter))
        
        // inner ring
        context.setStrokeColor(NSColor.black.cgColor)
        context.strokeEllipse(in: NSRect(x: Int(CGFloat(delegate.chartStyle.margin + delegate.chartStyle.innerRingWidth)), y: Int(CGFloat(delegate.chartStyle.margin + delegate.chartStyle.innerRingWidth)), width: innerDiameter, height: innerDiameter))

        // center ring
        context.setStrokeColor(NSColor.black.cgColor)
        let c = getCenterRect(rect: dirtyRect, diameter: centerDiameter)
        context.strokeEllipse(in: c)

        if (delegate.bands == 2) {
            context.setStrokeColor(NSColor.gray.cgColor)
            let c2 = getCenterRect(rect: dirtyRect, diameter: (centerDiameter + innerDiameter) / 2)
            context.strokeEllipse(in: c2)
        }

        if (delegate.bands == 3) {
            context.setStrokeColor(NSColor.gray.cgColor)
            let c2 = getCenterRect(rect: dirtyRect, diameter: (centerDiameter + (innerDiameter - centerDiameter) * 2 / 3))
            context.strokeEllipse(in: c2)
            let c3 = getCenterRect(rect: dirtyRect, diameter: (centerDiameter + (innerDiameter - centerDiameter) * 1 / 3))
            context.strokeEllipse(in: c3)
        }

        let config = delegate.config
        let swiss = delegate.swiss
        let calc = AstroCalc(config: config, swiss: swiss)
        let ring1 = delegate.viewController!.GetTargetUser(ringIndex: 0)
        let ring2 = delegate.viewController!.GetTargetUser(ringIndex: 1)
        let ring3 = delegate.viewController!.GetTargetUser(ringIndex: 2)
        
        let d1 = MyDate()
        d1.setUserData(u: ring1)
        let d2 = MyDate()
        d2.setUserData(u: ring2)
        let d3 = MyDate()
        d3.setUserData(u: ring3)

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
        
        let rett = calc.CuspCalc(d: d1, timezone: 9, lat: Double(config.defaultLat)!, lng: Double(config.defaultLng)!, houseKind: houseKind)
        

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
            if (i == 1 || i == 4 || i == 7 || i == 10) {
                continue
            }
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
        context.setLineDash(phase: 0.3, lengths: [CGFloat(3.8)])
        context.addPath(path2)
        context.strokePath()
        let path3 = CGMutablePath()
        for i in 1..<13 {
            if (i == 1 || i == 4 || i == 7 || i == 10) {
                var point = Position(x: 0, y: 0)
                if (delegate.currentSetting.houseCalc == EHouse.ZEROARIES.rawValue) {
                    point = Util.Rotate(x: Double(innerRadius), y: 0, degree: Double(i * 30))
                } else {
                    point = Util.Rotate(x: Double(innerRadius), y: 0, degree: Double(rett[i] - rett[1]))
                }
                let outerNewPoint = outerPosition(point: point, radius: radius)
                path3.move(to: CGPoint(x: outerNewPoint.x, y: outerNewPoint.y))
                
                var innerPoint = Position(x: 0, y: 0)
                if (delegate.currentSetting.houseCalc == EHouse.ZEROARIES.rawValue) {
                    innerPoint = Util.Rotate(x: Double(centerDiameter / 2), y: 0, degree: Double(i * 30))
                } else {
                    innerPoint = Util.Rotate(x: Double(centerDiameter / 2), y: 0, degree: Double(rett[i] - rett[1]))
                }

                let innerNewPoint = centerPosition(point: innerPoint, outerRadius: diameter / 2, centerRadius: centerDiameter / 2)
                path3.addLine(to: CGPoint(x: innerNewPoint.x, y: innerNewPoint.y))
            }
        }

        context.setLineWidth(3)
        context.setStrokeColor(NSColor.lightGray.cgColor)
        context.setLineDash(phase: 0.3, lengths: [CGFloat(2.8)])
        context.addPath(path3)
        context.strokePath()

        context.setLineWidth(1)
        for i in 1..<13 {
            
            let d = (Int)(rett[i].truncatingRemainder(dividingBy: 30))
            let strSymbol = NSAttributedString(
                string: (String(format: "%02d", d)),
                attributes: [
                    NSAttributedString.Key.foregroundColor: NSColor.lightGray
                ]
            )

            let posSymbol = Util.Rotate(x: Double(innerDiameter) * 1.04 / 2, y: 0, degree: Double(rett[i] - rett[1]))
            // yはシンボルのフォントサイズの都合若干下になるので+2する
            strSymbol.draw(at: NSPoint(
                x: Double(posSymbol.x + Double(diameter / 2) + 10 + 1),
                y: Double(posSymbol.y + Double(diameter / 2) + 10 + 2)
            ))


        }
        
//        let path3 = CGMutablePath()
//        let point31 = CGPoint(x: delegate.chartStyle.margin + diameter / 2, y:0)
//        path3.move(to: point31)
//        let point32 = CGPoint(x: delegate.chartStyle.margin + diameter / 2, y:delegate.chartStyle.margin + diameter)
//        path3.addLine(to: point32)
//        context.setStrokeColor(NSColor.lightGray.cgColor)
//        context.addPath(path3)
//        context.strokePath()
//
//        let path4 = CGMutablePath()
//        let point41 = CGPoint(x: 0, y:delegate.chartStyle.margin + diameter / 2)
//        path4.move(to: point41)
//        let point42 = CGPoint(x: delegate.chartStyle.margin + diameter, y: delegate.chartStyle.margin + diameter / 2)
//        path4.addLine(to: point42)
//        context.setStrokeColor(NSColor.lightGray.cgColor)
//        context.addPath(path4)
//        context.strokePath()

        // 獣帯に入るサインシンボル
        for i in 0..<12 {
            var degree: Double = Double(i * 30)
            let offset = 15 - rett[1]
            degree = degree + offset
            let str = NSAttributedString(
                string: CommonData.getSignSymbol(n: i),
                attributes: [
                    NSAttributedString.Key.font: NSFont(name: "microcosm", size: 24.0)!,
                    NSAttributedString.Key.foregroundColor: CommonData.getSignColor(n: i)
                ]
            )
            // getSignColor
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
        let xMargin = Double(delegate.chartStyle.margin)
        let yMargin = Double(delegate.chartStyle.margin)
        delegate.list1.keys.forEach{ key in
            if (!delegate.list1[key]!.isDisp) {
                return;
            }
            if (key == 10000 || key == 10001) {
                return;
            }
//            print("absolute_position:")
//            print(delegate.list1[key]!.absolute_position)
            let index = (Int)(delegate.list1[key]!.absolute_position / 5);
            b.boxUpdate(initIndex: index);
            
//            print("index:")
//            print(index * 5)

            // 🌙とか
            let str = NSAttributedString(
                string: CommonData.getPlanetSymbol2(n: key),
                attributes: [
                    NSAttributedString.Key.font: NSFont(name: "microcosm", size: 24.0)!,
                    NSAttributedString.Key.foregroundColor: CommonData.getPlanetColor(n: key)
                ]
            )
            
            // 3重は若干寄せる
            if (delegate.bands < 3) {
                let pos = Util.Rotate(x: Double(diameter) * 0.38, y: 0, degree: Double(b.index * 5) - rett[1])
                str.draw(at: NSPoint(
                    x: Double(pos.x + radius2 + xMargin),
                    y: Double(pos.y) + radius2 + yMargin
                ))
            } else {
                let pos = Util.Rotate(x: Double(diameter) * 0.40, y: 0, degree: Double(b.index * 5) - rett[1])
                str.draw(at: NSPoint(
                    x: Double(pos.x + radius2 + xMargin),
                    y: Double(pos.y) + radius2 - 2.0 + yMargin
                ))
            }
            
            let IntDegree = (Int)(delegate.list1[key]!.absolute_position.truncatingRemainder(dividingBy: 30))
            let strDegree = NSAttributedString(string: (String)(format: "%02d", IntDegree))
            if (delegate.bands < 3) {
                let posDegree = Util.Rotate(x: Double(diameter) * 0.34, y: 0, degree: Double(b.index * 5) - rett[1])
                // yはシンボルのフォントサイズの都合若干下になるので+5する
                strDegree.draw(at: NSPoint(
                    x: Double(posDegree.x + radius2 + 2.0 + xMargin),
                    y: Double(posDegree.y + radius2 + 5.0 + yMargin)
                ))
            } else {
                let posDegree = Util.Rotate(x: Double(diameter) * 0.37, y: 0, degree: Double(b.index * 5) - rett[1])
                // yはシンボルのフォントサイズの都合若干下になるので+5する
                strDegree.draw(at: NSPoint(
                    x: Double(posDegree.x + radius2 + 2.0 + xMargin),
                    y: Double(posDegree.y + radius2 + 5.0 + yMargin)
                ))
            }

            if (delegate.bands == 1 && delegate.config.dispPattern == EDispPattern.FULL) {
                let strSymbol = NSAttributedString(
                    string: CommonData.getSignSymbol(n: (Int)(delegate.list1[key]!.absolute_position / 30)),
                    attributes: [
                        NSAttributedString.Key.font: NSFont(name: "microcosm", size: 24.0)!,
                        NSAttributedString.Key.foregroundColor: CommonData.getSignColor(n: (Int)(delegate.list1[key]!.absolute_position / 30))
                    ]
                )

                let posSymbol = Util.Rotate(x: Double(diameter) * 0.30, y: 0, degree: Double(b.index * 5) - rett[1])
                // yはシンボルのフォントサイズの都合若干下になるので+2する
                strSymbol.draw(at: NSPoint(
                    x: Double(posSymbol.x + radius2 + 1 + xMargin),
                    y: Double(posSymbol.y + radius2 + 2 + yMargin)
                ))

                let subDegree = getSubDegree(value: delegate.list1[key]!.absolute_position, decimalDisp: delegate.config.decimalDisp)
                let strSubDegree = NSAttributedString(string: (String)(format: "%02d", subDegree))
                let posSubDegree = Util.Rotate(x: Double(diameter) * 0.27, y: 0, degree: Double(b.index * 5) - rett[1])
                // yはシンボルのフォントサイズの都合若干下になるので+5する
                strSubDegree.draw(at: NSPoint(
                    x: Double(posSubDegree.x + radius2 + 2 + xMargin),
                    y: Double(posSubDegree.y + radius2 + 5 + yMargin)
                ))
                if (delegate.list1[key]!.speed < 0) {
                    let reverse = NSAttributedString(
                        string: "Z",
                        attributes: [
                            NSAttributedString.Key.font: NSFont(name: "microcosm", size: 24.0)!,
                            NSAttributedString.Key.foregroundColor: NSColor.black
                        ]
                    )
                    let pos3 = Util.Rotate(x: Double(diameter) * 0.24, y: 0, degree: Double(b.index * 5) - rett[1])
                    reverse.draw(at: NSPoint(
                        x: Double(pos3.x + radius2 + xMargin),
                        y: Double(pos3.y) + radius2 + yMargin
                    ))

                }
            }
        }
        
        if (delegate.bands == 2) {
            let b2 = Box()
            b2.boxInit();
            delegate.list2.keys.forEach{ key in
                if (!delegate.list2[key]!.isDisp) {
                    return;
                }
                let index = (Int)(delegate.list2[key]!.absolute_position / 5);
                b2.boxUpdate(initIndex: index);

                // 🌙とか
                let str = NSAttributedString(
                    string: CommonData.getPlanetSymbol2(n: key),
                    attributes: [
                        NSAttributedString.Key.font: NSFont(name: "microcosm", size: 24.0)!,
                        NSAttributedString.Key.foregroundColor: CommonData.getPlanetColor(n: key)
                    ]
                )
                let pos = Util.Rotate(x: Double(diameter) * 0.28, y: 0, degree: Double(b2.index * 5) - rett[1])
                str.draw(at: NSPoint(
                    x: Double(pos.x + radius2 + xMargin),
                    y: Double(pos.y) + radius2 + yMargin
                ))
                
                let IntDegree = (Int)(delegate.list2[key]!.absolute_position.truncatingRemainder(dividingBy: 30))
                let strDegree = NSAttributedString(string: (String)(format: "%02d", IntDegree))
                let posDegree = Util.Rotate(x: Double(diameter) * 0.24, y: 0, degree: Double(b2.index * 5) - rett[1])
                // yはシンボルのフォントサイズの都合若干下になるので+5する
                strDegree.draw(at: NSPoint(
                    x: Double(posDegree.x + radius2 + 2.0 + xMargin),
                    y: Double(posDegree.y + radius2 + 5.0 + yMargin)
                ))
            }
        }
        
        if (delegate.bands == 3) {
            let b2 = Box()
            b2.boxInit();
            delegate.list2.keys.forEach{ key in
                if (!delegate.list2[key]!.isDisp) {
                    return;
                }
                let index = (Int)(delegate.list2[key]!.absolute_position / 5);
                b2.boxUpdate(initIndex: index);

                // 🌙とか
                let str = NSAttributedString(
                    string: CommonData.getPlanetSymbol2(n: key),
                    attributes: [
                        NSAttributedString.Key.font: NSFont(name: "microcosm", size: 24.0)!,
                        NSAttributedString.Key.foregroundColor: CommonData.getPlanetColor(n: key)
                    ]
                )
                let pos = Util.Rotate(x: Double(diameter) * 0.32, y: 0, degree: Double(b2.index * 5) - rett[1])
                str.draw(at: NSPoint(
                    x: Double(pos.x + radius2 + xMargin),
                    y: Double(pos.y) + radius2 + yMargin
                ))
                
                let IntDegree = (Int)(delegate.list2[key]!.absolute_position.truncatingRemainder(dividingBy: 30))
                let strDegree = NSAttributedString(string: (String)(format: "%02d", IntDegree))
                let posDegree = Util.Rotate(x: Double(diameter) * 0.30, y: 0, degree: Double(b2.index * 5) - rett[1])
                // yはシンボルのフォントサイズの都合若干下になるので+5する
                strDegree.draw(at: NSPoint(
                    x: Double(posDegree.x + radius2 + 2.0 + xMargin),
                    y: Double(posDegree.y + radius2 + 5.0 + yMargin)
                ))

            }
        }

        // aspects
        var aspectPt: Position = Position(x: 0, y: 0)
        var aspectPtEnd: Position = Position(x: 0, y: 0)
        // aspectsData[0, 0] => natal-natal
        delegate.list1.keys.forEach{ key in
            if (!delegate.list1[key]!.isDisp) {
                return
            }
            // isAspectDispは不要
            // たぶんそもそも入れていないから？
            
            delegate.list1[key]!.aspects.forEach { aspect in
                if (!delegate.list1[aspect.targetPlanetNo]!.isDisp) {
                    return
                }
                let positionSrc = Util.Rotate(x: Double(centerRadius), y: 0, degree: Double(aspect.sourceDegree) - rett[1])
                positionSrc.x = positionSrc.x + xMargin + Double(radius)
                positionSrc.y = positionSrc.y + yMargin + Double(radius)
                let positionTarget = Util.Rotate(x: Double(centerRadius), y: 0, degree: Double(aspect.targetDegree) - rett[1])
                positionTarget.x = positionTarget.x + xMargin + Double(radius)
                positionTarget.y = positionTarget.y + yMargin + Double(radius)

                // これ描画コストかからない？
                if (aspect.aspectKind == EAspect.TRINE) {
                    let pathAspect = CGMutablePath()
                    pathAspect.move(to: CGPoint(x: positionSrc.x, y: positionSrc.y))
                    pathAspect.addLine(to: CGPoint(x: positionTarget.x, y: positionTarget.y))
                    context.setStrokeColor(NSColor.orange.cgColor)
                    if (aspect.softHard == SoftHard.SOFT) {
                        context.setLineDash(phase: 0.4, lengths: [CGFloat(3.8)])
                    } else {
                        context.setLineDash(phase: 0, lengths: [])
                    }
                    context.addPath(pathAspect)
                    context.strokePath()
                } else if (aspect.aspectKind == EAspect.OPPOSITION) {
                    let pathAspect = CGMutablePath()
                    pathAspect.move(to: CGPoint(x: positionSrc.x, y: positionSrc.y))
                    pathAspect.addLine(to: CGPoint(x: positionTarget.x, y: positionTarget.y))
                    context.setStrokeColor(NSColor.systemPink.cgColor)
                    if (aspect.softHard == SoftHard.SOFT) {
                        context.setLineDash(phase: 0.4, lengths: [CGFloat(3.8)])
                    } else {
                        context.setLineDash(phase: 0, lengths: [])
                    }
                    context.addPath(pathAspect)
                    context.strokePath()
                } else if (aspect.aspectKind == EAspect.SQUARE) {
                    let pathAspect = CGMutablePath()
                    pathAspect.move(to: CGPoint(x: positionSrc.x, y: positionSrc.y))
                    pathAspect.addLine(to: CGPoint(x: positionTarget.x, y: positionTarget.y))
                    context.setStrokeColor(NSColor.purple.cgColor)
                    if (aspect.softHard == SoftHard.SOFT) {
                        context.setLineDash(phase: 0.4, lengths: [CGFloat(3.8)])
                    } else {
                        context.setLineDash(phase: 0, lengths: [])
                    }

                    context.addPath(pathAspect)
                    context.strokePath()
                } else if (aspect.aspectKind == EAspect.SEXTILE) {
                    let pathAspect = CGMutablePath()
                    pathAspect.move(to: CGPoint(x: positionSrc.x, y: positionSrc.y))
                    pathAspect.addLine(to: CGPoint(x: positionTarget.x, y: positionTarget.y))
                    context.setStrokeColor(NSColor.green.cgColor)
                    if (aspect.softHard == SoftHard.SOFT) {
                        context.setLineDash(phase: 0.4, lengths: [CGFloat(3.8)])
                    } else {
                        context.setLineDash(phase: 0, lengths: [])
                    }
                    context.addPath(pathAspect)
                    context.strokePath()
                } else {
                    let pathAspect = CGMutablePath()
                    pathAspect.move(to: CGPoint(x: positionSrc.x, y: positionSrc.y))
                    pathAspect.addLine(to: CGPoint(x: positionTarget.x, y: positionTarget.y))
                    context.setStrokeColor(NSColor.black.cgColor)
                    if (aspect.softHard == SoftHard.SOFT) {
                        context.setLineDash(phase: 0.4, lengths: [CGFloat(3.8)])
                    } else {
                        context.setLineDash(phase: 0, lengths: [])
                    }
                    context.addPath(pathAspect)
                    context.strokePath()
                }
            }
        }

        
//        Position aspectPt;
//        Position aspectPtEnd;
//        SKPaint aspectLine = new SKPaint();
//        aspectLine.Style = SKPaintStyle.Stroke;
//        aspectLine.StrokeWidth = 1.0F;
//        SKPaint aspectSymboolText = new SKPaint()
//        {
//            TextSize = 24,
//            Style = SKPaintStyle.Fill
//        };

//        // aspectsData[0, 0] => natal-natal
//        if (appDelegate.aspect11disp)
//        {
//            foreach (KeyValuePair<int, PlanetData> pData in sortPlanetData)
//            {
//                PlanetData planet = pData.Value;
//                Debug.WriteLine("planet: " + planet.no + "(" + CommonData.getPlanetSymbolText(planet.no) + ")");
//                Debug.WriteLine("isDisp: " + planet.isDisp);
//                Debug.WriteLine("display: " + planet.absolute_position);
//
//                if (!planet.isDisp)
//                {
//                    continue;
//                }
//                // isAspectDispは不要
//
//                foreach (AspectInfo x in planet.aspects)
//                {
//                    if (!list1[x.targetPlanetNo].isDisp)
//                    {
//                        continue;
//                    }
//
//                    if (x.softHard == SoftHard.SOFT)
//                    {
//                        aspectLine.PathEffect = SKPathEffect.CreateDash(new float[] { 1, 4 }, (float)2.0);
//                    }
//                    else
//                    {
//                        aspectLine.PathEffect = null;
//                    }
//        double calcDegree = x.sourceDegree - houseList1[1];
//        if (calcDegree < 0)
//        {
//            calcDegree += 360;
//        }
//        double calcDegree2 = x.targetDegree - houseList1[1];
//        if (calcDegree2 < 0)
//        {
//            calcDegree2 += 360;
//        }
//        aspectPt = Util.Rotate(centerDiameter, 0, calcDegree);
//        aspectPt.x = aspectPt.x + CenterX;
//        aspectPt.y = -1 * aspectPt.y + CenterY;
//
//        aspectPtEnd = Util.Rotate(centerDiameter, 0, calcDegree2);
//        aspectPtEnd.x = aspectPtEnd.x + CenterX;
//        aspectPtEnd.y = -1 * aspectPtEnd.y + CenterY;
//
//        GetAspectLineAndText(x.aspectKind, ref aspectLine, ref aspectSymboolText);
//        DrawAspect(cvs, x, aspectPt, aspectPtEnd, aspectLine, aspectSymboolText);
//    }
//
//    /*
//    aspectSymbolPt = Util.Rotate(diameter - 160, 0, planet.absolute_position - houseList1[1]);
//    aspectSymbolPt.x = aspectSymbolPt.x + CenterX;
//    aspectSymbolPt.y = -1 * aspectSymbolPt.y + CenterY + 10;
//    */
//}
//}

        //NSBitmapImageRep(cgImage: context.makeImage())
        
//        let i = context.makeImage()
        
        context.restoreGState()
    }
    
    
    /// 小数点以下を求める、config次第では59度まで
    /// - Parameter value: 絶対度数
    /// - Returns: 結果
    func getSubDegree(value: Double, decimalDisp: EDecimalDisp) -> Int {
        let tmp = value.truncatingRemainder(dividingBy: 1) * 100
        if decimalDisp == EDecimalDisp.DEGREE {
            let tmp2 = CommonData.DecimalToHex(deci: tmp)
            return (Int)(tmp2)
        }
        return (Int)(tmp)
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
