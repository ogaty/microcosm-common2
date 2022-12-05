using microcosm.common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace microcosm.ViewModel
{
    public class DatabaseWindowDirViewModel
    {
        public DatabaseWindow databaseWindow;

        public ObservableCollection<UserFileListData> dirList { get; set; }

        public DatabaseWindowDirViewModel(DatabaseWindow databaseWindow)
        {
            this.databaseWindow = databaseWindow;

            dirList = new ObservableCollection<UserFileListData>();
            string root = Util.root();

            if (!Directory.Exists(root + @"\data"))
            {
                _ = Directory.CreateDirectory(root + @"\data");
            }
            DirectoryInfo dir = new DirectoryInfo(root + @"\data");
            DirectoryInfo[] subdirList = dir.GetDirectories();

            dirList.Add(new UserFileListData()
            {
                fileName = ".",
                fileNameFullPath = Util.root() + @"\data"
            });
            foreach (var file in subdirList)
            {
                dirList.Add(new UserFileListData()
                {
                    fileName = file.Name,
                    fileNameFullPath = file.FullName
                });
            }
        }

        public void ReRender()
        {
            dirList.Clear();
            DirectoryInfo dir = new DirectoryInfo(Util.root() + @"\data");
            DirectoryInfo[] subdirList = dir.GetDirectories();

            string root = Util.root();
            dirList.Add(new UserFileListData()
            {
                fileName = ".",
                fileNameFullPath = Util.root() + @"\data"
            });

            foreach (var file in subdirList)
            {
                dirList.Add(new UserFileListData()
                {
                    fileName = file.Name,
                    fileNameFullPath = file.FullName
                });
            }
        }
    }
}
