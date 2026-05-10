using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace wfaPaint
{
    public partial class Form1 : Form
    {
        private Bitmap b;
        private Graphics g;
        private Pen myPen;
        private Point startLocation;
        private Point endLocation;
        private bool isDrawing = false;
        private MyDrawMode myDrawMode = MyDrawMode.Карандаш;
        private Color currentColor = Color.Red;
        private int currentPenWidth = 2;

        private bool isSelecting = false;
        private bool isMovingSelection = false;
        private Rectangle selectionRectangle;
        private Bitmap selectedAreaBitmap;
        private Point selectionOffset;

        private Stack<Bitmap> undoStack = new Stack<Bitmap>();
        private Stack<Bitmap> redoStack = new Stack<Bitmap>();

        public enum MyDrawMode
        {
            Карандаш,
            Линия,
            Эллипс,
            Прямоугольник,
            Треугольник,
            Звезда,
            Выделение
        }

        public Form1()
        {
            InitializeComponent();
            InitializePaintCanvas();
            SetupEventHandlers();
        }

        private void InitializePaintCanvas()
        {
            b = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            g = Graphics.FromImage(b);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            g.Clear(Color.White);

            myPen = new Pen(currentColor, currentPenWidth);
            myPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            myPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            pxImage.Image = b;

            SaveStateForUndo();
        }

        private void SetupEventHandlers()
        {
            btnКарандаш.Click += (s, e) => { myDrawMode = MyDrawMode.Карандаш; UpdateSelectedButton(btnКарандаш); };
            btnЛиния.Click += (s, e) => { myDrawMode = MyDrawMode.Линия; UpdateSelectedButton(btnЛиния); };
            btnЭллипс.Click += (s, e) => { myDrawMode = MyDrawMode.Эллипс; UpdateSelectedButton(btnЭллипс); };
            btnПрямоугольник.Click += (s, e) => { myDrawMode = MyDrawMode.Прямоугольник; UpdateSelectedButton(btnПрямоугольник); };
            btnТреугольник.Click += (s, e) => { myDrawMode = MyDrawMode.Треугольник; UpdateSelectedButton(btnТреугольник); };
            btnЗвезда.Click += (s, e) => { myDrawMode = MyDrawMode.Звезда; UpdateSelectedButton(btnЗвезда); };
            btnВыделение.Click += (s, e) => { myDrawMode = MyDrawMode.Выделение; UpdateSelectedButton(btnВыделение); DeselectArea(); };

            btnClearImage.Click += btnClearImage_Click;
            btnСохранить.Click += btnСохранить_Click;
            btnЗагрузить.Click += btnЗагрузить_Click;
            btnОтменить.Click += btnОтменить_Click;
            btnПовторить.Click += btnПовторить_Click;

            pxImage.MouseDown += PxImage_MouseDown;
            pxImage.MouseMove += PxImage_MouseMove;
            pxImage.MouseUp += PxImage_MouseUp;
            pxImage.Paint += PxImage_Paint;

            panelЦветКрасный.Click += (s, e) => { currentColor = Color.Red; UpdatePenColor(); };
            panelЦветЗеленый.Click += (s, e) => { currentColor = Color.Green; UpdatePenColor(); };
            panelЦветСиний.Click += (s, e) => { currentColor = Color.Blue; UpdatePenColor(); };
            panelЦветЖелтый.Click += (s, e) => { currentColor = Color.Yellow; UpdatePenColor(); };
            panelЦветПурпурный.Click += (s, e) => { currentColor = Color.Magenta; UpdatePenColor(); };
            panelЦветЧерный.Click += (s, e) => { currentColor = Color.Black; UpdatePenColor(); };
            btnВыбратьЦвет.Click += btnВыбратьЦвет_Click;

            trackBarТолщинаПера.Scroll += trackBarТолщинаПера_Scroll;

            UpdateSelectedButton(btnКарандаш);
            UpdatePenColor();
        }

        private void UpdateSelectedButton(Button selectedButton)
        {
            foreach (Control control in panelРежимыРисования.Controls)
            {
                if (control is Button button)
                {
                    button.FlatAppearance.BorderSize = 1;
                    button.FlatAppearance.BorderColor = SystemColors.ControlDark;
                }
            }

            selectedButton.FlatAppearance.BorderSize = 2;
            selectedButton.FlatAppearance.BorderColor = Color.Blue;
        }

        private void UpdatePenColor()
        {
            myPen.Color = currentColor;
            panelТекущийЦвет.BackColor = currentColor;
        }

        private void trackBarТолщинаПера_Scroll(object sender, EventArgs e)
        {
            currentPenWidth = trackBarТолщинаПера.Value;
            myPen.Width = currentPenWidth;
            lblТолщинаПера.Text = $"Толщина: {currentPenWidth}";
        }

        private void SaveStateForUndo()
        {
            undoStack.Push(new Bitmap(b));
            redoStack.Clear();
        }

        private void RestoreBitmap(Bitmap previousBitmap)
        {
            if (g != null) g.Dispose();
            if (b != null) b.Dispose();

            b = new Bitmap(previousBitmap);
            g = Graphics.FromImage(b);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            pxImage.Image = b;
            pxImage.Invalidate();
        }

        private void btnОтменить_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 1)
            {
                DeselectArea();
                redoStack.Push(undoStack.Pop());
                RestoreBitmap(undoStack.Peek());
            }
        }

        private void btnПовторить_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                DeselectArea();
                Bitmap nextState = redoStack.Pop();
                undoStack.Push(new Bitmap(b));
                RestoreBitmap(nextState);
            }
        }

        private void DeselectArea()
        {
            if (selectedAreaBitmap != null)
            {
                selectedAreaBitmap.Dispose();
                selectedAreaBitmap = null;
                selectionRectangle = Rectangle.Empty;
                pxImage.Invalidate();
            }
        }

        private void PxImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (myDrawMode == MyDrawMode.Выделение)
                {
                    if (selectedAreaBitmap != null && selectionRectangle.Contains(e.Location))
                    {
                        isMovingSelection = true;
                        selectionOffset = new Point(e.Location.X - selectionRectangle.X, e.Location.Y - selectionRectangle.Y);
                        SaveStateForUndo();
                    }
                    else
                    {
                        DeselectArea();
                        isSelecting = true;
                        startLocation = e.Location;
                        selectionRectangle = new Rectangle(startLocation, new Size(0, 0));
                    }
                }
                else
                {
                    DeselectArea();
                    isDrawing = true;
                    startLocation = e.Location;
                    endLocation = e.Location;
                    SaveStateForUndo();
                }
            }
        }

        private void PxImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                endLocation = e.Location;

                switch (myDrawMode)
                {
                    case MyDrawMode.Карандаш:
                        g.DrawLine(myPen, startLocation, endLocation);
                        startLocation = endLocation;
                        break;
                    case MyDrawMode.Линия:
                    case MyDrawMode.Эллипс:
                    case MyDrawMode.Прямоугольник:
                    case MyDrawMode.Треугольник:
                    case MyDrawMode.Звезда:
                        RestoreBitmap(undoStack.Peek());
                        DrawCurrentShape(g, myPen, startLocation, endLocation, myDrawMode);
                        break;
                }
                pxImage.Invalidate();
            }
            else if (myDrawMode == MyDrawMode.Выделение)
            {
                if (isSelecting)
                {
                    endLocation = e.Location;
                    selectionRectangle = new Rectangle(
                        Math.Min(startLocation.X, endLocation.X),
                        Math.Min(startLocation.Y, endLocation.Y),
                        Math.Abs(startLocation.X - endLocation.X),
                        Math.Abs(startLocation.Y - endLocation.Y)
                    );
                }
                else if (isMovingSelection)
                {
                    Point newLocation = new Point(e.Location.X - selectionOffset.X, e.Location.Y - selectionOffset.Y);

                    RestoreBitmap(undoStack.Peek());

                    selectionRectangle.Location = newLocation;
                    g.DrawImage(selectedAreaBitmap, selectionRectangle);
                }
                pxImage.Invalidate();
            }
        }

        private void PxImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                isDrawing = false;
                endLocation = e.Location;

                switch (myDrawMode)
                {
                    case MyDrawMode.Линия:
                    case MyDrawMode.Эллипс:
                    case MyDrawMode.Прямоугольник:
                    case MyDrawMode.Треугольник:
                    case MyDrawMode.Звезда:
                        RestoreBitmap(undoStack.Peek());
                        DrawCurrentShape(g, myPen, startLocation, endLocation, myDrawMode);
                        break;
                }
                pxImage.Invalidate();
            }
            else if (myDrawMode == MyDrawMode.Выделение)
            {
                if (isSelecting)
                {
                    isSelecting = false;
                    if (selectionRectangle.Width > 0 && selectionRectangle.Height > 0)
                    {
                        if (selectedAreaBitmap != null) selectedAreaBitmap.Dispose();
                        selectedAreaBitmap = b.Clone(selectionRectangle, b.PixelFormat);
                        g.FillRectangle(new SolidBrush(Color.White), selectionRectangle);
                        pxImage.Invalidate();
                        SaveStateForUndo();
                    }
                    else
                    {
                        DeselectArea();
                    }
                }
                else if (isMovingSelection)
                {
                    isMovingSelection = false;
                    if (selectedAreaBitmap != null)
                    {
                        g.DrawImage(selectedAreaBitmap, selectionRectangle);
                    }
                    pxImage.Invalidate();
                }
            }
        }

        private void PxImage_Paint(object sender, PaintEventArgs e)
        {
            if (myDrawMode == MyDrawMode.Выделение)
            {
                if (isSelecting && !selectionRectangle.IsEmpty)
                {
                    using (Pen dashPen = new Pen(Color.Gray, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot })
                    {
                        e.Graphics.DrawRectangle(dashPen, selectionRectangle);
                    }
                }
                else if (selectedAreaBitmap != null && !selectionRectangle.IsEmpty && !isMovingSelection)
                {
                    using (Pen dashPen = new Pen(Color.Gray, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot })
                    {
                        e.Graphics.DrawRectangle(dashPen, selectionRectangle);
                    }
                }
            }
        }

        private void DrawCurrentShape(Graphics graphics, Pen pen, Point start, Point end, MyDrawMode mode)
        {
            int x = Math.Min(start.X, end.X);
            int y = Math.Min(start.Y, end.Y);
            int width = Math.Abs(start.X - end.X);
            int height = Math.Abs(start.Y - end.Y);

            switch (mode)
            {
                case MyDrawMode.Линия:
                    graphics.DrawLine(pen, start, end);
                    break;
                case MyDrawMode.Эллипс:
                    graphics.DrawEllipse(pen, x, y, width, height);
                    break;
                case MyDrawMode.Прямоугольник:
                    graphics.DrawRectangle(pen, x, y, width, height);
                    break;
                case MyDrawMode.Треугольник:
                    DrawTriangle(graphics, pen, start, end);
                    break;
                case MyDrawMode.Звезда:
                    DrawStar(graphics, pen, start, end);
                    break;
            }
        }

        private void DrawTriangle(Graphics graphics, Pen pen, Point start, Point end)
        {
            Point[] points = new Point[3];
            points[0] = new Point(start.X, end.Y);
            points[1] = new Point(end.X, end.Y);
            points[2] = new Point((start.X + end.X) / 2, start.Y);
            graphics.DrawPolygon(pen, points);
        }

        private void DrawStar(Graphics graphics, Pen pen, Point start, Point end)
        {
            float centerX = (start.X + end.X) / 2f;
            float centerY = (start.Y + end.Y) / 2f;
            float outerRadius = Math.Max(Math.Abs(start.X - end.X), Math.Abs(start.Y - end.Y)) / 2f;
            float innerRadius = outerRadius * 0.4f;

            PointF[] starPoints = new PointF[10];

            for (int i = 0; i < 10; i++)
            {
                float angle = (float)(Math.PI / 5 * i);
                float radius = (i % 2 == 0) ? outerRadius : innerRadius;

                starPoints[i] = new PointF(
                    centerX + radius * (float)Math.Cos(angle - Math.PI / 2),
                    centerY + radius * (float)Math.Sin(angle - Math.PI / 2)
                );
            }
            graphics.DrawPolygon(pen, starPoints.Select(p => Point.Round(p)).ToArray());
        }

        private void btnClearImage_Click(object sender, EventArgs e)
        {
            SaveStateForUndo();
            g.Clear(Color.White);
            DeselectArea();
            pxImage.Invalidate();
        }

        private void btnСохранить_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG Изображение|*.png|JPEG Изображение|*.jpg|BMP Изображение|*.bmp";
            sfd.Title = "Сохранить изображение";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = sfd.FileName;
                    ImageFormat format;

                    switch (System.IO.Path.GetExtension(fileName).ToLower())
                    {
                        case ".png":
                            format = ImageFormat.Png;
                            break;
                        case ".jpg":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            format = ImageFormat.Bmp;
                            break;
                        default:
                            format = ImageFormat.Png;
                            break;
                    }
                    b.Save(fileName, format);
                    MessageBox.Show("Изображение успешно сохранено!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении изображения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnЗагрузить_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Файлы изображений|*.png;*.jpg;*.jpeg;*.gif;*.bmp|Все файлы|*.*";
            ofd.Title = "Загрузить изображение";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SaveStateForUndo();
                    DeselectArea();

                    Bitmap loadedImage = new Bitmap(ofd.FileName);

                    if (loadedImage.Width > b.Width || loadedImage.Height > b.Height)
                    {
                        int newWidth = Math.Max(loadedImage.Width, b.Width);
                        int newHeight = Math.Max(loadedImage.Height, b.Height);

                        if (g != null) g.Dispose();
                        if (b != null) b.Dispose();
                        b = new Bitmap(newWidth, newHeight);
                        g = Graphics.FromImage(b);
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.Clear(Color.White);
                    }
                    else
                    {
                        g.Clear(Color.White);
                    }

                    g.DrawImage(loadedImage, 0, 0, loadedImage.Width, loadedImage.Height);

                    loadedImage.Dispose();
                    pxImage.Image = b;
                    pxImage.Invalidate();
                    MessageBox.Show("Изображение успешно загружено!", "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnВыбратьЦвет_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                currentColor = cd.Color;
                UpdatePenColor();
            }
        }
    }
}