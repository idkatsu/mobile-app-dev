using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FractionTrainer.Core.Enums;

namespace FractionTrainer.WPF.Views
{
    public partial class FractionCircleControl : UserControl
    {
        public static readonly RoutedEvent SectorClickedEvent =
            EventManager.RegisterRoutedEvent(
                "SectorClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(FractionCircleControl));

        public event RoutedEventHandler SectorClicked
        {
            add { AddHandler(SectorClickedEvent, value); }
            remove { RemoveHandler(SectorClickedEvent, value); }
        }

        public static readonly DependencyProperty TotalSectorsProperty =
            DependencyProperty.Register(
                nameof(TotalSectors),
                typeof(int),
                typeof(FractionCircleControl),
                new PropertyMetadata(4, OnSectorsChanged));

        public static readonly DependencyProperty SelectedSectorsProperty =
            DependencyProperty.Register(
                nameof(SelectedSectors),
                typeof(HashSet<int>),
                typeof(FractionCircleControl),
                new PropertyMetadata(new HashSet<int>(), OnSectorsChanged));

        public static readonly DependencyProperty DefaultSectorColorProperty =
            DependencyProperty.Register(
                nameof(DefaultSectorColor),
                typeof(Color),
                typeof(FractionCircleControl),
                new PropertyMetadata(Colors.LightGray, OnSectorsChanged));

        public static readonly DependencyProperty SelectedSectorColorProperty =
            DependencyProperty.Register(
                nameof(SelectedSectorColor),
                typeof(Color),
                typeof(FractionCircleControl),
                new PropertyMetadata(Colors.DodgerBlue, OnSectorsChanged));

        public static readonly DependencyProperty ShapeProperty =
            DependencyProperty.Register(
                nameof(Shape),
                typeof(ShapeType),
                typeof(FractionCircleControl),
                new PropertyMetadata(ShapeType.Circle, OnSectorsChanged));

        public int TotalSectors
        {
            get { return (int)GetValue(TotalSectorsProperty); }
            set { SetValue(TotalSectorsProperty, value); }
        }

        public HashSet<int> SelectedSectors
        {
            get { return (HashSet<int>)GetValue(SelectedSectorsProperty); }
            set { SetValue(SelectedSectorsProperty, value); }
        }

        public Color DefaultSectorColor
        {
            get { return (Color)GetValue(DefaultSectorColorProperty); }
            set { SetValue(DefaultSectorColorProperty, value); }
        }

        public Color SelectedSectorColor
        {
            get { return (Color)GetValue(SelectedSectorColorProperty); }
            set { SetValue(SelectedSectorColorProperty, value); }
        }

        public ShapeType Shape
        {
            get { return (ShapeType)GetValue(ShapeProperty); }
            set { SetValue(ShapeProperty, value); }
        }

        private bool _needsRedraw;

        public FractionCircleControl()
        {
            InitializeComponent();
            Loaded += (_, _) => DrawShape();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawShape();
        }

        private static void OnSectorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FractionCircleControl control)
            {
                control.DrawShape();
            }
        }

        private Brush GetBorderBrush()
        {
            try
            {
                return (Brush)FindResource("BorderColor");
            }
            catch
            {
                return Brushes.Gray;
            }
        }

        private void DrawShape()
        {
            double w = ActualWidth;
            double h = ActualHeight;

            if (w <= 0 || h <= 0 || TotalSectors <= 0)
            {
                _needsRedraw = true;
                return;
            }

            if (_needsRedraw)
            {
                _needsRedraw = false;
            }

            SectorCanvas.Children.Clear();

            switch (Shape)
            {
                case ShapeType.Circle:
                    DrawCircle(w, h);
                    break;
                case ShapeType.Rectangle:
                    DrawRectangle(w, h);
                    break;
                case ShapeType.Triangle:
                    DrawTriangle(w, h);
                    break;
                case ShapeType.Pentagon:
                    DrawPolygon(w, h, 5);
                    break;
                case ShapeType.Hexagon:
                    DrawPolygon(w, h, 6);
                    break;
                case ShapeType.Diamond:
                    DrawDiamond(w, h);
                    break;
                case ShapeType.Cross:
                    DrawCross(w, h);
                    break;
                case ShapeType.Star:
                    DrawStar(w, h);
                    break;
            }
        }

