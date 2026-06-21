using System.Windows;

namespace KanbanBoard.WPF.Views
{
    public partial class HelpWindow : Window
    {
        public HelpWindow() { InitializeComponent(); }
        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}
