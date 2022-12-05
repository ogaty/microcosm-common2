using microcosm.common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.ViewModel
{
    public class DatabaseWindowViewModel
    {
        public DatabaseWindow databaseWindow;

        public ObservableCollection<UserFileListData> fileList { get; set; }

        public DatabaseWindowViewModel(DatabaseWindow databaseWindow)
        {
            this.databaseWindow = databaseWindow;
            fileList = new ObservableCollection<UserFileListData>();
            string root = Util.root();

            if (!Directory.Exists(root + @"\data"))
            {
                _ = Directory.CreateDirectory(root + @"\data");
            }
            string[] files = Directory.GetFiles(root + @"\data", "*.json");

            foreach (var file in files)
            {
                Debug.WriteLine(Path.GetFileName(file));
                fileList.Add(new UserFileListData() { 
                    fileName = Path.GetFileName(file),
                    fileNameFullPath = file
                });
            }
        }

        public void ReRender()
        {
            fileList.Clear();
            string root = Util.root();
            string[] files = Directory.GetFiles(databaseWindow.currentFullPath);
            foreach (var file in files)
            {
                fileList.Add(new UserFileListData()
                {
                    fileName = Path.GetFileName(file),
                    fileNameFullPath = file
                });
            }
        }

        public void Clear()
        {
            fileList.Clear();
        }
    }
}
