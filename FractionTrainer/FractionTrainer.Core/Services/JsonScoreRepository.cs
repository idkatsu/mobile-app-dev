using System.Text.Json;
using FractionTrainer.Core.Interfaces;
using FractionTrainer.Core.Models;

namespace FractionTrainer.Core.Services
{
    /// <summary>
    /// Хранилище результатов сессий в JSON-файле.
    /// Файл находится в %AppData%/FractionTrainer/scores.json.
    /// </summary>
    public class JsonScoreRepository : IScoreRepository
    {
        private readonly string _filePath;

        /// <summary>
        /// Создаёт экземпляр JsonScoreRepository с путём по умолчанию.
        /// </summary>
        public JsonScoreRepository()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string directoryPath = Path.Combine(appDataPath, "FractionTrainer");
            _filePath = Path.Combine(directoryPath, "scores.json");
        }

        /// <summary>
        /// Создаёт экземпляр JsonScoreRepository с указанным путём к файлу.
        /// </summary>
        /// <param name="filePath">Путь к JSON-файлу.</param>
        public JsonScoreRepository(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        /// <summary>
        /// Сохраняет результат сессии, добавляя его к существующим данным.
        /// </summary>
        /// <param name="result">Результат для сохранения.</param>
        public void Save(SessionResult result)
        {
            if (result is null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            List<SessionResult> existingResults = LoadAll();
            existingResults.Add(result);

            EnsureDirectoryExists();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(existingResults, options);
            File.WriteAllText(_filePath, jsonString);
        }

        /// <summary>
        /// Загружает все сохранённые результаты из JSON-файла.
        /// </summary>
        /// <returns>Список результатов. Если файл не существует — пустой список.</returns>
        public List<SessionResult> LoadAll()
        {
            if (!File.Exists(_filePath))
            {
                return new List<SessionResult>();
            }

            string jsonString = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return new List<SessionResult>();
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<SessionResult>? results = JsonSerializer.Deserialize<List<SessionResult>>(jsonString, options);

            return results ?? new List<SessionResult>();
        }

        /// <summary>
        /// Очищает файл с результатами.
        /// </summary>
        public void Clear()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }

        /// <summary>
        /// Убеждается, что директория для файла существует.
        /// </summary>
        private void EnsureDirectoryExists()
        {
            string? directory = Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
