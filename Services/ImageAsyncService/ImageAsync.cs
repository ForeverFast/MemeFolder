using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace MemeFolder.Services.ImageAsyncService
{
    /// <summary>Реализация базового класса асинхронной загрузки
    /// изображения с Контекстом данных</summary>
    public partial class ImageAsync<T> : ImageAsyncBase<T>
    {
        public ImageAsync()
        {
            
        }
        public ImageAsync(object uri)
            : this()
        {
            //ImageDefault = new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud.Wpf;component/Images/fsc_icon.png"));
            Debug.WriteLine($"Создал ImageDefault. Поток: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            ImageUri = uri;
            Debug.WriteLine($"Отработал Ctor с uri и дефалтом. Поток: {System.Threading.Thread.CurrentThread.ManagedThreadId}" );
        }
        public ImageAsync(object uri, T content)
            : this(uri)
        {
            Content = content;
        }
        //public ImageAsync(object uri, T content, ImageSource imageDefault)
        //    : this(uri, content)
        //{
        //    ImageDefault = imageDefault;
        //}


        /// <summary>Конструктор BitmapImage.</summary>
        /// <param name="stream">Поток с источником изображения.</param>
        /// <returns>Новый экземпляр BitmapImage с загруженным изображением</returns>
        private BitmapImage ImageLoad(Stream stream)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            return bitmapImage;
        }
        /// <summary>Делегат метода создания BitmapImage.
        /// Без делегата не возможно вызвать перегрузку Dispatcher.Invoke,
        /// возвращающую результат по переданому параметру</summary>
        /// <param name="stream">Поток с изображением</param>
        /// <returns>Новый экземпляр BitmapImage с загруженным изображением</returns>
        private delegate BitmapImage ImageLoadDispatcherHandler(Stream stream);

        public override ImageSource ImageLoad(object uri)
        {
            Debug.WriteLine($"Начало ImageLoad. Поток: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            if (uri == null)
                return ImageDefault;

            try
            {
                Uri _uri;
                if (uri is string str)
                    _uri = new Uri(str);
                else
                    _uri = (Uri)uri;

                StreamResourceInfo res = Application.GetResourceStream(_uri);
                TagLibFile resStream = new TagLibFile(_uri.LocalPath, res.Stream, null);
                TagLib.File file_TAG = TagLib.File.Create(resStream);
                if (file_TAG.Tag.Pictures.Length < 1)
                    return ImageDefault;

                using var stream = new MemoryStream(file_TAG.Tag.Pictures[0].Data.Data);
                return ImageLoad(stream);

            }
            catch (Exception)
            {
                try
                {
                    TagLib.File file_TAG = TagLib.File.Create((string)uri);
                    //Debug.WriteLine($"Exception в ImageLoad. Поток: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                    if (file_TAG.Tag.Pictures.Length < 1)
                        return ImageDefault;

                    using var stream = new MemoryStream(file_TAG.Tag.Pictures[0].Data.Data);
                    return ImageLoad(stream);


                }
                catch (Exception) { }
                return ImageDefault;
            }
        }

    }
}