using SiamSyntax.Models;
using System;
using System.Collections.ObjectModel;

namespace SiamSyntax.Helpers
{
    public static class TranslationParser
    {
        public static ObservableCollection<TranslationEntry> Parse(string content)
        {
            var translations = new ObservableCollection<TranslationEntry>();

            var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var parts = line.Split(new[] { " | " }, StringSplitOptions.None);
                if (parts.Length == 2)
                {
                    translations.Add(new TranslationEntry { English = parts[0].Trim(), Thai = parts[1].Trim() });
                }
            }

            return translations;
        }
    }
}