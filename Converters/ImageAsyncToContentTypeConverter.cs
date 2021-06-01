using MemeFolder.Domain.Models.AbstractModels;
using MemeFolder.Services.ImageAsyncService;
using System;
using System.Globalization;
using System.Windows.Data;

namespace MemeFolder.Converters
{
    public class ImageAsyncToContentTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
          => (value as ImageAsync<FolderObject>).Content.GetType();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
