using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicNotebook
{
    internal partial class PasswordDialogViewModel : ObservableObject
    {
        public bool DialogResult { get; private set; }
        [ObservableProperty]
        private string _password = string.Empty;
    }
}
