using System.Windows;
using System.Windows.Media;

namespace KanbanBoard.WPF.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow() { InitializeComponent(); Loaded += SettingsWindow_Loaded; }

        private string _currentTheme = "Light";
        private string _currentAccent = "#2196F3";

        private void SettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.MainViewModel vm)
            {
                var s = vm.GetPomodoroSettings();
                WorkSlider.Value = s.WorkMinutes;
                ShortSlider.Value = s.ShortBreakMinutes;
                LongSlider.Value = s.LongBreakMinutes;
                PomBeforeLong.Value = s.PomodorosBeforeLongBreak;
            }
            UpdateLabels();
            _currentTheme = App.Current.Resources.MergedDictionaries
                .OfType<ResourceDictionary>()
                .Any(d => d.Source?.OriginalString.Contains("DarkTheme") == true) ? "Dark" : "Light";
            if (_currentTheme == "Dark") DarkRadio.IsChecked = true;
            else LightRadio.IsChecked = true;
        }

        private void UpdateLabels()
        {
            WorkValue.Text = $"{(int)WorkSlider.Value} мин";
            ShortValue.Text = $"{(int)ShortSlider.Value} мин";
            LongValue.Text = $"{(int)LongSlider.Value} мин";
            PomValue.Text = ((int)PomBeforeLong.Value).ToString();
        }

        private void WorkSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) { if (IsLoaded) UpdateLabels(); }
        private void ShortSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) { if (IsLoaded) UpdateLabels(); }
        private void LongSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) { if (IsLoaded) UpdateLabels(); }
        private void PomBeforeLong_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) { if (IsLoaded) UpdateLabels(); }

        private void LightTheme_Checked(object sender, RoutedEventArgs e) { _currentTheme = "Light"; App.SwitchTheme("Light"); }
        private void DarkTheme_Checked(object sender, RoutedEventArgs e) { _currentTheme = "Dark"; App.SwitchTheme("Dark"); }

        private void Color_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.Border b && b.Tag is string color)
            {
                _currentAccent = color;
                App.Current.Resources["AccentColor"] = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString(color));
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.MainViewModel vm)
            {
                vm.SavePomodoroSettings((int)WorkSlider.Value, (int)ShortSlider.Value,
                    (int)LongSlider.Value, (int)PomBeforeLong.Value);
                vm.SaveThemeSettings(_currentTheme, _currentAccent);
            }
            MessageBox.Show("Настройки сохранены", "OK", MessageBoxButton.OK);
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}
