using System;
using System.Text.Json.Serialization;

namespace microcosmMac2.Config
{
    public class TestClass
    {
        [JsonPropertyName("a")]
        public int a { get; set; }
        [JsonPropertyName("b")]
        public string b { get; set; }

        public TestClass()
        {
        }
    }
}

