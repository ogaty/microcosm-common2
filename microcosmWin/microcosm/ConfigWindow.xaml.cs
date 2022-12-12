using microcosm.common;
using microcosm.Db;
using microcosm.ViewModel;
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
    /// ConfigWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public MainWindow main;
        public LatLngCsvViewModel? vm;
        public TimeZoneViewModel? timezoneVM;

        public ConfigWindow(MainWindow main)
        {
            this.main = main;
            InitializeComponent();

            vm = null;
            timezoneVM = null;
            ReSet();
        }

        private void ReSet()
        {
            if (main.configData.centric == config.ECentric.GEO_CENTRIC)
            {
                geoCentric.IsChecked = true;
            }
            else
            {
                helioCentric.IsChecked = true;
            }

            if (main.configData.sidereal == config.Esidereal.TROPICAL)
            {
                tropical.IsChecked = true;
            }
            else if (main.configData.sidereal == config.Esidereal.SIDEREAL)
            {
                tropical.IsChecked = true;
            }
            else
            {
                draconic.IsChecked = true;
            }

            if (main.configData.progression == config.EProgression.SECONDARY)
            {
                secondaryProgression.IsChecked = true;
            }
            else if (main.configData.progression == config.EProgression.PRIMARY)
            {
                primaryProgression.IsChecked = true;
            }
            else if (main.configData.progression == config.EProgression.SOLAR)
            {
                solarArcProgression.IsChecked = true;
            }
            else if (main.configData.progression == config.EProgression.CPS)
            {
                compositProgression.IsChecked = true;
            }

            if (main.configData.decimalDisp == config.EDecimalDisp.DECIMAL)
            {
                decimalDisp.IsChecked = true;
            }
            else
            {
                degreeDisp.IsChecked = true;
            }

            if (main.configData.dispPattern == config.EDispPetern.FULL)
            {
                fullDisp.IsChecked = true;
            }
            else
            {
                miniDisp.IsChecked = true;
            }

            if (main.configData.houseCalc == config.EHouseCalc.PLACIDUS)
            {
                placidus.IsChecked = true;
            }
            else if (main.configData.houseCalc == config.EHouseCalc.KOCH)
            {
                koch.IsChecked = true;
            }
            else if (main.configData.houseCalc == config.EHouseCalc.CAMPANUS)
            {
                campanus.IsChecked = true;
            }
            else if (main.configData.houseCalc == config.EHouseCalc.EQUAL)
            {
                equal.IsChecked = true;
            }
            else if (main.configData.houseCalc == config.EHouseCalc.ZEROARIES)
            {
                zeroaries.IsChecked = true;
            }

            defaultLat.Text = main.configData.lat.ToString();
            defaultLng.Text = main.configData.lng.ToString();
            defaultPlace.Text = main.configData.defaultPlace;
            int index = 0;
            foreach (string item in defaultTimezone.Items)
            {
                if (item == main.configData.defaultTimezone)
                {
                    defaultTimezone.SelectedIndex = index;
                }
                index++;
            }

            vm = new LatLngCsvViewModel();
            latlngList.DataContext = vm;
            timezoneVM = new TimeZoneViewModel();
            string[] timezones = CommonData.GetTimeTables();
            foreach (string timezone in timezones)
            {
                timezoneVM.timezone.Add(timezone);
            }
            this.DataContext = timezoneVM;
            defaultTimezone.SelectedItem = "Asia/Tokyo (+9:00)";
        }

        private void ConfigSave()
        {
            string root = Util.root();
            string systemDirName = root + @"\system";
            string filename = systemDirName + @"\config.json";

            string configJson = JsonSerializer.Serialize(main.configData, new JsonSerializerOptions
            { 
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true,
            });
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(configJson);
                sw.Close();
            }
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (geoCentric == null)
            {
                return;
            }
            bool? geoCentricIsChecked = geoCentric.IsChecked;
            if (geoCentricIsChecked == true)
            {
                main.configData.centric = config.ECentric.GEO_CENTRIC;
            } else
            {
                main.configData.centric = config.ECentric.HELIO_CENTRIC;
            }

            bool? sideRealIsChecked = sidereal.IsChecked;
            bool? draconicIsChecked = draconic.IsChecked;
            if (sideRealIsChecked == true)
            {
                main.configData.sidereal = config.Esidereal.SIDEREAL;
            }
            else if (draconicIsChecked == true)
            {
                main.configData.sidereal = config.Esidereal.DRACONIC;
            }
            else
            {
                main.configData.sidereal = config.Esidereal.TROPICAL;
            }

            bool? decimalDispIsChecked = decimalDisp.IsChecked;
            if (decimalDispIsChecked == true)
            {
                main.configData.decimalDisp = config.EDecimalDisp.DECIMAL;
            }
            else
            {
                main.configData.decimalDisp = config.EDecimalDisp.DEGREE;
            }

            bool? fullDispIsChecked = fullDisp.IsChecked;
            if (fullDispIsChecked == true)
            {
                main.configData.dispPattern = config.EDispPetern.FULL;
            }
            else
            {
                main.configData.dispPattern = config.EDispPetern.MINI;
            }

            if ((bool)secondaryProgression.IsChecked)
            {
                main.configData.progression = config.EProgression.SECONDARY;
            }
            else if ((bool)primaryProgression.IsChecked)
            {
                main.configData.progression = config.EProgression.PRIMARY;
            }
            else if ((bool)solarArcProgression.IsChecked)
            {
                main.configData.progression = config.EProgression.SOLAR;
            }
            else if ((bool)compositProgression.IsChecked)
            {
                main.configData.progression = config.EProgression.CPS;
            }

            if ((bool)placidus.IsChecked)
            {
                main.configData.houseCalc = config.EHouseCalc.PLACIDUS;
            }
            else if ((bool)koch.IsChecked)
            {
                main.configData.houseCalc = config.EHouseCalc.KOCH;
            }
            else if ((bool)campanus.IsChecked)
            {
                main.configData.houseCalc = config.EHouseCalc.CAMPANUS;
            }
            else if ((bool)equal.IsChecked)
            {
                main.configData.houseCalc = config.EHouseCalc.EQUAL;
            }
            else if ((bool)zeroaries.IsChecked)
            {
                main.configData.houseCalc = config.EHouseCalc.ZEROARIES;
            }

            if ((bool)trueHead.IsChecked)
            {
                main.configData.nodeCalc = config.ENodeCalc.TRUE;
            }
            else if ((bool)meanHead.IsChecked)
            {
                main.configData.nodeCalc = config.ENodeCalc.MEAN;
            }

            if ((bool)oscuApogee.IsChecked)
            {
                main.configData.lilithCalc = config.ELilithCalc.OSCU;
            }
            else if ((bool)meanApogee.IsChecked)
            {
                main.configData.lilithCalc = config.ELilithCalc.MEAN;
            }

            main.configData.defaultPlace = defaultPlace.Text;
            main.configData.lat = Double.Parse(defaultLat.Text);
            main.configData.lng = Double.Parse(defaultLng.Text);
            string item = (string)defaultTimezone.SelectedItem;
            main.configData.defaultTimezone = item;

            ConfigSave();
            main.ReCalc();
            main.ReRender();
            this.Visibility = Visibility.Hidden;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            ReSet();
            this.Visibility = Visibility.Hidden;
        }

        private void latlngList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = (ListView)sender;
            LatLng ll = (LatLng)lv.SelectedItem;
            defaultPlace.Text = ll.name;
            defaultLat.Text = ll.lat.ToString();
            defaultLng.Text = ll.lng.ToString();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Collapsed;
        }
    }
}
