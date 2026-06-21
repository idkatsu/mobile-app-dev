namespace FractionTrainer.Core.Events
{
    /// <summary>
    /// Аргументы события решения головоломки.
    /// </summary>
    public class PuzzleSolvedEventArgs : EventArgs
    {
        /// <summary>Решённая головоломка.</summary>
        public FractionTrainer.Core.Models.FractionPuzzle Puzzle { get; }

        /// <summary>Время, затраченное на решение.</summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Создаёт экземпляр PuzzleSolvedEventArgs.
        /// </summary>
        /// <param name="puzzle">Решённая головоломка.</param>
        /// <param name="duration">Затраченное время.</param>
        public PuzzleSolvedEventArgs(FractionTrainer.Core.Models.FractionPuzzle puzzle, TimeSpan duration)
        {
            Puzzle = puzzle;
            Duration = duration;
        }
    }
}
