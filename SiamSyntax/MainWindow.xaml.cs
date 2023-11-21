using SiamSyntax.ViewModels;
using System.Windows;

namespace SiamSyntax
{
    public partial class MainWindow : Window
    {
        public MenuViewModel MenuViewModel { get; set; }
        public NoteListViewModel NoteListViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            string notesDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LearnThaï");
            NoteListViewModel = new NoteListViewModel(notesDirectory);
            MenuViewModel = new MenuViewModel(NoteListViewModel);

            menu.DataContext = MenuViewModel;
            noteList.DataContext = NoteListViewModel;
        }
    }
}