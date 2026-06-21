using System.Windows.Media;
using FractionTrainer.Core.Enums;
using FractionTrainer.Core.Interfaces;
using FractionTrainer.Core.Models;
using FractionTrainer.WPF.Infrastructure;

namespace FractionTrainer.WPF.Views
{
    /// <summary>
    /// Модель представления для экрана настроек.
    /// </summary>
    public class SettingsViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;

        private string _selectedTheme = "Light";
        private string _selectedAccentColor = "#4A90D9";
        private DifficultyLevel _selectedDifficulty = DifficultyLevel.Easy;
        private AppMode _selectedMode = AppMode.Learning;
        private bool _showHints = true;
        private string _statusText = string.Empty;

        /// <summary>Выбранная тема.</summary>
        public string SelectedTheme
        {
            get { return _selectedTheme; }
            set { _selectedTheme = value; OnPropertyChanged(); }
        }

        /// <summary>Выбранный акцентный цвет (HEX).</summary>
        public string SelectedAccentColor
        {
            get { return _selectedAccentColor; }
            set { _selectedAccentColor = value; OnPropertyChanged(); }
        }

        /// <summary>Выбранный уровень сложности.</summary>
        public DifficultyLevel SelectedDifficulty
        {
            get { return _selectedDifficulty; }
            set { _selectedDifficulty = value; OnPropertyChanged(); }
        }

        /// <summary>Выбранный режим по умолчанию.</summary>
        public AppMode SelectedMode
        {
            get { return _selectedMode; }
            set { _selectedMode = value; OnPropertyChanged(); }
        }

        /// <summary>Показывать ли подсказки.</summary>
        public bool ShowHints
        {
            get { return _showHints; }
            set { _showHints = value; OnPropertyChanged(); }
        }

        /// <summary>Текст статуса.</summary>
        public string StatusText
        {
            get { return _statusText; }
            set { _statusText = value; OnPropertyChanged(); }
        }

        /// <summary>Доступные цвета для выбора акцента.</summary>
        public List<string> AccentColors { get; } = new List<string>
        {
            "#4A90D9",
            "#E74C3C",
            "#2ECC71",
            "#F39C12",
            "#9B59B6",
            "#1ABC9C",
            "#E67E22",
            "#3498DB"
        };

        /// <summary>Команда: сохранить настройки.</summary>
        public RelayCommand SaveCommand { get; }

        /// <summary>Команда: сбросить к стандартным.</summary>
        public RelayCommand ResetCommand { get; }

        /// <summary>Команда: выбрать цвет.</summary>
        public RelayCommand<string> SelectColorCommand { get; }

        /// <summary>
        /// Создаёт экземпляр SettingsViewModel.
        /// </summary>
        /// <param name="settingsService">Сервис настроек.</param>
        public SettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            SaveCommand = new RelayCommand(ExecuteSave);
            ResetCommand = new RelayCommand(ExecuteReset);
            SelectColorCommand = new RelayCommand<string>(ExecuteSelectColor);

            LoadSettings();
        }

        /// <summary>
        /// Загружает настройки из хранилища.
        /// </summary>
        private void LoadSettings()
        {
            AppSettings settings = _settingsService.Load();
            SelectedTheme = settings.Theme;
            SelectedAccentColor = settings.AccentColor;
            SelectedDifficulty = settings.DefaultDifficulty;
            SelectedMode = settings.DefaultMode;
            ShowHints = settings.ShowHints;
        }

        /// <summary>
        /// Сохраняет настройки.
        /// </summary>
        public void ExecuteSave()
        {
            AppSettings settings = new AppSettings
            {
                Theme = SelectedTheme,
                AccentColor = SelectedAccentColor,
                DefaultDifficulty = SelectedDifficulty,
                DefaultMode = SelectedMode,
                ShowHints = ShowHints
            };

            _settingsService.Save(settings);

            App.SwitchTheme(SelectedTheme);

            StatusText = "Настройки сохранены!";
        }

        /// <summary>
        /// Сбрасывает настройки к стандартным.
        /// </summary>
        public void ExecuteReset()
        {
            _settingsService.Reset();
            LoadSettings();
            App.SwitchTheme(SelectedTheme);
            StatusText = "Настройки сброшены к стандартным.";
        }

        /// <summary>
        /// Выбирает акцентный цвет.
        /// </summary>
        /// <param name="color">HEX-строка цвета.</param>
        public void ExecuteSelectColor(string? color)
        {
            if (!string.IsNullOrEmpty(color))
            {
                SelectedAccentColor = color;
            }
        }
    }
}
