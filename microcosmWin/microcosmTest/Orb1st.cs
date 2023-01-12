using microcosm.common;
using microcosm.config;
using microcosm.calc;
using microcosm.Planet;
using microcosm.Aspect;
using NUnit.Framework;

namespace microcosmTest
{
    /// <summary>
    /// 一種オーブだけやる、二種は別クラスで
    /// </summary>
    public class Orb1st
    {
        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// Orb1stじゃなくてOrbSunMoonを使っているか
        /// </summary>
        [Test]
        public void testOrbSun()
        {
            SettingData setting = new SettingData(0);
            setting.orb1st[0] = 0;
            setting.orb1st[1] = 0;
            // sunは8,6になっている

            Dictionary<int, PlanetData> list = new Dictionary<int, PlanetData>();
            list.Add(CommonData.ZODIAC_SUN, new PlanetData()
            {
                no = CommonData.ZODIAC_SUN,
                absolute_position = 100.0,
                isAspectDisp = true,
                isDisp = true,
                speed = 1.0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = false
            });
            //5度ハード
            list.Add(CommonData.ZODIAC_MOON, new PlanetData()
            {
                no = CommonData.ZODIAC_MOON,
                absolute_position = 285.0,
                isAspectDisp = true,
                isDisp = true,
                speed = 1.0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = false
            });
            //対象外
            list.Add(CommonData.ZODIAC_MERCURY, new PlanetData()
            {
                no = CommonData.ZODIAC_MERCURY,
                absolute_position = 289.0,
                isAspectDisp = true,
                isDisp = true,
                speed = 1.0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = false
            });
            //7度ソフト
            list.Add(CommonData.ZODIAC_VENUS, new PlanetData()
            {
                no = CommonData.ZODIAC_VENUS,
                absolute_position = 287.0,
                isAspectDisp = true,
                isDisp = true,
                speed = 1.0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = false
            });
            AspectCalc aspect = new AspectCalc();
            Dictionary<int, PlanetData> result = aspect.AspectCalcSame(setting, list);
            Assert.That(result[CommonData.ZODIAC_SUN].aspects.Count, Is.EqualTo(2));
            Assert.That(result[CommonData.ZODIAC_SUN].aspects[0].aspectKind, Is.EqualTo(AspectKind.OPPOSITION)); ;
            Assert.That(result[CommonData.ZODIAC_SUN].aspects[0].softHard, Is.EqualTo(SoftHard.HARD)); ;
            Assert.That(result[CommonData.ZODIAC_SUN].aspects[1].aspectKind, Is.EqualTo(AspectKind.OPPOSITION)); ;
            Assert.That(result[CommonData.ZODIAC_SUN].aspects[1].softHard, Is.EqualTo(SoftHard.SOFT)); ;
        }

        /// <summary>
        /// Orb1stじゃなくてOrbSunMoonを使っているかMoon版
        /// </summary>
        [Test]
        public void testOrbMoon()
        {
            SettingData setting = new SettingData(0);
            setting.orb1st[0] = 0;
            setting.orb1st[1] = 0;
            // sunは8,6になっている

            Dictionary<int, PlanetData> list = new Dictionary<int, PlanetData>();
            list.Add(CommonData.ZODIAC_MOON, new PlanetData()
            {
                no = CommonData.ZODIAC_MOON,
                absolute_position = 100.0,
                isAspectDisp = true,
                isDisp = true,
                speed = 1.0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = false
            });
            //5度ハード
            list.Add(CommonData.ZODIAC_MERCURY, new PlanetData()
            {
                no = CommonData.ZODIAC_MERCURY,
                absolute_position = 285.0,
                isAspectDisp = true,
                isDisp = true,
                speed = 1.0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = false
            });
            //対象外
            list.Add(CommonData.ZODIAC_VENUS, new PlanetData()
            {
                no = CommonData.ZODIAC_VENUS,
                absolute_position = 289.0,
                isAspectDisp = true,
                isDisp = true,
                speed = 1.0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = false
            });
            //7度ソフト
            list.Add(CommonData.ZODIAC_MARS, new PlanetData()
            {
                no = CommonData.ZODIAC_MARS,
                absolute_position = 287.0,
                isAspectDisp = true,
                isDisp = true,
                speed = 1.0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = false
            });
            AspectCalc aspect = new AspectCalc();
            Dictionary<int, PlanetData> result = aspect.AspectCalcSame(setting, list);
            Assert.That(result[CommonData.ZODIAC_MOON].aspects.Count, Is.EqualTo(2));
            Assert.That(result[CommonData.ZODIAC_MOON].aspects[0].aspectKind, Is.EqualTo(AspectKind.OPPOSITION)); ;
            Assert.That(result[CommonData.ZODIAC_MOON].aspects[0].softHard, Is.EqualTo(SoftHard.HARD)); ;
            Assert.That(result[CommonData.ZODIAC_MOON].aspects[1].aspectKind, Is.EqualTo(AspectKind.OPPOSITION)); ;
            Assert.That(result[CommonData.ZODIAC_MOON].aspects[1].softHard, Is.EqualTo(SoftHard.SOFT)); ;
        }

        /// <summary>
        /// Orb1st版
        /// </summary>
        [Test]
        public void testOrb1st()
        {
            SettingData setting = new SettingData(0);
            setting.orb1st[0] = 6;
            setting.orb1st[1] = 4;

            Dictionary<int, PlanetData> list = new Dictionary<int, PlanetData>();
            list.Add(CommonData.ZODIAC_MERCURY, new PlanetData()
            {
                no = CommonData.ZODIAC_MERCURY,
                absolute_position = 100.0,
                isAspectDisp = true,
                isDisp = true,
                speed = 1.0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = false
            });
            //3度ハード
            list.Add(CommonData.ZODIAC_VENUS, new PlanetData()
            {
                no = CommonData.ZODIAC_VENUS,
                absolute_position = 283.0,
                isAspectDisp = true,
                isDisp = true,
                speed = 1.0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = false
            });
            //対象外
            list.Add(CommonData.ZODIAC_MARS, new PlanetData()
            {
                no = CommonData.ZODIAC_MARS,
                absolute_position = 287.0,
                isAspectDisp = true,
                isDisp = true,
                speed = 1.0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = false
            });
            //5度ソフト
            list.Add(CommonData.ZODIAC_JUPITER, new PlanetData()
            {
                no = CommonData.ZODIAC_JUPITER,
                absolute_position = 285.0,
                isAspectDisp = true,
                isDisp = true,
                speed = 1.0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = false
            });
            AspectCalc aspect = new AspectCalc();
            Dictionary<int, PlanetData> result = aspect.AspectCalcSame(setting, list);
            Assert.That(result[CommonData.ZODIAC_MERCURY].aspects.Count, Is.EqualTo(2));
            Assert.That(result[CommonData.ZODIAC_MERCURY].aspects[0].aspectKind, Is.EqualTo(AspectKind.OPPOSITION)); ;
            Assert.That(result[CommonData.ZODIAC_MERCURY].aspects[0].softHard, Is.EqualTo(SoftHard.HARD)); ;
            Assert.That(result[CommonData.ZODIAC_MERCURY].aspects[1].aspectKind, Is.EqualTo(AspectKind.OPPOSITION)); ;
            Assert.That(result[CommonData.ZODIAC_MERCURY].aspects[1].softHard, Is.EqualTo(SoftHard.SOFT)); ;
        }
    }
}