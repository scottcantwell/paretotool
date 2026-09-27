using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ParetoTool.Controls
{
    public partial class TagEditorControl : UserControl
    {
        public static readonly DependencyProperty TagsProperty =
            DependencyProperty.Register(
                nameof(Tags),
                typeof(ObservableCollection<string>),
                typeof(TagEditorControl),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnTagsChanged));

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(
                nameof(Placeholder),
                typeof(string),
                typeof(TagEditorControl),
                new PropertyMetadata("Type a tag and press Enter"));

        public static readonly DependencyProperty AllowDuplicatesProperty =
            DependencyProperty.Register(
                nameof(AllowDuplicates),
                typeof(bool),
                typeof(TagEditorControl),
                new PropertyMetadata(false));

        public static readonly RoutedEvent TagAddedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(TagAdded),
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(TagEditorControl));

        public static readonly RoutedEvent TagRemovedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(TagRemoved),
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(TagEditorControl));

        public TagEditorControl()
        {
            InitializeComponent();
            Tags = new ObservableCollection<string>();
            Loaded += (_, _) => UpdatePlaceholder();
        }

        public ObservableCollection<string> Tags
        {
            get => (ObservableCollection<string>)GetValue(TagsProperty);
            set => SetValue(TagsProperty, value);
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public bool AllowDuplicates
        {
            get => (bool)GetValue(AllowDuplicatesProperty);
            set => SetValue(AllowDuplicatesProperty, value);
        }

        public event RoutedEventHandler TagAdded
        {
            add => AddHandler(TagAddedEvent, value);
            remove => RemoveHandler(TagAddedEvent, value);
        }

        public event RoutedEventHandler TagRemoved
        {
            add => AddHandler(TagRemovedEvent, value);
            remove => RemoveHandler(TagRemovedEvent, value);
        }

        private static void OnTagsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (TagEditorControl)d;

            if (e.OldValue is ObservableCollection<string> oldTags)
                oldTags.CollectionChanged -= control.Tags_CollectionChanged;

            if (e.NewValue is ObservableCollection<string> newTags)
                newTags.CollectionChanged += control.Tags_CollectionChanged;
            else
                control.Tags = new ObservableCollection<string>();
        }

        private void Tags_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            UpdatePlaceholder();
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TryAddTag();
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Back &&
                string.IsNullOrEmpty(InputBox.Text) &&
                Tags.Count > 0)
            {
                RemoveTag(Tags[^1]);
                e.Handled = true;
            }
        }

        private void RemoveTag_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tag)
                RemoveTag(tag);
        }

        private void TryAddTag()
        {
            var tag = (InputBox.Text ?? string.Empty).Trim();
            if (tag.Length == 0)
                return;

            if (!AllowDuplicates &&
                Tags.Contains(tag, StringComparer.CurrentCultureIgnoreCase))
            {
                InputBox.Clear();
                return;
            }

            Tags.Add(tag);
            InputBox.Clear();
            RaiseEvent(new RoutedEventArgs(TagAddedEvent, this));
            UpdatePlaceholder();
        }

        private void RemoveTag(string tag)
        {
            if (Tags.Remove(tag))
            {
                RaiseEvent(new RoutedEventArgs(TagRemovedEvent, this));
                UpdatePlaceholder();
            }
        }

        private void UpdatePlaceholder()
        {
            InputBox.Tag = Placeholder;
        }
    }
}