//
//  PlanetData.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/07/17.
//

import Foundation

public class PlanetData
{
    // swiss_epheと対応した惑星番号
    public var no: Int
    // 0.0～359.99、絶対値
    public var absolute_position: Double
    // 速度(マイナスなら逆行)
    public var speed: Double;
    // 感受点はtrue
    public var sensitive: Bool
    // アスペクト対象リスト
    public var aspects: [AspectInfo] = [AspectInfo]()
    // Pとのアスペクトはここ
    public var secondAspects: [AspectInfo] = [AspectInfo]()
    // Tとのアスペクトはここ
    public var thirdAspects: [AspectInfo] = [AspectInfo]()

    // 天体を表示するか
    public var isDisp: Bool = true
    // アスペクトを表示するか
    public var isAspectDisp: Bool = true

    init()
    {
        no = -1
        absolute_position = 0
        speed = 0
        sensitive = false
    }
}
