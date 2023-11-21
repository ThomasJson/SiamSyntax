using SiamSyntax.Commands;
using SiamSyntax.Helpers;
using SiamSyntax.Models;
using SiamSyntax.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace SiamSyntax.ViewModels
{
    public class NoteWindowViewModel : INotifyPropertyChanged
    {
        public string Title { get; set; }
        private string _content;
        public string Content
        {
            get { return _content; }
            set
            {
                if (_content != value)
                {
                    _content = value;
                    OnPropertyChanged(nameof(Content));

                    UpdateArray(_content);
                }
            }
        }
        private bool _isAddButtonEnabled = true;
        public bool IsAddButtonEnabled
        {
            get => _isAddButtonEnabled;
            set
            {
                if (_isAddButtonEnabled != value)
                {
                    _isAddButtonEnabled = value;
                    OnPropertyChanged(nameof(IsAddButtonEnabled));
                }
            }
        }
        private bool _isUpdateButtonEnabled = false;
        public bool IsUpdateButtonEnabled
        {
            get => _isUpdateButtonEnabled;
            set
            {
                if (_isUpdateButtonEnabled != value)
                {
                    _isUpdateButtonEnabled = value;
                    OnPropertyChanged(nameof(IsUpdateButtonEnabled));
                }
            }
        }
        private bool _isDeleteButtonEnabled = false;
        public bool IsDeleteButtonEnabled
        {
            get => _isDeleteButtonEnabled;
            set
            {
                if (_isDeleteButtonEnabled != value)
                {
                    _isDeleteButtonEnabled = value;
                    OnPropertyChanged(nameof(IsDeleteButtonEnabled));
                }
            }
        }
        public ObservableCollection<TranslationEntry> Translations { get; set; } = new ObservableCollection<TranslationEntry>();
        public string DisplayedEnglishTranslation { get; set; }
        public string DisplayedThaiTranslation { get; set; }
        private TranslationEntry _selectedTranslation;
        public TranslationEntry SelectedTranslation
        {
            get { return _selectedTranslation; }
            set
            {
                _selectedTranslation = value;
                OnPropertyChanged(nameof(SelectedTranslation));

                if (_selectedTranslation != null)
                {
                    NewEnglishTranslation = _selectedTranslation.English;
                    NewThaiTranslation = _selectedTranslation.Thai;
                }
                else
                {
                    NewEnglishTranslation = string.Empty;
                    NewThaiTranslation = string.Empty;
                }

                UpdateButtonStates();
            }
        }
        private string _newEnglishTranslation;
        public string NewEnglishTranslation
        {
            get { return _newEnglishTranslation; }
            set
            {
                if (_newEnglishTranslation != value)
                {
                    _newEnglishTranslation = value;
                    OnPropertyChanged(nameof(NewEnglishTranslation));
                }
            }
        }

        private string _newThaiTranslation;
        public string NewThaiTranslation
        {
            get { return _newThaiTranslation; }
            set
            {
                if (_newThaiTranslation != value)
                {
                    _newThaiTranslation = value;
                    OnPropertyChanged(nameof(NewThaiTranslation));
                }
            }
        }

        public ICommand AddOrUpdateCommand { get; }
        public ICommand DeleteCommand { get; private set; }
        private void AddOrUpdateTranslation(object obj)
        {
            if (SelectedTranslation != null)
            {
                SelectedTranslation.English = NewEnglishTranslation;
                SelectedTranslation.Thai = NewThaiTranslation;

                UpdateContentWithTranslations();

                SaveNoteChanges();

                int index = Translations.IndexOf(SelectedTranslation);

                if (index != -1)
                {
                    Translations[index] = SelectedTranslation;
                }

                else
                {
                    return;
                }
            }

            else
            {
                AddNewTranslation();
            }
        }

        private bool CanAddOrUpdateTranslation(object obj)
        {
            return true;
        }

        private void AddNewTranslation()
        {
            var newTranslationLine = $"{NewEnglishTranslation} | {NewThaiTranslation}";
            Content += "\n" + newTranslationLine;

            SaveNoteChanges();

            NewEnglishTranslation = string.Empty;
            NewThaiTranslation = string.Empty;
            OnPropertyChanged(nameof(NewEnglishTranslation));
            OnPropertyChanged(nameof(NewThaiTranslation));
        }

        private void UpdateContentWithTranslations()
        {
            var updatedContent = new StringBuilder();
            foreach (var translation in Translations)
            {
                updatedContent.AppendLine($"{translation.English} | {translation.Thai}");
            }

            Content = updatedContent.ToString().TrimEnd();
        }

        private void SaveNoteChanges()
        {
            var noteDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LearnThaï");
            var noteService = new FileNoteService(noteDirectory);
            var note = new Note { Title = Title, FilePath = "C:\\Users\\ThomasPy\\Documents\\LearnThaï\\" + Title + ".txt", Content = Content };
            noteService.UpdateNoteContent(note);
        }

        public NoteWindowViewModel(Note note)
        {
            Title = note.Title;
            Content = note.Content;
            AddOrUpdateCommand = new RelayCommand(AddOrUpdateTranslation, CanAddOrUpdateTranslation);
            DeleteCommand = new RelayCommand(DeleteTranslation, CanDeleteTranslation);
        }

        private void DeleteTranslation(object obj)
        {
            if (SelectedTranslation != null)
            {
                Translations.Remove(SelectedTranslation);
                UpdateContentWithTranslations();
                SaveNoteChanges();
                SelectedTranslation = null;
            }
        }
        private bool CanDeleteTranslation(object obj)
        {
            return SelectedTranslation != null;
        }

        private void UpdateArray(string content)
        {
            var parsedTranslations = TranslationParser.Parse(content);
            Translations.Clear();

            foreach (var translation in parsedTranslations)
            {
                Translations.Add(translation);
            }
        }

        private void UpdateButtonStates()
        {
            IsAddButtonEnabled = SelectedTranslation == null;
            IsUpdateButtonEnabled = SelectedTranslation != null;
            IsDeleteButtonEnabled = SelectedTranslation != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}