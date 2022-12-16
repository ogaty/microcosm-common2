using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.Db
{
    public class KeplerCsv
    {
        [Index(0)]
        public string NAME { get; set; }

        [Index(1)]
        public string DATE { get; set; }

        [Index(3)]
        public string TIME { get; set; }

        [Index(4)]
        public string PLACENAME { get; set; }

        [Index(5)]
        public string LATITUDE { get; set; }

        [Index(6)]
        public string LONGITUDE { get; set; }
    }
}
