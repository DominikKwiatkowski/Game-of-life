using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using GameOfLife.Enums;

namespace GameOfLife.Converters
{
    /// <summary>
    /// Converts Status to String.
    /// </summary>
    [ValueConversion(typeof(Status), typeof(String))]
    public class StatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Status status = (Status)value;
            return status.ToString("g");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
