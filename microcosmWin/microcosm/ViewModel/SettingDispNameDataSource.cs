using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.ViewModel
{
    class SettingDispNameDataSource
    {
        public ObservableCollection<SettingDispNameData> dispNames { get; set; }

        public SettingDispNameDataSource(string[] names)
        {
            dispNames = new ObservableCollection<SettingDispNameData>();
            for (int i = 0; i < 10; i++)
            {
                dispNames.Add(new SettingDispNameData() { dispName = names[i] });
            }
        }

    }
}
