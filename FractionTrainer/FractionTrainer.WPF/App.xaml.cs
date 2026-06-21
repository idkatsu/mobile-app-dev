using System.Windows;

namespace FractionTrainer.WPF
{
    /// <summary>
    /// Точка входа приложения WPF.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Переключает тему приложения.
        /// </summary>
        /// <param name="themeName">Имя темы: "Light" или "Dark".</param>
        public static void SwitchTheme(string themeName)
        {
            Application currentApp = Application.Current;
            ResourceDictionary? oldTheme = currentApp.Resources.MergedDictionaries
                .OfType<ResourceDictionary>()
                .FirstOrDefault(d => d.Source?.OriginalString.Contains("Theme") == true);

            if (oldTheme != null)
            {
                currentApp.Resources.MergedDictionaries.Remove(oldTheme);
            }

            string themePath = themeName == "Dark"
                ? "Themes/DarkTheme.xaml"
                : "Themes/LightTheme.xaml";

            ResourceDictionary newTheme = new ResourceDictionary
            {
                Source = new Uri(themePath, UriKind.Relative)
            };

            currentApp.Resources.MergedDictionaries.Add(newTheme);
        }
    }
}
