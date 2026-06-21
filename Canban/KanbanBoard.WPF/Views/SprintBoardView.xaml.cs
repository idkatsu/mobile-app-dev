using System.Windows;
using System.Windows.Controls;
using KanbanBoard.Core.Enums;
using KanbanBoard.WPF.ViewModels;

namespace KanbanBoard.WPF.Views
{
    public partial class SprintBoardView : UserControl
    {
        public SprintBoardView() { InitializeComponent(); }

        private MainViewModel? VM => DataContext as MainViewModel;

        private void NewSprint_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SprintDialog { Owner = Application.Current.MainWindow };
            if (dialog.ShowDialog() == true)
                VM?.CreateSprint(dialog.SprintName, dialog.SprintGoal, dialog.SprintStart, dialog.SprintEnd);
        }

        private void StartSprint_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Guid sprintId)
            {
                var result = MessageBox.Show("Начать спринт? Предыдущий активный спринт будет завершён.", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes) VM?.StartSprint(sprintId);
            }
        }

        private void DeleteSprint_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Guid sprintId)
            {
                var result = MessageBox.Show("Удалить спринт?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) VM?.DeleteSprint(sprintId);
            }
        }
    }
}
