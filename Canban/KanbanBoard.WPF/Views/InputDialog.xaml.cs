using System.Windows;
using System.Windows.Input;

namespace KanbanBoard.WPF.Views
{
    public partial class InputDialog : Window
    {
        public string InputText => InputBox.Text.Trim();

        public InputDialog(string title, string prompt, string defaultValue = "")
        {
            InitializeComponent();
            Title = title;
            PromptText.Text = prompt;
            InputBox.Text = defaultValue;
            Loaded += (_, _) => InputBox.Focus();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(InputBox.Text))
            {
                MessageBox.Show("Введите название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { Ok_Click(sender, e); e.Handled = true; }
            else if (e.Key == Key.Escape) { Cancel_Click(sender, e); e.Handled = true; }
        }
    }
}
