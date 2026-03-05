using System;
using System.Collections.Generic;
using System.Text;

namespace MusicNotebook
{
    internal class PasswordService 
    {
        public string Password { get; private set; } = string.Empty;
        public bool ValidPassword { get; set; }
        public bool RefreshPassword()
        {
            PasswordDialogViewModel viewModel = new PasswordDialogViewModel();
            PasswordDialog dialog = new PasswordDialog
            {
                DataContext = viewModel
            };
            if(dialog.ShowDialog() == true)
            {
                Password = viewModel.Password;
                ValidPassword = true;
                return true;
            }
            return false;
        }
    }
}
