using ParetoTool.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ParetoTool.Views;

public partial class StartWindow : Window
{
    private readonly List<RecentGroup> _all = new();
    public ICommand FocusSearchCommand { get; }
    public string SelectedItemPath { get; private set; }
    private RecentItems recentItems = new RecentItems(Enumerable.Empty<RecentItemData>());
    private string startwindowitemscompletepath => System.IO.Path.Combine(ParetoTool.Resources.Application.APP_FILES_PATH,
            ParetoTool.Resources.StartWindow.START_WINDOW_RECENT_ITEMS_FIELNAME);


    public StartWindow(bool showContinueButton = false)
    {
        FocusSearchCommand = new RelayCommand(() => SearchBox.Focus());
        InitializeComponent();
        DataContext = this;
        LoadData();
        RecentList.ItemsSource = _all;

        ContinueButton.Visibility = showContinueButton ? Visibility.Visible : Visibility.Collapsed;

    }

    private void LoadData()
    {

        
        var todayGroup = new RecentGroup("Today", new List<RecentItem>());

        var yesterdayGroup = new RecentGroup("Yesterday", new List<RecentItem>());

        var thisWeekGroup = new RecentGroup("This week", new List<RecentItem>());

        var thisMonthGroup = new RecentGroup("This month", new List<RecentItem>());

        var olderGroup = new RecentGroup("Older", new List<RecentItem>());

        var pinnedGroup = new RecentGroup("Pinned", new List<RecentItem>());

        recentItems.LoadItems("C:\\Users\\scant\\Downloads\\recent (3).json");

        var groupedItems = recentItems.Items
            .GroupBy(item =>
            {
                DateTime dt;
                return DateTime.TryParse(item.LastAccessed, out dt) ? dt.ToString("MMMM yyyy") : item.LastAccessed;
            })
            .Select(group => new RecentGroup(group.Key, group.Select(item =>
                // keep storing the original string in RecentItem.LastAccessed (signature says it's a string)
                new RecentItem(item.Name, item.Path.Replace("/",@"\"), item.LastAccessed, FolderIcon(), item.Pinned)).ToList()))
            .ToList();

        foreach (var group in groupedItems)
        {

            var item = group.Items.FirstOrDefault();

            if (item == null)
                continue;

            pinnedGroup = new RecentGroup("Pinned", group.Items.Where(x => x.IsPinned).ToList());
            if (pinnedGroup.Items.Count > 0)
                _all.Add(pinnedGroup);

            todayGroup = new RecentGroup("Today", group.Items.Except(pinnedGroup.Items).Where(x => DateTime.TryParse(x.LastAccessed, out var dt)
                                                                                                   && dt.Day == DateTime.Now.Day).ToList());
            if (todayGroup.Items.Count > 0)
                _all.Add(todayGroup);

            yesterdayGroup = new RecentGroup("Yesterday", group.Items.Except(pinnedGroup.Items).Except(todayGroup.Items).Where(x => DateTime.TryParse(x.LastAccessed, out var dt)
                                                                                                                                    && dt.Date == DateTime.Now.Date.AddDays(-1)).ToList());
            if (yesterdayGroup.Items.Count > 0)
                _all.Add(yesterdayGroup);

            thisWeekGroup = new RecentGroup("This week", group.Items.Except(pinnedGroup.Items).Except(todayGroup.Items).Except(yesterdayGroup.Items).Where(x => DateTime.TryParse(x.LastAccessed, out var dt)
                                                                                                                                                     && dt >= DateTime.Now.AddDays(-7)).ToList());
            if (thisWeekGroup.Items.Count > 0)
                _all.Add(thisWeekGroup);

            thisMonthGroup = new RecentGroup("This month", group.Items.Except(pinnedGroup.Items).Except(todayGroup.Items).Except(yesterdayGroup.Items).Except(thisWeekGroup.Items).ToList().Where(x => DateTime.TryParse(x.LastAccessed, out var dt)
                                                                                                                                                                                            && dt.Month == DateTime.Now.Month && dt.Year == DateTime.Now.Year).ToList());
            if (thisMonthGroup.Items.Count > 0)
                _all.Add(thisMonthGroup);

            olderGroup = new RecentGroup("Older", group.Items.Except(pinnedGroup.Items).Except(todayGroup.Items).Except(yesterdayGroup.Items).Except(thisWeekGroup.Items).Except(thisMonthGroup.Items).ToList().Where(x => DateTime.TryParse(x.LastAccessed, out var dt) && dt < DateTime.Now.AddMonths(-1)).ToList());

            if (olderGroup.Items.Count > 0)
                _all.Add(olderGroup);

        }

    }



    private static RecentItem Item(string name, string path, string lastAccessed, object icon, bool isPinned) =>
        new(name, path, lastAccessed, icon, isPinned);

    private static object SolutionIcon()
    {
        var g = new Grid { Width = 16, Height = 16 };
        g.Children.Add(new Path
        {
            Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(154, 42, 201)),
            Data = Geometry.Parse("M2,1 H10.2 L14,4.8 V15 H2 Z"),
            Stretch = Stretch.Fill
        });
        g.Children.Add(new Path
        {
            Fill = System.Windows.Media.Brushes.White,
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
            Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(184, 126, 0)),
            Data = Geometry.Parse("M1,3.4 H6.3 L7.6,4.8 H15 V13.4 H1 Z"),
            Stretch = Stretch.Fill
        });
        g.Children.Add(new Path
        {
            Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 205, 92)),
            Data = Geometry.Parse("M1.4,6.2 H14.6 V12.8 H1.4 Z"),
            Stretch = Stretch.Fill
        });
        return g;
    }

    private static object ProjectIcon()
    {
        var g = new Grid { Width = 16, Height = 16 };
        g.Children.Add(new System.Windows.Shapes.Rectangle
        {
            Width = 13,
            Height = 13,
            RadiusX = 1,
            RadiusY = 1,
            Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(22, 140, 62)),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        });
        g.Children.Add(new Path
        {
            Stroke = System.Windows.Media.Brushes.White,
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

    private void GroupHeader_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button || button.DataContext is not RecentGroup group)
            return;
        if (VisualTreeHelper.GetParent(button) is not Panel panel)
            return;

        var items = panel.Children.OfType<ItemsControl>().FirstOrDefault();
        var chevron = FindChevron(button);
        if (items == null)
            return;

        var expanding = !group.IsExpanded;
        group.IsExpanded = expanding;
        AnimateSection(items, expanding);
        AnimateChevron(chevron, expanding);
    }

    private static void AnimateChevron(Path? chevron, bool expanding)
    {
        if (chevron == null)
            return;

        var rotate = chevron.RenderTransform as RotateTransform;
        if (rotate == null || rotate.IsFrozen)
        {
            rotate = new RotateTransform(rotate?.Angle ?? (expanding ? 0 : 90));
            chevron.RenderTransform = rotate;
        }

        rotate.BeginAnimation(RotateTransform.AngleProperty,
            new DoubleAnimation(expanding ? 90 : 0, TimeSpan.FromMilliseconds(180))
            {
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            });
    }

    private static Path? FindChevron(DependencyObject root)
    {
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
        {
            var child = VisualTreeHelper.GetChild(root, i);
            if (child is System.Windows.Shapes.Path path && path.RenderTransform is RotateTransform)
                return path;
            var nested = FindChevron(child);
            if (nested != null)
                return nested;
        }

        return null;
    }

    private static void AnimateSection(FrameworkElement items, bool expanding)
    {
        items.BeginAnimation(FrameworkElement.HeightProperty, null);

        if (expanding)
        {
            items.Visibility = Visibility.Visible;
            items.Height = double.NaN;
            items.UpdateLayout();

            var width = items.ActualWidth;
            if (width <= 1 && items.Parent is FrameworkElement parent)
                width = parent.ActualWidth;

            items.Measure(new System.Windows.Size(Math.Max(width, 1), double.PositiveInfinity));
            var target = items.DesiredSize.Height;
            if (target <= 0)
                target = items.ActualHeight;

            items.Height = 0;
            var anim = new DoubleAnimation(0, Math.Max(target, 0), TimeSpan.FromMilliseconds(220))
            {
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            anim.Completed += (_, _) =>
            {
                items.BeginAnimation(FrameworkElement.HeightProperty, null);
                items.Height = double.NaN;
            };
            items.BeginAnimation(FrameworkElement.HeightProperty, anim);
            return;
        }

        items.Height = items.ActualHeight;
        var from = items.ActualHeight;
        if (from <= 0)
        {
            items.Visibility = Visibility.Collapsed;
            items.Height = double.NaN;
            return;
        }

        var collapse = new DoubleAnimation(from, 0, TimeSpan.FromMilliseconds(180))
        {
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
        };
        collapse.Completed += (_, _) =>
        {
            items.BeginAnimation(FrameworkElement.HeightProperty, null);
            items.Height = 0;
            items.Visibility = Visibility.Collapsed;
        };
        items.BeginAnimation(FrameworkElement.HeightProperty, collapse);
    }

    private void Maximize_Click(object sender, RoutedEventArgs e)
    {

        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        MaximizeButton.Content = WindowState == WindowState.Maximized ? "&#xE923;" : "&#xE922;";

    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {

        Close();

    }

    private void OpenProject_Click(object sender, RoutedEventArgs e)
    {

    }

    private void CreateProject_Click(object sender, RoutedEventArgs e)
    {

    }

    private void OpenFolder_Click(object sender, RoutedEventArgs e)
    {

    }

    private void SearchBox_Focus(object sender, RoutedEventArgs e)
    {

    }

    private void RecentItem_Click(object sender, MouseButtonEventArgs e)
    {

    }

    private void ContinueButton_Click(object sender, RoutedEventArgs e)
    {

        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();

    }

    private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {

        if (e.LeftButton == MouseButtonState.Released)
        {

            if (e.OriginalSource is TextBlock textBlock)
            {

                if (textBlock.Tag == null)
                {
                    return;
                }

                string? path = textBlock.Tag?.ToString();

                bool isOpen = Application.Current.Windows
                .OfType<MainWindow>()
                .Any(w => w.IsLoaded);

                if (isOpen)
                {

                    MainWindow? mainWindow = Application.Current.Windows
                    .OfType<MainWindow>()
                    .FirstOrDefault();

                    if (mainWindow != null)
                    {
                        mainWindow.OpenProject(path);
                        mainWindow.Activate();
                    }


                }
                else
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    mainWindow.OpenProject(path);

                }


            }

        }
    }

    private void TextBlock_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
    {

        if (e.RightButton == MouseButtonState.Released)
        {
            if (e.OriginalSource is TextBlock textBlock)
            {

                if (textBlock.Tag == null)
                {
                    return;
                }

                SelectedItemPath = textBlock.Tag?.ToString();

                StartWindowContextMenu.Visibility = Visibility.Visible;

            }
        }
    }

    private void RemoveItem_Click(object sender, RoutedEventArgs e)
    {

        if (SelectedItemPath == null)
        {
            return;
        }

        var selectedItem = (from item in recentItems.Items
                            where item.CompletePath.Replace("/", @"\") == SelectedItemPath
                            select item).FirstOrDefault();

        if (selectedItem == null)
        {

            return;

        }

        recentItems.RemoveItem(selectedItem);

        //string startwindowitemscompletepath = System.IO.Path.Combine(ParetoTool.Resources.Application.APP_FILES_PATH, 
        //    ParetoTool.Resources.StartWindow.START_WINDOW_RECENT_ITEMS_FIELNAME);

        recentItems.SaveItems(Environment.ExpandEnvironmentVariables(startwindowitemscompletepath));

        //Refresh data

    }

    private void PinItem_Click(object sender, RoutedEventArgs e)
    {

        if (SelectedItemPath == null)
        {
            return;
        }

        var selectedItem = (from item in recentItems.Items
                            where item.Path == SelectedItemPath
                            select item).FirstOrDefault();

        if (selectedItem == null)
        {

            return;

        }

        selectedItem.Pinned = true;

        var updatedItem = selectedItem;

        recentItems.RemoveItem(selectedItem);

        recentItems.AddItem(updatedItem);

        recentItems.SaveItems(startwindowitemscompletepath);

        //Refresh data

    }

    private void CopyPath_Click(object sender, RoutedEventArgs e)
    {

        if (SelectedItemPath == null)
        {
            return;
        }

        Clipboard.SetText(SelectedItemPath);

    }
}