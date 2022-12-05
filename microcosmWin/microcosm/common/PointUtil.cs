using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.common
{
    public class PointUtil
    {
        /// <summary>
        /// Canvasの下へ移動
        /// </summary>
        /// <param name="p"></param>
        /// <param name="px"></param>
        /// <returns></returns>
        public static PointF pointMoveUnder(PointF p, int px)
        {
            p.Y -= px;
            return p;
        }

        /// <summary>
        /// Canvasの上へ移動
        /// </summary>
        /// <param name="p"></param>
        /// <param name="px"></param>
        /// <returns></returns>
        public static PointF pointMoveUpper(PointF p, int px)
        {
            p.Y += px;
            return p;
        }

        /// <summary>
        /// Canvasの左へ移動
        /// </summary>
        /// <param name="p"></param>
        /// <param name="px"></param>
        /// <returns></returns>
        public static PointF pointMoveLeft(PointF p, int px)
        {
            p.X -= px;
            return p;
        }

        /// <summary>
        /// Canvasの右へ移動
        /// </summary>
        /// <param name="p"></param>
        /// <param name="px"></param>
        /// <returns></returns>
        public static PointF pointMoveRight(PointF p, int px)
        {
            p.X += px;
            return p;
        }
    }
}
