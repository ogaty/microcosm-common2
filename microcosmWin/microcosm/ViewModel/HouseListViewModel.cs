using microcosm.common;
using microcosm.config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.ViewModel
{
    public class HouseListViewModel
    {
        public ObservableCollection<HouseListData> hList { get; set; }

        public HouseListViewModel(
            EDecimalDisp disp,
            double[] list1,
            double[] list2,
            double[] list3
            )
        {
            hList = new ObservableCollection<HouseListData>();
            Enumerable.Range(0, 12).ToList().ForEach(i => {
                hList.Add(new HouseListData(disp,
                    i,
                    list1[i + 1],
                    list2[i + 1],
                    list3[i + 1]
                    ));
            });

        }

        public void ReRender(
            EDecimalDisp disp,
            double[] list1,
            double[] list2,
            double[] list3
            )
        {
            hList.Clear();
            Enumerable.Range(0, 12).ToList().ForEach(i => {
                hList.Add(new HouseListData(disp,
                    i,
                    list1[i + 1],
                    list2[i + 1],
                    list3[i + 1]
                    ));
            });
        }
    }
}
