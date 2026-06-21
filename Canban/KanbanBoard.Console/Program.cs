using KanbanBoard.Core.Enums;
using KanbanBoard.Core.Interfaces;
using KanbanBoard.Core.Services;

namespace KanbanBoard.Cli;

public class Program
{
    private static IKanbanService _kanban = new KanbanService();
    private static IStorageService _storage = new JsonStorageService();
    private static Core.Models.Board? _selectedBoard;

    public static void Main(string[] args)
    {
        var boards = _storage.LoadAllBoards();
        foreach (var b in boards) {
            var board = _kanban.CreateBoard(b.Name, b.Description);
            board.Id = b.Id;
            board.Columns = b.Columns;
            board.CreatedAt = b.CreatedAt;
        }

        if (args.Length == 0) { ShowHelp(); return; }

        string cmd = args[0].ToLower();
        switch (cmd)
        {
            case "boards": ListBoards(); break;
            case "board": HandleBoard(args); break;
            case "tasks": ListTasks(); break;
            case "task": HandleTask(args); break;
            case "stats": ShowStats(); break;
            case "pomodoro": HandlePomodoro(args); break;
            case "export": HandleExport(args); break;
            case "import": HandleImport(args); break;
            case "help": ShowHelp(); break;
            default: Console.WriteLine($"Неизвестная команда: {cmd}"); ShowHelp(); break;
        }
    }

    private static void ListBoards()
    {
        var boards = _kanban.GetAllBoards();
        if (boards.Count == 0) { Console.WriteLine("Нет досок. Создайте: kanban board create \"Название\""); return; }
        Console.WriteLine("=== Доски ===");
        foreach (var b in boards)
        {
            int tasks = b.AllTasks.Count;
            Console.WriteLine($"  [{b.Id.ToString()[..8]}] {b.Name} ({tasks} задач)");
        }
    }

    private static void HandleBoard(string[] args)
    {
        if (args.Length < 2) { Console.WriteLine("Использование: kanban board [create|select] ..."); return; }
        switch (args[1].ToLower())
        {
            case "create":
                if (args.Length < 3) { Console.WriteLine("Укажите название: kanban board create \"Название\""); return; }
                var name = string.Join(" ", args.Skip(2)).Trim('"');
                var board = _kanban.CreateBoard(name);
                _storage.SaveBoard(board);
                _selectedBoard = board;
                Console.WriteLine($"Доска создана: {name} [{board.Id.ToString()[..8]}]");
                break;
            case "select":
                if (args.Length < 3) { Console.WriteLine("Укажите ID: kanban board select <id>"); return; }
                var id = args[2];
                _selectedBoard = _kanban.GetAllBoards().FirstOrDefault(b => b.Id.ToString().StartsWith(id, StringComparison.OrdinalIgnoreCase));
                if (_selectedBoard != null) Console.WriteLine($"Выбрана доска: {_selectedBoard.Name}");
                else Console.WriteLine("Доска не найдена");
                break;
        }
    }

    private static void ListTasks()
    {
        EnsureBoard();
        Console.WriteLine($"=== Задачи: {_selectedBoard!.Name} ===");
        foreach (var col in _selectedBoard!.Columns.OrderBy(c => c.Position))
        {
            Console.WriteLine($"\n  [{col.Name}] ({col.Tasks.Count} задач):");
            foreach (var t in col.Tasks)
            {
                string due = t.DueDate.HasValue ? $" | дедлайн: {t.DueDate:dd.MM}" : "";
                string pom = t.EstimatedPomodoros > 0 ? $" | 🍅{t.CompletedPomodoros}/{t.EstimatedPomodoros}" : "";
                Console.WriteLine($"    {t.Priority.ToString()[..1]} | {t.Title}{due}{pom}");
            }
        }
    }

