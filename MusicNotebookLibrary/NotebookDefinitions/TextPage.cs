using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicNotebook.NotebookDefinitions
{
    public partial class TextPage : ObservableObject, INotebookPage
    {
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private string _content;
    }
}
