using System.Text.Json.Serialization;

namespace MusicNotebook.NotebookDefinitions;

[JsonDerivedType(typeof(ImagePage), typeDiscriminator: nameof(ImagePage))]
[JsonDerivedType(typeof(TextPage), typeDiscriminator: nameof(TextPage))]
public interface INotebookPage
{
    string Name { get; set; }
    string Content { get; set; }
}
