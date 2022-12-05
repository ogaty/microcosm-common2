using microcosm.common;
using microcosm.config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.ViewModel
{
    public class HouseListData
    {
        public EDecimalDisp disp;
        public string hName { get; set; }
        public string firstData { get; set; }
        public string secondData { get; set; }
        public string thirdData { get; set; }


        protected string[] houses = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII" };

        public HouseListData()
        {

        }
        public HouseListData(
            EDecimalDisp disp,
            int i,
            double data1,
            double data2,
            double data3
            )
        {
            this.disp = disp;
            hName = houses[i];
            firstData = getTxt(data1);
            secondData = getTxt(data2);
            thirdData = getTxt(data3);
        }

        private string getTxt(double absolute_position)
        {
            string dataTxt = CommonData.getSignTextJpSimple(absolute_position);

            if (disp == EDecimalDisp.DECIMAL)
            {
               dataTxt += string.Format("{0,00:F3}", CommonData.getDeg(absolute_position));
            }
            else
            {
                dataTxt += ((int)(absolute_position % 30)).ToString() + ".";
                dataTxt += ((int)CommonData.DecimalToHex(absolute_position % 1 * 100)).ToString("00") + "'";
            }
            return dataTxt;
        }
    }
}
