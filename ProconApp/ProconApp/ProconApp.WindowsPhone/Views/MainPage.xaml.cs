using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.StoreApps;


namespace ProconApp.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : VisualStateAwarePage
    {
        public MainPage()
        {
            this.InitializeComponent();
            ViewModelLocator.SetAutoWireViewModel(this, true);
        }

    }
}
