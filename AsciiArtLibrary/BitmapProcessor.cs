using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AsciiArtLibrary
{
    public static class BitmapProcessor
    {
        public static Bitmap NewSizeBitmap(Bitmap image, int newWidth, int newHeight)
        {
            if (image.Width == newWidth && image.Height == newHeight)
            {
                return new Bitmap(image);
            }

            Bitmap bitmap = new(newWidth, newHeight);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return bitmap;
        }

        public static ImageBrush GetImageBrush(Bitmap image)
        {
            BitmapSource bs = Imaging.CreateBitmapSourceFromHBitmap(image.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(image.Width, image.Height));
            return new ImageBrush(bs);
        }

        public static List<List<System.Drawing.Color>> GetPixels(Bitmap image)
        {
            List<List<System.Drawing.Color>> pixels = new();
            for (int y = 0; y < image.Height; y++)
            {
                List<System.Drawing.Color> row = new();
                for (int x = 0; x < image.Width; x++)
                {
                    row.Add(image.GetPixel(x, y));
                }
                pixels.Add(row);
            }
            return pixels;
        }
    }
}
