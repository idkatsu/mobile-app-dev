using KanbanBoard.Core.Enums;
using KanbanBoard.Core.Interfaces;
using KanbanBoard.Core.Models;
using TaskStatus = KanbanBoard.Core.Enums.TaskStatus;
namespace KanbanBoard.Core.Services;
public class KanbanService : IKanbanService {
    private readonly List<Board> _boards = new();

    public Board CreateBoard(string name, string description = "") {
        var board = new Board { Name = name, Description = description };
        board.Columns.Add(new KanbanColumn { Name = "К выполнению", Color = "#2196F3", Position = 0 });
        board.Columns.Add(new KanbanColumn { Name = "В работе", Color = "#FF9800", Position = 1 });
        board.Columns.Add(new KanbanColumn { Name = "На проверке", Color = "#9C27B0", Position = 2 });
        board.Columns.Add(new KanbanColumn { Name = "Готово", Color = "#4CAF50", Position = 3 });
        _boards.Add(board);
        return board;
    }

    public void DeleteBoard(Guid boardId) => _boards.RemoveAll(b => b.Id == boardId);
    public void RenameBoard(Guid boardId, string newName) { var b = _boards.FirstOrDefault(x => x.Id == boardId); if (b != null) b.Name = newName; }

    public KanbanColumn AddColumn(Guid boardId, string name, string color = "#2196F3", int wipLimit = 0) {
        var board = _boards.FirstOrDefault(b => b.Id == boardId);
        if (board == null) throw new InvalidOperationException("Доска не найдена");
        var col = new KanbanColumn { Name = name, Color = color, WipLimit = wipLimit, Position = board.Columns.Count };
        board.Columns.Add(col);
        return col;
    }

    public void DeleteColumn(Guid boardId, Guid columnId) { var b = _boards.FirstOrDefault(x => x.Id == boardId); b?.Columns.RemoveAll(c => c.Id == columnId); }
    public void RenameColumn(Guid columnId, string newName) { foreach (var b in _boards) { var c = b.Columns.FirstOrDefault(x => x.Id == columnId); if (c != null) { c.Name = newName; return; } } }

    public void MoveColumn(Guid boardId, Guid columnId, int newPosition) {
        var board = _boards.FirstOrDefault(b => b.Id == boardId);
        if (board == null) return;
        var col = board.Columns.FirstOrDefault(c => c.Id == columnId);
        if (col == null) return;
        board.Columns.Remove(col);
        col.Position = Math.Clamp(newPosition, 0, board.Columns.Count);
        board.Columns.Insert(col.Position, col);
        for (int i = 0; i < board.Columns.Count; i++) board.Columns[i].Position = i;
    }

    public KanbanTask AddTask(Guid columnId, string title, string description = "", TaskPriority priority = TaskPriority.Medium, TaskLabel label = TaskLabel.None) {
        foreach (var b in _boards) {
            var col = b.Columns.FirstOrDefault(c => c.Id == columnId);
            if (col != null) {
                var task = new KanbanTask { Title = title, Description = description, Priority = priority, Label = label, Status = MapColumnToStatus(col.Name) };
                col.Tasks.Add(task);
                return task;
            }
        }
        throw new InvalidOperationException("Колонка не найдена");
    }

    public void DeleteTask(Guid taskId) { foreach (var b in _boards) foreach (var c in b.Columns) c.Tasks.RemoveAll(t => t.Id == taskId); }
    public void UpdateTask(KanbanTask task) { foreach (var b in _boards) foreach (var c in b.Columns) { int i = c.Tasks.FindIndex(t => t.Id == task.Id); if (i >= 0) { c.Tasks[i] = task; return; } } }

