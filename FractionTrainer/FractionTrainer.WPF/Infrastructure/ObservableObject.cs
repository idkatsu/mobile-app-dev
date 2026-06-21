using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FractionTrainer.WPF.Infrastructure
{
    /// <summary>
    /// Базовый класс для моделей представлений, реализующий INotifyPropertyChanged.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>Событие изменения свойства.</summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Вызывает событие изменения свойства.
        /// </summary>
        /// <param name="name">Имя изменившегося свойства.</param>
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
