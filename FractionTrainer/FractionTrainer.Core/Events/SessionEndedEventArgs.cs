namespace FractionTrainer.Core.Events
{
    /// <summary>
    /// Аргументы события завершения сессии.
    /// </summary>
    public class SessionEndedEventArgs : EventArgs
    {
        /// <summary>Результат завершённой сессии.</summary>
        public Models.SessionResult Result { get; }

        /// <summary>
        /// Создаёт экземпляр SessionEndedEventArgs.
        /// </summary>
        /// <param name="result">Результат сессии.</param>
        public SessionEndedEventArgs(Models.SessionResult result)
        {
            Result = result;
        }
    }
}
