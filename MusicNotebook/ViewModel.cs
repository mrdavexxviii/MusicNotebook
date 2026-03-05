using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using MusicNotebook.NotebookDefinitions;
using MusicNotebookLibrary.Serialisation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MusicNotebook
{
    public partial class ViewModel : ObservableObject
    {
        const string unprotectedExtension = ".umnb";
        const string protectedExtension = ".mnb";
        readonly string filterString = $"Music Notebook files (*{protectedExtension})|*{protectedExtension}|Unprotected Music Notebook files (*{unprotectedExtension})|*{unprotectedExtension}|All files (*.*)|*.*";

        [ObservableProperty]
        private string _currentFilename = string.Empty;

        [ObservableProperty]
        private Notebook _noteBook= new Notebook();
     
        private PasswordService _passwordService = new PasswordService();

        [RelayCommand]
        private void New()
        {
            NoteBook = new Notebook();  
            CurrentFilename = string.Empty;
        }

        private ISerialiser? InitialiseEncryptedSerialiser()
        {
            if (!_passwordService.ValidPassword)
            {
                if (!_passwordService.RefreshPassword())
                {
                    return null;
                }
            }
            return new EncryptedSerialiser(_passwordService.Password);
            
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
            
            FileDialog dialog = new OpenFileDialog();
            dialog.Filter = filterString;
   
            if (dialog.ShowDialog() == true)
            {
                ISerialiser? serialiser = GetSerialiserForFile(dialog.FileName);
                if (serialiser != null)
                {
                    NoteBook = serialiser.Load(dialog.FileName);
                    this.CurrentFilename = dialog.FileName;
                } else
                {
                    MessageBox.Show("Unable to open file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        [RelayCommand]
        void SaveAs()
        {
            FileDialog dialog = new SaveFileDialog();
            dialog.Filter = filterString;
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
                serialiser.Save(CurrentFilename, NoteBook);
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
        }


    }
}
