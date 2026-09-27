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

            if (DataContext != null)
            {

                if (!((ProjectViewModel)DataContext).CanSave)
                {
                  
                    return;
                
                }


                ((ProjectViewModel)DataContext).SaveProject();


            }

        }
        
        public void Close_Click(object sender, RoutedEventArgs e)
        {

            if (DataContext != null)
            {

                if (!((ProjectViewModel)DataContext).CanSave)
                {
                    Close();
                    return;
                }   

                var result = MessageBox.Show("The project has unsaved changes. Do you want to save the changes?", "Unsaved Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {



                    ((ProjectViewModel)DataContext).SaveProject();


                    MessageBox.Show("Project saved successfully!", "Save Project", MessageBoxButton.OK, MessageBoxImage.Information);

                }

                if (result == MessageBoxResult.Cancel)
                {
                    return;

                }

                if (result == MessageBoxResult.No)
                {


                }



                Close();


            }
        
        }

    }
}
