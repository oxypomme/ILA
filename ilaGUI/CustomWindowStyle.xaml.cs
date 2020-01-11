using System.Windows;

namespace ilaGUI
{
    public partial class CustomWindowStyle
    {
        public CustomWindowStyle()
        {
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            ((Window)((FrameworkElement)sender).TemplatedParent).Close();
        }

        private void MaximizeRestoreClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            if (window.WindowState == WindowState.Normal)
                window.WindowState = WindowState.Maximized;
            else
                window.WindowState = WindowState.Normal;
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            ((Window)((FrameworkElement)sender).TemplatedParent).WindowState = WindowState.Minimized;
        }
    }
}