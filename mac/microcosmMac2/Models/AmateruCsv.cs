using System;
namespace microcosmMac2.Models
{
    public class AmateruCsv
    {
        public AmateruCsv()
        {
        }

        public string NAME { get; set; }

        public string KANA { get; set; }
        public int GENDER { get; set; }
        public string JOB { get; set; }
        public string MEMO { get; set; }
        public string DATE { get; set; }
        public string TIME { get; set; }
        public string PLACENAME { get; set; }
        public string LATITUDE { get; set; }
        public string LONGITUDE { get; set; }

        public string TIMEZONE { get; set; }
    }
}

