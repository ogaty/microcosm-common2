using System;
using System.Collections.Generic;

namespace microcosmMac2.Models
{
    public class Calculation
    {
        public Dictionary<int, PlanetData> planetData;
        public double[] cusps;
        public Calculation(Dictionary<int, PlanetData> p, double[] c)
        {
            planetData = p;
            cusps = c;
        }
    }
}

