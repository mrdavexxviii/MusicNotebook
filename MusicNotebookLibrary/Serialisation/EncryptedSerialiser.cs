using MusicNotebook.NotebookDefinitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace MusicNotebookLibrary.Serialisation;

public interface ISerialiser
{
    void Save(string filename, Notebook notebook);
    Notebook? Load(string filename);
}

public class EncryptedSerialiser : ISerialiser
{
    private readonly string password;

    public EncryptedSerialiser(string password)
    {
        this.password = password;
    }
    public void Save(string filename, Notebook notebook)
    {
        try
        {
            using (FileStream fileStream = new(filename, FileMode.Create))
            {
                byte[] plaintextBytes = Encoding.UTF8.GetBytes(JsonHandler.ToJson(notebook));

                // Generate a random salt (store this with the ciphertext)
                byte[] salt = new byte[16];
                RandomNumberGenerator.Fill(salt);

                // Derive a key from password+salt

                byte[] key = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);

                using Aes aes = Aes.Create();
                aes.Key = key;
                aes.GenerateIV();
                byte[] iv = aes.IV;

                // Prepend salt and IV so they are available for decryption
                fileStream.Write(salt, 0, salt.Length);
                fileStream.Write(iv, 0, iv.Length);

                using (var cs = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(plaintextBytes, 0, plaintextBytes.Length);
                    cs.FlushFinalBlock();
                }

            }

            Console.WriteLine("The file was encrypted.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"The encryption failed. {ex}");
        }
    }
    public Notebook? Load(string filename)
    {
        try
        {
            byte[] encryptedBytes = File.ReadAllBytes(filename);

            // Extract salt and IV that were prepended during encryption
            const int saltLength = 16;
            const int ivLength = 16; // AES block size = 128 bits = 16 bytes

            if (encryptedBytes.Length < saltLength + ivLength)
                throw new ArgumentException("Invalid encrypted data");

            byte[] salt = new byte[saltLength];
            Array.Copy(encryptedBytes, 0, salt, 0, saltLength);

            byte[] iv = new byte[ivLength];
            Array.Copy(encryptedBytes, saltLength, iv, 0, ivLength);

            int ciphertextOffset = saltLength + ivLength;
            int ciphertextLength = encryptedBytes.Length - ciphertextOffset;
            byte[] ciphertext = new byte[ciphertextLength];
            Array.Copy(encryptedBytes, ciphertextOffset, ciphertext, 0, ciphertextLength);

            // Derive the same key from password+salt

            byte[] key = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);

            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(ciphertext, 0, ciphertext.Length);
                cs.FlushFinalBlock();
            }

            return JsonHandler.FromJson( Encoding.UTF8.GetString(ms.ToArray()));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"The decryption failed. {ex}");
            return null;
        }
    }

}
