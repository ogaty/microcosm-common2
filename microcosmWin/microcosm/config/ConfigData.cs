using microcosm.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace microcosm.config
{
    public enum EProgression
    {
        SECONDARY = 0,
        PRIMARY = 1,
        SOLAR = 2,
        CPS = 3
    }

    public enum EHouseCalc
    {
        PLACIDUS = 0,
        KOCH = 1,
        CAMPANUS = 2,
        EQUAL = 3,
        ZEROARIES = 4,
    }

    public enum ECentric
    {
        GEO_CENTRIC = 0,
        HELIO_CENTRIC = 1
    }

    public enum Esidereal
    {
        TROPICAL = 0,
        SIDEREAL = 1,
        DRACONIC = 2,
    }
    public enum EDecimalDisp
    {
        DECIMAL = 0,
        DEGREE = 1
    }
    public enum EDispPetern
    {
        FULL = 0,
        MINI = 1
    }
    public enum ENodeCalc
    {
        TRUE = 0,
        MEAN = 1
    }

    public enum ELilithCalc
    {
        OSCU = 0,
        MEAN = 1
    }

    public class ConfigData
    {
        // 天文データパス
        [JsonPropertyName("ephepath")]
        public string ephepath;

        // GEO or HERIO
        [JsonPropertyName("centric")]
        public ECentric centric { get; set; }

        // TROPICAL or SIDEREAL
        [JsonPropertyName("sidereal")]
        public Esidereal sidereal { get; set; }

        [JsonPropertyName("defaultPlace")]
        public string defaultPlace { get; set; }

        [JsonPropertyName("lat")]
        public double lat { get; set; }

        [JsonPropertyName("lng")]
        public double lng { get; set; }

        [JsonPropertyName("defaultTimezone")]
        public string defaultTimezone { get; set; }

        [JsonPropertyName("progression")]
        public EProgression progression { get; set; }

        [JsonPropertyName("houseCalc")]
        public EHouseCalc houseCalc { get; set; }

        // 10進、60進
        [JsonPropertyName("decimalDisp")]
        public EDecimalDisp decimalDisp { get; set; }

        // フル表示かミニ表示か
        [JsonPropertyName("dispPattern")]
        public EDispPetern dispPattern { get; set; }

        // ノード計算
        [ JsonPropertyName("nodeCalc")]
        public ENodeCalc nodeCalc { get; set; }

        // リリス計算
        [JsonPropertyName("lilithCalc")]
        public ELilithCalc lilithCalc { get; set; }

        // 獣帯外側幅
        [JsonPropertyName("zodiacOuterWidth")]
        public int zodiacOuterWidth { get; set; }

        // 獣帯幅
        [JsonPropertyName("zodiacWidth")]
        public int zodiacWidth { get; set; }

        // 中心円幅
        [JsonPropertyName("zodiacCenter")]
        public int zodiacCenter { get; set; }

        public ConfigData(string path)
        {
            ephepath = path;
            defaultPlace = "東京都";
            lat = common.CommonData.defaultLat;
            lng = common.CommonData.defaultLng;
            houseCalc = EHouseCalc.PLACIDUS;
            zodiacOuterWidth = 470;
            zodiacWidth = 60;
            zodiacCenter = 380;
            defaultTimezone = "Asia/Tokyo (UTC+09:00)";
            progression = EProgression.SECONDARY;
        }


        public ConfigData()
        {
            string root = Util.root();
            ephepath = root + @"\ephe";
            defaultPlace = "東京都";
            lat = common.CommonData.defaultLat;
            lng = common.CommonData.defaultLng;
            houseCalc = EHouseCalc.PLACIDUS;
            zodiacOuterWidth = 470;
            zodiacWidth = 60;
            zodiacCenter = 380;
            defaultTimezone = "Asia/Tokyo (UTC+09:00)";
            progression = EProgression.SECONDARY;
        }

    }
}
