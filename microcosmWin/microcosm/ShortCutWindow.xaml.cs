using microcosm.common;
using microcosm.config;
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
    /// ShortCutWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ShortCutWindow : Window
    {
        public MainWindow main;

        public ShortCutWindow()
        {
            InitializeComponent();
        }

        public ShortCutWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.main = mainWindow;

            Dictionary<EShortCut, string> shortCutList = Util.createShortCut();

            ShortCut shortcut = null;
            string root = Util.root();
            string systemDirName = root + @"\system";
            string fileName = systemDirName + @"\shortcut.json";
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                string jsonData = sr.ReadToEnd();
                shortcut = JsonSerializer.Deserialize<ShortCut>(jsonData);
            }

            int index = 0;
            foreach (KeyValuePair<EShortCut, string> s in shortCutList)
            {
                ctrlH.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrlH]) ctrlH.SelectedIndex = index;
                ctrlJ.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrlJ]) ctrlJ.SelectedIndex = index;
                ctrlK.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrlK]) ctrlK.SelectedIndex = index;
                ctrlL.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrlL]) ctrlL.SelectedIndex = index;
                ctrlN.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrlN]) ctrlN.SelectedIndex = index;
                ctrlM.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrlM]) ctrlM.SelectedIndex = index;
                ctrlComma.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrlComma]) ctrlComma.SelectedIndex = index;
                ctrlDot.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrlDot]) ctrlDot.SelectedIndex = index;
                ctrlOpenBracket.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrlOpenBracket]) ctrlOpenBracket.SelectedIndex = index;
                ctrlCloseBracket.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrlCloseBracket]) ctrlCloseBracket.SelectedIndex = index;
                ctrl0.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrl0]) ctrl0.SelectedIndex = index;
                ctrl1.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrl1]) ctrl1.SelectedIndex = index;
                ctrl2.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrl2]) ctrl2.SelectedIndex = index;
                ctrl3.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrl3]) ctrl3.SelectedIndex = index;
                ctrl4.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrl4]) ctrl4.SelectedIndex = index;
                ctrl5.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrl5]) ctrl5.SelectedIndex = index;
                ctrl6.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrl6]) ctrl6.SelectedIndex = index;
                ctrl7.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrl7]) ctrl7.SelectedIndex = index;
                ctrl8.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrl8]) ctrl8.SelectedIndex = index;
                ctrl9.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.ctrl9]) ctrl9.SelectedIndex = index;
                f1.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.F1]) f1.SelectedIndex = index;
                f2.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.F2]) f2.SelectedIndex = index;
                f3.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.F3]) f3.SelectedIndex = index;
                f4.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.F4]) f4.SelectedIndex = index;
                f5.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.F5]) f5.SelectedIndex = index;
                f6.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.F6]) f6.SelectedIndex = index;
                f7.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.F7]) f7.SelectedIndex = index;
                f8.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.F8]) f8.SelectedIndex = index;
                f9.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.F9]) f9.SelectedIndex = index;
                f10.Items.Add(s.Value);
                if (s.Value == shortCutList[shortcut.F10]) f10.SelectedIndex = index;
                index++;
            }

            main.shortCutData = shortcut;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Collapsed;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            main.shortCutData.ctrlH = Util.ShortCutStringToEnum((string)ctrlH.SelectedItem);
            main.shortCutData.ctrlJ = Util.ShortCutStringToEnum((string)ctrlJ.SelectedItem);
            main.shortCutData.ctrlK = Util.ShortCutStringToEnum((string)ctrlK.SelectedItem);
            main.shortCutData.ctrlL = Util.ShortCutStringToEnum((string)ctrlL.SelectedItem);
            main.shortCutData.ctrlN = Util.ShortCutStringToEnum((string)ctrlN.SelectedItem);
            main.shortCutData.ctrlM = Util.ShortCutStringToEnum((string)ctrlM.SelectedItem);
            main.shortCutData.ctrlComma = Util.ShortCutStringToEnum((string)ctrlComma.SelectedItem);
            main.shortCutData.ctrlDot = Util.ShortCutStringToEnum((string)ctrlDot.SelectedItem);
            main.shortCutData.ctrlOpenBracket = Util.ShortCutStringToEnum((string)ctrlOpenBracket.SelectedItem);
            main.shortCutData.ctrlCloseBracket = Util.ShortCutStringToEnum((string)ctrlCloseBracket.SelectedItem);
            main.shortCutData.ctrl0 = Util.ShortCutStringToEnum((string)ctrl0.SelectedItem);
            main.shortCutData.ctrl1 = Util.ShortCutStringToEnum((string)ctrl1.SelectedItem);
            main.shortCutData.ctrl2 = Util.ShortCutStringToEnum((string)ctrl2.SelectedItem);
            main.shortCutData.ctrl3 = Util.ShortCutStringToEnum((string)ctrl3.SelectedItem);
            main.shortCutData.ctrl4 = Util.ShortCutStringToEnum((string)ctrl4.SelectedItem);
            main.shortCutData.ctrl5 = Util.ShortCutStringToEnum((string)ctrl5.SelectedItem);
            main.shortCutData.ctrl6 = Util.ShortCutStringToEnum((string)ctrl6.SelectedItem);
            main.shortCutData.ctrl7 = Util.ShortCutStringToEnum((string)ctrl7.SelectedItem);
            main.shortCutData.ctrl8 = Util.ShortCutStringToEnum((string)ctrl8.SelectedItem);
            main.shortCutData.ctrl9 = Util.ShortCutStringToEnum((string)ctrl9.SelectedItem);
            main.shortCutData.F1 = Util.ShortCutStringToEnum((string)f1.SelectedItem);
            main.shortCutData.F2 = Util.ShortCutStringToEnum((string)f2.SelectedItem);
            main.shortCutData.F3 = Util.ShortCutStringToEnum((string)f3.SelectedItem);
            main.shortCutData.F4 = Util.ShortCutStringToEnum((string)f4.SelectedItem);
            main.shortCutData.F5 = Util.ShortCutStringToEnum((string)f5.SelectedItem);
            main.shortCutData.F6 = Util.ShortCutStringToEnum((string)f6.SelectedItem);
            main.shortCutData.F7 = Util.ShortCutStringToEnum((string)f7.SelectedItem);
            main.shortCutData.F8 = Util.ShortCutStringToEnum((string)f8.SelectedItem);
            main.shortCutData.F9 = Util.ShortCutStringToEnum((string)f9.SelectedItem);
            main.shortCutData.F10 = Util.ShortCutStringToEnum((string)f10.SelectedItem);
            main.keyEventCtrl[4] = main.shortCutData.ctrlH;
            main.keyEventCtrl[38] = main.shortCutData.ctrlJ;
            main.keyEventCtrl[40] = main.shortCutData.ctrlK;
            main.keyEventCtrl[37] = main.shortCutData.ctrlL;
            main.keyEventCtrl[45] = main.shortCutData.ctrlN;
            main.keyEventCtrl[46] = main.shortCutData.ctrlM;
            main.keyEventCtrl[43] = main.shortCutData.ctrlComma;
            main.keyEventCtrl[47] = main.shortCutData.ctrlDot;
            main.keyEventCtrl[33] = main.shortCutData.ctrlOpenBracket;
            main.keyEventCtrl[30] = main.shortCutData.ctrlCloseBracket;
            main.keyEventCtrl[29] = main.shortCutData.ctrl0;
            main.keyEventCtrl[18] = main.shortCutData.ctrl1;
            main.keyEventCtrl[19] = main.shortCutData.ctrl2;
            main.keyEventCtrl[20] = main.shortCutData.ctrl3;
            main.keyEventCtrl[21] = main.shortCutData.ctrl4;
            main.keyEventCtrl[23] = main.shortCutData.ctrl5;
            main.keyEventCtrl[22] = main.shortCutData.ctrl6;
            main.keyEventCtrl[26] = main.shortCutData.ctrl7;
            main.keyEventCtrl[28] = main.shortCutData.ctrl8;
            main.keyEventCtrl[25] = main.shortCutData.ctrl9;
            main.keyEvent[122] = main.shortCutData.F1;
            main.keyEvent[120] = main.shortCutData.F2;
            main.keyEvent[99] = main.shortCutData.F3;
            main.keyEvent[118] = main.shortCutData.F4;
            main.keyEvent[96] = main.shortCutData.F5;
            main.keyEvent[97] = main.shortCutData.F6;
            main.keyEvent[98] = main.shortCutData.F7;
            main.keyEvent[100] = main.shortCutData.F8;
            main.keyEvent[101] = main.shortCutData.F9;
            main.keyEvent[109] = main.shortCutData.F10;
            string root = Util.root();
            string systemDirName = root + @"\system";
            string fileName = systemDirName + @"\shortcut.json";

            string shortCutJson = JsonSerializer.Serialize(main.shortCutData, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true,
            });
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(shortCutJson);
                sw.Close();
            }

            this.Visibility = Visibility.Hidden;
        }
    }
}
