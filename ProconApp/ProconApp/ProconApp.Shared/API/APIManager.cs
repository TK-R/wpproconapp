using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Net.Http.Headers;
using Windows.Storage;
using Windows.ApplicationModel.Resources;

namespace ProconAPI
{
    public static class APIManager
    {
        /// <summary>
        /// プロコンAPIサーバーのURL
        /// </summary>
        private static string APIDomain;

        /// <summary>
        /// Manager内で共通で使用するユーザートークン
        /// </summary>
        public static string UserToken;


        /// <summary>
        /// API管理クラスの初期化処理
        /// </summary>
        /// <returns></returns>
        public static async Task Initialize()
        {
            var resLoader = ResourceLoader.GetForCurrentView("Resources");
            APIDomain = resLoader.GetString("domain");

            var tkn = ApplicationData.Current.LocalSettings.Values["Token"] as string;
            if (tkn != null)
            {
                UserToken = tkn;
                return;
            }

            var httpClient = new HttpClient();
         
            try
            {
                var res = await httpClient.PostAsync(new Uri(APIDomain + "/auth/new_user"), null);
                UserToken = JsonConvert.DeserializeObject<NewUserResponseJson>(await res.Content.ReadAsStringAsync()).user_token;
                ApplicationData.Current.LocalSettings.Values["Token"] = UserToken;
            }
            catch (HttpRequestException he)
            {
                Debug.WriteLine(he.ToString());
                return ;
            }
            catch (Exception e)
            {
                // For debugging
                Debug.WriteLine(e.ToString());
                return;
            }
        }

        /// <summary>
        /// ユーザー情報取得メソッド
        /// </summary>
        /// <returns></returns>
        public static async Task<string> UserInfo()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-User-Token", UserToken);
            try
            {                
                var res = await httpClient.GetAsync(new Uri(APIDomain + "/user/me/info"));
                return await res.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException he)
            {
                Debug.WriteLine(he.ToString());
                return null;
            }
            catch (Exception e)
            {
                // For debugging
                Debug.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// お知らせ一覧取得
        /// </summary>
        /// <param name="page">リクエストするページNo.</param>
        /// <param name="count">取得するお知らせ数</param>
        /// <returns>お知らせ一覧の文字列データ</returns>
        public static async Task<string> NoticeList(int page, int count)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-User-Token", UserToken);

            try
            {
                var res = await httpClient.GetAsync(new Uri(APIDomain + "/notices/list?page=" + page.ToString() + "&count=" + count.ToString()));
                return await res.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException he)
            {
                Debug.WriteLine(he.ToString());
                return null;
            }
            catch (Exception e)
            {
                // For debugging
                Debug.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// お知らせ本文取得
        /// </summary>
        /// <param name="itemID">リクエストするアイテムID</param>
        /// <returns>お知らせ本文の文字列</returns>
        public static async Task<string> NoticeItem(int itemID)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-User-Token", UserToken);

            try
            {
                var res = await httpClient.GetAsync(new Uri(APIDomain + "/notices/info?id="+ itemID.ToString()));
                return await res.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException he)
            {
                Debug.WriteLine(he.ToString());
                return null;
            }
            catch (Exception e)
            {
                // For debugging
                Debug.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// 出場校リスト取得
        /// </summary>
        /// <returns>出場校リストの文字列</returns>
        public static async Task<string> Players()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-User-Token", UserToken);

            try
            {
                var res = await httpClient.GetAsync(new Uri(APIDomain + "/players"));
                return await res.Content.ReadAsStringAsync();

            }
            catch (HttpRequestException he)
            {
                Debug.WriteLine(he.ToString());
                return null;
            }
            catch (Exception e)
            {
                // For debugging
                Debug.WriteLine(e.ToString());
                return null;
            }

        }

        /// <summary>
        /// 競技結果取得
        /// </summary>
        /// <param name="count">競技結果取得件数</param>
        /// <returns>競技結果文字列</returns>
        public static async Task<string> GameResult(int count)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-User-Token", UserToken);

            try
            {
                var res = await httpClient.GetAsync(new Uri(APIDomain + "/game/game_results?count=" + count.ToString()));
                return await res.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException he)
            {
                Debug.WriteLine(he.ToString());
                return null;
            }
            catch (Exception e)
            {
                // For debugging
                Debug.WriteLine(e.ToString());
                return null;
            }
        }


        /// <summary>
        /// 通知を行う学校を登録
        /// </summary>
        /// <param name="IDs">競技結果を取得する学校のID</param>
        /// <returns>実行結果</returns>
        public static async Task<string> GameNotificationSet(int[] IDs)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-User-Token", UserToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var jsonText = JsonConvert.SerializeObject(new GameNotificationIDs { ids = IDs});
            var content = new ByteArrayContent(Encoding.UTF8.GetBytes(jsonText));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json"); 
    
            try
            {
                var res = await httpClient.PutAsync(new Uri(APIDomain + "/user/me/game_notification"), content);
                return await res.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException he)
            {
                Debug.WriteLine(he.ToString());
                return null;
            }
            catch (Exception e)
            {
                // For debugging
                Debug.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// 通知を行う学校のリストを取得
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GameNotificationGet()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-User-Token", UserToken);

            try
            {
                var res = await httpClient.GetAsync(new Uri(APIDomain + "/user/me/game_notification"));
                return await res.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException he)
            {
                Debug.WriteLine(he.ToString());
                return null;
            }
            catch (Exception e)
            {
                // For debugging
                Debug.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// プッシュデバイス登録メソッド
        /// アプリケーションごとに一回だけコールすること
        /// </summary>
        /// <param name="uri">通知ハブから取得したUri</param>
        /// <returns>結果</returns>
        public static async Task<string> PushDeviceSet(string uri)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("X-User-Token", UserToken);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // シリアライズしたオブジェクトをバイト形式のcontentに変換
                var jsonText = JsonConvert.SerializeObject(
                    new DeviceTokenObject { device_type = "wp", device_token = uri });

                var content = new ByteArrayContent(Encoding.UTF8.GetBytes(jsonText));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var res = await httpClient.PutAsync(new Uri(APIDomain + "/user/me/push_token"), content);
                return await res.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException he)
            {
                Debug.WriteLine(he.ToString());
                return null;
            }
            catch (Exception e)
            {
                // For debugging
                Debug.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// 写真情報の取得
        /// </summary>
        /// <param name="count">取得枚数</param>
        /// <returns></returns>
        public static async Task<string> PhotoData(int count)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-User-Token", UserToken);

            try
            {
                var res = await httpClient.GetAsync(new Uri(APIDomain + "/game/photos?count=" + count));
                return await res.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException he)
            {
                Debug.WriteLine(he.ToString());
                return null;
            }
            catch (Exception e)
            {
                // For debugging
                Debug.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// ソーシャルフィードのアイテムを取得する
        /// </summary>
        /// <param name="count">取得個数</param>
        /// <returns></returns>
        public static async Task<string> SocialData(int count)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-User-Token", UserToken);

            try
            {
                var res = await httpClient.GetAsync(new Uri(APIDomain + "/social_feed/twitter?count=" + count));
                return await res.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException he)
            {
                Debug.WriteLine(he.ToString());
                return null;
            }
            catch (Exception e)
            {
                // For debugging
                Debug.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
