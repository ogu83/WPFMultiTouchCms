using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace USM_TouchControls
{
    public class TouchImage : Image, ITouchElement
    {
        #region TouchElementInterface Members
        public Guid ThumbID { get; set; }
        public bool IsMaximized { get; set; }
        public double oHeight { get; set; }
        public double oWidth { get; set; }
        #endregion

        /// <summary>
        /// 1. Touch
        /// </summary>
        public TouchPoint IlkDokunus { get; set; }
        /// <summary>
        /// 2. Touch
        /// </summary>
        public TouchPoint IkinciDokunus { get; set; }
        /// <summary>
        /// 1. ve 2. Touchların Analitik Ortalaması
        /// </summary>
        public Point TouchCenter { get; set; }
        /// <summary>
        /// Translate Edilemez mi?
        /// </summary>
        public bool IsTranslateDisabled { get; set; }
        /// <summary>
        /// Rotate Edilemez mi?
        /// </summary>
        public bool IsRotateDisabled { get; set; }
        /// <summary>
        /// Scale Edilemez mi?
        /// </summary>
        public bool IsScaleDisable { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public TouchImage()
        {
            //Daron Yöndemden aldık
            //Touch.FrameReported += new TouchFrameEventHandler(Touch_FrameReported);
        }

        public TouchImage(Guid thumbID)
            : this()
        {
            ThumbID = thumbID;
        }

        /// <summary>
        /// Bir touch algılanırsa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            //Point _FirstTouch = new Point(0, 0);
            //Point _SecondTouch = new Point(0, 0);
            if ((e.GetPrimaryTouchPoint(this) != null))
            {
                IlkDokunus = e.GetPrimaryTouchPoint(this);
                if (IlkDokunus.Action == TouchAction.Down)
                {
                    //_FirstTouch = new Point(0, 0);
                    //_SecondTouch = new Point(0, 0);
                }
                else if (IlkDokunus.Action == TouchAction.Move)
                {
                    if (e.GetTouchPoints(this).Count > 1)
                    {
                        IkinciDokunus = e.GetTouchPoints(this)[1];
                    }
                }
                if (IkinciDokunus != null)
                {

                    var matrix = ((MatrixTransform)this.RenderTransform).Matrix;
                    TouchCenter = matrix.Transform(
                        AnaliticGeometryHelper.CenterPoint(
                            IlkDokunus.Position,
                            IkinciDokunus.Position
                        )
                    );
                }
            }
        }


    }
}

