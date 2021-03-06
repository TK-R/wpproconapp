﻿using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

using ProconApp.Models;
using Windows.UI.Xaml;
using Windows.UI.Core;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml.Data;

namespace ProconApp.ViewModels
{

    public class MainPageViewModel : ViewModel
    {

        /// <summary>
        /// コンストラクタで渡してもらったNavigationService 
        /// </summary>
        private INavigationService navigationService;

        #region URL
        /// <summary>
        /// WebPageに表示するMapのURL
        /// </summary>
        static string RouteUrl;

        /// <summary>
        /// WebPageに表示する当日プログラムのURL
        /// </summary>
        static string ProgramUrl;

        /// <summary>
        /// WebPageに表示する「このアプリについて」のURL
        /// </summary>
        static string InfoUrl;

        #endregion

        #region NoticeViewModel

        private IndexPageViewModel noticeViewModel;
        /// <summary>
        /// お知らせ一覧に表示されるList
        /// </summary>
        public IndexPageViewModel NoticeViewModel
        {
            get { return this.noticeViewModel; }
            set { this.SetProperty(ref noticeViewModel, value); }
        }

        #endregion

        #region ResultViewModel

        /// <summary>
        /// 競技結果速報に表示されるList
        /// </summary>
        private IndexPageViewModel resultViewModel;
        public IndexPageViewModel ResultViewModel
        {
            get { return this.resultViewModel; }
            set { this.SetProperty(ref resultViewModel, value); }
        }

        #endregion

        #region SocialItemList

        private ObservableCollection<Social.SocialItem> socialItemList = new ObservableCollection<Social.SocialItem>();
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Social.SocialItem> SocialItemList
        {
            get { return this.socialItemList; }
            set { this.SetProperty(ref socialItemList, value); }
        }

        #endregion

        #region PhotoViewModel

        private IndexPageViewModel photoViewModel;
        /// <summary>
        /// メインページに表示する画像
        /// </summary>
        public IndexPageViewModel PhotoViewModel
        {
            get { return photoViewModel; }
            set { this.SetProperty(ref photoViewModel, value); }
        }

        #endregion

        #region SelectedIndex

        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { this.SetProperty(ref selectedIndex, value); }
        }
        #endregion

        #region MainPageEnum

        enum MainPageEnum
        {
            Home,
            Social
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

        public async Task ItemUpdate()
        {
            NoticeViewModel.setIndex(NavigateEnum.Notice);
            ResultViewModel.setIndex(NavigateEnum.GameResult);
            PhotoViewModel.setIndex(NavigateEnum.PhotoList);

            // ツイートデータを取得
            var social = new ObservableCollection<Social.SocialItem>(await Social.getSocialItems(30));

            if (social != null)
                SocialItemList = social;

            if (SelectedIndex == (int)MainPageEnum.Home)
            {
                // GoogleAnalyticsに情報を送信（Home）
                AnalyticHelper.SendGAnalytics(AnalyticHelper.ViewParam.Home);
            }
            else
            {
                // GoogleAnalyticsに情報を送信（Social）
                AnalyticHelper.SendGAnalytics(AnalyticHelper.ViewParam.Social);
            }
        }

        public async Task Update()
        {
            // 失敗時にはネットワークエラーを表示
            var profile = NetworkInformation.GetInternetConnectionProfile();

            if (profile != null && profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
            {
                Loading = true;
                await ItemUpdate();
                Loading = false;
            }
            else
            {
                await new Windows.UI.Popups.MessageDialog("ネットワーク接続に失敗しました。接続状況を確認してください。").ShowAsync();
            }

        }

        /// <summary>
        /// NavigationServiceを受け取るためのコンストラクタ
        /// </summary>
        /// <param name="navigationService"></param>
        public MainPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            // URLを初期化
            var resLoader = ResourceLoader.GetForCurrentView("Resources");
            RouteUrl = resLoader.GetString("routeurl");
            ProgramUrl = resLoader.GetString("programurl");
            InfoUrl = resLoader.GetString("infourl");

            NoticeViewModel = new IndexPageViewModel(this.navigationService);
            ResultViewModel = new IndexPageViewModel(this.navigationService);
            PhotoViewModel = new IndexPageViewModel(this.navigationService);

        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            // 画面遷移してきたときに呼ばれる
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            // 競技/お知らせ/画像を取得
            await Update();
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            // 画面遷移する前に呼ばれる
            base.OnNavigatedFrom(viewModelState, suspending);
        }

        #region NavigateCommand

        private DelegateCommand<string> navigateCommand;
        /// <summary>
        /// ViewにバインドされるNavigateCommand
        /// </summary>
        public DelegateCommand<string> NavigateCommand
        {
            get { return this.navigateCommand ?? (this.navigateCommand = new DelegateCommand<string>(Navigate)); }
        }

        #endregion

        #region RefreshCommand

        private DelegateCommand refreshCommand;
        /// <summary>
        /// ViewniバインドされるRefreshCommand
        /// </summary>
        public DelegateCommand RefreshCommand
        {
            get { return this.refreshCommand ?? (this.refreshCommand = DelegateCommand.FromAsyncHandler(Update)); }
        }

        #endregion

        #region ItemNavigateCommand

        private DelegateCommand<Social.SocialItem> itemNavigateCommand;
        /// <summary>
        /// ListViewItemにバインドされるItemNavigateCommand
        /// </summary>
        public DelegateCommand<Social.SocialItem> ItemNavigateCommand
        {
            get { return this.itemNavigateCommand ?? (this.itemNavigateCommand = new DelegateCommand<Social.SocialItem>(Navigate)); }
        }

        #endregion

        protected void Navigate(Social.SocialItem item)
        {
            var url = "https://twitter.com/" + item.ScreenName.Remove(0, 1) + "/status/" + item.TweeetID;
            this.navigationService.Navigate("Web", url);
        }

        /// <summary>
        /// 画面の呼び出しを行う
        /// </summary>
        protected void Navigate(string parameter)
        {
            var param = (NavigateEnum)Enum.Parse(typeof(NavigateEnum), parameter);

            switch (param)
            {
                case NavigateEnum.Setting:
                    this.navigationService.Navigate("NotifyConfig", null);
                    break;
                case NavigateEnum.Map:
                    this.navigationService.Navigate("Web", RouteUrl);
                    break;
                case NavigateEnum.Program:
                    this.navigationService.Navigate("Web", ProgramUrl);
                    break;
                case NavigateEnum.Info:
                    this.navigationService.Navigate("Web", InfoUrl);
                    break;

                default:
                    break;
            }
        }
    }

    public enum NavigateEnum
    {
        Setting,
        Notice,
        GameResult,
        PhotoList,
        Info,
        Map,
        Program
    }

}
