using System;
namespace microcosmMac2.Views.Entity
{
    public class SettingName
    {
        public string Title { get; set; } = "";

        public SettingName()
        {
        }

        public SettingName(string Title)
        {
            this.Title = Title;
        }
    }
}

