using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ProconAPI;

namespace ProconApp.Models
{
    public class ItemSelector : DataTemplateSelector
    {
        public DataTemplate SummaryTemplate { get; set; }
        public DataTemplate PhotoTemplate { get; set; }
        public DataTemplate GameResultTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if(item is Photo.PhotoItem)
                return PhotoTemplate;

            if (item is GameResult.GameReultItem)
                return GameResultTemplate;
            
            return SummaryTemplate;
        }
    }
}