        private void DrawCircle(double w, double h)
        {
            double radius = Math.Min(w, h) / 2.0 - 4.0;
            Point center = new Point(w / 2.0, h / 2.0);
            double sectorAngle = 360.0 / TotalSectors;

            for (int i = 0; i < TotalSectors; i++)
            {
                double startAngle = sectorAngle * i - 90.0;
                double endAngle = startAngle + sectorAngle;

                Point startPoint = GetPointOnCircle(center, radius, startAngle);
                Point endPoint = GetPointOnCircle(center, radius, endAngle);

                bool isLargeArc = sectorAngle > 180.0;

                PathFigure pathFigure = new PathFigure
                {
                    StartPoint = center,
                    IsClosed = true
                };

                pathFigure.Segments.Add(new LineSegment(startPoint, true));

                ArcSegment arcSegment = new ArcSegment
                {
                    Point = endPoint,
                    Size = new Size(radius, radius),
                    RotationAngle = 0,
                    IsLargeArc = isLargeArc,
                    SweepDirection = SweepDirection.Clockwise
                };

                pathFigure.Segments.Add(arcSegment);
                pathFigure.Segments.Add(new LineSegment(center, true));

                PathGeometry pathGeometry = new PathGeometry();
                pathGeometry.Figures.Add(pathFigure);

                AddSectorPath(pathGeometry, i);
            }
        }

        private void DrawRectangle(double w, double h)
        {
            double margin = 4.0;
            double rectW = w - margin * 2;
            double rectH = h - margin * 2;
            double cellW = rectW / TotalSectors;

            for (int i = 0; i < TotalSectors; i++)
            {
                double x = margin + i * cellW;

                RectangleGeometry rectGeo = new RectangleGeometry(
                    new Rect(x, margin, cellW, rectH));

                AddSectorPath(rectGeo, i);
            }
        }

        private void DrawTriangle(double w, double h)
        {
            double margin = 4.0;
            Point top = new Point(w / 2.0, margin);
            Point bottomLeft = new Point(margin, h - margin);
            Point bottomRight = new Point(w - margin, h - margin);

            for (int i = 0; i < TotalSectors; i++)
            {
                double t1 = (double)i / TotalSectors;
                double t2 = (double)(i + 1) / TotalSectors;

                Point leftEdge = Lerp(bottomLeft, top, t1);
                Point rightEdge = Lerp(bottomLeft, top, t2);
                Point bottomEdgeLeft = Lerp(bottomLeft, bottomRight, t1);
                Point bottomEdgeRight = Lerp(bottomLeft, bottomRight, t2);

                PathFigure pathFigure = new PathFigure
                {
                    StartPoint = leftEdge,
                    IsClosed = true
                };

                pathFigure.Segments.Add(new LineSegment(rightEdge, true));
                pathFigure.Segments.Add(new LineSegment(bottomEdgeRight, true));
                pathFigure.Segments.Add(new LineSegment(bottomEdgeLeft, true));

                PathGeometry pathGeometry = new PathGeometry();
                pathGeometry.Figures.Add(pathFigure);

                AddSectorPath(pathGeometry, i);
            }
        }

        private void DrawPolygon(double w, double h, int sides)
        {
            double margin = 4.0;
            double radius = Math.Min(w, h) / 2.0 - margin;
            Point center = new Point(w / 2.0, h / 2.0);
            double sectorAngle = 360.0 / TotalSectors;

            Brush borderBrush = GetBorderBrush();

            Point[] vertices = new Point[sides];
            for (int v = 0; v < sides; v++)
            {
                double angle = 360.0 / sides * v - 90.0;
                vertices[v] = GetPointOnCircle(center, radius, angle);
            }

            PathFigure outlineFigure = new PathFigure { StartPoint = vertices[0], IsClosed = true };
            for (int v = 1; v < sides; v++)
            {
                outlineFigure.Segments.Add(new LineSegment(vertices[v], true));
            }
            PathGeometry outlineGeo = new PathGeometry();
            outlineGeo.Figures.Add(outlineFigure);
            Path outlinePath = new Path
            {
                Data = outlineGeo,
                Stroke = borderBrush,
                StrokeThickness = 1.5,
                IsHitTestVisible = false
            };
            SectorCanvas.Children.Add(outlinePath);

            for (int i = 0; i < TotalSectors; i++)
            {
                double startAngle = sectorAngle * i - 90.0;
                double endAngle = startAngle + sectorAngle;

                Point startPoint = GetPointOnCircle(center, radius, startAngle);
                Point endPoint = GetPointOnCircle(center, radius, endAngle);

                bool isLargeArc = sectorAngle > 180.0;

                PathFigure pathFigure = new PathFigure
                {
                    StartPoint = center,
                    IsClosed = true
                };

                pathFigure.Segments.Add(new LineSegment(startPoint, true));

                ArcSegment arcSegment = new ArcSegment
                {
                    Point = endPoint,
                    Size = new Size(radius, radius),
                    RotationAngle = 0,
                    IsLargeArc = isLargeArc,
                    SweepDirection = SweepDirection.Clockwise
                };

                pathFigure.Segments.Add(arcSegment);
                pathFigure.Segments.Add(new LineSegment(center, true));

                PathGeometry pathGeometry = new PathGeometry();
                pathGeometry.Figures.Add(pathFigure);

                AddSectorPath(pathGeometry, i);
            }
        }

