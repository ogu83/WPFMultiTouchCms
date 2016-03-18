using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using USM_TouchControls.ViewModels;

namespace USM_TouchControls
{
    /// <summary>
    /// Interaction logic for TouchVideoPlayer.xaml
    /// </summary>
    public partial class TouchVideoPlayer : UserControl, ITouchElement
    {
        #region ITouchElement Members
        public Guid ThumbID { get; set; }
        public bool IsMaximized { get; set; }
        public double oHeight { get; set; }
        public double oWidth { get; set; }
        #endregion

        private DispatcherTimer hideTimer = new DispatcherTimer();
        private const int hideTime = 10;
        private int nTime = 0;
        private bool _isControlsHidden;

        private TouchVideoPlayerViewModel _myViewModel { get { return this.DataContext as TouchVideoPlayerViewModel; } }

        public double Volume
        {
            get { return _myViewModel.Volume; }
            set { _myViewModel.Volume = value; }
        }

        private void OpenControlPanel()
        {
            if (!_isControlsHidden) return;

            nTime = 0;
            (TryFindResource("SB_OpenPnlControl") as Storyboard).Begin(this);
            _isControlsHidden = false;
        }

        public TouchVideoPlayer(string videoFile, bool autoStart, Guid thumbID)
        {
            InitializeComponent();

            hideTimer.Interval = new TimeSpan(0, 0, 1);
            hideTimer.Tick += new EventHandler(hideTimer_Tick);
            hideTimer.Start();

            this.DataContext = new TouchVideoPlayerViewModel(videoFile);
            _myViewModel.AutoStart = autoStart;
            this.ThumbID = thumbID;

            if (_myViewModel.AutoStart)
            {
                myMediaElement.Play();
                _myViewModel.IsPlaying = true;
            }
        }

        public TouchVideoPlayer()
            : this("C:\\Users\\Public\\Videos\\Sample Videos\\Wildlife.wmv", false, new Guid())
        {

        }

        private void hideTimer_Tick(object sender, EventArgs e)
        {
            nTime++;
            this._myViewModel.Position = myMediaElement.Position;

            if (nTime == hideTime)
            {
                _isControlsHidden = true;
                (TryFindResource("SB_ClosePnlControl") as Storyboard).Begin(this);
            }
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenControlPanel();
        }

        private void UserControl_TouchDown(object sender, TouchEventArgs e)
        {
            OpenControlPanel();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            this.myMediaElement.Play();
            this._myViewModel.IsPlaying = true;
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            this.myMediaElement.Pause();
            this._myViewModel.IsPlaying = false;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            this.myMediaElement.Stop();
            this._myViewModel.IsPlaying = false;
        }

        private void Play_TouchDown(object sender, TouchEventArgs e)
        {
            this.myMediaElement.Play();
            this._myViewModel.IsPlaying = true;

            VisualStateManager.GoToState(sender as FrameworkElement, "Pressed", true);
        }

        private void Pause_TouchDown(object sender, TouchEventArgs e)
        {
            this.myMediaElement.Pause();
            this._myViewModel.IsPlaying = false;

            VisualStateManager.GoToState(sender as FrameworkElement, "Pressed", true);
        }

        private void Stop_TouchDown(object sender, TouchEventArgs e)
        {
            this.myMediaElement.Stop();
            this._myViewModel.IsPlaying = false;

            VisualStateManager.GoToState(sender as FrameworkElement, "Pressed", true);
        }

        private void Slider_TouchDown(object sender, TouchEventArgs e)
        {
            //e.Handled = true;
        }

        private void Slider_TouchMove(object sender, TouchEventArgs e)
        {
            //e.Handled = true;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.myMediaElement.Stop();
            this._myViewModel.IsPlaying = false;
        }
    }
}
