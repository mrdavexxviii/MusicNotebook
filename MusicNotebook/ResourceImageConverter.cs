using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MusicNotebook;

[ValueConversion(typeof(string), typeof(ImageSource))]
public class ResourceImageConverter : IValueConverter
{
    [DllImport("gdi32.dll")]
    private static extern bool DeleteObject(IntPtr hObject);

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (value is not string key || string.IsNullOrWhiteSpace(key))
            return null;

        // Try to get the resource object (likely a System.Drawing.Bitmap from .resx)
        var obj = Resources.ResourceManager.GetObject(key);
        if (obj is System.Drawing.Bitmap bmp)
        {
            var hBitmap = bmp.GetHbitmap();
            try
            {
                var src = Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                src.Freeze();
                return src;
            }
            finally
            {
                DeleteObject(hBitmap);
            }
        }

        // If resource isn't a bitmap, try to interpret `key` as a pack URI path
        // e.g. "Resources/Bass_DenseTab.png" or "pack://application:,,,/MusicNotebook;component/Resources/Bass_DenseTab.png"
        //try
        //{
        //    var uri = new Uri(key, UriKind.RelativeOrAbsolute);
        //    var img = new BitmapImage(uri);
        //    img.Freeze();
        //    return img;
        //}
        //catch
        //{
        //    return null;
        //}
        return null;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
        => throw new NotSupportedException();
}