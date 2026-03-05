using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace MusicNotebook.NotebookDefinitions;


public partial class Notebook : ObservableObject
{
    [ObservableProperty]
    private string _name = "New Notebook";
    [ObservableProperty]
    [JsonIgnore]
    private INotebookPage? _selectedPage;
    public ObservableCollection<INotebookPage> Pages { get; set; } = [];
}
