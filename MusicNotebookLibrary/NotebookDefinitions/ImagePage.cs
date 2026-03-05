using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Ink;

namespace MusicNotebook.NotebookDefinitions
{
    public class ImagePage : INotebookPage
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public StrokeCollection ImageData { get; set; } = new();

      
    }
}
