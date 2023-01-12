using microcosm.calc;
using microcosm.common;
using microcosm.config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosmTest
{
    public class SolarIngress
    {
        public ConfigData config;
        public SettingData setting;
        public AstroCalc calc;
        public EclipseCalc e;

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void testSolarIngress()
        {
            config = new ConfigData();
            setting = new SettingData(0);

            calc = new AstroCalc(config, setting);
            e = calc.GetEclipseInstance();
            DateTime newDay = e.GetEclipse(new DateTime(2023, 1, 10, 12, 0, 0), 9.0, CommonData.ZODIAC_SUN , 300, true);

            Assert.That(newDay.Year, Is.EqualTo(2023));
            Assert.That(newDay.Month, Is.EqualTo(1));
            Assert.That(newDay.Day, Is.EqualTo(20));
            calc.GetSwiss().swe_close();
        }

        [Test]
        public void testSolarIngressOver360()
        {
            DateTime newDay = e.GetEclipse(new DateTime(2023, 2, 25, 12, 0, 0), 9.0, CommonData.ZODIAC_SUN, 0, true);

            Assert.That(newDay.Year, Is.EqualTo(2023));
            Assert.That(newDay.Month, Is.EqualTo(3));
            Assert.That(newDay.Day, Is.EqualTo(21));
            calc.GetSwiss().swe_close();
        }
    }
}
