using microcosm.common;
using microcosm.config;
using microcosm.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// SettingWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingWindow : Window
    {
        public MainWindow main;
        public SettingWindowViewModel vm;

        public SettingWindow(MainWindow mainWindow)
        {
            InitializeComponent();

            main = mainWindow;

            vm = new SettingWindowViewModel(main.settings);
            this.DataContext = vm;
            SettingDispNameList.SelectedIndex = 0;

            dispName.Text = main.settings[0].dispName;

            dispPlanetSun.IsChecked = main.settings[0].dispPlanetSun == 1;
            dispPlanetMoon.IsChecked = main.settings[0].dispPlanetMoon == 1;
            dispPlanetMercury.IsChecked = main.settings[0].dispPlanetMercury == 1;
            dispPlanetVenus.IsChecked = main.settings[0].dispPlanetVenus == 1;
            dispPlanetMars.IsChecked = main.settings[0].dispPlanetMars == 1;
            dispPlanetJupiter.IsChecked = main.settings[0].dispPlanetJupiter == 1;
            dispPlanetSaturn.IsChecked = main.settings[0].dispPlanetSaturn == 1;
            dispPlanetUranus.IsChecked = main.settings[0].dispPlanetUranus == 1;
            dispPlanetNeptune.IsChecked = main.settings[0].dispPlanetNeptune == 1;
            dispPlanetPluto.IsChecked = main.settings[0].dispPlanetPluto == 1;
            dispPlanetAsc.IsChecked = main.settings[0].dispPlanetAsc == 1;
            dispPlanetMc.IsChecked = main.settings[0].dispPlanetMc == 1;
            dispPlanetChiron.IsChecked = main.settings[0].dispPlanetChiron == 1;
            dispPlanetDH.IsChecked = main.settings[0].dispPlanetDH == 1;
            dispPlanetDT.IsChecked = main.settings[0].dispPlanetDT == 1;
            dispPlanetLilith.IsChecked = main.settings[0].dispPlanetLilith == 1;
            dispPlanetEarth.IsChecked = main.settings[0].dispPlanetEarth == 1;
            dispPlanetCeres.IsChecked = main.settings[0].dispPlanetCeres == 1;
            dispPlanetPallas.IsChecked = main.settings[0].dispPlanetPallas == 1;
            dispPlanetJuno.IsChecked = main.settings[0].dispPlanetJuno == 1;
            dispPlanetVesta.IsChecked = main.settings[0].dispPlanetVesta == 1;

            dispAspectSun.IsChecked = main.settings[0].dispAspectPlanetSun == 1;
            dispAspectMoon.IsChecked = main.settings[0].dispAspectPlanetMoon == 1;
            dispAspectMercury.IsChecked = main.settings[0].dispAspectPlanetMercury == 1;
            dispAspectVenus.IsChecked = main.settings[0].dispAspectPlanetVenus == 1;
            dispAspectMars.IsChecked = main.settings[0].dispAspectPlanetMars == 1;
            dispAspectJupiter.IsChecked = main.settings[0].dispAspectPlanetJupiter == 1;
            dispAspectSaturn.IsChecked = main.settings[0].dispAspectPlanetSaturn == 1;
            dispAspectUranus.IsChecked = main.settings[0].dispAspectPlanetUranus == 1;
            dispAspectNeptune.IsChecked = main.settings[0].dispAspectPlanetNeptune == 1;
            dispAspectPluto.IsChecked = main.settings[0].dispAspectPlanetPluto == 1;
            dispAspectAsc.IsChecked = main.settings[0].dispAspectPlanetAsc == 1;
            dispAspectMc.IsChecked = main.settings[0].dispAspectPlanetMc == 1;
            dispAspectChiron.IsChecked = main.settings[0].dispAspectPlanetChiron == 1;
            dispAspectDH.IsChecked = main.settings[0].dispAspectPlanetDH == 1;
            dispAspectDT.IsChecked = main.settings[0].dispAspectPlanetDT == 1;
            dispAspectLilith.IsChecked = main.settings[0].dispAspectPlanetLilith == 1;
            dispAspectEarth.IsChecked = main.settings[0].dispAspectPlanetEarth == 1;
            dispAspectCeres.IsChecked = main.settings[0].dispAspectPlanetCeres == 1;
            dispAspectPallas.IsChecked = main.settings[0].dispAspectPlanetPallas == 1;
            dispAspectJuno.IsChecked = main.settings[0].dispAspectPlanetJuno == 1;
            dispAspectVesta.IsChecked = main.settings[0].dispAspectPlanetVesta == 1;

            aspectConjunction.IsChecked = main.settings[0].dispAspectConjunction == 1;
            aspectOpposition.IsChecked = main.settings[0].dispAspectOpposition == 1;
            aspectTrine.IsChecked = main.settings[0].dispAspectTrine == 1;
            aspectSquare.IsChecked = main.settings[0].dispAspectSquare == 1;
            aspectSextile.IsChecked = main.settings[0].dispAspectSextile == 1;
            aspectInconjunct.IsChecked = main.settings[0].dispAspectInconjunct == 1;
            aspectSesquiQuadrate.IsChecked = main.settings[0].dispAspectSesquiQuadrate == 1;
            aspectSemiSquare.IsChecked = main.settings[0].dispAspectSemiSquare == 1;
            aspectSemiSextile.IsChecked = main.settings[0].dispAspectSemiSextile == 1;
            aspectQuintile.IsChecked = main.settings[0].dispAspectQuintile == 1;
            aspectNovile.IsChecked = main.settings[0].dispAspectNovile == 1;
            aspectBiQuintile.IsChecked = main.settings[0].dispAspectBiQuintile == 1;
            aspectSemiQuintile.IsChecked = main.settings[0].dispAspectSemiQuintile == 1;
            aspectSeptile.IsChecked = main.settings[0].dispAspectSeptile == 1;
            aspectQuindecile.IsChecked = main.settings[0].dispAspectQuindecile == 1;


            orbSunMoonSoft.Text = main.settings[0].orbSunMoon[0].ToString();
            orbSunMoonHard.Text = main.settings[0].orbSunMoon[1].ToString();
            orb1stSoft.Text = main.settings[0].orb1st[0].ToString();
            orb1stHard.Text = main.settings[0].orb1st[1].ToString();
            orb2ndSoft.Text = main.settings[0].orb2nd[0].ToString();
            orb2ndHard.Text = main.settings[0].orb2nd[1].ToString();

            saveDispNameLabel.Content = "";
            SavePlanetLabel.Content = "";
            SaveAspectLabel.Content = "";
            SaveAspectKindLabel.Content = "";
            SaveOrbsLabel.Content = "";

            if (main.settings[0].houseCalc == config.EHouseCalc.PLACIDUS)
            {
                placidus.IsChecked = true;
            }
            else if (main.settings[0].houseCalc == config.EHouseCalc.KOCH)
            {
                koch.IsChecked = true;
            }
            else if (main.settings[0].houseCalc == config.EHouseCalc.CAMPANUS)
            {
                campanus.IsChecked = true;
            }
            else if (main.settings[0].houseCalc == config.EHouseCalc.EQUAL)
            {
                equal.IsChecked = true;
            }
            else if (main.settings[0].houseCalc == config.EHouseCalc.ZEROARIES)
            {
                zeroaries.IsChecked = true;
            }

            if (main.settings[0].progression == config.EProgression.SECONDARY)
            {
                secondaryProgression.IsChecked = true;
            }
            else if (main.settings[0].progression == config.EProgression.PRIMARY)
            {
                koch.IsChecked = true;
            }
            else if (main.settings[0].progression == config.EProgression.SOLAR)
            {
                solarArcProgression.IsChecked = true;
            }
            else if (main.settings[0].progression == config.EProgression.CPS)
            {
                compositProgression.IsChecked = true;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Collapsed;
        }

        private void SettingDispNameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView view = (ListView)sender;
            int index = view.SelectedIndex;
            if (view.SelectedIndex == -1)
            {
                index = 0;
            }

            dispName.Text = main.settings[index].dispName;

            dispPlanetSun.IsChecked = main.settings[index].dispPlanetSun == 1;
            dispPlanetMoon.IsChecked = main.settings[index].dispPlanetMoon == 1;
            dispPlanetMercury.IsChecked = main.settings[index].dispPlanetMercury == 1;
            dispPlanetVenus.IsChecked = main.settings[index].dispPlanetVenus == 1;
            dispPlanetMars.IsChecked = main.settings[index].dispPlanetMars == 1;
            dispPlanetJupiter.IsChecked = main.settings[index].dispPlanetJupiter == 1;
            dispPlanetSaturn.IsChecked = main.settings[index].dispPlanetSaturn == 1;
            dispPlanetUranus.IsChecked = main.settings[index].dispPlanetUranus == 1;
            dispPlanetNeptune.IsChecked = main.settings[index].dispPlanetNeptune == 1;
            dispPlanetPluto.IsChecked = main.settings[index].dispPlanetPluto == 1;
            dispPlanetAsc.IsChecked = main.settings[index].dispPlanetAsc == 1;
            dispPlanetMc.IsChecked = main.settings[index].dispPlanetMc == 1;
            dispPlanetChiron.IsChecked = main.settings[index].dispPlanetChiron == 1;
            dispPlanetDH.IsChecked = main.settings[index].dispPlanetDH == 1;
            dispPlanetDT.IsChecked = main.settings[index].dispPlanetDT == 1;
            dispPlanetLilith.IsChecked = main.settings[index].dispPlanetLilith == 1;
            dispPlanetEarth.IsChecked = main.settings[index].dispPlanetEarth == 1;
            dispPlanetCeres.IsChecked = main.settings[index].dispPlanetCeres == 1;
            dispPlanetPallas.IsChecked = main.settings[index].dispPlanetPallas == 1;
            dispPlanetJuno.IsChecked = main.settings[index].dispPlanetJuno == 1;
            dispPlanetVesta.IsChecked = main.settings[index].dispPlanetVesta == 1;

            dispAspectSun.IsChecked = main.settings[index].dispAspectPlanetSun == 1;
            dispAspectMoon.IsChecked = main.settings[index].dispAspectPlanetMoon == 1;
            dispAspectMercury.IsChecked = main.settings[index].dispAspectPlanetMercury == 1;
            dispAspectVenus.IsChecked = main.settings[index].dispAspectPlanetVenus == 1;
            dispAspectMars.IsChecked = main.settings[index].dispAspectPlanetMars == 1;
            dispAspectJupiter.IsChecked = main.settings[index].dispAspectPlanetJupiter == 1;
            dispAspectSaturn.IsChecked = main.settings[index].dispAspectPlanetSaturn == 1;
            dispAspectUranus.IsChecked = main.settings[index].dispAspectPlanetUranus == 1;
            dispAspectNeptune.IsChecked = main.settings[index].dispAspectPlanetNeptune == 1;
            dispAspectPluto.IsChecked = main.settings[index].dispAspectPlanetPluto == 1;
            dispAspectAsc.IsChecked = main.settings[index].dispAspectPlanetAsc == 1;
            dispAspectMc.IsChecked = main.settings[index].dispAspectPlanetMc == 1;
            dispAspectChiron.IsChecked = main.settings[index].dispAspectPlanetChiron == 1;
            dispAspectDH.IsChecked = main.settings[index].dispAspectPlanetDH == 1;
            dispAspectDT.IsChecked = main.settings[index].dispAspectPlanetDT == 1;
            dispAspectLilith.IsChecked = main.settings[index].dispAspectPlanetLilith == 1;
            dispAspectEarth.IsChecked = main.settings[index].dispAspectPlanetEarth == 1;
            dispAspectCeres.IsChecked = main.settings[index].dispAspectPlanetCeres == 1;
            dispAspectPallas.IsChecked = main.settings[index].dispAspectPlanetPallas == 1;
            dispAspectJuno.IsChecked = main.settings[index].dispAspectPlanetJuno == 1;
            dispAspectVesta.IsChecked = main.settings[index].dispAspectPlanetVesta == 1;


            aspectConjunction.IsChecked = main.settings[index].dispAspectConjunction == 1;
            aspectOpposition.IsChecked = main.settings[index].dispAspectOpposition == 1;
            aspectTrine.IsChecked = main.settings[index].dispAspectTrine == 1;
            aspectSquare.IsChecked = main.settings[index].dispAspectSquare == 1;
            aspectSextile.IsChecked = main.settings[index].dispAspectSextile == 1;
            aspectInconjunct.IsChecked = main.settings[index].dispAspectInconjunct == 1;
            aspectSesquiQuadrate.IsChecked = main.settings[index].dispAspectSesquiQuadrate == 1;
            aspectSemiSquare.IsChecked = main.settings[index].dispAspectSemiSquare == 1;
            aspectSemiSextile.IsChecked = main.settings[index].dispAspectSemiSextile == 1;
            aspectQuintile.IsChecked = main.settings[index].dispAspectQuintile == 1;
            aspectNovile.IsChecked = main.settings[index].dispAspectNovile == 1;
            aspectBiQuintile.IsChecked = main.settings[index].dispAspectQuintile == 1;
            aspectSemiQuintile.IsChecked = main.settings[index].dispAspectSemiQuintile == 1;
            aspectSeptile.IsChecked = main.settings[index].dispAspectSeptile == 1;
            aspectQuindecile.IsChecked = main.settings[index].dispAspectQuindecile == 1;

            orbSunMoonSoft.Text = main.settings[index].orbSunMoon[0].ToString();
            orbSunMoonHard.Text = main.settings[index].orbSunMoon[1].ToString();
            orb1stSoft.Text = main.settings[index].orb1st[0].ToString();
            orb1stHard.Text = main.settings[index].orb1st[1].ToString();
            orb2ndSoft.Text = main.settings[index].orb2nd[0].ToString();
            orb2ndHard.Text = main.settings[index].orb2nd[1].ToString();


            saveDispNameLabel.Content = "";
            SavePlanetLabel.Content = "";
            SaveAspectLabel.Content = "";
            SaveAspectKindLabel.Content = "";
            SaveOrbsLabel.Content = "";

            if (main.settings[index].houseCalc == config.EHouseCalc.PLACIDUS)
            {
                placidus.IsChecked = true;
            }
            else if (main.settings[index].houseCalc == config.EHouseCalc.KOCH)
            {
                koch.IsChecked = true;
            }
            else if (main.settings[index].houseCalc == config.EHouseCalc.CAMPANUS)
            {
                campanus.IsChecked = true;
            }
            else if (main.settings[index].houseCalc == config.EHouseCalc.EQUAL)
            {
                equal.IsChecked = true;
            }
            else if (main.settings[index].houseCalc == config.EHouseCalc.ZEROARIES)
            {
                zeroaries.IsChecked = true;
            }

            if (main.settings[index].progression == config.EProgression.SECONDARY)
            {
                secondaryProgression.IsChecked = true;
            }
            else if (main.settings[index].progression == config.EProgression.PRIMARY)
            {
                primaryProgression.IsChecked = true;
            }
            else if (main.settings[index].progression == config.EProgression.SOLAR)
            {
                solarArcProgression.IsChecked = true;
            }
            else if (main.settings[index].progression == config.EProgression.CPS)
            {
                compositProgression.IsChecked = true;
            }
        }

        private void saveDispName_Click(object sender, RoutedEventArgs e)
        {
            int index = SettingDispNameList.SelectedIndex;
            if (index == -1)
            {
                index = 0;
            }
            SettingData setting = main.settings[index];
            setting.dispName = dispName.Text;
            vm.ReSet(main.settings);

            saveDispNameLabel.Content = "保存しました";

            SettingSave(index, setting);
            vm.ReSet(main.settings);
            SettingDispNameList.SelectedIndex = index;
            main.ReCalc();
            main.ReRender();

        }

        private void SettingSave(int index, SettingData setting)
        {
            string fileName = Util.root() + String.Format(@"\system/setting{0}.json", index);

            SettingJson settingJson = new SettingJson(setting);
            string settingJsonStr = JsonSerializer.Serialize(settingJson, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true,
            });
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(settingJsonStr);
                sw.Close();
            }

        }

        private void SaveOrbs_Click(object sender, RoutedEventArgs e)
        {
            int index = SettingDispNameList.SelectedIndex;
            if (index == -1)
            {
                index = 0;
            }
            SettingData setting = main.settings[index];
            setting.orbSunMoon[0] = Double.Parse(orbSunMoonSoft.Text);
            setting.orbSunMoon[1] = Double.Parse(orbSunMoonHard.Text);
            setting.orb1st[0] = Double.Parse(orb1stSoft.Text);
            setting.orb1st[1] = Double.Parse(orb1stHard.Text);
            setting.orb2nd[0] = Double.Parse(orb2ndSoft.Text);
            setting.orb2nd[1] = Double.Parse(orb2ndHard.Text);
            SaveOrbsLabel.Content = "保存しました";

            SettingSave(index, setting);
            main.ReCalc();
            main.ReRender();

        }

        private void SaveAspectKind_Click(object sender, RoutedEventArgs e)
        {
            int index = SettingDispNameList.SelectedIndex;
            if (index == -1)
            {
                index = 0;
            }
            SettingData setting = main.settings[index];
            setting.dispAspectConjunction = aspectConjunction.IsChecked == true ? 1 : 0;
            setting.dispAspectOpposition = aspectOpposition.IsChecked == true ? 1 : 0;
            setting.dispAspectTrine = aspectTrine.IsChecked == true ? 1 : 0;
            setting.dispAspectSquare = aspectSquare.IsChecked == true ? 1 : 0;
            setting.dispAspectSextile = aspectSextile.IsChecked == true ? 1 : 0;
            setting.dispAspectInconjunct = aspectInconjunct.IsChecked == true ? 1 : 0;
            setting.dispAspectSesquiQuadrate = aspectSesquiQuadrate.IsChecked == true ? 1 : 0;
            setting.dispAspectSemiSquare = aspectSemiSquare.IsChecked == true ? 1 : 0;
            setting.dispAspectSemiSextile = aspectSemiSextile.IsChecked == true ? 1 : 0;
            setting.dispAspectQuintile = aspectQuintile.IsChecked == true ? 1 : 0;
            setting.dispAspectSemiQuintile= aspectSemiQuintile.IsChecked == true ? 1 : 0;
            setting.dispAspectSeptile = aspectSeptile.IsChecked == true ? 1 : 0;
            setting.dispAspectNovile = aspectNovile.IsChecked == true ? 1 : 0;
            setting.dispAspectBiQuintile = aspectBiQuintile.IsChecked == true ? 1 : 0;
            setting.dispAspectQuindecile = aspectQuindecile.IsChecked == true ? 1 : 0;

            SaveAspectKindLabel.Content = "保存しました";

            SettingSave(index, setting);
            main.ReCalc();
            main.ReRender();

        }

        private void SavePlanet_Click(object sender, RoutedEventArgs e)
        {
            int index = SettingDispNameList.SelectedIndex;
            if (index == -1)
            {
                index = 0;
            }
            SettingData setting = main.settings[index];
            setting.dispPlanetSun = dispPlanetSun.IsChecked == true ? 1 : 0;
            setting.dispPlanetMoon = dispPlanetMoon.IsChecked == true ? 1 : 0;
            setting.dispPlanetMercury = dispPlanetMercury.IsChecked == true ? 1 : 0;
            setting.dispPlanetVenus = dispPlanetVenus.IsChecked == true ? 1 : 0;
            setting.dispPlanetMars = dispPlanetMars.IsChecked == true ? 1 : 0;
            setting.dispPlanetJupiter = dispPlanetJupiter.IsChecked == true ? 1 : 0;
            setting.dispPlanetSaturn = dispPlanetSaturn.IsChecked == true ? 1 : 0;
            setting.dispPlanetUranus = dispPlanetUranus.IsChecked == true ? 1 : 0;
            setting.dispPlanetNeptune = dispPlanetNeptune.IsChecked == true ? 1 : 0;
            setting.dispPlanetPluto = dispPlanetPluto.IsChecked == true ? 1 : 0;
            setting.dispPlanetAsc = dispPlanetAsc.IsChecked == true ? 1 : 0;
            setting.dispPlanetMc = dispPlanetMc.IsChecked == true ? 1 : 0;
            setting.dispPlanetChiron = dispPlanetChiron.IsChecked == true ? 1 : 0;
            setting.dispPlanetDH = dispPlanetDH.IsChecked == true ? 1 : 0;
            setting.dispPlanetDT = dispPlanetDT.IsChecked == true ? 1 : 0;
            setting.dispPlanetLilith = dispPlanetLilith.IsChecked == true ? 1 : 0;
            setting.dispPlanetEarth = dispPlanetEarth.IsChecked == true ? 1 : 0;
            setting.dispPlanetCeres = dispPlanetCeres.IsChecked == true ? 1 : 0;
            setting.dispPlanetPallas = dispPlanetPallas.IsChecked == true ? 1 : 0;
            setting.dispPlanetJuno = dispPlanetJuno.IsChecked == true ? 1 : 0;
            setting.dispPlanetVesta = dispPlanetVesta.IsChecked == true ? 1 : 0;

            SavePlanetLabel.Content = "保存しました";

            SettingSave(index, setting);
            main.ReCalc();
            main.ReRender();

        }

        private void SaveAspectPlanet_Click(object sender, RoutedEventArgs e)
        {
            int index = SettingDispNameList.SelectedIndex;
            if (index == -1)
            {
                index = 0;
            }
            SettingData setting = main.settings[index];
            setting.dispAspectPlanetSun = dispAspectSun.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetMoon = dispAspectMoon.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetMercury = dispAspectMercury.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetVenus = dispAspectVenus.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetMars = dispAspectMars.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetJupiter = dispAspectJupiter.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetSaturn = dispAspectSaturn.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetUranus = dispAspectUranus.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetNeptune = dispAspectNeptune.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetPluto = dispAspectPluto.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetAsc = dispAspectAsc.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetMc = dispAspectMc.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetChiron = dispAspectChiron.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetDH = dispAspectDH.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetDT = dispAspectDT.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetLilith = dispAspectLilith.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetEarth = dispAspectEarth.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetCeres = dispAspectCeres.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetPallas = dispAspectPallas.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetJuno = dispAspectJuno.IsChecked == true ? 1 : 0;
            setting.dispAspectPlanetVesta = dispAspectVesta.IsChecked == true ? 1 : 0;

            SaveAspectLabel.Content = "保存しました";

            SettingSave(index, setting);
            main.ReCalc();
            main.ReRender();

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            saveDispNameLabel.Content = "";
            SavePlanetLabel.Content = "";
            SaveAspectLabel.Content = "";
            SaveAspectKindLabel.Content = "";
            SaveOrbsLabel.Content = "";

        }

        private void saveDispKind_Click(object sender, RoutedEventArgs e)
        {
            int index = SettingDispNameList.SelectedIndex;
            if (index == -1)
            {
                index = 0;
            }
            SettingData setting = main.settings[index];
            if (secondaryProgression.IsChecked == true)
            {
                setting.progression = EProgression.SECONDARY;
            }
            else if (primaryProgression.IsChecked == true)
            {
                setting.progression = EProgression.PRIMARY;
            }
            else if (solarArcProgression.IsChecked == true)
            {
                setting.progression = EProgression.SOLAR;
            }
            else if (compositProgression.IsChecked == true)
            {
                setting.progression = EProgression.CPS;
            }

            if (placidus.IsChecked == true)
            {
                setting.houseCalc = EHouseCalc.PLACIDUS;
            }
            else if (koch.IsChecked == true)
            {
                setting.houseCalc = EHouseCalc.KOCH;
            }
            else if (campanus.IsChecked == true)
            {
                setting.houseCalc = EHouseCalc.CAMPANUS;
            }
            else if (equal.IsChecked == true)
            {
                setting.houseCalc = EHouseCalc.EQUAL;
            }
            else if (zeroaries.IsChecked == true)
            {
                setting.houseCalc = EHouseCalc.ZEROARIES;
            }

            saveDispKind.Content = "保存しました";

            SettingSave(index, setting);
            main.ReCalc();
            main.ReRender();
        }
    }
}
