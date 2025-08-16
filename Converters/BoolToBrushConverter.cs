using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OfflineCodingBot.Converters
{
    public class BoolToBrushConverter : IValueConverter
    {
        public Brush TrueBrush { get; set; } = new SolidColorBrush(Color.FromRgb(230, 240, 255)); // user
        public Brush FalseBrush { get; set; } = Brushes.White; // bot

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = value is bool && (bool)value;
            return b ? TrueBrush : FalseBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
