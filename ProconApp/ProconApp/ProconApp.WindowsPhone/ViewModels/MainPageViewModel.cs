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
using Windows.UI.Xaml.Navigation;


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

            var api = new APIManager();

            /* ページ表示時に、トークン取得済みでなければ取得処理を実行 */
            string tkn = ApplicationData.Current.RoamingSettings.Values["Token"] as string;

            try
            {
                /* トークンがnullなら取得処理を実行 */
                if (tkn == null)
                {
                    tkn = JsonConvert.DeserializeObject<NewUserResponseJson>(await api.NewUser()).user_token;
                    ApplicationData.Current.RoamingSettings.Values["Token"] = tkn;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            /* 速報と競技結果を取得 */
            try
            {
                var notice = JsonConvert.DeserializeObject<List<NoticeListObject>>(await api.NoticeList(tkn, 0));
                var result = JsonConvert.DeserializeObject<List<GameResultObject>>(await api.GameResult(tkn, 3));

                foreach (var n in notice)
                    NoticeItemList.Add(new SummaryItem {
                        Date = DateTimeHelper.DiffTimeString(n.published_at, DateTime.Now),
                        Title = n.title });

                foreach (var r in result)
                    ResultItemList.Add(new SummaryItem {
                        Date = DateTimeHelper.DiffTimeString(r.finished_at, DateTime.Now),
                        Title = r.title
                    });
               
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
            this.navigationService.Navigate("Setting", null);
        }
    }

}
