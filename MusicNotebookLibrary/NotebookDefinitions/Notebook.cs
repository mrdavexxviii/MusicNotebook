using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace MusicNotebook.NotebookDefinitions;


public class Notebook : ObservableObject
{
    private INotebookPage? _selectedPage;
    [JsonIgnore]
    public INotebookPage? SelectedPage
    {
        get => _selectedPage;
        set => SetProperty(ref _selectedPage, value);
    }

    public ObservableCollection<INotebookPage> Pages { get; set; } = [new TitlePage { Name= "New Notebook"}];
}
