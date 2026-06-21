using System.Globalization;
using System.Windows.Data;

namespace FractionTrainer.WPF.Infrastructure
{
    public class AccentMatchConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is string selected && values[1] is string current)
            {
                return string.Equals(selected, current, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
