using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParetoTool.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using File = System.IO.File;

namespace ParetoTool.ViewModels
{

    public partial class DelimitedFileImportViewModel : ObservableObject
    {
        private readonly List<string> _rawLines = new();

        public DelimitedFileImportViewModel()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            EncodingOptions = new ObservableCollection<EncodingOption>
            {
                new() { DisplayName = "65001: Unicode (UTF-8)", Encoding = new UTF8Encoding(false) },
                new() { DisplayName = "1200: Unicode (UTF-16 LE)", Encoding = Encoding.Unicode },
                new() { DisplayName = "1201: Unicode (UTF-16 BE)", Encoding = Encoding.BigEndianUnicode },
                new() { DisplayName = "1252: Western European (Windows)", Encoding = Encoding.GetEncoding(1252) },
                new() { DisplayName = "28591: Western European (ISO)", Encoding = Encoding.GetEncoding(28591) },
                new() { DisplayName = "20127: US-ASCII", Encoding = Encoding.ASCII },
                new() { DisplayName = "932: Japanese (Shift-JIS)", Encoding = Encoding.GetEncoding(932) },
                new() { DisplayName = "936: Chinese Simplified (GB2312)", Encoding = Encoding.GetEncoding(936) },
            };

            DelimiterOptions = new ObservableCollection<DelimiterOption>
            {
                new() { DisplayName = "Tab", Value = "\t" },
                new() { DisplayName = "Comma", Value = "," },
                new() { DisplayName = "Semicolon", Value = ";" },
                new() { DisplayName = "Space", Value = " " },
                new() { DisplayName = "Pipe", Value = "|" },
                new() { DisplayName = "--Custom--", Value = "", IsCustom = true },
            };

            SelectedEncodingOption = EncodingOptions.FirstOrDefault(o => o.Encoding.CodePage == 1252)
                                     ?? EncodingOptions[0];
            SelectedDelimiterOption = DelimiterOptions.First(o => o.IsCustom);
            CustomDelimiter = " ";
        }

        public ObservableCollection<EncodingOption> EncodingOptions { get; }
        public ObservableCollection<DelimiterOption> DelimiterOptions { get; }
        public ObservableCollection<ColumnOption> Columns { get; } = new();
        public ObservableCollection<string[]> PreviewGridRows { get; } = new();

        [ObservableProperty]
        private string? filePath;

