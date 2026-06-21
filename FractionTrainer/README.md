# FractionTrainer

## Описание проекта

**Тренажёр «Собрать дробь»** — это приложение для изучения дробей с помощью визуальной интерактивной игры. Пользователь видит круг, разделённый на секторы, и должен выбрать нужное количество секторов, чтобы собрать указанную дробь.

Приложение поддерживает два режима:
- **Обучение** — с подсказками и возможностью посмотреть правильный ответ
- **Проверка знаний** — без подсказок, с проверкой правильности ответа

Три уровня сложности определяют диапазон знаменателей:
- **Лёгкий**: знаменатели 2–4
- **Средний**: знаменатели 5–8
- **Сложный**: знаменатели 9–16

## Структура решения

```
FractionTrainer/
├── FractionTrainer.sln
├── FractionTrainer.Core/          # Бизнес-логика (Class Library)
│   ├── Models/                    # Fraction, FractionPuzzle, PuzzleSession и др.
│   ├── Enums/                     # AppMode, DifficultyLevel
│   ├── Events/                    # Аргументы событий
│   ├── Interfaces/                # IPuzzleGenerator, IScoreRepository, ISettingsService
│   └── Services/                  # PuzzleGenerator, JsonScoreRepository, SettingsService
├── FractionTrainer.WPF/           # GUI приложение (WPF)
│   ├── Views/                     # MainWindow, TrainerView, HistoryView, SettingsView, HelpWindow
│   ├── Infrastructure/            # ObservableObject, RelayCommand (MVVM)
│   └── Themes/                    # LightTheme.xaml, DarkTheme.xaml
├── FractionTrainer.Console/       # Консольный интерфейс
│   └── Program.cs
└── FractionTrainer.Tests/         # Unit-тесты (xUnit)
    ├── FractionTests.cs
    ├── FractionPuzzleTests.cs
    └── PuzzleGeneratorTests.cs
```

## Как запустить (WPF)

1. Откройте `FractionTrainer.sln` в Visual Studio 2026
2. Выберите проект `FractionTrainer.WPF` как запускаемый
3. Нажмите **F5** или кнопку **Запуск**

Альтернативно через командную строку:
```bash
cd FractionTrainer.WPF
dotnet run
```

## Как запустить (Console)

```bash
cd FractionTrainer.Console
dotnet run
```

### Примеры аргументов

```bash
# Режим.quiz, 3 задачи, лёгкая сложность
dotnet run -- --mode quiz --count 3 --difficulty easy

# Режим обучения с показом ответов
dotnet run -- --mode learning --show-answer

# Показать историю последних 10 сессий
dotnet run -- --history

# Экспорт истории в CSV
dotnet run -- --export-csv history.csv

# Справка
dotnet run -- --help
```

## Горячие клавиши

| Клавиша | Действие |
|---------|----------|
| F1 | Открыть окно справки |
| Ctrl+N | Новая задача (на экране тренажёра) |
| Ctrl+R | Сбросить выбор секторов |
| Ctrl+T | Переключить тему (светлая/тёмная) |
| Escape | Сбросить выбор секторов |

## Скриншоты

[скриншот здесь]
