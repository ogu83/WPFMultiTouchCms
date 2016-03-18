using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace USM_TouchControls
{
    public class Daire
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
        public Daire()
        {
            UseGradient = false;
        }
        /// <summary>
        /// Bir Ok Oluştur 
        /// (gövder rengi sarı, kalınlığı 4 olur otomatik)
        /// </summary>
        /// <param name="p1">İlk Noktası</param>
        /// <param name="p2">İkinci Noktası</param>
        public Daire(Point p1, Point p2)
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
        public Daire(Point p1, Point p2, Color c, int k)
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
        public Daire(Point p1, Point p2, Color c, int k, bool ug)
        {
            FirstPoint = p1;
            SecondPoint = p2;
            GovdeRengi = c;
            GovdeKalinligi = k;
            UseGradient = ug;
        }

        /// <summary>
        /// Gövde Elemanı Shapes.Ellipse Olarak
        /// </summary>
        public Ellipse Govde
        {
            get
            {
                Ellipse myEllipse = new Ellipse();
                myEllipse.Width = Math.Abs(FirstPoint.X - SecondPoint.X);
                myEllipse.Height = Math.Abs(FirstPoint.Y - SecondPoint.Y);

                if (FirstPoint.X <= SecondPoint.X && FirstPoint.Y <= SecondPoint.Y)         //İlk Noktadan Sağ Alt köşeye doğru çizilim
                {
                    myEllipse.Margin = new Thickness(FirstPoint.X, FirstPoint.Y, 0, 0);
                }
                else if (FirstPoint.X > SecondPoint.X && FirstPoint.Y > SecondPoint.Y)      //İlk Noktadan Sol Üst köşeye doğru çizilim
                {
                    myEllipse.Margin = new Thickness(SecondPoint.X, SecondPoint.Y, 0, 0);
                }
                else if (FirstPoint.X > SecondPoint.X && FirstPoint.Y < SecondPoint.Y)      //İlk Noktadan Sağ Üst köşeye doğru çizilim
                {
                    myEllipse.Margin = new Thickness(SecondPoint.X, FirstPoint.Y, 0, 0);
                }
                else if (FirstPoint.X < SecondPoint.X && FirstPoint.Y > SecondPoint.Y)      //İlk Noktadan Sol Alt köşeye doğru çizilim
                {
                    myEllipse.Margin = new Thickness(FirstPoint.X, SecondPoint.Y, 0, 0);
                }

                myEllipse.StrokeThickness = GovdeKalinligi;
                if (!UseGradient)
                {
                    myEllipse.Stroke = new SolidColorBrush(GovdeRengi);
                }
                else
                {
                    myEllipse.Stroke = new LinearGradientBrush(Colors.Black, GovdeRengi, 0);
                }

                return myEllipse;
            }
        }
    }
}
