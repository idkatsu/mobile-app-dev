using System.Windows;
using System.Windows.Controls;
using FractionTrainer.Core.Enums;

namespace FractionTrainer.WPF.Views
{
    /// <summary>
    /// Экран настроек приложения.
    /// </summary>
    public partial class SettingsView : UserControl
    {
        /// <summary>
        /// Создаёт экземпляр SettingsView.
        /// </summary>
        public SettingsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Получает текущую ViewModel.
        /// </summary>
        private SettingsViewModel ViewModel
        {
            get { return (SettingsViewModel)DataContext; }
        }

        /// <summary>
        /// Обработчик клика по кнопке цвета.
        /// </summary>
        private void ColorButton_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is string color)
            {
                ViewModel.ExecuteSelectColor(color);
            }
        }

        /// <summary>
        /// Обработчик выбора светлой темы.
        /// </summary>
        private void LightTheme_Checked(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel viewModel)
            {
                viewModel.SelectedTheme = "Light";
            }
        }

        /// <summary>
        /// Обработчик выбора тёмной темы.
        /// </summary>
        private void DarkTheme_Checked(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel viewModel)
            {
                viewModel.SelectedTheme = "Dark";
            }
        }

        /// <summary>
        /// Обработчик изменения сложности.
        /// </summary>
        private void DifficultyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is SettingsViewModel viewModel && sender is ComboBox comboBox)
            {
                switch (comboBox.SelectedIndex)
                {
                    case 0:
                        viewModel.SelectedDifficulty = DifficultyLevel.Easy;
                        break;
                    case 1:
                        viewModel.SelectedDifficulty = DifficultyLevel.Medium;
                        break;
                    case 2:
                        viewModel.SelectedDifficulty = DifficultyLevel.Hard;
                        break;
                }
            }
        }

        /// <summary>
        /// Обработчик выбора режима «Обучение».
        /// </summary>
        private void LearningMode_Checked(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel viewModel)
            {
                viewModel.SelectedMode = AppMode.Learning;
            }
        }

        /// <summary>
        /// Обработчик выбора режима «Проверка знаний».
        /// </summary>
        private void QuizMode_Checked(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel viewModel)
            {
                viewModel.SelectedMode = AppMode.Quiz;
            }
        }
    }

    /// <summary>
    /// Конвертер темы → bool для RadioButtons.
    /// </summary>
    public class ThemeToBoolConverter : System.Windows.Data.IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string theme && parameter is string param)
            {
                return theme == param;
            }
            return false;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool boolValue && boolValue && parameter is string param)
            {
                return param;
            }
            return "Light";
        }
    }
}
