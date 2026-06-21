using FractionTrainer.Core.Enums;
using FractionTrainer.Core.Events;

namespace FractionTrainer.Core.Models
{
    /// <summary>
    /// Сессия решения головоломок, содержащая список попыток и результат.
    /// </summary>
    public class PuzzleSession
    {
        private readonly List<PuzzleAttempt> _attempts = new List<PuzzleAttempt>();

        /// <summary>Режим работы сессии.</summary>
        public AppMode Mode { get; }

        /// <summary>Список попыток в рамках сессии.</summary>
        public List<PuzzleAttempt> Attempts
        {
            get { return new List<PuzzleAttempt>(_attempts); }
        }

        /// <summary>Время начала сессии.</summary>
        public DateTime StartedAt { get; }

        /// <summary>Количество правильных ответов.</summary>
        public int Score
        {
            get { return _attempts.Count(a => a.IsCorrect); }
        }

        /// <summary>
        /// Событие завершения сессии.
        /// </summary>
        public event EventHandler<SessionEndedEventArgs>? SessionEnded;

        /// <summary>
        /// Создаёт новую сессию.
        /// </summary>
        /// <param name="mode">Режим работы.</param>
        public PuzzleSession(AppMode mode)
        {
            Mode = mode;
            StartedAt = DateTime.Now;
        }

        /// <summary>
        /// Начинает новую попытку решения головоломки.
        /// </summary>
        /// <param name="puzzle">Головоломка для решения.</param>
        /// <returns>Созданная попытка.</returns>
        public PuzzleAttempt StartAttempt(FractionPuzzle puzzle)
        {
            PuzzleAttempt attempt = new PuzzleAttempt(puzzle);
            _attempts.Add(attempt);
            return attempt;
        }

        /// <summary>
        /// Завершает попытку, фиксируя результат.
        /// </summary>
        /// <param name="attempt">Попытка для завершения.</param>
        /// <param name="isCorrect">Правильный ли ответ.</param>
        public void FinishAttempt(PuzzleAttempt attempt, bool isCorrect)
        {
            if (attempt is null)
            {
                throw new ArgumentNullException(nameof(attempt));
            }

            attempt.FinishedAt = DateTime.Now;
            attempt.IsCorrect = isCorrect;
        }

        /// <summary>
        /// Формирует результат сессии.
        /// </summary>
        /// <returns>Объект SessionResult с итоговой статистикой.</returns>
        public SessionResult GetResult()
        {
            int totalAttempts = _attempts.Count;
            int correctAnswers = _attempts.Count(a => a.IsCorrect);
            double accuracyPercent = totalAttempts > 0
                ? (double)correctAnswers / totalAttempts * 100.0
                : 0.0;

            TimeSpan totalTime = TimeSpan.Zero;
            foreach (PuzzleAttempt attempt in _attempts)
            {
                totalTime = totalTime.Add(attempt.Duration);
            }

            SessionResult result = new SessionResult
            {
                TotalAttempts = totalAttempts,
                CorrectAnswers = correctAnswers,
                AccuracyPercent = accuracyPercent,
                TotalTime = totalTime,
                Mode = Mode
            };

            SessionEnded?.Invoke(this, new SessionEndedEventArgs(result));

            return result;
        }
    }
}
