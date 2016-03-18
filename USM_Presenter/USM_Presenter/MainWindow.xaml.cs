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
using USM_Presenter.ViewModels;
using Microsoft.Win32;
using USM_Presenter.Helpers;
using System.Windows.Media.Animation;
using System.IO;
using USM_TouchControls;

namespace USM_Presenter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PresentationViewModel _myViewModel { get { return this.DataContext as PresentationViewModel; } }

        private enum UserStateEnum { None, AtPage, AtHome }
        private UserStateEnum _userState = UserStateEnum.AtHome;

        private bool _onRunningStoryBoard = false;

        public MainWindow()
        {
            this.DataContext = new PresentationViewModel();
            InitializeComponent();

            this._myViewModel.SlideShow.SlideChanged += new EventHandler(SlideShow_SlideChanged);
        }

        public MainWindow(PresentationViewModel presentation)
        {
            this.DataContext = presentation;
            InitializeComponent();

            this._myViewModel.SlideShow.SlideChanged += new EventHandler(SlideShow_SlideChanged);
        }

        private void btnNav_Click(object sender, RoutedEventArgs e)
        {
            Storyboard SB;
            switch ((sender as Button).Name)
            {
                case "btnHome":
                    if (_onRunningStoryBoard) return;
                    SB = new Storyboard();
                    if (_userState != UserStateEnum.AtHome)
                        SB = (TryFindResource("SB_ButtonsOpen") as Storyboard).Clone();
                    else
                        SB = (TryFindResource("SB_ButtonsClose") as Storyboard).Clone();

                    SB.Completed += (object sender1, EventArgs e1) =>
                    {
                        if (_userState == UserStateEnum.AtPage) (TryFindResource("SB_ThumbClose") as Storyboard).Begin(this);
                        if (_userState != UserStateEnum.None) _userState = UserStateEnum.None; else _userState = UserStateEnum.AtHome;

                        _onRunningStoryBoard = false;
                        SB = null;
                    };
                    SB.Begin(this);
                    _onRunningStoryBoard = true;

                    //removes all images from touch canvas and set it removed
                    TouchCanvas.Children.Clear();
                    if (_myViewModel.SelectedPage != null)
                        foreach (ThumbElementBase element in _myViewModel.SelectedPage.Elements)
                            element.IsOnCanvas = false;

                    _myViewModel.GotoHome();

                    break;
                case "btnPlay":
                    mediaMusic.Play();
                    break;
                case "btnStop":
                    mediaMusic.Stop();
                    break;
                case "btnMute":
                    mediaMusic.IsMuted = !mediaMusic.IsMuted;
                    break;
                case "btnSlideShow":
                    _myViewModel.SlideShow.Start();
                    break;
                case "btnBack":
                    if (_onRunningStoryBoard) return;
                    SB = new Storyboard();
                    SB = (TryFindResource("SB_ButtonsOpen") as Storyboard).Clone();
                    SB.Completed += (object sender1, EventArgs e1) =>
                    {
                        if (_userState == UserStateEnum.AtPage) (TryFindResource("SB_ThumbClose") as Storyboard).Begin(this);
                        if (_userState != UserStateEnum.None) _userState = UserStateEnum.None; else _userState = UserStateEnum.AtHome;

                        _onRunningStoryBoard = false;
                        SB = null;
                    };
                    SB.Begin(this);
                    _onRunningStoryBoard = true;

                    _myViewModel.GoToBack();
                    break;
                case "btnExit":
                    Application.Current.Shutdown();
                    break;
            }
        }

        private void btnChangePicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog1;
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = PictureDialogFilters.DialogFilterList_PictureFormats();
            myDialog.Title = "Arka plan seçin";
            myDialog.Multiselect = false;
            if (myDialog.ShowDialog().Value)
            {
                if (myDialog.CheckFileExists)
                {
                    switch ((sender as MenuItem).Name)
                    {
                        case "btnChangeBackgroundPicture":
                            _myViewModel.BackgroundImage = myDialog.FileName;
                            return;
                        case "btnNav1ChangePicture":
                            _myViewModel.Nav1Image = myDialog.FileName;
                            return;
                        case "btnNav2ChangePicture":
                            _myViewModel.Nav2Image = myDialog.FileName;
                            return;
                        case "btnNav3ChangePicture":
                            _myViewModel.Nav3Image = myDialog.FileName;
                            return;
                        case "btnNav4ChangePicture":
                            _myViewModel.Nav4Image = myDialog.FileName;
                            return;
                        case "btnNav5ChangePicture":
                            _myViewModel.Nav5Image = myDialog.FileName;
                            return;
                        case "btnNav6ChangePicture":
                            _myViewModel.Nav6Image = myDialog.FileName;
                            return;
                        case "btnNav7ChangePicture":
                            _myViewModel.Nav7Image = myDialog.FileName;
                            return;
                        case "btnChangeTranslatePicture":
                            myDialog1 = new OpenFileDialog();
                            myDialog1.Filter = PictureDialogFilters.DialogFilterList_PictureFormats();
                            myDialog1.Title = "Ön plan seçin";
                            myDialog1.Multiselect = false;
                            if (myDialog1.ShowDialog().Value)
                                if (myDialog1.CheckFileExists)
                                {
                                    _myViewModel.TranslateOFFImage = myDialog.FileName;
                                    _myViewModel.TranslateONImage = myDialog1.FileName;
                                }
                            return;
                        case "btnChangeRotatePicture":
                            myDialog1 = new OpenFileDialog();
                            myDialog1.Filter = PictureDialogFilters.DialogFilterList_PictureFormats();
                            myDialog1.Title = "Ön plan seçin";
                            myDialog1.Multiselect = false;
                            if (myDialog1.ShowDialog().Value)
                                if (myDialog1.CheckFileExists)
                                {
                                    _myViewModel.RotateOFFImage = myDialog.FileName;
                                    _myViewModel.RotateONImage = myDialog1.FileName;
                                }
                            return;
                        case "btnChangeScalePicture":
                            myDialog1 = new OpenFileDialog();
                            myDialog1.Filter = PictureDialogFilters.DialogFilterList_PictureFormats();
                            myDialog1.Title = "Ön plan seçin";
                            myDialog1.Multiselect = false;
                            if (myDialog1.ShowDialog().Value)
                                if (myDialog1.CheckFileExists)
                                {
                                    _myViewModel.ScaleOFFImage = myDialog.FileName;
                                    _myViewModel.ScaleONImage = myDialog1.FileName;
                                }
                            return;
                    }
                    (sender as MenuItem).Tag = myDialog.FileName;
                }
            }
        }

        private void btnChangeNavFontSize_Click(object sender, RoutedEventArgs e)
        {
            int fSize = Convert.ToInt32((sender as MenuItem).Header.ToString());
            _myViewModel.NavFontSize = fSize;
        }

        private void btnNavFontChange_Click(object sender, SelectionChangedEventArgs e)
        {
            _myViewModel.NavFont = (sender as ListBox).SelectedValue.ToString();
        }

        private void btnNavFontColorChange_Click(object sender, SelectionChangedEventArgs e)
        {
            System.Drawing.Color c = System.Drawing.Color.FromName((sender as ListBox).SelectedValue.ToString());
            _myViewModel.NavForeColor = Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        private void btnChangePageFontSize_Click(object sender, RoutedEventArgs e)
        {
            int fSize = Convert.ToInt32((sender as MenuItem).Header.ToString());
            _myViewModel.PageFontSize = fSize;
        }

        private void btnPageFontChange_Click(object sender, SelectionChangedEventArgs e)
        {
            _myViewModel.PageFont = (sender as ListBox).SelectedValue.ToString();
        }

        private void btnPageFontColorChange_Click(object sender, SelectionChangedEventArgs e)
        {
            System.Drawing.Color c = System.Drawing.Color.FromName((sender as ListBox).SelectedValue.ToString());
            _myViewModel.PageForeColor = Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        private void btnAddPage_Click(object sender, RoutedEventArgs e)
        {
            _myViewModel.AddNewPage(PageViewModel.UseAsEnum.Thumb);
        }

        private void btnAddSubPage_Click(object sender, RoutedEventArgs e)
        {
            _myViewModel.AddNewPage(PageViewModel.UseAsEnum.Page);
        }

        private void btnDeletePage_Click(object sender, RoutedEventArgs e)
        {
            Guid g = (Guid)(sender as MenuItem).Tag;
            _myViewModel.DeletePage(g);
        }

        private void btnPage_Click(object sender, RoutedEventArgs e)
        {
            Guid g = (Guid)(sender as Button).Tag;
            _myViewModel.SelectPage(g);

            if (_onRunningStoryBoard) return;

            if (_myViewModel.SelectedPage.UseAs == PageViewModel.UseAsEnum.Thumb)
            {
                Storyboard SB = (TryFindResource("SB_ButtonsClose") as Storyboard).Clone();
                SB.Completed += (object sender1, EventArgs e1) =>
                {
                    _onRunningStoryBoard = false;
                    _userState = UserStateEnum.AtPage;
                    (TryFindResource("SB_ThumbOpen") as Storyboard).Begin(this);

                    SB = null;
                };
                SB.Begin(this);
                _onRunningStoryBoard = true;
            }
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = PictureDialogFilters.DialogFilterList_PictureFormats();
            myDialog.Title = "Resim seçin";
            myDialog.Multiselect = true;
            if (myDialog.ShowDialog().Value)
                foreach (string f in myDialog.FileNames)
                    if (File.Exists(f))
                        _myViewModel.SelectedPage.AddImage(f);
        }

        private void btnAddVideo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = PictureDialogFilters.DialogFilterList_VideoFormats();
            myDialog.Title = "Video seçin";
            myDialog.Multiselect = false;
            if (myDialog.ShowDialog().Value)
                if (File.Exists(myDialog.FileName))
                {
                    OpenFileDialog myDialog1 = new OpenFileDialog();
                    myDialog1.Filter = PictureDialogFilters.DialogFilterList_PictureFormats();
                    myDialog1.Title = "Video İçin Küçük Resim Seçin";
                    myDialog.Multiselect = false;
                    if (myDialog1.ShowDialog().Value)
                        if (File.Exists(myDialog1.FileName))
                            _myViewModel.SelectedPage.AddVideo(myDialog1.FileName, myDialog.FileName);
                }
        }

        private void btnDeleteElement_Click(object sender, RoutedEventArgs e)
        {
            Guid g = (Guid)(sender as MenuItem).Tag;
            _myViewModel.SelectedPage.DeleteElement(g);
        }

        private void btnDeleteAllElements_Click(object sender, RoutedEventArgs e)
        {
            _myViewModel.SelectedPage.DeleteAllElements();
        }

        private void btnElement_Click(object sender, RoutedEventArgs e)
        {
            if (_userState != UserStateEnum.AtPage) return;

            Guid g = (Guid)(sender as Button).Tag;

            ThumbElementBase elem = _myViewModel.SelectedPage.FindElement(g);
            if (elem != null)
            {
                if (elem.FileType == ThumbElementBase.FileTypeEnum.Image)
                {
                    this.TouchCanvas.Add_A_Picture((elem as ImageViewModel).Path, g);
                    elem.IsOnCanvas = true;
                }
                else if (elem.FileType == ThumbElementBase.FileTypeEnum.Video)
                {
                    this.TouchCanvas.Add_A_Video((elem as VideoViewModel).VideoPath, (elem as VideoViewModel).AutoStart, g);
                    elem.IsOnCanvas = true;
                }
            }
        }

        private void TouchCanvas_OnThrownOut(object sender, UIElement element)
        {
            ITouchElement TouchElement = element as ITouchElement;
            if (TouchElement != null)
            {
                ThumbElementBase elem = _myViewModel.SelectedPage.FindElement(TouchElement.ThumbID);
                if (elem != null)
                    elem.IsOnCanvas = false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SavePresentation();
        }

        private void btnSaveReadOnly(object sender, RoutedEventArgs e)
        {
            _myViewModel.ReadOnly = true;
            SavePresentation();
        }

        private void SavePresentation()
        {
            SaveFileDialog myDialog = new SaveFileDialog();
            myDialog.Filter = "Sunum Dosyası (*.usm)|*.usm";
            myDialog.Title = "Sunumu Kaydet";
            if (myDialog.ShowDialog().Value)
            {
                _myViewModel.SerializeToXML(myDialog.FileName);
                MessageBox.Show("Dosya Başarıyla Kaydedildi.", "Sunum Kaydet", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Sunum Dosyası (*.usm)|*.usm";
            myDialog.Title = "Sunum Dosyası Aç";
            if (myDialog.ShowDialog().Value)
                if (myDialog.CheckFileExists)
                {
                    MainWindow newWindow = new MainWindow(PresentationViewModel.DeserializeFromXML(myDialog.FileName));
                    newWindow.Show();
                    this.Close();
                }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMusicAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Muzik Dosyaları (*.wav, *.mp3, *.wma)|*.wma;*.mp3;*.wav";
            myDialog.Title = "Muzik Dosyası Aç";
            if (myDialog.ShowDialog().Value)
                if (myDialog.CheckFileExists)
                    _myViewModel.Music = myDialog.FileName;
        }

        private void btnMusicRemove_Click(object sender, RoutedEventArgs e)
        {
            _myViewModel.Music = string.Empty;
        }

        private void mediaMusic_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaElement elem = (sender as MediaElement);
            elem.Position = new TimeSpan();
            elem.Play();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            (new About()).ShowDialog();
        }

        private void btnAddSlides_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = PictureDialogFilters.DialogFilterList_PictureFormats();
            myDialog.Title = "Resim seçin";
            myDialog.Multiselect = true;
            if (myDialog.ShowDialog().Value)
                foreach (string f in myDialog.FileNames)
                    if (File.Exists(f))
                        _myViewModel.SlideShow.AddSlide(f);
        }

        private void btnDeleteAllSlides_Click(object sender, RoutedEventArgs e)
        {
            _myViewModel.SlideShow.DeleteAllSlides();
        }

        private void grdSlideShow_TouchDown(object sender, TouchEventArgs e)
        {
            _myViewModel.SlideShow.Stop();
        }

        private void grdSlideShow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _myViewModel.SlideShow.Stop();
        }

        private void SlideShow_SlideChanged(object sender, EventArgs e)
        {
            Storyboard SB = TryFindResource("SB_SlideShow") as Storyboard;
            SB.Begin(this);
        }

        private void btnRemovePanelBackgroundImage_Click(object sender, RoutedEventArgs e)
        {
            (sender as MenuItem).Tag = null;
        }
    }
}