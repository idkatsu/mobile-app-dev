namespace FractionTrainer.Core.Models
{
    /// <summary>
    /// Одна попытка решения головоломки в рамках сессии.
    /// </summary>
    public class PuzzleAttempt
    {
        /// <summary>Головоломка, которую решали.</summary>
        public FractionPuzzle Puzzle { get; }

        /// <summary>Время начала попытки.</summary>
        public DateTime StartedAt { get; }

        /// <summary>Время завершения попытки (null, если ещё не завершена).</summary>
        public DateTime? FinishedAt { get; set; }

        /// <summary>Правильно ли решена задача.</summary>
        public bool IsCorrect { get; set; }

        /// <summary>
        /// Длительность попытки. Если попытка ещё не завершена, вычисляет время от начала до текущего момента.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                DateTime endTime = FinishedAt ?? DateTime.Now;
                return endTime - StartedAt;
            }
        }

        /// <summary>
        /// Создаёт новую попытку решения.
        /// </summary>
        /// <param name="puzzle">Головоломка.</param>
        public PuzzleAttempt(FractionPuzzle puzzle)
        {
            Puzzle = puzzle ?? throw new ArgumentNullException(nameof(puzzle));
            StartedAt = DateTime.Now;
        }
    }
}
