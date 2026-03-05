using AwesomeAssertions;
using MusicNotebookLibrary.Serialisation;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using MusicNotebook.NotebookDefinitions;

namespace MusicNotebookLibrary.Test;

public class SaveLoadTests
{
    [Fact]
    public void SaveLoadEncrypted_Successful()
    {
        string mypass = "asdjkaegfwekfbwefv";
        string file = Path.GetTempFileName();
        try
        {
            Notebook original = new Notebook
            {
                Name = "MyTestNotebook",
                Pages = new System.Collections.ObjectModel.ObservableCollection<INotebookPage>()
            };
            original.Pages.Add(new TextPage { Name = "Page1", Content = "This is the content of page 1." });
            var s = new EncryptedSerialiser(mypass);
            s.Save(file, original);
            var loaded = s.Load (file);
            loaded.Should().BeEquivalentTo(original);
        } 
        finally
        {
            File.Delete(file);
        }
    }
}
