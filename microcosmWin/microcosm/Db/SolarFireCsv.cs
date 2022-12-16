using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.Db
{
    public class SolarFireCsv
    {
        [Index(0)]
        public string NAME { get; set; }

        [Index(1)]
        public int YEAR { get; set; }

        [Index(2)]
        public int MONTH { get; set; }

        [Index(3)]
        public int DAY { get; set; }

        [Index(4)]
        public int HOUR { get; set; }

        [Index(5)]
        public int MINUTE { get; set; }

        [Index(6)]
        public int SECOND { get; set; }

        [Index(7)]
        public string PLACENAME { get; set; }

        [Index(8)]
        public double LATITUDE { get; set; }

        [Index(9)]
        public double LONGITUDE { get; set; }
    }
}
