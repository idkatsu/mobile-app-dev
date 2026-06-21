using System.Windows;
using System.Windows.Input;

namespace KanbanBoard.WPF.Views
{
    public partial class TaskDialog : Window
    {
        private readonly List<string> _tags = new();
        private int _pomCount;

        public string TaskTitle { get; private set; } = "";
        public string TaskDescription { get; private set; } = "";
        public string TaskPriority { get; private set; } = "Medium";
        public string TaskLabel { get; private set; } = "None";
        public DateTime? TaskDueDate { get; private set; }
        public int TaskPomodoros { get; private set; }
        public List<string> TaskTags => new(_tags);
        public List<string> TagChips => _tags;

        public TaskDialog() { InitializeComponent(); }

        private void TagBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrWhiteSpace(TagBox.Text))
            {
                _tags.Add(TagBox.Text.Trim());
                TagBox.Text = "";
                TagChipsControl.ItemsSource = null;
                TagChipsControl.ItemsSource = TagChips;
                e.Handled = true;
            }
        }

        private void TagRemove_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBlock tb && tb.Tag is string tag)
            {
                _tags.Remove(tag);
                TagChipsControl.ItemsSource = null;
                TagChipsControl.ItemsSource = TagChips;
            }
        }

        private void PomMinus_Click(object sender, RoutedEventArgs e)
        {
            if (_pomCount > 0) _pomCount--;
            PomCount.Text = _pomCount.ToString();
        }

        private void PomPlus_Click(object sender, RoutedEventArgs e)
        {
            _pomCount++;
            PomCount.Text = _pomCount.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                MessageBox.Show("Введите заголовок задачи", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            TaskTitle = TitleBox.Text.Trim();
            TaskDescription = DescBox.Text.Trim();
            TaskPomodoros = _pomCount;
            TaskDueDate = DueDatePicker.SelectedDate;
            TaskPriority = (PriorityCombo.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content?.ToString() ?? "Medium";
            TaskLabel = (LabelCombo.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content?.ToString() ?? "None";
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
