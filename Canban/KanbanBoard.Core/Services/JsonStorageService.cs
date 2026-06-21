using System.Text.Json;
using KanbanBoard.Core.Interfaces;
using KanbanBoard.Core.Models;
namespace KanbanBoard.Core.Services;
public class JsonStorageService : IStorageService {
    private readonly string _dataDir;
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public JsonStorageService() {
        _dataDir = Environment.OSVersion.Platform == PlatformID.Win32NT
            ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KanbanBoard")
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".kanbanboard");
        Directory.CreateDirectory(_dataDir);
    }

    private string BoardPath(Guid id) => Path.Combine(_dataDir, $"board_{id}.json");
    private string SettingsPath() => Path.Combine(_dataDir, "settings.json");
    private string PomodoroHistoryPath() => Path.Combine(_dataDir, "pomodoro_history.json");

    public void SaveBoard(Board board) {
        var json = JsonSerializer.Serialize(board, JsonOptions);
        File.WriteAllText(BoardPath(board.Id), json);
    }

    public Board? LoadBoard(Guid id) {
        var path = BoardPath(id);
        if (!File.Exists(path)) return null;
        var board = JsonSerializer.Deserialize<Board>(File.ReadAllText(path), JsonOptions);
        board?.EnsureLists();
        return board;
    }

    public List<Board> LoadAllBoards() {
        var boards = new List<Board>();
        foreach (var file in Directory.GetFiles(_dataDir, "board_*.json")) {
            try {
                var board = JsonSerializer.Deserialize<Board>(File.ReadAllText(file), JsonOptions);
                if (board != null) { board.EnsureLists(); boards.Add(board); }
            } catch { }
        }
        return boards;
    }

    public void SaveSettings(AppSettings settings) {
        var json = JsonSerializer.Serialize(settings, JsonOptions);
        File.WriteAllText(SettingsPath(), json);
    }

    public AppSettings LoadSettings() {
        var path = SettingsPath();
        if (!File.Exists(path)) return new AppSettings();
        return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(path), JsonOptions) ?? new AppSettings();
    }

    public void ExportToJson(Guid boardId, string filePath) {
        var board = LoadBoard(boardId);
        if (board != null) File.WriteAllText(filePath, JsonSerializer.Serialize(board, JsonOptions));
    }

    public Board? ImportFromJson(string filePath) {
        if (!File.Exists(filePath)) return null;
        var board = JsonSerializer.Deserialize<Board>(File.ReadAllText(filePath), JsonOptions);
        if (board != null) { board.EnsureLists(); SaveBoard(board); }
        return board;
    }

    public void ExportToCsv(Guid boardId, string filePath) {
        var board = LoadBoard(boardId);
        if (board == null) return;
        using var writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
        writer.WriteLine("Заголовок;Приоритет;Статус;Метка;Дедлайн;Помидоры;Теги");
        foreach (var task in board.AllTasks) {
            writer.WriteLine($"{task.Title};{task.Priority};{task.Status};{task.Label};{task.DueDate:yyyy-MM-dd};{task.CompletedPomodoros}/{task.EstimatedPomodoros};{string.Join(",", task.Tags)}");
        }
    }

    public void SavePomodoroHistory(List<PomodoroSession> sessions) {
        var json = JsonSerializer.Serialize(sessions, JsonOptions);
        File.WriteAllText(PomodoroHistoryPath(), json);
    }

    public List<PomodoroSession> LoadPomodoroHistory() {
        var path = PomodoroHistoryPath();
        if (!File.Exists(path)) return new List<PomodoroSession>();
        return JsonSerializer.Deserialize<List<PomodoroSession>>(File.ReadAllText(path), JsonOptions) ?? new List<PomodoroSession>();
    }

    public void DeleteBoard(Guid boardId) {
        var path = BoardPath(boardId);
        if (File.Exists(path)) File.Delete(path);
    }
}
