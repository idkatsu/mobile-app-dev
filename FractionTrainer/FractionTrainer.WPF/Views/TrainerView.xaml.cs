using System.Windows;
using System.Windows.Controls;
using FractionTrainer.Core.Enums;

namespace FractionTrainer.WPF.Views
{
    public partial class TrainerView : UserControl
    {
        public TrainerView()
        {
            InitializeComponent();
        }

        private TrainerViewModel ViewModel
        {
            get { return (TrainerViewModel)DataContext; }
        }

        private void CircleControl_SectorClicked(object sender, RoutedEventArgs e)
        {
            if (sender is FractionCircleControl sectorControl &&
                e is FractionCircleControl.SectorClickedRoutedEventArgs sectorArgs)
            {
                ViewModel.ExecuteToggleSector(sectorArgs.SectorIndex);
            }
        }

        private void LearningMode_Checked(object sender, RoutedEventArgs e)
        {
            if (DataContext is TrainerViewModel viewModel)
            {
                viewModel.SelectedMode = AppMode.Learning;
            }
        }

        private void QuizMode_Checked(object sender, RoutedEventArgs e)
        {
            if (DataContext is TrainerViewModel viewModel)
            {
                viewModel.SelectedMode = AppMode.Quiz;
            }
        }

        private void DifficultyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is TrainerViewModel viewModel && sender is ComboBox comboBox)
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

        private void CompareOption_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb && DataContext is TrainerViewModel viewModel)
            {
                string? content = rb.Content as string;
                if (content != null)
                {
                    int index = viewModel.CompareOptions.IndexOf(content);
                    if (index >= 0)
                    {
                        viewModel.SelectedCompareIndex = index;
                    }
                }
            }
        }

        private void EquivalentOption_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb && DataContext is TrainerViewModel viewModel)
            {
                string? content = rb.Content as string;
                if (content != null)
                {
                    int index = viewModel.EquivalentOptions.IndexOf(content);
                    if (index >= 0)
                    {
                        viewModel.SelectedEquivalentIndex = index;
                    }
                }
            }
        }

        private void PairCard_MouseLeftDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is PairCardViewModel cardVm &&
                DataContext is TrainerViewModel viewModel)
            {
                int index = viewModel.PairCards.IndexOf(cardVm);
                if (index >= 0)
                {
                    viewModel.ExecuteSelectPairCard(index);
                }
            }
        }
    }

    public class BoolToVisibilityConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }
    }

    public class BoolToDoubleConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? 1.0 : 0.0;
            }
            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                return doubleValue > 0.5;
            }
            return false;
        }
    }

    public class AppModeToBoolConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is AppMode mode && parameter is string param)
            {
                return mode.ToString() == param;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool boolValue && boolValue && parameter is string param)
            {
                if (Enum.TryParse<AppMode>(param, out AppMode mode))
                {
                    return mode;
                }
            }
            return AppMode.Learning;
        }
    }
}
