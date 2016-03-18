using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using USM_Presenter.ViewModels;
using System.IO;

namespace USM_Presenter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
#else
            if (!CheckForProductKey()) App.Current.Shutdown();
#endif
            if (e.Args == null)
                (new MainWindow()).Show();
            else if (e.Args.Length == 0)
                (new MainWindow()).Show();
            else
            {
                string fileName = e.Args[0];
                if (File.Exists(fileName))
                    new MainWindow(PresentationViewModel.DeserializeFromXML(fileName)).Show();
                else
                    (new MainWindow()).Show();
            }

            base.OnStartup(e);
        }

        private static bool CheckForProductKey()
        {
            string file = Environment.SystemDirectory + "\\" + "UsmPresenterKey" + ".ini";
            if (!File.Exists(file))
            {
                MessageBox.Show("Ürün anahtarı eksik", "Ürün Anahtarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            string s = "Man bite the dog.";
            string s1 = File.ReadAllText(file);
            if (s != s1)
            {
                MessageBox.Show("Ürün anahtarı eksik", "Ürün Anahtarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
