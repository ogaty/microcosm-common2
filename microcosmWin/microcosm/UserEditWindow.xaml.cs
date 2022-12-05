using microcosm.common;
using microcosm.Db;
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
    /// UserEditWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class UserEditWindow : Window
    {
        public MainWindow mainWindow;
        public DatabaseWindow databaseWindow;
        public LatLngCsvViewModel vm;
        public string oldFile;

        public UserEditWindow()
        {
            InitializeComponent();
        }

        public UserEditWindow(MainWindow main, DatabaseWindow dbWindow)
        {
            this.mainWindow = main;
            databaseWindow = dbWindow;
            InitializeComponent();

        }

        public void SettingDispNameData(string file)
        {
            oldFile = file;
            fileName.Text = System.IO.Path.GetFileNameWithoutExtension(file);
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            string root = Util.root();

            string newFileName = databaseWindow.currentFullPath + @"\" + fileName.Text + @".json";

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
            if (oldFile != newFileName)
            {
                try
                {
                    File.Move(oldFile, newFileName);
                    databaseWindow.ReRender();
                    databaseWindow.ReRenderEvent(newFileName);
                }
                catch (IOException ex)
                {
                    Debug.WriteLine(ex.Message);
                    MessageBox.Show("エラーが発生しました。");
                    return;
                }
            }
            this.Visibility = Visibility.Hidden;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Collapsed;
        }
    }
}
