using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace USM_TouchControls.ViewModels
{
    public class TouchVideoPlayerViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        #endregion

        public TouchVideoPlayerViewModel()
        {

        }

        public TouchVideoPlayerViewModel(string videoFile)
        {
            Video = videoFile;
        }

        private TimeSpan _position = new TimeSpan();
        public TimeSpan Position
        {
            get { return _position; }
            set
            {
                if (value != _position)
                {
                    _position = value;
                    NotifyPropertyChanged("Position");
                }
            }
        }

        private string _video;
        public string Video
        {
            get { return _video; }
            set
            {
                if (_video != value)
                {
                    _video = value;
                    NotifyPropertyChanged("Video");
                }
            }
        }

        private bool _autoStart;
        public bool AutoStart
        {
            get { return _autoStart; }
            set
            {
                if (value != _autoStart)
                {
                    _autoStart = value;
                    NotifyPropertyChanged("AutoStart");
                }
            }
        }

        private double _volume = 1;
        public double Volume
        {
            get { return _volume; }
            set
            {
                if (_volume != value)
                {
                    _volume = value;
                    NotifyPropertyChanged("Volume");
                }
            }
        }

        private bool _isPlaying;
        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                if (value != _isPlaying)
                {
                    _isPlaying = value;
                    NotifyPropertyChanged("IsPlaying");
                    NotifyPropertyChanged("IsNotPlaying");
                }
            }
        }

        public bool IsNotPlaying
        {
            get { return !IsPlaying; }
        }
    }
}
