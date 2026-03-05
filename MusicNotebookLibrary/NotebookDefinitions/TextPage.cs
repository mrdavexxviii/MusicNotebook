using CommunityToolkit.Mvvm.ComponentModel;

namespace MusicNotebook.NotebookDefinitions;

public partial class TextPage : ObservableObject, INotebookPage
{
    [ObservableProperty]
    private string _name = string.Empty;
    [ObservableProperty]
    private string _content = string.Empty;
}
