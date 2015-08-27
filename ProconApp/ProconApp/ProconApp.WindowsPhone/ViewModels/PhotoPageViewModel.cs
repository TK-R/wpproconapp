using Microsoft.Practices.Prism.Mvvm;
using ProconApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace ProconApp.ViewModels
{
    public class PhotoPageViewModel : ViewModel
    {
        #region PhotoULR

        private Photo.PhotoItem photo;
        public Photo.PhotoItem Photo
        {
            set { this.SetProperty(ref photo, value); }
            get { return photo; }
        }

        #endregion

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            // 画面遷移してきたときに呼ばれる
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            Photo = navigationParameter as Photo.PhotoItem;
        }
    }
}
