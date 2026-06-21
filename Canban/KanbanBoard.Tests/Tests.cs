using KanbanBoard.Core.Enums;
using KanbanBoard.Core.Interfaces;
using KanbanBoard.Core.Models;
using KanbanBoard.Core.Services;
using Xunit;

namespace KanbanBoard.Tests;

public class KanbanServiceTests
{
    private readonly IKanbanService _service = new KanbanService();

    [Fact]
    public void CreateBoard_CreatesWith4DefaultColumns()
    {
        var board = _service.CreateBoard("Тестовая");
        Assert.Equal("Тестовая", board.Name);
        Assert.Equal(4, board.Columns.Count);
    }

    [Fact]
    public void DeleteBoard_RemovesBoard()
    {
        var board = _service.CreateBoard("Удалить");
        _service.DeleteBoard(board.Id);
        Assert.Null(_service.GetBoard(board.Id));
    }

    [Fact]
    public void RenameBoard_UpdatesName()
    {
        var board = _service.CreateBoard("Старое");
        _service.RenameBoard(board.Id, "Новое");
        Assert.Equal("Новое", _service.GetBoard(board.Id)!.Name);
    }

    [Fact]
    public void AddColumn_AddsToBoard()
    {
        var board = _service.CreateBoard("Тест");
        var col = _service.AddColumn(board.Id, "Новая колонка");
        Assert.Equal("Новая колонка", col.Name);
        Assert.Equal(5, _service.GetBoard(board.Id)!.Columns.Count);
    }

    [Fact]
    public void AddTask_AddsToColumn()
    {
        var board = _service.CreateBoard("Тест");
        var col = board.Columns[0];
        var task = _service.AddTask(col.Id, "Задача 1");
        Assert.Equal("Задача 1", task.Title);
        Assert.Single(_service.GetBoard(board.Id)!.Columns[0].Tasks);
    }

    [Fact]
    public void MoveTask_ChangesColumn()
    {
        var board = _service.CreateBoard("Тест");
        var col1 = board.Columns[0];
        var col2 = board.Columns[1];
        var task = _service.AddTask(col1.Id, "Переместить");
        _service.MoveTask(task.Id, col2.Id);
        Assert.Empty(_service.GetBoard(board.Id)!.Columns[0].Tasks);
        Assert.Single(_service.GetBoard(board.Id)!.Columns[1].Tasks);
    }

    [Fact]
    public void DeleteTask_RemovesFromColumn()
    {
        var board = _service.CreateBoard("Тест");
        var task = _service.AddTask(board.Columns[0].Id, "Удалить");
        _service.DeleteTask(task.Id);
        Assert.Empty(_service.GetBoard(board.Id)!.Columns[0].Tasks);
    }

    [Fact]
    public void SearchTasks_FindsByTitle()
    {
        var board = _service.CreateBoard("Тест");
        _service.AddTask(board.Columns[0].Id, "Купить молоко");
        _service.AddTask(board.Columns[0].Id, "Написать код");
        var results = _service.SearchTasks(board.Id, "молоко");
        Assert.Single(results);
        Assert.Equal("Купить молоко", results[0].Title);
    }

    [Fact]
    public void FilterTasks_ByPriority()
    {
        var board = _service.CreateBoard("Тест");
        _service.AddTask(board.Columns[0].Id, "Низкая", priority: TaskPriority.Low);
        _service.AddTask(board.Columns[0].Id, "Высокая", priority: TaskPriority.High);
        var results = _service.FilterTasks(board.Id, priority: TaskPriority.High);
        Assert.Single(results);
    }

    [Fact]
    public void GetStatistics_ComputesCorrectly()
    {
        var board = _service.CreateBoard("Тест");
        _service.AddTask(board.Columns[0].Id, "Задача 1");
        _service.AddTask(board.Columns[0].Id, "Задача 2");
        _service.MoveTask(board.Columns[0].Tasks[0].Id, board.Columns[3].Id);
        var stats = _service.GetStatistics(board.Id);
        Assert.Equal(2, stats.TotalTasks);
        Assert.Equal(1, stats.CompletedTasks);
        Assert.Equal(50.0, stats.CompletionRate);
    }
}

