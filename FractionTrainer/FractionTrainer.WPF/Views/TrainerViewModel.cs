using System.Windows;
using System.Windows.Media;
using FractionTrainer.Core.Enums;
using FractionTrainer.Core.Interfaces;
using FractionTrainer.Core.Models;
using FractionTrainer.WPF.Infrastructure;

namespace FractionTrainer.WPF.Views
{
    public class TrainerViewModel : ObservableObject
    {
        private readonly IPuzzleGenerator _puzzleGenerator;
        private readonly ISettingsService _settingsService;

        private FractionPuzzle? _currentPuzzle;
        private ComparePuzzle? _currentComparePuzzle;
        private CompleteToWholePuzzle? _currentCompletePuzzle;
        private EquivalentPuzzle? _currentEquivalentPuzzle;
        private FindPairsPuzzle? _currentPairsPuzzle;

        private int _correctCount;
        private int _totalCount;

        private PuzzleType _currentPuzzleType = PuzzleType.BuildFraction;
        private string _targetFractionText = string.Empty;
        private string _questionText = string.Empty;
        private string _currentFractionText = string.Empty;
        private string _scoreText = string.Empty;
        private int _totalSectors = 4;
        private HashSet<int> _selectedSectors = new HashSet<int>();
        private AppMode _selectedMode = AppMode.Learning;
        private DifficultyLevel _selectedDifficulty = DifficultyLevel.Easy;
        private Color _selectedSectorColor = Color.FromRgb(74, 144, 217);
        private Color _defaultSectorColor = Color.FromRgb(200, 200, 200);
        private bool _isCheckVisible = false;
        private bool _isShowAnswerVisible = true;
        private string _feedbackText = string.Empty;
        private Brush _feedbackBrush = Brushes.Transparent;
        private bool _isShaking = false;
        private bool _showCheckmark = false;
        private ShapeType _currentShape = ShapeType.Circle;
        private System.Windows.Threading.DispatcherTimer _feedbackTimer;

        private string _userNumeratorInput = string.Empty;
        private string _userDenominatorInput = string.Empty;
        private int _selectedCompareIndex = -1;
        private string _completeToWholeInput = string.Empty;
        private int _selectedEquivalentIndex = -1;

        private bool _showFractionInput = false;
        private bool _showCompareOptions = false;
        private bool _showCompleteToWhole = false;
        private bool _showEquivalentOptions = false;
        private bool _showShapeCanvas = true;
        private bool _showPairsBoard = false;

        private List<string> _compareOptions = new List<string>();
        private List<string> _equivalentOptions = new List<string>();
        private List<PairCardViewModel> _pairCards = new List<PairCardViewModel>();
        private int _firstSelectedPairIndex = -1;

        public string TargetFractionText
        {
            get { return _targetFractionText; }
            set { _targetFractionText = value; OnPropertyChanged(); }
        }

        public string QuestionText
        {
            get { return _questionText; }
            set { _questionText = value; OnPropertyChanged(); }
        }

        public string CurrentFractionText
        {
            get { return _currentFractionText; }
            set { _currentFractionText = value; OnPropertyChanged(); }
        }

        public string ScoreText
        {
            get { return _scoreText; }
            set { _scoreText = value; OnPropertyChanged(); }
        }

        public int TotalSectors
        {
            get { return _totalSectors; }
            set { _totalSectors = value; OnPropertyChanged(); }
        }

        public HashSet<int> SelectedSectors
        {
            get { return _selectedSectors; }
            set { _selectedSectors = value; OnPropertyChanged(); }
        }

        public AppMode SelectedMode
        {
            get { return _selectedMode; }
            set
            {
                _selectedMode = value;
                IsCheckVisible = value == AppMode.Quiz;
                IsShowAnswerVisible = value == AppMode.Learning;
                OnPropertyChanged();
            }
        }

        public DifficultyLevel SelectedDifficulty
        {
            get { return _selectedDifficulty; }
            set { _selectedDifficulty = value; OnPropertyChanged(); }
        }