        [ObservableProperty]
        private string fileName = "No file selected";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedEncoding))]
        [NotifyPropertyChangedFor(nameof(SelectedEncodingName))]
        [NotifyCanExecuteChangedFor(nameof(ReloadCommand))]
        private EncodingOption? selectedEncodingOption;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedDelimiter))]
        [NotifyPropertyChangedFor(nameof(IsCustomDelimiter))]
        private DelimiterOption? selectedDelimiterOption;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedDelimiter))]
        private string customDelimiter = " ";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CategoryColumnHeader))]
        private ColumnOption? selectedCategoryColumn;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DataColumnHeader))]
        private ColumnOption? selectedDataColumn;

        [ObservableProperty]
        private IReadOnlyList<string[]> previewRows = Array.Empty<string[]>();

        public Encoding SelectedEncoding =>
            SelectedEncodingOption?.Encoding ?? Encoding.UTF8;

        public string SelectedEncodingName =>
            SelectedEncodingOption?.DisplayName ?? SelectedEncoding.EncodingName;

        public bool IsCustomDelimiter =>
            SelectedDelimiterOption?.IsCustom == true;

        public string SelectedDelimiter
        {
            get
            {
                if (SelectedDelimiterOption?.IsCustom == true)
                    return UnescapeDelimiter(CustomDelimiter);

                return string.IsNullOrEmpty(SelectedDelimiterOption?.Value)
                    ? " "
                    : SelectedDelimiterOption!.Value;
            }
        }

        public int CategoryColumnIndex => SelectedCategoryColumn?.Index ?? -1;
        public int DataColumnIndex => SelectedDataColumn?.Index ?? -1;
        public string? CategoryColumnHeader => SelectedCategoryColumn?.Header;
        public string? DataColumnHeader => SelectedDataColumn?.Header;

        public IEnumerable<(string Category, string Data)> GetSelectedPairs()
        {
            int cat = CategoryColumnIndex;
            int data = DataColumnIndex;
            if (cat < 0 || data < 0)
                yield break;

            foreach (var row in PreviewRows)
            {
                string c = cat < row.Length ? row[cat] : "";
                string d = data < row.Length ? row[data] : "";
                if (!string.IsNullOrWhiteSpace(c) || !string.IsNullOrWhiteSpace(d))
                    yield return (c, d);
            }
        }

        [RelayCommand]
        public void LoadFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                throw new FileNotFoundException("File not found.", path);

            FilePath = path;
            FileName = Path.GetFileName(path);
            Reload();
        }

        [RelayCommand(CanExecute = nameof(CanReload))]
        private void Reload()
        {
            ReloadRawLines();
            RebuildPreview();
        }

        private bool CanReload() => !string.IsNullOrWhiteSpace(FilePath) && File.Exists(FilePath);

        partial void OnSelectedEncodingOptionChanged(EncodingOption? value)
        {
            if (CanReload())
                Reload();
        }

        partial void OnSelectedDelimiterOptionChanged(DelimiterOption? value)
        {
            RebuildPreview();
        }

        partial void OnCustomDelimiterChanged(string value)
        {
            if (IsCustomDelimiter)
                RebuildPreview();
        }

        private void ReloadRawLines()
        {
            _rawLines.Clear();
            if (FilePath is null) return;

            using var reader = new StreamReader(FilePath, SelectedEncoding, detectEncodingFromByteOrderMarks: true);
            while (reader.ReadLine() is { } line)
                _rawLines.Add(line);
        }

        private void RebuildPreview()
        {
            string delimiter = string.IsNullOrEmpty(SelectedDelimiter) ? " " : SelectedDelimiter;

            var rows = _rawLines
                .Where(static l => !string.IsNullOrWhiteSpace(l))
                .Select(l => SplitLine(l, delimiter))
                .ToList();

            PreviewRows = rows;

            int previousCategory = CategoryColumnIndex;
            int previousData = DataColumnIndex;
            int columnCount = rows.Count == 0 ? 0 : rows.Max(r => r.Length);

            Columns.Clear();
            for (int i = 0; i < columnCount; i++)
                Columns.Add(new ColumnOption { Header = $"Column{i + 1}", Index = i });

            PreviewGridRows.Clear();
            foreach (var row in rows)
                PreviewGridRows.Add(ToRowView(row, columnCount));

            SelectedCategoryColumn = FindColumn(previousCategory, fallback: 0);
            SelectedDataColumn = FindColumn(previousData, fallback: Math.Min(2, columnCount - 1));

            OnPropertyChanged(nameof(CategoryColumnIndex));
            OnPropertyChanged(nameof(DataColumnIndex));
        }

        private ColumnOption? FindColumn(int previousIndex, int fallback)
        {
            if (Columns.Count == 0)
                return null;

            if (previousIndex >= 0 && previousIndex < Columns.Count)
                return Columns[previousIndex];

            int index = Math.Clamp(fallback, 0, Columns.Count - 1);
            return Columns[index];
        }

        private static string[] SplitLine(string line, string delimiter)
        {
            if (delimiter.Length == 1)
                return line.Split(delimiter[0]);

            return line.Split(new[] { delimiter }, StringSplitOptions.None);
        }

        private static string[] ToRowView(string[] row, int columnCount)
        {
            var view = new string[columnCount];
            for (int i = 0; i < columnCount; i++)
                view[i] = i < row.Length ? row[i] : "";
            return view;
        }

        private static string UnescapeDelimiter(string? text)
        {
            if (string.IsNullOrEmpty(text))
                return " ";

            return text
                .Replace("\\t", "\t", StringComparison.Ordinal)
                .Replace("\\n", "\n", StringComparison.Ordinal)
                .Replace("\\r", "\r", StringComparison.Ordinal);
        }
    }
}