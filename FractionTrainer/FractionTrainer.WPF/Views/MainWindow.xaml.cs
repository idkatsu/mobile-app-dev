using System.Windows;
using System.Windows.Input;
using FractionTrainer.Core.Interfaces;
using FractionTrainer.Core.Services;

namespace FractionTrainer.WPF.Views
{
    public partial class MainWindow : Window
    {
        private readonly ISettingsService _settingsService;
        private readonly IPuzzleGenerator _puzzleGenerator;

        private TrainerView? _trainerView;
        private SettingsView? _settingsView;

        public MainWindow()
        {
            InitializeComponent();

            _settingsService = new SettingsService();
            _puzzleGenerator = new PuzzleGenerator();

            ShowTrainerView();
        }

        private void ShowTrainerView()
        {
            if (_trainerView == null)
            {
                _trainerView = new TrainerView
                {
                    DataContext = new TrainerViewModel(_puzzleGenerator, _settingsService)
                };
            }
            else if (_trainerView.DataContext is TrainerViewModel vm)
            {
                vm.RefreshAccentColor();
            }

            MainContent.Content = _trainerView;
        }

        private void ShowSettingsView()
        {
            if (_settingsView == null)
            {
                _settingsView = new SettingsView
                {
                    DataContext = new SettingsViewModel(_settingsService)
                };
            }

            MainContent.Content = _settingsView;
        }

        private void ShowHelpWindow()
        {
            HelpWindow helpWindow = new HelpWindow
            {
                Owner = this
            };
            helpWindow.ShowDialog();
        }

        private void ToggleTheme()
        {
            Core.Models.AppSettings settings = _settingsService.Load();
            string newTheme = settings.Theme == "Light" ? "Dark" : "Light";
            settings.Theme = newTheme;
            _settingsService.Save(settings);
            App.SwitchTheme(newTheme);
        }

        private void NavTrainer_Click(object sender, RoutedEventArgs e)
        {
            ShowTrainerView();
        }

        private void NavSettings_Click(object sender, RoutedEventArgs e)
        {
            ShowSettingsView();
        }

        private void NavHelp_Click(object sender, RoutedEventArgs e)
        {
            ShowHelpWindow();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                ShowHelpWindow();
                e.Handled = true;
            }
            else if (e.Key == Key.T && Keyboard.Modifiers == ModifierKeys.Control)
            {
                ToggleTheme();
                e.Handled = true;
            }
            else if (e.Key == Key.N && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (MainContent.Content is TrainerView && _trainerView?.DataContext is TrainerViewModel trainerVm)
                {
                    trainerVm.ExecuteNewPuzzle();
                }
                e.Handled = true;
            }
            else if (e.Key == Key.R && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (MainContent.Content is TrainerView && _trainerView?.DataContext is TrainerViewModel trainerVm)
                {
                    trainerVm.ExecuteReset();
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                if (MainContent.Content is TrainerView && _trainerView?.DataContext is TrainerViewModel trainerVm)
                {
                    trainerVm.ExecuteReset();
                }
                e.Handled = true;
            }
        }
    }
}
