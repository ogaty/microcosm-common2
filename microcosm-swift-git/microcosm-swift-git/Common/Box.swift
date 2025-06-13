//
//  Untitled.swift
//  microcosm-swift-git
//
//  Created by 緒形雄二 on 2025/06/13.
//

public class Box
{
    public var index: Int = 0
    public var box: [Int] = []
    
    public func boxInit()
    {
        index = 0
        box = [Int](repeating: 0, count: 72)
    }
    public func boxUpdate(initIndex: Int)
    {
        if (box.count < index || index < 0)
        {
            return;
        }
        index = initIndex
        // 重ならないようにずらしを入れる
        // 1サインに5度単位6個、最大72個までデータが入る
        if (box[index] == 1)
        {
            while (box[index] == 1)
            {
                index = index + 1
                if (index == 72)
                {
                    index = 0
                }
            }
            box[index] = 1;
        }
        else
        {
            box[index] = 1;
        }
    }
}
