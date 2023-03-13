using System;
using System.Text.Json.Serialization;

namespace microcosmMac2.Config
{
    public enum ECentric
    {
        GEO_CENTRIC = 0,
        HELIO_CENTRIC = 1,
        DRACONIC = 2,
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
    public enum EDispPettern
    {
        FULL = 0,
        MINI = 1
    }
    public enum EColor29
    {
        NOCHANGE = 0,
        CHANGE = 1
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

    /// <summary>
    /// 共通設定
    /// </summary>
    public class ConfigData
    {
        // 天文データパス
        [JsonPropertyName("ephepath")]
        public string ephepath { get; set; }

        // GEO or HERIO
        [JsonPropertyName("centric")]
        public ECentric centric { get; set; }

        // TROPICAL or SIDEREAL
        [JsonPropertyName("sidereal")]
        public Esidereal sidereal { get; set; }

        // 現在地
        [JsonPropertyName("defaultPlace")]
        public string defaultPlace { get; set; }

        // 緯度
        [JsonPropertyName("lat")]
        public double lat { get; set; }
        // 経度
        [JsonPropertyName("lng")]
        public double lng { get; set; }

        // ノード計算
        [JsonPropertyName("nodeCalc")]
        public ENodeCalc nodeCalc { get; set; }

        // リリス計算
        [JsonPropertyName("lilithCalc")]
        public ELilithCalc lilithCalc { get; set; }

        // デフォルトタイムゾーン
        [JsonPropertyName("defaultTimezoneStr")]
        public string defaultTimezoneStr { get; set; }

        // デフォルトタイムゾーン
        [JsonPropertyName("defaultTimezone")]
        public double defaultTimezone { get; set; }

        // デフォルト表示
        public int defaultBands { get; set; }

        // 獣帯外側幅
        public int zodiacOuterWidth { get; set; }

        // 獣帯幅
        public int zodiacWidth { get; set; }

        // 中心円
        public int zodiacCenter { get; set; }

        // 10進、60進
        [JsonPropertyName("decimalDisp")]
        public EDecimalDisp decimalDisp { get; set; }

        // フル表示かミニ表示か
        // old
        public int dispPattern { get; set; }

        // new
        [JsonPropertyName("dispPattern2")]
        public EDispPettern dispPattern2 { get; set; }

        // 29度で色変える
        public EColor29 color29 { get; set; }

        public int colorChange { get; set; }

        public ConfigData(string path)
        {
            ephepath = path;
            setDefault();
        }

        public ConfigData()
        {
            ephepath = "./";
            setDefault();
        }

        public void setDefault()
        {
            centric = ECentric.GEO_CENTRIC;
            sidereal = Esidereal.TROPICAL;
            sidereal = Esidereal.TROPICAL;
            defaultTimezone = 9.0;
            defaultTimezoneStr = "Asia/Tokyo (+9:00)";
            defaultPlace = "東京都";
            lat = Common.CommonData.defaultLat;
            lng = Common.CommonData.defaultLng;
            decimalDisp = EDecimalDisp.DECIMAL;
        }
    }
}

