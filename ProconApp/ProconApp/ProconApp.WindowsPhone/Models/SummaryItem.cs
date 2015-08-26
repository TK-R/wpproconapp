using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProconApp.Models
{
    public class SummaryItem 
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 時刻
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// タイトル
        /// </summary>
        public string Title { get; set; }
    }
}
