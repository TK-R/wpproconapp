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
    /// ソーシャル
    /// Twitter
    /// </summary>
    public class Social
    {
        public class SocialItem
        {
            /// <summary>
            /// ツイートテキスト
            /// </summary>
            public string Tweet { get; set; }
            /// <summary>
            /// 表示ID
            /// </summary>
            public string ScreenName { get; set; }
            /// <summary>
            /// Name
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// アイコンURL
            /// </summary>
            public string IconURL { get; set; }
            /// <summary>
            /// 時刻
            /// </summary>
            public string Date { get; set; }
        }

        public static async Task<IEnumerable<SocialItem>> getSocialItems(int count)
        {
            var socialData = JsonConvert.DeserializeObject<APISocialObject>(await APIManager.SocialData(30));

            var result = socialData.statuses
                .Select(status => new SocialItem
                {
                    Tweet = status.text,
                    Name = status.user.name,
                    ScreenName = "@" + status.user.screen_name,
                    IconURL = status.user.profile_image_url,
                    Date = DateTimeHelper.DiffTimeString(DateTimeHelper.FromTweetTime(status.created_at), DateTime.UtcNow)
                });
            return result;
        }

    }
}
