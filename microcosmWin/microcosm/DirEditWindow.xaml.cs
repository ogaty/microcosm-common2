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
    /// DirEditWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DirEditWindow : Window
    {
        public MainWindow mainWindow;
        public DatabaseWindow databaseWindow;
        public string targetFullPath;

        public DirEditWindow()
        {
            InitializeComponent();
        }

        public DirEditWindow(MainWindow main, DatabaseWindow databaseWindow)
        {
            InitializeComponent();
            mainWindow = main;
            this.databaseWindow = databaseWindow;
        }

        public void SetData(string file, string targetFullPath)
        {
            fileName.Text = file;
            this.targetFullPath = targetFullPath;
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


            DirectoryInfo dir = new DirectoryInfo(targetFullPath);
            if (dir.Name == fileName.Text)
            {
                this.Visibility = Visibility.Hidden;
                return;
            }
            try
            {
                Directory.Move(targetFullPath, Util.root() + @"\data\" + fileName.Text);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageBox.Show("エラーが発生しました。");
                return;
            }
            // ディレクトリ変更なのでカレントパスは変わらない
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
