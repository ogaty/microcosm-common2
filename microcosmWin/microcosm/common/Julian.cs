using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.common
{
    public class Julian
    {
        public static double ToJulianDate(DateTime date)
        {
            return date.ToOADate() + 2415018.5;
        }
    }
}
