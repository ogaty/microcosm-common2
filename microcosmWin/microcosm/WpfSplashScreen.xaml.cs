using microcosm.Aspect;
using microcosm.calc;
using microcosm.common;
using microcosm.config;
using microcosm.Db;
using microcosm.Planet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;


namespace microcosm
{
    /// <summary>
    /// WpfSplashScreen.xaml の相互作用ロジック
    /// </summary>
    public partial class WpfSplashScreen : Window
    {
        ConfigData configData;
        SettingData[] settings = new SettingData[10];
        List<string> sabians;

        UserData user1data;
        UserData user2data;
        UserData event1data;
        UserData event2data;

        Dictionary<int, PlanetData> list1;
        Dictionary<int, PlanetData> list2;
        Dictionary<int, PlanetData> list3;

        double[] houseList1;
        double[] houseList2;
        double[] houseList3;

        TempSetting tempSettings;
        AstroCalc calc;

        public WpfSplashScreen()
        {
            InitializeComponent();
            Onloading();
        }

        private async void Onloading()
        {
            await Task.Run(() =>
            {
                DataInit();
                FirstCalc();

                for (int i = 0; i <= 50; i++)
                {
                    Thread.Sleep(10);
                }
            });

            BaseData baseData = new BaseData();

            baseData.configData = configData;
            baseData.settings = settings;
            baseData.sabians = sabians;
            baseData.user1data = user1data;
            baseData.user2data = user2data;
            baseData.event1data= event1data;
            baseData.event2data= event2data;
            baseData.list1 = list1;
            baseData.list2 = list2;
            baseData.list3 = list3;
            baseData.houseList1 = houseList1;
            baseData.houseList2 = houseList2;
            baseData.houseList3 = houseList3;
            baseData.currentSetting = settings[0];
            baseData.tempSetting = tempSettings;
            baseData.calc = calc;


            MainWindow mainWindow = new MainWindow(baseData);
            mainWindow.Show();
            Close();
        }

