namespace KanbanBoard.Core.Models;
public class SubTask {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = "";
    public bool IsCompleted { get; set; }
}
