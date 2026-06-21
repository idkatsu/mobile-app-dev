namespace KanbanBoard.Core.Models;
public class PomodoroSettings {
    public int WorkMinutes { get; set; } = 25;
    public int ShortBreakMinutes { get; set; } = 5;
    public int LongBreakMinutes { get; set; } = 15;
    public int PomodorosBeforeLongBreak { get; set; } = 4;
    public bool AutoStartBreaks { get; set; }
    public bool AutoStartWork { get; set; }
    public bool SoundEnabled { get; set; } = true;
    public string SoundTheme { get; set; } = "Classic";
}
