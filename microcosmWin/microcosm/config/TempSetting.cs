using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.config
{
    public class TempSetting
    {


        // 一時的な設定
        // 保存しない
        public enum BandKind
        {
            NATAL = 0,
            PROGRESS = 1,
            TRANSIT = 2,
            COMPOSIT = 3,
            SOLAR_RETURN = 10,
            LUNA_RETURN = 11,
            MERCURY_RETURN = 12,
            VENUS_RETURN = 13,
            MARS_RETURN = 14,
            JUPITER_RETURN = 15,
            SATURN_RETURN = 16,
            URANUS_RETURN = 17,
            NEPTUNE_RETURN = 18,
            PLUTO_RETURN = 19
        }
        public enum HouseDivide
        {
            USER1 = 0,
            EVENT1 = 1,
            PROGRESS = 2
        }

        public int bands = 1;
        public BandKind firstBand;
        public BandKind secondBand;
        public BandKind thirdBand;
        public HouseDivide firstHouseDiv;
        public HouseDivide secondHouseDiv;
        public HouseDivide thirdHouseDiv;
        public double zodiacCenter;

        public TempSetting(ConfigData configData)
        {
            bands = 1;
            firstBand = BandKind.NATAL;
            secondBand = BandKind.PROGRESS;
            thirdBand = BandKind.TRANSIT;

            firstHouseDiv = HouseDivide.USER1;
            secondHouseDiv = HouseDivide.PROGRESS;
            thirdHouseDiv = HouseDivide.EVENT1;

       }
    }
}
