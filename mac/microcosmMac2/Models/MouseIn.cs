using System;
using SkiaSharp;

namespace microcosmMac2.Models
{
    public class MouseIn
    {
        public int kind; // 0:planet 1:aspect
        public SKRect rect;
        public string message;
        public string sabian;
        public double degree;
        public MouseIn()
        {
        }
    }
}

