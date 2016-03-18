using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Windows;

namespace USM_Presenter.Helpers
{
    public class ExtendedToggleButton : ToggleButton, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        #endregion      

        public bool IsNotChecked { get { return !this.IsChecked.Value; } }

        protected override void OnChecked(System.Windows.RoutedEventArgs e)
        {
            base.OnChecked(e);
            NotifyPropertyChanged("IsNotChecked");
        }

        protected override void OnUnchecked(System.Windows.RoutedEventArgs e)
        {
            base.OnUnchecked(e);
            NotifyPropertyChanged("IsNotChecked");
        }        
    }
}
