using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProconApp.Models
{
    /// <summary>
    /// お知らせページに表示されるアイテム
    /// </summary>
    public class NoticeItem
    {
        public string Title { set; get; }
        public string Date { set; get; }
        public string Text { set; get; }
    }
}
