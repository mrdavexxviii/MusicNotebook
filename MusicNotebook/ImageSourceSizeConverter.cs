using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MusicNotebook;

[ValueConversion(typeof(double), typeof(Rect))]
public class ImageSourceSizeConverter : IValueConverter
{
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        var source = (double)value;
        return new Rect(0, 0, 1920, source);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        var source = (Rect)value;
        return source.Height;
    }

    #endregion
}