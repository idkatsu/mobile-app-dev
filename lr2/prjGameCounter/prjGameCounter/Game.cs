using System;
using System.Collections.Generic;
using System.Linq;

namespace prjGameCounter
{
    public enum OperationType
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    public enum DifficultyLevel
    {
        Easy_10,
        Medium_20,
        Hard_50,
        Expert_100
    }

    public class Question
    {
        public int Operand1 { get; private set; }
        public int Operand2 { get; private set; }
        public OperationType Operation { get; private set; }
        public int CorrectResult { get; private set; }
        public int SuggestedAnswer { get; private set; }
        public bool IsSuggestedAnswerTrue { get; private set; }
        public string ExpressionText { get; private set; }

        public Question(int operand1, int operand2, OperationType operation, int maxOperandValue, Random random)
        {
            Operand1 = operand1;
            Operand2 = operand2;
            Operation = operation;

            CalculateCorrectResult();
            GenerateSuggestedAnswer(maxOperandValue, random);
            ExpressionText = $"{Operand1} {GetOperationSymbol(Operation)} {Operand2} = {SuggestedAnswer}";
        }

        private void CalculateCorrectResult()
        {
            switch (Operation)
            {
                case OperationType.Addition:
                    CorrectResult = Operand1 + Operand2;
                    break;
                case OperationType.Subtraction:
                    CorrectResult = Operand1 - Operand2;
                    break;
                case OperationType.Multiplication:
                    CorrectResult = Operand1 * Operand2;
                    break;
                case OperationType.Division:
                    CorrectResult = Operand1 / Operand2;
                    break;
                default:
                    throw new InvalidOperationException("Неизвестный тип операции.");
            }
        }

        private void GenerateSuggestedAnswer(int maxOperandValue, Random random)
        {
            IsSuggestedAnswerTrue = random.Next(0, 2) == 0;
            SuggestedAnswer = CorrectResult;

            if (!IsSuggestedAnswerTrue)
            {
                int offset;
                do
                {
                    offset = random.Next(1, Math.Max(2, maxOperandValue / 5)) * (random.Next(0, 2) == 0 ? 1 : -1);
                } while (CorrectResult + offset == CorrectResult);

                SuggestedAnswer = CorrectResult + offset;
            }
        }

        private string GetOperationSymbol(OperationType operation)
        {
            switch (operation)
            {
                case OperationType.Addition: return "+";
                case OperationType.Subtraction: return "-";
                case OperationType.Multiplication: return "*";
                case OperationType.Division: return "/";
                default: return "?";
            }
        }
    }

    public class Game
    {
        private Random _random = new Random();

        public int CorrectAnswersCount { get; private set; }
        public int IncorrectAnswersCount { get; private set; }
        public int QuestionNumber { get; private set; }
        public int Coins { get; private set; }
        public int CorrectStreak { get; private set; }
        public int IncorrectStreak { get; private set; }

        public Question CurrentQuestion { get; private set; }

        private DifficultyLevel _currentDifficulty = DifficultyLevel.Easy_10;
        public List<OperationType> EnabledOperations { get; private set; } = new List<OperationType>();

        public event EventHandler GameStateChanged;

        public Game()
        {
            ResetGame();
        }

        public void ResetGame()
        {
            CorrectAnswersCount = 0;
            IncorrectAnswersCount = 0;
            QuestionNumber = 0;
            Coins = 0;
            CorrectStreak = 0;
            IncorrectStreak = 0;
            if (!EnabledOperations.Any())
            {
                EnabledOperations.Add(OperationType.Addition);
            }
            GenerateNewQuestion();
        }

        public void GenerateNewQuestion()
        {
            QuestionNumber++;

            int maxOperandValue = GetMaxOperandValue(_currentDifficulty);

            int operand1 = _random.Next(1, maxOperandValue + 1);
            int operand2 = _random.Next(1, maxOperandValue + 1);

            OperationType operation = EnabledOperations[_random.Next(EnabledOperations.Count)];

            if (operation == OperationType.Division)
            {
                if (operand2 == 0) operand2 = 1;
                operand1 = _random.Next(1, maxOperandValue / Math.Max(1, operand2) + 1) * operand2;
                if (operand1 == 0 && operand2 != 0) operand1 = operand2;
            }

            if (operation == OperationType.Subtraction && _random.Next(0, 2) == 0 && operand1 < operand2)
            {
                int temp = operand1;
                operand1 = operand2;
                operand2 = temp;
            }

            CurrentQuestion = new Question(operand1, operand2, operation, maxOperandValue, _random);
            OnGameStateChanged();
        }

        public bool CheckAnswer(bool userAnswerIsCorrectStatement)
        {
            bool isCorrect = (userAnswerIsCorrectStatement == CurrentQuestion.IsSuggestedAnswerTrue);

            if (isCorrect)
            {
                CorrectAnswersCount++;
                CorrectStreak++;
                IncorrectStreak = 0;

                int earnedCoins = (int)Math.Pow(2, Math.Max(0, CorrectStreak - 1));
                Coins += earnedCoins;
            }
            else
            {
                IncorrectAnswersCount++;
                IncorrectStreak++;
                CorrectStreak = 0;

                int lostCoins = (int)Math.Pow(2, Math.Max(0, IncorrectStreak - 1));
                Coins -= lostCoins;
            }

            OnGameStateChanged();
            return isCorrect;
        }

        public void ProcessTimeout()
        {
            IncorrectAnswersCount++;
            IncorrectStreak++;
            CorrectStreak = 0;

            int lostCoins = (int)Math.Pow(2, Math.Max(0, IncorrectStreak - 1));
            Coins -= lostCoins;
            OnGameStateChanged();
        }

        public void SetDifficulty(DifficultyLevel difficulty)
        {
            _currentDifficulty = difficulty;
            OnGameStateChanged();
        }

        private int GetMaxOperandValue(DifficultyLevel difficulty)
        {
            switch (difficulty)
            {
                case DifficultyLevel.Easy_10: return 10;
                case DifficultyLevel.Medium_20: return 20;
                case DifficultyLevel.Hard_50: return 50;
                case DifficultyLevel.Expert_100: return 100;
                default: return 10;
            }
        }

        public void AddOperation(OperationType operation)
        {
            if (!EnabledOperations.Contains(operation))
            {
                EnabledOperations.Add(operation);
                OnGameStateChanged();
            }
        }

        public void RemoveOperation(OperationType operation)
        {
            if (EnabledOperations.Contains(operation) && EnabledOperations.Count > 1)
            {
                EnabledOperations.Remove(operation);
                OnGameStateChanged();
            }
        }

        protected virtual void OnGameStateChanged()
        {
            GameStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}