using System.Windows;
using System.Windows.Threading;

namespace KanbanBoard.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DispatcherUnhandledException += (_, args) =>
            {
                MessageBox.Show($"Ошибка: {args.Exception.Message}\n\n{args.Exception.InnerException?.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                args.Handled = true;
            };
        }

        public static void SwitchTheme(string themeName)
        {
            var app = Current;
            var oldTheme = app.Resources.MergedDictionaries
                .OfType<ResourceDictionary>()
                .FirstOrDefault(d => d.Source?.OriginalString.Contains("Theme") == true);
            if (oldTheme != null) app.Resources.MergedDictionaries.Remove(oldTheme);
            string path = themeName == "Dark" ? "Themes/DarkTheme.xaml" : "Themes/LightTheme.xaml";
            app.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(path, UriKind.Relative) });
        }
    }
}
