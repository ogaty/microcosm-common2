//
//  AspectInfo.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/07/17.
//

import Foundation

public class AspectInfo
{
    public var sourceDegree: Double = 0.0
    public var targetDegree: Double = 0.0
    public var aspectKind: EAspect = EAspect.CONJUNCTION
    public var softHard: SoftHard = SoftHard.HARD
    public var sourcePlanetNo: Int = 0
    public var targetPlanetNo: Int = 0
    public var aspectDegree: Double = 0.0
    
    init() {
    }
}
