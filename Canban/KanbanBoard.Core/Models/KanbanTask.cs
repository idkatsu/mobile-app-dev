using KanbanBoard.Core.Enums;
using TaskStatus = KanbanBoard.Core.Enums.TaskStatus;
namespace KanbanBoard.Core.Models;
public class KanbanTask {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public TaskStatus Status { get; set; } = TaskStatus.Todo;
    public TaskLabel Label { get; set; } = TaskLabel.None;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int EstimatedPomodoros { get; set; }
    public int CompletedPomodoros { get; set; }
    public Guid? SprintId { get; set; }
    public List<string> Tags { get; set; } = new();
    public List<SubTask> SubTasks { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public TimeSpan TotalTimeSpent { get; set; }
    public string GetProgress() { int total = SubTasks.Count; int done = SubTasks.Count(s => s.IsCompleted); return $"{done}/{total}"; }
    public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.Now && Status != Enums.TaskStatus.Done && Status != Enums.TaskStatus.Archived;
}
