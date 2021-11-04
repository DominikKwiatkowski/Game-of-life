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
    [ValueConversion(typeof(Status), typeof(Brush))]
    public class StatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Status status = (Status)value;
            if (status == Status.Alive)
                return Brushes.Green;
            else if (status == Status.Dead)
                return Brushes.Gray;
            else if (status == Status.Born)
                return Brushes.GreenYellow;
            else if (status == Status.WillRise)
                return Brushes.LightGray;
            else if (status == Status.WillDie)
                return Brushes.Red;
            else if (status == Status.Died)
                return Brushes.Black;
            else
            {
                throw new NotImplementedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
