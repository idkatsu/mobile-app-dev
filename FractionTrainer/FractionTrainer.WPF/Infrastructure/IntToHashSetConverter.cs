using System.Globalization;
using System.Windows.Data;

namespace FractionTrainer.WPF.Infrastructure
{
    public class IntToHashSetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count)
            {
                var set = new HashSet<int>();
                for (int i = 0; i < count; i++)
                    set.Add(i);
                return set;
            }
            return new HashSet<int>();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HashSet<int> set)
                return set.Count;
            return 0;
        }
    }
}
