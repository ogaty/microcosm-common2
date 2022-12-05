using System;
namespace microcosmMac2.Common
{
    /// <summary>
    /// ユリウス日計算用
    /// AMATERUはswiss ephemerisとできるだけ切り離したくて使ってる
    /// </summary>
    public class Julian
    {
        public static double ToJulianDate(DateTime date)
        {
            return date.ToOADate() + 2415018.5;
        }
    }
}

