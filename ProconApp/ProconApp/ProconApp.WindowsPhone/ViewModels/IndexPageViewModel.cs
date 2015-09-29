using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using ProconApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using Windows.UI.Xaml.Data;
using System.Globalization;
using Windows.UI.Text;
using System.Net.NetworkInformation;
using Windows.Networking.Connectivity;

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

        #region Visibility

        private Visibility visibility;

        public Visibility Visibility
        {
            get { return visibility; }
            set { this.SetProperty(ref visibility, value); }
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

        #region Loading
        private bool loading = false;
        public bool Loading
        {
            get { return loading; }
            set { this.SetProperty(ref loading, value); }
        }
        #endregion


        public IndexPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }
        public async void setIndex(NavigateEnum type)
        {
            PageType = type;
            Visibility = Visibility.Visible;

            try
            {
                switch (this.PageType)
                {
                    case NavigateEnum.Notice:
                        Name = "お知らせ";
                        ItemList = new ObservableCollection<SummaryItem>(await Notice.getNotices(0, 3));
                        break;
                    case NavigateEnum.GameResult:
                        Name = "競技結果";
                        ItemList = new ObservableCollection<SummaryItem>(await GameResult.getGameResults(3));
                        break;
                    case NavigateEnum.PhotoList:
                        Name = "会場フォト";
                        ItemList = new ObservableCollection<SummaryItem>(await Photo.getPhotos(1));
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
            catch
            {
                return;
            }
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            // 画面遷移してきたときに呼ばれる
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            this.PageType = (NavigateEnum)navigationParameter;
            this.Visibility = Visibility.Collapsed;

            Loading = true;
            
            var profile = NetworkInformation.GetInternetConnectionProfile();
            
            if (profile != null &&  profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
                await ItemUpdate();
            else
                await new Windows.UI.Popups.MessageDialog("ネットワーク接続に失敗しました。接続状況を確認してください。").ShowAsync();

            Loading = false;
        }

        private async Task ItemUpdate()
        {
            try
            {
                switch (this.PageType)
                {
                    case NavigateEnum.Notice:
                        Name = "お知らせ";
                        ItemList = new ObservableCollection<SummaryItem>(await Notice.getNotices(0, 20));
                        break;
                    case NavigateEnum.GameResult:
                        Name = "競技結果";
                        ItemList = new ObservableCollection<SummaryItem>(await GameResult.getGameResultItems(20));
                        break;
                    case NavigateEnum.PhotoList:
                        Name = "会場フォト";
                        ItemList = new ObservableCollection<SummaryItem>(await Photo.getPhotos(20));
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
            catch(NullReferenceException)
            {
                return;
            }
        }

        #region NavigateCommand

        private DelegateCommand navigateCommand;
        /// <summary>
        /// ViewにバインドされるNavigateCommand
        /// </summary>
        public DelegateCommand NavigateCommand
        {
            get { return this.navigateCommand ?? (this.navigateCommand = new DelegateCommand(Navigate)); }
        }

        #endregion

        protected void Navigate()
        {
            this.navigationService.Navigate("Index", this.PageType);
        }

        #region ItemNavigateCommand

        private DelegateCommand<SummaryItem> itemNavigateCommand;
        /// <summary>
        /// ListViewItemにバインドされるItemNavigateCommand
        /// </summary>
        public DelegateCommand<SummaryItem> ItemNavigateCommand
        {
            get { return this.itemNavigateCommand ?? (this.itemNavigateCommand = new DelegateCommand<SummaryItem>(Navigate)); }
        }

        #endregion

        protected void Navigate(SummaryItem item)
        {
            switch (this.PageType)
            {
                case NavigateEnum.Notice:
                    // NoticeView
                    this.navigationService.Navigate("Notice", item);

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
