//
//  CommonData.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/02/05.
//

import Foundation
import CoreGraphics

enum ETargetUser: Int {
    case USER1 = 1
    case USER2 = 2
    case EVENT1 = 3
    case EVENT2 = 4
}
enum EBandKind: Int {
    case NATAL    = 0
    case PROGRESS = 1
    case TRANSIT  = 2
    case COMPOSIT = 3
}

/// 今後作り直すとしてもずっと使い回すであろう処理
public class CommonData
{
    
    /// 天体番号から対応文字を返す(microcosm.otf)
    /// 別フォントでgetPlanetSymbolを作っていたからgetPlanetSymbol2になっている
    /// Dictionary形式のほうがいい気もするけど、どうせ初期化時に同じようなこと書くからなぁ
    ///
    /// - Parameter n: 天体番号
    /// - Returns: フォント文字
    public static func getPlanetSymbol2(n: Int) -> String
    {
        switch (n) {
        case EPlanets.SUN.rawValue:
            return "A"
        case EPlanets.MOON.rawValue:
            return "B"
        // Cは逆向きmoon
        case EPlanets.MERCURY.rawValue:
            return "D"
        case EPlanets.VENUS.rawValue:
            return "E"
        case EPlanets.MARS.rawValue:
            return "F"
        case EPlanets.JUPITER.rawValue:
            return "G"
        case EPlanets.SATURN.rawValue:
            return "H"
        case EPlanets.URANUS.rawValue:
            return "I"
        case EPlanets.NEPTUNE.rawValue:
            return "J"
        case EPlanets.PLUTO.rawValue:
            return "K"
        case EPlanets.DH_TRUENODE.rawValue:
            return "O"
        case EPlanets.DH_MEANNODE.rawValue:
            return "O"
        case EPlanets.DT_TRUE.rawValue:
            return "P"
        case EPlanets.DT_MEAN.rawValue:
            return "P"
        case EPlanets.EARTH.rawValue:
            return "S"
        case EPlanets.CHIRON.rawValue:
            return "Q"
        case EPlanets.OSCU_LILITH.rawValue:
            return "R"
        case EPlanets.MEAN_LILITH.rawValue:
            return "R"
        case EPlanets.CERES.rawValue:
            return "T"
        case EPlanets.PALLAS.rawValue:
            return "U"
        case EPlanets.JUNO.rawValue:
            return "V"
        case EPlanets.VESTA.rawValue:
            return "W"
        case EPlanets.ASC.rawValue:
            return "M"
        case EPlanets.MC.rawValue:
            return "N"
        case EPlanets.VT.rawValue:
            return "X"
        case EPlanets.POF.rawValue:
            return "Y"
        default:
            return ""
        }
    }
    
    /// 天体番号から対応文字を返す(microcosm.otf)
    /// getPlanetSymbol2と返す文字が違うだけ
    /// Dictionary形式のほうがいい気もするけど、どうせ初期化時に同じようなこと書くからなぁ
    /// ヘッドとテイルはオプションでノースノードにしても良さげ
    /// キロン/カイロンとかセレス/ケレスとか
    ///
    /// - Parameter n: 天体番号
    /// - Returns: 太陽とか
    public static func getPlanetText2(n: Int) -> String
    {
        switch (n) {
        case EPlanets.SUN.rawValue:
            return "太陽"
        case EPlanets.MOON.rawValue:
            return "月"
        case EPlanets.MERCURY.rawValue:
            return "水星"
        case EPlanets.VENUS.rawValue:
            return "金星"
        case EPlanets.MARS.rawValue:
            return "火星"
        case EPlanets.JUPITER.rawValue:
            return "木星"
        case EPlanets.SATURN.rawValue:
            return "土星"
        case EPlanets.URANUS.rawValue:
            return "天王星"
        case EPlanets.NEPTUNE.rawValue:
            return "海王星"
        case EPlanets.PLUTO.rawValue:
            return "冥王星"
        case EPlanets.DH_TRUENODE.rawValue:
            // if (xxx)
            // return "ノースノード"
            return "ドラゴンヘッド(true)"
        case EPlanets.DH_MEANNODE.rawValue:
            return "ドラゴンヘッド(mean)"
        case EPlanets.DT_TRUE.rawValue:
            return "ドラゴンテイル(true)"
        case EPlanets.DT_MEAN.rawValue:
            return "ドラゴンテイル(mean)"
        case EPlanets.EARTH.rawValue:
            return "地球"
        case EPlanets.CHIRON.rawValue:
            return "キロン"
        case EPlanets.OSCU_LILITH.rawValue:
            return "リリス(true)"
        case EPlanets.MEAN_LILITH.rawValue:
            return "リリス(mean)"
        case EPlanets.CERES.rawValue:
            return "セレス"
        case EPlanets.PALLAS.rawValue:
            return "パラス"
        case EPlanets.JUNO.rawValue:
            return "ジュノー"
        case EPlanets.VESTA.rawValue:
            return "ベスタ"
        case EPlanets.ASC.rawValue:
            return "ASC"
        case EPlanets.MC.rawValue:
            return "MC"
        case EPlanets.VT.rawValue:
            return "VT"
        case EPlanets.POF.rawValue:
            return "PoF"
        default:
            return ""
        }
    }
    

