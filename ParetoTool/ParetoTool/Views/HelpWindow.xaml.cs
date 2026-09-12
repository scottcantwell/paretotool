using System.Windows;

namespace ParetoTool.Views
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {


        /// <summary>
        /// Initializes a new instance of the HelpWindow class, setting up the user interface components.
        /// </summary>
        public HelpWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the click event for the AddSampleData button, loading sample data into the main window's input grid and generating the chart.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void AddSampleData_Click(object sender, RoutedEventArgs e)
        {
            // Logic to add sample data to the main window's input grid
            if (this.Owner is MainWindow mainWindow)
            {

                mainWindow.LoadSample(silent: false);
                mainWindow.GenerateChart();

            }
            DialogResult = true;
            this.Close();
        }


        /// <summary>
        /// Handles the Loaded event of the HelpWindow, setting the window title and displaying instructions in the InstructionsText TextBlock.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the window title
            this.Title = "Pareto Chart Tool - Help";
            InstructionsText.Text = "Instructions:\n\n" +
           "1. Input your categories and values in the grid.\n" +
           "2. Click 'Generate' to create the PBareto chart.\n" +
           "3. Use 'Add Row' or 'Remove Row' to modify your data.\n" +
           "4. Click 'Sample' to load example data.\n" +
           "5. Click 'Clear' to reset the data.\n\n";
        }

        /// <summary>
        /// Handles the click event for the Close button, closing the HelpWindow and setting the DialogResult to false.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

    }

}
