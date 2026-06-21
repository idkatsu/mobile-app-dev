using System.Windows;

namespace KanbanBoard.WPF.Views
{
    public partial class SprintDialog : Window
    {
        public string SprintName => NameBox.Text.Trim();
        public string SprintGoal => GoalBox.Text.Trim();
        public DateTime SprintStart => StartDate.SelectedDate ?? DateTime.Today;
        public DateTime SprintEnd => EndDate.SelectedDate ?? DateTime.Today.AddDays(14);

        public SprintDialog()
        {
            InitializeComponent();
            StartDate.SelectedDate = DateTime.Today;
            EndDate.SelectedDate = DateTime.Today.AddDays(14);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Введите название спринта", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (EndDate.SelectedDate <= StartDate.SelectedDate)
            {
                MessageBox.Show("Дата окончания должна быть позже даты начала", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
