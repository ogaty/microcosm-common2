using microcosm.common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.config
{
    public class SettingData
    {
        public string dispName { get; set; }

        public int dispPlanetSun;
        public int dispPlanetMoon;
        public int dispPlanetMercury;
        public int dispPlanetVenus;
        public int dispPlanetMars;
        public int dispPlanetJupiter;
        public int dispPlanetSaturn;
        public int dispPlanetUranus;
        public int dispPlanetNeptune;
        public int dispPlanetPluto;
        public int dispPlanetAsc;
        public int dispPlanetMc;
        public int dispPlanetChiron;
        public int dispPlanetDH;
        public int dispPlanetDT;
        public int dispPlanetLilith;
        public int dispPlanetEarth;
        public int dispPlanetCeres;
        public int dispPlanetPallas;
        public int dispPlanetJuno;
        public int dispPlanetVesta;

        public int dispAspectPlanetSun;
        public int dispAspectPlanetMoon;
        public int dispAspectPlanetMercury;
        public int dispAspectPlanetVenus;
        public int dispAspectPlanetMars;
        public int dispAspectPlanetJupiter;
        public int dispAspectPlanetSaturn;
        public int dispAspectPlanetUranus;
        public int dispAspectPlanetNeptune;
        public int dispAspectPlanetPluto;
        public int dispAspectPlanetAsc;
        public int dispAspectPlanetMc;
        public int dispAspectPlanetChiron;
        public int dispAspectPlanetDH;
        public int dispAspectPlanetDT;
        public int dispAspectPlanetLilith;
        public int dispAspectPlanetEarth;
        public int dispAspectPlanetCeres;
        public int dispAspectPlanetPallas;
        public int dispAspectPlanetJuno;
        public int dispAspectPlanetVesta;

        public int dispAspectConjunction;
        public int dispAspectOpposition;
        public int dispAspectTrine;
        public int dispAspectSquare;
        public int dispAspectSextile;
        public int dispAspectInconjunct;
        public int dispAspectSesquiQuadrate;
        public int dispAspectSemiSquare;
        public int dispAspectSemiSextile;
        public int dispAspectQuintile;
        public int dispAspectSeptile;
        public int dispAspectNovile;
        public int dispAspectBiQuintile;
        public int dispAspectSemiQuintile;
        public int dispAspectQuindecile;

        // sorf/hardの順
        public double[] orbSunMoon;
        public double[] orb1st;
        public double[] orb2nd;


        // no: 設定番号
        public SettingData(int no)
        {
            init(no);
        }

        public SettingData(SettingJson json)
        {
            init(json);
        }

        public void init(int no)
        {
            this.dispName = "設定" + no.ToString();
            this.orbSunMoon = new double[2];
            this.orb1st = new double[2];
            this.orb2nd = new double[2];
            this.orbSunMoon[0] = 8.0;
            this.orbSunMoon[1] = 6.0;
            this.orb1st[0] = 6.0;
            this.orb1st[1] = 4.0;
            this.orb2nd[0] = 3.0;
            this.orb2nd[1] = 1.0;
            this.dispAspectConjunction = 1;
            this.dispAspectOpposition = 1;
            this.dispAspectTrine = 1;
            this.dispAspectSquare = 1;
            this.dispAspectSextile = 1;
            this.dispAspectInconjunct = 0;
            this.dispAspectSesquiQuadrate = 0;
            this.dispAspectSemiSquare = 0;
            this.dispAspectSemiSextile = 0;
            this.dispAspectQuintile = 0;
            this.dispAspectSeptile = 0;
            this.dispAspectNovile = 0;
            this.dispAspectBiQuintile = 0;
            this.dispAspectSemiQuintile = 0;
            this.dispAspectQuindecile = 0;
            dispPlanetSun = 1;
            dispPlanetMoon = 1;
            dispPlanetMercury = 1;
            dispPlanetVenus = 1;
            dispPlanetMars = 1;
            dispPlanetJupiter = 1;
            dispPlanetSaturn = 1;
            dispPlanetUranus = 1;
            dispPlanetNeptune = 1;
            dispPlanetPluto = 1;
            dispPlanetAsc = 1;
            dispPlanetMc = 1;
            dispPlanetDH = 1;
            dispPlanetDT = 1;
            dispPlanetChiron = 1;
            dispPlanetLilith = 1;
            dispPlanetEarth = 0;
            dispPlanetCeres = 0;
            dispPlanetPallas = 0;
            dispPlanetJuno = 0;
            dispPlanetVesta = 0;

            dispAspectPlanetSun = 1;
            dispAspectPlanetMoon = 1;
            dispAspectPlanetMercury = 1;
            dispAspectPlanetVenus = 1;
            dispAspectPlanetMars = 1;
            dispAspectPlanetJupiter = 1;
            dispAspectPlanetSaturn = 1;
            dispAspectPlanetUranus = 1;
            dispAspectPlanetNeptune = 1;
            dispAspectPlanetPluto = 1;
            dispAspectPlanetAsc = 1;
            dispAspectPlanetMc = 1;
            dispAspectPlanetDH = 1;
            dispAspectPlanetDT = 1;
            dispAspectPlanetChiron = 1;
            dispAspectPlanetLilith = 1;
            dispAspectPlanetEarth = 0;
            dispAspectPlanetCeres = 0;
            dispAspectPlanetPallas = 0;
            dispAspectPlanetJuno = 0;
            dispAspectPlanetVesta = 0;
        }

        public void init(SettingJson json)
        {
            this.dispName = json.name;
            this.orbSunMoon = new double[2];
            this.orb1st = new double[2];
            this.orb2nd = new double[2];
            if (json.orbSunMoon == null || json.orbSunMoon.Length < 2)
            {
                Debug.WriteLine("Jsonデータ不正");
                throw new InvalidProgramException("Jsonデータ不正");
            }
            this.orbSunMoon[0] = json.orbSunMoon[0];
            this.orbSunMoon[1] = json.orbSunMoon[1];
            this.orb1st[0] = json.orb1st[0];
            this.orb1st[1] = json.orb1st[1];
            this.orb2nd[0] = json.orb2nd[0];
            this.orb2nd[1] = json.orb2nd[1];
            this.dispAspectConjunction = json.dispAspectConjunction;
            this.dispAspectOpposition = json.dispAspectOpposition;
            this.dispAspectTrine = json.dispAspectTrine;
            this.dispAspectSquare = json.dispAspectSquare;
            this.dispAspectSextile = json.dispAspectSextile;
            this.dispAspectInconjunct = json.dispAspectInconjunct;
            this.dispAspectSesquiQuadrate = json.dispAspectSesquiQuadrate;
            this.dispAspectSemiSquare = json.dispAspectSemiSquare;
            this.dispAspectSemiSextile = json.dispAspectSemiSextile;
            this.dispAspectQuintile = json.dispAspectQuintile;
            this.dispAspectSeptile = json.dispAspectSeptile;
            this.dispAspectNovile = json.dispAspectNovile;
            this.dispAspectBiQuintile = json.dispAspectBiQuintile;
            this.dispAspectSemiQuintile = json.dispAspectSemiQuintile;
            this.dispAspectQuindecile = json.dispAspectQuindecile;
            dispPlanetSun = json.dispPlanetSun;
            dispPlanetMoon = json.dispPlanetMoon;
            dispPlanetMercury = json.dispPlanetMercury;
            dispPlanetVenus = json.dispPlanetVenus;
            dispPlanetMars = json.dispPlanetMars;
            dispPlanetJupiter = json.dispPlanetJupiter;
            dispPlanetSaturn = json.dispPlanetSaturn;
            dispPlanetUranus = json.dispPlanetUranus;
            dispPlanetNeptune = json.dispPlanetNeptune;
            dispPlanetPluto = json.dispPlanetPluto;
            dispPlanetAsc = json.dispPlanetAsc;
            dispPlanetMc = json.dispPlanetMc;
            dispPlanetChiron = json.dispPlanetChiron;
            dispPlanetDH = json.dispPlanetDH;
            dispPlanetDT = json.dispPlanetDT;
            dispPlanetLilith = json.dispPlanetLilith;
            dispPlanetEarth = json.dispPlanetEarth;
            dispPlanetCeres = json.dispPlanetCeres;
            dispPlanetPallas = json.dispPlanetPallas;
            dispPlanetJuno = json.dispPlanetJuno;
            dispPlanetVesta = json.dispPlanetVesta;
            dispAspectPlanetSun = json.dispAspectPlanetSun;
            dispAspectPlanetMoon = json.dispAspectPlanetMoon;
            dispAspectPlanetMercury = json.dispAspectPlanetMercury;
            dispAspectPlanetVenus = json.dispAspectPlanetVenus;
            dispAspectPlanetMars = json.dispAspectPlanetMars;
            dispAspectPlanetJupiter = json.dispAspectPlanetJupiter;
            dispAspectPlanetSaturn = json.dispAspectPlanetSaturn;
            dispAspectPlanetUranus = json.dispAspectPlanetUranus;
            dispAspectPlanetNeptune = json.dispAspectPlanetNeptune;
            dispAspectPlanetPluto = json.dispAspectPlanetPluto;
            dispAspectPlanetAsc = json.dispAspectPlanetAsc;
            dispAspectPlanetMc = json.dispAspectPlanetMc;
            dispAspectPlanetChiron = json.dispAspectPlanetChiron;
            dispAspectPlanetDH = json.dispAspectPlanetDH;
            dispAspectPlanetDT = json.dispAspectPlanetDT;
            dispAspectPlanetLilith = json.dispAspectPlanetLilith;
            dispAspectPlanetEarth = json.dispAspectPlanetEarth;
            dispAspectPlanetCeres = json.dispAspectPlanetCeres;
            dispAspectPlanetPallas = json.dispAspectPlanetPallas;
            dispAspectPlanetJuno = json.dispAspectPlanetJuno;
            dispAspectPlanetVesta = json.dispAspectPlanetVesta;
        }

        public bool GetDispPlanet(int planetNo)
        {
            switch (planetNo)
            {
                case CommonData.ZODIAC_SUN:
                    return dispPlanetSun == 1;
                case CommonData.ZODIAC_MOON:
                    return dispPlanetMoon == 1;
                case CommonData.ZODIAC_MERCURY:
                    return dispPlanetMercury == 1;
                case CommonData.ZODIAC_VENUS:
                    return dispPlanetVenus == 1;
                case CommonData.ZODIAC_MARS:
                    return dispPlanetMars == 1;
                case CommonData.ZODIAC_JUPITER:
                    return dispPlanetJupiter == 1;
                case CommonData.ZODIAC_SATURN:
                    return dispPlanetSaturn == 1;
                case CommonData.ZODIAC_URANUS:
                    return dispPlanetUranus == 1;
                case CommonData.ZODIAC_NEPTUNE:
                    return dispPlanetNeptune == 1;
                case CommonData.ZODIAC_PLUTO:
                    return dispPlanetPluto == 1;
                case CommonData.ZODIAC_DH_TRUENODE:
                    return dispPlanetDH == 1;
                case CommonData .ZODIAC_DH_MEANNODE:
                    return dispPlanetDH == 1;
                case CommonData.ZODIAC_DT_TRUE:
                    return dispPlanetDT == 1;
                case CommonData.ZODIAC_DT_MEAN:
                    return dispPlanetDT == 1;
                case CommonData.ZODIAC_OSCU_LILITH:
                    return dispPlanetLilith == 1;
                case CommonData.ZODIAC_MEAN_LILITH:
                    return dispPlanetLilith == 1;
                case CommonData.ZODIAC_CHIRON:
                    return dispPlanetChiron == 1;
                case CommonData.ZODIAC_EARTH:
                    return dispPlanetEarth == 1;
                case CommonData.ZODIAC_ASC:
                    return dispPlanetAsc == 1;
                case CommonData.ZODIAC_MC:
                    return dispPlanetMc == 1;
                case CommonData.ZODIAC_CERES:
                    return dispPlanetCeres == 1;
                case CommonData.ZODIAC_PALLAS:
                    return dispPlanetPallas == 1;
                case CommonData.ZODIAC_JUNO:
                    return dispPlanetJuno == 1;
                case CommonData.ZODIAC_VESTA:
                    return dispPlanetVesta == 1;

            }

            return false;
        }

        public bool GetDispAspectPlanet(int planetNo)
        {
            switch (planetNo)
            {
                case CommonData.ZODIAC_SUN:
                    return dispAspectPlanetSun == 1;
                case CommonData.ZODIAC_MOON:
                    return dispAspectPlanetMoon == 1;
                case CommonData.ZODIAC_MERCURY:
                    return dispAspectPlanetMercury == 1;
                case CommonData.ZODIAC_VENUS:
                    return dispAspectPlanetVenus == 1;
                case CommonData.ZODIAC_MARS:
                    return dispAspectPlanetMars == 1;
                case CommonData.ZODIAC_JUPITER:
                    return dispAspectPlanetJupiter == 1;
                case CommonData.ZODIAC_SATURN:
                    return dispAspectPlanetSaturn == 1;
                case CommonData.ZODIAC_URANUS:
                    return dispAspectPlanetUranus == 1;
                case CommonData.ZODIAC_NEPTUNE:
                    return dispAspectPlanetNeptune == 1;
                case CommonData.ZODIAC_PLUTO:
                    return dispAspectPlanetPluto == 1;
                case CommonData.ZODIAC_DH_TRUENODE:
                    return dispAspectPlanetDH == 1;
                case CommonData.ZODIAC_DH_MEANNODE:
                    return dispAspectPlanetDH == 1;
                case CommonData.ZODIAC_DT_TRUE:
                    return dispAspectPlanetDT == 1;
                case CommonData.ZODIAC_DT_MEAN:
                    return dispAspectPlanetDT == 1;
                case CommonData.ZODIAC_OSCU_LILITH:
                    return dispAspectPlanetLilith == 1;
                case CommonData.ZODIAC_MEAN_LILITH:
                    return dispAspectPlanetLilith == 1;
                case CommonData.ZODIAC_CHIRON:
                    return dispAspectPlanetChiron == 1;
                case CommonData.ZODIAC_EARTH:
                    return dispAspectPlanetEarth == 1;
                case CommonData.ZODIAC_ASC:
                    return dispAspectPlanetAsc == 1;
                case CommonData.ZODIAC_MC:
                    return dispAspectPlanetMc == 1;
                case CommonData.ZODIAC_CERES:
                    return dispAspectPlanetCeres == 1;
                case CommonData.ZODIAC_PALLAS:
                    return dispAspectPlanetPallas == 1;
                case CommonData.ZODIAC_JUNO:
                    return dispAspectPlanetJuno == 1;
                case CommonData.ZODIAC_VESTA:
                    return dispAspectPlanetVesta == 1;

            }

            return false;
        }

    }
}
