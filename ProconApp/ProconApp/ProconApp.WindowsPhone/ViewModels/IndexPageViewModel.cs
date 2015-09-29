﻿using Microsoft.Practices.Prism.Mvvm;
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

    #region Converter
    public class BoldConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string culture)
        {
            if (value is bool)
                if ((bool)value)
                    return FontWeights.Bold;
                else
                    return FontWeights.Normal;
            else
                return FontWeights.Normal;
        }

        public object ConvertBack(object value, Type type, object parameter, string culture)
        {
            if (value is FontWeight)
                if (FontWeights.Equals(value, FontWeights.Bold))
                    return true;
                else
                    return false;
            else
                return false;
        }
    }
    #endregion

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

    

        public IndexPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }
        public async void setIndex(NavigateEnum type)
        {
            PageType = type;
            Visibility = Visibility.Visible;

            IEnumerable<SummaryItem> list;
            var count = 3;
            try
            {
                switch (this.PageType)
                {   
                    // 取得した要素の中から、表示中のものに対してidが一致しないものだけ追加
                    case NavigateEnum.Notice:
                        Name = "お知らせ";
                        list = (await Notice.getNotices(0, 3)).Where(l => !ItemList.Any(i => i.Id == l.Id)).ToList();
                        break;
                    case NavigateEnum.GameResult:
                        Name = "競技結果";
                        list = (await GameResult.getGameResults(3)).Where(l => !ItemList.Any(i => i.Id == l.Id)).ToList();
                        break;
                    case NavigateEnum.PhotoList:
                        Name = "会場フォト";
                        list = (await Photo.getPhotos(1)).Where(l => !ItemList.Any(i => i.Id == l.Id)).ToList();
                        count = 1;
                        break;
                    default:
                        throw new ArgumentException();
                }

                foreach (var item in list)
                {
                    ItemList.Add(item);
                    if(ItemList.Count > count)
                        ItemList.Remove(ItemList.Last());
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

            var profile = NetworkInformation.GetInternetConnectionProfile();
            
            if (profile != null &&  profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
                ItemUpdate();
            else
                await new Windows.UI.Popups.MessageDialog("ネットワーク接続に失敗しました。接続状況を確認してください。").ShowAsync();
            
        }

        private async void ItemUpdate()
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
