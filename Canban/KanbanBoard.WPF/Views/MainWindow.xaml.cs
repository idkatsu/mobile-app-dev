using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using KanbanBoard.Core.Interfaces;
using KanbanBoard.Core.Models;
using KanbanBoard.Core.Services;
using KanbanBoard.WPF.ViewModels;

namespace KanbanBoard.WPF.Views
{
    public partial class MainWindow : Window
    {
        private readonly IStorageService _storage = new JsonStorageService();
        private readonly IKanbanService _kanbanService;
        private MainViewModel? _vm;

        private BoardView? _boardView;
        private ListView? _listView;
        private StatisticsView? _statsView;
        private PomodoroView? _pomodoroView;
        private SettingsWindow? _settingsWindow;
        private HelpWindow? _helpWindow;
        private SprintBoardView? _sprintBoardView;

        public MainWindow()
        {
            InitializeComponent();
            _kanbanService = new KanbanService();

            foreach (var b in _storage.LoadAllBoards())
            {
                var board = _kanbanService.CreateBoard(b.Name, b.Description);
                board.Id = b.Id;
                board.Columns = b.Columns;
                board.Sprints = b.Sprints ?? new();
                board.CreatedAt = b.CreatedAt;
                _storage.SaveBoard(board);
            }

            var settings = _storage.LoadSettings();
            if (settings.Theme == "Dark") App.SwitchTheme("Dark");
            if (!string.IsNullOrEmpty(settings.AccentColor) && settings.AccentColor != "#2196F3")
            {
                App.Current.Resources["AccentColor"] = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(settings.AccentColor));
            }

            _vm = new MainViewModel(_kanbanService, _storage);
            DataContext = _vm;
            ShowBoardView();
        }

        private void ShowBoardView()
        {
            if (_boardView == null) _boardView = new BoardView { DataContext = DataContext };
            else _boardView.DataContext = DataContext;
            MainContent.Content = _boardView;
        }

        private void ShowListView()
        {
            if (_vm != null) _vm.RefreshListTasks();
            if (_listView == null) _listView = new ListView { DataContext = DataContext };
            else _listView.DataContext = DataContext;
            MainContent.Content = _listView;
        }

        private void ShowStatsView()
        {
            if (_vm != null) _vm.RefreshStats();
            if (_statsView == null) _statsView = new StatisticsView { DataContext = DataContext };
            else _statsView.DataContext = DataContext;
            MainContent.Content = _statsView;
        }

        private void ShowPomodoroView()
        {
            if (_pomodoroView == null) _pomodoroView = new PomodoroView { DataContext = DataContext };
            else _pomodoroView.DataContext = DataContext;
            MainContent.Content = _pomodoroView;
        }

        private void ShowSprintBoardView()
        {
            if (_vm != null) _vm.RefreshSprints();
            if (_sprintBoardView == null) _sprintBoardView = new SprintBoardView { DataContext = DataContext };
            else _sprintBoardView.DataContext = DataContext;
            MainContent.Content = _sprintBoardView;
        }

        private void NavBoard_Click(object s, RoutedEventArgs e) => ShowBoardView();
        private void NavList_Click(object s, RoutedEventArgs e) => ShowListView();
        private void NavStats_Click(object s, RoutedEventArgs e) => ShowStatsView();
        private void NavPomodoro_Click(object s, RoutedEventArgs e) => ShowPomodoroView();
        private void NavSprints_Click(object s, RoutedEventArgs e) => ShowSprintBoardView();

        private void NavSettings_Click(object s, RoutedEventArgs e)
        {
            _settingsWindow = new SettingsWindow { Owner = this, DataContext = DataContext };
            _settingsWindow.ShowDialog();
        }

        private void NavHelp_Click(object s, RoutedEventArgs e)
        {
            _helpWindow = new HelpWindow { Owner = this };
            _helpWindow.ShowDialog();
        }

        private void NewBoard_Click(object s, RoutedEventArgs e)
        {
            var dialog = new InputDialog("Новая доска", "Введите название доски:") { Owner = this };
            if (dialog.ShowDialog() == true)
                _vm?.CreateNewBoard(dialog.InputText);
        }

        private void NewSprint_Click(object s, RoutedEventArgs e)
        {
            var dialog = new SprintDialog { Owner = this, DataContext = DataContext };
            if (dialog.ShowDialog() == true)
                _vm?.CreateSprint(dialog.SprintName, dialog.SprintGoal, dialog.SprintStart, dialog.SprintEnd);
        }

        private void BoardItem_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is string name)
            {
                _vm?.SelectBoardByName(name);
            }
        }

        private void DeleteBoard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string name)
            {
                var board = _kanbanService.GetAllBoards().FirstOrDefault(b => b.Name == name);
                if (board != null) _vm?.DeleteBoard(board.Id);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1) { NavHelp_Click(sender, e); e.Handled = true; }
            else if (e.Key == Key.N && Keyboard.Modifiers == ModifierKeys.Control) { _vm?.AddNewTask(); e.Handled = true; }
            else if (e.Key == Key.D1 && Keyboard.Modifiers == ModifierKeys.Control) { NavBoard_Click(sender, e); e.Handled = true; }
            else if (e.Key == Key.D2 && Keyboard.Modifiers == ModifierKeys.Control) { NavList_Click(sender, e); e.Handled = true; }
            else if (e.Key == Key.D3 && Keyboard.Modifiers == ModifierKeys.Control) { NavStats_Click(sender, e); e.Handled = true; }
            else if (e.Key == Key.P) { _vm?.TogglePomodoro(); e.Handled = true; }
        }
    }
}
