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
    public class WebPageViewModel : ViewModel
    {
        #region PageURL

        private string pageURL;
        public string PageURL
        {
            set { this.SetProperty(ref pageURL, value); }
            get { return pageURL; }
        }

        #endregion

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            // 画面遷移してきたときに呼ばれる
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            PageURL = navigationParameter as string;
        }
    }
}
