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
    /// お知らせ概要
    /// </summary>
    public class SummaryNotice
    {
        public static async Task<IEnumerable<SummaryItem>> getNotices(int page, int num)
        {
            var notices = JsonConvert.DeserializeObject<List<NoticeListObject>>(await APIManager.NoticeList(page));

            var result = notices
                .Take(num)
                .Select(notice =>
                    new SummaryItem
                    {
                        Date = DateTimeHelper.DiffTimeString(notice.published_at, DateTime.UtcNow),
                        Title = notice.title
                    }
                );
            return result;
        }
    }
}
