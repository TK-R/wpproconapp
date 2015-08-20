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
    /// 出場校
    /// </summary>
    public class Player
    {
        public static async Task<IEnumerable<PlayerObject>> getPlayers()
        {
            var players = JsonConvert.DeserializeObject<List<PlayerObject>>(await APIManager.Players());

            return players;
        }
    }
}
