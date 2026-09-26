using Microsoft.Win32;
using ParetoTool.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ParetoTool.Controls
{
    public partial class DelimitedFileImportControl : UserControl
    {
        public DelimitedFileImportViewModel ViewModel { get; } = new();

        public DelimitedFileImportControl()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Columns.CollectionChanged += (_, _) => RebuildGridColumns();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Title = "Select a text file",
                Filter = "Text files (*.txt;*.csv;*.tsv;*.log)|*.txt;*.csv;*.tsv;*.log|All files (*.*)|*.*"
            };

            if (dlg.ShowDialog() == true)
                ViewModel.LoadFileCommand.Execute(dlg.FileName);
        }

        private void RebuildGridColumns()
        {
            PreviewGrid.Columns.Clear();
            int columnCount = ViewModel.Columns.Count;

            for (int i = 0; i < columnCount; i++)
            {
                PreviewGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = $"Column{i + 1}",
                    Binding = new Binding($"[{i}]"),
                    Width = i == columnCount - 1
                        ? new DataGridLength(1, DataGridLengthUnitType.Star)
                        : DataGridLength.Auto,
                    MinWidth = 90
                });
            }
        }
    }
}