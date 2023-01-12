using microcosm.Db;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.common
{
    public enum ETargetUser
    {
        USER1 = 0,
        USER2 = 1,
        EVENT1 = 2,
        EVENT2 = 3
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

    public class CommonData
    {


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
        public const int ZODIAC_VT = 10002;
        public const int ZODIAC_POF = 10011;
        public const int ZODIAC_ERIS = 136199;

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



        /// <summary>
        /// 番号を引数に天体のシンボルを返す
        /// </summary>
        /// <param name="number">天体番号</param>
        /// <returns></returns>
        public static string getPlanetSymbol(int number)
        {
            switch (number)
            {
                case ZODIAC_SUN:
                    return "\u2609";
                case ZODIAC_MOON:
                    return "\u263d";
                case ZODIAC_MERCURY:
                    return "\u263f";
                case ZODIAC_VENUS:
                    return "\u2640";
                case ZODIAC_MARS:
                    return "\u2642";
                case ZODIAC_JUPITER:
                    return "\u2643";
                case ZODIAC_SATURN:
                    return "\u2644";
                case ZODIAC_URANUS:
                    return "\u2645";
                case ZODIAC_NEPTUNE:
                    return "\u2646";
                case ZODIAC_PLUTO:
                    return "\u2647";
                // 外部フォントだと天文学用のPLUTOになっているのが困りどころ
                case ZODIAC_DH_TRUENODE:
                    return "\u260a";
                case ZODIAC_EARTH:
                    return "\u2641";
                case ZODIAC_CHIRON:
                    return "\u26b7";
                case ZODIAC_OSCU_LILITH:
                    return "\u26b8";
                case ZODIAC_MEAN_LILITH:
                    return "\u26b8";
                case ZODIAC_ERIS:
                    return "\u2641";
            }
            return "";
        }

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
                case ZODIAC_CHIRON:
                    return "Q";
                case ZODIAC_OSCU_LILITH:
                    return "R";
                case ZODIAC_MEAN_LILITH:
                    return "R";
                case ZODIAC_EARTH:
                    return "S";
                case ZODIAC_CERES:
                    return "T";
                case ZODIAC_PALLAS:
                    return "U";
                case ZODIAC_JUNO:
                    return "V";
                case ZODIAC_VESTA:
                    return "W";
                case ZODIAC_VT:
                    return "X";
                case ZODIAC_POF:
                    return "Y";
                case ZODIAC_ASC:
                    return "M";
                case ZODIAC_MC:
                    return "N";
            }
            return "";
        }


        /// <summary>
        /// 番号を引数に天体の文字列を返す
        /// </summary>
        /// <param name="number">天体番号</param>
        /// <returns></returns>
        public static string getPlanetText(int number)
        {
            switch (number)
            {
                case ZODIAC_SUN:
                    return "太陽";
                case ZODIAC_MOON:
                    return "月";
                case ZODIAC_MERCURY:
                    return "水星";
                case ZODIAC_VENUS:
                    return "金星";
                case ZODIAC_MARS:
                    return "火星";
                case ZODIAC_JUPITER:
                    return "木星";
                case ZODIAC_SATURN:
                    return "土星";
                case ZODIAC_URANUS:
                    return "天王星";
                case ZODIAC_NEPTUNE:
                    return "海王星";
                case ZODIAC_PLUTO:
                    return "冥王星";
                case ZODIAC_DH_TRUENODE:
                    return "ヘッド";
                case ZODIAC_DH_MEANNODE:
                    return "ヘッド";
                case ZODIAC_CHIRON:
                    return "キロン";
                case ZODIAC_ASC:
                    return "ASC";
                case ZODIAC_MC:
                    return "MC";
                case ZODIAC_EARTH:
                    return "地球";
                case ZODIAC_OSCU_LILITH:
                    return "リリス";
                case ZODIAC_MEAN_LILITH:
                    return "リリス";
                case ZODIAC_ERIS:
                    return "エリス";
            }
            return "";
        }

        /// <summary>
        /// 番号を引数に天体の文字列を返す(マウスオーバー)
        /// </summary>
        /// <param name="number">天体番号</param>
        /// <returns></returns>
        public static string getPlanetTextOnMouse(int number)
        {
            switch (number)
            {
                case ZODIAC_SUN:
                    return "太陽";
                case ZODIAC_MOON:
                    return "月";
                case ZODIAC_MERCURY:
                    return "水星";
                case ZODIAC_VENUS:
                    return "金星";
                case ZODIAC_MARS:
                    return "火星";
                case ZODIAC_JUPITER:
                    return "木星";
                case ZODIAC_SATURN:
                    return "土星";
                case ZODIAC_URANUS:
                    return "天王星";
                case ZODIAC_NEPTUNE:
                    return "海王星";
                case ZODIAC_PLUTO:
                    return "冥王星";
                case ZODIAC_DH_TRUENODE:
                    return "ヘッド(true)";
                case ZODIAC_DH_MEANNODE:
                    return "ヘッド(mean)";
                case ZODIAC_CHIRON:
                    return "キロン";
                case ZODIAC_ASC:
                    return "ASC";
                case ZODIAC_MC:
                    return "MC";
                case ZODIAC_EARTH:
                    return "地球";
                case ZODIAC_OSCU_LILITH:
                    return "リリス(true)";
                case ZODIAC_MEAN_LILITH:
                    return "リリス(mean)";
                case ZODIAC_ERIS:
                    return "エリス";
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
        /// 番号を引数にサインルーラーのシンボル(UTF文字)を返す
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string getSignRulersSymbol(int number)
        {
            switch (number)
            {
                case SIGN_ARIES:
                    // 火星
                    return "\u2642";
                case SIGN_TAURUS:
                    // 金星
                    return "\u2640";
                case SIGN_GEMINI:
                    // 水星
                    return "\u263f";
                case SIGN_CANCER:
                    // 月
                    return "\u263d";
                case SIGN_LEO:
                    // 太陽
                    return "\u2609";
                case SIGN_VIRGO:
                    // 水星
                    return "\u263f";
                case SIGN_LIBRA:
                    // 金星
                    return "\u2640";
                case SIGN_SCORPIO:
                    // 冥王星
                    return "\u2647";
                case SIGN_SAGITTARIUS:
                    // 木星
                    return "\u2643";
                case SIGN_CAPRICORN:
                    // 土星
                    return "\u2644";
                case SIGN_AQUARIUS:
                    // 天王星
                    return "\u2645";
                case SIGN_PISCES:
                    // 海王星
                    return "\u2646";
            }
            return "";
        }

        /// <summary>
        /// planetNoを引数に所属するサインルーラーの番号を返す
        /// </summary>
        /// <param name="planetNo"></param>
        /// <returns></returns>
        public static int getSignRulersNo(int planetNo)
        {
            switch (planetNo)
            {
                case SIGN_ARIES:
                    // 火星
                    return 4;
                case SIGN_TAURUS:
                    // 金星
                    return 3;
                case SIGN_GEMINI:
                    // 水星
                    return 2;
                case SIGN_CANCER:
                    // 月
                    return 1;
                case SIGN_LEO:
                    // 太陽
                    return 0;
                case SIGN_VIRGO:
                    // 水星
                    return 2;
                case SIGN_LIBRA:
                    // 金星
                    return 3;
                case SIGN_SCORPIO:
                    // 冥王星
                    return 9;
                case SIGN_SAGITTARIUS:
                    // 木星
                    return 5;
                case SIGN_CAPRICORN:
                    // 土星
                    return 6;
                case SIGN_AQUARIUS:
                    // 天王星
                    return 7;
                case SIGN_PISCES:
                    // 海王星
                    return 8;
            }
            return 0;
        }

        // 番号を引数に感受点のシンボルを返す
        public static string getSensitiveSymbol(int number)
        {
            switch (number)
            {
                // UNICODEが無い！
                case ZODIAC_ASC:
                    return "Ac";
                // return "K";
                // UNICODEが無い！
                case ZODIAC_MC:
                    return "Mc";
                // return "L";
                case ZODIAC_DH_TRUENODE:
                    return "\u260a";
                    // return "M";
            }
            return "";
        }

        // 番号を引数に感受点の文字列を返す
        public static string getSensitiveText(int number)
        {
            switch (number)
            {
                case ZODIAC_ASC:
                    return "ASC";
                case ZODIAC_MC:
                    return "MC";
                case ZODIAC_DH_TRUENODE:
                    return "D.H.";
            }
            return "";
        }

        public static System.Windows.Media.Brush getPlanetColor(int number)
        {
            if (number == (int)CommonData.ZODIAC_SUN)
            {
                return System.Windows.Media.Brushes.Olive;
            }
            else if (number == (int)CommonData.ZODIAC_MOON)
            {
                return System.Windows.Media.Brushes.DarkGoldenrod;
            }
            else if (number == (int)CommonData.ZODIAC_MERCURY)
            {
                return System.Windows.Media.Brushes.Purple;
            }
            else if (number == (int)CommonData.ZODIAC_VENUS)
            {
                return System.Windows.Media.Brushes.Green;
            }
            else if (number == (int)CommonData.ZODIAC_MARS)
            {
                return System.Windows.Media.Brushes.Red;
            }
            else if (number == (int)CommonData.ZODIAC_JUPITER)
            {
                return System.Windows.Media.Brushes.Maroon;
            }
            else if (number == (int)CommonData.ZODIAC_SATURN)
            {
                return System.Windows.Media.Brushes.DimGray;
            }
            else if (number == (int)CommonData.ZODIAC_URANUS)
            {
                return System.Windows.Media.Brushes.DarkTurquoise;
            }
            else if (number == (int)CommonData.ZODIAC_NEPTUNE)
            {
                return System.Windows.Media.Brushes.DodgerBlue;
            }
            else if (number == (int)CommonData.ZODIAC_PLUTO)
            {
                return System.Windows.Media.Brushes.DeepPink;
            }
            else if (number == (int)CommonData.ZODIAC_EARTH)
            {
                return System.Windows.Media.Brushes.SkyBlue;
            }
            else if (number == (int)CommonData.ZODIAC_DH_TRUENODE)
            {
                return System.Windows.Media.Brushes.DarkCyan;
            }
            else if (number == (int)CommonData.ZODIAC_DH_MEANNODE)
            {
                return System.Windows.Media.Brushes.DarkCyan;
            }
            else if (number == (int)CommonData.ZODIAC_DT_TRUE)
            {
                return System.Windows.Media.Brushes.DarkCyan;
            }
            else if (number == (int)CommonData.ZODIAC_DT_MEAN)
            {
                return System.Windows.Media.Brushes.DarkCyan;
            }
            else if (number == (int)CommonData.ZODIAC_OSCU_LILITH)
            {
                return System.Windows.Media.Brushes.MediumSeaGreen;
            }
            else if (number == (int)CommonData.ZODIAC_MEAN_LILITH)
            {
                return System.Windows.Media.Brushes.MediumSeaGreen;
            }
            return System.Windows.Media.Brushes.Black;
        }


        // サイン番号を返す(0:牡羊座、11:魚座)
        public static int getSign(double absolute_position)
        {
            return (int)absolute_position / 30;
        }

        // サインテキストを返す(0:♈、11:♓)
        public static string getSignText(double absolute_position)
        {
            return getSignSymbol((int)absolute_position / 30);
        }

        // サインテキストを返す(0:羊、11:魚)
        public static string getSignTextJpSimple(double absolute_position)
        {
            switch ((int)absolute_position / 30)
            {
                case 0:
                    return "羊";
                case 1:
                    return "牛";
                case 2:
                    return "双";
                case 3:
                    return "蟹";
                case 4:
                    return "獅";
                case 5:
                    return "乙";
                case 6:
                    return "天";
                case 7:
                    return "蠍";
                case 8:
                    return "射";
                case 9:
                    return "山";
                case 10:
                    return "水";
                case 11:
                    return "魚";
                default:
                    break;
            }
            return "";

        }

        /// <summary>
        /// サインテキストを返す(0:牡羊座、11:魚座)
        /// </summary>
        /// <param name="absolute_position"></param>
        /// <returns></returns>
        public static string getSignTextJp(double absolute_position)
        {
            switch ((int)absolute_position / 30)
            {
                case 0:
                    return "牡羊座";
                case 1:
                    return "牡牛座";
                case 2:
                    return "双子座";
                case 3:
                    return "蟹座";
                case 4:
                    return "獅子座";
                case 5:
                    return "乙女座";
                case 6:
                    return "天秤座";
                case 7:
                    return "蠍座";
                case 8:
                    return "射手座";
                case 9:
                    return "山羊座";
                case 10:
                    return "水瓶座";
                case 11:
                    return "魚座";
                default:
                    break;
            }
            return "";
        }


        // サイン色を返す
        public static System.Windows.Media.Brush getSignColor(double absolute_position)
        {
            switch ((int)absolute_position / 30)
            {
                case 0:
                    // 牡羊座
                    return System.Windows.Media.Brushes.OrangeRed;
                case 1:
                    // 牡牛座
                    return System.Windows.Media.Brushes.Goldenrod;
                case 2:
                    // 双子座
                    return System.Windows.Media.Brushes.MediumSeaGreen;
                case 3:
                    // 蟹座
                    return System.Windows.Media.Brushes.SteelBlue;
                case 4:
                    // 獅子座
                    return System.Windows.Media.Brushes.Crimson;
                case 5:
                    // 乙女座
                    return System.Windows.Media.Brushes.Maroon;
                case 6:
                    // 天秤座
                    return System.Windows.Media.Brushes.Teal;
                case 7:
                    // 蠍座
                    return System.Windows.Media.Brushes.CornflowerBlue;
                case 8:
                    // 射手座
                    return System.Windows.Media.Brushes.DeepPink;
                case 9:
                    // 山羊座
                    return System.Windows.Media.Brushes.SaddleBrown;
                case 10:
                    // 水瓶座
                    return System.Windows.Media.Brushes.CadetBlue;
                case 11:
                    // 魚座
                    return System.Windows.Media.Brushes.DodgerBlue;
                default:
                    break;
            }
            return System.Windows.Media.Brushes.Black;
        }

        // サイン度数を返す(0～29.9)
        public static double getDeg(double absolute_position)
        {
            return absolute_position % 30;
        }

        public static string getRetrograde(double speed)
        {
            if (speed < 0)
            {
                return "\u211e";
            }
            return "";
        }

        // SelectBoxのIndexを返す
        public static int getTimezoneIndex(string timezone)
        {
            switch (timezone)
            {
                case "JST":
                    return 0;
                case "UTC":
                    return 1;
            }
            return 0;
        }

        /// <summary>
        /// タイムゾーン一覧
        /// </summary>
        /// <returns></returns>
        public static string[] GetTimeTables()
        {
            string[] items = new string[68];
            items[0] = "UTC (+0:00)";
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

            return items;
        }

        /// <summary>
        /// 新タイムゾーン変換
        /// Asia/Tokyoを-9.0にする
        /// </summary>
        /// <param name="timezone"></param>
        /// <returns></returns>
        public static double GetTimezoneValue(string timezone)
        {
            if (timezone.IndexOf("Johannesburg") > 0) { return +2; }
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


        public static UserEventData udata2event(UserData udata)
        {
            return new UserEventData()
            {
                name = udata.name,
                birth_year = udata.birth_year,
                birth_month = udata.birth_month,
                birth_day = udata.birth_day,
                birth_hour = udata.birth_hour,
                birth_minute = udata.birth_minute,
                birth_second = udata.birth_second,
                birth_place = udata.birth_place,
                lat = udata.lat,
                lng = udata.lng,
                lat_lng = udata.lat_lng,
                timezone = udata.timezone,
                timezone_str = udata.timezone_str,
                memo = udata.memo
            };
        }

        // ポイントの回転
        // 左サイドバーのwidthマイナスして呼び、後で足すこと
        public static PointF rotate(double x, double y, double degree)
        {
            // ホロスコープは180°から始まる
            degree += 180.0;

            double rad = (degree / 180.0) * Math.PI;
            double newX = x * Math.Cos(rad) - y * Math.Sin(rad);
            double newY = x * Math.Sin(rad) + y * Math.Cos(rad);


            return new PointF((float)newX, (float)newY);
        }

        /// <summary>
        /// 50を30にする
        /// </summary>
        /// <param name="deci"></param>
        /// <returns></returns>
        public static double DecimalToHex(double deci)
        {
            deci = deci / 100 * 60;
            return deci;
        }
    }
}
