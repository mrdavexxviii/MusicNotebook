using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Ink;

namespace MusicNotebook.NotebookDefinitions;

public partial class ImagePage : ObservableObject, INotebookPage
{
    [ObservableProperty]
    string _name = string.Empty;
    [ObservableProperty]
    string _content = string.Empty;
    [ObservableProperty]
    string _background = string.Empty;
    public StrokeCollection ImageData { get; set; } = [];
    [ObservableProperty]
    double _backgroundPitch = 1;

}
