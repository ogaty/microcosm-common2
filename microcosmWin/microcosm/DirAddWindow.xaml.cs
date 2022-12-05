using microcosm.common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// DirAddWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DirAddWindow : Window
    {
        public MainWindow mainWindow;
        public DatabaseWindow databaseWindow;

        public DirAddWindow()
        {
            InitializeComponent();
        }

        public DirAddWindow(MainWindow main, DatabaseWindow dbWindow)
        {
            InitializeComponent();
            mainWindow = main;
            databaseWindow = dbWindow;
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            string root = Util.root();

            if (fileName.Text.IndexOf(@"\") >= 0)
            {
                MessageBox.Show("そのファイル名は指定できません。");
                return;
            }
            if (fileName.Text.IndexOf(@"/") >= 0)
            {
                MessageBox.Show("そのファイル名は指定できません。");
                return;
            }

            try
            {
                Directory.CreateDirectory(root + @"\data\" + fileName.Text);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageBox.Show("エラーが発生しました。");
                return;
            }
            // サブディレクトリにするならこっち
            //DirectoryInfo dir = new DirectoryInfo(databaseWindow.currentFullPath);
            //dir.CreateSubdirectory(fileName.Text);
            databaseWindow.ReRenderDir();
            this.Visibility = Visibility.Hidden;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Collapsed;
        }
    }
}
