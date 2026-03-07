using AwesomeAssertions;
using MusicNotebook.NotebookDefinitions;
using MusicNotebook.Serialisation;
using NSubstitute;

namespace MusicNotebook.Test;

public class SaveLoadTests
{
    [Fact]
    public void SaveLoadEncrypted_Successful()
    {
        var mockPasswordService = Substitute.For<IPasswordService>();
        mockPasswordService.Password.Returns("asdjkaegfwekfbwefv");
        mockPasswordService.ValidPassword.Returns(true);

        string file = Path.GetTempFileName();
        try
        {
            Notebook original = new();
            original.Pages.Add(new TextPage { Name = "Page1", Content = "This is the content of page 1." });
            var s = new EncryptedSerialiser(mockPasswordService);
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
