using microcosm.config;
using microcosm.Planet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.ViewModel
{
    public class PlanetListViewModel
    {
        public ObservableCollection<PlanetListData> pList { get; set; }
        public MainWindow main;

        public PlanetListViewModel(
            MainWindow main,
            Dictionary<int, PlanetData> list1,
            Dictionary<int, PlanetData> list2,
            Dictionary<int, PlanetData> list3
            )
        {
            this.main = main;
            pList = new ObservableCollection<PlanetListData>();
            Enumerable.Range(0, 10).ToList().ForEach(i => {
                pList.Add(new PlanetListData(main.configData.decimalDisp, i,
                    list1[i],
                    list2[i],
                    list3[i]
                    ));
            });

        }

        public void ReRender(
            Dictionary<int, PlanetData> list1,
            Dictionary<int, PlanetData> list2,
            Dictionary<int, PlanetData> list3
            )
        {
            pList.Clear();
            Enumerable.Range(0, 10).ToList().ForEach(i => {
                pList.Add(new PlanetListData(main.configData.decimalDisp, i,
                    list1[i],
                    list2[i],
                    list3[i]
                    ));
            });
        }
    }
}
