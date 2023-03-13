using System;
using microcosmMac2.Config;
using microcosmMac2.Models;
using SkiaSharp;

namespace microcosmMac2.Common
{
    public enum OrbKind
    {
        SUN_HARD_1ST = 0,
        SUN_SOFT_1ST = 1,
        SUN_HARD_2ND = 2,
        SUN_SOFT_2ND = 3,
        SUN_HARD_150 = 4,
        SUN_SOFT_150 = 5,
        MOON_HARD_1ST = 6,
        MOON_SOFT_1ST = 7,
        MOON_HARD_2ND = 8,
        MOON_SOFT_2ND = 9,
        MOON_HARD_150 = 10,
        MOON_SOFT_150 = 11,
        OTHER_HARD_1ST = 12,
        OTHER_SOFT_1ST = 13,
        OTHER_HARD_2ND = 14,
        OTHER_SOFT_2ND = 15,
        OTHER_HARD_150 = 16,
        OTHER_SOFT_150 = 17
    }

    public enum ETargetUser
    {
        USER1 = 0,
        USER2 = 1,
        EVENT1 = 2,
        EVENT2 = 3,
    }

    public enum SpanType
    {
        UNIT = 0,
        NEWMOON = 1,
        FULLMOON = 2,
        SOLARINGRESS = 11,
        MOONINGRESS = 12,
        SOLARRETURN = 21,
    }

    public enum mainChart
    {
        CHART = 0,
        GRID = 1,
    }

    public static class CommonData
    {
        // AstroCalcで回すループ番号
        public static int[] target_numbers = {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            11, 13, 14, 15,
            17, 18, 19, 20,
            100377, 146108, 146199, 146472
        };

        public const double TIMEZONE_JST = 9.0;
        public const double TIMEZONE_GMT = 0.0;

        public const int ZODIAC_SUN = 0;
        public const int ZODIAC_MOON = 1;
        public const int ZODIAC_MERCURY = 2;
        public const int ZODIAC_VENUS = 3;
        public const int ZODIAC_MARS = 4;
        public const int ZODIAC_JUPITER = 5;
        public const int ZODIAC_SATURN = 6;
        public const int ZODIAC_URANUS = 7;
        public const int ZODIAC_NEPTUNE = 8;
        public const int ZODIAC_PLUTO = 9;
        public const int ZODIAC_DH_MEANNODE = 10;
        public const int ZODIAC_DH_TRUENODE = 11;
        public const int ZODIAC_DT_MEAN = 101;
        public const int ZODIAC_DT_TRUE = 102;
        public const int ZODIAC_MEAN_LILITH = 12; // 小惑星のリリス(1181)と混同しないこと
        public const int ZODIAC_OSCU_LILITH = 13; // 小惑星のリリス(1181)と混同しないこと
        public const int ZODIAC_EARTH = 14;
        public const int ZODIAC_CHIRON = 15;
        public const int ZODIAC_POLUS = 16;
        public const int ZODIAC_CERES = 17;
        public const int ZODIAC_PALLAS = 18;
        public const int ZODIAC_JUNO = 19;
        public const int ZODIAC_VESTA = 20;
        public const int ZODIAC_ASC = 10000;
        public const int ZODIAC_MC = 10001;
        public const int ZODIAC_ERIS = 136199;


        public const int ZODIAC_SEDNA = 90377;
        public const int ZODIAC_HAUMEA = 136108;
        public const int ZODIAC_MAKEMAKE = 136472;

        public const int ZODIAC_VT = 10003;
        public const int ZODIAC_POF = -1;

        public const int SIGN_ARIES = 0;
        public const int SIGN_TAURUS = 1;
        public const int SIGN_GEMINI = 2;
        public const int SIGN_CANCER = 3;
        public const int SIGN_LEO = 4;
        public const int SIGN_VIRGO = 5;
        public const int SIGN_LIBRA = 6;
        public const int SIGN_SCORPIO = 7;
        public const int SIGN_SAGITTARIUS = 8;
        public const int SIGN_CAPRICORN = 9;
        public const int SIGN_AQUARIUS = 10;
        public const int SIGN_PISCES = 11;


