using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using KanbanBoard.Core.Enums;
using KanbanBoard.Core.Interfaces;
using KanbanBoard.Core.Models;
using KanbanBoard.Core.Services;
using KanbanBoard.WPF.Infrastructure;
using KanbanBoard.WPF.Views;

namespace KanbanBoard.WPF.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly IKanbanService _kanban;
    private readonly IStorageService _storage;
    private readonly IPomodoroService _pomodoro;
    private readonly Dispatcher _dispatcher;

    private string _currentBoardName = "";
    private Board? _currentBoard;
    private List<KanbanColumnViewModel> _columns = new();
    private List<TaskViewModel> _listTasks = new();
    private BoardStatistics _stats = new();
    private int _pomodoroCount;
    private string _timerDisplay = "";
    private string _pomodoroModeText = "Работа";
    private bool _isRunning;
    private string _currentTaskTitle = "";
    private string _searchQuery = "";
    private int _criticalCount;
    private int _highCount;
    private int _mediumCount;
    private int _lowCount;
    private Sprint? _activeSprint;
    private bool _showSprintOnly;
    private List<Sprint> _sprints = new();

    public List<string> BoardNames => _kanban.GetAllBoards().Select(b => b.Name).ToList();
    public string CurrentBoardName { get => _currentBoardName; set { _currentBoardName = value; OnPropertyChanged(); } }
    public List<KanbanColumnViewModel> Columns { get => _columns; set { _columns = value; OnPropertyChanged(); } }
    public List<TaskViewModel> ListTasks { get => _listTasks; set { _listTasks = value; OnPropertyChanged(); } }
    public BoardStatistics Stats { get => _stats; set { _stats = value; OnPropertyChanged(); } }
    public Sprint? ActiveSprint { get => _activeSprint; set { _activeSprint = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasActiveSprint)); OnPropertyChanged(nameof(SprintInfo)); OnPropertyChanged(nameof(SprintProgress)); OnPropertyChanged(nameof(SprintDaysText)); } }
    public bool HasActiveSprint => ActiveSprint != null;
    public bool ShowSprintOnly { get => _showSprintOnly; set { _showSprintOnly = value; OnPropertyChanged(); RefreshColumns(); } }
    public List<Sprint> Sprints { get => _sprints; set { _sprints = value; OnPropertyChanged(); } }
    public string SprintInfo => ActiveSprint != null ? $"Sprint: {ActiveSprint.Name}" : "Нет активного спринта";
    public double SprintProgress => ActiveSprint?.ProgressPercent ?? 0;
    public string SprintDaysText => ActiveSprint != null ? $"{ActiveSprint.DaysRemaining} дн. осталось" : "";
    public string PomodoroToday
    {
        get
        {
            if (_currentBoard == null) return _pomodoro.GetTodayStats().CompletedToday.ToString();
            var boardTaskIds = _currentBoard.AllTasks.Select(t => t.Id).ToHashSet();
            var today = DateTime.Today;
            var count = _pomodoro.GetHistory().Count(s => s.StartedAt.Date == today && s.IsCompleted && !s.IsBreak && s.TaskId.HasValue && boardTaskIds.Contains(s.TaskId.Value));
            return count.ToString();
        }
    }
    public string TimerDisplay { get => _timerDisplay; set { _timerDisplay = value; OnPropertyChanged(); } }
    public string PomodoroModeText { get => _pomodoroModeText; set { _pomodoroModeText = value; OnPropertyChanged(); } }
    public bool IsRunning { get => _isRunning; set { _isRunning = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanStart)); } }
    public bool IsPaused => _pomodoro.IsPaused;
    public bool CanStart => !IsRunning && !IsPaused;
    public string CurrentTaskTitle { get => _currentTaskTitle; set { _currentTaskTitle = value; OnPropertyChanged(); } }
    public int PomodoroCount { get => _pomodoroCount; set { _pomodoroCount = value; OnPropertyChanged(); } }
    public string SearchQuery { get => _searchQuery; set { _searchQuery = value; OnPropertyChanged(); RefreshColumns(); } }
    public int PomodoroTarget => _pomodoro.Settings.PomodorosBeforeLongBreak;
    public List<int> TodayTomatoes => Enumerable.Range(0, PomodoroCount).ToList();
    public double RemainingFraction
    {
        get
        {
            if (!_pomodoro.IsRunning && !_pomodoro.IsPaused) return 1.0;
            int totalSeconds = _pomodoro.IsBreak
                ? (_pomodoro.IsLongBreak ? _pomodoro.Settings.LongBreakMinutes * 60 : _pomodoro.Settings.ShortBreakMinutes * 60)
                : _pomodoro.Settings.WorkMinutes * 60;
            return totalSeconds > 0 ? (double)_pomodoro.RemainingSeconds / totalSeconds : 0;
        }
    }

    public int CriticalCount { get => _criticalCount; set { _criticalCount = value; OnPropertyChanged(); } }
    public int HighCount { get => _highCount; set { _highCount = value; OnPropertyChanged(); } }
    public int MediumCount { get => _mediumCount; set { _mediumCount = value; OnPropertyChanged(); } }
    public int LowCount { get => _lowCount; set { _lowCount = value; OnPropertyChanged(); } }

    public MainViewModel(IKanbanService kanban, IStorageService storage)
    {
        _kanban = kanban;
        _storage = storage;
        _pomodoro = new PomodoroService(storage);
        _dispatcher = Dispatcher.CurrentDispatcher;

        _pomodoro.Tick += (_, e) =>
        {
            _dispatcher.Invoke(() =>
            {
                int min = e.RemainingSeconds / 60;
                int sec = e.RemainingSeconds % 60;
                TimerDisplay = $"{min:D2}:{sec:D2}";
                PomodoroModeText = _pomodoro.IsBreak ? "Перерыв" : "Работа";
                IsRunning = _pomodoro.IsRunning;
                OnPropertyChanged(nameof(RemainingFraction));
                OnPropertyChanged(nameof(IsPaused));
                OnPropertyChanged(nameof(CanStart));
            });
        };
        _pomodoro.WorkCompleted += (_, _) =>
        {
            _dispatcher.Invoke(() =>
            {
                PomodoroCount++;
                OnPropertyChanged(nameof(TodayTomatoes));
                OnPropertyChanged(nameof(PomodoroToday));
                IsRunning = _pomodoro.IsRunning;
                OnPropertyChanged(nameof(IsPaused));
                OnPropertyChanged(nameof(CanStart));
                OnPropertyChanged(nameof(RemainingFraction));
                PomodoroModeText = "Работа";
                TimerDisplay = $"{_pomodoro.Settings.WorkMinutes:D2}:00";
            });
        };
        _pomodoro.BreakCompleted += (_, _) =>
        {
            _dispatcher.Invoke(() =>
            {
                OnPropertyChanged(nameof(TodayTomatoes));
                OnPropertyChanged(nameof(PomodoroToday));
                IsRunning = _pomodoro.IsRunning;
                OnPropertyChanged(nameof(IsPaused));
                OnPropertyChanged(nameof(CanStart));
                OnPropertyChanged(nameof(RemainingFraction));
                PomodoroModeText = "Работа";
                TimerDisplay = $"{_pomodoro.Settings.WorkMinutes:D2}:00";
            });
        };

        var boards = _kanban.GetAllBoards();
        if (boards.Count > 0) SelectBoard(boards[0]);
        TimerDisplay = $"{_pomodoro.Settings.WorkMinutes:D2}:00";
    }

    public void SelectBoardByName(string name)
    {
        var b = _kanban.GetAllBoards().FirstOrDefault(x => x.Name == name);
        if (b != null) SelectBoard(b);
    }

    public void SelectBoard(Board board)
    {
        _currentBoard = board;
        CurrentBoardName = board.Name;
        RefreshColumns();
        RefreshStats();
        RefreshSprints();
    }

    public void CreateNewBoard(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) name = $"Доска {DateTime.Now:HH:mm}";
        var board = _kanban.CreateBoard(name.Trim());
        _storage.SaveBoard(board);
        SelectBoard(board);
        OnPropertyChanged(nameof(BoardNames));
    }

    public void DeleteBoard(Guid boardId)
    {
        var board = _kanban.GetBoard(boardId);
        if (board == null) return;
        var result = MessageBox.Show($"Удалить доску \"{board.Name}\" и все её задачи?", "Подтверждение",
            MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result != MessageBoxResult.Yes) return;

        _storage.DeleteBoard(boardId);
        _kanban.DeleteBoard(boardId);

        if (_currentBoard?.Id == boardId)
        {
            _currentBoard = null;
            CurrentBoardName = "";
            Columns = new();
            ListTasks = new();
            Stats = new();
            var remaining = _kanban.GetAllBoards();
            if (remaining.Count > 0) SelectBoard(remaining[0]);
        }
        OnPropertyChanged(nameof(BoardNames));
    }

    public void DeleteColumn(Guid columnId)
    {
        if (_currentBoard == null) return;
        var col = _currentBoard.Columns.FirstOrDefault(c => c.Id == columnId);
        if (col == null) return;
        string taskInfo = col.Tasks.Count > 0 ? $" (содержит {col.Tasks.Count} задач)" : "";
        var result = MessageBox.Show($"Удалить колонку \"{col.Name}\"{taskInfo}?", "Подтверждение",
            MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result != MessageBoxResult.Yes) return;

        _kanban.DeleteColumn(_currentBoard.Id, columnId);
        _storage.SaveBoard(_currentBoard);
        RefreshColumns();
        RefreshStats();
    }

    public void AddNewTask()
    {
        if (_currentBoard == null) return;
        var col = _currentBoard.Columns.FirstOrDefault();
        if (col == null) return;
        AddTaskToColumn(col.Id);
    }

    public void AddTaskToColumn(Guid columnId)
    {
        if (_currentBoard == null) return;
        var dialog = new TaskDialog { Owner = Application.Current.MainWindow };
        if (dialog.ShowDialog() == true)
        {
            var priority = dialog.TaskPriority switch
            {
                "Низкий" => TaskPriority.Low,
                "Средний" => TaskPriority.Medium,
                "Высокий" => TaskPriority.High,
                "Критический" => TaskPriority.Critical,
                _ => TaskPriority.Medium
            };
            var label = dialog.TaskLabel switch
            {
                "Баг" => TaskLabel.Bug,
                "Фича" => TaskLabel.Feature,
                "Улучшение" => TaskLabel.Improvement,
                "Исследование" => TaskLabel.Research,
                "Дизайн" => TaskLabel.Design,
                _ => TaskLabel.None
            };
            var task = _kanban.AddTask(columnId, dialog.TaskTitle, dialog.TaskDescription, priority, label);
            task.DueDate = dialog.TaskDueDate;
            task.EstimatedPomodoros = dialog.TaskPomodoros;
            task.Tags = dialog.TaskTags;
            _kanban.UpdateTask(task);
            _storage.SaveBoard(_currentBoard);
            RefreshColumns();
        }
    }

    public void MoveTask(Guid taskId, Guid targetColumnId)
    {
        if (_currentBoard == null) return;
        _kanban.MoveTask(taskId, targetColumnId);
        _storage.SaveBoard(_currentBoard);
        RefreshColumns();
        RefreshStats();
    }

    public void DeleteTask(Guid taskId)
    {
        if (_currentBoard == null) return;
        _kanban.DeleteTask(taskId);
        _storage.SaveBoard(_currentBoard);
        RefreshColumns();
        RefreshStats();
    }

    public void SearchTasks(string query)
    {
        SearchQuery = query;
        if (_currentBoard == null) return;
        if (string.IsNullOrWhiteSpace(query)) { RefreshColumns(); return; }
        var found = _kanban.SearchTasks(_currentBoard.Id, query);
        ListTasks = found.Select(t => new TaskViewModel(t, GetColumnName(t))).ToList();
    }

    public void RefreshListTasks()
    {
        if (_currentBoard == null) { ListTasks = new(); return; }
        ListTasks = _currentBoard.AllTasks.Select(t => new TaskViewModel(t, GetColumnName(t))).ToList();
    }

    private string GetColumnName(KanbanTask task)
    {
        if (_currentBoard == null) return "";
        return _currentBoard.Columns.FirstOrDefault(c => c.Tasks.Any(t => t.Id == task.Id))?.Name ?? "";
    }

    public void RefreshColumns()
    {
        if (_currentBoard == null) { Columns = new(); return; }
        Columns = _currentBoard.Columns.OrderBy(c => c.Position)
            .Select(c => new KanbanColumnViewModel(c, _searchQuery, _showSprintOnly ? ActiveSprint?.Id : null)).ToList();
        OnPropertyChanged(nameof(BoardNames));
        RefreshListTasks();
    }

    public void RefreshStats()
    {
        if (_currentBoard != null) Stats = _kanban.GetStatistics(_currentBoard.Id);
        OnPropertyChanged(nameof(PomodoroToday));
        OnPropertyChanged(nameof(TodayTomatoes));
        CriticalCount = Stats.TasksByPriority.GetValueOrDefault(TaskPriority.Critical);
        HighCount = Stats.TasksByPriority.GetValueOrDefault(TaskPriority.High);
        MediumCount = Stats.TasksByPriority.GetValueOrDefault(TaskPriority.Medium);
        LowCount = Stats.TasksByPriority.GetValueOrDefault(TaskPriority.Low);
    }

    public void StartPomodoro()
    {
        if (_pomodoro.IsPaused) { ResumePomodoro(); return; }
        CurrentTaskTitle = "Общая задача";
        _pomodoro.Start();
        IsRunning = true;
        OnPropertyChanged(nameof(IsPaused));
        OnPropertyChanged(nameof(CanStart));
    }

    public void PausePomodoro() { _pomodoro.Pause(); IsRunning = false; OnPropertyChanged(nameof(IsPaused)); OnPropertyChanged(nameof(CanStart)); }
    public void ResumePomodoro() { _pomodoro.Resume(); IsRunning = true; OnPropertyChanged(nameof(IsPaused)); OnPropertyChanged(nameof(CanStart)); }
    public void StopPomodoro()
    {
        _pomodoro.Stop();
        IsRunning = false;
        TimerDisplay = $"{_pomodoro.Settings.WorkMinutes:D2}:00";
        PomodoroModeText = "Работа";
        PomodoroCount = 0;
        OnPropertyChanged(nameof(IsPaused));
        OnPropertyChanged(nameof(CanStart));
        OnPropertyChanged(nameof(RemainingFraction));
    }
    public void SkipPomodoro()
    {
        _pomodoro.Skip();
        IsRunning = _pomodoro.IsRunning;
        PomodoroModeText = _pomodoro.IsBreak ? "Перерыв" : "Работа";
        OnPropertyChanged(nameof(IsPaused));
        OnPropertyChanged(nameof(CanStart));
        OnPropertyChanged(nameof(RemainingFraction));
    }
    public void TogglePomodoro()
    {
        if (_pomodoro.IsRunning) PausePomodoro();
        else if (_pomodoro.IsPaused) ResumePomodoro();
        else StartPomodoro();
    }

    public void SavePomodoroSettings(int work, int shortBreak, int longBreak, int pomCount)
    {
        _pomodoro.Settings.WorkMinutes = work;
        _pomodoro.Settings.ShortBreakMinutes = shortBreak;
        _pomodoro.Settings.LongBreakMinutes = longBreak;
        _pomodoro.Settings.PomodorosBeforeLongBreak = pomCount;
        _storage.SaveSettings(new AppSettings { Pomodoro = _pomodoro.Settings, Theme = _storage.LoadSettings().Theme, AccentColor = _storage.LoadSettings().AccentColor });
        OnPropertyChanged(nameof(PomodoroTarget));
        TimerDisplay = $"{work:D2}:00";
    }

    public PomodoroSettings GetPomodoroSettings() => _pomodoro.Settings;

    public void SaveThemeSettings(string theme, string accentColor)
    {
        var settings = _storage.LoadSettings();
        settings.Theme = theme;
        settings.AccentColor = accentColor;
        _storage.SaveSettings(settings);
    }

    public void LoadAllListTasks()
    {
        if (_currentBoard == null) return;
        ListTasks = _currentBoard.AllTasks.Select(t => new TaskViewModel(t, GetColumnName(t))).ToList();
    }

    public void FilterByPriority(string? priority)
    {
        if (_currentBoard == null) return;
        if (string.IsNullOrEmpty(priority) || priority == "All" || priority == "Все") { RefreshListTasks(); return; }
        TaskPriority? p = priority switch
        {
            "Critical" => TaskPriority.Critical,
            "High" => TaskPriority.High,
            "Medium" => TaskPriority.Medium,
            "Low" => TaskPriority.Low,
            _ => null
        };
        if (p.HasValue)
            ListTasks = _currentBoard.AllTasks.Where(t => t.Priority == p.Value).Select(t => new TaskViewModel(t, GetColumnName(t))).ToList();
    }

    public void RefreshSprints()
    {
        try
        {
            if (_currentBoard == null) { Sprints = new(); ActiveSprint = null; return; }
            Sprints = _kanban.GetSprints(_currentBoard.Id) ?? new();
            ActiveSprint = _kanban.GetActiveSprint(_currentBoard.Id);
        }
        catch { Sprints = new(); ActiveSprint = null; }
    }

    public void CreateSprint(string name, string goal, DateTime startDate, DateTime endDate)
    {
        if (_currentBoard == null) return;
        try
        {
            _kanban.CreateSprint(_currentBoard.Id, name, goal, startDate, endDate);
            _storage.SaveBoard(_currentBoard);
            RefreshSprints();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка создания спринта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void StartSprint(Guid sprintId)
    {
        if (_currentBoard == null) return;
        _kanban.StartSprint(_currentBoard.Id, sprintId);
        _storage.SaveBoard(_currentBoard);
        RefreshSprints();
        RefreshColumns();
    }

    public void CloseSprint(Guid sprintId)
    {
        if (_currentBoard == null) return;
        _kanban.CloseSprint(_currentBoard.Id, sprintId);
        _storage.SaveBoard(_currentBoard);
        RefreshSprints();
        RefreshColumns();
    }

    public void DeleteSprint(Guid sprintId)
    {
        if (_currentBoard == null) return;
        _kanban.DeleteSprint(_currentBoard.Id, sprintId);
        _storage.SaveBoard(_currentBoard);
        RefreshSprints();
        RefreshColumns();
    }

    public void AssignTaskToSprint(Guid taskId, Guid sprintId)
    {
        if (_currentBoard == null) return;
        _kanban.AssignTaskToSprint(taskId, sprintId);
        _storage.SaveBoard(_currentBoard);
        RefreshColumns();
    }

    public void RemoveTaskFromSprint(Guid taskId)
    {
        if (_currentBoard == null) return;
        _kanban.RemoveTaskFromSprint(taskId);
        _storage.SaveBoard(_currentBoard);
        RefreshColumns();
    }
}

public class KanbanColumnViewModel : ObservableObject
{
    private readonly KanbanColumn _column;
    private readonly string _searchQuery;
    private readonly Guid? _sprintFilterId;
    public Guid Id => _column.Id;
    public string Name => _column.Name;
    public string Color => _column.Color;
    public int TaskCount => _column.Tasks.Count;
    public string WipDisplay => _column.WipLimit > 0 ? _column.WipLimit.ToString() : "∞";
    public List<TaskViewModel> Tasks
    {
        get
        {
            var tasks = _column.Tasks.Select(t => new TaskViewModel(t, _column.Name)).ToList();
            if (_sprintFilterId.HasValue)
                tasks = tasks.Where(t => t.SprintId == _sprintFilterId.Value).ToList();
            if (!string.IsNullOrWhiteSpace(_searchQuery))
            {
                var q = _searchQuery.ToLower();
                tasks = tasks.Where(t => t.Title.ToLower().Contains(q) || t.Description.ToLower().Contains(q) || t.TagChips.Any(tag => tag.ToLower().Contains(q))).ToList();
            }
            return tasks;
        }
    }

    public KanbanColumnViewModel(KanbanColumn column, string searchQuery = "", Guid? sprintFilterId = null)
    {
        _column = column;
        _searchQuery = searchQuery;
        _sprintFilterId = sprintFilterId;
    }
}

public class TaskViewModel : ObservableObject
{
    private readonly KanbanTask _task;
    public Guid Id => _task.Id;
    public string Title => _task.Title;
    public string Description => _task.Description;
    public string ColumnName { get; }
    public string Priority => _task.Priority.ToString();
    public string Label => _task.Label.ToString();
    public Guid? SprintId => _task.SprintId;
    public bool HasSprint => _task.SprintId.HasValue;

    public Brush PriorityColor => _task.Priority switch
    {
        TaskPriority.Critical => new SolidColorBrush(Color.FromRgb(244, 67, 54)),
        TaskPriority.High => new SolidColorBrush(Color.FromRgb(255, 152, 0)),
        TaskPriority.Medium => new SolidColorBrush(Color.FromRgb(33, 150, 243)),
        _ => new SolidColorBrush(Color.FromRgb(158, 158, 158))
    };

    public string LabelIcon => _task.Label switch
    {
        TaskLabel.Bug => "⚠",
        TaskLabel.Feature => "⭐",
        TaskLabel.Improvement => "↑",
        TaskLabel.Research => "🔍",
        TaskLabel.Design => "✨",
        _ => ""
    };

    public string ProgressText => _task.SubTasks.Count > 0 ? $"✓ {_task.GetProgress()}" : "";
    public string PomodoroText => _task.EstimatedPomodoros > 0 ? $"🍅 {_task.CompletedPomodoros}/{_task.EstimatedPomodoros}" : "";
    public string DeadlineText => _task.DueDate.HasValue ? _task.DueDate.Value.ToString("dd.MM") : "";
    public Brush DeadlineBrush => _task.IsOverdue ? new SolidColorBrush(Color.FromRgb(244, 67, 54)) : new SolidColorBrush(Colors.Gray);
    public List<string> TagChips => _task.Tags;
    public bool IsOverdue => _task.IsOverdue;

    public TaskViewModel(KanbanTask task, string columnName)
    {
        _task = task;
        ColumnName = columnName;
    }
}
