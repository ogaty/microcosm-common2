using microcosm.calc;
using microcosm.config;
using microcosm.Db;
using microcosm.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.common
{
    /// <summary>
    /// 今までmainデータに持っていた情報、splashからmainの受け渡し
    /// 将来的にはmainデータのメンバから外してここを参照したい
    /// </summary>
    public class BaseData
    {
        public ConfigData configData;
        public SettingData[] settings = new SettingData[10];
        public SettingData currentSetting;
        public TempSetting tempSetting;
        public List<string> sabians;

        public UserData user1data;
        public UserData user2data;
        public UserData event1data;
        public UserData event2data;


        public Dictionary<int, PlanetData> list1 = new Dictionary<int, PlanetData>();
        public Dictionary<int, PlanetData> list2 = new Dictionary<int, PlanetData>();
        public Dictionary<int, PlanetData> list3 = new Dictionary<int, PlanetData>();

        public double[] houseList1 = new double[13];
        public double[] houseList2 = new double[13];
        public double[] houseList3 = new double[13];

        public AstroCalc? calc;
    }
}
