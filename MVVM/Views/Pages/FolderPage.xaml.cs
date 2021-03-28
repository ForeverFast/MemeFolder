using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MemeFolder.MVVM.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для FolderPage.xaml
    /// </summary>
    public partial class FolderPage : Page
    {
        public FolderPage()
        {
            InitializeComponent();
        }

        private void empListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = sender;
            var parent = ((ListBox)sender).Parent as UIElement;
            parent.RaiseEvent(eventArg);
        }
    }
}
