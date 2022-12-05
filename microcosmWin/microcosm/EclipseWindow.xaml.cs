using microcosm.calc;
using microcosm.common;
using microcosm.Db;
using microcosm.Planet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace microcosm
{
    /// <summary>
    /// EclipseWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class EclipseWindow : Window
    {
        public MainWindow mainWindow;
        public DatabaseWindow databaseWindow;
        public UserData savedUserData;
        public DateTime savedDateTime;
        private string oldFile;

        public EclipseWindow()
        {
            InitializeComponent();
        }

        public EclipseWindow(MainWindow main, DatabaseWindow databaseWindow)
        {
            mainWindow = main;
            this.databaseWindow = databaseWindow;
            InitializeComponent();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            bool isForward = true;
            if (prev.IsChecked == true)
            {
                isForward = false;
            }

            ComboBoxItem item = (ComboBoxItem)planetList.SelectedItem;

            UserJsonList userJsonList = null;
            using (FileStream fs = new FileStream(oldFile, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                string userJsonData = sr.ReadToEnd();
                userJsonList = JsonSerializer.Deserialize<UserJsonList>(userJsonData);
            }


            int planetId = GetPlanetId(item.Content.ToString());

            EclipseCalc eclipse = mainWindow.calc.GetEclipseInstance();
            DateTime newDay = eclipse.GetEclipse(DateTime.Parse(targetDate.Text), savedUserData.timezone, planetId, Double.Parse(targetDegree.Text), isForward);

            UserData userJson = new UserData()
            {
                name = GetPlanetText(item.Content.ToString()) + "回帰",
                birth_year = newDay.Year,
                birth_month = newDay.Month,
                birth_day = newDay.Day,
                birth_hour = newDay.Hour,
                birth_minute = newDay.Minute,
                birth_second = newDay.Second,
                birth_place = savedUserData.birth_place,
                memo = "",
                timezone = savedUserData.timezone,
                timezone_str = savedUserData.timezone_str,
                lat = savedUserData.lat,
                lng = savedUserData.lng
            };
            userJsonList.list.Add(userJson);
            string userJsonStr = JsonSerializer.Serialize(userJsonList, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true,
            });

            using (FileStream fs = new FileStream(oldFile, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(userJsonStr);
                sw.Close();
            }

            databaseWindow.ReRenderEvent(oldFile);

            this.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 外から呼ばれる、初期ファイルを読み込んでラベルにセット
        /// </summary>
        /// <param name="dirName"></param>
        /// <param name="fileName"></param>
        public void SetData(string fileName, int userIndex, int eventIndex)
        {
            oldFile = fileName;

            UserJsonList? jsonList = null;
            ComboBoxItem item = (ComboBoxItem)planetList.SelectedItem;
            int planetId = GetPlanetId(item.Content.ToString());
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                string jsonData = sr.ReadToEnd();
                jsonList = JsonSerializer.Deserialize<UserJsonList>(jsonData);
                savedDateTime = DateTime.Now;

                UserData udata = jsonList.list[eventIndex];
                targetDate.Text = String.Format("{0}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}",
                    savedDateTime.Year, savedDateTime.Month, savedDateTime.Day,
                    savedDateTime.Hour, savedDateTime.Minute, savedDateTime.Second);
                savedUserData = udata;
                PlanetData planet = mainWindow.calc.PositionCalcSingle(planetId, udata.timezone, udata.GetBirthDateTime());
                targetDegree.Text = planet.absolute_position.ToString("0.00");
            }
        }

        public int GetPlanetId(string planetName)
        {
            int planetId = 0;
            switch (planetName)
            {
                case "sun":
                    planetId = CommonData.ZODIAC_SUN;
                    break;
                case "moon":
                    planetId = CommonData.ZODIAC_MOON;
                    break;
                case "mercury":
                    planetId = CommonData.ZODIAC_MERCURY;
                    break;
                case "venus":
                    planetId = CommonData.ZODIAC_VENUS;
                    break;
                case "mars":
                    planetId = CommonData.ZODIAC_MARS;
                    break;
                case "jupiter":
                    planetId = CommonData.ZODIAC_JUPITER;
                    break;
                case "saturn":
                    planetId = CommonData.ZODIAC_SATURN;
                    break;
            }
            return planetId;
        }

        public string GetPlanetText(string planetName)
        {
            string planetText = "";
            switch (planetName)
            {
                case "sun":
                    planetText = "太陽";
                    break;
                case "moon":
                    planetText = "月";
                    break;
                case "mercury":
                    planetText = "水星";
                    break;
                case "venus":
                    planetText = "金星";
                    break;
                case "mars":
                    planetText = "火星";
                    break;
                case "jupiter":
                    planetText = "木星";
                    break;
                case "saturn":
                    planetText = "土星";
                    break;
            }
            return planetText;
        }

        /// <summary>
        /// 対象天体の変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void planetList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)planetList.SelectedItem;
            int planetId = GetPlanetId(item.Content.ToString());

            // インスタンス生成直後だけnullになる
            if (savedUserData != null)
            {
                PlanetData planet = mainWindow.calc.PositionCalcSingle(planetId, savedUserData.timezone, savedUserData.GetBirthDateTime());
                targetDegree.Text = planet.absolute_position.ToString("0.00");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Collapsed;
        }
    }
}
