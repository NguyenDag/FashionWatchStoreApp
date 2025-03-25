using System;
using System.Globalization;
using System.Windows.Data;

namespace ProjectPRN
{
    public class RatingToStarsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int rating)
            {
                return new string('★', rating) + new string('☆', 5 - rating); // Hiển thị số sao
            }
            return "☆☆☆☆☆";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // Không cần ConvertBack
        }
    }
}
