using KanbanBoard.Core.Events;
using KanbanBoard.Core.Interfaces;
using KanbanBoard.Core.Models;
namespace KanbanBoard.Core.Services;
public class PomodoroService : IPomodoroService, IDisposable {
    private readonly IStorageService _storage;
    private System.Timers.Timer? _timer;
    private readonly List<PomodoroSession> _history = new();
    private Guid? _currentTaskId;
    private int _remainingSeconds;
    private int _totalSeconds;
    private int _pomodoroCount;
    private bool _isBreak;
    private bool _isRunning;
    private bool _isPaused;
    private bool _isLongBreak;

    public PomodoroSettings Settings { get; set; } = new();
    public bool IsRunning => _isRunning;
    public bool IsPaused => _isPaused;
    public bool IsBreak => _isBreak;
    public bool IsLongBreak => _isLongBreak;
    public int RemainingSeconds => _remainingSeconds;
    public int CurrentPomodoroNumber => _pomodoroCount;

    public event EventHandler<PomodoroTickEventArgs>? Tick;
    public event EventHandler<PomodoroSessionEventArgs>? WorkCompleted;
    public event EventHandler<PomodoroSessionEventArgs>? BreakCompleted;
    public event EventHandler? SessionStarted;
    public event EventHandler? SessionPaused;
    public event EventHandler? SessionStopped;

    public PomodoroService(IStorageService storage) {
        _storage = storage;
        Settings = _storage.LoadSettings().Pomodoro;
        _history.AddRange(_storage.LoadPomodoroHistory());
    }

    public void Start(Guid? taskId = null) {
        Stop();
        _currentTaskId = taskId;
        _pomodoroCount = 0;
        _isBreak = false;
        StartWork();
    }

    private void StartWork() {
        _pomodoroCount++;
        _isBreak = false;
        _totalSeconds = Settings.WorkMinutes * 60;
        _remainingSeconds = _totalSeconds;
        var session = new PomodoroSession { TaskId = _currentTaskId, DurationMinutes = Settings.WorkMinutes, IsBreak = false };
        _history.Add(session);
        StartTimer();
        SessionStarted?.Invoke(this, EventArgs.Empty);
    }

    private void StartBreak(bool isLong) {
        _isBreak = true;
        _isLongBreak = isLong;
        _totalSeconds = (isLong ? Settings.LongBreakMinutes : Settings.ShortBreakMinutes) * 60;
        _remainingSeconds = _totalSeconds;
        var session = new PomodoroSession { TaskId = _currentTaskId, DurationMinutes = isLong ? Settings.LongBreakMinutes : Settings.ShortBreakMinutes, IsBreak = true };
        _history.Add(session);
        StartTimer();
        SessionStarted?.Invoke(this, EventArgs.Empty);
    }

    private void StartTimer() {
        _timer?.Dispose();
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += (_, _) => OnTick();
        _isRunning = true;
        _isPaused = false;
        _timer.Start();
    }

    private void OnTick() {
        _remainingSeconds--;
        Tick?.Invoke(this, new PomodoroTickEventArgs(_remainingSeconds, _totalSeconds));
        if (_remainingSeconds <= 0) {
            _timer?.Stop();
            _isRunning = false;
            if (!_isBreak) {
                var session = _history.LastOrDefault(s => !s.IsBreak);
                if (session != null) { session.IsCompleted = true; session.EndedAt = DateTime.Now; }
                SaveHistory();
                WorkCompleted?.Invoke(this, new PomodoroSessionEventArgs(session ?? new PomodoroSession()));
                bool isLong = _pomodoroCount % Settings.PomodorosBeforeLongBreak == 0;
                if (Settings.AutoStartBreaks) StartBreak(isLong);
            } else {
                var session = _history.LastOrDefault(s => s.IsBreak);
                if (session != null) { session.IsCompleted = true; session.EndedAt = DateTime.Now; }
                SaveHistory();
                BreakCompleted?.Invoke(this, new PomodoroSessionEventArgs(session ?? new PomodoroSession()));
                if (Settings.AutoStartWork) StartWork();
            }
        }
    }

    public void Pause() { if (_isRunning && !_isPaused) { _timer?.Stop(); _isPaused = true; _isRunning = false; SessionPaused?.Invoke(this, EventArgs.Empty); } }
    public void Resume() { if (_isPaused) { _isPaused = false; _isRunning = true; _timer?.Start(); } }
    public void Stop() { _timer?.Stop(); _timer?.Dispose(); _timer = null; _isRunning = false; _isPaused = false; _remainingSeconds = 0; _pomodoroCount = 0; _isBreak = false; _isLongBreak = false; SessionStopped?.Invoke(this, EventArgs.Empty); }
    public void Skip() {
        _timer?.Stop();
        _remainingSeconds = 0;
        if (!_isBreak) {
            var session = _history.LastOrDefault(s => !s.IsBreak);
            if (session != null) { session.IsCompleted = true; session.EndedAt = DateTime.Now; }
            SaveHistory();
            WorkCompleted?.Invoke(this, new PomodoroSessionEventArgs(session ?? new PomodoroSession()));
            bool isLong = _pomodoroCount % Settings.PomodorosBeforeLongBreak == 0;
            StartBreak(isLong);
        } else {
            var session = _history.LastOrDefault(s => s.IsBreak);
            if (session != null) { session.IsCompleted = true; session.EndedAt = DateTime.Now; }
            SaveHistory();
            BreakCompleted?.Invoke(this, new PomodoroSessionEventArgs(session ?? new PomodoroSession()));
            StartWork();
        }
    }
    public List<PomodoroSession> GetHistory() => new(_history);
    private void SaveHistory() { try { _storage.SavePomodoroHistory(_history); } catch { } }
    public (int CompletedToday, int MinutesToday) GetTodayStats() {
        var today = DateTime.Today;
        var todaySessions = _history.Where(s => s.StartedAt.Date == today && s.IsCompleted && !s.IsBreak).ToList();
        return (todaySessions.Count, todaySessions.Sum(s => s.DurationMinutes));
    }
    public void Dispose() { _timer?.Dispose(); }
}
