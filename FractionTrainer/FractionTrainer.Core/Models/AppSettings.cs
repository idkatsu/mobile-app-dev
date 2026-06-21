using FractionTrainer.Core.Enums;

namespace FractionTrainer.Core.Models
{
    /// <summary>
    /// Настройки приложения.
    /// </summary>
    public class AppSettings
    {
        /// <summary>Тема оформления: "Light" или "Dark".</summary>
        public string Theme { get; set; } = "Light";

        /// <summary>Акцентный цвет в формате HEX, например "#4A90D9".</summary>
        public string AccentColor { get; set; } = "#4A90D9";

        /// <summary>Уровень сложности по умолчанию.</summary>
        public DifficultyLevel DefaultDifficulty { get; set; } = DifficultyLevel.Easy;

        /// <summary>Режим работы по умолчанию.</summary>
        public AppMode DefaultMode { get; set; } = AppMode.Learning;

        /// <summary>Показывать ли подсказки.</summary>
        public bool ShowHints { get; set; } = true;

        /// <summary>
        /// Создаёт настройки по умолчанию.
        /// </summary>
        /// <returns>Экземпляр AppSettings со значениями по умолчанию.</returns>
        public static AppSettings CreateDefault()
        {
            return new AppSettings
            {
                Theme = "Light",
                AccentColor = "#4A90D9",
                DefaultDifficulty = DifficultyLevel.Easy,
                DefaultMode = AppMode.Learning,
                ShowHints = true
            };
        }
    }
}
