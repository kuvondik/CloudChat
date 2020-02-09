using System;
using System.Globalization;
using System.Windows;

namespace CloudChat
{
    public class NumberToVisibilityConverter : BaseValueConverter<NumberToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var number = int.Parse(value.ToString());
            return number == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}