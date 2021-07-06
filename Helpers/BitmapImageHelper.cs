using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MemeFolder.Wpf.Helpers
{
    public static class BitmapImageHelper
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObjetc);

        public static ImageSource ToImageSource(this Bitmap bitmap)
        {
           
            using (MemoryStream outStream = new MemoryStream())
            {
                IntPtr hBitmap = bitmap.GetHbitmap();

                ImageSource image =
                    Imaging.CreateBitmapSourceFromHBitmap(
                        hBitmap,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());

                if (!DeleteObject(hBitmap))
                {
                    MessageBox.Show("Failed to delete unused bitmap object");
                }
                return image;
            }
            
        }
    }
}
