using System;
using microcosmMac2.Config;
using microcosmMac2.User;

namespace microcosmMac2.Common
{
    /// <summary>
    /// シングルトンインスタンスを用意したくて作った
    /// けどAppDelegate.csがその役目を果たしてくれてた
    /// </summary>
    public class CommonInstance
    {
        private static CommonInstance instance = new CommonInstance();
        //public ViewController controller;
        //public UserAddViewController userAdd;
        //public UserEditViewController userEdit;
        public ConfigData config;
        public SettingData[] settings;
        public int[] customRings = { 1, 3, 3, 1, 1, 1, 1 };

        public SettingData currentSetting;
        public int currentSettingIndex;

        public string SelectedDirectoryName;
        public string SelectedDirectoryFullPath;
        public string SelectedFileName = "";
        public UserData SelectedUserData;

        public string searchPlace = "";

        private CommonInstance()
        {
        }

        public static CommonInstance getInstance()
        {
            return instance;
        }
    }
}

