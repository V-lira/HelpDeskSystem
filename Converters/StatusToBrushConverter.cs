using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HelpDeskSystem.Converters
{
    public class StatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value?.ToString();
            switch (status)
            {
                case "В работе":
                    return Brushes.Orange;

                case "Закрыт":
                    return Brushes.Green;

                case "Просрочен":
                    return Brushes.Red;

                case "Новый":
                    return Brushes.DodgerBlue;

                default:
                    return Brushes.Gray;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}