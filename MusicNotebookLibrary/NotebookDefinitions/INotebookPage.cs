using System.Text.Json.Serialization;

namespace MusicNotebook.NotebookDefinitions;

[JsonDerivedType(typeof(ImagePage), typeDiscriminator: nameof(ImagePage))]
[JsonDerivedType(typeof(TextPage), typeDiscriminator: nameof(TextPage))]
[JsonDerivedType(typeof(TitlePage), typeDiscriminator: nameof(TitlePage))]
public interface INotebookPage
{
    string Name { get; set; }
    string Content { get; set; }
    bool CanDelete { get; }
}
