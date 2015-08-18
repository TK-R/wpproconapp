using Microsoft.Practices.Prism.Mvvm;
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
    public class NotifyConfigItem : ViewModel
    {
        /// <summary>
        /// 学校名
        /// </summary>
        public string SchoolName { set; get; }

        private bool notifyFlag;        
        /// <summary>
        /// 通知設定 ON/OFF
        /// </summary>
        public bool NotifyFlag {
            set { this.SetProperty(ref notifyFlag, value); }
            get { return notifyFlag; }
        }

        /// <summary>
        /// APIから取得した学校のID
        /// </summary>
        public int ID { get; set; }
        
        
    }
}
