using System.Windows;
using System.Windows.Controls;
using KanbanBoard.WPF.ViewModels;

namespace KanbanBoard.WPF.Views
{
    public partial class ListView : UserControl
    {
        public ListView() { InitializeComponent(); }
        private MainViewModel? VM => DataContext as MainViewModel;

        private void AddTask_Click(object sender, RoutedEventArgs e) => VM?.AddNewTask();

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (VM == null) return;
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
                VM.RefreshListTasks();
            else
                VM.SearchTasks(SearchBox.Text);
        }

        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VM == null || FilterPriority.SelectedItem is not ComboBoxItem selected) return;
            string? tag = selected.Tag as string;
            VM.FilterByPriority(tag);
        }
    }
}