        public static double defaultLat = 35.670587;
        public static double defaultLng = 139.772003;
        public static string defaultPlace = "東京都";

        /// <summary>
        /// 番号を引数に天体のシンボルを返す(AstroDotBasic.ttf)
        /// </summary>
        /// <param name="number">天体番号</param>
        /// <returns></returns>
        public static string getPlanetSymbol(int number)
        {
            switch (number)
            {
                case ZODIAC_SUN:
                    return "A";
                case ZODIAC_MOON:
                    return "B";
                case ZODIAC_MERCURY:
                    return "C";
                case ZODIAC_VENUS:
                    return "D";
                case ZODIAC_MARS:
                    return "E";
                case ZODIAC_JUPITER:
                    return "F";
                case ZODIAC_SATURN:
                    return "G";
                case ZODIAC_URANUS:
                    return "H";
                case ZODIAC_NEPTUNE:
                    return "I";
                case ZODIAC_PLUTO:
                    return "J";
                case ZODIAC_DH_TRUENODE:
                    return "L";
                case ZODIAC_DH_MEANNODE:
                    return "L";
                case ZODIAC_DT_TRUE:
                    return "M";
                case ZODIAC_DT_MEAN:
                    return "M";
                case ZODIAC_EARTH:
                    return "O";
                case ZODIAC_CHIRON:
                    return "U";
                case ZODIAC_OSCU_LILITH:
                    return "T";
                case ZODIAC_MEAN_LILITH:
                    return "T";
                case ZODIAC_CERES:
                    return "V";
                case ZODIAC_PALLAS:
                    return "W";
                case ZODIAC_JUNO:
                    return "X";
                case ZODIAC_VESTA:
                    return "Y";
                case ZODIAC_ERIS:
                    return "E";
                case ZODIAC_SEDNA:
                    return "S";
                case ZODIAC_HAUMEA:
                    return "H";
                case ZODIAC_MAKEMAKE:
                    return "M";
                case ZODIAC_ASC:
                    return "P";
                case ZODIAC_MC:
                    return "Q";
            }
            return "";
        }

        /// <summary>
        /// 番号を引数に天体のシンボルを返す(microcosm.otf)
        /// </summary>
        /// <param name="number">天体番号</param>
        /// <returns></returns>
        public static string getPlanetSymbol2(int number)
        {
            switch (number)
            {
                case ZODIAC_SUN:
                    return "A";
                case ZODIAC_MOON:
                    return "B";
                case ZODIAC_MERCURY:
                    return "D";
                case ZODIAC_VENUS:
                    return "E";
                case ZODIAC_MARS:
                    return "F";
                case ZODIAC_JUPITER:
                    return "G";
                case ZODIAC_SATURN:
                    return "H";
                case ZODIAC_URANUS:
                    return "I";
                case ZODIAC_NEPTUNE:
                    return "J";
                case ZODIAC_PLUTO:
                    return "K";
                case ZODIAC_DH_TRUENODE:
                    return "O";
                case ZODIAC_DH_MEANNODE:
                    return "O";
                case ZODIAC_DT_TRUE:
                    return "P";
                case ZODIAC_DT_MEAN:
                    return "P";
                case ZODIAC_EARTH:
                    return "S";
                case ZODIAC_CHIRON:
                    return "Q";
                case ZODIAC_OSCU_LILITH:
                    return "R";
                case ZODIAC_MEAN_LILITH:
                    return "R";
                case ZODIAC_CERES:
                    return "T";
                case ZODIAC_PALLAS:
                    return "U";
                case ZODIAC_JUNO:
                    return "V";
                case ZODIAC_VESTA:
                    return "W";
                case ZODIAC_ASC:
                    return "M";
                case ZODIAC_MC:
                    return "N";
                case ZODIAC_VT:
                    return "X";
                case ZODIAC_POF:
                    return "Y";
            }
            return "";
        }

