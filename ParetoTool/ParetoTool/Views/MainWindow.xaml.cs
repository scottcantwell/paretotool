using ParetoTool.Classes;
using ParetoTool.Models;
using ParetoTool.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace ParetoTool;


public partial class MainWindow : Window
{
    public ObservableCollection<InputItem> InputItems { get; } = new();
    public ObservableCollection<ParetoRow> ParetoRows { get; } = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        //LoadSample(silent: true);
        //GenerateChart();
    }


    private void About_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(
            "Pareto Chart Tool\n\n" +
            "This tool allows you to input categories and their corresponding values, " +
            "and generates a Pareto chart based on the data.\n\n"
                    );
    }

    private void Help_Click(object sender, RoutedEventArgs e)
    {

        HelpWindow helpWindow = new HelpWindow() { Owner = this };

        helpWindow.ShowDialog();

    }


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

    private void Generate_Chart_Click(object sender, RoutedEventArgs e)
    {
        GenerateChart();
    }   

    private void Exit_Click(object sender, RoutedEventArgs e) => Close();


    private void AddRow_Click(object sender, RoutedEventArgs e)
    {
        InputItems.Add(new InputItem { Category = "", Value = 0 });
        StatusText.Text = "Row added.";
    }

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

    private void Generate_Click(object sender, RoutedEventArgs e) => GenerateChart();

    private void Sample_Click(object sender, RoutedEventArgs e)
    {
        LoadSample(silent: false);
        GenerateChart();
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        InputItems.Clear();
        ParetoRows.Clear();
        Chart.SetData(ParetoRows);
        StatusText.Text = "Cleared.";
    }

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