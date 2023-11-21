using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SiamSyntax.Views
{
    public partial class NoteWindow : Window
    {
        public NoteWindow()
        {
            InitializeComponent();
        }

        private void DataGridPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dg = sender as DataGrid;

            var clickedRow = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (clickedRow != null && clickedRow.IsSelected)
            {
                dg.SelectedItem = null;
                e.Handled = true;
            }
        }
    }
}