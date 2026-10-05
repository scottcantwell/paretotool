using ParetoTool.Views;
using System.Windows;

namespace ParetoTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var window = new StartWindow(true);
            window.Show();
        }

    }
}
