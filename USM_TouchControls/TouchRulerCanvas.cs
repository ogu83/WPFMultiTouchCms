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
    public class TouchRulerCanvas : Canvas
    {
        /// <summary>
        /// Üzerinde en yukarda olan elemanın değişme olayı
        /// </summary>
        /// <param name="sender">Last elemanı değişen canvasın ta kendisi</param>
        public delegate void lastChangedHandler(object sender);
        public event lastChangedHandler lastChanged;
        protected virtual void OnlastChanged()
        {
            if (lastChanged != null)
                lastChanged(this);
        }

        public delegate void ThrownOutHandler(object sender, UIElement element);
        public event ThrownOutHandler thrownOut;
        protected virtual void OnthrownOut(UIElement element)
        {
            if (this.thrownOut != null)
                this.thrownOut(this, element);

            if (IsRemoveOnThrowOut)
                this.Children.Remove(element);
        }

        /// <summary>
        /// Dışarı fırlatıldığında eleman atma
        /// </summary>
        public bool IsRemoveOnThrowOut { get; set; }

        public bool IsRotateDisabled
        {
            get { return (bool)GetValue(IsRotateDisabledProperty); }
            set { SetValue(IsRotateDisabledProperty, value); }
        }
        public static readonly DependencyProperty IsRotateDisabledProperty =
            DependencyProperty.Register("IsRotateDisabled", typeof(bool), typeof(TouchRulerCanvas));

        public bool IsTranslateDisabled
        {
            get { return (bool)GetValue(IsTranslateDisabledProperty); }
            set { SetValue(IsTranslateDisabledProperty, value); }
        }
        public static readonly DependencyProperty IsTranslateDisabledProperty =
            DependencyProperty.Register("IsTranslateDisabled", typeof(bool), typeof(TouchRulerCanvas));

        public bool IsScaleDisabled
        {
            get { return (bool)GetValue(IsScaleDisabledProperty); }
            set { SetValue(IsScaleDisabledProperty, value); }
        }
        public static readonly DependencyProperty IsScaleDisabledProperty =
            DependencyProperty.Register("IsScaleDisabled", typeof(bool), typeof(TouchRulerCanvas));

        /// <summary>
        /// Bir Child'ın en az küçülebildiği katsayı
        /// </summary>
        public double MinScaleFactor { get; set; }
        /// <summary>
        /// Bir Child'ın en falza büyüyebildiği katsayı
        /// </summary>
        public double MaxScaleFactor { get; set; }

        /// <summary>
        /// Kareli Kağıt Açık/Kapalı
        /// </summary>
        public bool IsRulerEnabled { get; set; }
        /// <summary>
        /// Arka plan resmi aktif
        /// gereksiz iptal edildi bu fonksiyon
        /// </summary>
        public bool IsBackPicture { get; set; }

        /// <summary>
        /// çizilecek kareli kağıdın X aralığı
        /// </summary>
        public int RulerX { get; set; }
        /// <summary>
        /// çizilecek kareli kağıdın Y aralığı
        /// </summary>
        public int RulerY { get; set; }
        /// <summary>
        /// çizilecek kareli kağıdın kareçizgilerinin rengi
        /// </summary>
        public SolidColorBrush RulerColor { get; set; }
        /// <summary>
        /// Kareli kağıdın çizgilerinin kalınlığı
        /// </summary>
        public int RulerThickness { get; set; }

        private FrameworkElement _last;
        /// <summary>
        /// En Yukarıda duran eleman (Image veya MediaElement vb.)
        /// </summary>
        public FrameworkElement last
        {
            get { return _last; }
            set
            {
                if (value == this) return;
                if (_last != value)
                {
                    if (_last != null) Canvas.SetZIndex(_last, 0);
                    Canvas.SetZIndex(value, 2);
                    _last = value;
                    OnlastChanged();
                }
            }
        }

        private bool _isTouched = false;
        private DateTime _firstClickTime = DateTime.Now;
        private bool _isDoubleClick
        {
            get
            {
                DateTime d = DateTime.Now;
                if (d.Subtract(_firstClickTime).TotalMilliseconds < 200)
                {
                    _firstClickTime = DateTime.Now;
                    return _isTouched;
                }

                _firstClickTime = DateTime.Now;
                return false;
            }
        }

        public TouchRulerCanvas()
        {
            this.TouchDown += new EventHandler<TouchEventArgs>(TouchRulerCanvas_TouchDown);
            this.TouchUp += new EventHandler<TouchEventArgs>(TouchRulerCanvas_TouchUp);

            this.ManipulationStarting += new EventHandler<ManipulationStartingEventArgs>(image_ManipulationStarting);
            this.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(image_ManipulationDelta);
            //inertia 
            this.ManipulationInertiaStarting += new EventHandler<ManipulationInertiaStartingEventArgs>(canvas_ManipulationInertiaStarting);
            //Mouse events handling when no touch device activated
            this.MouseLeftButtonDown += new MouseButtonEventHandler(TouchRulerCanvas_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(TouchRulerCanvas_MouseLeftButtonUp);
            this.MouseMove += new MouseEventHandler(TouchRulerCanvas_MouseMove);
            this.MouseWheel += new MouseWheelEventHandler(TouchRulerCanvas_MouseWheel);
        }

        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            base.OnRender(dc);

            if (IsRulerEnabled)
            {
                Pen pen = new Pen(RulerColor, RulerThickness);
                for (int x = RulerX; x < this.ActualWidth; x += RulerX)
                    dc.DrawLine(pen, new Point(x, 0), new Point(x, this.ActualHeight));

                for (int y = RulerY; y < this.ActualHeight; y += RulerY)
                    dc.DrawLine(pen, new Point(0, y), new Point(this.ActualWidth, y));
            }
        }

        #region Mouse Events

        private void TouchRulerCanvas_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
#if DEBUG
            _isTouched = true;
            FirstMouseCoor = e.GetPosition(this);
            var uie = e.Source as FrameworkElement;
            if (uie == this) return;
            if (uie != null)
            {
                //if (last != null) Canvas.SetZIndex(last, 0);
                //Canvas.SetZIndex(uie, 2);
                last = uie;

                DoubleClickBehavior();
            }
#endif
        }

        private void TouchRulerCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