        private void DrawDiamond(double w, double h)
        {
            double margin = 4.0;
            double hw = (w - margin * 2) / 2.0;
            double hh = (h - margin * 2) / 2.0;
            Point center = new Point(w / 2.0, h / 2.0);
            double radius = Math.Min(hw, hh);
            double sectorAngle = 360.0 / TotalSectors;

            Brush borderBrush = GetBorderBrush();

            Point top = new Point(center.X, margin);
            Point right = new Point(w - margin, center.Y);
            Point bottom = new Point(center.X, h - margin);
            Point left = new Point(margin, center.Y);

            PathFigure outlineFigure = new PathFigure { StartPoint = top, IsClosed = true };
            outlineFigure.Segments.Add(new LineSegment(right, true));
            outlineFigure.Segments.Add(new LineSegment(bottom, true));
            outlineFigure.Segments.Add(new LineSegment(left, true));
            PathGeometry outlineGeo = new PathGeometry();
            outlineGeo.Figures.Add(outlineFigure);
            Path outlinePath = new Path
            {
                Data = outlineGeo,
                Stroke = borderBrush,
                StrokeThickness = 1.5,
                IsHitTestVisible = false
            };
            SectorCanvas.Children.Add(outlinePath);

            for (int i = 0; i < TotalSectors; i++)
            {
                double startAngle = sectorAngle * i - 90.0;
                double endAngle = startAngle + sectorAngle;

                Point sp = GetPointOnCircle(center, radius, startAngle);
                Point ep = GetPointOnCircle(center, radius, endAngle);

                PathFigure pathFigure = new PathFigure
                {
                    StartPoint = center,
                    IsClosed = true
                };

                pathFigure.Segments.Add(new LineSegment(sp, true));
                pathFigure.Segments.Add(new LineSegment(ep, true));
                pathFigure.Segments.Add(new LineSegment(center, true));

                PathGeometry pathGeometry = new PathGeometry();
                pathGeometry.Figures.Add(pathFigure);

                AddSectorPath(pathGeometry, i);
            }
        }

        private void DrawCross(double w, double h)
        {
            double margin = 4.0;
            double armThickness = Math.Min(w, h) * 0.3;
            double armLength = Math.Min(w, h) / 2.0 - margin;

            Point center = new Point(w / 2.0, h / 2.0);
            double half = armThickness / 2.0;

            Brush borderBrush = GetBorderBrush();

            Point tl = new Point(center.X - half, margin);
            Point tr = new Point(center.X + half, margin);
            Point tr2 = new Point(center.X + half, center.Y - half);
            Point rr = new Point(w - margin, center.Y - half);
            Point rb = new Point(w - margin, center.Y + half);
            Point br2 = new Point(center.X + half, center.Y + half);
            Point br = new Point(center.X + half, h - margin);
            Point bl = new Point(center.X - half, h - margin);
            Point bl2 = new Point(center.X - half, center.Y + half);
            Point ll = new Point(margin, center.Y + half);
            Point lt = new Point(margin, center.Y - half);
            Point tl2 = new Point(center.X - half, center.Y - half);

            PathFigure outline = new PathFigure { StartPoint = tl, IsClosed = true };
            outline.Segments.Add(new LineSegment(tr, true));
            outline.Segments.Add(new LineSegment(tr2, true));
            outline.Segments.Add(new LineSegment(rr, true));
            outline.Segments.Add(new LineSegment(rb, true));
            outline.Segments.Add(new LineSegment(br2, true));
            outline.Segments.Add(new LineSegment(br, true));
            outline.Segments.Add(new LineSegment(bl, true));
            outline.Segments.Add(new LineSegment(bl2, true));
            outline.Segments.Add(new LineSegment(ll, true));
            outline.Segments.Add(new LineSegment(lt, true));
            outline.Segments.Add(new LineSegment(tl2, true));

            PathGeometry outlineGeo = new PathGeometry();
            outlineGeo.Figures.Add(outline);

            Path outlinePath = new Path
            {
                Data = outlineGeo,
                Stroke = borderBrush,
                StrokeThickness = 1.5,
                IsHitTestVisible = false
            };
            SectorCanvas.Children.Add(outlinePath);

            double sectorAngle = 360.0 / TotalSectors;
            for (int i = 0; i < TotalSectors; i++)
            {
                double startAngle = sectorAngle * i - 90.0;
                double endAngle = startAngle + sectorAngle;

                Point sp = GetPointOnCircle(center, armLength, startAngle);
                Point ep = GetPointOnCircle(center, armLength, endAngle);

                PathFigure pathFigure = new PathFigure
                {
                    StartPoint = center,
                    IsClosed = true
                };

                pathFigure.Segments.Add(new LineSegment(sp, true));
                pathFigure.Segments.Add(new LineSegment(ep, true));
                pathFigure.Segments.Add(new LineSegment(center, true));

                PathGeometry pathGeometry = new PathGeometry();
                pathGeometry.Figures.Add(pathFigure);

                AddSectorPath(pathGeometry, i);
            }
        }

