using MusicNotebook.NotebookDefinitions;

namespace MusicNotebookLibrary.Serialisation;

public interface ISerialiser
{
    bool Save(string filename, Notebook notebook);
    Notebook? Load(string filename);
}
