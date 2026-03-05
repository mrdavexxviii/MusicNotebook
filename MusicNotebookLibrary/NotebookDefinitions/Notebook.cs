using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;

namespace MusicNotebook.NotebookDefinitions
{

    public partial class Notebook : ObservableObject
    {
        [ObservableProperty]
        private string _name = "New Notebook";
        public ObservableCollection<INotebookPage> Pages { get; set; } = new ();
    }
}
