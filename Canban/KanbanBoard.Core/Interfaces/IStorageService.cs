using KanbanBoard.Core.Models;
namespace KanbanBoard.Core.Interfaces;
public interface IStorageService {
    void SaveBoard(Board board);
    Board? LoadBoard(Guid id);
    List<Board> LoadAllBoards();
    void SaveSettings(AppSettings settings);
    AppSettings LoadSettings();
    void ExportToJson(Guid boardId, string filePath);
    Board? ImportFromJson(string filePath);
    void ExportToCsv(Guid boardId, string filePath);
    void SavePomodoroHistory(List<PomodoroSession> sessions);
    List<PomodoroSession> LoadPomodoroHistory();
    void DeleteBoard(Guid boardId);
}