    public void MoveTask(Guid taskId, Guid targetColumnId, int position = -1) {
        KanbanTask? task = null;
        foreach (var b in _boards) {
            bool found = false;
            foreach (var c in b.Columns) {
                var t = c.Tasks.FirstOrDefault(x => x.Id == taskId);
                if (t != null) { task = t; c.Tasks.Remove(t); found = true; break; }
            }
            if (found) break;
        }
        if (task == null) return;
        foreach (var b in _boards) {
            var col = b.Columns.FirstOrDefault(c => c.Id == targetColumnId);
            if (col != null) {
                var newStatus = MapColumnToStatus(col.Name);
                task.Status = newStatus;
                if (newStatus == TaskStatus.Done)
                    task.CompletedAt = DateTime.Now;
                else
                    task.CompletedAt = null;
                if (position < 0 || position >= col.Tasks.Count) col.Tasks.Add(task);
                else col.Tasks.Insert(position, task);
                return;
            }
        }
    }

    public void ArchiveTask(Guid taskId) { foreach (var b in _boards) foreach (var c in b.Columns) { var t = c.Tasks.FirstOrDefault(x => x.Id == taskId); if (t != null) { t.Status = TaskStatus.Archived; c.Tasks.Remove(t); var lastCol = b.Columns.LastOrDefault(); if (lastCol != null) { t.Status = TaskStatus.Archived; lastCol.Tasks.Add(t); } return; } } }
    public void RestoreTask(Guid taskId) { foreach (var b in _boards) foreach (var c in b.Columns) { var t = c.Tasks.FirstOrDefault(x => x.Id == taskId); if (t != null) { t.Status = TaskStatus.Todo; c.Tasks.Remove(t); var firstCol = b.Columns.FirstOrDefault(); if (firstCol != null) { t.Status = TaskStatus.Todo; firstCol.Tasks.Add(t); } return; } } }

    public Board? GetBoard(Guid boardId) => _boards.FirstOrDefault(b => b.Id == boardId);
    public List<Board> GetAllBoards() => new(_boards);

    public BoardStatistics GetStatistics(Guid boardId) {
        var board = _boards.FirstOrDefault(b => b.Id == boardId);
        if (board == null) return new BoardStatistics();
        var all = board.AllTasks;
        return new BoardStatistics {
            TotalTasks = all.Count,
            CompletedTasks = all.Count(t => t.Status == TaskStatus.Done),
            InProgressTasks = all.Count(t => t.Status == TaskStatus.InProgress),
            CompletionRate = all.Count > 0 ? (double)all.Count(t => t.Status == TaskStatus.Done) / all.Count * 100 : 0,
            TotalPomodorosSpent = all.Sum(t => t.CompletedPomodoros),
            TotalTimeSpent = TimeSpan.FromTicks(all.Sum(t => t.TotalTimeSpent.Ticks)),
            TasksByPriority = all.GroupBy(t => t.Priority).ToDictionary(g => g.Key, g => g.Count()),
            TasksByLabel = all.Where(t => t.Label != TaskLabel.None).GroupBy(t => t.Label).ToDictionary(g => g.Key, g => g.Count()),
            DailyProgress = Enumerable.Range(0, 7).Select(i => DateTime.Today.AddDays(-6 + i)).Select(d => (Date: d, Completed: all.Count(t => {
                if (t.Status == TaskStatus.Done && !t.CompletedAt.HasValue) return d.Date == DateTime.Today.Date;
                return t.CompletedAt.HasValue && t.CompletedAt.Value.Date == d.Date;
            }))).ToList()
        };
    }

    public List<KanbanTask> SearchTasks(Guid boardId, string query) {
        var board = _boards.FirstOrDefault(b => b.Id == boardId);
        if (board == null) return new();
        var q = query.ToLower();
        return board.AllTasks.Where(t => t.Title.ToLower().Contains(q) || t.Description.ToLower().Contains(q) || t.Tags.Any(tag => tag.ToLower().Contains(q))).ToList();
    }

