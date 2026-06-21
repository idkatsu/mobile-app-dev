using System.Diagnostics;
using FractionTrainer.Core.Enums;
using FractionTrainer.Core.Interfaces;
using FractionTrainer.Core.Models;
using FractionTrainer.Core.Services;

namespace FractionTrainer.Console
{
    /// <summary>
    /// Консольный интерфейс тренажёра собрать дробь.
    /// </summary>
    public class Program
    {
        private static AppMode _mode = AppMode.Learning;
        private static DifficultyLevel _difficulty = DifficultyLevel.Easy;
        private static int _taskCount = 5;
        private static bool _showAnswer = false;
        private static bool _showHistory = false;
        private static string? _exportCsvPath = null;

        /// <summary>
        /// Точка входа консольного приложения.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        public static void Main(string[] args)
        {
            if (!ParseArguments(args))
            {
                return;
            }

            IPuzzleGenerator puzzleGenerator = new PuzzleGenerator();
            IScoreRepository scoreRepository = new JsonScoreRepository();

            if (_showHistory)
            {
                ShowHistory(scoreRepository);
                return;
            }

            if (!string.IsNullOrEmpty(_exportCsvPath))
            {
                ExportCsv(scoreRepository, _exportCsvPath);
                return;
            }

            RunSession(puzzleGenerator, scoreRepository);
        }

        /// <summary>
        /// Парсит аргументы командной строки.
        /// </summary>
        /// <returns>true, если парсинг успешен; false — показать справку.</returns>
        private static bool ParseArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i].ToLower();

                switch (arg)
                {
                    case "--mode":
                        if (i + 1 < args.Length)
                        {
                            string modeValue = args[i + 1].ToLower();
                            if (modeValue == "learning")
                                _mode = AppMode.Learning;
                            else if (modeValue == "quiz")
                                _mode = AppMode.Quiz;
                            else
                            {
                                System.Console.WriteLine($"Неизвестный режим: {args[i + 1]}");
                                return false;
                            }
                            i++;
                        }
                        break;

                    case "--difficulty":
                        if (i + 1 < args.Length)
                        {
                            string diffValue = args[i + 1].ToLower();
                            if (diffValue == "easy")
                                _difficulty = DifficultyLevel.Easy;
                            else if (diffValue == "medium")
                                _difficulty = DifficultyLevel.Medium;
                            else if (diffValue == "hard")
                                _difficulty = DifficultyLevel.Hard;
                            else
                            {
                                System.Console.WriteLine($"Неизвестная сложность: {args[i + 1]}");
                                return false;
                            }
                            i++;
                        }
                        break;

                    case "--count":
                        if (i + 1 < args.Length && int.TryParse(args[i + 1], out int count))
                        {
                            _taskCount = count;
                            i++;
                        }
                        break;

                    case "--show-answer":
                        _showAnswer = true;
                        break;

                    case "--history":
                        _showHistory = true;
                        break;

                    case "--export-csv":
                        if (i + 1 < args.Length)
                        {
                            _exportCsvPath = args[i + 1];
                            i++;
                        }
                        break;

                    case "--help":
                    case "-h":
                        ShowHelp();
                        return false;

                    default:
                        System.Console.WriteLine($"Неизвестный аргумент: {args[i]}");
                        ShowHelp();
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Показывает справку по использованию.
        /// </summary>
        private static void ShowHelp()
        {
            System.Console.WriteLine("=== Тренажёр собрать дробь ===");
            System.Console.WriteLine();
            System.Console.WriteLine("Параметры:");
            System.Console.WriteLine("  --mode [learning|quiz]         Режим работы");
            System.Console.WriteLine("  --difficulty [easy|medium|hard] Уровень сложности");
            System.Console.WriteLine("  --count N                      Количество задач (по умолчанию 5)");
            System.Console.WriteLine("  --show-answer                  Автоматически показывать ответ (только learning)");
            System.Console.WriteLine("  --history                      Вывести последние 10 сессий");
            System.Console.WriteLine("  --export-csv <путь>            Экспорт истории в CSV");
            System.Console.WriteLine("  --help / -h                    Показать эту справку");
            System.Console.WriteLine();
            System.Console.WriteLine("Примеры:");
            System.Console.WriteLine("  dotnet FractionTrainer.Console.dll --mode quiz --count 3 --difficulty easy");
            System.Console.WriteLine("  dotnet FractionTrainer.Console.dll --mode learning --show-answer");
            System.Console.WriteLine("  dotnet FractionTrainer.Console.dll --history");
        }

        /// <summary>
        /// Запускает интерактивную сессию тренировки.
        /// </summary>
        private static void RunSession(IPuzzleGenerator puzzleGenerator, IScoreRepository scoreRepository)
        {
            string modeText = _mode == AppMode.Learning ? "Обучение" : "Проверка знаний";
            string diffText = _difficulty.ToString();

            System.Console.WriteLine("=== Тренажёр собрать дробь ===");
            System.Console.WriteLine($"Режим: {modeText} | Сложность: {diffText} | Задач: {_taskCount}");
            System.Console.WriteLine();

            PuzzleSession session = new PuzzleSession(_mode);
            Stopwatch sessionStopwatch = Stopwatch.StartNew();

            for (int taskNumber = 1; taskNumber <= _taskCount; taskNumber++)
            {
                FractionPuzzle puzzle = puzzleGenerator.Generate(_difficulty);
                PuzzleAttempt attempt = session.StartAttempt(puzzle);

                System.Console.Write($"Задача {taskNumber}/{_taskCount}: Собери дробь ");
                System.Console.ForegroundColor = ConsoleColor.Cyan;
                System.Console.WriteLine(puzzle.TargetFraction);
                System.Console.ResetColor();

                System.Console.WriteLine($"Всего секторов: {puzzle.TotalSectors}");

                if (_showAnswer && _mode == AppMode.Learning)
                {
                    ShowCorrectAnswer(puzzle);
                }

                System.Console.Write("Введи номера выбранных секторов через запятую (0-based): ");
                string? input = System.Console.ReadLine();

                Stopwatch taskStopwatch = Stopwatch.StartNew();
                bool isCorrect = ProcessUserInput(input, puzzle);
                taskStopwatch.Stop();

                session.FinishAttempt(attempt, isCorrect);

                if (isCorrect)
                {
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.Write("  ✓ Верно!");
                }
                else
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.Write("  ✗ Неверно.");
                    System.Console.Write($" Правильный ответ: секторы {GetCorrectSectorIndices(puzzle)}");
                }

                System.Console.ResetColor();
                System.Console.WriteLine($" ({taskStopwatch.Elapsed.TotalSeconds:F1} сек)");
                System.Console.WriteLine();
            }

            sessionStopwatch.Stop();

            SessionResult result = session.GetResult();
            scoreRepository.Save(result);

            ShowSessionResult(result);
        }

