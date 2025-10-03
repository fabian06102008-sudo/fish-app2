using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace FishApp.Converters;

/// <summary>
/// Converts a null value to false and any other value to true.
/// </summary>
public class NullToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var isNotNull = value is not null;
        if (parameter is string text && text.Equals("invert", StringComparison.OrdinalIgnoreCase))
        {
            return !isNotNull;
        }

        return isNotNull;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