        /// <summary>
        /// 番号を引数にサインのシンボルを返す
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string getSignSymbol(int number)
        {
            switch (number)
            {
                case SIGN_ARIES:
                    return "a";
                case SIGN_TAURUS:
                    return "b";
                case SIGN_GEMINI:
                    return "c";
                case SIGN_CANCER:
                    return "d";
                case SIGN_LEO:
                    return "e";
                case SIGN_VIRGO:
                    return "f";
                case SIGN_LIBRA:
                    return "g";
                case SIGN_SCORPIO:
                    return "h";
                case SIGN_SAGITTARIUS:
                    return "i";
                case SIGN_CAPRICORN:
                    return "j";
                case SIGN_AQUARIUS:
                    return "k";
                case SIGN_PISCES:
                    return "l";
            }
            return "";
        }

        /// <summary>
        /// 番号を引数にサインのシンボルを返す
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string getSignSymbolJp(int number)
        {
            switch (number)
            {
                case SIGN_ARIES:
                    return "羊";
                case SIGN_TAURUS:
                    return "牛";
                case SIGN_GEMINI:
                    return "双";
                case SIGN_CANCER:
                    return "蟹";
                case SIGN_LEO:
                    return "獅";
                case SIGN_VIRGO:
                    return "乙";
                case SIGN_LIBRA:
                    return "天";
                case SIGN_SCORPIO:
                    return "蠍";
                case SIGN_SAGITTARIUS:
                    return "射";
                case SIGN_CAPRICORN:
                    return "山";
                case SIGN_AQUARIUS:
                    return "水";
                case SIGN_PISCES:
                    return "魚";
            }
            return "";
        }

        // サインテキストを返す(0:♈、11:♓)
        public static string getSignText(double absolute_position)
        {
            return getSignSymbol((int)absolute_position / 30);
        }

        /// <summary>
        /// サインテキストを返す(0:羊、11:魚)
        /// </summary>
        /// <param name="absolute_position"></param>
        /// <returns></returns>
        public static string getSignTextJp(double absolute_position)
        {
            return getSignSymbolJp((int)absolute_position / 30);
        }

        /// <summary>
        /// 番号を引数に天体名を返す
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string getPlanetSymbolText(int number)
        {
            if (number == (int)CommonData.ZODIAC_SUN)
            {
                return "太陽";
            }
            else if (number == (int)CommonData.ZODIAC_MOON)
            {
                return "月";
            }
            else if (number == (int)CommonData.ZODIAC_MERCURY)
            {
                return "水星";
            }
            else if (number == (int)CommonData.ZODIAC_VENUS)
            {
                return "金星";
            }
            else if (number == (int)CommonData.ZODIAC_MARS)
            {
                return "火星";
            }
            else if (number == (int)CommonData.ZODIAC_JUPITER)
            {
                return "木星";
            }
            else if (number == (int)CommonData.ZODIAC_SATURN)
            {
                return "土星";
            }
            else if (number == (int)CommonData.ZODIAC_URANUS)
            {
                return "天王星";
            }
            else if (number == (int)CommonData.ZODIAC_NEPTUNE)
            {
                return "海王星";
            }
            else if (number == (int)CommonData.ZODIAC_PLUTO)
            {
                return "冥王星";
            }
            else if (number == (int)CommonData.ZODIAC_EARTH)
            {
                return "地球";
            }
            else if (number == (int)CommonData.ZODIAC_DH_TRUENODE)
            {
                return "ドラゴンヘッド(true)";
            }
            else if (number == (int)CommonData.ZODIAC_DH_MEANNODE)
            {
                return "ドラゴンヘッド(mean)";
            }
            else if (number == (int)CommonData.ZODIAC_DT_TRUE)
            {
                return "ドラゴンテイル(true)";
            }
            else if (number == (int)CommonData.ZODIAC_DT_MEAN)
            {
                return "ドラゴンテイル(mean)";
            }
            else if (number == (int)CommonData.ZODIAC_OSCU_LILITH)
            {
                return "リリス(true)";
            }
            else if (number == (int)CommonData.ZODIAC_MEAN_LILITH)
            {
                return "リリス(mean)";
            }
            else if (number == (int)CommonData.ZODIAC_CHIRON)
            {
                return "Chiron";
            }
            else if (number == (int)CommonData.ZODIAC_CERES)
            {
                return "セレス";
            }
            else if (number == (int)CommonData.ZODIAC_PALLAS)
            {
                return "パラス";
            }
            else if (number == (int)CommonData.ZODIAC_JUNO)
            {
                return "ジュノー";
            }
            else if (number == (int)CommonData.ZODIAC_VESTA)
            {
                return "ベスタ";
            }
            else if (number == (int)CommonData.ZODIAC_ASC)
            {
                return "ASC";
            }
            else if (number == (int)CommonData.ZODIAC_MC)
            {
                return "MC";
            }
            return "";
        }

