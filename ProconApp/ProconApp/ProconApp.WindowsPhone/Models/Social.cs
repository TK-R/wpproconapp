using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using ProconAPI;

namespace ProconApp.Models
{
    public class Social
    {
        public class SocialItem
        {
            public string Tweet { get; set; }
            public string ScreenName { get; set; }
            public string Name { get; set; }
            public string IconURL { get; set; }
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
