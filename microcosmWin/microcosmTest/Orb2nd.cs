using microcosm.Aspect;
using microcosm.calc;
using microcosm.common;
using microcosm.config;
using microcosm.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosmTest
{
    public class Orb2nd
    {
        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// 2種はsunでも2ndを使う
        /// </summary>
        [Test]
        public void testOrbSun()
        {
            SettingData setting = new SettingData(0);
            // sunは8,6、2ndは3,1になっている
            setting.dispAspectInconjunct = 1;

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
            //0.5度ハード
            list.Add(CommonData.ZODIAC_MOON, new PlanetData()
            {
                no = CommonData.ZODIAC_MOON,
                absolute_position = 250.5,
                isAspectDisp = true,
                isDisp = true,
                speed = 1.0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = false
            });
            //対象外(sunMoon内)
            list.Add(CommonData.ZODIAC_MERCURY, new PlanetData()
            {
                no = CommonData.ZODIAC_MERCURY,
                absolute_position = 255.0,
                isAspectDisp = true,
                isDisp = true,
                speed = 1.0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = false
            });
            //2.5度ソフト
            list.Add(CommonData.ZODIAC_VENUS, new PlanetData()
            {
                no = CommonData.ZODIAC_VENUS,
                absolute_position = 252.5,
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
            Assert.That(result[CommonData.ZODIAC_SUN].aspects[0].aspectKind, Is.EqualTo(AspectKind.INCONJUNCT)); ;
            Assert.That(result[CommonData.ZODIAC_SUN].aspects[0].softHard, Is.EqualTo(SoftHard.HARD)); ;
            Assert.That(result[CommonData.ZODIAC_SUN].aspects[1].aspectKind, Is.EqualTo(AspectKind.INCONJUNCT)); ;
            Assert.That(result[CommonData.ZODIAC_SUN].aspects[1].softHard, Is.EqualTo(SoftHard.SOFT)); ;
        }

        /// <summary>
        /// 2種はmoonでも2ndを使う
        /// </summary
        [Test]
        public void testOrbMoon()
        {
            SettingData setting = new SettingData(0);
            // sunは8,6、2ndは3,1になっている
            setting.dispAspectInconjunct = 1;

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
            //0.5度ハード
            list.Add(CommonData.ZODIAC_MERCURY, new PlanetData()
            {
                no = CommonData.ZODIAC_MERCURY,
                absolute_position = 250.5,
                isAspectDisp = true,
                isDisp = true,
                speed = 1.0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = false
            });
            //対象外(sunMoon内)
            list.Add(CommonData.ZODIAC_VENUS, new PlanetData()
            {
                no = CommonData.ZODIAC_VENUS,
                absolute_position = 255.0,
                isAspectDisp = true,
                isDisp = true,
                speed = 1.0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = false
            });
            //2.5度ソフト
            list.Add(CommonData.ZODIAC_MARS, new PlanetData()
            {
                no = CommonData.ZODIAC_MARS,
                absolute_position = 252.5,
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
            Assert.That(result[CommonData.ZODIAC_MOON].aspects[0].aspectKind, Is.EqualTo(AspectKind.INCONJUNCT)); ;
            Assert.That(result[CommonData.ZODIAC_MOON].aspects[0].softHard, Is.EqualTo(SoftHard.HARD)); ;
            Assert.That(result[CommonData.ZODIAC_MOON].aspects[1].aspectKind, Is.EqualTo(AspectKind.INCONJUNCT)); ;
            Assert.That(result[CommonData.ZODIAC_MOON].aspects[1].softHard, Is.EqualTo(SoftHard.SOFT)); ;
        }

        /// <summary>
        /// Orb2nd版
        /// </summary>
        [Test]
        public void testOrb2nd()
        {
            SettingData setting = new SettingData(0);
            // 2ndは3,1になっている
            setting.dispAspectInconjunct = 1;

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
            //0.5度ハード
            list.Add(CommonData.ZODIAC_VENUS, new PlanetData()
            {
                no = CommonData.ZODIAC_VENUS,
                absolute_position = 250.5,
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
                absolute_position = 255.0,
                isAspectDisp = true,
                isDisp = true,
                speed = 1.0,
                aspects = new List<AspectInfo>(),
                secondAspects = new List<AspectInfo>(),
                thirdAspects = new List<AspectInfo>(),
                sensitive = false
            });
            //2.5度ソフト
            list.Add(CommonData.ZODIAC_JUPITER, new PlanetData()
            {
                no = CommonData.ZODIAC_JUPITER,
                absolute_position = 252.5,
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
            Assert.That(result[CommonData.ZODIAC_MERCURY].aspects[0].aspectKind, Is.EqualTo(AspectKind.INCONJUNCT)); ;
            Assert.That(result[CommonData.ZODIAC_MERCURY].aspects[0].softHard, Is.EqualTo(SoftHard.HARD)); ;
            Assert.That(result[CommonData.ZODIAC_MERCURY].aspects[1].aspectKind, Is.EqualTo(AspectKind.INCONJUNCT)); ;
            Assert.That(result[CommonData.ZODIAC_MERCURY].aspects[1].softHard, Is.EqualTo(SoftHard.SOFT)); ;
        }
    }
}
