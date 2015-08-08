using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ProconAPI
{
    public class APIManager
    {
        /// <summary>
        /// プロコンAPIサーバーのURL
        /// </summary>
        private static string APIDomainDev = "https://proconapp-dev.azure-mobile.net/api";

        /// <summary>
        /// 新規ユーザー登録用
        /// </summary>
        /// <returns>登録したUUID</returns>
        public async Task<string> NewUser()
        {
            /* HTTPクライアント */
            var httpClient = new HttpClient();

            try
            {
                var res = await httpClient.PostAsync(new Uri(APIDomainDev + "/auth/new_user"), null);
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
        /// ユーザー情報取得メソッド
        /// </summary>
        /// <param name="userToken">取得済みのユーザートークン</param>
        /// <returns></returns>
        public async Task<string> UserInfo(string userToken)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-User-Token", userToken);
            try
            {                
                var res = await httpClient.GetAsync(new Uri(APIDomainDev + "/user/me/info"));
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
        /// <param name="userToken">ユーザートークン</param>
        /// <param name="page">リクエストするページNo.</param>
        /// <returns>お知らせ一覧の文字列データ</returns>
        public async Task<string> NoticeList(string userToken, int page)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-User-Token", userToken);

            try
            {
                var res = await httpClient.GetAsync(new Uri(APIDomainDev + "/notices/list?page=" + page.ToString()));
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
        /// <param name="userToken">ユーザートークン</param>
        /// <param name="itemID">リクエストするアイテムID</param>
        /// <returns>お知らせ本文の文字列</returns>
        public async Task<string> NoticeItem(string userToken, int itemID)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-User-Token", userToken);

            try
            {
                var res = await httpClient.GetAsync(new Uri(APIDomainDev + "/notices/info?id="+ itemID.ToString()));
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
        /// <param name="userToken">ユーザートークン</param>
        /// <returns>出場校リストの文字列</returns>
        public async Task<string> Players(string userToken)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-User-Token", userToken);

            try
            {
                var res = await httpClient.GetAsync(new Uri(APIDomainDev + "/players"));
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
        /// <param name="userToken">ユーザートークン</param>
        /// <param name="count">競技結果取得件数</param>
        /// <returns>競技結果文字列</returns>
        public async Task<string> GameResult(string userToken, int count)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-User-Token", userToken);

            try
            {
                var res = await httpClient.GetAsync(new Uri(APIDomainDev + "/game/game_results?count=" + count.ToString()));
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
