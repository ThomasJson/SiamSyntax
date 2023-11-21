using System.ComponentModel;

namespace SiamSyntax.Models
{
    public class TranslationEntry : INotifyPropertyChanged
    {
        private string _english;
        public string English
        {
            get { return _english; }
            set
            {
                _english = value;
                OnPropertyChanged(nameof(English));
            }
        }

        private string _thai;
        public string Thai
        {
            get { return _thai; }
            set
            {
                _thai = value;
                OnPropertyChanged(nameof(Thai));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}