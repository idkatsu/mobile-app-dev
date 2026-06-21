using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using KanbanBoard.WPF.ViewModels;

namespace KanbanBoard.WPF.Views
{
    public partial class BoardView : UserControl
    {
        private Point _dragStart;
        private TaskViewModel? _draggedTask;

        public BoardView() { InitializeComponent(); }

        private MainViewModel? VM => DataContext as MainViewModel;

        private void AddTask_Click(object sender, RoutedEventArgs e) => VM?.AddNewTask();
        private void AddTaskToColumn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Guid colId) VM?.AddTaskToColumn(colId);
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Guid taskId)
            {
                var result = MessageBox.Show("Удалить эту задачу?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes) VM?.DeleteTask(taskId);
            }
        }

        private void DeleteColumn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Guid colId) VM?.DeleteColumn(colId);
        }

        private void CloseSprint_Click(object sender, RoutedEventArgs e)
        {
            if (VM?.ActiveSprint != null)
            {
                var result = MessageBox.Show($"Завершить спринт \"{VM.ActiveSprint.Name}\"?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes) VM.CloseSprint(VM.ActiveSprint.Id);
            }
        }

        private void AssignToSprint_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mi && mi.Tag is TaskViewModel task && VM?.ActiveSprint != null)
                VM.AssignTaskToSprint(task.Id, VM.ActiveSprint.Id);
        }

        private void RemoveFromSprint_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mi && mi.Tag is TaskViewModel task)
                VM?.RemoveTaskFromSprint(task.Id);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb) VM?.SearchTasks(tb.Text);
        }

        private void TaskCard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is TaskViewModel task)
            {
                _dragStart = e.GetPosition(null);
                _draggedTask = task;
                fe.CaptureMouse();
            }
        }

        private void TaskCard_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed || _draggedTask == null) return;
            var pos = e.GetPosition(null);
            if (Math.Abs(pos.X - _dragStart.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(pos.Y - _dragStart.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                var data = new DataObject("KanbanTask", _draggedTask);
                DragDrop.DoDragDrop((DependencyObject)sender, data, DragDropEffects.Move);
            }
        }

        private void TaskCard_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe) fe.ReleaseMouseCapture();
            _draggedTask = null;
        }

        private void Column_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent("KanbanTask") ? DragDropEffects.Move : DragDropEffects.None;
            e.Handled = true;
        }

        private void Column_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData("KanbanTask") is TaskViewModel task && sender is FrameworkElement fe &&
                fe.DataContext is KanbanColumnViewModel col)
            {
                VM?.MoveTask(task.Id, col.Id);
            }
            _draggedTask = null;
        }
    }
}
