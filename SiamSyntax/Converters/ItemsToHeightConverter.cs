using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace SiamSyntax.Converters
{
    public class ItemsToHeightConverter : IValueConverter
    {
        public double RowHeight { get; set; } = 25;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection items && items.Count > 0)
            {
                double estimatedRowHeight = 25.0;

                double totalHeight = (items.Count * estimatedRowHeight) + 40;

                return totalHeight;
            }

            return double.NaN;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}