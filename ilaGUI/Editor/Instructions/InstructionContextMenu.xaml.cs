using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ilaGUI.Editor
{
    /// <summary>
    /// Logique d'interaction pour InstructionContextMenu.xaml
    /// </summary>
    public partial class InstructionContextMenu : UserControl
    {
        public InstructionContextMenu()
        {
            InitializeComponent();

            (undoMenu.Icon as Image).Source = App.MakeDarkTheme((undoMenu.Icon as Image).Source as BitmapSource);
            (redoMenu.Icon as Image).Source = App.MakeDarkTheme((redoMenu.Icon as Image).Source as BitmapSource);
            (cutMenu.Icon as Image).Source = App.MakeDarkTheme((cutMenu.Icon as Image).Source as BitmapSource);
            (copyMenu.Icon as Image).Source = App.MakeDarkTheme((copyMenu.Icon as Image).Source as BitmapSource);
            (editMenu.Icon as Image).Source = App.MakeDarkTheme((editMenu.Icon as Image).Source as BitmapSource);
            (deleteMenu.Icon as Image).Source = App.MakeDarkTheme((deleteMenu.Icon as Image).Source as BitmapSource);
        }
    }
}