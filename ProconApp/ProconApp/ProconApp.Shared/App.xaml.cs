using Microsoft.Practices.Prism.Mvvm;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ProconApp.ViewModels;
using ProconApp.Views;
using ProconAPI;

using Windows.Networking.PushNotifications;
using Microsoft.WindowsAzure.Messaging;
using Windows.UI.Popups;
using System;
using System.Diagnostics;
using Windows.Storage;
using Newtonsoft.Json;
using Windows.ApplicationModel.Resources;

namespace ProconApp
{
    /// <summary>
    /// 既定の Application クラスに対してアプリケーション独自の動作を実装します。
    /// </summary>
    public sealed partial class App : MvvmAppBase
    {
        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {

            var rootFrame = (Frame)Window.Current.Content;
            rootFrame.Language = ApplicationLanguages.Languages[0];

            return Task.FromResult(default(object));
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            // 通知ハブに登録
            await InitNotificationsAsync();
            this.NavigationService.Navigate("Main", args.Arguments);
            
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {

#if WINDOWS_PHONE_APP
            ViewModelLocationProvider.Register(
                typeof(MainPage).FullName, () => new MainPageViewModel(this.NavigationService));
            ViewModelLocationProvider.Register(
                typeof(NotifyConfigPage).FullName, () => new NotifyConfigPageViewModel());
            ViewModelLocationProvider.Register(
                typeof(PhotoPage).FullName, () => new PhotoPageViewModel());
            ViewModelLocationProvider.Register(
                typeof(WebPage).FullName, () => new WebPageViewModel());
            ViewModelLocationProvider.Register(
                typeof(IndexPage).FullName, () => new IndexPageViewModel(this.NavigationService));
#endif
            return base.OnInitializeAsync(args);
        }

        private async Task InitNotificationsAsync()
        {
            // APIManagerの初期化処理
            await APIManager.Initialize();

            // 通知ハブからuriを取得していない（初回起動時）のみ実行
            string uri = ApplicationData.Current.LocalSettings.Values["Ch-Uri"] as string;
            if (uri != null)
                return;

            // リソースファイルから通知ハブに関するパラメータを取得
            var resLoader = ResourceLoader.GetForCurrentView("Resources");
            var endppoint = resLoader.GetString("endpoint");
            var hubname = resLoader.GetString("hubname");

            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            var hub = new NotificationHub(hubname, endppoint);

            var result = await hub.RegisterNativeAsync(channel.Uri);

            Debug.WriteLine(channel.Uri.ToString());


            // 通知ハブに接続成功時 
            if (result.RegistrationId != null)
            {
                /* ページ表示時に、トークン取得済みでなければ取得処理を実行 */
                string tkn = ApplicationData.Current.LocalSettings.Values["Token"] as string;

                var res = APIManager.PushDeviceSet(channel.Uri.ToString());
                Debug.WriteLine(res.ToString());

                ApplicationData.Current.LocalSettings.Values["Ch-Uri"] = channel.Uri.ToString();
            }
        }
    }
}