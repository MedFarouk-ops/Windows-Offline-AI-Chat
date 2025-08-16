using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;
using Markdown.Xaml;

namespace OfflineCodingBot.Converters
{
    public class MarkdownToFlowDocumentConverter : IValueConverter
    {
        private readonly Markdown.Xaml.Markdown _markdown = new Markdown.Xaml.Markdown();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            string markdownText = value.ToString();

            return _markdown.Transform(markdownText);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
