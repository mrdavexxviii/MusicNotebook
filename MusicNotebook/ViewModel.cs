using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using MusicNotebook.NotebookDefinitions;
using MusicNotebook.Serialisation;
using System.Windows;

namespace MusicNotebook;

public partial class ViewModel : ObservableObject
{
    public event Action<INotebookPage>? RequestEditPageName;

    const string unprotectedExtension = ".umnb";
    const string protectedExtension = ".mnb";
    readonly string filterString = $"Music Notebook files (*{protectedExtension})|*{protectedExtension}|Unprotected Music Notebook files (*{unprotectedExtension})|*{unprotectedExtension}|All files (*.*)|*.*";
    
    [ObservableProperty]
    private string _currentFilename = string.Empty;
    private string currentFileChecksum = string.Empty;

    [ObservableProperty]
    private Notebook _noteBook;
 
    readonly private PasswordService _passwordService = new();

    public ViewModel()
    {
        NoteBook = new Notebook();
        CurrentFilename = string.Empty;
        currentFileChecksum = ModelChecksum.Checksum(NoteBook);
    }

    public bool CanOverwrite()
    {
        if (currentFileChecksum == ModelChecksum.Checksum(NoteBook))
        {
            return true;
        }
        else
        {
            return MessageBox.Show("Current notebook has unsaved changes. Do you want to discard them?", "Unsaved Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }
    }

    [RelayCommand]
    private void New()
    {
        if (CanOverwrite())
        {
            NoteBook = new Notebook();
            CurrentFilename = string.Empty;
            currentFileChecksum = ModelChecksum.Checksum(NoteBook);
        }
    }

    private EncryptedSerialiser? InitialiseEncryptedSerialiser()
    {
        //if (!_passwordService.ValidPassword)
        //{
        //    if (!_passwordService.RefreshPassword())
        //    {
        //        return null;
        //    }
        //}
        return new EncryptedSerialiser(_passwordService);
        
    }

    private ISerialiser? GetSerialiserForFile(string filename)
    {
        string extension = System.IO.Path.GetExtension(filename).ToLower();
        if (extension == unprotectedExtension)
        {
            return new UnencryptedSerialiser();
        }
        else
        {
            return InitialiseEncryptedSerialiser();
        }
    }

    [RelayCommand]
    private void Open()
    {
        if (!CanOverwrite())
        {
            return;
        }
        FileDialog dialog = new OpenFileDialog
        {
            Filter = filterString
        };

        if (dialog.ShowDialog() == true)
        {
            ISerialiser? serialiser = GetSerialiserForFile(dialog.FileName);
            if (serialiser != null)
            {
                var notebook = serialiser.Load(dialog.FileName);
                if (notebook != null)
                {
                    NoteBook = notebook;
                    this.CurrentFilename = dialog.FileName;
                    this.currentFileChecksum = ModelChecksum.Checksum(NoteBook);
                    notebook.SelectedPage = notebook.Pages.Count > 0 ? notebook.Pages[0] : null;
                }
            } else
            {
                MessageBox.Show("Unable to open file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }



    [RelayCommand]
    void SaveAs()
    {
        FileDialog dialog = new SaveFileDialog
        {
            Filter = filterString
        };
        if (dialog.ShowDialog() == true)
        {
            this.CurrentFilename = dialog.FileName;
            Save();
        }
    }
    [RelayCommand]
    void Save()
    {

        if (string.IsNullOrEmpty( CurrentFilename))
        {
            SaveAs();
            return;
        }
        ISerialiser? serialiser = GetSerialiserForFile(CurrentFilename);
        if (serialiser != null)
        {
            if (serialiser.Save(CurrentFilename, NoteBook))
            {
                currentFileChecksum = ModelChecksum.Checksum(NoteBook);
            } else
            {
                CurrentFilename = string.Empty;
            }
        }
        else
        {
            CurrentFilename = string.Empty;
            MessageBox.Show("Unable to save file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    void AddTextPage()
    {
        
            AddPage(new TextPage { Name = "New Text Page", Content = "" });
        
    }
    [RelayCommand]
    void AddImagePage()
    {

        AddPage(new ImagePage { Name = "New Image Page", Content = "" });

    }

    [RelayCommand]
    void DeleteCurrentPage()
    {
        if (NoteBook.SelectedPage != null)
        {
            if (MessageBox.Show("Delete current page", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                NoteBook.Pages.Remove(NoteBook.SelectedPage);
            }
        }
    }

    void AddPage(INotebookPage page)
    {
        NoteBook.Pages.Add(page);

        NoteBook.SelectedPage = page; // select newly added tab
        RequestEditPageName?.Invoke(page);
    }

    [RelayCommand]
     void Properties()
    {
        if (NoteBook.SelectedPage != null)
        {
            if (NoteBook.SelectedPage is TextPage textPage)
            {
                //var vm = new TextPagePropertiesViewModel(textPage);
                //var view = new TextPagePropertiesView { DataContext = vm };
                //view.ShowDialog();
            }
             else if (NoteBook.SelectedPage is ImagePage imagePage)
            {
                var vm = new ImagePagePropertiesViewModel(imagePage);
                var view = new ImagePagePropertiesView { DataContext = vm };
                view.ShowDialog();
            }
        }
    }

}