    private static void HandleTask(string[] args)
    {
        if (args.Length < 2) { Console.WriteLine("Использование: kanban task [add|move|done|delete] ..."); return; }
        switch (args[1].ToLower())
        {
            case "add":
                EnsureBoard();
                if (args.Length < 3) { Console.WriteLine("Укажите заголовок"); return; }
                string title = args[2].Trim('"');
                var priority = TaskPriority.Medium;
                Guid? colId = _selectedBoard!.Columns.FirstOrDefault()?.Id;
                DateTime? due = null;
                for (int i = 3; i < args.Length; i++)
                {
                    if (args[i] == "--priority" && i + 1 < args.Length) { priority = args[++i].ToLower() switch { "low" => TaskPriority.Low, "high" => TaskPriority.High, "critical" => TaskPriority.Critical, _ => TaskPriority.Medium }; }
                    if (args[i] == "--column" && i + 1 < args.Length) { colId = _selectedBoard!.Columns.FirstOrDefault(c => c.Name.Contains(args[++i], StringComparison.OrdinalIgnoreCase))?.Id; }
                    if (args[i] == "--due" && i + 1 < args.Length && DateTime.TryParse(args[++i], out var d)) { due = d; }
                }
                if (colId == null) { Console.WriteLine("Колонка не найдена"); return; }
                var task = _kanban.AddTask(colId.Value, title, priority: priority);
                if (due.HasValue) { task.DueDate = due; _kanban.UpdateTask(task); }
                _storage.SaveBoard(_selectedBoard!);
                Console.WriteLine($"Задача создана: {task.Title} [{task.Id.ToString()[..8]}]");
                break;
            case "move":
                EnsureBoard();
                if (args.Length < 4) { Console.WriteLine("Использование: kanban task move <id> \"Колонка\""); return; }
                var taskId = Guid.Parse(args[2]);
                var targetCol = _selectedBoard!.Columns.FirstOrDefault(c => c.Name.Contains(args[3], StringComparison.OrdinalIgnoreCase));
                if (targetCol == null) { Console.WriteLine("Колонка не найдена"); return; }
                _kanban.MoveTask(taskId, targetCol.Id);
                _storage.SaveBoard(_selectedBoard!);
                Console.WriteLine("Задача перемещена");
                break;
            case "done":
                EnsureBoard();
                if (args.Length < 3) { Console.WriteLine("Укажите ID задачи"); return; }
                var doneId = Guid.Parse(args[2]);
                var doneCol = _selectedBoard!.Columns.LastOrDefault();
                if (doneCol != null) { _kanban.MoveTask(doneId, doneCol.Id); _storage.SaveBoard(_selectedBoard); Console.WriteLine("Задача выполнена"); }
                break;
            case "delete":
                EnsureBoard();
                if (args.Length < 3) { Console.WriteLine("Укажите ID задачи"); return; }
                _kanban.DeleteTask(Guid.Parse(args[2]));
                _storage.SaveBoard(_selectedBoard!);
                Console.WriteLine("Задача удалена");
                break;
        }
    }

    private static void ShowStats()
    {
        EnsureBoard();
        var stats = _kanban.GetStatistics(_selectedBoard!.Id);
        Console.WriteLine($"=== Статистика: {_selectedBoard!.Name} ===");
        Console.WriteLine($"Всего задач: {stats.TotalTasks}");
        Console.WriteLine($"Выполнено: {stats.CompletedTasks}");
        Console.WriteLine($"В работе: {stats.InProgressTasks}");
        Console.WriteLine($"Процент выполнения: {stats.CompletionRate:F1}%");
        Console.WriteLine($"Помидоров потрачено: {stats.TotalPomodorosSpent}");
        Console.WriteLine($"Время: {stats.TotalTimeSpent:hh\\:mm\\:ss}");
    }

    private static void HandlePomodoro(string[] args)
    {
        if (args.Length < 2 || args[1] != "start") { Console.WriteLine("Использование: kanban pomodoro start <taskId>"); return; }
        Console.WriteLine("🍅 Pomodoro запущен! (25 минут)");
        Console.WriteLine("Нажмите Enter для остановки...");
        Console.ReadLine();
        Console.WriteLine("🍅 Pomodoro завершён!");
    }

    private static void HandleExport(string[] args)
    {
        EnsureBoard();
        string format = "json", output = "board.json";
        for (int i = 1; i < args.Length; i++)
        {
            if (args[i] == "--format" && i + 1 < args.Length) format = args[++i].ToLower();
            if (args[i] == "--output" && i + 1 < args.Length) output = args[++i];
        }
        if (format == "csv") _storage.ExportToCsv(_selectedBoard!.Id, output);
        else _storage.ExportToJson(_selectedBoard!.Id, output);
        Console.WriteLine($"Экспортировано в {output}");
    }

    private static void HandleImport(string[] args)
    {
        string? file = null;
        for (int i = 1; i < args.Length; i++)
            if (args[i] == "--file" && i + 1 < args.Length) file = args[++i];
        if (file == null) { Console.WriteLine("Укажите файл: kanban import --file board.json"); return; }
        var imported = _storage.ImportFromJson(file);
        if (imported != null) Console.WriteLine($"Импортировано: {imported.Name}");
        else Console.WriteLine("Ошибка импорта");
    }

    private static void EnsureBoard()
    {
        if (_selectedBoard == null)
        {
            _selectedBoard = _kanban.GetAllBoards().FirstOrDefault();
            if (_selectedBoard == null) { Console.WriteLine("Сначала создайте или выберите доску"); Environment.Exit(1); }
        }
    }

    private static void ShowHelp()
    {
        Console.WriteLine("=== Канбан-доска CLI ===");
        Console.WriteLine("kanban boards                          — список досок");
        Console.WriteLine("kanban board create \"Название\"         — создать доску");
        Console.WriteLine("kanban board select <id>               — выбрать доску");
        Console.WriteLine("kanban tasks                           — список задач");
        Console.WriteLine("kanban task add \"Заголовок\" --priority high --column \"В работе\"");
        Console.WriteLine("kanban task move <id> \"Готово\"         — переместить задачу");
        Console.WriteLine("kanban task done <id>                  — выполнить задачу");
        Console.WriteLine("kanban task delete <id>                — удалить задачу");
        Console.WriteLine("kanban stats                           — статистика");
        Console.WriteLine("kanban pomodoro start <taskId>         — запустить pomodoro");
        Console.WriteLine("kanban export --format json --output board.json");
        Console.WriteLine("kanban import --file board.json");
        Console.WriteLine("kanban help                            — справка");
    }
}
