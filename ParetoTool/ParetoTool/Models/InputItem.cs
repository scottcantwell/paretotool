using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ParetoTool.Models;

public class InputItem : INotifyPropertyChanged
{
    private string _category = string.Empty;
    private double _value;

    public string Category
    {
        get => _category;
        set { _category = value; OnPropertyChanged(); }
    }

    public double Value
    {
        get => _value;
        set { _value = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
