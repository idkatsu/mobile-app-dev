namespace KanbanBoard.Core.Models;
public class AppSettings {
    public string Theme { get; set; } = "Light";
    public string AccentColor { get; set; } = "#2196F3";
    public string Language { get; set; } = "ru";
    public bool ShowCompletedTasks { get; set; } = true;
    public bool EnableNotifications { get; set; } = true;
    public bool MinimizeToTray { get; set; } = true;
    public PomodoroSettings Pomodoro { get; set; } = new();
    public string? LastOpenedBoardId { get; set; }
    public bool ShowColumnWipWarning { get; set; } = true;
}
