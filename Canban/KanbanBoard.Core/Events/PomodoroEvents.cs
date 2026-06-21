using KanbanBoard.Core.Models;
namespace KanbanBoard.Core.Events;
public class PomodoroTickEventArgs : EventArgs {
    public int RemainingSeconds { get; }
    public int TotalSeconds { get; }
    public PomodoroTickEventArgs(int remaining, int total) { RemainingSeconds = remaining; TotalSeconds = total; }
}
public class PomodoroSessionEventArgs : EventArgs {
    public PomodoroSession Session { get; }
    public PomodoroSessionEventArgs(PomodoroSession session) { Session = session; }
}
