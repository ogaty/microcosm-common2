using microcosm.Aspect;
using microcosm.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.Planet
{
    public class PlanetData
    {
        // swiss_epheと対応した惑星番号
        public int no;
        // 0.0～359.99、絶対値
        public double absolute_position;
        // 速度(マイナスなら逆行)
        public double speed;
        // 感受点はtrue
        public bool sensitive;
        // アスペクト対象リスト
        public List<AspectInfo> aspects;
        // Pとのアスペクトはここ
        public List<AspectInfo> secondAspects;
        // Tとのアスペクトはここ
        public List<AspectInfo> thirdAspects;

        // 天体を表示するか
        public bool isDisp = true;
        // アスペクトを表示するか
        public bool isAspectDisp = true;


        public PlanetData()
        {
            absolute_position = 0.0;
            speed = 0.0;
            no = 0;
        }

        public PlanetData(int kind)
        {
            switch (kind)
            {
                case CommonData.ZODIAC_SUN:
                    no = 0;
                    absolute_position = 0.0;
                    speed = 0.0;
                    sensitive = false;
                    break;
                case CommonData.ZODIAC_MOON:
                    no = 1;
                    absolute_position = 0.0;
                    speed = 0.0;
                    sensitive = false;
                    break;
                case CommonData.ZODIAC_MERCURY:
                    no = 2;
                    absolute_position = 0.0;
                    speed = 0.0;
                    sensitive = false;
                    break;
                case CommonData.ZODIAC_VENUS:
                    no = 3;
                    absolute_position = 0.0;
                    speed = 0.0;
                    sensitive = false;
                    break;
                case CommonData.ZODIAC_MARS:
                    no = 4;
                    absolute_position = 0.0;
                    speed = 0.0;
                    sensitive = false;
                    break;
                case CommonData.ZODIAC_JUPITER:
                    no = 5;
                    absolute_position = 0.0;
                    speed = 0.0;
                    sensitive = false;
                    break;
                case CommonData.ZODIAC_SATURN:
                    no = 6;
                    absolute_position = 0.0;
                    speed = 0.0;
                    sensitive = false;
                    break;
                case CommonData.ZODIAC_URANUS:
                    no = 7;
                    absolute_position = 0.0;
                    speed = 0.0;
                    sensitive = false;
                    break;
                case CommonData.ZODIAC_NEPTUNE:
                    no = 8;
                    absolute_position = 0.0;
                    speed = 0.0;
                    sensitive = false;
                    break;
                case CommonData.ZODIAC_PLUTO:
                    no = 8;
                    absolute_position = 0.0;
                    speed = 0.0;
                    sensitive = false;
                    break;
                case CommonData.ZODIAC_ASC:
                    no = 0;
                    absolute_position = 0.0;
                    speed = 0.0;
                    sensitive = true;
                    break;
                case CommonData.ZODIAC_MC:
                    no = 1;
                    absolute_position = 0.0;
                    speed = 0.0;
                    sensitive = true;
                    break;
                case CommonData.ZODIAC_DH_TRUENODE:
                    no = 1;
                    absolute_position = 0.0;
                    speed = 0.0;
                    sensitive = true;
                    break;
                case 10003:
                    no = 1;
                    absolute_position = 0.0;
                    speed = 0.0;
                    sensitive = true;
                    break;

            }
        }

    }
}
