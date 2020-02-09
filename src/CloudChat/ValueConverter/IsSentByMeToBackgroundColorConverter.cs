using System;
using System.Globalization;
using System.Windows;

namespace CloudChat
{
    public class IsSentByMeBackgroundColor : BaseValueConverter<IsSentByMeBackgroundColor>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return (bool)value ? Application.Current.FindResource("ChatBubbleBackgroundBrush") : Application.Current.FindResource("ForegroundLightBrush");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}