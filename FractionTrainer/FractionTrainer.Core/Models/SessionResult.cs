using FractionTrainer.Core.Enums;

namespace FractionTrainer.Core.Models
{
    /// <summary>
    /// Результат завершённой сессии решения головоломок.
    /// </summary>
    public class SessionResult
    {
        /// <summary>Общее количество попыток.</summary>
        public int TotalAttempts { get; set; }

        /// <summary>Количество правильных ответов.</summary>
        public int CorrectAnswers { get; set; }

        /// <summary>Процент правильных ответов.</summary>
        public double AccuracyPercent { get; set; }

        /// <summary>Общее время сессии.</summary>
        public TimeSpan TotalTime { get; set; }

        /// <summary>Режим работы сессии.</summary>
        public AppMode Mode { get; set; }

        /// <summary>Время завершения сессии (для сохранения в истории).</summary>
        public DateTime CompletedAt { get; set; } = DateTime.Now;
    }
}
