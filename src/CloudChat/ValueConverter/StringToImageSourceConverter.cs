using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CloudChat
{
    public class StringToImageSourceConverter : BaseValueConverter<StringToImageSourceConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = (string)value;
            if (path == null || path.EndsWith("Cache\\"))
                return Application.Current.FindResource("DefaultPersonImage") as BitmapImage;
            var bitmapImage = new BitmapImage(new Uri(path));
            return bitmapImage;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}