using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Ink;

namespace MusicNotebook.NotebookDefinitions;

public partial class ImagePage : ObservableObject, INotebookPage
{
    [ObservableProperty]
    string _name = string.Empty;
    [ObservableProperty]
    string _content = string.Empty;
    public StrokeCollection ImageData { get; set; } = [];

  
}
