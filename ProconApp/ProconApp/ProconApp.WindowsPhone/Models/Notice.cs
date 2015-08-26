using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using ProconAPI;

namespace ProconApp.Models
{
    /// <summary>
    /// お知らせ
    /// </summary>
    public class Notice
    {
        /// <summary>
        /// お知らせページに表示されるアイテム
        /// </summary>
        public class NoticeItem : SummaryItem
        {
            /// <summary>
            /// テキスト
            /// </summary>
            public string Text { set; get; }
        }

        /// <summary>
        /// お知らせ概要
        /// </summary>
        public static async Task<IEnumerable<SummaryItem>> getNotices(int page, int count = 3)
        {
            var notices = JsonConvert.DeserializeObject<List<NoticeListObject>>(await APIManager.NoticeList(page, count));

            var result = notices
                .Select(notice =>
                    new SummaryItem
                    {
                        Id = notice.id,
                        Date = DateTimeHelper.DiffTimeString(notice.published_at, DateTime.UtcNow),
                        Title = notice.title
                    }
                );
            return result;
        }
        /// <summary>
        /// お知らせ
        /// </summary>
        public static async Task<NoticeItem> getNotice(int itemID)
        {
            var notice = JsonConvert.DeserializeObject<NoticeItemObject>(await APIManager.NoticeItem(itemID));

            var result = 
                    new NoticeItem
                    {
                        Date = DateTimeHelper.DiffTimeString(notice.published_at, DateTime.UtcNow),
                        Title = notice.title,
                        Text = notice.body
                    };
            return result;
        }
    }
}
