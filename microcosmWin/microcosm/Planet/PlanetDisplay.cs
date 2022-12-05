using microcosm.Aspect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.Planet
{
    public class PlanetDisplay
    {
        public int planetNo { get; set; }
        public bool isDisp { get; set; }

        public Explanation explanation { get; set; }

        public string planetTxt { get; set; }
        public PointF planetPt { get; set; }
        public System.Windows.Media.Brush planetColor { get; set; }

        public string degreeTxt { get; set; }
        public PointF degreePt { get; set; }

        public string symbolTxt { get; set; }
        public PointF symbolPt { get; set; }
        public System.Windows.Media.Brush symbolColor { get; set; }

        public string minuteTxt { get; set; }
        public PointF minutePt { get; set; }

        public string retrogradeTxt { get; set; }
        public PointF retrogradePt { get; set; }
    }
}