        /// <summary>
        /// Обрабатывает ввод пользователя и проверяет ответ.
        /// </summary>
        private static bool ProcessUserInput(string? input, FractionPuzzle puzzle)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            HashSet<int> selectedSectors = new HashSet<int>();
            string[] parts = input.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in parts)
            {
                string trimmed = part.Trim();
                if (int.TryParse(trimmed, out int sectorIndex))
                {
                    if (sectorIndex >= 0 && sectorIndex < puzzle.TotalSectors)
                    {
                        puzzle.ToggleSector(sectorIndex);
                        selectedSectors.Add(sectorIndex);
                    }
                }
            }

            return puzzle.IsSolved;
        }

        /// <summary>
        /// Показывает правильный ответ.
        /// </summary>
        private static void ShowCorrectAnswer(FractionPuzzle puzzle)
        {
            string correctIndices = GetCorrectSectorIndices(puzzle);
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine($"  Подсказка: выбери секторы {correctIndices}");
            System.Console.ResetColor();
        }

        /// <summary>
        /// Возвращает строку с правильными индексами секторов.
        /// </summary>
        private static string GetCorrectSectorIndices(FractionPuzzle puzzle)
        {
            List<int> correctIndices = new List<int>();
            for (int i = 0; i < puzzle.TargetFraction.Numerator; i++)
            {
                correctIndices.Add(i);
            }
            return string.Join(",", correctIndices);
        }

        /// <summary>
        /// Показывает результат сессии.
        /// </summary>
        private static void ShowSessionResult(SessionResult result)
        {
            System.Console.WriteLine("=== Результат сессии ===");
            System.Console.Write("Правильно: ");
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.Write($"{result.CorrectAnswers} / {result.TotalAttempts}");
            System.Console.ResetColor();
            System.Console.Write($" ({result.AccuracyPercent:F1}%) за ");
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine($"{result.TotalTime:hh\\:mm\\:ss}");
            System.Console.ResetColor();
        }

        /// <summary>
        /// Показывает последние 10 сессий из истории.
        /// </summary>
        private static void ShowHistory(IScoreRepository scoreRepository)
        {
            List<SessionResult> results = scoreRepository.LoadAll();
            List<SessionResult> lastTen = results
                .OrderByDescending(r => r.CompletedAt)
                .Take(10)
                .ToList();

            if (lastTen.Count == 0)
            {
                System.Console.WriteLine("История пуста.");
                return;
            }

            System.Console.WriteLine("=== Последние сессии ===");
            System.Console.WriteLine(
                string.Format("{0,-20} {1,-20} {2,-10} {3,-8} {4,-10} {5,-10}",
                    "Дата", "Режим", "Правильно", "Всего", "Точность", "Время"));
            System.Console.WriteLine(new string('-', 78));

            foreach (SessionResult result in lastTen)
            {
                System.Console.WriteLine(
                    string.Format("{0,-20} {1,-20} {2,-10} {3,-8} {4,-10} {5,-10}",
                        result.CompletedAt.ToString("dd.MM.yyyy HH:mm"),
                        result.Mode.ToString(),
                        result.CorrectAnswers.ToString(),
                        result.TotalAttempts.ToString(),
                        result.AccuracyPercent.ToString("F1") + "%",
                        result.TotalTime.ToString("hh\\:mm\\:ss")));
            }
        }

        /// <summary>
        /// Экспортирует историю в CSV-файл.
        /// </summary>
        private static void ExportCsv(IScoreRepository scoreRepository, string filePath)
        {
            List<SessionResult> results = scoreRepository.LoadAll();

            using (StreamWriter writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                writer.WriteLine("Дата;Режим;Правильно;Всего;Точность;Время");

                foreach (SessionResult result in results)
                {
                    writer.WriteLine(
                        $"{result.CompletedAt:dd.MM.yyyy HH:mm};" +
                        $"{result.Mode};" +
                        $"{result.CorrectAnswers};" +
                        $"{result.TotalAttempts};" +
                        $"{result.AccuracyPercent:F1};" +
                        $"{result.TotalTime:hh\\:mm\\:ss}");
                }
            }

            System.Console.WriteLine($"История экспортирована в файл: {filePath}");
        }
    }
}
