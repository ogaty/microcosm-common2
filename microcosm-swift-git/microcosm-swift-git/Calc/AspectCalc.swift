//
//  AspectCalc.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/02/15.
//

import Foundation

public class AspectCalc
{
    public func AspectCalcSame(a_setting: SettingData, list: [Int:PlanetData]) -> [Int:PlanetData]
    {
        var calcList = [Int: PlanetData]()
        list.keys.forEach { key in
            calcList[key] = list[key]
        }

        // if (natal-natal)
        list.keys.forEach { key in
            if (!list[key]!.isAspectDisp) {
                return
            }
            list.keys.forEach { key2 in
                if (key == key2) {
                    return
                }
                if (!list[key2]!.isAspectDisp) {
                    return
                }
                
                // 90.0 と　300.0では210度ではなく150度にならなければいけない
                var aspect_degree = list[key]!.absolute_position - list[key2]!.absolute_position
                
                if (aspect_degree > 180)
                {
                    aspect_degree = list[key]!.absolute_position + 360 - list[key2]!.absolute_position
                }
                if (aspect_degree < 0)
                {
                    aspect_degree = abs(aspect_degree);
                }
                
                for i in 0...15 {
                    if (i == EAspect.CONJUNCTION.rawValue && a_setting.dispConjunction == 0) {
                        return
                    }
                    if (i == EAspect.OPPOSITION.rawValue && a_setting.dispOpposition == 0) {
                        return
                    }
                    if (i == EAspect.TRINE.rawValue && a_setting.dispTrine == 0) {
                        return
                    }
                    if (i == EAspect.SQUARE.rawValue && a_setting.dispSquare == 0) {
                        return
                    }
                    if (i == EAspect.SEXTILE.rawValue && a_setting.dispSextile == 0) {
                        return
                    }
                    if (i == EAspect.INCONJUNCT.rawValue && a_setting.dispInconjunct == 0) {
                        return
                    }
                    if (i == EAspect.SESQUIQUADRATE.rawValue && a_setting.dispSesquiQuadrate == 0) {
                        return
                    }
                    if (i == EAspect.SEMISEXTILE.rawValue && a_setting.dispSemiSextile == 0) {
                        return
                    }
                    if (i == EAspect.SEMISQUARE.rawValue && a_setting.dispSemiSquare == 0) {
                        return
                    }
                    if (i == EAspect.QUINTILE.rawValue && a_setting.dispQuintile == 0) {
                        return
                    }
                    if (i == EAspect.BIQUINTILE.rawValue && a_setting.dispAspectBiQuintile == 0) {
                        return
                    }
                    if (i == EAspect.SEMIQINTILE.rawValue && a_setting.dispAspectSemiQuintile == 0) {
                        return
                    }
                    if (i == EAspect.NOVILE.rawValue && a_setting.dispNovile == 0) {
                        return
                    }
                    if (i == EAspect.SEPTILE.rawValue && a_setting.dispAspectSeptile == 0) {
                        return
                    }
                    if (i == EAspect.QUINDECILE.rawValue && a_setting.dispAspectQuindecile == 0) {
                        return
                    }
                    
                    let degree = getAspectDegree(kind: getEAspect(key: i))
                    
                    var status: AspectStatus = AspectStatus()
                    
                    if (key == EPlanets.SUN.rawValue || key == EPlanets.MOON.rawValue)
                    {
                        if (isFirstAspectKind(kind: getEAspect(key: i)))
                        {
                            status = IsAspect(aspectDegree: aspect_degree, targetDegree: degree, orb: a_setting.orbSunMoon)
                        } else {
                            status = IsAspect(aspectDegree: aspect_degree, targetDegree: degree, orb: a_setting.orb2nd)
                        }
                    } else {
                        if (isFirstAspectKind(kind: getEAspect(key: i)))
                        {
                            status = IsAspect(aspectDegree: aspect_degree, targetDegree: degree, orb: a_setting.orb1st)
                        }
                        else
                        {
                            status = IsAspect(aspectDegree: aspect_degree, targetDegree: degree, orb: a_setting.orb2nd)
                        }
                    }
                    if (status.aspect)
                    {
                        let asp = AspectInfo()
                        asp.sourceDegree = list[key]!.absolute_position
                        asp.targetDegree = list[key2]!.absolute_position
                        asp.sourcePlanetNo = list[key]!.no
                        asp.targetPlanetNo = list[key2]!.no
                        asp.aspectDegree = aspect_degree
                        asp.softHard = status.softHard
                        asp.aspectDegree = aspect_degree
                        if (i == EAspect.CONJUNCTION.rawValue) {
                            asp.aspectKind = EAspect.CONJUNCTION
                        } else if (i == EAspect.OPPOSITION.rawValue) {
                            asp.aspectKind = EAspect.CONJUNCTION
                        } else if (i == EAspect.TRINE.rawValue) {
                            asp.aspectKind = EAspect.TRINE
                        } else if (i == EAspect.SQUARE.rawValue) {
                            asp.aspectKind = EAspect.SQUARE
                        } else if (i == EAspect.SEXTILE.rawValue) {
                            asp.aspectKind = EAspect.SEXTILE
                        } else if (i == EAspect.INCONJUNCT.rawValue) {
                            asp.aspectKind = EAspect.INCONJUNCT
                        } else if (i == EAspect.SESQUIQUADRATE.rawValue) {
                            asp.aspectKind = EAspect.SESQUIQUADRATE
                        } else if (i == EAspect.SEMISEXTILE.rawValue) {
                            asp.aspectKind = EAspect.SEMISEXTILE
                        } else if (i == EAspect.SEMISQUARE.rawValue) {
                            asp.aspectKind = EAspect.SEMISQUARE
                        } else if (i == EAspect.QUINTILE.rawValue) {
                            asp.aspectKind = EAspect.QUINTILE
                        } else if (i == EAspect.BIQUINTILE.rawValue) {
                            asp.aspectKind = EAspect.BIQUINTILE
                        } else if (i == EAspect.SEMIQINTILE.rawValue) {
                            asp.aspectKind = EAspect.SEMIQINTILE
                        } else if (i == EAspect.NOVILE.rawValue) {
                            asp.aspectKind = EAspect.NOVILE
                        } else if (i == EAspect.SEPTILE.rawValue) {
                            asp.aspectKind = EAspect.SEPTILE
                        } else if (i == EAspect.QUINDECILE.rawValue) {
                            asp.aspectKind = EAspect.QUINDECILE
                        }
                        
                        calcList[key]?.aspects.append(asp)
                        
                        return
                    }
                }
            }
        }
        
        var ret: [Int: PlanetData] = [:]
        calcList.keys.forEach { key in
            ret[calcList[key]!.no] = calcList[key]
        }

        return ret
    }

    
    public func AspectCalcOther(
        a_setting: SettingData,
        fromList: [Int:PlanetData],
        toList: [Int:PlanetData],
        listKind: Int
    ) -> [Int:PlanetData]
    {
        var calcFromList = [Int: PlanetData]()
        fromList.keys.forEach { key in
            calcFromList[key] = fromList[key]
        }
        var calcToList = [Int: PlanetData]()
        toList.keys.forEach { key in
            calcToList[key] = toList[key]
        }

        fromList.keys.forEach { key in
            if (!fromList[key]!.isAspectDisp) {
                return
            }
            toList.keys.forEach { key2 in
                if (key == key2) {
                    return
                }
                if (!toList[key2]!.isAspectDisp) {
                    return
                }

                // 90.0 と　300.0では210度ではなく150度にならなければいけない
                var aspect_degree = calcFromList[key]!.absolute_position - calcToList[key2]!.absolute_position

                if (aspect_degree > 180)
                {
                    aspect_degree = calcFromList[key]!.absolute_position + 360 - calcToList[key2]!.absolute_position;
                }
                if (aspect_degree < 0)
                {
                    aspect_degree = abs(aspect_degree);
                }
                
                for i in 0...15 {
                    if (i == EAspect.CONJUNCTION.rawValue && a_setting.dispConjunction == 0) {
                        return
                    }
                    if (i == EAspect.OPPOSITION.rawValue && a_setting.dispOpposition == 0) {
                        return
                    }
                    if (i == EAspect.TRINE.rawValue && a_setting.dispTrine == 0) {
                        return
                    }
                    if (i == EAspect.SQUARE.rawValue && a_setting.dispSquare == 0) {
                        return
                    }
                    if (i == EAspect.SEXTILE.rawValue && a_setting.dispSextile == 0) {
                        return
                    }
                    if (i == EAspect.INCONJUNCT.rawValue && a_setting.dispInconjunct == 0) {
                        return
                    }
                    if (i == EAspect.SESQUIQUADRATE.rawValue && a_setting.dispSesquiQuadrate == 0) {
                        return
                    }
                    if (i == EAspect.SEMISEXTILE.rawValue && a_setting.dispSemiSextile == 0) {
                        return
                    }
                    if (i == EAspect.SEMISQUARE.rawValue && a_setting.dispSemiSquare == 0) {
                        return
                    }
                    if (i == EAspect.QUINTILE.rawValue && a_setting.dispQuintile == 0) {
                        return
                    }
                    if (i == EAspect.BIQUINTILE.rawValue && a_setting.dispAspectBiQuintile == 0) {
                        return
                    }
                    if (i == EAspect.SEMIQINTILE.rawValue && a_setting.dispAspectSemiQuintile == 0) {
                        return
                    }
                    if (i == EAspect.NOVILE.rawValue && a_setting.dispNovile == 0) {
                        return
                    }
                    if (i == EAspect.SEPTILE.rawValue && a_setting.dispAspectSeptile == 0) {
                        return
                    }
                    if (i == EAspect.QUINDECILE.rawValue && a_setting.dispAspectQuindecile == 0) {
                        return
                    }
                    
                    var degree = getAspectDegree(kind: getEAspect(key: i))

                    var status: AspectStatus = AspectStatus()
                    
                    // 1-2
                    if (listKind == 3)
                    {
                        if (key == EPlanets.SUN.rawValue || key == EPlanets.MOON.rawValue)
                        {
                            if (isFirstAspectKind(kind: getEAspect(key: i)))
                            {
                                status = IsAspect(aspectDegree: aspect_degree, targetDegree: degree, orb: a_setting.orbSunMoon)
                            } else {
                                status = IsAspect(aspectDegree: aspect_degree, targetDegree: degree, orb: a_setting.orb2nd)
                            }
                        } else {
                            if (isFirstAspectKind(kind: getEAspect(key: i)))
                            {
                                status = IsAspect(aspectDegree: aspect_degree, targetDegree: degree, orb: a_setting.orb1st)
                            }
                            else
                            {
                                status = IsAspect(aspectDegree: aspect_degree, targetDegree: degree, orb: a_setting.orb2nd)
                            }
                        }
                        if (status.aspect)
                        {
                            var asp = AspectInfo()
                            asp.sourceDegree = calcFromList[key]!.absolute_position
                            asp.targetDegree = calcToList[key2]!.absolute_position
                            asp.sourcePlanetNo = calcFromList[key]!.no
                            asp.targetPlanetNo = calcToList[key2]!.no
                            asp.aspectDegree = aspect_degree
                            asp.softHard = status.softHard
                            asp.aspectDegree = aspect_degree
                            if (i == EAspect.CONJUNCTION.rawValue) {
                                asp.aspectKind = EAspect.CONJUNCTION
                            } else if (i == EAspect.OPPOSITION.rawValue) {
                                asp.aspectKind = EAspect.CONJUNCTION
                            } else if (i == EAspect.TRINE.rawValue) {
                                asp.aspectKind = EAspect.TRINE
                            } else if (i == EAspect.SQUARE.rawValue) {
                                asp.aspectKind = EAspect.SQUARE
                            } else if (i == EAspect.SEXTILE.rawValue) {
                                asp.aspectKind = EAspect.SEXTILE
                            } else if (i == EAspect.INCONJUNCT.rawValue) {
                                asp.aspectKind = EAspect.INCONJUNCT
                            } else if (i == EAspect.SESQUIQUADRATE.rawValue) {
                                asp.aspectKind = EAspect.SESQUIQUADRATE
                            } else if (i == EAspect.SEMISEXTILE.rawValue) {
                                asp.aspectKind = EAspect.SEMISEXTILE
                            } else if (i == EAspect.SEMISQUARE.rawValue) {
                                asp.aspectKind = EAspect.SEMISQUARE
                            } else if (i == EAspect.QUINTILE.rawValue) {
                                asp.aspectKind = EAspect.QUINTILE
                            } else if (i == EAspect.BIQUINTILE.rawValue) {
                                asp.aspectKind = EAspect.BIQUINTILE
                            } else if (i == EAspect.SEMIQINTILE.rawValue) {
                                asp.aspectKind = EAspect.SEMIQINTILE
                            } else if (i == EAspect.NOVILE.rawValue) {
                                asp.aspectKind = EAspect.NOVILE
                            } else if (i == EAspect.SEPTILE.rawValue) {
                                asp.aspectKind = EAspect.SEPTILE
                            } else if (i == EAspect.QUINDECILE.rawValue) {
                                asp.aspectKind = EAspect.QUINDECILE
                            }

                            calcFromList[key]?.secondAspects.append(asp)
                            
                            return
                        }

                    } else {
                        // 1-3
                        // 2-3
                        if (key == EPlanets.SUN.rawValue || key == EPlanets.MOON.rawValue)
                        {
                            if (isFirstAspectKind(kind: getEAspect(key: i)))
                            {
                                status = IsAspect(aspectDegree: aspect_degree, targetDegree: degree, orb: a_setting.orbSunMoon)
                            } else {
                                status = IsAspect(aspectDegree: aspect_degree, targetDegree: degree, orb: a_setting.orb2nd)
                            }
                        } else {
                            if (isFirstAspectKind(kind: getEAspect(key: i)))
                            {
                                status = IsAspect(aspectDegree: aspect_degree, targetDegree: degree, orb: a_setting.orb1st)
                            }
                            else
                            {
                                status = IsAspect(aspectDegree: aspect_degree, targetDegree: degree, orb: a_setting.orb2nd)
                            }
                        }
                        if (status.aspect)
                        {
                            var asp = AspectInfo()
                            asp.sourceDegree = calcFromList[key]!.absolute_position
                            asp.targetDegree = calcToList[key2]!.absolute_position
                            asp.sourcePlanetNo = calcFromList[key]!.no
                            asp.targetPlanetNo = calcToList[key2]!.no
                            asp.aspectDegree = aspect_degree
                            asp.softHard = status.softHard
                            asp.aspectDegree = aspect_degree
                            if (i == EAspect.CONJUNCTION.rawValue) {
                                asp.aspectKind = EAspect.CONJUNCTION
                            } else if (i == EAspect.OPPOSITION.rawValue) {
                                asp.aspectKind = EAspect.CONJUNCTION
                            } else if (i == EAspect.TRINE.rawValue) {
                                asp.aspectKind = EAspect.TRINE
                            } else if (i == EAspect.SQUARE.rawValue) {
                                asp.aspectKind = EAspect.SQUARE
                            } else if (i == EAspect.SEXTILE.rawValue) {
                                asp.aspectKind = EAspect.SEXTILE
                            } else if (i == EAspect.INCONJUNCT.rawValue) {
                                asp.aspectKind = EAspect.INCONJUNCT
                            } else if (i == EAspect.SESQUIQUADRATE.rawValue) {
                                asp.aspectKind = EAspect.SESQUIQUADRATE
                            } else if (i == EAspect.SEMISEXTILE.rawValue) {
                                asp.aspectKind = EAspect.SEMISEXTILE
                            } else if (i == EAspect.SEMISQUARE.rawValue) {
                                asp.aspectKind = EAspect.SEMISQUARE
                            } else if (i == EAspect.QUINTILE.rawValue) {
                                asp.aspectKind = EAspect.QUINTILE
                            } else if (i == EAspect.BIQUINTILE.rawValue) {
                                asp.aspectKind = EAspect.BIQUINTILE
                            } else if (i == EAspect.SEMIQINTILE.rawValue) {
                                asp.aspectKind = EAspect.SEMIQINTILE
                            } else if (i == EAspect.NOVILE.rawValue) {
                                asp.aspectKind = EAspect.NOVILE
                            } else if (i == EAspect.SEPTILE.rawValue) {
                                asp.aspectKind = EAspect.SEPTILE
                            } else if (i == EAspect.QUINDECILE.rawValue) {
                                asp.aspectKind = EAspect.QUINDECILE
                            }

                            calcFromList[key]?.thirdAspects.append(asp)
                            
                            return
                        }
                    }
                }
            }
        }

        var ret: [Int: PlanetData] = [:]
        calcFromList.keys.forEach { key in
            ret[calcFromList[key]!.no] = calcFromList[key]
        }

        return ret
    }

