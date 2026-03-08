using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Ink;

namespace MusicNotebook.NotebookDefinitions;

public class BackgroundItem
{
    public string Name { get; private set; }
    public string Resource { get; private set; }
    public double Pitch { get; private set; }
    public BackgroundItem(string name, string resource, double pitch)
    {
        Name = name;
        Resource = resource;
        Pitch = pitch;

    }
}
public partial class ImagePage : ObservableObject, INotebookPage
{
    [ObservableProperty]
    string _name = string.Empty;
    [ObservableProperty]
    string _content = string.Empty;
    [ObservableProperty]
    string _background = string.Empty;
    public StrokeCollection ImageData { get; set; } = [];

    [JsonIgnore]
    public bool CanDelete => true;

    [ObservableProperty]
    double _backgroundPitch = 1;

    [JsonIgnore]
    private BackgroundItem? _selectedBackground;

    [JsonIgnore]
    public BackgroundItem? SelectedBackground
    {
        get => _selectedBackground;
        set
        {
            if (_selectedBackground == value) return;
            _selectedBackground = value;
            OnPropertyChanged(nameof(SelectedBackground));

            if (value is not null)
            {
                // propagate the resource key and pitch to the existing observable properties
                Background = value.Resource ?? string.Empty;
                BackgroundPitch = value.Pitch;
            }
            else
            {
                Background = string.Empty;
                BackgroundPitch = 1;
            }
        }
    }

}
