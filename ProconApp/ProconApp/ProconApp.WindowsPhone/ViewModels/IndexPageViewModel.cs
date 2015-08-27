using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using ProconApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;

namespace ProconApp.ViewModels
{
    public class IndexPageViewModel : ViewModel
    {

        private INavigationService navigationService;

        #region PageType

        public NavigateEnum PageType { get; private set; }

        #endregion

        #region Name

        private string name;

        public string Name
        {
            get { return name; }
            set { this.SetProperty(ref name, value); }
        }

        #endregion

        #region ItemList

        private ObservableCollection<SummaryItem> itemList = new ObservableCollection<SummaryItem>();
        /// <summary>
        /// お知らせ一覧に表示されるList
        /// </summary>
        public ObservableCollection<SummaryItem> ItemList
        {
            get { return this.itemList; }
            set { this.SetProperty(ref itemList, value); }
        }

        #endregion

        #region NavigateCommand

        private DelegateCommand<SummaryItem> navigateCommand;
        /// <summary>
        /// ViewにバインドされるNavigateCommand
        /// </summary>
        public DelegateCommand<SummaryItem> NavigateCommand
        {
            get { return this.navigateCommand ?? (this.navigateCommand = new DelegateCommand<SummaryItem>(Navigate)); }
        }

        #endregion

        public IndexPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            // 画面遷移してきたときに呼ばれる
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            this.PageType = (NavigateEnum)navigationParameter;
            switch (this.PageType)
            {
                case NavigateEnum.Notice:
                    Name = "お知らせ";
                    ItemList = new ObservableCollection<SummaryItem>(await Notice.getNotices(0, 20));
                    break;
                case NavigateEnum.GameResult:
                    Name = "競技結果";
                    ItemList = new ObservableCollection<SummaryItem>(await GameResult.getGameResults(20));
                    break;
                case NavigateEnum.PhotoList:
                    Name = "会場フォト";
                    ItemList = new ObservableCollection<SummaryItem>(await Photo.getPhotos(20));
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        protected void Navigate(SummaryItem item)
        {
            switch (this.PageType)
            {
                case NavigateEnum.Notice:
                    // WebView
                    break;
                case NavigateEnum.GameResult:
                    // 詳細？
                    break;
                case NavigateEnum.PhotoList:
                    this.navigationService.Navigate("Photo", ((Photo.PhotoItem)item));
                    break;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