    public func getEAspect(key: Int) -> EAspect
    {
        switch (key) {
        case 1:
            return EAspect.CONJUNCTION
        case 2:
            return EAspect.OPPOSITION
        case 3:
            return EAspect.INCONJUNCT
        case 4:
            return EAspect.SESQUIQUADRATE
        case 5:
            return EAspect.TRINE
        case 6:
            return EAspect.SQUARE
        case 7:
            return EAspect.SEXTILE
        case 8:
            return EAspect.SEMISEXTILE
        case 9:
            return EAspect.SEMIQINTILE
        case 10:
            return EAspect.NOVILE
        case 11:
            return EAspect.SEMISQUARE
        case 12:
            return EAspect.SEPTILE
        case 13:
            return EAspect.QUINTILE
        case 14:
            return EAspect.BIQUINTILE
        case 15:
            return EAspect.QUINDECILE
        default:
            return EAspect.CONJUNCTION
        }
    }
    
    
    /// Aspectの度数を求める
    /// - Parameter kind: AspectKind
    /// - Returns: 度数
    public func getAspectDegree(kind: EAspect) -> Double
    {
        switch (kind)
        {
            case EAspect.CONJUNCTION:
                return 0.0
            case EAspect.OPPOSITION:
                return 180.0
            case EAspect.TRINE:
                return 120.0
            case EAspect.SQUARE:
                return 90.0
            case EAspect.SEXTILE:
                return 60.0
            case EAspect.INCONJUNCT:
                return 150.0
            case EAspect.SESQUIQUADRATE:
                return 135.0
            case EAspect.SEMISQUARE:
                return 45.0
            case EAspect.SEMISEXTILE:
                return 30.0
            case EAspect.QUINTILE:
                return 72.0
            case EAspect.BIQUINTILE:
                return 144.0
            case EAspect.SEMIQINTILE:
                return 36.0
            case EAspect.SEPTILE:
                return 360.0 / 7
            case EAspect.NOVILE:
                return 40.0
            case EAspect.QUINDECILE:
                return 165.0
        }
    }
    
    public func IsAspect(aspectDegree: Double, targetDegree: Double, orb: [Float]) -> AspectStatus
    {
        var aspect = AspectStatus()
        if (targetDegree < aspectDegree + Double(orb[1]) &&
            targetDegree > aspectDegree - Double(orb[1]))
        {
            
            aspect.aspect = true
            aspect.softHard = SoftHard.HARD
        }
        else if (targetDegree < aspectDegree + Double(orb[0]) &&
                 targetDegree > aspectDegree - Double(orb[0]))
        {
            aspect.aspect = true
            aspect.softHard = SoftHard.SOFT
        }
        
        return aspect
    }
    
    public func isFirstAspectKind(kind: EAspect) -> Bool
    {
        if (kind == EAspect.CONJUNCTION ||
            kind == EAspect.OPPOSITION ||
            kind == EAspect.TRINE ||
            kind == EAspect.SQUARE ||
            kind == EAspect.SEXTILE) {
            return true
        }
        return false
    }

}
