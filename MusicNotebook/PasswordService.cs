using MusicNotebookLibrary.Serialisation;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MusicNotebook
{

    internal class PasswordService : IPasswordService
    {
        private const string FileName = "credentials.dat";
        private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("MusicNotebookCredentialV1");
        private readonly string storagePath;

        public string Password { get; private set; } = string.Empty;
        public bool ValidPassword { get; set; }

        public PasswordService()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folder = Path.Combine(appData, "MusicNotebook");
            Directory.CreateDirectory(folder);
            storagePath = Path.Combine(folder, FileName);

            // Try to load a previously saved password for the current user
            TryLoadSavedPassword();
        }

        // Existing behaviour: prompt the user (UI) for a password
        public bool RefreshPassword()
        {
            PasswordDialogViewModel viewModel = new PasswordDialogViewModel();
            PasswordDialog dialog = new PasswordDialog
            {
                DataContext = viewModel
            };
            if (dialog.ShowDialog() == true)
            {
                Password = viewModel.Password;
                ValidPassword = true;
                if (viewModel.RememberPassword)
                {
                    SavePassword();
                }
                return true;
            }
            ValidPassword = false;
            Password = string.Empty;
            return false;
        }

        // Persist the current Password securely for the current user.
        // Returns true if saved successfully.
        public bool SavePassword()
        {
            try
            {
                if (string.IsNullOrEmpty(Password))
                    return false;

                byte[] plain = Encoding.UTF8.GetBytes(Password);
                byte[] encrypted = ProtectedData.Protect(plain, Entropy, DataProtectionScope.CurrentUser);
                File.WriteAllBytes(storagePath, encrypted);
                return true;
            }
            catch
            {
                // Don't throw from service; upper layers can show UI if needed.
                return false;
            }
        }

        // Remove any stored password.
        public bool ClearSavedPassword()
        {
            try
            {
                if (File.Exists(storagePath))
                    File.Delete(storagePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Attempt to load saved password from disk (user-scoped).
        // Returns true when a password was successfully loaded.
        public bool TryLoadSavedPassword()
        {
            try
            {
                if (!File.Exists(storagePath))
                    return false;

                byte[] encrypted = File.ReadAllBytes(storagePath);
                byte[] plain = ProtectedData.Unprotect(encrypted, Entropy, DataProtectionScope.CurrentUser);
                Password = Encoding.UTF8.GetString(plain);
                ValidPassword = true;
                return true;
            }
            catch
            {
                // Corrupt file or DPAPI failure - treat as missing
                Password = string.Empty;
                ValidPassword = false;
                return false;
            }
        }

        public void RefreshPasswordIfInvalid()
        {
            if (!ValidPassword)
            {
                RefreshPassword();
            }
        }

        public void NotifyInvalidPassword()
        {
            ValidPassword = false;
            Password = string.Empty;
        }
    }
}
