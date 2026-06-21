namespace KanbanBoard.Core.Models;
public class Board {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<KanbanColumn> Columns { get; set; } = new();
    public List<Sprint> Sprints { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public List<KanbanTask> AllTasks => Columns.SelectMany(c => c.Tasks).ToList();
    public void EnsureLists() {
        Columns ??= new();
        Sprints ??= new();
        foreach (var col in Columns) col.Tasks ??= new();
    }
}
