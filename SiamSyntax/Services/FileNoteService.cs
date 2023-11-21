using SiamSyntax.Models;
using System.Collections.Generic;
using System.IO;

namespace SiamSyntax.Services
{
    public class FileNoteService
    {
        private readonly string notesDirectory;

        public FileNoteService(string notesDirectory)
        {
            this.notesDirectory = notesDirectory;
        }

        public List<Note> GetNotes()
        {
            var notes = new List<Note>();

            var files = Directory.GetFiles(notesDirectory);

            foreach (var file in files)
            {

                var content = File.ReadAllText(file);
                var note = new Note
                {
                    Title = Path.GetFileNameWithoutExtension(file),
                    FilePath = file,
                    Content = content
                };

                notes.Add(note);
            }

            return notes;
        }

        public void UpdateNoteContent(Note note)
        {
            File.WriteAllText(note.FilePath, note.Content);
        }
    }
}