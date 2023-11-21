using SiamSyntax.Commands;
using SiamSyntax.Models;
using SiamSyntax.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SiamSyntax.ViewModels
{
    public class NoteListViewModel : INotifyPropertyChanged
    {
        private readonly FileNoteService fileNoteService;
        public ObservableCollection<Note> Notes { get; }

        private Note _selectedNote;
        public Note SelectedNote
        {
            get { return _selectedNote; }
            set
            {
                if (_selectedNote != value)
                {
                    _selectedNote = value;
                    OnPropertyChanged(nameof(SelectedNote));
                }
            }
        }

        public ICommand OpenNoteCommand { get; }

        public NoteListViewModel(string notesDirectory)
        {
            fileNoteService = new FileNoteService(notesDirectory);
            Notes = new ObservableCollection<Note>(fileNoteService.GetNotes());
            OpenNoteCommand = new RelayCommand(OpenNoteExecute, OpenNoteCanExecute);
        }

        private bool OpenNoteCanExecute(object parameter)
        {
            return parameter is Note;
        }

        private void OpenNoteExecute(object parameter)
        {
            if (parameter is Note note)
            {
                if (SelectedNote == note && note.IsSelected)
                {
                    note.IsSelected = false;
                    SelectedNote = null;
                }
                else
                {
                    foreach (var n in Notes)
                    {
                        n.IsSelected = false;
                    }

                    note.IsSelected = true;
                    SelectedNote = note;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}