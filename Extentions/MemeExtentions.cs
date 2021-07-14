using MemeFolder.Wpf.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;

namespace MemeFolder.Extentions
{
    public static class MemeExtentions
    {
        public static byte[] ConvertImageToByteArray(string fileName)
        {
            try
            {
                Bitmap bitMap = new Bitmap(fileName);
                Bitmap bitMapMini = new Bitmap(bitMap, new System.Drawing.Size(120, 72));
                ImageFormat bmpFormat = bitMapMini.RawFormat;

                using (MemoryStream stream = new MemoryStream())
                {
                    byte[] data;
                    bitMapMini.Save(stream, ImageFormat.Bmp);
                    stream.Position = 0;
                    data = new byte[stream.Length];
                    stream.Read(data, 0, (int)stream.Length);
                    stream.Close();
                    return data;

                }
            }
            finally
            {
                GC.Collect();
            }
        }

        public static ImageSource ConvertByteArrayToImage(byte[] array)
        {
            if (array != null)
            {
                using (MemoryStream ms = new MemoryStream(array))
                {
                    return new Bitmap(ms).ToImageSource();
                }
            }
            return null;
        }

        public static void ReplaceReference<T>(IList<T> list, T newReference, Func<T, bool> predicate)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                if (predicate(list[i]))
                {
                    list[i] = newReference;
                    break;
                }
            }
        }

        public static IEnumerable<T> SelectRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> recursiveFunc)
        {
            if (source != null)
            {
                foreach (var mainItem in source)
                {
                    yield return mainItem;

                    IEnumerable<T> recursiveSequence = (recursiveFunc(mainItem) ?? new T[] { }).SelectRecursive(recursiveFunc);

                    if (recursiveSequence != null)
                    {
                        foreach (var recursiveItem in recursiveSequence)
                        {
                            yield return recursiveItem;
                        }
                    }
                }
            }
        }
    }
}
