using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using KanbanBoard.WPF.ViewModels;

namespace KanbanBoard.WPF.Views
{
    public partial class StatisticsView : UserControl
    {
        public StatisticsView()
        {
            InitializeComponent();
            DataContextChanged += (_, _) => SubscribeToVm();
            Loaded += (_, _) => { SubscribeToVm(); ScheduleRedraw(); };
        }

        private MainViewModel? _subscribedVm;

        private void SubscribeToVm()
        {
            if (DataContext is not MainViewModel vm) return;
            if (_subscribedVm == vm) return;
            if (_subscribedVm != null) _subscribedVm.PropertyChanged -= OnVmPropertyChanged;
            _subscribedVm = vm;
            _subscribedVm.PropertyChanged += OnVmPropertyChanged;
        }

        private void OnVmPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.Stats))
                ScheduleRedraw();
        }

        private void ProgressChart_SizeChanged(object sender, SizeChangedEventArgs e) => ScheduleRedraw();

        private void ScheduleRedraw()
        {
            Dispatcher.BeginInvoke(new Action(DrawProgressChart), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        private void DrawProgressChart()
        {
            ProgressChart.Children.Clear();
            if (DataContext is not MainViewModel vm) return;
            var progress = vm.Stats?.DailyProgress;
            if (progress == null || progress.Count == 0) return;
            double w = ProgressChart.ActualWidth;
            double h = ProgressChart.ActualHeight;
            if (w <= 0 || h <= 0) return;

            int maxVal = Math.Max(progress.Max(p => p.Completed), 1);
            var points = new PointCollection();

            for (int i = 0; i < progress.Count; i++)
            {
                double x = progress.Count > 1 ? (w - 40) * i / (progress.Count - 1) + 20 : w / 2;
                double y = h - 40 - (progress[i].Completed / (double)maxVal) * (h - 60);
                points.Add(new Point(x, y));

                var dateLabel = new TextBlock
                {
                    Text = progress[i].Date.ToString("dd.MM"),
                    FontSize = 10,
                    Foreground = new SolidColorBrush(Color.FromRgb(150, 150, 150))
                };
                Canvas.SetLeft(dateLabel, x - 15);
                Canvas.SetTop(dateLabel, h - 18);
                ProgressChart.Children.Add(dateLabel);

                var valLabel = new TextBlock
                {
                    Text = progress[i].Completed.ToString(),
                    FontSize = 11,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new SolidColorBrush(Color.FromRgb(33, 150, 243))
                };
                Canvas.SetLeft(valLabel, x - 6);
                Canvas.SetTop(valLabel, y - 18);
                ProgressChart.Children.Add(valLabel);
            }

            if (points.Count > 1)
            {
                var polyline = new Polyline
                {
                    Points = points,
                    Stroke = new SolidColorBrush(Color.FromRgb(33, 150, 243)),
                    StrokeThickness = 2,
                    StrokeLineJoin = PenLineJoin.Round
                };
                ProgressChart.Children.Add(polyline);
            }

            foreach (var pt in points)
            {
                var dot = new Ellipse
                {
                    Width = 8,
                    Height = 8,
                    Fill = new SolidColorBrush(Color.FromRgb(33, 150, 243))
                };
                Canvas.SetLeft(dot, pt.X - 4);
                Canvas.SetTop(dot, pt.Y - 4);
                ProgressChart.Children.Add(dot);
            }
        }
    }
}
