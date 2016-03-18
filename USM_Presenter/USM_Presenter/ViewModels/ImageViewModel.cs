using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace USM_Presenter.ViewModels
{
    [Serializable()]
    public class ImageViewModel : ThumbElementBase
    {
        public ImageViewModel() { base._fileType = FileTypeEnum.Image; }

        public ImageViewModel(PageViewModel parent)
            : this()
        {
            _path = "images/ButtonNone.png";
            _parent = parent;
        }

        public ImageViewModel(string path, PageViewModel parent)
            : this(parent)
        {
            Path = path;
        }

    }
}
