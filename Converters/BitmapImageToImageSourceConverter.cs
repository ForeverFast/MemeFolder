using MemeFolder.Wpf.Helpers;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MemeFolder.Converters
{
    public class BitmapImageToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Bitmap ico)
            {
                ImageSource img = ico.ToImageSource();
                //ico.Freeze();
                return img;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
