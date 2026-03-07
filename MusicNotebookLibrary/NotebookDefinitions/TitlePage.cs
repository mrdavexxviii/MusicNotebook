using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace MusicNotebook.NotebookDefinitions;

public partial class TitlePage : ObservableObject, INotebookPage
{
    [ObservableProperty]
    private string _name = string.Empty;
    [ObservableProperty]
    private string _content = string.Empty;
    [ObservableProperty]
    private string _productionNotes = string.Empty;

    [JsonIgnore]
    public bool CanDelete => false;
}
