//
//  Addr.swift
//  microcosm-swift-1
//
//  Created by 緒形雄二 on 2024/02/05.
//

import Foundation

public class Addr
{
    public var name: String
    public var lat: Double
    public var lng: Double
    
    public init(name: String, lat: Double, lng: Double) {
        self.name = name
        self.lat = lat
        self.lng = lng
    }
}
