using System.Windows;

namespace ParetoTool.Views
{
    /// <summary>
    /// Interaction logic for Project.xaml
    /// </summary>
    public partial class ProjectWindow : Window
    {
        public ProjectWindow()
        {
            InitializeComponent();
        }
        public void SaveProject_Click(object sender, RoutedEventArgs e)
        {
            // Logic to save the project
            MessageBox.Show("Project saved successfully!", "Save Project", MessageBoxButton.OK, MessageBoxImage.Information);
        }   
        public void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