        public Color SelectedSectorColor
        {
            get { return _selectedSectorColor; }
            set { _selectedSectorColor = value; OnPropertyChanged(); }
        }

        public Color DefaultSectorColor
        {
            get { return _defaultSectorColor; }
            set { _defaultSectorColor = value; OnPropertyChanged(); }
        }

        public bool IsCheckVisible
        {
            get { return _isCheckVisible; }
            set { _isCheckVisible = value; OnPropertyChanged(); }
        }

        public bool IsShowAnswerVisible
        {
            get { return _isShowAnswerVisible; }
            set { _isShowAnswerVisible = value; OnPropertyChanged(); }
        }

        public string FeedbackText
        {
            get { return _feedbackText; }
            set { _feedbackText = value; OnPropertyChanged(); }
        }

        public Brush FeedbackBrush
        {
            get { return _feedbackBrush; }
            set { _feedbackBrush = value; OnPropertyChanged(); }
        }

        public bool IsShaking
        {
            get { return _isShaking; }
            set { _isShaking = value; OnPropertyChanged(); }
        }

        public bool ShowCheckmark
        {
            get { return _showCheckmark; }
            set { _showCheckmark = value; OnPropertyChanged(); }
        }

        public ShapeType CurrentShape
        {
            get { return _currentShape; }
            set { _currentShape = value; OnPropertyChanged(); }
        }

        public FractionPuzzle? CurrentPuzzle
        {
            get { return _currentPuzzle; }
            set { _currentPuzzle = value; OnPropertyChanged(); }
        }

        public string UserNumeratorInput
        {
            get { return _userNumeratorInput; }
            set { _userNumeratorInput = value; OnPropertyChanged(); }
        }

        public string UserDenominatorInput
        {
            get { return _userDenominatorInput; }
            set { _userDenominatorInput = value; OnPropertyChanged(); }
        }

        public int SelectedCompareIndex
        {
            get { return _selectedCompareIndex; }
            set { _selectedCompareIndex = value; OnPropertyChanged(); }
        }

        public string CompleteToWholeInput
        {
            get { return _completeToWholeInput; }
            set { _completeToWholeInput = value; OnPropertyChanged(); }
        }

        public int SelectedEquivalentIndex
        {
            get { return _selectedEquivalentIndex; }
            set { _selectedEquivalentIndex = value; OnPropertyChanged(); }
        }

        public bool ShowFractionInput
        {
            get { return _showFractionInput; }
            set { _showFractionInput = value; OnPropertyChanged(); }
        }

        public bool ShowCompareOptions
        {
            get { return _showCompareOptions; }
            set { _showCompareOptions = value; OnPropertyChanged(); }
        }

        public bool ShowCompleteToWhole
        {
            get { return _showCompleteToWhole; }
            set { _showCompleteToWhole = value; OnPropertyChanged(); }
        }

        public bool ShowEquivalentOptions
        {
            get { return _showEquivalentOptions; }
            set { _showEquivalentOptions = value; OnPropertyChanged(); }
        }

        public bool ShowShapeCanvas
        {
            get { return _showShapeCanvas; }
            set { _showShapeCanvas = value; OnPropertyChanged(); }
        }

        public bool ShowPairsBoard
        {
            get { return _showPairsBoard; }
            set { _showPairsBoard = value; OnPropertyChanged(); }
        }

        public List<string> CompareOptions
        {
            get { return _compareOptions; }
            set { _compareOptions = value; OnPropertyChanged(); }
        }

        public List<string> EquivalentOptions
        {
            get { return _equivalentOptions; }
            set { _equivalentOptions = value; OnPropertyChanged(); }
        }

        public List<PairCardViewModel> PairCards
        {
            get { return _pairCards; }
            set { _pairCards = value; OnPropertyChanged(); }
        }

        public RelayCommand NewPuzzleCommand { get; }
        public RelayCommand ResetCommand { get; }
        public RelayCommand CheckAnswerCommand { get; }
        public RelayCommand ShowAnswerCommand { get; }
        public RelayCommand<int> ToggleSectorCommand { get; }
        public RelayCommand<int> SelectPairCardCommand { get; }

