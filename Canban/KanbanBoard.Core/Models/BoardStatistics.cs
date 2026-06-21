using KanbanBoard.Core.Enums;
namespace KanbanBoard.Core.Models;
public class BoardStatistics {
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int InProgressTasks { get; set; }
    public double CompletionRate { get; set; }
    public int TotalPomodorosSpent { get; set; }
    public TimeSpan TotalTimeSpent { get; set; }
    public Dictionary<TaskPriority, int> TasksByPriority { get; set; } = new();
    public Dictionary<TaskLabel, int> TasksByLabel { get; set; } = new();
    public List<(DateTime Date, int Completed)> DailyProgress { get; set; } = new();
}
