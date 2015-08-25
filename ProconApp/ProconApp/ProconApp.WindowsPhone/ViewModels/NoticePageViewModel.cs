using Microsoft.Practices.Prism.Mvvm;
using ProconApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;

namespace ProconApp.ViewModels
{
    public class NoticePageViewModel : ViewModel
    {
        #region NoticeItemList

        private ObservableCollection<Notice.NoticeSummaryItem> noticeItemList = new ObservableCollection<Notice.NoticeSummaryItem>();
        /// <summary>
        /// お知らせ一覧に表示されるList
        /// </summary>
        public ObservableCollection<Notice.NoticeSummaryItem> NoticeItemList
        {
            get { return this.noticeItemList; }
            set { this.SetProperty(ref noticeItemList, value); }
        }

        #endregion

        #region SelectedItem

        private Notice.NoticeSummaryItem selectedItem = null;
        public Notice.NoticeSummaryItem SelectedItem
        {
            get { return this.selectedItem; }
            set { this.SetProperty(ref selectedItem, value); }
        }

        #endregion

        #region NavigateCommand

        private ICommand navigateCommand;
        /// <summary>
        /// ViewにバインドされるNavigateCommand
        /// </summary>
        public ICommand NavigateCommand
        {
            get { return this.navigateCommand ?? (this.navigateCommand = new DelegateCommand(Navigate)); }
        }

        #endregion

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            // 画面遷移してきたときに呼ばれる
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            NoticeItemList = new ObservableCollection<Notice.NoticeSummaryItem>(await Notice.getNotices(0, 20));
        }

        protected void Navigate()
        {
            // TODO: SelectedItem.Id;
        }
    }
}
