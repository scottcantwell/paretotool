using ParetoTool.Classes;
using ParetoTool.Models;
using ParetoTool.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace ParetoTool;


/// <summary>
/// The MainWindow class serves as the primary user interface for the Pareto Chart Tool application. It provides functionality for users to input categories and values, 
/// generate Pareto charts, and manage the data through various UI elements such as buttons and menus.
/// </summary>
public partial class MainWindow : Window
{

    /// <summary>
    /// Gets the collection of input items that the user can modify in the input grid. Each item represents a category and its corresponding value.
    /// </summary>
    public ObservableCollection<InputItem> InputItems { get; } = new();

    /// <summary>
    /// Gets the collection of Pareto rows that are generated from the input items. Each row contains the rank, category, value, percentage, 
    /// cumulative value, and cumulative percentage for the Pareto chart.
    /// </summary>
    public ObservableCollection<ParetoRow> ParetoRows { get; } = new();

    /// <summary>
    /// Initializes a new instance of the MainWindow class, setting up the data context and preparing the UI for user interaction.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
     
    }


    /// <summary>
    /// Handles the click event for the "About" menu item, displaying information about the Pareto Chart Tool in a message box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void About_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(
            "This tool allows you to input categories and their corresponding values, " +
            "and generates a Pareto chart based on the data.\n\n","ParetoTool", MessageBoxButton.OK,MessageBoxImage.Information
                    );
    }

    /// <summary>
    /// Handles the click event for the "Help" menu item, opening a HelpWindow that provides instructions and guidance on how to use the Pareto Chart Tool.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Help_Click(object sender, RoutedEventArgs e)
    {

        HelpWindow helpWindow = new HelpWindow() { Owner = this };

        helpWindow.ShowDialog();

    }


    /// <summary>
    /// Handles the click event for the "New Chart" menu item, clearing the input items and Pareto rows, resetting the chart, 
    /// and updating the status text to indicate that a new chart has been created.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NewChart_Click(object sender, RoutedEventArgs e)
    {
        InputItems.Clear();
        ParetoRows.Clear();
        Chart.SetData(ParetoRows);
        StatusText.Text = "New chart created.";
    }   

    
    private void InputData_Click(object sender, RoutedEventArgs e)
    {
        
    }

    private void Chart_Click(object sender, RoutedEventArgs e)
    {

    }

    
    private void TransformedData_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    /// Handles the click event for the "Cut" menu item, copying the selected input item's category and value to the clipboard in a tab-separated format.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void Cut_Click(object sender, RoutedEventArgs e)
    {

            Clipboard.GetText();

            if (Clipboard.ContainsText() == true)
            {

            }

       
      
    }


    /// <summary>
    /// Copies the selected text ti the clipboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Copy_Click(object sender, RoutedEventArgs e)
    {

        Clipboard.GetText();


    }

    /// <summary>
    /// Handles the click event for the "Paste" menu item, retrieving text data from the clipboard and parsing it into input items.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void Paste_Click(object sender, RoutedEventArgs e)
    {

        if (Clipboard.ContainsText())
        {
            string clipboardText = Clipboard.GetText();
            string[] lines = clipboardText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    string[] parts = line.Split('\t');
                    if (parts.Length >= 2)
                    {
                        string category = parts[0].Trim();
                        if (double.TryParse(parts[1].Trim(), out double value))
                        {
                            InputItems.Add(new InputItem { Category = category, Value = value });
                        }
                    }
                }
            }
        }

    }

    /// <summary>
    /// Handles the click event for the "Generate Chart" button, invoking the GenerateChart method to process the input items and update the Pareto chart accordingly.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void Generate_Chart_Click(object sender, RoutedEventArgs e)
    {
        GenerateChart();
    }   

    private void Exit_Click(object sender, RoutedEventArgs e) => Close();


    /// <summary>
    /// Handles the click event for the "Add Row" button, adding a new empty input item to the InputItems collection and updating the 
    /// status text to indicate that a row has been added.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void AddRow_Click(object sender, RoutedEventArgs e)
    {
        InputItems.Add(new InputItem { Category = "", Value = 0 });
        StatusText.Text = "Row added.";
    }

    /// <summary>
    /// Handles the click event for the "Remove Row" button, removing the selected input item from the InputItems collection if one is selected,
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RemoveRow_Click(object sender, RoutedEventArgs e)
    {
        if (InputGrid.SelectedItem is InputItem item)
        {
            InputItems.Remove(item);
            StatusText.Text = "Row removed.";
        }
        else
        {
            StatusText.Text = "Select a row to remove.";
        }
    }

    /// <summary>
    /// Handles the click event for the "Generate" button, invoking the GenerateChart method to process the input items and update the Pareto chart accordingly.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Generate_Click(object sender, RoutedEventArgs e) => GenerateChart();

    /// <summary>
    /// Handles the click event for the "Sample" button, loading a sample set of defect data into the InputItems collection and generating the Pareto chart.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void Sample_Click(object sender, RoutedEventArgs e)
    {
        LoadSample(silent: false);
        GenerateChart();
    }

    /// <summary>
    /// Handles the click event for the "Clear" button, clearing the InputItems and ParetoRows collections, resetting the chart, 
    /// and updating the status text to indicate that the data has been cleared.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        InputItems.Clear();
        ParetoRows.Clear();
        Chart.SetData(ParetoRows);
        StatusText.Text = "Cleared.";
    }

    /// <summary>
    /// Generates the Pareto chart based on the current input items. It transforms the input data into Pareto rows, updates the chart, and 
    /// provides status information about the number of categories and their contribution to the total value.
    /// </summary>
    public void GenerateChart()
    {
        var rows = ParetoCalculator.Transform(InputItems);
        ParetoRows.Clear();
        foreach (var row in rows)
            ParetoRows.Add(row);

        Chart.SetData(ParetoRows);

        if (rows.Count == 0)
        {
            StatusText.Text = "No valid rows. Add categories with values greater than zero.";
            return;
        }

        var vital = rows.Where(r => r.CumulativePercentage <= 80.0001).ToList();
        if (vital.Count == 0)
            vital.Add(rows[0]);

        StatusText.Text =
            $"{rows.Count} categories · total {rows.Last().CumulativeValue:0.##} · " +
            $"~80% comes from {vital.Count} categor{(vital.Count == 1 ? "y" : "ies")}.";
    }

    /// <summary>
    /// Loads a sample set of defect data into the InputItems collection. This method clears any existing input items and adds predefined categories with their corresponding values.
    /// If the 'silent' parameter is false, it updates the status text to indicate that sample data has been loaded.
    /// </summary>
    /// <param name="silent"></param>
    public void LoadSample(bool silent)
    {
        InputItems.Clear();
        InputItems.Add(new InputItem { Category = "Scratches", Value = 42 });
        InputItems.Add(new InputItem { Category = "Dents", Value = 28 });
        InputItems.Add(new InputItem { Category = "Misalignment", Value = 18 });
        InputItems.Add(new InputItem { Category = "Missing part", Value = 9 });
        InputItems.Add(new InputItem { Category = "Wrong color", Value = 6 });
        InputItems.Add(new InputItem { Category = "Packaging", Value = 4 });
        InputItems.Add(new InputItem { Category = "Other", Value = 3 });
        if (!silent)
            StatusText.Text = "Sample defect data loaded.";
    }
}