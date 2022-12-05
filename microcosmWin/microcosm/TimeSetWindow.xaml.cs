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
    /// TimeSetWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TimeSetWindow : Window
    {
        private MainWindow main;

        public TimeSetWindow()
        {
            InitializeComponent();
        }

        public TimeSetWindow(MainWindow mainWindow)
        {
            InitializeComponent();

            this.main = mainWindow;

            if (main.NT.SelectedIndex == 0)
            {
                TargetDate.SelectedDate = new DateTime(
                    main.user1data.birth_year,
                    main.user1data.birth_month,
                    main.user1data.birth_day,
                    main.user1data.birth_hour,
                    main.user1data.birth_minute,
                    main.user1data.birth_second
                    );

                Hour.Text = main.user1data.birth_hour.ToString();
                Minute.Text = main.user1data.birth_minute.ToString();
                Second.Text = main.user1data.birth_second.ToString();
            }
            else if (main.NT.SelectedIndex == 1)
            {
                TargetDate.SelectedDate = new DateTime(
                    main.user2data.birth_year,
                    main.user2data.birth_month,
                    main.user2data.birth_day,
                    main.user2data.birth_hour,
                    main.user2data.birth_minute,
                    main.user2data.birth_second
                    );

                Hour.Text = main.user2data.birth_hour.ToString();
                Minute.Text = main.user2data.birth_minute.ToString();
                Second.Text = main.user2data.birth_second.ToString();

            }
            else if (main.NT.SelectedIndex == 2)
            {
                TargetDate.SelectedDate = new DateTime(
                    main.event1data.birth_year,
                    main.event1data.birth_month,
                    main.event1data.birth_day,
                    main.event1data.birth_hour,
                    main.event1data.birth_minute,
                    main.event1data.birth_second
                    );

                Hour.Text = main.event1data.birth_hour.ToString();
                Minute.Text = main.event1data.birth_minute.ToString();
                Second.Text = main.event1data.birth_second.ToString();
            }
            else if (main.NT.SelectedIndex == 3)
            {
                TargetDate.SelectedDate = new DateTime(
                    main.event2data.birth_year,
                    main.event2data.birth_month,
                    main.event2data.birth_day,
                    main.event2data.birth_hour,
                    main.event2data.birth_minute,
                    main.event2data.birth_second
                    );

                Hour.Text = main.event2data.birth_hour.ToString();
                Minute.Text = main.event2data.birth_minute.ToString();
                Second.Text = main.event2data.birth_second.ToString();

            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (TargetDate.SelectedDate == null)
            {
                return;
            }
            int h = 0;
            if (!Int32.TryParse(Hour.Text, out h))
            {
                MessageBox.Show("時刻は数値で指定してください。");
                return;
            }
            int m = 0;
            if (!Int32.TryParse(Minute.Text, out m))
            {
                MessageBox.Show("時刻は数値で指定してください。");
                return;
            }
            int s = 0;
            if (!Int32.TryParse(Second.Text, out s))
            {
                MessageBox.Show("時刻は数値で指定してください。");
                return;
            }
            DateTime dt = new DateTime((int)TargetDate.SelectedDate?.Year,
                (int)TargetDate.SelectedDate?.Month,
                (int)TargetDate.SelectedDate?.Day,
                h,
                m,
                s
                );

            if (main.NT.SelectedIndex == 0)
            {
                main.user1data.birth_year = dt.Year;
                main.user1data.birth_month = dt.Month;
                main.user1data.birth_day = dt.Day;

                main.user1data.birth_hour = dt.Hour;
                main.user1data.birth_minute = dt.Minute;
                main.user1data.birth_second = dt.Second;

                main.ReCalc();
                main.mainWindowVM.ReSet(0, main.mainWindowVM.userName, dt, main.mainWindowVM.userBirthPlace, main.mainWindowVM.userLatLng, main.mainWindowVM.userTimezone);
            }
            else if (main.NT.SelectedIndex == 1)
            {
                main.user2data.birth_year = dt.Year;
                main.user2data.birth_month = dt.Month;
                main.user2data.birth_day = dt.Day;

                main.user2data.birth_hour = dt.Hour;
                main.user2data.birth_minute = dt.Minute;
                main.user2data.birth_second = dt.Second;

                main.ReCalc();
                main.mainWindowVM.ReSet(1, main.mainWindowVM.user2Name, dt, main.mainWindowVM.user2BirthPlace, main.mainWindowVM.user2LatLng, main.mainWindowVM.user2Timezone);
            }
            else if (main.NT.SelectedIndex == 2)
            {
                main.event1data.birth_year = dt.Year;
                main.event1data.birth_month = dt.Month;
                main.event1data.birth_day = dt.Day;

                main.event1data.birth_hour = dt.Hour;
                main.event1data.birth_minute = dt.Minute;
                main.event1data.birth_second = dt.Second;

                main.ReCalc();
                main.mainWindowVM.ReSet(2, main.mainWindowVM.transitName, dt, main.mainWindowVM.transitBirthPlace, main.mainWindowVM.transitLatLng, main.mainWindowVM.transitTimezone);
            }
            else if (main.NT.SelectedIndex == 3)
            {
                main.event2data.birth_year = dt.Year;
                main.event2data.birth_month = dt.Month;
                main.event2data.birth_day = dt.Day;

                main.event2data.birth_hour = dt.Hour;
                main.event2data.birth_minute = dt.Minute;
                main.event2data.birth_second = dt.Second;

                main.ReCalc();
                main.mainWindowVM.ReSet(3, main.mainWindowVM.transit2Name, dt, main.mainWindowVM.transit2BirthPlace, main.mainWindowVM.transit2LatLng, main.mainWindowVM.transit2Timezone);
            }
            main.ReRender();
            this.Visibility = Visibility.Collapsed;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Collapsed;
        }

    }
}
