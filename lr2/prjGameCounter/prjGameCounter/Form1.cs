using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace prjGameCounter
{
    public partial class Form1 : Form
    {
        private Game game;
        private int _timeLeft;
        private const int INITIAL_TIME = 10;

        public Form1()
        {
            InitializeComponent();
            game = new Game();
            game.GameStateChanged += Game_GameStateChanged;

            InitializeDifficultyOptions();
            InitializeOperationCheckboxes();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartNewGame();
            UpdateUI();
        }

        private void InitializeDifficultyOptions()
        {
            cmbDifficulty.Items.Clear();
            cmbDifficulty.Items.Add(new { Text = "Легкий (до 10)", Value = DifficultyLevel.Easy_10 });
            cmbDifficulty.Items.Add(new { Text = "Средний (до 20)", Value = DifficultyLevel.Medium_20 });
            cmbDifficulty.Items.Add(new { Text = "Сложный (до 50)", Value = DifficultyLevel.Hard_50 });
            cmbDifficulty.Items.Add(new { Text = "Эксперт (до 100)", Value = DifficultyLevel.Expert_100 });
            cmbDifficulty.DisplayMember = "Text";
            cmbDifficulty.ValueMember = "Value";
            cmbDifficulty.SelectedIndex = 0;
        }

        private void InitializeOperationCheckboxes()
        {
            chkAddition.Checked = true;
            chkSubtraction.Checked = false;
            chkMultiplication.Checked = false;
            chkDivision.Checked = false;

            game.EnabledOperations.Clear();
            game.AddOperation(OperationType.Addition);
        }

        private void StartNewGame()
        {
            game.ResetGame();
            SetDifficultyFromComboBox();
            ApplyOperationsFromCheckboxes();
            UpdateUI();

            lblResultFeedback.Text = "";
            ResetQuestionTimer();
        }

        private void Game_GameStateChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            lblQuestionNum.Text = $"Вопрос №: {game.QuestionNumber}";
            lblStats.Text = $"Верно: {game.CorrectAnswersCount} | Неверно: {game.IncorrectAnswersCount}";
            lblCoins.Text = $"💰 Монеты: {game.Coins}";

            if (game.CorrectStreak > 1)
                lblCoins.Text += $" (Комбо x{game.CorrectStreak}!)";
            else if (game.IncorrectStreak > 1)
                lblCoins.Text += $" (Штраф x{game.IncorrectStreak}!)";

            if (game.CurrentQuestion != null)
            {
                lblExpression.Text = game.CurrentQuestion.ExpressionText;
            }

            btnTrue.BackColor = Color.SeaGreen;
            btnFalse.BackColor = Color.IndianRed;
        }

        private void ProcessUserChoice(bool userBelievesStatementIsTrue)
        {
            gameTimer.Stop();
            bool isAnswerCorrect = game.CheckAnswer(userBelievesStatementIsTrue);

            if (isAnswerCorrect)
            {
                lblResultFeedback.Text = "✅ Верно!";
                lblResultFeedback.ForeColor = Color.LimeGreen;
            }
            else
            {
                lblResultFeedback.Text = $"❌ Неверно! (Предп. {game.CurrentQuestion.SuggestedAnswer}, Ист. {game.CurrentQuestion.CorrectResult})";
                lblResultFeedback.ForeColor = Color.Red;
            }

            System.Windows.Forms.Timer pauseTimer = new System.Windows.Forms.Timer { Interval = 700 };
            pauseTimer.Tick += (s, ev) =>
            {
                pauseTimer.Stop();
                pauseTimer.Dispose();
                lblResultFeedback.Text = "";
                game.GenerateNewQuestion();
                ResetQuestionTimer();
            };
            pauseTimer.Start();
        }

        private void btnTrue_Click(object sender, EventArgs e) => ProcessUserChoice(true);
        private void btnFalse_Click(object sender, EventArgs e) => ProcessUserChoice(false);

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            _timeLeft--;
            lblTime.Text = $"⏳ {_timeLeft}";

            if (_timeLeft <= 3)
            {
                lblTime.ForeColor = Color.Red;
            }
            else
            {
                lblTime.ForeColor = Color.White;
            }

            if (_timeLeft <= 0)
            {
                gameTimer.Stop();
                game.ProcessTimeout();
                lblResultFeedback.Text = $"Время вышло! Ист. ответ: {game.CurrentQuestion.CorrectResult}";
                lblResultFeedback.ForeColor = Color.Orange;

                System.Windows.Forms.Timer pauseTimer = new System.Windows.Forms.Timer { Interval = 1000 };
                pauseTimer.Tick += (s, ev) =>
                {
                    pauseTimer.Stop();
                    pauseTimer.Dispose();
                    lblResultFeedback.Text = "";
                    game.GenerateNewQuestion();
                    ResetQuestionTimer();
                };
                pauseTimer.Start();
            }
        }

        private void ResetQuestionTimer()
        {
            _timeLeft = INITIAL_TIME;
            lblTime.Text = $"⏳ {_timeLeft}";
            lblTime.ForeColor = Color.White;
            gameTimer.Start();
        }

        private void cmbDifficulty_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDifficultyFromComboBox();
            StartNewGame();
        }

        private void SetDifficultyFromComboBox()
        {
            if (cmbDifficulty.SelectedItem != null)
            {
                DifficultyLevel selectedDifficulty = (DifficultyLevel)(cmbDifficulty.SelectedItem as dynamic).Value;
                game.SetDifficulty(selectedDifficulty);
            }
        }

        private void chkOperation_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            OperationType opType;

            if (chk == chkAddition) opType = OperationType.Addition;
            else if (chk == chkSubtraction) opType = OperationType.Subtraction;
            else if (chk == chkMultiplication) opType = OperationType.Multiplication;
            else if (chk == chkDivision) opType = OperationType.Division;
            else return;

            if (!chk.Checked && game.EnabledOperations.Contains(opType) && game.EnabledOperations.Count == 1)
            {
                MessageBox.Show("Должна быть выбрана хотя бы одна операция.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                chk.Checked = true;
                return;
            }

            ApplyOperationsFromCheckboxes();
            StartNewGame();
        }

        private void ApplyOperationsFromCheckboxes()
        {
            game.EnabledOperations.Clear();
            if (chkAddition.Checked) game.AddOperation(OperationType.Addition);
            if (chkSubtraction.Checked) game.AddOperation(OperationType.Subtraction);
            if (chkMultiplication.Checked) game.AddOperation(OperationType.Multiplication);
            if (chkDivision.Checked) game.AddOperation(OperationType.Division);

            if (!game.EnabledOperations.Any())
            {
                game.AddOperation(OperationType.Addition);
                chkAddition.Checked = true;
            }
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }
    }
}