    public List<KanbanTask> FilterTasks(Guid boardId, TaskPriority? priority = null, TaskLabel? label = null, DateTime? dueDateBefore = null, string? tag = null) {
        var board = _boards.FirstOrDefault(b => b.Id == boardId);
        if (board == null) return new();
        IEnumerable<KanbanTask> tasks = board.AllTasks;
        if (priority.HasValue) tasks = tasks.Where(t => t.Priority == priority.Value);
        if (label.HasValue) tasks = tasks.Where(t => t.Label == label.Value);
        if (dueDateBefore.HasValue) tasks = tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value <= dueDateBefore.Value);
        if (!string.IsNullOrEmpty(tag)) tasks = tasks.Where(t => t.Tags.Contains(tag, StringComparer.OrdinalIgnoreCase));
        return tasks.ToList();
    }

    private static TaskStatus MapColumnToStatus(string name) => name.ToLower() switch {
        "к выполнению" => TaskStatus.Todo,
        "в работе" => TaskStatus.InProgress,
        "на проверке" => TaskStatus.Review,
        "готово" => TaskStatus.Done,
        _ => TaskStatus.Todo
    };

    public Sprint CreateSprint(Guid boardId, string name, string goal, DateTime startDate, DateTime endDate) {
        var board = _boards.FirstOrDefault(b => b.Id == boardId);
        if (board == null) throw new InvalidOperationException("Доска не найдена");
        board.Sprints ??= new();
        var sprint = new Sprint { Name = name, Goal = goal, BoardId = boardId, StartDate = startDate, EndDate = endDate };
        board.Sprints.Add(sprint);
        return sprint;
    }

    public void StartSprint(Guid boardId, Guid sprintId) {
        var board = _boards.FirstOrDefault(b => b.Id == boardId);
        var sprint = board?.Sprints?.FirstOrDefault(s => s.Id == sprintId);
        if (board == null || sprint == null) return;
        var active = board.Sprints.FirstOrDefault(s => s.Status == SprintStatus.Active);
        if (active != null) active.Status = SprintStatus.Closed;
        sprint.Status = SprintStatus.Active;
        sprint.StartDate = DateTime.Now;
    }

    public void CloseSprint(Guid boardId, Guid sprintId) {
        var board = _boards.FirstOrDefault(b => b.Id == boardId);
        var sprint = board?.Sprints?.FirstOrDefault(s => s.Id == sprintId);
        if (sprint == null) return;
        sprint.Status = SprintStatus.Closed;
        sprint.EndDate = DateTime.Now;
        foreach (var task in board!.AllTasks.Where(t => t.SprintId == sprintId))
            task.SprintId = null;
    }

    public void DeleteSprint(Guid boardId, Guid sprintId) {
        var board = _boards.FirstOrDefault(b => b.Id == boardId);
        if (board == null) return;
        board.Sprints?.RemoveAll(s => s.Id == sprintId);
        foreach (var task in board.AllTasks.Where(t => t.SprintId == sprintId))
            task.SprintId = null;
    }

    public Sprint? GetActiveSprint(Guid boardId) {
        var board = _boards.FirstOrDefault(b => b.Id == boardId);
        return board?.Sprints?.FirstOrDefault(s => s.Status == SprintStatus.Active);
    }

    public List<Sprint> GetSprints(Guid boardId) {
        var board = _boards.FirstOrDefault(b => b.Id == boardId);
        return board?.Sprints?.OrderByDescending(s => s.StartDate).ToList() ?? new();
    }

    public List<KanbanTask> GetSprintTasks(Guid boardId, Guid sprintId) {
        var board = _boards.FirstOrDefault(b => b.Id == boardId);
        if (board == null) return new();
        return board.AllTasks.Where(t => t.SprintId == sprintId).ToList();
    }

    public void AssignTaskToSprint(Guid taskId, Guid sprintId) {
        foreach (var b in _boards) foreach (var c in b.Columns) {
            var t = c.Tasks.FirstOrDefault(x => x.Id == taskId);
            if (t != null) { t.SprintId = sprintId; return; }
        }
    }

    public void RemoveTaskFromSprint(Guid taskId) {
        foreach (var b in _boards) foreach (var c in b.Columns) {
            var t = c.Tasks.FirstOrDefault(x => x.Id == taskId);
            if (t != null) { t.SprintId = null; return; }
        }
    }
}
