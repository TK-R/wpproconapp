using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ProconApp.Helper
{
    public class VisiblityConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string culture)
        {
            if (value is bool)
                if ((bool)value)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type type, object parameter, string culture)
        {
            if (value is Visibility)
                if (Visibility.Equals(value, Visibility.Visible))
                    return true;
                else
                    return false;
            else
                return false;
        }
    }

    public class BoldConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string culture)
        {
            if (value is bool)
                if ((bool)value)
                    return FontWeights.Bold;
                else
                    return FontWeights.Normal;
            else
                return FontWeights.Normal;
        }

        public object ConvertBack(object value, Type type, object parameter, string culture)
        {
            if (value is FontWeight)
                if (FontWeights.Equals(value, FontWeights.Bold))
                    return true;
                else
                    return false;
            else
                return false;
        }
    }


}