        public static string getAspectSymbol(AspectKind kind)
        {
            switch (kind)
            {
                case AspectKind.CONJUNCTION:
                    return "";
                case AspectKind.OPPOSITION:
                    return "B";
                case AspectKind.TRINE:
                    return "C";
                case AspectKind.SQUARE:
                    return "D";
                case AspectKind.SEXTILE:
                    return "E";
                case AspectKind.INCONJUNCT:
                    return "H";
                case AspectKind.SESQUIQUADRATE:
                    return "I";
                case AspectKind.SEMISQUARE:
                    return "G";
                case AspectKind.SEMISEXTILE:
                    return "F";
                case AspectKind.QUINTILE:
                    return "J";
                case AspectKind.BIQUINTILE:
                    return "K";
                case AspectKind.SEMIQINTILE:
                    return "L";
                case AspectKind.NOVILE:
                    return "M";
                case AspectKind.SEPTILE:
                    return "N";
                case AspectKind.QUINDECILE:
                    return "O";
            }
            return "";

        }

        public static string getAspectSymbol2(AspectKind kind)
        {
            if (kind == AspectKind.CONJUNCTION)
            {
                return "A";
            }
            else
            {
                return getAspectSymbol(kind);
            }
        }


        /// <summary>
        /// サインの色をSKColorで返却
        /// </summary>
        /// <returns>The sign color.</returns>
        /// <param name="absolute_position">角度</param>
        public static SKColor getSignColor(double absolute_position)
        {
            switch ((int)absolute_position / 30)
            {
                case 0:
                    // 牡羊座
                    return SKColors.OrangeRed;
                case 1:
                    // 牡牛座
                    return SKColors.Goldenrod;
                case 2:
                    // 双子座
                    return SKColors.MediumSeaGreen;
                case 3:
                    // 蟹座
                    return SKColors.SteelBlue;
                case 4:
                    // 獅子座
                    return SKColors.Crimson;
                case 5:
                    // 乙女座
                    return SKColors.Maroon;
                case 6:
                    // 天秤座
                    return SKColors.Teal;
                case 7:
                    // 蠍座
                    return SKColors.CornflowerBlue;
                case 8:
                    // 射手座
                    return SKColors.DeepPink;
                case 9:
                    // 山羊座
                    return SKColors.SaddleBrown;
                case 10:
                    // 水瓶座
                    return SKColors.CadetBlue;
                case 11:
                    // 魚座
                    return SKColors.DodgerBlue;
                default:
                    break;
            }
            return SKColors.Black;
        }

