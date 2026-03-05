using CommunityToolkit.Mvvm.ComponentModel;

namespace MusicNotebook;

internal partial class PasswordDialogViewModel : ObservableObject
{
    public bool DialogResult { get; private set; }
    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _rememberPassword = false;
}