        public void DataInit()
        {
            DateTime now = DateTime.Now;
            string root = Util.root();
            string systemDirName = root + @"\system";
            string epheDirName = root + @"\ephe";
            string dataDirName = root + @"\data";
            string licenseDirName = root + @"\license";
            string exePath = Environment.GetCommandLineArgs()[0];
            string exeDir = System.IO.Path.GetDirectoryName(exePath);
            if (!Directory.Exists(systemDirName))
            {
                _ = Directory.CreateDirectory(systemDirName);
            }
            if (!Directory.Exists(epheDirName))
            {
                _ = Directory.CreateDirectory(epheDirName);
            }
            if (!Directory.Exists(dataDirName))
            {
                _ = Directory.CreateDirectory(dataDirName);
            }
            if (!Directory.Exists(licenseDirName))
            {
                _ = Directory.CreateDirectory(licenseDirName);
            }
            string filename = systemDirName + @"\config.json";
            if (!File.Exists(filename))
            {
                configData = new ConfigData(root + @"\ephe");
                // 生成も
                string configJson = JsonSerializer.Serialize(configData, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true,
                });
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(configJson);
                    sw.Close();
                }
            }
            else
            {
                // 読み込み
                try
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        mainText.Text = "コンフィグ読み込み";
                    }));

                    using (FileStream fs = new FileStream(filename, FileMode.Open))
                    {
                        StreamReader sr = new StreamReader(fs);
                        string jsonData = sr.ReadToEnd();
                        configData = JsonSerializer.Deserialize<ConfigData>(jsonData);
                    }
                }
                catch
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        mainText.Text = "コンフィグ再作成";
                    }));
                    configData = new ConfigData(epheDirName);

                    string configJson = JsonSerializer.Serialize(configData, new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                        WriteIndented = true,
                    });
                    using (FileStream fs = new FileStream(filename, FileMode.Create))
                    {
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine(configJson);
                        sw.Close();
                    }
                }

                using (StreamWriter sw = new StreamWriter(Util.root() + @"\log.txt", true, Encoding.UTF8))
                {
                    sw.WriteLine("config done." + now.ToString() + " " + now.Millisecond);
                }
                this.Dispatcher.Invoke((Action)(() =>
                {
                    mainText.Text = "コンフィグ読み込み完了";
                }));
            }



            // 個別設定ファイル作成
            this.Dispatcher.Invoke((Action)(() =>
            {
                mainText.Text = "setting読み込み開始";
            }));

            for (int i = 0; i < 10; i++)
            {
                string settingFileName = systemDirName + @"\setting" + i + ".json";

                if (!File.Exists(settingFileName))
                {
                    settings[i] = new SettingData(i);
                    SettingJson settingJson = new SettingJson(settings[i]);

                    // 生成も
                    string settingJsonStr = JsonSerializer.Serialize(settingJson, new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                        WriteIndented = true,
                    });
                    using (FileStream fs = new FileStream(settingFileName, FileMode.Create))
                    {
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine(settingJsonStr);
                        sw.Close();
                    }
                }
                else
                {
                    // 読み込み
                    try
                    {
                        using (FileStream fs = new FileStream(settingFileName, FileMode.Open))
                        {
                            StreamReader sr = new StreamReader(fs);
                            string settingJsonData = sr.ReadToEnd();
                            SettingJson settingJson = JsonSerializer.Deserialize<SettingJson>(settingJsonData);
                            settings[i] = new SettingData(settingJson);
                        }
                    }
                    catch
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            mainText.Text = "settingファイル再作成";
                        }));
                        settings[i] = new SettingData(i);
                        SettingJson settingJson = new SettingJson(settings[i]);
                        string settingJsonStr = JsonSerializer.Serialize(settingJson, new JsonSerializerOptions
                        {
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                            WriteIndented = true,
                        });
                        using (FileStream fs = new FileStream(settingFileName, FileMode.Create))
                        {
                            StreamWriter sw = new StreamWriter(fs);
                            sw.WriteLine(settingJsonStr);
                            sw.Close();
                        }
                    }
                }

                using (StreamWriter sw = new StreamWriter(Util.root() + @"\log.txt", true, Encoding.UTF8))
                {
                    sw.WriteLine("setting done." + now.ToString() + " " + now.Millisecond);
                }
            }
            this.Dispatcher.Invoke((Action)(() =>
            {
                mainText.Text = "setting読み込み完了";
            }));


            this.Dispatcher.Invoke((Action)(() =>
            {
                mainText.Text = "ファイルチェック開始";
            }));

            if (!File.Exists(systemDirName + @"\addr.csv"))
            {
                Debug.WriteLine("no addr.csv");
                File.Copy(exeDir + @"\system\addr.csv", systemDirName + @"\addr.csv");
            }


            if (!File.Exists(systemDirName + @"\sabian.csv"))
            {
                Debug.WriteLine("sabian.csv");
                File.Copy(exeDir + @"\system\sabian.csv", systemDirName + @"\sabian.csv");
            }
            if (!File.Exists(systemDirName + @"\microcosm.otf"))
            {
                Debug.WriteLine("microcosm.otf");
                File.Copy(exeDir + @"\system\microcosm.otf", systemDirName + @"\microcosm.otf");
            }
            if (!File.Exists(systemDirName + @"\microcosm-aspects.otf"))
            {
                Debug.WriteLine("microcosm-aspects.otf");
                File.Copy(exeDir + @"\system\microcosm-aspects.otf", systemDirName + @"\microcosm-aspects.otf");
            }

            if (!File.Exists(epheDirName + @"\seas_18.se1"))
            {
                Debug.WriteLine("no seas_18");
                File.Copy(exeDir + @"\ephe\seas_18.se1", epheDirName + @"\seas_18.se1");
            }

            if (!File.Exists(epheDirName + @"\sepl_18.se1"))
            {
                Debug.WriteLine("no sepl_18");
                File.Copy(exeDir + @"\ephe\sepl_18.se1", epheDirName + @"\sepl_18.se1");
            }

            if (!File.Exists(epheDirName + @"\semo_18.se1"))
            {
                Debug.WriteLine("no semo_18");
                File.Copy(exeDir + @"\ephe\semo_18.se1", epheDirName + @"\semo_18.se1");
            }

            if (!File.Exists(epheDirName + @"\seleapsec.txt"))
            {
                Debug.WriteLine("no seleapsec");
                File.Copy(exeDir + @"\ephe\seleapsec.txt", epheDirName + @"\seleapsec.txt");
            }

            if (!File.Exists(epheDirName + @"\sedeltat.txt"))
            {
                Debug.WriteLine("no sedeltat");
                File.Copy(exeDir + @"\ephe\sedeltat.txt", epheDirName + @"\sedeltat.txt");
            }

            if (!File.Exists(epheDirName + @"\swe_deltat.txt"))
            {
                Debug.WriteLine("no swe_deltat");
                File.Copy(exeDir + @"\ephe\swe_deltat.txt", epheDirName + @"\swe_deltat.txt");
            }

            if (!File.Exists(licenseDirName + @"\swiss.txt"))
            {
                Debug.WriteLine("no sweph license");
                File.Copy(exeDir + @"\license\swiss.txt", licenseDirName + @"\swiss.txt");
            }
            if (!File.Exists(licenseDirName + @"\csvhelper.txt"))
            {
                Debug.WriteLine("no csv license");
                File.Copy(exeDir + @"\license\csvhelper.txt", licenseDirName + @"\csvhelper.txt");
            }
            if (!File.Exists(licenseDirName + @"\agpl-3.0.txt"))
            {
                Debug.WriteLine("no agpl license");
                File.Copy(exeDir + @"\license\agpl-3.0.txt", licenseDirName + @"\agpl-3.0.txt");
            }
            if (!File.Exists(licenseDirName + @"\agpl-3.0_ja.txt"))
            {
                Debug.WriteLine("no agpl ja license");
                File.Copy(exeDir + @"\license\agpl-3.0_ja.txt", licenseDirName + @"\agpl-3.0_ja.txt");
            }

            using (StreamWriter sw = new StreamWriter(Util.root() + @"\log.txt", true, Encoding.UTF8))
            {
                sw.WriteLine("file check done." + now.ToString() + " " + now.Millisecond);
            }
            this.Dispatcher.Invoke((Action)(() =>
            {
                mainText.Text = "ファイルチェック完了";
            }));

            sabians = new List<string>();

            using (var reader = new StreamReader(root + @"\system\sabian.csv", Encoding.GetEncoding("UTF-8")))
            using (var csv = new CsvHelper.CsvReader(reader, new CultureInfo("ja-JP", false)))
            {
                var records = csv.GetRecords<Sabian>();

                foreach (Sabian record in records)
                {
                    sabians.Add(record.text);
                }
            }

            this.Dispatcher.Invoke((Action)(() =>
            {
                mainText.Text = "サビアン読み込み完了";
            }));
        }

        public void FirstCalc()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                mainText.Text = "初期計算開始";
            }));
            //UserEventData edata = CommonData.udata2event(targetUser);
            List<UserData> listEventData = new List<UserData>();
            user1data = new UserData(configData);
            user2data = new UserData(configData);
            event1data = new UserData(configData);
            event2data = new UserData(configData);
            listEventData.Add(user1data);
            listEventData.Add(user1data);
            listEventData.Add(event1data);

            // ここから先udata,edataは使わない
            // list1Data～list3Dataを使う
            UserData list1Data = listEventData[0];
            UserData list2Data = listEventData[1];
            UserData list3Data = listEventData[2];


            tempSettings = new TempSetting(configData);
            calc = new AstroCalc(configData, settings[0]);
            //ring1計算
            //splashの時点ではNPT計算

            list1 = calc.PositionCalc(list1Data.GetBirthDateTime(),
                list1Data.lat, list1Data.lng, configData.houseCalc, 0);
            houseList1 = calc.CuspCalc(list1Data.GetBirthDateTime(),
                list1Data.timezone, list1Data.lat, list1Data.lng, configData.houseCalc);
            list1[CommonData.ZODIAC_ASC] = new PlanetData
            {
                no = CommonData.ZODIAC_ASC,
                absolute_position = houseList1[1],
                speed = 0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = true
            };
            list1[CommonData.ZODIAC_MC] = new PlanetData
            {
                no = CommonData.ZODIAC_MC,
                absolute_position = houseList1[10],
                speed = 0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = true
            };
            // progresはlist1とlist3の時刻で固定
            list2 = calc.Progress(list1, list1Data, list3Data.GetBirthDateTime(), list1Data.timezone, list1Data.lat, list1Data.lng);
            houseList2 = calc.CuspCalc(list1Data.GetBirthDateTime(),
                    list1Data.timezone, list3Data.lat, list3Data.lng, configData.houseCalc);
            list2[CommonData.ZODIAC_ASC] = new PlanetData
            {
                no = CommonData.ZODIAC_ASC,
                absolute_position = houseList2[1],
                speed = 0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = true
            };
            list2[CommonData.ZODIAC_MC] = new PlanetData
            {
                no = CommonData.ZODIAC_MC,
                absolute_position = houseList2[10],
                speed = 0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = true
            };

            // natal or transit
            list3 = calc.PositionCalc(list3Data.GetBirthDateTime(),
                list3Data.lat, list3Data.lng, configData.houseCalc, 2);
            houseList3 = calc.CuspCalc(list3Data.GetBirthDateTime(),
                        list3Data.timezone, list1Data.lat, list1Data.lng, configData.houseCalc);
            list3[CommonData.ZODIAC_ASC] = new PlanetData
            {
                no = CommonData.ZODIAC_ASC,
                absolute_position = houseList3[1],
                speed = 0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = true
            };
            list3[CommonData.ZODIAC_MC] = new PlanetData
            {
                no = CommonData.ZODIAC_MC,
                absolute_position = houseList3[10],
                speed = 0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = true
            };


            AspectCalc aspect = new AspectCalc();
            list1 = aspect.AspectCalcSame(settings[0], list1);
            list1 = aspect.AspectCalcOther(settings[0], list1, list2, 3);
            list1 = aspect.AspectCalcOther(settings[0], list1, list3, 4);
            list2 = aspect.AspectCalcSame(settings[0], list2);
            list2 = aspect.AspectCalcOther(settings[0], list2, list3, 5);
            list3 = aspect.AspectCalcSame(settings[0], list3);

            this.Dispatcher.Invoke((Action)(() =>
            {
                mainText.Text = "初期計算終了";
            }));

        }


    }
}
