using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace microcosm.config
{
    /// <summary>
    /// jsonファイルをデシリアライズした情報
    /// </summary>
    public class SettingJson
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("progression")]
        public EProgression progression { get; set; }

        [JsonPropertyName("houseCalc")]
        public EHouseCalc houseCalc { get; set; }

        [JsonPropertyName("dispPlanetSun")]
        public int dispPlanetSun { get; set; }
        [JsonPropertyName("dispPlanetMoon")]
        public int dispPlanetMoon { get; set; }
        [JsonPropertyName("dispPlanetMercury")]
        public int dispPlanetMercury { get; set; }
        [JsonPropertyName("dispPlanetVenus")]
        public int dispPlanetVenus { get; set; }
        [JsonPropertyName("dispPlanetMars")]
        public int dispPlanetMars { get; set; }
        [JsonPropertyName("dispPlanetJupiter")]
        public int dispPlanetJupiter { get; set; }
        [JsonPropertyName("dispPlanetSaturn")]
        public int dispPlanetSaturn { get; set; }
        [JsonPropertyName("dispPlanetUranus")]
        public int dispPlanetUranus { get; set; }
        [JsonPropertyName("dispPlanetNeptune")]
        public int dispPlanetNeptune { get; set; }
        [JsonPropertyName("dispPlanetPluto")]
        public int dispPlanetPluto { get; set; }
        [JsonPropertyName("dispPlanetAsc")]
        public int dispPlanetAsc { get; set; }
        [JsonPropertyName("dispPlanetMc")]
        public int dispPlanetMc { get; set; }
        [JsonPropertyName("dispPlanetChiron")]
        public int dispPlanetChiron { get; set; }
        [JsonPropertyName("dispPlanetDH")]
        public int dispPlanetDH { get; set; }
        [JsonPropertyName("dispPlanetDT")]
        public int dispPlanetDT { get; set; }
        [JsonPropertyName("dispPlanetLilith")]
        public int dispPlanetLilith { get; set; }
        [JsonPropertyName("dispPlanetEarth")]
        public int dispPlanetEarth { get; set; }
        [JsonPropertyName("dispPlanetCeres")]
        public int dispPlanetCeres { get; set; }
        [JsonPropertyName("dispPlanetPallas")]
        public int dispPlanetPallas { get; set; }
        [JsonPropertyName("dispPlanetJuno")]
        public int dispPlanetJuno { get; set; }
        [JsonPropertyName("dispPlanetVesta")]
        public int dispPlanetVesta { get; set; }

        [JsonPropertyName("dispAspectPlanetSun")]
        public int dispAspectPlanetSun { get; set; }
        [JsonPropertyName("dispAspectPlanetMoon")]
        public int dispAspectPlanetMoon { get; set; }
        [JsonPropertyName("dispAspectPlanetMercury")]
        public int dispAspectPlanetMercury { get; set; }
        [JsonPropertyName("dispAspectPlanetVenus")]
        public int dispAspectPlanetVenus { get; set; }
        [JsonPropertyName("dispAspectPlanetMars")]
        public int dispAspectPlanetMars { get; set; }
        [JsonPropertyName("dispAspectPlanetJupiter")]
        public int dispAspectPlanetJupiter { get; set; }
        [JsonPropertyName("dispAspectPlanetSaturn")]
        public int dispAspectPlanetSaturn { get; set; }
        [JsonPropertyName("dispAspectPlanetUranus")]
        public int dispAspectPlanetUranus { get; set; }
        [JsonPropertyName("dispAspectPlanetNeptune")]
        public int dispAspectPlanetNeptune { get; set; }
        [JsonPropertyName("dispAspectPlanetPluto")]
        public int dispAspectPlanetPluto { get; set; }
        [JsonPropertyName("dispAspectPlanetAsc")]
        public int dispAspectPlanetAsc { get; set; }
        [JsonPropertyName("dispAspectPlanetMc")]
        public int dispAspectPlanetMc { get; set; }
        [JsonPropertyName("dispAspectPlanetChiron")]
        public int dispAspectPlanetChiron { get; set; }
        [JsonPropertyName("dispAspectPlanetDH")]
        public int dispAspectPlanetDH { get; set; }
        [JsonPropertyName("dispAspectPlanetDT")]
        public int dispAspectPlanetDT { get; set; }
        [JsonPropertyName("dispAspectPlanetLilith")]
        public int dispAspectPlanetLilith { get; set; }
        [JsonPropertyName("dispAspectPlanetEarth")]
        public int dispAspectPlanetEarth { get; set; }
        [JsonPropertyName("dispAspectPlanetCeres")]
        public int dispAspectPlanetCeres { get; set; }
        [JsonPropertyName("dispAspectPlanetPallas")]
        public int dispAspectPlanetPallas { get; set; }
        [JsonPropertyName("dispAspectPlanetJuno")]
        public int dispAspectPlanetJuno { get; set; }
        [JsonPropertyName("dispAspectPlanetVesta")]
        public int dispAspectPlanetVesta { get; set; }

        [JsonPropertyName("orbSunMoon")]
        public double[] orbSunMoon { get; set; }
        [JsonPropertyName("orb1st")]
        public double[] orb1st { get; set; }
        [JsonPropertyName("orb2nd")]
        public double[] orb2nd { get; set; }

        [JsonPropertyName("dispConjunction")]
        public int dispAspectConjunction { get; set; }

        [JsonPropertyName("dispOpposition")]
        public int dispAspectOpposition { get; set; }

        [JsonPropertyName("dispTrine")]
        public int dispAspectTrine { get; set; }

        [JsonPropertyName("dispSquare")]
        public int dispAspectSquare { get; set; }

        [JsonPropertyName("dispSextile")]
        public int dispAspectSextile { get; set; }

        [JsonPropertyName("dispInconjunct")]
        public int dispAspectInconjunct { get; set; }

        [JsonPropertyName("dispSesquiQuadrate")]
        public int dispAspectSesquiQuadrate { get; set; }

        [JsonPropertyName("dispSemiSquare")]
        public int dispAspectSemiSquare { get; set; }

        [JsonPropertyName("dispSemiSextile")]
        public int dispAspectSemiSextile { get; set; }

        [JsonPropertyName("dispQuintile")]
        public int dispAspectQuintile { get; set; }

        [JsonPropertyName("dispSeptile")]
        public int dispAspectSeptile { get; set; }

        [JsonPropertyName("dispNovile")]
        public int dispAspectNovile { get; set; }

        [JsonPropertyName("dispAspectBiQuintile")]
        public int dispAspectBiQuintile { get; set; }

        [JsonPropertyName("dispAspectSemiQuintile")]
        public int dispAspectSemiQuintile { get; set; }

        [JsonPropertyName("dispAspectQuindecile")]
        public int dispAspectQuindecile { get; set; }

        [JsonPropertyName("sameCusps")]
        public bool sameCusps { get; set; }


        public SettingJson(int i)
        {
            this.name = String.Format("設定{0}", i);

            this.orbSunMoon = new double[2];
            this.orb1st = new double[2];
            this.orb2nd = new double[2];
            orbSunMoon[0] = 8.0;
            orbSunMoon[1] = 6.0;
            orb1st[0] = 6.0;
            orb1st[1] = 4.0;
            orb2nd[0] = 3.0;
            orb2nd[1] = 1.0;

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

            dispAspectConjunction = 1;
            dispAspectOpposition = 1;
            dispAspectTrine = 1;
            dispAspectSquare = 1;
            dispAspectSextile = 1;
            dispAspectInconjunct = 0;
            dispAspectSesquiQuadrate = 0;
            dispAspectSemiSquare = 0;
            dispAspectSemiSextile = 0;
            dispAspectQuintile = 0;
            dispAspectSeptile = 0;
            dispAspectNovile = 0;
            dispAspectBiQuintile = 0;
            dispAspectSemiQuintile = 0;
            dispAspectQuindecile = 0;

            progression = EProgression.SECONDARY;
            houseCalc = EHouseCalc.PLACIDUS;
            sameCusps = false;
        }

        public SettingJson(SettingData data)
        {
            this.name = data.dispName;
            this.orbSunMoon = new double[2];
            this.orb1st = new double[2];
            this.orb2nd = new double[2];
            if (data.orbSunMoon == null || data.orbSunMoon.Length < 2)
            {
                Debug.WriteLine("SettingData不正");
                throw new InvalidProgramException("SettingData不正");
            }
            orbSunMoon[0] = data.orbSunMoon[0];
            orbSunMoon[1] = data.orbSunMoon[1];
            orb1st[0] = data.orb1st[0];
            orb1st[1] = data.orb1st[1];
            orb2nd[0] = data.orb2nd[0];
            orb2nd[1] = data.orb2nd[1];

            dispAspectConjunction = data.dispAspectConjunction;
            dispAspectOpposition = data.dispAspectOpposition;
            dispAspectTrine = data.dispAspectTrine;
            dispAspectSquare = data.dispAspectSquare;
            dispAspectSextile = data.dispAspectSextile;
            dispAspectInconjunct = data.dispAspectInconjunct;
            dispAspectSesquiQuadrate = data.dispAspectSesquiQuadrate;
            dispAspectSemiSquare = data.dispAspectSemiSquare;
            dispAspectSemiSextile = data.dispAspectSemiSextile;
            dispAspectQuintile = data.dispAspectQuintile;
            dispAspectSeptile = data.dispAspectSeptile;
            dispAspectNovile = data.dispAspectNovile;
            dispAspectBiQuintile = data.dispAspectBiQuintile;
            dispAspectSemiQuintile = data.dispAspectSemiQuintile;
            dispAspectQuindecile = data.dispAspectQuindecile;

            dispPlanetSun = data.dispPlanetSun;
            dispPlanetMoon = data.dispPlanetMoon;
            dispPlanetMercury = data.dispPlanetMercury;
            dispPlanetVenus = data.dispPlanetVenus;
            dispPlanetMars = data.dispPlanetMars;
            dispPlanetJupiter = data.dispPlanetJupiter;
            dispPlanetSaturn = data.dispPlanetSaturn;
            dispPlanetUranus = data.dispPlanetUranus;
            dispPlanetNeptune = data.dispPlanetNeptune;
            dispPlanetPluto = data.dispPlanetPluto;
            dispPlanetAsc = data.dispPlanetAsc;
            dispPlanetMc = data.dispPlanetMc;
            dispPlanetChiron = data.dispPlanetChiron;
            dispPlanetDH = data.dispPlanetDH;
            dispPlanetDT = data.dispPlanetDT;
            dispPlanetLilith = data.dispPlanetLilith;
            dispPlanetEarth = data.dispPlanetEarth;
            dispPlanetCeres = data.dispPlanetCeres;
            dispPlanetPallas = data.dispPlanetPallas;
            dispPlanetJuno = data.dispPlanetJuno;
            dispPlanetVesta = data.dispPlanetVesta;

            dispAspectPlanetSun = data.dispAspectPlanetSun;
            dispAspectPlanetMoon = data.dispAspectPlanetMoon;
            dispAspectPlanetMercury = data.dispAspectPlanetMercury;
            dispAspectPlanetVenus = data.dispAspectPlanetVenus;
            dispAspectPlanetMars = data.dispAspectPlanetMars;
            dispAspectPlanetJupiter = data.dispAspectPlanetJupiter;
            dispAspectPlanetSaturn = data.dispAspectPlanetSaturn;
            dispAspectPlanetUranus = data.dispAspectPlanetUranus;
            dispAspectPlanetNeptune = data.dispAspectPlanetNeptune;
            dispAspectPlanetPluto = data.dispAspectPlanetPluto;
            dispAspectPlanetAsc = data.dispAspectPlanetAsc;
            dispAspectPlanetMc = data.dispAspectPlanetMc;
            dispAspectPlanetChiron = data.dispAspectPlanetChiron;
            dispAspectPlanetDH = data.dispAspectPlanetDH;
            dispAspectPlanetDT = data.dispAspectPlanetDT;
            dispAspectPlanetLilith = data.dispAspectPlanetLilith;
            dispAspectPlanetEarth = data.dispAspectPlanetEarth;
            dispAspectPlanetCeres = data.dispAspectPlanetCeres;
            dispAspectPlanetPallas = data.dispAspectPlanetPallas;
            dispAspectPlanetJuno = data.dispAspectPlanetJuno;
            dispAspectPlanetVesta = data.dispAspectPlanetVesta;

            progression = data.progression;
            houseCalc = data.houseCalc;
            sameCusps = data.sameCusps; 
        }

        public SettingJson()
        {
            name = "noName";
            progression = EProgression.SECONDARY;
            this.orbSunMoon = new double[2];
            this.orb1st = new double[2];
            this.orb2nd = new double[2];
        }
    }
}
