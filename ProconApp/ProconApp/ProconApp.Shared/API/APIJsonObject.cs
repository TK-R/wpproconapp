using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProconAPI
{
    /// <summary>
    /// 新規ユーザーデータ
    /// </summary>
    public class NewUserResponseJson
    {
        public int user_id { get; set; }
        public string user_token { get; set; }
        public object twitter_id { get; set; }
        public object facebook_id { get; set; }

        public override string ToString()
        {
            return "User ID:" + user_id + " Token:" + user_token; 
        }
    }

    /// <summary>
    /// お知らせ一覧
    /// </summary>
    public class NoticeListObject
    {
        public int id { get; set; }
        public string title { get; set; }
        public int published_at { get; set; }
        public int body_size { get; set; }

        public override string ToString()
        {
            return "ID:" + id + " Title:" + title + " Published:" + published_at + " BodySize:" + body_size;
        } 
    }

    /// <summary>
    /// お知らせ本文
    /// </summary>
    public class NoticeItemObject
    {
        public int id { get; set; }
        public string title { get; set; }
        public int published_at { get; set; }
        public string body { get; set; }
        public int body_size { get; set; }
   
        public override string ToString()
        {
            return "ID:" + id + " Title:" + title + " Published:" + published_at + " Body:" + body + " BodySize:" + body_size;
        }
    }

    /// <summary>
    /// 競技結果オブジェクト
    /// </summary>
    public class GameResultObject
    {

        public class Result
        {
            public PlayerObject player { get; set; }
            public int score { get; set; }
            public int rank { get; set; }
        }

        public int id { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public List<Result> result { get; set; }
        public int started_at { get; set; }
        public int finished_at { get; set; }

        public override string ToString()
        {
            return "ID:" + id + " Title:" + title + " Status:" + status + " Start:" + started_at + " Finish:" + finished_at;  
        }
    }


    public class PlayerObject
    {
        public int id { get; set; }
        public string name { get; set; }
        public string short_name { get; set; }

        public override string ToString()
        {
            return "ID:" + id + " Name:" + name + " Short Name:" + short_name;
        }
    }

    /// <summary>
    /// 通知設定の反映/取得に用いる
    /// </summary>
    public class GameNotificationIDs
    {
        public int[] ids { get; set; }
    }

    /// <summary>
    /// デバイス情報の登録に用いる
    /// </summary>
    public class DeviceTokenObject
    {
        public string device_type { get; set; }
        public string device_token { get; set; }
    }

    /// <summary>
    /// 写真情報の取得
    /// </summary>
    public class PhotoData
    {
        public int id { get; set; }
        /// <summary>
        /// 写真のタイトル
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// オリジナル画像のURL
        /// </summary>
        public string original_url { get; set; }
        /// <summary>
        /// サムネイル画像のURL
        /// </summary>
        public string thumbnail_url { get; set; }
        /// <summary>
        /// 作成日時(UNIX Timestamp)
        /// </summary>
        public int created_at { get; set; }
    }
}



