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
        #endregion

        #region NoticeItemList

        private ObservableCollection<Notice.NoticeSummaryItem> noticeItemList = new ObservableCollection<Notice.NoticeSummaryItem>();
        /// <summary>
        /// お知らせ一覧に表示されるList
        /// </summary>
        public ObservableCollection<Notice.NoticeSummaryItem> NoticeItemList
        {
            get { return this.noticeItemList; }
            set { this.SetProperty(ref noticeItemList, value); }
        }
        
        #endregion

        #region ResultItemList

        /// <summary>
        /// 競技結果速報に表示されるList
        /// </summary>
        private ObservableCollection<SummaryItem> resultItemList = new ObservableCollection<SummaryItem>();
        public ObservableCollection<SummaryItem> ResultItemList
        {
            get { return this.resultItemList; }
            set { this.SetProperty(ref resultItemList, value); }
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

        #region PhotoItem

        private Photo.PhotoItem photoItem;
        /// <summary>
        /// メインページに表示する画像
        /// </summary>
        public Photo.PhotoItem PhotoItem
        {
            get { return photoItem; }
            set { this.SetProperty(ref photoItem, value); }
        }
        
        #endregion

        #region SelectedIndex

        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (this.SetProperty(ref selectedIndex, value))
                {
                    update();
                }
            }
        }

        #endregion

        #region MainPageEnum

        enum MainPageEnum
        {
            Home,
            Social
        }

        #endregion

        public async Task update()
        {
            if (SelectedIndex == (int)MainPageEnum.Home)
            {
                NoticeItemList = new ObservableCollection<Notice.NoticeSummaryItem>(await Notice.getNotices(0, 3));
                ResultItemList = new ObservableCollection<SummaryItem>(await GameResult.getGameResults(3));
                PhotoItem = (await Photo.getPhotos(1)).FirstOrDefault();
                return;
            }
            else if(SelectedIndex == (int)MainPageEnum.Social)
            {
                SocialItemList = new ObservableCollection<Social.SocialItem>(await Social.getSocialItems(30));
                return;
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
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            // 画面遷移してきたときに呼ばれる
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            // 競技/お知らせ/画像を取得
            try
            {
               await update();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            // 画面遷移する前に呼ばれる
            base.OnNavigatedFrom(viewModelState, suspending);
        }

        #region NavigateCommand

        public class NavigateCommandClass : System.Windows.Input.ICommand
        {
            private Action<NavigateEnum> action;
            public NavigateCommandClass(Action<NavigateEnum> action)
            {
                this.action = action;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                action.Invoke((NavigateEnum)Enum.Parse(typeof(NavigateEnum), (string)parameter));
            }
        }


        public enum NavigateEnum
        {
            Setting,
            Notice,
            GameResult,
            PhotoList,
            Map,
            Program
        }

        private NavigateCommandClass navigateCommand;
        /// <summary>
        /// ViewにバインドされるNavigateCommand
        /// </summary>
        public NavigateCommandClass NavigateCommand
        {
            get { return this.navigateCommand ?? (this.navigateCommand = new NavigateCommandClass(Navigate)); }
        }

        #endregion

        

        /// <summary>
        /// 画面の呼び出しを行う
        /// </summary>
        protected void Navigate(NavigateEnum param)
        {
            switch (param)
            {
                case NavigateEnum.Setting:
                    this.navigationService.Navigate("NotifyConfig", null);
                    break;
                case NavigateEnum.Notice:
                    this.navigationService.Navigate("Notify", null);
                    break;
                case NavigateEnum.GameResult:
                    break;
                case NavigateEnum.PhotoList:
                    this.navigationService.Navigate("PhotoList", null);
                    break;
                case NavigateEnum.Map:
                    this.navigationService.Navigate("Web", RouteUrl);
                    break;
                case NavigateEnum.Program:
                    
                    this.navigationService.Navigate("Web", ProgramUrl);
                    break;
                default:
                    break;
            }
        }

    }

}
