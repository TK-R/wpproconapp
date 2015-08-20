using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using ProconAPI;

namespace ProconApp.Models
{
    public class Photo
    {
        public class PhotoItem
        {
            public string Title { get; set; }
            public string URL { get; set; }
            public string Date { get; set; }
        }

        public static async Task<IEnumerable<PhotoItem>> getPhotos(int count)
        {
            var photoData = JsonConvert.DeserializeObject<List<PhotoData>>(await APIManager.PhotoData(count));
            var result = photoData
                .Select(photo => new PhotoItem
                {
                    Title = photo.title,
                    URL = photo.thumbnail_url,
                    Date = ProconApp.DateTimeHelper.DiffTimeString(photo.created_at, DateTime.UtcNow)
                });
            return result;
        }
    }
}
