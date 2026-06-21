using KanbanBoard.Core.Events;
using KanbanBoard.Core.Models;
namespace KanbanBoard.Core.Interfaces;
public interface IPomodoroService {
    event EventHandler<PomodoroTickEventArgs> Tick;
    event EventHandler<PomodoroSessionEventArgs> WorkCompleted;
    event EventHandler<PomodoroSessionEventArgs> BreakCompleted;
    event EventHandler SessionStarted;
    event EventHandler SessionPaused;
    event EventHandler SessionStopped;
    void Start(Guid? taskId = null);
    void Pause();
    void Resume();
    void Stop();
    void Skip();
    PomodoroSettings Settings { get; set; }
    List<PomodoroSession> GetHistory();
    (int CompletedToday, int MinutesToday) GetTodayStats();
    bool IsRunning { get; }
    bool IsPaused { get; }
    bool IsBreak { get; }
    bool IsLongBreak { get; }
    int RemainingSeconds { get; }
    int CurrentPomodoroNumber { get; }
}
