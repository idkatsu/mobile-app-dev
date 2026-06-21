namespace KanbanBoard.Core.Models;

public class Sprint
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public Guid BoardId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Enums.SprintStatus Status { get; set; } = Enums.SprintStatus.Planning;
    public string Goal { get; set; } = "";

    public int TotalDays => Math.Max(1, (EndDate - StartDate).Days + 1);
    public int DaysElapsed => Math.Max(0, Math.Min(TotalDays, (DateTime.Now - StartDate).Days + 1));
    public int DaysRemaining => Math.Max(0, TotalDays - DaysElapsed);
    public double ProgressPercent => TotalDays > 0 ? Math.Min(100, DaysElapsed * 100.0 / TotalDays) : 0;
    public bool IsActive => Status == Enums.SprintStatus.Active;
    public bool IsOverdue => Status == Enums.SprintStatus.Active && DateTime.Now > EndDate;
}
