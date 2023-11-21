using SiamSyntax.Commands;
using SiamSyntax.Views;
using System.ComponentModel;
using System.Windows.Input;

namespace SiamSyntax.ViewModels
{
    public class MenuViewModel : INotifyPropertyChanged
    {
        private readonly NoteListViewModel _notesListViewModel;
        private bool _isOpenNoteEnabled;

        public bool IsOpenNoteEnabled
        {
            get { return _isOpenNoteEnabled; }
            set
            {
                if (_isOpenNoteEnabled != value)
                {
                    _isOpenNoteEnabled = value;
                    OnPropertyChanged(nameof(IsOpenNoteEnabled));
                }
            }
        }

        public ICommand OpenSelectedNoteCommand { get; }

        public MenuViewModel(NoteListViewModel notesViewerViewModel)
        {
            _notesListViewModel = notesViewerViewModel;
            OpenSelectedNoteCommand = new RelayCommand(OpenSelectedNoteExecute, OpenSelectedNoteCanExecute);
            _notesListViewModel.PropertyChanged += NoteListViewModel_PropertyChanged;
        }

        private bool OpenSelectedNoteCanExecute(object parameter)
        {
            return _notesListViewModel.SelectedNote != null;
        }

        private void OpenSelectedNoteExecute(object parameter)
        {
            var noteWindow = new NoteWindow();
            noteWindow.DataContext = new NoteWindowViewModel(_notesListViewModel.SelectedNote);
            noteWindow.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void NoteListViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_notesListViewModel.SelectedNote))
            {
                IsOpenNoteEnabled = _notesListViewModel.SelectedNote != null;
            }
        }
    }
}