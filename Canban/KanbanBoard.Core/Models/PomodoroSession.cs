namespace KanbanBoard.Core.Models;
public class PomodoroSession {
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? TaskId { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.Now;
    public DateTime? EndedAt { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsBreak { get; set; }
}
