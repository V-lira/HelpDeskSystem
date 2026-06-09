using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HelpDeskSystem.Converters
{
    public class PriorityToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string priority = value?.ToString();
            switch (priority)
            {
                case "Критический":
                    return Brushes.Red;

                case "Высокий":
                    return Brushes.DarkOrange;

                case "Средний":
                    return Brushes.DodgerBlue;

                case "Низкий":
                    return Brushes.Gray;

                default:
                    return Brushes.LightGray;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}