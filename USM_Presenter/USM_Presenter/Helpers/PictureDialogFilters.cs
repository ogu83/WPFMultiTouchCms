using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USM_Presenter.Helpers
{
    public class PictureDialogFilters
    {
        public enum DialogFilterList_PictureFormats_Enum
        {
            BMP = 1, JPG = 0, PNG = 2, GIF = 3,
            TIFF = 4, WMF = 5, EMF = 6, EXIF = 7
        }

        public static string DialogFilterList_PictureFormats()
        {
            return "Resim Dosyaları (*.jpg, *.gif, *.bmp, *.png, *.jpeg)|*.jpg;*.gif;*.bmp;*.png;*.jpeg";
            //return "resim Dosyaları|*.jpg;*.gif;*.bmp;*.png;*.jpeg|Tüm Dosyalar|*.*";

            //string retVal;
            //retVal = "Portable Network Graphics (*.png)|*.png";
            //retVal += "|Bitmap (*.bmp)|*.bmp";
            //retVal += "|Joint Photographics Experts Group (*.jpg)|*.jpg";
            //retVal += "|Graphics Interchange Format (*.gif)|*.gif";
            //retVal += "|Tagged Image File Format (*.tiff)|*.tiff";
            //retVal += "|Enchanced Meta File (*.emf)|*.emf";
            //retVal += "|Exchangeable Image File (*.exif)|*.exif";
            //retVal += "|All Files (*.*)|*.*";
            //return retVal;
        }

        public static string DialogFilterList_VideoFormats()
        {
            return "Video Dosyaları (*.wmv, *.avi)|*.wmv;*.avi";
        }
    }
}
