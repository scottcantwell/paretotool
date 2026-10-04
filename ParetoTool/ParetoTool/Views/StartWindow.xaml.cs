using ParetoTool.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ParetoTool.Views;

public partial class StartWindow : Window
{
    private readonly List<RecentGroup> _all = new();
    public ICommand FocusSearchCommand { get; }

    public StartWindow()
    {
        FocusSearchCommand = new RelayCommand(() => SearchBox.Focus());
        InitializeComponent();
        DataContext = this;
        LoadSample();
        RecentList.ItemsSource = _all;
        Loaded += (_, _) => SearchBox.Focus();
    }

    private void LoadSample()
    {
        _all.Add(new RecentGroup("This week", new[]
        {
            Item("ParetoTool.slnx", @"C:\Users\scant\source\repos\scottcantwell\paretotool\ParetoTool", "9/29/2026 7:27 PM", SolutionIcon()),
        }));

        _all.Add(new RecentGroup("This month", new[]
        {
            Item("queryforge.slnx", @"C:\Users\scant\source\repos\scottcantwell\queryforge", "9/21/2026 10:55 AM", SolutionIcon()),
            Item("queryforge", @"C:\Users\scant\source\repos\scottcantwell", "9/13/2026 9:43 AM", FolderIcon()),
            Item("paretotool", @"C:\Users\scant\source\repos\scottcantwell", "9/8/2026 7:35 PM", FolderIcon()),
            Item("GithubMonitor.slnx", @"C:\Users\scant\source\repos\scottcantwell\GitHubMonitor\src", "9/7/2026 12:10 PM", SolutionIcon()),
            Item("GitHubMonitor", @"C:\Users\scant\source\repos\scottcantwell", "9/6/2026 8:06 PM", FolderIcon()),
            Item("GithubMonitor.csproj", @"C:\Users\scant\source\repos\GitHubMonitor\src\GithubMonitor", "9/6/2026 7:42 PM", ProjectIcon()),
            Item("GithubMonitor.slnx", @"C:\Users\scant\source\repos\GitHubMonitor\src", "9/6/2026 7:42 PM", SolutionIcon()),
            Item("GitHubMonitor", @"C:\Users\scant\source\repos", "9/6/2026 7:30 PM", FolderIcon()),
        }));
    }

    private static RecentItem Item(string name, string path, string when, object icon) =>
        new(name, path, when, icon);

    private static object SolutionIcon()
    {
        var g = new Grid { Width = 16, Height = 16 };
        g.Children.Add(new Path
        {
            Fill = new SolidColorBrush(Color.FromRgb(154, 42, 201)),
            Data = Geometry.Parse("M2,1 H10.2 L14,4.8 V15 H2 Z"),
            Stretch = Stretch.Fill
        });
        g.Children.Add(new Path
        {
            Fill = Brushes.White,
            Data = Geometry.Parse("M10.2,1 V4.8 H14"),
            Stretch = Stretch.Fill,
            Margin = new Thickness(1)
        });
        return g;
    }

    private static object FolderIcon()
    {
        var g = new Grid { Width = 16, Height = 16 };
        g.Children.Add(new Path
        {
            Fill = new SolidColorBrush(Color.FromRgb(184, 126, 0)),
            Data = Geometry.Parse("M1,3.4 H6.3 L7.6,4.8 H15 V13.4 H1 Z"),
            Stretch = Stretch.Fill
        });
        g.Children.Add(new Path
        {
            Fill = new SolidColorBrush(Color.FromRgb(255, 205, 92)),
            Data = Geometry.Parse("M1.4,6.2 H14.6 V12.8 H1.4 Z"),
            Stretch = Stretch.Fill
        });
        return g;
    }

    private static object ProjectIcon()
    {
        var g = new Grid { Width = 16, Height = 16 };
        g.Children.Add(new Rectangle
        {
            Width = 13,
            Height = 13,
            RadiusX = 1,
            RadiusY = 1,
            Fill = new SolidColorBrush(Color.FromRgb(22, 140, 62)),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        });
        g.Children.Add(new Path
        {
            Stroke = Brushes.White,
            StrokeThickness = 1.1,
            Data = Geometry.Parse("M4,5 H12 M4,8 H12 M4,11 H12 M8,4 V12"),
            Stretch = Stretch.None,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        });
        return g;
    }

    private void SearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        Cue.Visibility = string.IsNullOrEmpty(SearchBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        var q = SearchBox.Text.Trim();
        if (string.IsNullOrEmpty(q))
        {
            RecentList.ItemsSource = _all;
            return;
        }

        RecentList.ItemsSource = _all
            .Select(g => new RecentGroup(g.Heading, g.Items.Where(i =>
                i.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                i.Path.Contains(q, StringComparison.OrdinalIgnoreCase))))
            .Where(g => g.Items.Count > 0)
            .ToList();
    }

    private void SearchBox_Focus(object sender, RoutedEventArgs e) => SearchBox.Focus();

    private void RecentItem_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is FrameworkElement { DataContext: RecentItem item })
            MessageBox.Show($"{item.Name}\n{item.Path}", "Open recent", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void CreateProject_Click(object sender, RoutedEventArgs e) =>
        MessageBox.Show("Create a new project", TitleOr("Start page"), MessageBoxButton.OK, MessageBoxImage.Information);

    private void OpenProject_Click(object sender, RoutedEventArgs e) =>
        MessageBox.Show("Open a project or solution", TitleOr("Start page"), MessageBoxButton.OK, MessageBoxImage.Information);

    private void OpenFolder_Click(object sender, RoutedEventArgs e) =>
        MessageBox.Show("Open a folder", TitleOr("Start page"), MessageBoxButton.OK, MessageBoxImage.Information);

    private void CloneRepo_Click(object sender, RoutedEventArgs e) =>
        MessageBox.Show("Clone a repository", TitleOr("Start page"), MessageBoxButton.OK, MessageBoxImage.Information);

    private static string TitleOr(string fallback) => fallback;

    private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void Maximize_Click(object sender, RoutedEventArgs e) =>
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

    private void Close_Click(object sender, RoutedEventArgs e) => Close();
}

public sealed class RecentGroup
{
    public RecentGroup(string heading, IEnumerable<RecentItem> items)
    {
        Heading = heading;
        Items = new ObservableCollection<RecentItem>(items);
    }

    public string Heading { get; }
    public ObservableCollection<RecentItem> Items { get; }
}
