using microcosm.common;
using microcosm.Db;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace microcosm.ViewModel
{
    public class DatabaseWindowUserEventListViewModel
    {
        public DatabaseWindow databaseWindow;
        public ObservableCollection<UserEventListData> userEventList { get; set; }

        public DatabaseWindowUserEventListViewModel(DatabaseWindow databaseWindow)
        {
            this.databaseWindow = databaseWindow;
            userEventList = new ObservableCollection<UserEventListData>();
        }

        public void SetUserEvent(string fileName)
        {
            string root = Util.root();

            ReRender(fileName);
        }

        public void AddEventList(UserJsonList userJsonList, string fileFullPath)
        {
            if (userJsonList.list == null)
            {
                MessageBox.Show("不正なJsonです。");
                return;
            }
            int index = 0;
            foreach (UserData userJson in userJsonList.list)
            {
                userEventList.Add(new UserEventListData(
                    index,
                    userJson.name,
                    userJson.birth_year.ToString(),
                    userJson.birth_month.ToString("00"),
                    userJson.birth_day.ToString("00"),
                    userJson.birth_hour.ToString("00"),
                    userJson.birth_minute.ToString("00"),
                    userJson.birth_second.ToString("00"),
                    userJson.timezone_str,
                    userJson.birth_place,
                    userJson.lat.ToString("00.000"),
                    userJson.lng.ToString("000.000"),
                    userJson.memo,
                    System.IO.Path.GetFileName(fileFullPath),
                    fileFullPath
                ));
                index++;
            }
        }

        public void Clear()
        {
            userEventList.Clear();
        }

        public void ReRender(string fileName)
        {
            userEventList.Clear();
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                string userJsonData = sr.ReadToEnd();
                UserJsonList userJsonList = JsonSerializer.Deserialize<UserJsonList>(userJsonData);

                AddEventList(userJsonList, fileName);

            }
        }
    }
}
