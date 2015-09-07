using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProconAPI;

namespace ProconApp.Models
{
    /// <summary>
    /// 通知設定のListViewに表示するアイテム
    /// </summary>
    public class NotifyConfig
    {
        public class NotifyConfigItem
        {
            /// <summary>
            /// 学校名
            /// </summary>
            public string SchoolName { get; set; }

            /// <summary>
            /// 通知設定 ON/OFF
            /// </summary>
            public bool NotifyFlag { get; set; }

            /// <summary>
            /// APIから取得した学校のID
            /// </summary>
            public int ID { get; set; }
        }

        /// <summary>
        /// 学校リストと通知設定リストから表示用Itemを作成
        /// </summary>
        /// <param name="players"></param>
        /// <param name="notifyList"></param>
        /// <returns></returns>
        public static IEnumerable<NotifyConfigItem> getNotifyConfigItems(IEnumerable<PlayerObject> players, GameNotificationIDs notifyList)
        {
            foreach (var p in players)
            {
                // サーバ側に登録されていれば、スイッチをONにする。
                var item = new NotifyConfigItem { SchoolName = p.short_name, ID = p.id };
                item.NotifyFlag = notifyList.ids.Any(n => n == item.ID);
                yield return item;
            }
        }
        /// <summary>
        /// 初回起動時に全通知フラグON
        /// </summary>
        /// <param name="players"></param>
        /// <returns></returns>
        public static IEnumerable<NotifyConfigItem> getNotifyConfigItems(IEnumerable<PlayerObject> players)
        {
            foreach (var p in players)
            {
                var item = new NotifyConfigItem { SchoolName = p.name, ID = p.id, NotifyFlag = true};
                yield return item;
            }
        }
    }
}
