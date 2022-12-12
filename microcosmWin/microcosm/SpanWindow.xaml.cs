using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// SpanWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SpanWindow : Window
    {
        private MainWindow main;
        private int unitValue = 1;
        private enum KindEnum
        {
            Seconds = 0,
            Minutes = 1,
            Hours = 2,
            Days = 3
        };
        private KindEnum kindValue;


        public SpanWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.main = mainWindow;

            SpanCombo.Items.Add("7 Days");
            SpanCombo.Items.Add("30 Days");
            SpanCombo.Items.Add("365 Days");
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            int value = 0; 
            if (!Int32.TryParse(unit.Text, out value))
            {
                MessageBox.Show("整数の値を指定してください。");
                return;
            }
            unitValue = value;


            string kind = "";
            int plusUnit = 0;
            if (RadioSeconds.IsChecked == true)
            {
                kind = " Seconds";
                kindValue = KindEnum.Seconds;
                plusUnit = unitValue;
            }
            if (RadioMinutes.IsChecked == true)
            {
                kind = " Minutes";
                kindValue = KindEnum.Minutes;
                plusUnit = unitValue * 60;
            }
            if (RadioHours.IsChecked == true)
            {
                kind = " Hours";
                kindValue = KindEnum.Hours;
                plusUnit = unitValue * 3600;
            }
            if (RadioDays.IsChecked == true)
            {
                kind = " Days";
                kindValue = KindEnum.Days;
                plusUnit = unitValue * 86400;
            }

            if (RadioUnit.IsChecked == true)
            {
                main.ChangeSpanButton.Content = unit.Text + kind;
                main.plusUnit = plusUnit;
                main.currentSpanType = common.SpanType.UNIT;
            }
            else if (RadioNewMoon.IsChecked == true)
            {
                main.ChangeSpanButton.Content = "NewMoon";
                main.currentSpanType = common.SpanType.NEWMOON;
            }
            else if (RadioFullMoon.IsChecked == true)
            {
                main.ChangeSpanButton.Content = "FullMoon";
                main.currentSpanType = common.SpanType.FULLMOON;
            }
            else if (RadioSolarReturn.IsChecked == true)
            {
                main.ChangeSpanButton.Content = "SolarReturn";
                main.currentSpanType = common.SpanType.SOLARRETURN;
            }
            else if (RadioIngress.IsChecked == true && (string)((ComboBoxItem)IngressCombo.SelectedItem).Content == "sun")
            {
                main.ChangeSpanButton.Content = "Sun Ing.";
                main.currentSpanType = common.SpanType.SOLARINGRESS;
            }
            else if (RadioIngress.IsChecked == true && (string)((ComboBoxItem)IngressCombo.SelectedItem).Content == "moon")
            {
                main.ChangeSpanButton.Content = "Moon Ing.";
                main.currentSpanType = common.SpanType.MOONINGRESS;
            }


            this.Visibility = Visibility.Hidden;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Collapsed;
        }

        private void SpanCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SpanCombo.SelectedIndex == 0)
            {
                RadioDays.IsChecked = true;
                unit.Text = "7";
            }
            else if (SpanCombo.SelectedIndex == 1)
            {
                RadioDays.IsChecked = true;
                unit.Text = "30";
            }
            else if (SpanCombo.SelectedIndex == 2)
            {
                RadioDays.IsChecked = true;
                unit.Text = "365";
            }

        }
    }
}
