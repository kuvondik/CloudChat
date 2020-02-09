using System;
using System.Globalization;

namespace CloudChat
{
    public class BooleanToCornerConverter : BaseValueConverter<BooleanToCornerConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return !(bool)value ? 0 : 20;
            else
                return (bool)value ? 0 : 20;
        }
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}