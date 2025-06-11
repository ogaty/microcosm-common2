//
//  AstroConst.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/02/05.
//

import Foundation

/// とりあえず今後作り直すとしてもずっと使うであろう処理

// このあたりのenumはswissEphemerisと値を揃えること
enum EProgression: Int {
    case PRIMARY = 0
    case SECONDARY = 1
    case SOLARARC = 2
    case CPS = 3
}

enum EHouse: Int {
    case PLACIDUS = 0
    case KOCH = 1
    case CAMPANUS = 2
    case EQUAL = 3
    case PORPHYRY = 4
    case REGIOMONTANUS = 5
    case SOLAR = 6
    case SOLARSIGN = 7
    case ZEROARIES = 8
}

// DragonTail、ASC、MC等は便宜上適当な番号を入れている
// 厳密には100も10000も小惑星番号として存在するけどまあいいや
enum EPlanets: Int {
    case SUN = 0
    case MOON = 1
    case MERCURY = 2
    case VENUS = 3
    case MARS = 4
    case JUPITER = 5
    case SATURN = 6
    case URANUS = 7
    case NEPTUNE = 8
    case PLUTO = 9
    case DH_MEANNODE = 10
    case DH_TRUENODE = 11
    case DT_MEAN = 101
    case DT_TRUE = 102
    case MEAN_LILITH = 12 // 小惑星のリリス(1181)と混同しないこと
    case OSCU_LILITH = 13 // 小惑星のリリス(1181)と混同しないこと
    case EARTH = 14
    case CHIRON = 15
    case POLUS = 16
    case CERES = 17
    case PALLAS = 18
    case JUNO = 19
    case VESTA = 20
    case ASC = 10000
    case MC = 10001
    case ERIS = 136199
    case SEDNA = 90377
    case HAUMEA = 136108
    case MAKEMAKE = 136472

    case VT = 10002
    case POF = 10003

}

enum ESign: Int {
    case ARIES = 0
    case TAURUS = 1
    case GEMINI = 2
    case CANCER = 3
    case LEO = 4
    case VIRGO = 5
    case LIBRA = 6
    case SCORPIO = 7
    case SAGITTARIUS = 8
    case CAPRICORN = 9
    case AQUARIUS = 10
    case PISCES = 11
}

// これ0始まりじゃなくて1始まりなんだ
public enum EAspect: Int {
    case CONJUNCTION = 1
    case OPPOSITION = 2 // 360 / 2
    case INCONJUNCT = 3
    case SESQUIQUADRATE = 4
    case TRINE = 5 // 360 / 3
    case SQUARE = 6 // 360 /4
    case SEXTILE = 7 // 360 / 60
    case SEMISEXTILE = 8 // 360 / 12 = 30
    case SEMIQINTILE = 9 // 360 / 10 = 36
    case NOVILE = 10 // 360 / 9 = 40
    case SEMISQUARE = 11 // 360 / 8 = 45
    case SEPTILE = 12 // 360 / 7 = 51.42
    case QUINTILE = 13 // 360 / 5 = 72
    case BIQUINTILE = 14 // (360 / 5) + 2 = 144
    case QUINDECILE = 15 // 165
}

// config
enum ECentric: Int, Codable {
    case GEO_CENTRIC = 0
    case HELIO_CENTRIC = 1
}

enum ESideReal: Int, Codable {
    case TROPICAL = 0
    case SIDEREAL = 1
    case DRACONIC = 2
}

enum EDecimalDisp: Int {
    case DECIMAL = 0
    case DEGREE = 1
}

enum EDispPattern: Int {
    case FULL = 0
    case MINI = 1
}

enum EColor29: Int {
    case NOCHANGE = 0
    case CHANGE = 1
}

enum ENodeCalc: Int, Codable {
    case TRUE = 0
    case MEAN = 1
}

enum ELilithCalc: Int, Codable {
    case OSCU = 0
    case MEAN = 1
}

enum EHouseCalc: Int {
    case TRUE = 0
    case MEAN = 1
}

enum SweConst: Int {
    case SEFLG_SWIEPH = 2
    case SEFLG_HELCTR = 8
    case SEFLG_SPEED = 256
    case SEFLG_SIDEREAL = 65536
}

enum SweSideReal: Int {
    case SE_SIDM_FAGAN_BRADLEY = 0
    case SE_SIDM_LAHIRI = 1
    case SE_SIDM_DELUCE = 2
    case SE_SIDM_RAMAN = 3
    case SE_SIDM_USHASHASHI = 4
}

public enum SoftHard: Int {
    case SOFT = 0
    case HARD = 1
}

public enum SpanType: Int {
    case UNIT = 0
    case NEWMOON = 1
    case FULLMOON = 2
    case SOLARINGRESS = 101
    case MOONINGRESS = 102
    case SOLARRETURN = 201
}


public class AstroConst {

}
