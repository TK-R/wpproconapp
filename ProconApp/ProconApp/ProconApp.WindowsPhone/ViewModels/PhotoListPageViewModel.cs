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
    public class PhotoListPageViewModel : ViewModel
    {
        #region NoticeItemList

        private ObservableCollection<Photo.PhotoItem> photoItemList = new ObservableCollection<Photo.PhotoItem>();
        /// <summary>
        /// 写真一覧一覧に表示されるList
        /// </summary>
        public ObservableCollection<Photo.PhotoItem> PhotoItemList
        {
            get { return this.photoItemList; }
            set { this.SetProperty(ref photoItemList, value); }
        }

        #endregion

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            // 画面遷移してきたときに呼ばれる
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            PhotoItemList = new ObservableCollection<Photo.PhotoItem>(await Photo.getPhotos(20));
        }
    }
}