#if DEBUG
            _isTouched = false;
#endif
        }

        private Point FirstMouseCoor { get; set; }

        private void TouchRulerCanvas_MouseMove(object sender, MouseEventArgs e)
        {
#if DEBUG
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (IsTranslateDisabled) return; //Eğer bu canvas için kaydırma kapalı ise
                var element = last;
                var matrix = ((MatrixTransform)element.RenderTransform).Matrix;
                if (element as TouchImage != null)
                    if ((element as TouchImage).IsTranslateDisabled) return; // eğer bu eleman için kaydırma kapalı ise
                matrix.Translate(e.GetPosition(this).X - FirstMouseCoor.X, e.GetPosition(this).Y - FirstMouseCoor.Y);
                FirstMouseCoor = e.GetPosition(this);
                try
                {
                    ((MatrixTransform)element.RenderTransform).Matrix = matrix;
                }
                catch { }

                Rect containingRect = new Rect((this).RenderSize);
                Rect shapeBounds = element.RenderTransform.TransformBounds(new Rect(element.RenderSize));
                if (!containingRect.Contains(shapeBounds) && !containingRect.IntersectsWith(shapeBounds))
                    OnthrownOut(element);
            }
#endif
        }
        private double _zoom = 1.0;
        private double Zoom
        {
            get { return _zoom; }
            set { _zoom = value; }
        }

        private void TouchRulerCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
#if DEBUG
            if (IsScaleDisabled) return; //Scale olayı bu canvas için kapalı ise
            var element = last as FrameworkElement;
            if (element as TouchImage != null)
                if ((element as TouchImage).IsScaleDisable) return; // scale olayı bu canvas için kapalı ise

            var matrix = ((MatrixTransform)element.RenderTransform).Matrix;
            Point center = new Point(element.ActualWidth / 2, element.ActualHeight / 2);
            center = matrix.Transform(center);
            Zoom += Zoom * (e.Delta / 1200.0f);
            matrix.ScaleAt((double)(Zoom), (double)(Zoom), center.X, center.Y);
            Zoom = 1;

            FirstMouseCoor = e.GetPosition(this);
            try
            {
                ((MatrixTransform)element.RenderTransform).Matrix = matrix;
            }
            catch { }
