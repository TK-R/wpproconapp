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
        /// <summary>
        /// タイトル
        /// </summary>
        public string Title { set; get; }
        /// <summary>
        /// 時刻
        /// </summary>
        public string Date { set; get; }
        /// <summary>
        /// テキスト
        /// </summary>
        public string Text { set; get; }
    }
}
