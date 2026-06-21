using KanbanBoard.Core.Enums;
using KanbanBoard.Core.Models;
namespace KanbanBoard.Core.Interfaces;
public interface IKanbanService {
    Board CreateBoard(string name, string description = "");
    void DeleteBoard(Guid boardId);
    void RenameBoard(Guid boardId, string newName);
    KanbanColumn AddColumn(Guid boardId, string name, string color = "#2196F3", int wipLimit = 0);
    void DeleteColumn(Guid boardId, Guid columnId);
    void RenameColumn(Guid columnId, string newName);
    void MoveColumn(Guid boardId, Guid columnId, int newPosition);
    KanbanTask AddTask(Guid columnId, string title, string description = "", TaskPriority priority = TaskPriority.Medium, TaskLabel label = TaskLabel.None);
    void DeleteTask(Guid taskId);
    void UpdateTask(KanbanTask task);
    void MoveTask(Guid taskId, Guid targetColumnId, int position = -1);
    void ArchiveTask(Guid taskId);
    void RestoreTask(Guid taskId);
    Board? GetBoard(Guid boardId);
    List<Board> GetAllBoards();
    BoardStatistics GetStatistics(Guid boardId);
    List<KanbanTask> SearchTasks(Guid boardId, string query);
    List<KanbanTask> FilterTasks(Guid boardId, TaskPriority? priority = null, TaskLabel? label = null, DateTime? dueDateBefore = null, string? tag = null);

    Sprint CreateSprint(Guid boardId, string name, string goal, DateTime startDate, DateTime endDate);
    void StartSprint(Guid boardId, Guid sprintId);
    void CloseSprint(Guid boardId, Guid sprintId);
    void DeleteSprint(Guid boardId, Guid sprintId);
    Sprint? GetActiveSprint(Guid boardId);
    List<Sprint> GetSprints(Guid boardId);
    List<KanbanTask> GetSprintTasks(Guid boardId, Guid sprintId);
    void AssignTaskToSprint(Guid taskId, Guid sprintId);
    void RemoveTaskFromSprint(Guid taskId);
}
