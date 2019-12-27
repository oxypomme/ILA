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
    /// Logique d'interaction pour DummyInstruction.xaml
    /// </summary>
    public partial class DummyInstruction : UserControl, IDropableInstruction
    {
        public DummyInstruction()
        {
            InitializeComponent();
        }

        private void UserControl_DragEnter(object sender, DragEventArgs e)
        {
            outline.Stroke = new SolidColorBrush(Color.FromArgb(255, 50, 250, 150));
        }

        private void UserControl_DragLeave(object sender, DragEventArgs e)
        {
            outline.Stroke = new SolidColorBrush(Colors.White);
        }

        private void UserControl_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
        }
    }
}