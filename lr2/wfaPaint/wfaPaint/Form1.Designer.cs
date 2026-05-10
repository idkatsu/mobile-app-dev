namespace wfaPaint
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
            this.pxImage = new System.Windows.Forms.PictureBox();
            this.panelИнструменты = new System.Windows.Forms.Panel();
            this.panelРежимыРисования = new System.Windows.Forms.Panel();
            this.btnВыделение = new System.Windows.Forms.Button();
            this.btnЗвезда = new System.Windows.Forms.Button();
            this.btnТреугольник = new System.Windows.Forms.Button();
            this.btnПрямоугольник = new System.Windows.Forms.Button();
            this.btnЭллипс = new System.Windows.Forms.Button();
            this.btnЛиния = new System.Windows.Forms.Button();
            this.btnКарандаш = new System.Windows.Forms.Button();
            this.groupBoxНастройкиПера = new System.Windows.Forms.GroupBox();
            this.lblТолщинаПера = new System.Windows.Forms.Label();
            this.trackBarТолщинаПера = new System.Windows.Forms.TrackBar();
            this.panelЦветоваяПалитра = new System.Windows.Forms.Panel();
            this.panelЦветЧерный = new System.Windows.Forms.Panel();
            this.panelЦветПурпурный = new System.Windows.Forms.Panel();
            this.panelЦветЖелтый = new System.Windows.Forms.Panel();
            this.panelЦветСиний = new System.Windows.Forms.Panel();
            this.panelЦветЗеленый = new System.Windows.Forms.Panel();
            this.panelЦветКрасный = new System.Windows.Forms.Panel();
            this.panelТекущийЦвет = new System.Windows.Forms.Panel();
            this.btnВыбратьЦвет = new System.Windows.Forms.Button();
            this.panelОперацииСФайлом = new System.Windows.Forms.Panel();
            this.btnПовторить = new System.Windows.Forms.Button();
            this.btnОтменить = new System.Windows.Forms.Button();
            this.btnЗагрузить = new System.Windows.Forms.Button();
            this.btnСохранить = new System.Windows.Forms.Button();
            this.btnClearImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pxImage)).BeginInit();
            this.panelИнструменты.SuspendLayout();
            this.panelРежимыРисования.SuspendLayout();
            this.groupBoxНастройкиПера.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarТолщинаПера)).BeginInit();
            this.panelЦветоваяПалитра.SuspendLayout();
            this.panelОперацииСФайлом.SuspendLayout();
            this.SuspendLayout();
            //
            // pxImage
            //
            this.pxImage.BackColor = System.Drawing.Color.White;
            this.pxImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pxImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pxImage.Location = new System.Drawing.Point(200, 0);
            this.pxImage.Name = "pxImage";
            this.pxImage.Size = new System.Drawing.Size(800, 600);
            this.pxImage.TabIndex = 0;
            this.pxImage.TabStop = false;
            //
            // panelИнструменты
            //
            this.panelИнструменты.Controls.Add(this.panelРежимыРисования);
            this.panelИнструменты.Controls.Add(this.groupBoxНастройкиПера);
            this.panelИнструменты.Controls.Add(this.panelЦветоваяПалитра);
            this.panelИнструменты.Controls.Add(this.panelОперацииСФайлом);
            this.panelИнструменты.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelИнструменты.Location = new System.Drawing.Point(0, 0);
            this.panelИнструменты.Name = "panelИнструменты";
            this.panelИнструменты.Size = new System.Drawing.Size(200, 600);
            this.panelИнструменты.TabIndex = 1;
            //
            // panelРежимыРисования
            //
            this.panelРежимыРисования.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelРежимыРисования.Controls.Add(this.btnВыделение);
            this.panelРежимыРисования.Controls.Add(this.btnЗвезда);
            this.panelРежимыРисования.Controls.Add(this.btnТреугольник);
            this.panelРежимыРисования.Controls.Add(this.btnПрямоугольник);
            this.panelРежимыРисования.Controls.Add(this.btnЭллипс);
            this.panelРежимыРисования.Controls.Add(this.btnЛиния);
            this.panelРежимыРисования.Controls.Add(this.btnКарандаш);
            this.panelРежимыРисования.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelРежимыРисования.Location = new System.Drawing.Point(0, 160);
            this.panelРежимыРисования.Name = "panelРежимыРисования";
            this.panelРежимыРисования.Padding = new System.Windows.Forms.Padding(5);
            this.panelРежимыРисования.Size = new System.Drawing.Size(200, 230);
            this.panelРежимыРисования.TabIndex = 4;
            //
            // btnВыделение
            //
            this.btnВыделение.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnВыделение.Location = new System.Drawing.Point(5, 195);
            this.btnВыделение.Name = "btnВыделение";
            this.btnВыделение.Size = new System.Drawing.Size(188, 30);
            this.btnВыделение.TabIndex = 6;
            this.btnВыделение.Text = "Выделение";
            this.btnВыделение.UseVisualStyleBackColor = true;
            //
            // btnЗвезда
            //
            this.btnЗвезда.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnЗвезда.Location = new System.Drawing.Point(5, 165);
            this.btnЗвезда.Name = "btnЗвезда";
            this.btnЗвезда.Size = new System.Drawing.Size(188, 30);
            this.btnЗвезда.TabIndex = 5;
            this.btnЗвезда.Text = "Звезда";
            this.btnЗвезда.UseVisualStyleBackColor = true;
            //
            // btnТреугольник
            //
            this.btnТреугольник.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnТреугольник.Location = new System.Drawing.Point(5, 135);
            this.btnТреугольник.Name = "btnТреугольник";
            this.btnТреугольник.Size = new System.Drawing.Size(188, 30);
            this.btnТреугольник.TabIndex = 4;
            this.btnТреугольник.Text = "Треугольник";
            this.btnТреугольник.UseVisualStyleBackColor = true;
            //
            // btnПрямоугольник
            //
            this.btnПрямоугольник.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnПрямоугольник.Location = new System.Drawing.Point(5, 105);
            this.btnПрямоугольник.Name = "btnПрямоугольник";
            this.btnПрямоугольник.Size = new System.Drawing.Size(188, 30);
            this.btnПрямоугольник.TabIndex = 3;
            this.btnПрямоугольник.Text = "Прямоугольник";
            this.btnПрямоугольник.UseVisualStyleBackColor = true;
            //
            // btnЭллипс
            //
            this.btnЭллипс.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnЭллипс.Location = new System.Drawing.Point(5, 75);
            this.btnЭллипс.Name = "btnЭллипс";
            this.btnЭллипс.Size = new System.Drawing.Size(188, 30);
            this.btnЭллипс.TabIndex = 2;
            this.btnЭллипс.Text = "Эллипс";
            this.btnЭллипс.UseVisualStyleBackColor = true;
            //
            // btnЛиния
            //
            this.btnЛиния.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnЛиния.Location = new System.Drawing.Point(5, 45);
            this.btnЛиния.Name = "btnЛиния";
            this.btnЛиния.Size = new System.Drawing.Size(188, 30);
            this.btnЛиния.TabIndex = 1;
            this.btnЛиния.Text = "Линия";
            this.btnЛиния.UseVisualStyleBackColor = true;
            //
            // btnКарандаш
            //
            this.btnКарандаш.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnКарандаш.Location = new System.Drawing.Point(5, 15);
            this.btnКарандаш.Name = "btnКарандаш";
            this.btnКарандаш.Size = new System.Drawing.Size(188, 30);
            this.btnКарандаш.TabIndex = 0;
            this.btnКарандаш.Text = "Карандаш";
            this.btnКарандаш.UseVisualStyleBackColor = true;
            //
            // groupBoxНастройкиПера
            //
            this.groupBoxНастройкиПера.Controls.Add(this.lblТолщинаПера);
            this.groupBoxНастройкиПера.Controls.Add(this.trackBarТолщинаПера);
            this.groupBoxНастройкиПера.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxНастройкиПера.Location = new System.Drawing.Point(0, 70);
            this.groupBoxНастройкиПера.Name = "groupBoxНастройкиПера";
            this.groupBoxНастройкиПера.Size = new System.Drawing.Size(200, 90);
            this.groupBoxНастройкиПера.TabIndex = 3;
            this.groupBoxНастройкиПера.TabStop = false;
            this.groupBoxНастройкиПера.Text = "Настройки пера";
            //
            // lblТолщинаПера
            //
            this.lblТолщинаПера.AutoSize = true;
            this.lblТолщинаПера.Location = new System.Drawing.Point(6, 68);
            this.lblТолщинаПера.Name = "lblТолщинаПера";
            this.lblТолщинаПера.Size = new System.Drawing.Size(71, 16);
            this.lblТолщинаПера.TabIndex = 1;
            this.lblТолщинаПера.Text = "Толщина: 2";
            //
            // trackBarТолщинаПера
            //
            this.trackBarТолщинаПера.Location = new System.Drawing.Point(6, 21);
            this.trackBarТолщинаПера.Maximum = 50;
            this.trackBarТолщинаПера.Minimum = 1;
            this.trackBarТолщинаПера.Name = "trackBarТолщинаПера";
            this.trackBarТолщинаПера.Size = new System.Drawing.Size(188, 45);
            this.trackBarТолщинаПера.TabIndex = 0;
            this.trackBarТолщинаПера.Value = 2;
            //
            // panelЦветоваяПалитра
            //
            this.panelЦветоваяПалитра.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelЦветоваяПалитра.Controls.Add(this.panelЦветЧерный);
            this.panelЦветоваяПалитра.Controls.Add(this.panelЦветПурпурный);
            this.panelЦветоваяПалитра.Controls.Add(this.panelЦветЖелтый);
            this.panelЦветоваяПалитра.Controls.Add(this.panelЦветСиний);
            this.panelЦветоваяПалитра.Controls.Add(this.panelЦветЗеленый);
            this.panelЦветоваяПалитра.Controls.Add(this.panelЦветКрасный);
            this.panelЦветоваяПалитра.Controls.Add(this.panelТекущийЦвет);
            this.panelЦветоваяПалитра.Controls.Add(this.btnВыбратьЦвет);
            this.panelЦветоваяПалитра.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelЦветоваяПалитра.Location = new System.Drawing.Point(0, 0);
            this.panelЦветоваяПалитра.Name = "panelЦветоваяПалитра";
            this.panelЦветоваяПалитра.Size = new System.Drawing.Size(200, 70);
            this.panelЦветоваяПалитра.TabIndex = 2;
            //
            // panelЦветЧерный
            //
            this.panelЦветЧерный.BackColor = System.Drawing.Color.Black;
            this.panelЦветЧерный.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelЦветЧерный.Location = new System.Drawing.Point(120, 10);
            this.panelЦветЧерный.Name = "panelЦветЧерный";
            this.panelЦветЧерный.Size = new System.Drawing.Size(20, 20);
            this.panelЦветЧерный.TabIndex = 5;
            //
            // panelЦветПурпурный
            //
            this.panelЦветПурпурный.BackColor = System.Drawing.Color.Magenta;
            this.panelЦветПурпурный.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelЦветПурпурный.Location = new System.Drawing.Point(100, 10);
            this.panelЦветПурпурный.Name = "panelЦветПурпурный";
            this.panelЦветПурпурный.Size = new System.Drawing.Size(20, 20);
            this.panelЦветПурпурный.TabIndex = 4;
            //
            // panelЦветЖелтый
            //
            this.panelЦветЖелтый.BackColor = System.Drawing.Color.Yellow;
            this.panelЦветЖелтый.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelЦветЖелтый.Location = new System.Drawing.Point(80, 10);
            this.panelЦветЖелтый.Name = "panelЦветЖелтый";
            this.panelЦветЖелтый.Size = new System.Drawing.Size(20, 20);
            this.panelЦветЖелтый.TabIndex = 3;
            //
            // panelЦветСиний
            //
            this.panelЦветСиний.BackColor = System.Drawing.Color.Blue;
            this.panelЦветСиний.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelЦветСиний.Location = new System.Drawing.Point(60, 10);
            this.panelЦветСиний.Name = "panelЦветСиний";
            this.panelЦветСиний.Size = new System.Drawing.Size(20, 20);
            this.panelЦветСиний.TabIndex = 2;
            //
            // panelЦветЗеленый
            //
            this.panelЦветЗеленый.BackColor = System.Drawing.Color.Lime;
            this.panelЦветЗеленый.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelЦветЗеленый.Location = new System.Drawing.Point(40, 10);
            this.panelЦветЗеленый.Name = "panelЦветЗеленый";
            this.panelЦветЗеленый.Size = new System.Drawing.Size(20, 20);
            this.panelЦветЗеленый.TabIndex = 1;
            //
            // panelЦветКрасный
            //
            this.panelЦветКрасный.BackColor = System.Drawing.Color.Red;
            this.panelЦветКрасный.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelЦветКрасный.Location = new System.Drawing.Point(20, 10);
            this.panelЦветКрасный.Name = "panelЦветКрасный";
            this.panelЦветКрасный.Size = new System.Drawing.Size(20, 20);
            this.panelЦветКрасный.TabIndex = 0;
            //
            // panelТекущийЦвет
            //
            this.panelТекущийЦвет.BackColor = System.Drawing.Color.Red;
            this.panelТекущийЦвет.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelТекущийЦвет.Location = new System.Drawing.Point(155, 10);
            this.panelТекущийЦвет.Name = "panelТекущийЦвет";
            this.panelТекущийЦвет.Size = new System.Drawing.Size(30, 20);
            this.panelТекущийЦвет.TabIndex = 6;
            //
            // btnВыбратьЦвет
            //
            this.btnВыбратьЦвет.Location = new System.Drawing.Point(10, 36);
            this.btnВыбратьЦвет.Name = "btnВыбратьЦвет";
            this.btnВыбратьЦвет.Size = new System.Drawing.Size(180, 25);
            this.btnВыбратьЦвет.TabIndex = 7;
            this.btnВыбратьЦвет.Text = "Выбрать цвет...";
            this.btnВыбратьЦвет.UseVisualStyleBackColor = true;
            //
            // panelОперацииСФайлом
            //
            this.panelОперацииСФайлом.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelОперацииСФайлом.Controls.Add(this.btnПовторить);
            this.panelОперацииСФайлом.Controls.Add(this.btnОтменить);
            this.panelОперацииСФайлом.Controls.Add(this.btnЗагрузить);
            this.panelОперацииСФайлом.Controls.Add(this.btnСохранить);
            this.panelОперацииСФайлом.Controls.Add(this.btnClearImage);
            this.panelОперацииСФайлом.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelОперацииСФайлом.Location = new System.Drawing.Point(0, 430);
            this.panelОперацииСФайлом.Name = "panelОперацииСФайлом";
            this.panelОперацииСФайлом.Padding = new System.Windows.Forms.Padding(5);
            this.panelОперацииСФайлом.Size = new System.Drawing.Size(200, 170);
            this.panelОперацииСФайлом.TabIndex = 1;
            //
            // btnПовторить
            //
            this.btnПовторить.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnПовторить.Location = new System.Drawing.Point(5, 135);
            this.btnПовторить.Name = "btnПовторить";
            this.btnПовторить.Size = new System.Drawing.Size(188, 30);
            this.btnПовторить.TabIndex = 4;
            this.btnПовторить.Text = "Повторить";
            this.btnПовторить.UseVisualStyleBackColor = true;
            //
            // btnОтменить
            //
            this.btnОтменить.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnОтменить.Location = new System.Drawing.Point(5, 105);
            this.btnОтменить.Name = "btnОтменить";
            this.btnОтменить.Size = new System.Drawing.Size(188, 30);
            this.btnОтменить.TabIndex = 3;
            this.btnОтменить.Text = "Отменить";
            this.btnОтменить.UseVisualStyleBackColor = true;
            //
            // btnЗагрузить
            //
            this.btnЗагрузить.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnЗагрузить.Location = new System.Drawing.Point(5, 75);
            this.btnЗагрузить.Name = "btnЗагрузить";
            this.btnЗагрузить.Size = new System.Drawing.Size(188, 30);
            this.btnЗагрузить.TabIndex = 2;
            this.btnЗагрузить.Text = "Загрузить из файла";
            this.btnЗагрузить.UseVisualStyleBackColor = true;
            //
            // btnСохранить
            //
            this.btnСохранить.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnСохранить.Location = new System.Drawing.Point(5, 45);
            this.btnСохранить.Name = "btnСохранить";
            this.btnСохранить.Size = new System.Drawing.Size(188, 30);
            this.btnСохранить.TabIndex = 1;
            this.btnСохранить.Text = "Сохранить в файл";
            this.btnСохранить.UseVisualStyleBackColor = true;
            //
            // btnClearImage
            //
            this.btnClearImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnClearImage.Location = new System.Drawing.Point(5, 15);
            this.btnClearImage.Name = "btnClearImage";
            this.btnClearImage.Size = new System.Drawing.Size(188, 30);
            this.btnClearImage.TabIndex = 0;
            this.btnClearImage.Text = "Очистить холст";
            this.btnClearImage.UseVisualStyleBackColor = true;
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.pxImage);
            this.Controls.Add(this.panelИнструменты);
            this.Name = "Form1";
            this.Text = "wfaPaint";
            ((System.ComponentModel.ISupportInitialize)(this.pxImage)).EndInit();
            this.panelИнструменты.ResumeLayout(false);
            this.panelРежимыРисования.ResumeLayout(false);
            this.groupBoxНастройкиПера.ResumeLayout(false);
            this.groupBoxНастройкиПера.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarТолщинаПера)).EndInit();
            this.panelЦветоваяПалитра.ResumeLayout(false);
            this.panelОперацииСФайлом.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pxImage;
        private System.Windows.Forms.Panel panelИнструменты;
        private System.Windows.Forms.Panel panelОперацииСФайлом;
        private System.Windows.Forms.Button btnClearImage;
        private System.Windows.Forms.Button btnЗагрузить;
        private System.Windows.Forms.Button btnСохранить;
        private System.Windows.Forms.Panel panelЦветоваяПалитра;
        private System.Windows.Forms.Panel panelЦветКрасный;
        private System.Windows.Forms.Panel panelЦветЧерный;
        private System.Windows.Forms.Panel panelЦветПурпурный;
        private System.Windows.Forms.Panel panelЦветЖелтый;
        private System.Windows.Forms.Panel panelЦветСиний;
        private System.Windows.Forms.Panel panelЦветЗеленый;
        private System.Windows.Forms.Panel panelРежимыРисования;
        private System.Windows.Forms.Button btnКарандаш;
        private System.Windows.Forms.Button btnПрямоугольник;
        private System.Windows.Forms.Button btnЭллипс;
        private System.Windows.Forms.Button btnЛиния;
        private System.Windows.Forms.Panel panelТекущийЦвет;
        private System.Windows.Forms.Button btnВыбратьЦвет;
        private System.Windows.Forms.GroupBox groupBoxНастройкиПера;
        private System.Windows.Forms.Label lblТолщинаПера;
        private System.Windows.Forms.TrackBar trackBarТолщинаПера;
        private System.Windows.Forms.Button btnПовторить;
        private System.Windows.Forms.Button btnОтменить;
        private System.Windows.Forms.Button btnЗвезда;
        private System.Windows.Forms.Button btnТреугольник;
        private System.Windows.Forms.Button btnВыделение;
    }
}