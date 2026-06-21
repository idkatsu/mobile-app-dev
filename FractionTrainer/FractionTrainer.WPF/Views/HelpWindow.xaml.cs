using System.Windows;

namespace FractionTrainer.WPF.Views
{
    /// <summary>
    /// Окно справки с описанием горячих клавиш и параметров командной строки.
    /// </summary>
    public partial class HelpWindow : Window
    {
        /// <summary>
        /// Создаёт экземпляр HelpWindow.
        /// </summary>
        public HelpWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик кнопки «Закрыть».
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
