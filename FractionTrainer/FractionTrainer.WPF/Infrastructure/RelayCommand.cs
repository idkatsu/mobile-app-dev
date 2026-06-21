using System.Windows.Input;

namespace FractionTrainer.WPF.Infrastructure
{
    /// <summary>
    /// Реализация ICommand для привязки команд в MVVM.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        /// <summary>
        /// Событие изменения возможности выполнения команды.
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Создаёт команду с указанием действия и условия выполнения.
        /// </summary>
        /// <param name="execute">Действие при выполнении команды.</param>
        /// <param name="canExecute">Условие, при котором команда доступна.</param>
        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Определяет, можно ли выполнить команду.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns>true, если команда доступна; иначе false.</returns>
        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        /// <summary>
        /// Выполняет команду.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        public void Execute(object? parameter)
        {
            _execute();
        }
    }

    /// <summary>
    /// Обобщённая реализация ICommand с параметром.
    /// </summary>
    /// <typeparam name="T">Тип параметра команды.</typeparam>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool>? _canExecute;

        /// <summary>Событие изменения возможности выполнения команды.</summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Создаёт команду с указанием действия и условия выполнения.
        /// </summary>
        /// <param name="execute">Действие при выполнении команды.</param>
        /// <param name="canExecute">Условие, при котором команда доступна.</param>
        public RelayCommand(Action<T> execute, Func<T, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Определяет, можно ли выполнить команду.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns>true, если команда доступна; иначе false.</returns>
        public bool CanExecute(object? parameter)
        {
            if (parameter is T typedParameter)
            {
                return _canExecute?.Invoke(typedParameter) ?? true;
            }
            return _canExecute == null;
        }

        /// <summary>
        /// Выполняет команду.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        public void Execute(object? parameter)
        {
            if (parameter is T typedParameter)
            {
                _execute(typedParameter);
            }
        }
    }
}
