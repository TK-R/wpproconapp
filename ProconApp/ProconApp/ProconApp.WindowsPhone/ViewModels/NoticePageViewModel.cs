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
    public class NoticePageViewModel : ViewModel
    {
        #region PageHTML

        private string pageHTML;
        public string PageHTML
        {
            set { this.SetProperty(ref pageHTML, value); }
            get { return pageHTML; }
        }

        #endregion

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            // 画面遷移してきたときに呼ばれる
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            
            // 概要を取得してから、IDを取得
            var summaryItem = (navigationParameter as SummaryItem);
            if (summaryItem == null)
                return;

            var item = await Notice.getNotice(summaryItem.Id);

            var html = HTMLHelper.GetFullHTMLString(item.Text);
            PageHTML = html;


        }
    }
}
