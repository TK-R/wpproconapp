﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using ProconAPI;

namespace ProconApp.Models
{
    /// <summary>
    /// 競技結果
    /// </summary>
    public class GameResult
    {
        public static async Task<IEnumerable<SummaryItem>> getGameResults(int count)
        {
            var gameResult = JsonConvert.DeserializeObject<List<GameResultObject>>(await APIManager.GameResult(count));

            var result = gameResult
                .Select(gr =>
                    new SummaryItem
                    {
                        Id = gr.id,
                        Date = DateTimeHelper.DiffTimeString(gr.finished_at, DateTime.UtcNow),
                        Title = gr.title
                    }
                );
            return result;
        }
    }
}
