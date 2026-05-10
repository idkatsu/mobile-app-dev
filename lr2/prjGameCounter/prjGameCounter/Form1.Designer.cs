namespace prjGameCounter
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblStats = new Label();
            lblExpression = new Label();
            btnTrue = new Button();
            btnFalse = new Button();
            lblQuestionNum = new Label();
            lblCoins = new Label();
            lblTime = new Label();
            cmbDifficulty = new ComboBox();
            gameTimer = new System.Windows.Forms.Timer(components);
            grpOperations = new GroupBox();
            chkDivision = new CheckBox();
            chkMultiplication = new CheckBox();
            chkSubtraction = new CheckBox();
            chkAddition = new CheckBox();
            btnNewGame = new Button();
            lblResultFeedback = new Label();
            label1 = new Label();
            grpOperations.SuspendLayout();
            SuspendLayout();

            lblStats.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblStats.AutoSize = true;
            lblStats.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblStats.ForeColor = Color.LightGray;
            lblStats.Location = new Point(645, 24);
            lblStats.Margin = new Padding(5, 0, 5, 0);
            lblStats.Name = "lblStats";
            lblStats.Size = new Size(319, 41);
            lblStats.TabIndex = 0;
            lblStats.Text = "Верно: 0 | Неверно: 0";
            lblStats.TextAlign = ContentAlignment.MiddleRight;

            lblExpression.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblExpression.Font = new Font("Segoe UI", 48F, FontStyle.Bold);
            lblExpression.ForeColor = Color.WhiteSmoke;
            lblExpression.Location = new Point(20, 160);
            lblExpression.Margin = new Padding(5, 0, 5, 0);
            lblExpression.Name = "lblExpression";
            lblExpression.Size = new Size(923, 160);
            lblExpression.TabIndex = 1;
            lblExpression.Text = "1 + 1 = 3";
            lblExpression.TextAlign = ContentAlignment.MiddleCenter;

            btnTrue.Anchor = AnchorStyles.Bottom;
            btnTrue.BackColor = Color.SeaGreen;
            btnTrue.Cursor = Cursors.Hand;
            btnTrue.FlatAppearance.BorderSize = 0;
            btnTrue.FlatStyle = FlatStyle.Flat;
            btnTrue.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            btnTrue.ForeColor = Color.White;
            btnTrue.Location = new Point(98, 464);
            btnTrue.Margin = new Padding(5, 5, 5, 5);
            btnTrue.Name = "btnTrue";
            btnTrue.Size = new Size(358, 112);
            btnTrue.TabIndex = 2;
            btnTrue.Text = "✅ Верно";
            btnTrue.UseVisualStyleBackColor = false;
            btnTrue.Click += btnTrue_Click;

            btnFalse.Anchor = AnchorStyles.Bottom;
            btnFalse.BackColor = Color.IndianRed;
            btnFalse.Cursor = Cursors.Hand;
            btnFalse.FlatAppearance.BorderSize = 0;
            btnFalse.FlatStyle = FlatStyle.Flat;
            btnFalse.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            btnFalse.ForeColor = Color.White;
            btnFalse.Location = new Point(504, 464);
            btnFalse.Margin = new Padding(5, 5, 5, 5);
            btnFalse.Name = "btnFalse";
            btnFalse.Size = new Size(358, 112);
            btnFalse.TabIndex = 3;
            btnFalse.Text = "❌ Неверно";
            btnFalse.UseVisualStyleBackColor = false;
            btnFalse.Click += btnFalse_Click;

            lblQuestionNum.AutoSize = true;
            lblQuestionNum.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblQuestionNum.ForeColor = Color.LightGray;
            lblQuestionNum.Location = new Point(24, 24);
            lblQuestionNum.Margin = new Padding(5, 0, 5, 0);
            lblQuestionNum.Name = "lblQuestionNum";
            lblQuestionNum.Size = new Size(192, 41);
            lblQuestionNum.TabIndex = 4;
            lblQuestionNum.Text = "Вопрос №: 1";

            lblCoins.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblCoins.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblCoins.ForeColor = Color.Gold;
            lblCoins.Location = new Point(20, 64);
            lblCoins.Margin = new Padding(5, 0, 5, 0);
            lblCoins.Name = "lblCoins";
            lblCoins.Size = new Size(923, 48);
            lblCoins.TabIndex = 5;
            lblCoins.Text = "💰 Монеты: 0";
            lblCoins.TextAlign = ContentAlignment.MiddleCenter;

            lblTime.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblTime.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTime.ForeColor = Color.White;
            lblTime.Location = new Point(20, 112);
            lblTime.Margin = new Padding(5, 0, 5, 0);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(923, 68);
            lblTime.TabIndex = 6;
            lblTime.Text = "⏳ 10";
            lblTime.TextAlign = ContentAlignment.MiddleCenter;

            cmbDifficulty.Anchor = AnchorStyles.Bottom;
            cmbDifficulty.BackColor = Color.FromArgb(50, 50, 50);
            cmbDifficulty.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDifficulty.FlatStyle = FlatStyle.Flat;
            cmbDifficulty.Font = new Font("Segoe UI", 10F);
            cmbDifficulty.ForeColor = Color.White;
            cmbDifficulty.FormattingEnabled = true;
            cmbDifficulty.Location = new Point(341, 672);
            cmbDifficulty.Margin = new Padding(5, 5, 5, 5);
            cmbDifficulty.Name = "cmbDifficulty";
            cmbDifficulty.Size = new Size(274, 45);
            cmbDifficulty.TabIndex = 7;
            cmbDifficulty.SelectedIndexChanged += cmbDifficulty_SelectedIndexChanged;

            gameTimer.Interval = 1000;
            gameTimer.Tick += gameTimer_Tick;

            grpOperations.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            grpOperations.Controls.Add(chkDivision);
            grpOperations.Controls.Add(chkMultiplication);
            grpOperations.Controls.Add(chkSubtraction);
            grpOperations.Controls.Add(chkAddition);
            grpOperations.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            grpOperations.ForeColor = Color.LightGray;
            grpOperations.Location = new Point(24, 608);
            grpOperations.Margin = new Padding(5, 5, 5, 5);
            grpOperations.Name = "grpOperations";
            grpOperations.Padding = new Padding(5, 5, 5, 5);
            grpOperations.Size = new Size(228, 192);
            grpOperations.TabIndex = 8;
            grpOperations.TabStop = false;
            grpOperations.Text = "Операции";

            chkDivision.AutoSize = true;
            chkDivision.Location = new Point(10, 147);
            chkDivision.Margin = new Padding(5, 5, 5, 5);
            chkDivision.Name = "chkDivision";
            chkDivision.Size = new Size(144, 36);
            chkDivision.TabIndex = 3;
            chkDivision.Text = "Деление";
            chkDivision.UseVisualStyleBackColor = true;
            chkDivision.CheckedChanged += chkOperation_CheckedChanged;

            chkMultiplication.AutoSize = true;
            chkMultiplication.Location = new Point(10, 109);
            chkMultiplication.Margin = new Padding(5, 5, 5, 5);
            chkMultiplication.Name = "chkMultiplication";
            chkMultiplication.Size = new Size(178, 36);
            chkMultiplication.TabIndex = 2;
            chkMultiplication.Text = "Умножение";
            chkMultiplication.UseVisualStyleBackColor = true;
            chkMultiplication.CheckedChanged += chkOperation_CheckedChanged;

            chkSubtraction.AutoSize = true;
            chkSubtraction.Location = new Point(10, 70);
            chkSubtraction.Margin = new Padding(5, 5, 5, 5);
            chkSubtraction.Name = "chkSubtraction";
            chkSubtraction.Size = new Size(171, 36);
            chkSubtraction.TabIndex = 1;
            chkSubtraction.Text = "Вычитание";
            chkSubtraction.UseVisualStyleBackColor = true;
            chkSubtraction.CheckedChanged += chkOperation_CheckedChanged;

            chkAddition.AutoSize = true;
            chkAddition.Checked = true;
            chkAddition.CheckState = CheckState.Checked;
            chkAddition.Location = new Point(10, 32);
            chkAddition.Margin = new Padding(5, 5, 5, 5);
            chkAddition.Name = "chkAddition";
            chkAddition.Size = new Size(161, 36);
            chkAddition.TabIndex = 0;
            chkAddition.Text = "Сложение";
            chkAddition.UseVisualStyleBackColor = true;
            chkAddition.CheckedChanged += chkOperation_CheckedChanged;

            btnNewGame.Anchor = AnchorStyles.Bottom;
            btnNewGame.BackColor = Color.DimGray;
            btnNewGame.Cursor = Cursors.Hand;
            btnNewGame.FlatAppearance.BorderSize = 0;
            btnNewGame.FlatStyle = FlatStyle.Flat;
            btnNewGame.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnNewGame.ForeColor = Color.WhiteSmoke;
            btnNewGame.Location = new Point(341, 736);
            btnNewGame.Margin = new Padding(5, 5, 5, 5);
            btnNewGame.Name = "btnNewGame";
            btnNewGame.Size = new Size(276, 64);
            btnNewGame.TabIndex = 9;
            btnNewGame.Text = "Новая игра";
            btnNewGame.UseVisualStyleBackColor = false;
            btnNewGame.Click += btnNewGame_Click;

            lblResultFeedback.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblResultFeedback.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblResultFeedback.ForeColor = Color.LimeGreen;
            lblResultFeedback.Location = new Point(20, 352);
            lblResultFeedback.Margin = new Padding(5, 0, 5, 0);
            lblResultFeedback.Name = "lblResultFeedback";
            lblResultFeedback.Size = new Size(923, 64);
            lblResultFeedback.TabIndex = 10;
            lblResultFeedback.TextAlign = ContentAlignment.MiddleCenter;

            label1.Anchor = AnchorStyles.Bottom;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            label1.ForeColor = Color.LightGray;
            label1.Location = new Point(341, 635);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(143, 32);
            label1.TabIndex = 11;
            label1.Text = "Сложность:";

            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(962, 827);
            Controls.Add(label1);
            Controls.Add(lblResultFeedback);
            Controls.Add(btnNewGame);
            Controls.Add(grpOperations);
            Controls.Add(cmbDifficulty);
            Controls.Add(lblTime);
            Controls.Add(lblCoins);
            Controls.Add(lblQuestionNum);
            Controls.Add(btnFalse);
            Controls.Add(btnTrue);
            Controls.Add(lblExpression);
            Controls.Add(lblStats);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(5, 5, 5, 5);
            MaximizeBox = false;
            Name = "Form1";
            Text = "Устный счет - PRO";
            Load += Form1_Load;
            grpOperations.ResumeLayout(false);
            grpOperations.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.Label lblExpression;
        private System.Windows.Forms.Button btnTrue;
        private System.Windows.Forms.Button btnFalse;
        private System.Windows.Forms.Label lblQuestionNum;
        private System.Windows.Forms.Label lblCoins;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.ComboBox cmbDifficulty;
        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.GroupBox grpOperations;
        private System.Windows.Forms.CheckBox chkDivision;
        private System.Windows.Forms.CheckBox chkMultiplication;
        private System.Windows.Forms.CheckBox chkSubtraction;
        private System.Windows.Forms.CheckBox chkAddition;
        private System.Windows.Forms.Button btnNewGame;
        private System.Windows.Forms.Label lblResultFeedback;
        private System.Windows.Forms.Label label1;
    }
}