namespace KanbanBoard.Core.Models;
public class KanbanColumn {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public int WipLimit { get; set; }
    public List<KanbanTask> Tasks { get; set; } = new();
    public string Color { get; set; } = "#2196F3";
    public int Position { get; set; }
    public bool IsWipExceeded => WipLimit > 0 && Tasks.Count > WipLimit;
}
