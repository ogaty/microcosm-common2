using System;
namespace microcosmMac2.Views.DataSources
{
    public class FileName
    {
        public string name { get; set; }
        public string nameFullPath { get; set; }

        public FileName()
        {
        }

        public FileName(string name)
        {
            this.name = name;
        }
    }
}