        /// <summary>
        /// 天体の色をSKColorで返却
        /// </summary>
        /// <returns>The planet color.</returns>
        /// <param name="number">天体番号</param>
        public static SKColor getPlanetColor(int number)
        {
            if (number == (int)CommonData.ZODIAC_SUN)
            {
                return SKColors.Olive;
            }
            else if (number == (int)CommonData.ZODIAC_MOON)
            {
                return SKColors.DarkGoldenrod;
            }
            else if (number == (int)CommonData.ZODIAC_MERCURY)
            {
                return SKColors.Purple;
            }
            else if (number == (int)CommonData.ZODIAC_VENUS)
            {
                return SKColors.Green;
            }
            else if (number == (int)CommonData.ZODIAC_MARS)
            {
                return SKColors.Red;
            }
            else if (number == (int)CommonData.ZODIAC_JUPITER)
            {
                return SKColors.Maroon;
            }
            else if (number == (int)CommonData.ZODIAC_SATURN)
            {
                return SKColors.DimGray;
            }
            else if (number == (int)CommonData.ZODIAC_URANUS)
            {
                return SKColors.DarkTurquoise;
            }
            else if (number == (int)CommonData.ZODIAC_NEPTUNE)
            {
                return SKColors.DodgerBlue;
            }
            else if (number == (int)CommonData.ZODIAC_PLUTO)
            {
                return SKColors.DeepPink;
            }
            else if (number == (int)CommonData.ZODIAC_EARTH)
            {
                return SKColors.SkyBlue;
            }
            else if (number == (int)CommonData.ZODIAC_DH_TRUENODE)
            {
                return SKColors.DarkCyan;
            }
            else if (number == (int)CommonData.ZODIAC_DH_MEANNODE)
            {
                return SKColors.DarkCyan;
            }
            else if (number == (int)CommonData.ZODIAC_DT_TRUE)
            {
                return SKColors.DarkCyan;
            }
            else if (number == (int)CommonData.ZODIAC_DT_MEAN)
            {
                return SKColors.DarkCyan;
            }
            else if (number == (int)CommonData.ZODIAC_OSCU_LILITH)
            {
                return SKColors.MediumSeaGreen;
            }
            else if (number == (int)CommonData.ZODIAC_MEAN_LILITH)
            {
                return SKColors.MediumSeaGreen;
            }
            return SKColors.Black;
        }

        public static string[] GetTimeTables()
        {
            string[] items = new string[69];
            items[0] = "";
            items[1] = "Africa/Johannesburg (+2:00)";
            items[2] = "Africa/Lagos (+1:00)";
            items[3] = "Africa/Windhoek (+1:00)";
            items[4] = "America/Adak (-10:00)";
            items[5] = "America/Anchorage (-9:00)";
            items[6] = "America/Argentina/Buenos_Aires (-3:00)";
            items[7] = "America/Bogota (-5:00)";
            items[8] = "America/Caracas (-4.5:00)";
            items[9] = "America/Chicago (-6:00)";
            items[10] = "America/Denver (-7:00)";
            items[11] = "America/Godthab (-3:00)";
            items[12] = "America/Guatemala (-6:00)";
            items[13] = "America/Halifax (-4:00)";
            items[14] = "America/Los_Angeles (-8:00)";
            items[15] = "America/Montevideo (-3:00)";
            items[16] = "America/New_York (-5:00)";
            items[17] = "America/Noronha (-2:00)";
            items[18] = "America/Phoenix (-7:00)";
            items[19] = "America/Santiago (-4:00)";
            items[20] = "America/Santo_Domingo (-4:00)";
            items[21] = "America/St_Johns (-3.30)";
            items[22] = "Asia/Baghdad (+3:00)";
            items[23] = "Asia/Baku (+4:00)";
            items[24] = "Asia/Beirut (+2:00)";
            items[25] = "Asia/Dhaka (+6:00)";
            items[26] = "Asia/Dubai (+4:00)";
            items[27] = "Asia/Irkutsk (+9:00)";
            items[28] = "Asia/Jakarta (+7:00)";
            items[29] = "Asia/Kabul (+4.30)";
            items[30] = "Asia/Kamchatka (+12:00)";
            items[31] = "Asia/Karachi (+5:00)";
            items[32] = "Asia/Kathmandu (+5:45)";
            items[33] = "Asia/Kolkata (+5:30)";
            items[34] = "Asia/Krasnoyarsk (+8:00)";
            items[35] = "Asia/Omsk (+7:00)";
            items[36] = "Asia/Rangoon (+6:30)";
            items[37] = "Asia/Shanghai (+8:00)";
            items[38] = "Asia/Tehran (+3:30)";
            items[39] = "Asia/Tokyo (+9:00)";
            items[40] = "Asia/Vladivostok (+11:00)";
            items[41] = "Asia/Yakutsk (+10:00)";
            items[42] = "Asia/Yekaterinburg (+6:00)";
            items[43] = "Atlantic/Azores (-1:00)";
            items[44] = "Atlantic/Cape_Verde (-1:00)";
            items[45] = "Australia/Adelaide (+9:30)";
            items[46] = "Australia/Brisbane (+10:00)";
            items[47] = "Australia/Darwin (+9:30)";
            items[48] = "Australia/Eucla (+8:45)";
            items[49] = "Australia/Lord_Howe (+10:30)";
            items[50] = "Australia/Sydney (+10:00)";
            items[51] = "Europe/Berlin (+1:00)";
            items[52] = "Europe/London (0:00)";
            items[53] = "Europe/Moscow (+4:00)";
            items[54] = "Pacific/Apia (+13:00)";
            items[55] = "Pacific/Auckland (+12:00)";
            items[56] = "Pacific/Chatham (+12:45)";
            items[57] = "Pacific/Easter (-6:00)";
            items[58] = "Pacific/Gambier (-9:00)";
            items[59] = "Pacific/Honolulu (-10:00)";
            items[60] = "Pacific/Kiritimati (+14:00)";
            items[61] = "Pacific/Majuro (+12:00)";
            items[62] = "Pacific/Marquesas (-9.30)";
            items[63] = "Pacific/Norfolk (+11:30)";
            items[64] = "Pacific/Noumea (+11:00)";
            items[65] = "Pacific/Pago_Pago (-11:00)";
            items[66] = "Pacific/Pitcairn (-8:00)";
            items[67] = "Pacific/Tongatapu (+13:00)";
            items[68] = "UTC (+0:00)";
            return items;
        }

