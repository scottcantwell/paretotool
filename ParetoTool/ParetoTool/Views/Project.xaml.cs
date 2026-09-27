using ParetoTool.ViewModels;
using System.Windows;

namespace ParetoTool.Views
{
    /// <summary>
    /// Interaction logic for ProjectWindow.xaml
    /// </summary>
    public partial class ProjectWindow : Window
    {

        public ProjectWindow()
        {
            InitializeComponent();
            DataContext = new ProjectViewModel();
        }
        public void SaveProject_Click(object sender, RoutedEventArgs e)
        {

            // Logic to save the project
           
        }   
        public void Close_Click(object sender, RoutedEventArgs e)
        {

            var result = MessageBox.Show("The project has unsaved changes. Do you want to save the changes?", "Unsaved Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {


                SaveProject_Click(sender, e); // Call the save method   


                MessageBox.Show("Project saved successfully!", "Save Project", MessageBoxButton.OK, MessageBoxImage.Information);

            }

            if (result == MessageBoxResult.No)
            {


            }

            if (result == MessageBoxResult.Cancel)
            {
                return;
            }

            this.Close();
        
        
        }
    }
}
