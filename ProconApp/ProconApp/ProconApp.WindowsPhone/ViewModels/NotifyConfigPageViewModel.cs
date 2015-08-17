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
    /// <summary>
    /// プッシュ通知設定画面のViewModel
    /// </summary>
    public class NotifyConfigPageViewModel : ViewModel
    {
        private ObservableCollection<NotifyConfigItem> notifyConfigItemList = new ObservableCollection<NotifyConfigItem>();
        /// <summary>
        /// 通知設定用の出場校一覧リスト
        /// </summary>
        public ObservableCollection<NotifyConfigItem> NotifyConfigItemList
        {
            get { return notifyConfigItemList; }
            set { this.SetProperty(ref notifyConfigItemList, value); }
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            // 画面遷移してきたときに呼ばれる
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        
            // APIを用いて出場校一覧を取得
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

            //出場校一覧を取得
            var players = JsonConvert.DeserializeObject<List<PlayerObject>>(await api.Players(tkn));

            NotifyConfigItemList.Clear();
            foreach (var p in players)
            {
                NotifyConfigItemList.Add(new NotifyConfigItem { SchoolName = p.name, ID = p.id });
            }
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            // 画面遷移する前に呼ばれる
            base.OnNavigatedFrom(viewModelState, suspending);

            // 選択済み出場校をもとに、通知を送信
        }


    }
}
