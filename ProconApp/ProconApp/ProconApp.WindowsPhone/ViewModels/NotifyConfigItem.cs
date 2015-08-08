using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProconApp.ViewModels
{
    /// <summary>
    /// 通知設定のListViewに表示するアイテム
    /// </summary>
    public class NotifyConfigItem
    {
        /// <summary>
        /// 学校名
        /// </summary>
        public string SchoolName { set; get; }

        /// <summary>
        /// 通知設定 ON/OFF
        /// </summary>
        public bool NotifyFlag { set; get; }

        /// <summary>
        /// APIから取得した学校のID
        /// </summary>
        public int ID { set; get; }
    }
}
