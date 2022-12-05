using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.Db
{
    public class ZetCsv
    {
        [Index(0)]
        public string name { get; set; }

        [Index(1)]
        public string date { get; set; }

        [Index(2)]
        public string time { get; set; }

        [Index(3)]
        public double timezone { get; set; }

        [Index(4)]
        public string place { get; set; }

        [Index(5)]
        public string lat { get; set; }

        [Index(6)]
        public string lng { get; set; }

        [Index(8)]
        public string memo { get; set; }

    }
}
