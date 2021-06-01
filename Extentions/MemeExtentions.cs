
using MemeFolder.Wpf.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MemeFolder.Extentions
{
    public static class MemeExtentions
    {
        public static byte[] ConvertImageToByteArray(string fileName)
        {
            Bitmap bitMap = new Bitmap(fileName);
            Bitmap bitMapMini = new Bitmap(bitMap, new Size(120,72));
            ImageFormat bmpFormat = bitMapMini.RawFormat;

            byte[] data;
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                bitMapMini.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Position = 0;
                data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                stream.Close();
                return data;
            }

            //using (MemoryStream ms = new MemoryStream())
            //{
            //    bitMapMini.Save(ms, bmpFormat);
            //    return ms.ToArray();
            //}
        }

        public static ImageSource ConvertByteArrayToImage(byte[] array)
        {
            if (array != null)
            {
                Bitmap bmp;
                using (var ms = new MemoryStream(array))
                {
                    bmp = new Bitmap(ms);
                }
                return bmp.ToImageSource();
                //using (var ms = new MemoryStream(array, 0, array.Length))
                //{
                //    var image = new BitmapImage();
                //    image.BeginInit();
                //    image.CacheOption = BitmapCacheOption.None;
                //    //image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                //    image.StreamSource = ms;
                //    image.EndInit();       
                //    return image.ToImageSource();
                //}
            }
            return null;
        }
    }
}
