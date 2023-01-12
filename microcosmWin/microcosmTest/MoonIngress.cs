using microcosm.calc;
using microcosm.common;
using microcosm.config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosmTest
{
    public class MoonIngress
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
        public void testMoonIngress()
        {
            config = new ConfigData();
            setting = new SettingData(0);

            calc = new AstroCalc(config, setting);
            e = calc.GetEclipseInstance();
            DateTime newDay = e.GetEclipse(new DateTime(2023, 2, 15, 12, 0, 0), 9.0, CommonData.ZODIAC_MOON, 300, true);

            Assert.That(newDay.Year, Is.EqualTo(2023));
            Assert.That(newDay.Month, Is.EqualTo(2));
            Assert.That(newDay.Day, Is.EqualTo(18));
            Assert.That(newDay.Hour, Is.EqualTo(14));
            calc.GetSwiss().swe_close();
        }

        [Test]
        public void testMoonIngressOver360()
        {
            config = new ConfigData();
            setting = new SettingData(0);

            calc = new AstroCalc(config, setting);
            e = calc.GetEclipseInstance();
            DateTime newDay = e.GetEclipse(new DateTime(2023, 2, 21, 12, 0, 0), 9.0, CommonData.ZODIAC_MOON, 0, true);

            Assert.That(newDay.Year, Is.EqualTo(2023));
            Assert.That(newDay.Month, Is.EqualTo(2));
            Assert.That(newDay.Day, Is.EqualTo(22));
            Assert.That(newDay.Hour, Is.EqualTo(14));
            calc.GetSwiss().swe_close();
        }
    }
}
