using ParetoTool.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ParetoTool.Views;

    public sealed class RecentGroup : INotifyPropertyChanged
    {
        private bool _isExpanded = true;

        public RecentGroup(string heading, IEnumerable<RecentItem> items)
        {
            Heading = heading;
            Items = new ObservableCollection<RecentItem>(items);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Heading { get; }
        public ObservableCollection<RecentItem> Items { get; }

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                OnPropertyChanged();
            }
        }
    }
