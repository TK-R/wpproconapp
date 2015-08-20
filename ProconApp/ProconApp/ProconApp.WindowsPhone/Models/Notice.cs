using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using ProconAPI;

namespace ProconApp.Models
{
    public class Notice
    {
        public static async Task<IEnumerable<SummaryItem>> getNotices(int page)
        {
            var notices = JsonConvert.DeserializeObject<List<NoticeListObject>>(await APIManager.NoticeList(page));

            var result = notices
                .Take(3)
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
