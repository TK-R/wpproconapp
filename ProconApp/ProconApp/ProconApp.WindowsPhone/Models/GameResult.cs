using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using ProconAPI;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Mvvm;

namespace ProconApp.Models
{
    /// <summary>
    /// 競技結果
    /// </summary>
    public class GameResult
    {
        public class Result
        {
            public string Name { get; set; }
            public string Rank { get; set; }
            public string Score { get; set; }
            public string Scores { get; set; }
            public bool Advance { get; set; }

        }

        public class GameReultItem : SummaryItem
        {
            public ObservableCollection<Result> ResultList
            {
                get;
                set;
            }
        }

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

        public static async Task<IEnumerable<GameReultItem>> getGameResultItems(int count)
        {
            var gameResult = JsonConvert.DeserializeObject<IEnumerable<GameResultObject>>(await APIManager.GameResult(count));
            var resultItem = gameResult
                .Select(gr =>
                    new GameReultItem
                    {
                        Id = gr.id,
                        Title = gr.title,
                        Date = "開始: " + DateTimeHelper.FromUnixTime(gr.started_at).ToString("HH:mm") + " - " +
                               (gr.status == 1 ? "[試合中]" : "終了: " + DateTimeHelper.FromUnixTime(gr.finished_at).ToString("HH:mm")),
                        ResultList = new ObservableCollection<Result>(gr.result
                        .OrderBy(p => p.rank)
                        .Select(p =>
                            new Result
                            {
                                Name = p.player.short_name,
                                Rank = p.rank + "位",
                                Score = p.score + " zk",
                                Scores = string.Join(" ",p.scores.Select((score, index)  => "問" + (index + 1) + ":" + (score != -1 ? score.ToString() : "×"))),
                                Advance = p.advance
                            }).ToList())
                    }
                );
            return resultItem;
        }
     }
}
