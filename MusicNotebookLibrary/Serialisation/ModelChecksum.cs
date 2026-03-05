using MusicNotebook.NotebookDefinitions;
using System.Security.Cryptography;

namespace MusicNotebook.Serialisation;

public static class ModelChecksum
{
    public static string Checksum(Notebook notebook)
    {
        string str = JsonHandler.ToJson(notebook);
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(str);
        byte[] hash = MD5.HashData(inputBytes);
        return Convert.ToBase64String(hash);
    }
}
