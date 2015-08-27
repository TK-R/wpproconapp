using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ProconApp.Models
{
    public class ItemSelector : DataTemplateSelector
    {
        public DataTemplate SummaryTemplate { get; set; }
        public DataTemplate PhotoTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if(item is Photo.PhotoItem)
            {
                return PhotoTemplate;
            }
            return SummaryTemplate;
        }
    }
}