#endif
        }
        #endregion

        #region Touch Events

        private void canvas_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            // Decrease the velocity of the Rectangle's movement by 
            // 10 inches per second every second.
            // (10 inches * 96 DIPS per inch / 1000ms^2)
            e.TranslationBehavior = new InertiaTranslationBehavior()
            {
                InitialVelocity = e.InitialVelocities.LinearVelocity,
                DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0)
            };

            // Decrease the velocity of the Rectangle's resizing by 
            // 0.1 inches per second every second.
            // (0.1 inches * 96 DIPS per inch / (1000ms^2)
            e.ExpansionBehavior = new InertiaExpansionBehavior()
            {
                InitialVelocity = e.InitialVelocities.ExpansionVelocity / 4,
                DesiredDeceleration = 0.01 * 96 / 1000.0 * 1000.0
            };

            // Decrease the velocity of the Rectangle's rotation rate by 
            // 2 rotations per second every second.
            // (2 * 360 degrees / (1000ms^2)
            e.RotationBehavior = new InertiaRotationBehavior()
            {
                InitialVelocity = e.InitialVelocities.AngularVelocity,
                DesiredDeceleration = 72 / (1000.0 * 1000.0)
            };
            e.Handled = true;
        }

        private void image_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            var uie = e.Source as FrameworkElement;
            if (uie == this) return;

            if (uie != null)
                last = uie;

            //canvas is the parent of the image starting the manipulation;
            //Container does not have to be parent, but that is the most common scenario
            e.ManipulationContainer = this;
            e.Handled = true;
            // you could set the mode here too 
            // e.Mode = ManipulationModes.All;              
        }

        private void image_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            //this just gets the source. 
            // I cast it to FE because I wanted to use ActualWidth for Center. You could try RenderSize as alternate
            var element = e.Source as FrameworkElement;
            if (element != null)
            {
                //e.DeltaManipulation has the changes 
                // Scale is a delta multiplier; 1.0 is last size,  (so 1.1 == scale 10%, 0.8 = shrink 20%) 
                // Rotate = Rotation, in degrees
                // Pan = Translation, == Translate offset, in Device Independent Pixels 
                var deltaManipulation = e.DeltaManipulation;
                var matrix = ((MatrixTransform)element.RenderTransform).Matrix;
                Point center = new Point(e.ManipulationOrigin.X, e.ManipulationOrigin.Y);

                // this will be a Zoom. 
                if (!IsScaleDisabled) // scale is enabled
                {
                    if (element as TouchImage != null)
                        if ((element as TouchImage).IsScaleDisable) return; // scale olayı bu canvas için kapalı ise
                    matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y);
                }
                // Rotation 
                if (!IsRotateDisabled) // rotation is enabled
                {
                    if (element as TouchImage != null)
                        if ((element as TouchImage).IsRotateDisabled) return; // scale olayı bu canvas için kapalı ise
                    matrix.RotateAt(e.DeltaManipulation.Rotation, center.X, center.Y);
                }
                // Translation (pan) 
                if (!IsTranslateDisabled) // translation is enabled
                {
                    if (element as TouchImage != null)
                        if ((element as TouchImage).IsTranslateDisabled) return; // scale olayı bu canvas için kapalı ise
                    matrix.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);
                }

                //Determinant = 1 ise ozaman büyüklük geldiği büyüklüktür.
                //Determinant bir child ' ın küçültme ve büyütme faktörüdür.
                if ((matrix.Determinant >= MinScaleFactor) && (matrix.Determinant <= MaxScaleFactor || MaxScaleFactor == 0))
                    ((MatrixTransform)element.RenderTransform).Matrix = matrix;

                var videoPlayer = element as TouchVideoPlayer;
                if (videoPlayer != null)
                    videoPlayer.Volume = Math.Max(0, Math.Min(matrix.Determinant, 1)); //bind the volume to the render size of the video in the screen

                e.Handled = true;

                if (e.IsInertial)
                {
                    Rect containingRect = new Rect(((FrameworkElement)e.ManipulationContainer).RenderSize);
                    Rect shapeBounds = element.RenderTransform.TransformBounds(new Rect(element.RenderSize));
                    // Check if the element is completely in the window.
                    // If it is not and intertia is occuring, stop the manipulation.
                    if (e.IsInertial && !containingRect.Contains(shapeBounds) && !containingRect.IntersectsWith(shapeBounds))
                    {
                        //Report that we have gone over our boundary 
                        e.ReportBoundaryFeedback(e.DeltaManipulation);
                        OnthrownOut(element);
                        // comment out this line to see the Window 'shake' or 'bounce' 
                        // similar to Win32 Windows when they reach a boundary; this comes for free in .NET 4                
                        e.Complete();
                    }
                }
            }
        }

        private void TouchRulerCanvas_TouchDown(object sender, TouchEventArgs e)
        {
            _isTouched = true;
        }

        private void TouchRulerCanvas_TouchUp(object sender, TouchEventArgs e)
        {
            DoubleClickBehavior();
        }

        #endregion

        private void DoubleClickBehavior()
        {
            if (_isDoubleClick)
            {
                last.RenderTransform = new MatrixTransform(1, 0, 0, 1, 0, 0);
                last.Height = this.ActualHeight;

                if ((last as TouchVideoPlayer) != null)
                {
                    last.Width = this.ActualWidth;
                    (last as TouchVideoPlayer).Volume = 1;
                }
                else
                {
                    double pWidth = last.Height * last.ActualWidth / last.ActualHeight;
                    last.RenderTransform = new MatrixTransform(1, 0, 0, 1, (this.ActualWidth - pWidth) / 2, 0);
                }
            }

            _isTouched = false;
        }

        public void Add_A_Picture(string MyFileName, int pWitdh)
        {
            TouchImage MyImage = new TouchImage();
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(MyFileName, UriKind.RelativeOrAbsolute);
            src.DecodePixelWidth = pWitdh;
            src.EndInit();

            int MyImageNo = this.Children.Count;

            MyImage.Source = src;
            MyImage.Width = pWitdh;
            MyImage.MinWidth = pWitdh;
            MyImage.IsManipulationEnabled = true;
            MyImage.Name = "Image" + MyImageNo.ToString();
            MyImage.Stretch = Stretch.UniformToFill;
            MyImage.RenderTransform = new MatrixTransform(1, 0, 0, 1, 200 + 10 * MyImageNo, 200 + 10 * MyImageNo);
            RenderOptions.SetBitmapScalingMode(MyImage, BitmapScalingMode.HighQuality);

            this.Children.Add(MyImage);
        }

        public void Add_A_Picture(string MyFileName)
        {
            Add_A_Picture(MyFileName, new Guid());
        }

        public void Add_A_Picture(string MyFileName, Guid thumbID)
        {
            TouchImage MyImage = new TouchImage(thumbID);
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(MyFileName, UriKind.RelativeOrAbsolute);
            src.EndInit();
            if (src.PixelHeight > 3000) src.DecodePixelHeight = 3000;

            int MyImageNo = this.Children.Count;

            MyImage.Source = src;
            MyImage.Height = 300;
            MyImage.IsManipulationEnabled = true;
            MyImage.Name = "Image" + MyImageNo.ToString();
            MyImage.Stretch = Stretch.UniformToFill;
            MyImage.RenderTransform = new MatrixTransform(1, 0, 0, 1, 200 + 10 * MyImageNo, 200 + 10 * MyImageNo);
            RenderOptions.SetBitmapScalingMode(MyImage, BitmapScalingMode.HighQuality);

            this.Children.Add(MyImage);
        }

        private TouchImage FindPicture(Guid thumbID)
        {
            foreach (UIElement elem in this.Children)
            {
                TouchImage image = elem as TouchImage;

                if (image != null)
                    if (image.ThumbID == thumbID)
                        return image;
            }

            return null;
        }

        private void RemovePicture(TouchImage image)
        {
            this.Children.Remove(image);
        }

        public void RemovePicture(Guid thumID)
        {
            TouchImage image = FindPicture(thumID);
            RemovePicture(image);
        }

        public void Add_A_Video(string MyFileName, bool autoStart, Guid thumbID)
        {
            TouchVideoPlayer myPlayer = new TouchVideoPlayer(MyFileName, autoStart, thumbID);

            int MyVideoNo = this.Children.Count;
            myPlayer.IsManipulationEnabled = true;
            myPlayer.RenderTransform = new MatrixTransform(1, 0, 0, 1, 200 + 10 * MyVideoNo, 200 + 10 * MyVideoNo);

            this.Children.Add(myPlayer);
        }

        private TouchVideoPlayer FindVideoPlayer(Guid thumbID)
        {
            foreach (UIElement elem in this.Children)
            {
                TouchVideoPlayer videoplayer = elem as TouchVideoPlayer;

                if (videoplayer != null)
                    if (videoplayer.ThumbID == thumbID)
                        return videoplayer;
            }

            return null;
        }

        private void RemoveVideoPlayer(TouchVideoPlayer player)
        {
            this.Children.Remove(player);
        }

        public void RemoveVideoPlayer(Guid thumID)
        {
            TouchVideoPlayer player = FindVideoPlayer(thumID);
            RemoveVideoPlayer(player);
        }
    }
}
