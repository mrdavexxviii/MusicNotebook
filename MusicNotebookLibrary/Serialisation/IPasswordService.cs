using System;
using System.Collections.Generic;
using System.Text;

namespace MusicNotebookLibrary.Serialisation
{
    public interface IPasswordService
    {
        string Password { get; }
        bool ValidPassword { get; }
        void RefreshPasswordIfInvalid();
        bool RefreshPassword();
        bool SavePassword();
        bool ClearSavedPassword();
        bool TryLoadSavedPassword();
        void NotifyInvalidPassword();
    }
}
