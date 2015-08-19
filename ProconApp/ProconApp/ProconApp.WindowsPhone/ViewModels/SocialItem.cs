using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProconAPI;

namespace ProconApp.ViewModels
{
    public class SocialItem
    {
        public string Tweet { get; set; }
        public string ScreenName { get; set; }
        public string Name { get; set; }
        public string IconURL { get; set; }
        public string Date { get; set; }

        public SocialItem(APISocialObject.Status status)
        {
            Tweet = status.text;

            Name = status.user.name;
            ScreenName = "@" + status.user.screen_name;
            IconURL = status.user.profile_image_url;

            Date = DateTimeHelper.DiffTimeString(status.created_at, DateTime.UtcNow);
        }

    }
}
