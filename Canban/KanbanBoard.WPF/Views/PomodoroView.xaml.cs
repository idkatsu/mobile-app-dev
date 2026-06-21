using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Shapes;
using KanbanBoard.WPF.ViewModels;

namespace KanbanBoard.WPF.Views
{
    public partial class PomodoroView : UserControl
    {
        public PomodoroView() { InitializeComponent(); DataContextChanged += (_, _) => SubscribeToVm(); SubscribeToVm(); }

        private MainViewModel? VM => DataContext as MainViewModel;
        private bool _subscribed;

        private void SubscribeToVm()
        {
            if (_subscribed || VM == null) return;
            _subscribed = true;
            VM.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(MainViewModel.TimerDisplay) || e.PropertyName == nameof(MainViewModel.RemainingFraction))
                    Dispatcher.BeginInvoke(new Action(UpdateArc));
            };
            UpdateArc();
        }

        private void Start_Click(object sender, RoutedEventArgs e) => VM?.StartPomodoro();
        private void Pause_Click(object sender, RoutedEventArgs e) => VM?.PausePomodoro();
        private void Stop_Click(object sender, RoutedEventArgs e) => VM?.StopPomodoro();
        private void Skip_Click(object sender, RoutedEventArgs e) => VM?.SkipPomodoro();

        public void UpdateArc()
        {
            if (VM == null) return;
            double fraction = VM.RemainingFraction;
            var fig = new PathFigure { StartPoint = new Point(130, 5), IsClosed = false };
            double angle = fraction * 360;
            if (angle < 0.1) { TimerArc.Data = null; TimerText.Text = VM.TimerDisplay; return; }
            bool largeArc = angle > 180;
            double rad = (angle - 90) * Math.PI / 180;
            var end = new Point(130 + 125 * Math.Cos(rad), 130 + 125 * Math.Sin(rad));
            fig.Segments.Add(new ArcSegment(end, new Size(125, 125), 0, largeArc, SweepDirection.Clockwise, true));
            var geo = new PathGeometry();
            geo.Figures.Add(fig);
            TimerArc.Data = geo;
            TimerText.Text = VM.TimerDisplay;
        }
    }
}
