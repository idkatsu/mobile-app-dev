using System.Globalization;
using System.Windows.Data;

namespace KanbanBoard.WPF.Infrastructure
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is bool b && b ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is System.Windows.Visibility v && v == System.Windows.Visibility.Visible;
    }

    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is bool b ? !b : value!;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is bool b ? !b : value!;
    }

    public class PriorityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is Core.Enums.TaskPriority p ? p switch {
                Core.Enums.TaskPriority.Critical => new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(244, 67, 54)),
                Core.Enums.TaskPriority.High => new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 152, 0)),
                Core.Enums.TaskPriority.Medium => new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(33, 150, 243)),
                _ => new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(158, 158, 158))
            } : new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value!;
    }

    public class LabelToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is Core.Enums.TaskLabel l ? l switch {
                Core.Enums.TaskLabel.Bug => "\u26A0",
                Core.Enums.TaskLabel.Feature => "\u2B50",
                Core.Enums.TaskLabel.Improvement => "\u2191",
                Core.Enums.TaskLabel.Research => "\uD83D\uDD0D",
                Core.Enums.TaskLabel.Design => "\u2728",
                _ => ""
            } : "";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value!;
    }

    public class SprintStatusToStartVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is Core.Enums.SprintStatus s && s == Core.Enums.SprintStatus.Planning
                ? System.Windows.Visibility.Visible
                : System.Windows.Visibility.Collapsed;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value!;
    }
}