        /// <summary>
        /// Asia/Tokyoを9.0で返す
        /// </summary>
        /// <param name="timezone"></param>
        /// <returns></returns>
        public static double GetTimezoneValue(string timezone)
        {
            if (timezone.IndexOf("Africa/Johannesburg") > 0) { return +2; }
            else if (timezone.IndexOf("agos") > 0) { return +1; }
            else if (timezone.IndexOf("indhoek") > 0) { return +1; }
            else if (timezone.IndexOf("Adak") > 0) { return -10; }
            else if (timezone.IndexOf("Anchorage") > 0) { return -9; }
            else if (timezone.IndexOf("Argentina/Buenos_Aires") > 0) { return -3; }
            else if (timezone.IndexOf("Bogota") > 0) { return -5; }
            else if (timezone.IndexOf("Caracas") > 0) { return -4.5; }
            else if (timezone.IndexOf("Chicago") > 0) { return -6; }
            else if (timezone.IndexOf("Denver") > 0) { return -7; }
            else if (timezone.IndexOf("Godthab") > 0) { return -3; }
            else if (timezone.IndexOf("Guatemala") > 0) { return -6; }
            else if (timezone.IndexOf("Halifax") > 0) { return -4; }
            else if (timezone.IndexOf("Los_Angeles") > 0) { return -8; }
            else if (timezone.IndexOf("Montevideo") > 0) { return -3; }
            else if (timezone.IndexOf("New_York") > 0) { return -5; }
            else if (timezone.IndexOf("Noronha") > 0) { return -2; }
            else if (timezone.IndexOf("Phoenix") > 0) { return -7; }
            else if (timezone.IndexOf("Santiago") > 0) { return -4; }
            else if (timezone.IndexOf("Santo_Domingo") > 0) { return -4; }
            else if (timezone.IndexOf("ca/St_Johns") > 0) { return -3.5; }
            else if (timezone.IndexOf("Baghdad") > 0) { return +3; }
            else if (timezone.IndexOf("Baku") > 0) { return +4; }
            else if (timezone.IndexOf("Beirut") > 0) { return +2; }
            else if (timezone.IndexOf("Dhaka") > 0) { return +6; }
            else if (timezone.IndexOf("Dubai") > 0) { return +4; }
            else if (timezone.IndexOf("Irkutsk") > 0) { return +9; }
            else if (timezone.IndexOf("Jakarta") > 0) { return +7; }
            else if (timezone.IndexOf("Kabul") > 0) { return +4.5; }
            else if (timezone.IndexOf("Kamchatka") > 0) { return +12; }
            else if (timezone.IndexOf("Karachi") > 0) { return +5; }
            else if (timezone.IndexOf("Kathmandu") > 0) { return +5.75; }
            else if (timezone.IndexOf("Kolkata") > 0) { return +5.5; }
            else if (timezone.IndexOf("Krasnoyarsk") > 0) { return +8; }
            else if (timezone.IndexOf("Omsk") > 0) { return +7; }
            else if (timezone.IndexOf("Rangoon") > 0) { return +6.5; }
            else if (timezone.IndexOf("Shanghai ") > 0) { return +8; }
            else if (timezone.IndexOf("Tehran") > 0) { return +3.5; }
            else if (timezone.IndexOf("Tokyo") > 0) { return +9; }
            else if (timezone.IndexOf("Vladivostok") > 0) { return +11; }
            else if (timezone.IndexOf("Yakutsk") > 0) { return +10; }
            else if (timezone.IndexOf("Yekaterinburg") > 0) { return +6; }
            else if (timezone.IndexOf("Azores") > 0) { return -1; }
            else if (timezone.IndexOf("Cape_Verde") > 0) { return -1; }
            else if (timezone.IndexOf("Adelaide") > 0) { return +9.5; }
            else if (timezone.IndexOf("Brisbane") > 0) { return +10; }
            else if (timezone.IndexOf("Darwin") > 0) { return +9.5; }
            else if (timezone.IndexOf("Eucla") > 0) { return +8.75; }
            else if (timezone.IndexOf("Lord_Howe") > 0) { return +10.5; }
            else if (timezone.IndexOf("Sydney ") > 0) { return +10; }
            else if (timezone.IndexOf("Berlin") > 0) { return +1; }
            else if (timezone.IndexOf("London") > 0) { return 0; }
            else if (timezone.IndexOf("Moscow") > 0) { return +4; }
            else if (timezone.IndexOf("Apia") > 0) { return +13; }
            else if (timezone.IndexOf("Auckland") > 0) { return +12; }
            else if (timezone.IndexOf("Chatham") > 0) { return +12.75; }
            else if (timezone.IndexOf("Easter") > 0) { return -6; }
            else if (timezone.IndexOf("Gambier") > 0) { return -9; }
            else if (timezone.IndexOf("Honolulu") > 0) { return -10; }
            else if (timezone.IndexOf("Kiritimati") > 0) { return +14; }
            else if (timezone.IndexOf("Majuro") > 0) { return +12; }
            else if (timezone.IndexOf("Marquesas") > 0) { return -9.5; }
            else if (timezone.IndexOf("Norfolk") > 0) { return +11.5; }
            else if (timezone.IndexOf("Noumea") > 0) { return +11; }
            else if (timezone.IndexOf("Pago_Pago") > 0) { return -11; }
            else if (timezone.IndexOf("Pitcairn") > 0) { return -8; }
            else if (timezone.IndexOf("Tongatapu") > 0) { return +13; }
            else if (timezone.IndexOf("UTC") > 0) { return 0; }

            return 0.0;
        }

        public static string ProgressionToString(EProgression e)
        {
            switch (e)
            {
                case EProgression.SECONDARY:
                    return "一日一年法";
                case EProgression.SOLARARC:
                    return "ソーラーアーク";
                case EProgression.PRIMARY:
                    return "一度一年法";
                case EProgression.CPS:
                    return "CPS";
            }
            return "";
        }

        public static string HouseCalcToString(EHouseCalc e)
        {
            switch (e)
            {
                case EHouseCalc.PLACIDUS:
                    return "Placidus";
                case EHouseCalc.KOCH:
                    return "Koch";
                case EHouseCalc.CAMPANUS:
                    return "Campanus";
                case EHouseCalc.PORPHYRY:
                    return "Porphyry";
                case EHouseCalc.REGIOMONTANUS:
                    return "Regiomontanus";
                case EHouseCalc.SOLAR:
                    return "Solar";
                case EHouseCalc.SOLARSIGN:
                    return "SolarSign";
                case EHouseCalc.EQUAL:
                    return "Equal";
                case EHouseCalc.ZEROARIES:
                    return "ZeroAries";
            }
            return "";
        }

        public static double DecimalToHex(double deci)
        {
            deci = deci / 100 * 60;
            return deci;
        }


    }
}
