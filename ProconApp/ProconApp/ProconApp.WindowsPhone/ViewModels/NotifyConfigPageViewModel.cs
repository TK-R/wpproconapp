using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Newtonsoft.Json;
using System.Linq;
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

            try
            {
                // 出場校一覧を取得
                var players = JsonConvert.DeserializeObject<List<PlayerObject>>(await APIManager.Players());

                // サーバ側の通知登録リストを取得
                var notifyList = JsonConvert.DeserializeObject<GameNotificationIDs>(await APIManager.GameNotificationGet());

                NotifyConfigItemList.Clear();

                // 出場校一覧を画面に反映
                foreach (var p in players)
                {
                    // サーバ側に登録されていれば、スイッチをONにする。
                    var item = new NotifyConfigItem { SchoolName = p.name, ID = p.id };
                    item.NotifyFlag = notifyList.ids.Any(n => n == item.ID);

                    NotifyConfigItemList.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public override async void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            // 画面遷移する前に呼ばれる
            base.OnNavigatedFrom(viewModelState, suspending);

            // 選択済み出場校をもとに、通知を送信
            var ids = NotifyConfigItemList.Where(t=> t.NotifyFlag).Select(t => t.ID).ToArray();

            /* ページ表示時に、トークン取得済みでなければ取得処理を実行 */
            string tkn = ApplicationData.Current.RoamingSettings.Values["Token"] as string;



            await APIManager.GameNotificationSet(ids);

        }
    }
}
