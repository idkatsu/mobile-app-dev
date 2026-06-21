using FractionTrainer.Core.Models;

namespace FractionTrainer.Core.Interfaces
{
    /// <summary>
    /// Интерфейс хранилища результатов сессий.
    /// </summary>
    public interface IScoreRepository
    {
        /// <summary>
        /// Сохраняет результат сессии.
        /// </summary>
        /// <param name="result">Результат для сохранения.</param>
        void Save(SessionResult result);

        /// <summary>
        /// Загружает все сохранённые результаты.
        /// </summary>
        /// <returns>Список результатов всех сессий.</returns>
        List<SessionResult> LoadAll();

        /// <summary>
        /// Очищает всю историю результатов.
        /// </summary>
        void Clear();
    }
}
