using MusicNotebook.NotebookDefinitions;
using MusicNotebook.Serialisation;
using MusicNotebookLibrary.Serialisation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace MusicNotebook
{
    public class UnencryptedSerialiser : ISerialiser
    {
        public void Save(string filename, Notebook notebook)
        {
            try
            {
                using (FileStream fileStream = new(filename, FileMode.Create))
                {

                    using (StreamWriter encryptWriter = new(fileStream))
                    {

                        string str = JsonHandler.ToJson(notebook);
                        encryptWriter.Write(str);
                    }

                }

                Console.WriteLine("The file was encrypted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The encryption failed. {ex}");
            }
        }
        public Notebook Load(string filename)
        {

            try
            {
                using (FileStream fileStream = new(filename, FileMode.Open))
                {

                    using (StreamReader decryptReader = new(fileStream))
                    {
                        string decryptedMessage =  decryptReader.ReadToEnd();
                        return JsonHandler.FromJson(decryptedMessage);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The decryption failed. {ex}");
                return null;
            }
        }

    }
}
