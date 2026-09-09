using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ParetoTool.Models;

/// <summary>
/// Represents an input item with a category and a value, implementing INotifyPropertyChanged for data binding.
/// </summary>
public class InputItem : INotifyPropertyChanged
{
    private string _category = string.Empty;
    private double _value;

    /// <summary>
    /// Gets or sets the category of the input item. Notifies listeners when the property value changes.
    /// </summary>
    public string Category
    {
        get => _category;
        set { _category = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Gets or sets the value associated with the input item. Notifies listeners when the property value changes.
    /// </summary>
    public double Value
    {
        get => _value;
        set { _value = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Occurs when a property value changes. This event is raised whenever the Category or Value properties are modified.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the PropertyChanged event for the specified property name. If no property name is provided, it uses the caller member name.
    /// </summary>
    /// <param name="name"></param>
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
