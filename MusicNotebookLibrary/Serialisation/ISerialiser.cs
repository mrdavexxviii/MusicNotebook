using MusicNotebook.NotebookDefinitions;

namespace MusicNotebook.Serialisation;

public interface ISerialiser
{
    bool Save(string filename, Notebook notebook);
    Notebook? Load(string filename);
}