    /// サイン文字を返す
    /// - Parameter n: 星座番号
    /// - Returns: ♈〜♓
    public static func getSignSymbol(n: Int) -> String
    {
        switch (n) {
        case ESign.ARIES.rawValue:
            return "a"
        case ESign.TAURUS.rawValue:
            return "b"
        case ESign.GEMINI.rawValue:
            return "c"
        case ESign.CANCER.rawValue:
            return "d"
        case ESign.LEO.rawValue:
            return "e"
        case ESign.VIRGO.rawValue:
            return "f"
        case ESign.LIBRA.rawValue:
            return "g"
        case ESign.SCORPIO.rawValue:
            return "h"
        case ESign.SAGITTARIUS.rawValue:
            return "i"
        case ESign.CAPRICORN.rawValue:
            return "j"
        case ESign.AQUARIUS.rawValue:
            return "k"
        case ESign.PISCES.rawValue:
            return "l"
        default:
            return ""
        }
    }
    
    /// サイン文字(日本語)を返す
    /// - Parameter n: 星座番号
    /// - Returns: 羊〜魚
    public static func getSignSymbolJp(n: Int) -> String
    {
        switch (n) {
        case ESign.ARIES.rawValue:
            return "羊"
        case ESign.TAURUS.rawValue:
            return "牛"
        case ESign.GEMINI.rawValue:
            return "双"
        case ESign.CANCER.rawValue:
            return "蟹"
        case ESign.LEO.rawValue:
            return "獅"
        case ESign.VIRGO.rawValue:
            return "乙"
        case ESign.LIBRA.rawValue:
            return "天"
        case ESign.SCORPIO.rawValue:
            return "蠍"
        case ESign.SAGITTARIUS.rawValue:
            return "射"
        case ESign.CAPRICORN.rawValue:
            return "山"
        case ESign.AQUARIUS.rawValue:
            return "水"
        case ESign.PISCES.rawValue:
            return "魚"
        default:
            return ""
        }
    }
    
    /// 絶対度数から星座を返す
    /// - Parameter n: 絶対度数
    /// - Returns: ♈〜♓
    public static func getSignSymbolByAbsoluteDegree(n: Double) -> String
    {
        return getSignSymbol(n: Int(floor(n / 30.0)))
    }

    /// 絶対度数から星座を返す
    /// - Parameter n: 絶対度数
    /// - Returns: 羊〜魚
    public static func getSignSymbolJpByAbsoluteDegree(n: Double) -> String
    {
        return getSignSymbolJp(n: Int(floor(n / 30.0)))
    }

    /// アスペクト文字を返す
    /// コンジャンクションは出さなくていい場合も多いので何も返さない
    /// 返したい場合はgetAspectSymbol2で
    /// - Parameter n: アスペクト番号
    /// - Returns: △とか□とか
    public static func getAspectSymbol(n: Int) -> String
    {
        switch (n) {
        case EAspect.CONJUNCTION.rawValue:
            return ""
        case EAspect.OPPOSITION.rawValue:
            return "B"
        case EAspect.TRINE.rawValue:
            return "C"
        case EAspect.SQUARE.rawValue:
            return "D"
        case EAspect.SEXTILE.rawValue:
            return "E"
        case EAspect.INCONJUNCT.rawValue:
            return "H"
        case EAspect.SESQUIQUADRATE.rawValue:
            return "I";
        case EAspect.SEMISQUARE.rawValue:
            return "G";
        case EAspect.SEMISEXTILE.rawValue:
            return "F";
        case EAspect.QUINTILE.rawValue:
            return "J";
        case EAspect.BIQUINTILE.rawValue:
            return "K";
        case EAspect.SEMIQINTILE.rawValue:
            return "L";
        case EAspect.NOVILE.rawValue:
            return "M";
        case EAspect.SEPTILE.rawValue:
            return "N";
        case EAspect.QUINDECILE.rawValue:
            return "O";
        default:
            return ""
        }
    }
    
    /// こちらはCONJUNCTIONもちゃんと出す
    /// - Parameter n: アスペクト番号
    /// - Returns: アスペクトシンボル
    public static func getAspectSymbol2(n: Int) -> String
    {
        if (n == EAspect.CONJUNCTION.rawValue) {
            return "A"
        }
        
        return getAspectSymbol(n: n)
    }
    
