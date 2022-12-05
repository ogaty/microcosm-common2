using System;
using System.Collections.Generic;

namespace microcosmMac2.Models
{
    public class PlanetData
    {
        // swiss_epheと対応した惑星番号
        public int no { get; set; }

        // 0.0～359.99、絶対値
        public double absolute_position { get; set; }

        // 速度(マイナスなら逆行)
        public double speed { get; set; }

        // アスペクト対象リスト
        public List<AspectInfo> aspects;
        // Pとのアスペクトはここ
        public List<AspectInfo> secondAspects;
        // Tとのアスペクトはここ
        public List<AspectInfo> thirdAspects;

        // 感受点はtrue
        public bool sensitive;

        // 天体を表示するか
        public bool isDisp = true;
        // アスペクトを表示するか
        public bool isAspectDisp = true;

        // PlanetData[1]のaspects1 = P-P
        //public List<AspectInfo> aspects0;
        //public List<AspectInfo> aspects1;
        //public List<AspectInfo> aspects2;
        //public List<AspectInfo> aspects3;
        //public List<AspectInfo> aspects4;
        //public List<AspectInfo> aspects5;
        //public List<AspectInfo> aspects6;

        public PlanetData()
        {
        }
    }
}

