using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace OfflineCodingBot.Converters
{
    public class MessageToFlowDocumentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string message = value as string ?? "";
            var doc = new FlowDocument();

            // Split message by triple backticks ```
            var parts = message.Split(new[] { "```" }, StringSplitOptions.None);
            bool isCode = false;

            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part)) continue;

                Paragraph paragraph;

                if (isCode)
                {
                    paragraph = new Paragraph(new Run(part.Trim()))
                    {
                        Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
                        Foreground = Brushes.LightGray,
                        FontFamily = new FontFamily("Consolas"),
                        Margin = new System.Windows.Thickness(0, 4, 0, 4),
                        Padding = new System.Windows.Thickness(8)
                    };
                }
                else
                {
                    paragraph = new Paragraph(new Run(part.Trim()))
                    {
                        Foreground = Brushes.Black,
                        Margin = new System.Windows.Thickness(0, 2, 0, 2)
                    };
                }

                doc.Blocks.Add(paragraph);
                isCode = !isCode;
            }

            return doc;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
