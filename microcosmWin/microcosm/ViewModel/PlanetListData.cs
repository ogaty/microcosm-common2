using microcosm.common;
using microcosm.config;
using microcosm.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.ViewModel
{
    public class PlanetListData
    {
        public string pName { get; set; }
        public string firstData { get; set; }
        public string secondData { get; set; }
        public string thirdData { get; set; }
        public string fourthData { get; set; }
        public string fifthData { get; set; }
        public string sixthData { get; set; }

        public PlanetListData()
        {

        }
        public PlanetListData(
            EDecimalDisp disp,
            int i,
            PlanetData data1,
            PlanetData data2,
            PlanetData data3
            )
        {
            pName = CommonData.getPlanetSymbol(i);
            firstData = getTxt(disp, data1.absolute_position);
            secondData = getTxt(disp, data2.absolute_position);
            thirdData = getTxt(disp, data3.absolute_position);
        }

        private string getTxt(EDecimalDisp disp, double absolute_position)
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
