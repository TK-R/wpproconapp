using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Newtonsoft.Json;
using ProconAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Linq;

namespace ProconApp.ViewModels
{

    public class MainPageViewModel : ViewModel
    {

        /// <summary>
        /// コンストラクタで渡してもらったNavigationService 
        /// </summary>
        private INavigationService navigationService;
        
        private ObservableCollection<SummaryItem> noticeItemList = new ObservableCollection<SummaryItem>();
        /// <summary>
        /// お知らせ一覧に表示されるList
        /// </summary>
        public ObservableCollection<SummaryItem> NoticeItemList
        {
            get { return this.noticeItemList; }
            set { this.SetProperty(ref noticeItemList, value); }
        }

        /// <summary>
        /// 競技結果速報に表示されるList
        /// </summary>
        private ObservableCollection<SummaryItem> resultItemList = new ObservableCollection<SummaryItem>();
        public ObservableCollection<SummaryItem> ResultItemList
        {
            get { return this.resultItemList; }
            set { this.SetProperty(ref resultItemList, value); }
        }

        private ObservableCollection<SocialItem> socialItemList = new ObservableCollection<SocialItem>();
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<SocialItem> SocialItemList
        {
            get { return this.socialItemList; }
            set { this.SetProperty(ref socialItemList, value); }
        }



        private string photoURL;
        /// <summary>
        /// メインページに表示する画像のURL
        /// </summary>
        public string PhotoURL
        {
            set { this.SetProperty(ref photoURL, value); }
            get { return photoURL; }
        }

        private string photoTitle;
        /// <summary>
        /// メインページに表示する画像のタイトル
        /// </summary>
        public string PhotoTitle
        {
            set { this.SetProperty(ref photoTitle, value); }
            get { return photoTitle; }
        }

        private string photoDate;
        /// <summary>
        /// メインページに表示する画像のタイトル
        /// </summary>
        public string PhotoDate
        {
            set { this.SetProperty(ref photoDate, value); }
            get { return photoDate; }
        }

        private DelegateCommand settingCommand;
        /// <summary>
        /// ViewにバインドされるSettingCommand
        /// </summary>
        public DelegateCommand SettingCommand
        {
            get { return this.settingCommand ?? (this.settingCommand = new DelegateCommand(SettingExecute)); }
        }


        /// <summary>
        /// NavigationServiceを受け取るためのコンストラクタ
        /// </summary>
        /// <param name="navigationService"></param>
        public MainPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            // 画面遷移してきたときに呼ばれる
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            // 競技/お知らせ/画像を取得
            try
            {
                var notice = JsonConvert.DeserializeObject<List<NoticeListObject>>(await APIManager.NoticeList(0));
                var result = JsonConvert.DeserializeObject<List<GameResultObject>>(await APIManager.GameResult(3));
                var photo = JsonConvert.DeserializeObject<List<PhotoData>>(await APIManager.PhotoData(1)).FirstOrDefault();
                var social = JsonConvert.DeserializeObject<APISocialObject>(await APIManager.SocialData(30));

                // 画像を反映
                PhotoURL = photo.thumbnail_url;
                PhotoTitle = photo.title;
                PhotoDate = DateTimeHelper.DiffTimeString(photo.created_at, DateTime.UtcNow); 

                foreach (var n in notice.Take(3))
                    NoticeItemList.Add(new SummaryItem {
                        Date = DateTimeHelper.DiffTimeString(n.published_at, DateTime.UtcNow),
                        Title = n.title });

                foreach (var r in result)
                    ResultItemList.Add(new SummaryItem {
                        Date = DateTimeHelper.DiffTimeString(r.finished_at, DateTime.UtcNow),
                        Title = r.title
                    });
                
                foreach(var status in  social.statuses)
                    SocialItemList.Add(new SocialItem(status));

            }catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString()); 
            }

        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            // 画面遷移する前に呼ばれる
            base.OnNavigatedFrom(viewModelState, suspending);

        }

        /// <summary>
        /// 通知画面の呼び出しを行う
        /// </summary>
        private void SettingExecute()
        {
            this.navigationService.Navigate("NotifyConfig", null);
        }
    }

}
