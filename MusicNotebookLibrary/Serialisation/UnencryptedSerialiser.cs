using MusicNotebook.NotebookDefinitions;
using System.IO;

namespace MusicNotebook.Serialisation;

public class UnencryptedSerialiser : ISerialiser
{
    public bool Save(string filename, Notebook notebook)
    {
        try
        {
            using FileStream fileStream = new(filename, FileMode.Create);

            using StreamWriter encryptWriter = new(fileStream);

            string str = JsonHandler.ToJson(notebook);
            encryptWriter.Write(str);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public Notebook? Load(string filename)
    {

        try
        {
            using FileStream fileStream = new(filename, FileMode.Open);

            using StreamReader decryptReader = new(fileStream);
            string decryptedMessage = decryptReader.ReadToEnd();
            return JsonHandler.FromJson(decryptedMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"The decryption failed. {ex}");
            return null;
        }
    }

}
