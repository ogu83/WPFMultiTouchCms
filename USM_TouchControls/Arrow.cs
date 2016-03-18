using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace USM_TouchControls
{
    public class Arrow
    {
        /// <summary>
        /// İlkNoktası
        /// </summary>
        public Point FirstPoint { get; set; }
        /// <summary>
        /// İkinci Noktası
        /// </summary>
        public Point SecondPoint { get; set; }
        /// <summary>
        /// Gövde Rengi
        /// </summary>
        public Color GovdeRengi { get; set; }
        /// <summary>
        /// Govde Kalınlığı
        /// </summary>
        public int GovdeKalinligi { get; set; }
        /// <summary>
        /// Gradient Usage or Not
        /// </summary>
        public bool UseGradient { get; set; }

        /// <summary>
        /// Empty Constructor
        /// </summary>        
        public Arrow()
        {
            UseGradient = false;
        }
        /// <summary>
        /// Bir Ok Oluştur 
        /// (gövder rengi sarı, kalınlığı 4 olur otomatik)
        /// </summary>
        /// <param name="p1">İlk Noktası</param>
        /// <param name="p2">İkinci Noktası</param>
        public Arrow(Point p1, Point p2)
        {
            FirstPoint = p1;
            SecondPoint = p2;
            GovdeRengi = Colors.Yellow;
            GovdeKalinligi = 10;
            UseGradient = false;
        }
        /// <summary>
        /// Bir Ok Oluştur
        /// </summary>
        /// <param name="p1">İlk Noktası</param>
        /// <param name="p2">İkinci Noktası</param>
        /// <param name="c">Renk</param>
        /// <param name="k">Gövde Kalınlığı</param>
        public Arrow(Point p1, Point p2, Color c, int k)
        {
            FirstPoint = p1;
            SecondPoint = p2;
            GovdeRengi = c;
            GovdeKalinligi = k;
            UseGradient = false;
        }
        /// <summary>
        /// Bir Ok Oluştur
        /// </summary>
        /// <param name="p1">İlk Noktası</param>
        /// <param name="p2">İkinci Noktası</param>
        /// <param name="c">Renk</param>
        /// <param name="k">Gövde Kalınlığı</param>
        /// <param name="ug">Gradient olacakmı?</param>
        public Arrow(Point p1, Point p2, Color c, int k, bool ug)
        {
            FirstPoint = p1;
            SecondPoint = p2;
            GovdeRengi = c;
            GovdeKalinligi = k;
            UseGradient = ug;
        }

        /// <summary>
        /// Gövde Elemanı Shapes.Line Olarak
        /// </summary>
        public Line Govde
        {
            get
            {
                Line MyLine= new Line();
                MyLine.X1 = FirstPoint.X; MyLine.Y1 = FirstPoint.Y;
                MyLine.X2 = SecondPoint.X; MyLine.Y2 = SecondPoint.Y;

                MyLine.StrokeEndLineCap = PenLineCap.Triangle;
                MyLine.StrokeThickness = GovdeKalinligi;
                if (!UseGradient)
                {
                    MyLine.Stroke = new SolidColorBrush(GovdeRengi);
                }
                else
                {
                    MyLine.Stroke = new LinearGradientBrush(Colors.Black, GovdeRengi, 0);
                }

                return MyLine;
            }
        }


    }
}