        private void DrawStar(double w, double h)
        {
            double margin = 4.0;
            double radius = Math.Min(w, h) / 2.0 - margin;
            double innerRadius = radius * 0.45;
            Point center = new Point(w / 2.0, h / 2.0);
            int starPoints = Math.Max(TotalSectors, 5);
            double sectorAngle = 360.0 / TotalSectors;

            Brush borderBrush = GetBorderBrush();

            double starVertexAngle = 360.0 / starPoints;
            Point[] starOuter = new Point[starPoints];
            Point[] starInner = new Point[starPoints];

            for (int i = 0; i < starPoints; i++)
            {
                double a = starVertexAngle * i - 90.0;
                starOuter[i] = GetPointOnCircle(center, radius, a);
                starInner[i] = GetPointOnCircle(center, innerRadius, a + starVertexAngle / 2.0);
            }

            PathFigure starOutline = new PathFigure { StartPoint = starOuter[0], IsClosed = true };
            for (int i = 0; i < starPoints; i++)
            {
                starOutline.Segments.Add(new LineSegment(starInner[i], true));
                starOutline.Segments.Add(new LineSegment(starOuter[(i + 1) % starPoints], true));
            }

            PathGeometry starGeo = new PathGeometry();
            starGeo.Figures.Add(starOutline);

            Path starOutlinePath = new Path
            {
                Data = starGeo,
                Stroke = borderBrush,
                StrokeThickness = 1.5,
                IsHitTestVisible = false
            };
            SectorCanvas.Children.Add(starOutlinePath);

            for (int i = 0; i < TotalSectors; i++)
            {
                double startAngle = sectorAngle * i - 90.0;
                double endAngle = startAngle + sectorAngle;

                Point sp = GetPointOnCircle(center, radius, startAngle);
                Point ep = GetPointOnCircle(center, radius, endAngle);

                PathFigure pathFigure = new PathFigure
                {
                    StartPoint = center,
                    IsClosed = true
                };

                pathFigure.Segments.Add(new LineSegment(sp, true));

                bool isLargeArc = sectorAngle > 180.0;
                ArcSegment arcSegment = new ArcSegment
                {
                    Point = ep,
                    Size = new Size(radius, radius),
                    RotationAngle = 0,
                    IsLargeArc = isLargeArc,
                    SweepDirection = SweepDirection.Clockwise
                };

                pathFigure.Segments.Add(arcSegment);
                pathFigure.Segments.Add(new LineSegment(center, true));

                PathGeometry pathGeometry = new PathGeometry();
                pathGeometry.Figures.Add(pathFigure);

                AddSectorPath(pathGeometry, i);
            }
        }

        private void AddSectorPath(Geometry geometry, int index)
        {
            bool isSelected = SelectedSectors.Contains(index);
            Color sectorColor = isSelected ? SelectedSectorColor : DefaultSectorColor;

            Brush borderBrush = GetBorderBrush();

            Path sectorPath = new Path
            {
                Data = geometry,
                Fill = new SolidColorBrush(sectorColor),
                Stroke = borderBrush,
                StrokeThickness = 1.5,
                Tag = index
            };

            sectorPath.MouseLeftButtonDown += OnSectorMouseLeftDown;
            sectorPath.MouseEnter += OnSectorMouseEnter;
            sectorPath.MouseLeave += OnSectorMouseLeave;

            SectorCanvas.Children.Add(sectorPath);
        }

        private static Point Lerp(Point a, Point b, double t)
        {
            return new Point(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);
        }

        private static Point GetPointOnCircle(Point center, double radius, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * Math.PI / 180.0;
            double x = center.X + radius * Math.Cos(angleInRadians);
            double y = center.Y + radius * Math.Sin(angleInRadians);
            return new Point(x, y);
        }

        private void OnSectorMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Path path && path.Tag is int sectorIndex)
            {
                RaiseEvent(new SectorClickedRoutedEventArgs(sectorIndex));
            }
        }

        private void OnSectorMouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Path path)
            {
                path.Opacity = 0.8;
            }
        }

        private void OnSectorMouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Path path)
            {
                path.Opacity = 1.0;
            }
        }

        public class SectorClickedRoutedEventArgs : RoutedEventArgs
        {
            public int SectorIndex { get; }

            public SectorClickedRoutedEventArgs(int sectorIndex)
                : base(SectorClickedEvent)
            {
                SectorIndex = sectorIndex;
            }
        }
    }
}
