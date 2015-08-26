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
    /// 画像
    /// </summary>
    public class Photo
    {
        public class PhotoItem : SummaryItem
        {
            /// <summary>
            /// URL
            /// </summary>
            public string URL { get; set; }
            /// <summary>
            /// オリジナルURL
            /// </summary>
            public string OriginalURL { get; set; }
        }

        /// <summary>
        /// ホームで使用するサムネイルデータの取得
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<PhotoItem>> getPhotos(int count)
        {
            var photoData = JsonConvert.DeserializeObject<List<PhotoData>>(await APIManager.PhotoData(count));
            var result = photoData
                .Select(photo => new PhotoItem
                {
                    Id = photo.id,
                    Title = photo.title,
                    URL = photo.thumbnail_url,
                    OriginalURL = photo.original_url,
                    Date = ProconApp.DateTimeHelper.DiffTimeString(photo.created_at, DateTime.UtcNow)
                });
            return result;
        }
    }
}
