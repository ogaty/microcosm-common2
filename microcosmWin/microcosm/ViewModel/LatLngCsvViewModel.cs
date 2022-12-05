using microcosm.common;
using microcosm.Db;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.ViewModel
{
    public class LatLngCsvViewModel
    {

        public ObservableCollection<LatLng> list { get; set; }

        public LatLngCsvViewModel()
        {
            list = new ObservableCollection<LatLng>();
            string root = Util.root();

            using (var reader = new StreamReader(root + @"\system\addr.csv", Encoding.GetEncoding("UTF-8")))
            using (var csv = new CsvHelper.CsvReader(reader, new CultureInfo("ja-JP", false)))
            {
                var records = csv.GetRecords<LatLng>();

                foreach (LatLng record in records)
                {
                    list.Add(record);
                }
            }
        }
    }
}
