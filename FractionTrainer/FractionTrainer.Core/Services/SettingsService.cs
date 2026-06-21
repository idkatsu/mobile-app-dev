using System.Text.Json;
using FractionTrainer.Core.Interfaces;
using FractionTrainer.Core.Models;

namespace FractionTrainer.Core.Services
{
    /// <summary>
    /// Сервис хранения настроек приложения в JSON-файле.
    /// Файл находится в %AppData%/FractionTrainer/settings.json.
    /// </summary>
    public class SettingsService : ISettingsService
    {
        private readonly string _filePath;

        /// <summary>
        /// Создаёт экземпляр SettingsService с путём по умолчанию.
        /// </summary>
        public SettingsService()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string directoryPath = Path.Combine(appDataPath, "FractionTrainer");
            _filePath = Path.Combine(directoryPath, "settings.json");
        }

        /// <summary>
        /// Создаёт экземпляр SettingsService с указанным путём к файлу.
        /// </summary>
        /// <param name="filePath">Путь к JSON-файлу настроек.</param>
        public SettingsService(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        /// <summary>
        /// Загружает настройки из файла. Если файл не существует — возвращает настройки по умолчанию.
        /// </summary>
        /// <returns>Текущие настройки приложения.</returns>
        public AppSettings Load()
        {
            if (!File.Exists(_filePath))
            {
                return AppSettings.CreateDefault();
            }

            string jsonString = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return AppSettings.CreateDefault();
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            AppSettings? settings = JsonSerializer.Deserialize<AppSettings>(jsonString, options);

            return settings ?? AppSettings.CreateDefault();
        }

        /// <summary>
        /// Сохраняет настройки в JSON-файл.
        /// </summary>
        /// <param name="settings">Настройки для сохранения.</param>
        public void Save(AppSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            EnsureDirectoryExists();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(settings, options);
            File.WriteAllText(_filePath, jsonString);
        }

        /// <summary>
        /// Сбрасывает настройки к значениям по умолчанию и сохраняет их.
        /// </summary>
        public void Reset()
        {
            AppSettings defaultSettings = AppSettings.CreateDefault();
            Save(defaultSettings);
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