public class PomodoroServiceTests
{
    private readonly IPomodoroService _pomodoro;

    public PomodoroServiceTests()
    {
        _pomodoro = new PomodoroService(new JsonStorageService());
    }

    [Fact]
    public void Start_SetsRunning() { _pomodoro.Start(); Assert.True(_pomodoro.IsRunning); _pomodoro.Stop(); }

    [Fact]
    public void Pause_StopsRunning() { _pomodoro.Start(); _pomodoro.Pause(); Assert.False(_pomodoro.IsRunning); Assert.True(_pomodoro.IsPaused); _pomodoro.Stop(); }

    [Fact]
    public void Stop_ResetsState() { _pomodoro.Start(); _pomodoro.Stop(); Assert.False(_pomodoro.IsRunning); Assert.False(_pomodoro.IsPaused); Assert.Equal(0, _pomodoro.RemainingSeconds); }

    [Fact]
    public void Skip_MovesToNextPhase() { _pomodoro.Start(); _pomodoro.Skip(); Assert.True(_pomodoro.IsBreak); _pomodoro.Stop(); }

    [Fact]
    public void CurrentPomodoroNumber_IncrementsOnStart() { _pomodoro.Start(); Assert.Equal(1, _pomodoro.CurrentPomodoroNumber); _pomodoro.Stop(); }

    [Fact]
    public void GetTodayStats_ReturnsData()
    {
        var (completed, minutes) = _pomodoro.GetTodayStats();
        Assert.True(completed >= 0);
        Assert.True(minutes >= 0);
    }
}

public class StorageServiceTests
{
    private readonly IStorageService _storage = new JsonStorageService();

    [Fact]
    public void SaveAndLoadBoard()
    {
        var board = new Board { Name = "Тест сохранения" };
        _storage.SaveBoard(board);
        var loaded = _storage.LoadBoard(board.Id);
        Assert.NotNull(loaded);
        Assert.Equal("Тест сохранения", loaded!.Name);
    }

    [Fact]
    public void SaveAndLoadSettings()
    {
        var settings = new AppSettings { Theme = "Dark", AccentColor = "#FF0000" };
        _storage.SaveSettings(settings);
        var loaded = _storage.LoadSettings();
        Assert.Equal("Dark", loaded.Theme);
        Assert.Equal("#FF0000", loaded.AccentColor);
    }

    [Fact]
    public void ExportAndImportJson()
    {
        var board = new Board { Name = "Экспорт" };
        board.Columns[0].Tasks.Add(new KanbanTask { Title = "Задача экспорта" });
        _storage.SaveBoard(board);
        string path = Path.Combine(Path.GetTempPath(), "test_export.json");
        _storage.ExportToJson(board.Id, path);
        Assert.True(File.Exists(path));
        var imported = _storage.ImportFromJson(path);
        Assert.NotNull(imported);
        File.Delete(path);
    }
}

public class BoardStatisticsTests
{
    [Fact]
    public void EmptyBoard_ReturnsZeros()
    {
        var stats = new BoardStatistics();
        Assert.Equal(0, stats.TotalTasks);
        Assert.Equal(0, stats.CompletedTasks);
        Assert.Equal(0, stats.CompletionRate);
    }

    [Fact]
    public void Statistics_CorrectComputation()
    {
        var service = new KanbanService();
        var board = service.CreateBoard("Стат");
        var col1 = board.Columns[0];
        var colDone = board.Columns[3];
        var task1 = service.AddTask(col1.Id, "T1", priority: TaskPriority.High);
        var task2 = service.AddTask(col1.Id, "T2", priority: TaskPriority.Low);
        service.MoveTask(task2.Id, colDone.Id);
        var stats = service.GetStatistics(board.Id);
        Assert.Equal(2, stats.TotalTasks);
        Assert.Equal(1, stats.CompletedTasks);
    }
}
