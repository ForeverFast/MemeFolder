using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MemeFolder.Pages
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
      

        //private bool IsUserVisible(FrameworkElement element, FrameworkElement container)
        //{
        //    if (!element.IsVisible)
        //        return false;

        //    Rect bounds = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
        //    Rect rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
        //    return rect.Contains(bounds.TopLeft) || rect.Contains(bounds.BottomRight);
        //}
    }
}
