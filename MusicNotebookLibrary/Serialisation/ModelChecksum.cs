using MusicNotebook;
using MusicNotebook.NotebookDefinitions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MusicNotebookLibrary.Serialisation
{
    public static class ModelChecksum
    {
        public static string Checksum(Notebook notebook)
        {
            string str = JsonHandler.ToJson(notebook);
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(str);
            byte[] hash = md5.ComputeHash(inputBytes);
            return Convert.ToBase64String(hash);
        }
    }
}