    public static func getPlanetColor(n: Int) -> CGColor
    {
        switch (n) {
        case EPlanets.SUN.rawValue:
            // SKColors.Olive
            return CGColor(red: 128, green: 128, blue: 0, alpha: 1)
        case EPlanets.MOON.rawValue:
            // SKColors.DarkGoldenrod
            return CGColor(red: 184, green: 134, blue: 11, alpha: 1)
        case EPlanets.MERCURY.rawValue:
            // SKColors.Purple
            return CGColor(red: 128, green: 0, blue: 128, alpha: 1)
        case EPlanets.VENUS.rawValue:
            // SKColors.Green
            return CGColor(red: 0, green: 80, blue: 0, alpha: 1)
        case EPlanets.MARS.rawValue:
            // SKColors.Red
            return CGColor(red: 255, green: 0, blue: 0, alpha: 1)
        case EPlanets.JUPITER.rawValue:
            // SKColors.Maroon
            return CGColor(red: 128, green: 0, blue: 0, alpha: 1)
        case EPlanets.SATURN.rawValue:
            // SKColors.DimGray
            return CGColor(red: 105, green: 105, blue: 105, alpha: 1)
        case EPlanets.URANUS.rawValue:
            // SKColors.DarkTurquoise
            return CGColor(red: 0, green: 206, blue: 209, alpha: 1)
        case EPlanets.NEPTUNE.rawValue:
            // SKColors.DodgerBlue
            return CGColor(red: 30, green: 144, blue: 255, alpha: 1)
        case EPlanets.PLUTO.rawValue:
            // SKColors.DeepPink
            return CGColor(red: 255, green: 20, blue: 147, alpha: 1)
        case EPlanets.EARTH.rawValue:
            // SKColors.SkyBlue
            return CGColor(red: 135, green: 206, blue: 235, alpha: 1)
        case EPlanets.CHIRON.rawValue:
            // todo
            // SKColors.SkyBlue
            return CGColor(red: 135, green: 206, blue: 235, alpha: 1)
        case EPlanets.DH_TRUENODE.rawValue:
            // SKColors.DarkCyan
            return CGColor(red: 0, green: 139, blue: 139, alpha: 1)
        case EPlanets.DH_MEANNODE.rawValue:
            // SKColors.DarkCyan
            return CGColor(red: 0, green: 139, blue: 139, alpha: 1)
        case EPlanets.DT_TRUE.rawValue:
            // SKColors.DarkCyan
            return CGColor(red: 0, green: 139, blue: 139, alpha: 1)
        case EPlanets.DT_MEAN.rawValue:
            // SKColors.DarkCyan
            return CGColor(red: 0, green: 139, blue: 139, alpha: 1)
        case EPlanets.MEAN_LILITH.rawValue:
            // SKColors.MediumSeaGreen
            return CGColor(red: 60, green: 179, blue: 113, alpha: 1)
        case EPlanets.OSCU_LILITH.rawValue:
            // SKColors.MediumSeaGreen
            return CGColor(red: 60, green: 179, blue: 113, alpha: 1)
        default:
            return CGColor(red: 0, green: 0, blue: 0, alpha: 1)
        }
    }
    
    public static func getSignColor(n: Int) -> CGColor
    {
        switch (n) {
        case ESign.ARIES.rawValue:
            // SKColors.OrangeRed
            return CGColor(red: 255, green: 69, blue: 0, alpha: 1)
        case ESign.TAURUS.rawValue:
            // SKColors.Goldenrod
            return CGColor(red: 218, green: 165, blue: 32, alpha: 1)
        case ESign.GEMINI.rawValue:
            // SKColors.MediumSeaGreen
            return CGColor(red: 60, green: 179, blue: 113, alpha: 1)
        case ESign.CANCER.rawValue:
            // SKColors.SteelBlue
            return CGColor(red: 70, green: 130, blue: 180, alpha: 1)
        case ESign.LEO.rawValue:
            // SKColors.Crimson
            return CGColor(red: 220, green: 20, blue: 60, alpha: 1)
        case ESign.VIRGO.rawValue:
            // SKColors.Maroon
            return CGColor(red: 128, green: 0, blue: 0, alpha: 1)
        case ESign.LIBRA.rawValue:
            // SKColors.Teal
            return CGColor(red: 0, green: 128, blue: 128, alpha: 1)
        case ESign.SCORPIO.rawValue:
            // SKColors.CornflowerBlue
            return CGColor(red: 100, green: 149, blue: 237, alpha: 1)
        case ESign.SAGITTARIUS.rawValue:
            // SKColors.DeepPink
            return CGColor(red: 255, green: 20, blue: 147, alpha: 1)
        case ESign.CAPRICORN.rawValue:
            // SKColors.SaddleBrown
            return CGColor(red: 139, green: 69, blue: 19, alpha: 1)
        case ESign.AQUARIUS.rawValue:
            // SKColors.CadetBlue
            return CGColor(red: 95, green: 158, blue: 160, alpha: 1)
        case ESign.PISCES.rawValue:
            // SKColors.DodgerBlue
            return CGColor(red: 30, green: 144, blue: 255, alpha: 1)
        default:
            return CGColor(red: 0, green: 0, blue: 0, alpha: 1)
        }
    }
    
    /// 10進を60進に直す
    /// 80を渡されたら45が返る
    /// 実際に整数部が来ることはなくて小数部を100倍したものが渡される(そして100で割って表示)
    /// - Parameter deci: 10進数の値
    /// - Returns: 60進数の値
    public static func DecimalToHex(deci: Double)-> Double
    {
        let ret = deci / 100 * 60
        return ret
    }
}
