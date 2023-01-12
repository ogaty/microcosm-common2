using System;
using System.Text.Json.Serialization;

namespace microcosmMac2.Models
{
    public enum EShortCut
    {
        Noop = 999,
        ChagngeSetting0 = 0,
        ChagngeSetting1 = 1,
        ChagngeSetting2 = 2,
        ChagngeSetting3 = 3,
        ChagngeSetting4 = 4,
        ChagngeSetting5 = 5,
        ChagngeSetting6 = 6,
        ChagngeSetting7 = 7,
        ChagngeSetting8 = 8,
        ChagngeSetting9 = 9,
        Ring1U1 = 10,
        Ring1U2 = 11,
        Ring1E1 = 12,
        Ring1E2 = 13,
        Ring2UU = 14,
        Ring2UE = 15,
        Ring3NPT = 16,
        Ring1Current = 17,
        Plus1Second = 30,
        Minus1Second = 31,
        Plus1Minute = 32,
        Minus1Minute = 33,
        Plus1Hour = 34,
        Minus1Hour = 35,
        Plus12Hour = 36,
        Minus12Hour = 37,
        Plus1Day = 38,
        Minus1Day = 39,
        Plus7Day = 40,
        Minus7Day = 41,
        Plus30Day = 42,
        Minus30Day = 43,
        Plus365Day = 44,
        Minus365Day = 45,
        InvisibleAllAspect = 60,
        VisibleAllAspect = 61,
        Visible11 = 62,
        Visible12 = 63,
        Visible13 = 64,
        Visible22 = 65,
        Visible23 = 66,
        Visible33 = 67,
        InVisible11 = 68,
        InVisible12 = 69,
        InVisible13 = 70,
        InVisible22 = 71,
        InVisible23 = 72,
        InVisible33 = 73,
    }

    public class ShortCut
    {
        [JsonPropertyName("ctrlH")]
        public EShortCut ctrlH { get; set; }

        [JsonPropertyName("ctrlJ")]
        public EShortCut ctrlJ { get; set; }

        [JsonPropertyName("ctrlK")]
        public EShortCut ctrlK { get; set; }

        [JsonPropertyName("ctrlL")]
        public EShortCut ctrlL { get; set; }

        [JsonPropertyName("ctrlN")]
        public EShortCut ctrlN { get; set; }

        [JsonPropertyName("ctrlM")]
        public EShortCut ctrlM { get; set; }

        [JsonPropertyName("ctrlY")]
        public EShortCut ctrlY { get; set; }

        [JsonPropertyName("ctrlU")]
        public EShortCut ctrlU { get; set; }

        [JsonPropertyName("ctrlI")]
        public EShortCut ctrlI { get; set; }

        [JsonPropertyName("ctrlO")]
        public EShortCut ctrlO { get; set; }

        [JsonPropertyName("ctrlP")]
        public EShortCut ctrlP { get; set; }

        [JsonPropertyName("ctrlComma")]
        public EShortCut ctrlComma { get; set; }

        [JsonPropertyName("ctrlDot")]
        public EShortCut ctrlDot { get; set; }

        [JsonPropertyName("ctrlOpenBracket")]
        public EShortCut ctrlOpenBracket { get; set; }

        [JsonPropertyName("ctrlCloseBracket")]
        public EShortCut ctrlCloseBracket { get; set; }

        [JsonPropertyName("ctrl1")]
        public EShortCut ctrl1 { get; set; }

        [JsonPropertyName("ctrl2")]
        public EShortCut ctrl2 { get; set; }

        [JsonPropertyName("ctrl3")]
        public EShortCut ctrl3 { get; set; }

        [JsonPropertyName("ctrl4")]
        public EShortCut ctrl4 { get; set; }

        [JsonPropertyName("ctrl5")]
        public EShortCut ctrl5 { get; set; }

        [JsonPropertyName("ctrl6")]
        public EShortCut ctrl6 { get; set; }

        [JsonPropertyName("ctrl7")]
        public EShortCut ctrl7 { get; set; }

        [JsonPropertyName("ctrl8")]
        public EShortCut ctrl8 { get; set; }

        [JsonPropertyName("ctrl9")]
        public EShortCut ctrl9 { get; set; }

        [JsonPropertyName("ctrl0")]
        public EShortCut ctrl0 { get; set; }

        [JsonPropertyName("F1")]
        public EShortCut F1 { get; set; }

        [JsonPropertyName("F2")]
        public EShortCut F2 { get; set; }

        [JsonPropertyName("F3")]
        public EShortCut F3 { get; set; }

        [JsonPropertyName("F4")]
        public EShortCut F4 { get; set; }

        [JsonPropertyName("F5")]
        public EShortCut F5 { get; set; }

        [JsonPropertyName("F6")]
        public EShortCut F6 { get; set; }

        [JsonPropertyName("F7")]
        public EShortCut F7 { get; set; }

        [JsonPropertyName("F8")]
        public EShortCut F8 { get; set; }

        [JsonPropertyName("F9")]
        public EShortCut F9 { get; set; }

        [JsonPropertyName("F10")]
        public EShortCut F10 { get; set; }

        public ShortCut()
        {
            ctrlH = EShortCut.Minus30Day;
            ctrlJ = EShortCut.Minus1Day;
            ctrlK = EShortCut.Plus1Day;
            ctrlL = EShortCut.Plus30Day;
            ctrlN = EShortCut.Ring1U1;
            ctrlM = EShortCut.Ring3NPT;
            ctrlY = EShortCut.Noop;
            ctrlU = EShortCut.Noop;
            ctrlI = EShortCut.Noop;
            ctrlO = EShortCut.Noop;
            ctrlP = EShortCut.Noop;
            ctrlComma = EShortCut.Noop;
            ctrlDot = EShortCut.Noop;
            ctrlOpenBracket = EShortCut.Noop;
            ctrlCloseBracket = EShortCut.Noop;
            ctrl0 = EShortCut.ChagngeSetting0;
            ctrl1 = EShortCut.ChagngeSetting1;
            ctrl2 = EShortCut.ChagngeSetting2;
            ctrl3 = EShortCut.ChagngeSetting3;
            ctrl4 = EShortCut.ChagngeSetting4;
            ctrl5 = EShortCut.ChagngeSetting5;
            ctrl6 = EShortCut.ChagngeSetting6;
            ctrl7 = EShortCut.ChagngeSetting7;
            ctrl8 = EShortCut.ChagngeSetting8;
            ctrl9 = EShortCut.ChagngeSetting9;
            F1 = EShortCut.Noop;
            F2 = EShortCut.Noop;
            F3 = EShortCut.Noop;
            F4 = EShortCut.Noop;
            F5 = EShortCut.Noop;
            F6 = EShortCut.Noop;
            F7 = EShortCut.Noop;
            F8 = EShortCut.Noop;
            F9 = EShortCut.Noop;
            F10 = EShortCut.Noop;
        }
    }
}

