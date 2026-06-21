using FractionTrainer.Core.Models;

namespace FractionTrainer.Core.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса настроек приложения.
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Загружает текущие настройки.
        /// </summary>
        /// <returns>Текущие настройки приложения.</returns>
        AppSettings Load();

        /// <summary>
        /// Сохраняет настройки.
        /// </summary>
        /// <param name="settings">Настройки для сохранения.</param>
        void Save(AppSettings settings);

        /// <summary>
        /// Сбрасывает настройки к значениям по умолчанию.
        /// </summary>
        void Reset();
    }
}