        public TrainerViewModel(
            IPuzzleGenerator puzzleGenerator,
            ISettingsService settingsService)
        {
            _puzzleGenerator = puzzleGenerator;
            _settingsService = settingsService;

            AppSettings settings = _settingsService.Load();
            SelectedDifficulty = settings.DefaultDifficulty;
            SelectedMode = settings.DefaultMode;

            _feedbackTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1.5)
            };
            _feedbackTimer.Tick += FeedbackTimer_Tick;

            NewPuzzleCommand = new RelayCommand(ExecuteNewPuzzle);
            ResetCommand = new RelayCommand(ExecuteReset);
            CheckAnswerCommand = new RelayCommand(ExecuteCheckAnswer);
            ShowAnswerCommand = new RelayCommand(ExecuteShowAnswer);
            ToggleSectorCommand = new RelayCommand<int>(ExecuteToggleSector);
            SelectPairCardCommand = new RelayCommand<int>(ExecuteSelectPairCard);

            RefreshAccentColor();
            ExecuteNewPuzzle();
        }

        public void RefreshAccentColor()
        {
            AppSettings settings = _settingsService.Load();
            if (TryParseHexColor(settings.AccentColor, out Color color))
            {
                SelectedSectorColor = color;
            }
        }

        private static bool TryParseHexColor(string hex, out Color color)
        {
            color = Colors.DodgerBlue;

            if (string.IsNullOrEmpty(hex) || hex.Length < 7)
            {
                return false;
            }

            try
            {
                string cleanHex = hex.StartsWith("#") ? hex.Substring(1) : hex;
                byte r = Convert.ToByte(cleanHex.Substring(0, 2), 16);
                byte g = Convert.ToByte(cleanHex.Substring(2, 2), 16);
                byte b = Convert.ToByte(cleanHex.Substring(4, 2), 16);
                color = Color.FromRgb(r, g, b);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ExecuteNewPuzzle()
        {
            _currentPuzzleType = (PuzzleType)new Random().Next(0, 6);

            ResetInputState();

            switch (_currentPuzzleType)
            {
                case PuzzleType.BuildFraction:
                    GenerateBuildFraction();
                    break;
                case PuzzleType.IdentifyFraction:
                    GenerateIdentifyFraction();
                    break;
                case PuzzleType.CompareFractions:
                    GenerateCompareFractions();
                    break;
                case PuzzleType.CompleteToWhole:
                    GenerateCompleteToWhole();
                    break;
                case PuzzleType.FindEquivalent:
                    GenerateFindEquivalent();
                    break;
                case PuzzleType.FindPairs:
                    GenerateFindPairs();
                    break;
            }
        }

        private void ResetInputState()
        {
            UserNumeratorInput = string.Empty;
            UserDenominatorInput = string.Empty;
            SelectedCompareIndex = -1;
            CompleteToWholeInput = string.Empty;
            SelectedEquivalentIndex = -1;
            FeedbackText = string.Empty;
            FeedbackBrush = Brushes.Transparent;
            ShowCheckmark = false;
            IsShaking = false;
            ShowFractionInput = false;
            ShowCompareOptions = false;
            ShowCompleteToWhole = false;
            ShowEquivalentOptions = false;
            ShowShapeCanvas = true;
            ShowPairsBoard = false;
            _firstSelectedPairIndex = -1;
        }

        private void GenerateBuildFraction()
        {
            _currentPuzzle = _puzzleGenerator.GenerateWithType(SelectedDifficulty, PuzzleType.BuildFraction);
            _currentComparePuzzle = null;
            _currentCompletePuzzle = null;
            _currentEquivalentPuzzle = null;
            _currentPairsPuzzle = null;

            TotalSectors = _currentPuzzle.TotalSectors;
            CurrentShape = _currentPuzzle.Shape;
            SelectedSectors = new HashSet<int>();
            TargetFractionText = $"Собери дробь: {_currentPuzzle.TargetFraction}";
            QuestionText = "Выбери нужное число долей на фигуре";
            CurrentFractionText = "Сейчас: 0/" + TotalSectors;
            ShowShapeCanvas = true;
        }

        private void GenerateIdentifyFraction()
        {
            _currentPuzzle = _puzzleGenerator.GenerateWithType(SelectedDifficulty, PuzzleType.IdentifyFraction);
            _currentComparePuzzle = null;
            _currentCompletePuzzle = null;
            _currentEquivalentPuzzle = null;
            _currentPairsPuzzle = null;

            TotalSectors = _currentPuzzle.TotalSectors;
            CurrentShape = _currentPuzzle.Shape;
            SelectedSectors = new HashSet<int>();

            int numToFill = _currentPuzzle.TargetFraction.Numerator;
            for (int i = 0; i < numToFill; i++)
            {
                _currentPuzzle.ToggleSector(i);
            }
            SelectedSectors = _currentPuzzle.SelectedSectors;

            TargetFractionText = "Какая дробь показана?";
            QuestionText = "Введи числитель и знаменатель";
            CurrentFractionText = "";
            ShowShapeCanvas = true;
            ShowFractionInput = true;
        }

        private void GenerateCompareFractions()
        {
            _currentPuzzle = null;
            _currentCompletePuzzle = null;
            _currentEquivalentPuzzle = null;
            _currentPairsPuzzle = null;

            _currentComparePuzzle = _puzzleGenerator.GenerateCompare(SelectedDifficulty);

            TargetFractionText = "Сравни дроби";
            QuestionText = $"Какая дробь больше: {_currentComparePuzzle.FractionA} или {_currentComparePuzzle.FractionB}?";
            CurrentFractionText = "";
            ShowShapeCanvas = false;
            ShowCompareOptions = true;
            CompareOptions = new List<string>
            {
                _currentComparePuzzle.FractionA.ToString(),
                _currentComparePuzzle.FractionB.ToString()
            };
        }

        private void GenerateCompleteToWhole()
        {
            _currentPuzzle = null;
            _currentComparePuzzle = null;
            _currentEquivalentPuzzle = null;
            _currentPairsPuzzle = null;

            _currentCompletePuzzle = _puzzleGenerator.GenerateCompleteToWhole(SelectedDifficulty);

            TotalSectors = _currentCompletePuzzle.GivenFraction.Denominator;
            CurrentShape = ShapeType.Circle;
            SelectedSectors = new HashSet<int>();
            for (int i = 0; i < _currentCompletePuzzle.GivenFraction.Numerator; i++)
            {
                SelectedSectors.Add(i);
            }

            TargetFractionText = "Дополни до целого";
            QuestionText = $"Дробь {_currentCompletePuzzle.GivenFraction}: сколько долей не хватает до целого?";
            CurrentFractionText = "";
            ShowShapeCanvas = true;
            ShowCompleteToWhole = true;
        }

        private void GenerateFindEquivalent()
        {
            _currentPuzzle = null;
            _currentComparePuzzle = null;
            _currentCompletePuzzle = null;
            _currentPairsPuzzle = null;

            _currentEquivalentPuzzle = _puzzleGenerator.GenerateEquivalent(SelectedDifficulty);

            TargetFractionText = "Найди равную дробь";
            QuestionText = $"Какая из дробей равна {_currentEquivalentPuzzle.TargetFraction}?";
            CurrentFractionText = "";
            ShowShapeCanvas = false;
            ShowEquivalentOptions = true;
            EquivalentOptions = _currentEquivalentPuzzle.Options.Select(o => o.ToString()).ToList();
        }

        private void GenerateFindPairs()
        {
            _currentPuzzle = null;
            _currentComparePuzzle = null;
            _currentCompletePuzzle = null;
            _currentEquivalentPuzzle = null;

            _currentPairsPuzzle = _puzzleGenerator.GenerateFindPairs(SelectedDifficulty);

            TargetFractionText = "Найди пары";
            QuestionText = "Нажми на две фигуры с одинаковыми дробями";
            CurrentFractionText = "";
            ShowShapeCanvas = false;
            ShowPairsBoard = true;

            PairCards = _currentPairsPuzzle.Cards.Select(c => new PairCardViewModel
            {
                Fraction = c.Fraction,
                FractionText = c.Fraction.ToString(),
                Shape = c.Shape,
                TotalSectors = c.TotalSectors,
                HighlightedSectors = c.HighlightedSectors,
                IsSelected = false,
                IsMatched = false,
                SelectedSectorColor = SelectedSectorColor,
                DefaultSectorColor = DefaultSectorColor
            }).ToList();
        }

        public void ExecuteReset()
        {
            ResetInputState();

            switch (_currentPuzzleType)
            {
                case PuzzleType.BuildFraction:
                    if (_currentPuzzle != null)
                    {
                        _currentPuzzle.Reset();
                        SelectedSectors = new HashSet<int>();
                        CurrentFractionText = "Сейчас: 0/" + TotalSectors;
                    }
                    break;
                case PuzzleType.IdentifyFraction:
                    QuestionText = "Введи числитель и знаменатель";
                    ShowFractionInput = true;
                    break;
                case PuzzleType.CompareFractions:
                    SelectedCompareIndex = -1;
                    break;
                case PuzzleType.CompleteToWhole:
                    CompleteToWholeInput = string.Empty;
                    break;
                case PuzzleType.FindEquivalent:
                    SelectedEquivalentIndex = -1;
                    break;
                case PuzzleType.FindPairs:
                    ShowPairsBoard = true;
                    GenerateFindPairs();
                    break;
            }
        }

        public void ExecuteCheckAnswer()
        {
            bool isCorrect = false;

            switch (_currentPuzzleType)
            {
                case PuzzleType.BuildFraction:
                    if (_currentPuzzle != null)
                    {
                        isCorrect = _currentPuzzle.IsSolved;
                    }
                    break;

                case PuzzleType.IdentifyFraction:
                    if (_currentPuzzle != null &&
                        int.TryParse(UserNumeratorInput, out int num) &&
                        int.TryParse(UserDenominatorInput, out int den) &&
                        den > 0)
                    {
                        Fraction userAnswer = new Fraction(num, den);
                        isCorrect = userAnswer.IsEquivalentTo(_currentPuzzle.TargetFraction);
                    }
                    break;

                case PuzzleType.CompareFractions:
                    if (_currentComparePuzzle != null && SelectedCompareIndex >= 0)
                    {
                        Fraction selected = SelectedCompareIndex == 0
                            ? _currentComparePuzzle.FractionA
                            : _currentComparePuzzle.FractionB;
                        isCorrect = _currentComparePuzzle.IsCorrect(selected);
                    }
                    break;

                case PuzzleType.CompleteToWhole:
                    if (_currentCompletePuzzle != null &&
                        int.TryParse(CompleteToWholeInput, out int cNum))
                    {
                        isCorrect = _currentCompletePuzzle.IsCorrect(cNum);
                    }
                    break;

                case PuzzleType.FindEquivalent:
                    if (_currentEquivalentPuzzle != null && SelectedEquivalentIndex >= 0)
                    {
                        Fraction selected = _currentEquivalentPuzzle.Options[SelectedEquivalentIndex];
                        isCorrect = _currentEquivalentPuzzle.IsCorrect(selected);
                    }
                    break;
            }

            _totalCount++;

            if (isCorrect)
            {
                _correctCount++;
                FeedbackText = "Правильно!";
                FeedbackBrush = Brushes.Green;
                ShowCheckmark = true;
                IsShaking = false;
            }
            else
            {
                FeedbackText = GetCorrectAnswerText();
                FeedbackBrush = Brushes.Red;
                ShowCheckmark = false;
                IsShaking = true;
            }

            ScoreText = $"Правильно: {_correctCount} / {_totalCount}";
            _feedbackTimer.Start();
        }

        private string GetCorrectAnswerText()
        {
            return _currentPuzzleType switch
            {
                PuzzleType.BuildFraction => _currentPuzzle != null
                    ? $"Неверно. Правильный ответ: {_currentPuzzle.TargetFraction}"
                    : "Неверно",
                PuzzleType.IdentifyFraction => _currentPuzzle != null
                    ? $"Неверно. Правильный ответ: {_currentPuzzle.TargetFraction}"
                    : "Неверно",
                PuzzleType.CompareFractions => _currentComparePuzzle != null
                    ? $"Неверно. Больше: {_currentComparePuzzle.LargerFraction}"
                    : "Неверно",
                PuzzleType.CompleteToWhole => _currentCompletePuzzle != null
                    ? $"Неверно. Нужно: {_currentCompletePuzzle.NeededFraction.Numerator}"
                    : "Неверно",
                PuzzleType.FindEquivalent => _currentEquivalentPuzzle != null
                    ? $"Неверно. Правильный ответ: {_currentEquivalentPuzzle.CorrectAnswer}"
                    : "Неверно",
                _ => "Неверно"
            };
        }

        public void ExecuteShowAnswer()
        {
            switch (_currentPuzzleType)
            {
                case PuzzleType.BuildFraction:
                    ShowBuildFractionAnswer();
                    break;
                case PuzzleType.IdentifyFraction:
                    ShowIdentifyFractionAnswer();
                    break;
                case PuzzleType.CompareFractions:
                    ShowCompareAnswer();
                    break;
                case PuzzleType.CompleteToWhole:
                    ShowCompleteToWholeAnswer();
                    break;
                case PuzzleType.FindEquivalent:
                    ShowEquivalentAnswer();
                    break;
                case PuzzleType.FindPairs:
                    ShowPairsAnswer();
                    break;
            }
        }

        private void ShowBuildFractionAnswer()
        {
            if (_currentPuzzle == null) return;

            HashSet<int> correctSectors = new HashSet<int>();
            for (int i = 0; i < _currentPuzzle.TargetFraction.Numerator; i++)
            {
                correctSectors.Add(i);
            }

            SelectedSectors = correctSectors;
            CurrentFractionText = $"Сейчас: {_currentPuzzle.TargetFraction.Numerator}/{TotalSectors}";
            FeedbackText = $"Правильный ответ: {_currentPuzzle.TargetFraction}";
            FeedbackBrush = new SolidColorBrush(Color.FromRgb(74, 144, 217));
        }

        private void ShowIdentifyFractionAnswer()
        {
            if (_currentPuzzle == null) return;
            FeedbackText = $"Правильный ответ: {_currentPuzzle.TargetFraction}";
            FeedbackBrush = new SolidColorBrush(Color.FromRgb(74, 144, 217));
        }

        private void ShowCompareAnswer()
        {
            if (_currentComparePuzzle == null) return;
            FeedbackText = $"Правильный ответ: {_currentComparePuzzle.LargerFraction}";
            FeedbackBrush = new SolidColorBrush(Color.FromRgb(74, 144, 217));
        }

        private void ShowCompleteToWholeAnswer()
        {
            if (_currentCompletePuzzle == null) return;
            FeedbackText = $"Нужно ещё: {_currentCompletePuzzle.NeededFraction.Numerator} долей";
            FeedbackBrush = new SolidColorBrush(Color.FromRgb(74, 144, 217));
        }

        private void ShowEquivalentAnswer()
        {
            if (_currentEquivalentPuzzle == null) return;
            FeedbackText = $"Правильный ответ: {_currentEquivalentPuzzle.CorrectAnswer}";
            FeedbackBrush = new SolidColorBrush(Color.FromRgb(74, 144, 217));
        }

        private void ShowPairsAnswer()
        {
            if (_currentPairsPuzzle == null) return;
            var pair = _currentPairsPuzzle.CorrectPair;
            FeedbackText = $"Пара: {pair.FractionA} и {pair.FractionB}";
            FeedbackBrush = new SolidColorBrush(Color.FromRgb(74, 144, 217));

            foreach (var card in PairCards)
            {
                if (card.Fraction.IsEquivalentTo(pair.FractionA) || card.Fraction.IsEquivalentTo(pair.FractionB))
                {
                    card.IsSelected = true;
                    card.IsMatched = true;
                }
            }
        }

        public void ExecuteToggleSector(int sectorIndex)
        {
            if (_currentPuzzle == null || _currentPuzzleType != PuzzleType.BuildFraction)
            {
                return;
            }

            if (sectorIndex < 0 || sectorIndex >= TotalSectors)
            {
                return;
            }

            _currentPuzzle.ToggleSector(sectorIndex);

            HashSet<int> newSelected = new HashSet<int>(_currentPuzzle.SelectedSectors);
            SelectedSectors = newSelected;

            Fraction currentFraction = _currentPuzzle.GetCurrentFraction();
            CurrentFractionText = $"Сейчас: {currentFraction}";
        }

        public void ExecuteSelectPairCard(int index)
        {
            if (_currentPuzzleType != PuzzleType.FindPairs || _currentPairsPuzzle == null)
                return;

            if (index < 0 || index >= PairCards.Count)
                return;

            var card = PairCards[index];
            if (card.IsMatched)
                return;

            if (_firstSelectedPairIndex == -1)
            {
                _firstSelectedPairIndex = index;
                card.IsSelected = true;
                FeedbackText = "Выбери вторую фигуру";
                FeedbackBrush = new SolidColorBrush(Color.FromRgb(74, 144, 217));
            }
            else
            {
                if (_firstSelectedPairIndex == index)
                {
                    card.IsSelected = false;
                    _firstSelectedPairIndex = -1;
                    FeedbackText = string.Empty;
                    return;
                }

                var firstCard = PairCards[_firstSelectedPairIndex];

                if (firstCard.Fraction.IsEquivalentTo(card.Fraction))
                {
                    firstCard.IsMatched = true;
                    card.IsMatched = true;
                    firstCard.IsSelected = true;
                    card.IsSelected = true;

                    _correctCount++;
                    _totalCount++;
                    FeedbackText = $"{firstCard.FractionText} = {card.FractionText} — верно!";
                    FeedbackBrush = Brushes.Green;

                    _firstSelectedPairIndex = -1;

                    bool allMatched = PairCards.All(c => c.IsMatched);
                    if (allMatched)
                    {
                        ShowCheckmark = true;
                        FeedbackText = "Все пары найдены!";
                        FeedbackBrush = Brushes.Green;
                    }
                }
                else
                {
                    _totalCount++;
                    FeedbackText = $"{firstCard.FractionText} ≠ {card.FractionText} — попробуй снова";
                    FeedbackBrush = Brushes.Red;
                    IsShaking = true;

                    firstCard.IsSelected = false;
                    card.IsSelected = false;
                    _firstSelectedPairIndex = -1;

                    _feedbackTimer.Start();
                }

                ScoreText = $"Правильно: {_correctCount} / {_totalCount}";
            }
        }

        private void FeedbackTimer_Tick(object? sender, EventArgs e)
        {
            _feedbackTimer.Stop();
            FeedbackText = string.Empty;
            FeedbackBrush = Brushes.Transparent;
            ShowCheckmark = false;
            IsShaking = false;
        }
    }

    public class PairCardViewModel : ObservableObject
    {
        private bool _isSelected;
        private bool _isMatched;

        public Fraction Fraction { get; set; } = new Fraction(1, 2);
        public string FractionText { get; set; } = "";
        public ShapeType Shape { get; set; }
        public int TotalSectors { get; set; }
        public int HighlightedSectors { get; set; }
        public Color SelectedSectorColor { get; set; } = Colors.DodgerBlue;
        public Color DefaultSectorColor { get; set; } = Colors.LightGray;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(); OnPropertyChanged(nameof(BorderBrush)); }
        }

        public bool IsMatched
        {
            get { return _isMatched; }
            set { _isMatched = value; OnPropertyChanged(); OnPropertyChanged(nameof(BorderBrush)); }
        }

        public Brush BorderBrush
        {
            get
            {
                if (IsMatched)
                    return new SolidColorBrush(Colors.Green);
                if (IsSelected)
                    return new SolidColorBrush(Colors.DodgerBlue);
                return (Brush)Application.Current.TryFindResource("BorderColor") ?? Brushes.Gray;
            }
        }
    }
}
