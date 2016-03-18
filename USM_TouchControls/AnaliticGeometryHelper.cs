using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace USM_TouchControls
{
    public class AnaliticGeometryHelper
    {
        public static Point CenterPoint(Point p1, Point p2)
        {            
            return new Point((p2.X - p1.X) / 2 + p1.X,(p2.Y-p1.Y)/2+p1.Y);
        }
    }
}