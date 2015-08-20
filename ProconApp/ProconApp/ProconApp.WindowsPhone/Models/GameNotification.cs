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
    /// 通知設定
    /// </summary>
    public class GameNotification
    {
        public static async Task<GameNotificationIDs> getGameNotification()
        {
            var notifyList = JsonConvert.DeserializeObject<GameNotificationIDs>(await APIManager.GameNotificationGet());

            return notifyList;
        }
    }
}
