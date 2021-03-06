﻿using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Newtonsoft.Json;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI.Xaml.Navigation;

using ProconApp.Models;

namespace ProconApp.ViewModels
{
    /// <summary>
    /// プッシュ通知設定画面のViewModel
    /// </summary>
    public class NotifyConfigPageViewModel : ViewModel
    {
        #region NotifyConfigItemList

        private ObservableCollection<NotifyConfig.NotifyConfigItem> notifyConfigItemList = new ObservableCollection<NotifyConfig.NotifyConfigItem>();
        /// <summary>
        /// 通知設定用の出場校一覧リスト
        /// </summary>
        public ObservableCollection<NotifyConfig.NotifyConfigItem> NotifyConfigItemList
        {
            get { return notifyConfigItemList; }
            set { this.SetProperty(ref notifyConfigItemList, value); }
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

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            // 画面遷移してきたときに呼ばれる
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            Loading = true;

            try
            {
                // 出場校一覧を取得
                var players = await Player.getPlayers();


                // 初回起動時の場合、設定画面に強制遷移
                var firstLaunch = ApplicationData.Current.LocalSettings.Values["FisrtLaunch"];
          
                // 初回起動時にはすべての通知をON
                if (firstLaunch as string == null)
                {
                    NotifyConfigItemList = new ObservableCollection<NotifyConfig.NotifyConfigItem>(NotifyConfig.getNotifyConfigItems(players));

                    // フラグを埋め立て
                    ApplicationData.Current.LocalSettings.Values["FisrtLaunch"] = "Done";
                }
                else
                {
                    // サーバ側の通知登録リストを取得
                    var notifyList = await GameNotification.getGameNotification();
                    NotifyConfigItemList = new ObservableCollection<NotifyConfig.NotifyConfigItem>(NotifyConfig.getNotifyConfigItems(players, notifyList));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            Loading = false;
        }

        public override async void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            // 画面遷移する前に呼ばれる
            base.OnNavigatedFrom(viewModelState, suspending);

            // 選択済み出場校をもとに、通知を送信
            var ids = NotifyConfigItemList
                .Where(t=> t.NotifyFlag)
                .Select(t => t.ID)
                .ToArray();

            await ProconAPI.APIManager.GameNotificationSet(ids);

        }
    }
}
