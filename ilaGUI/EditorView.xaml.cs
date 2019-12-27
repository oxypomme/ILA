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

namespace ilaGUI
{
    /// <summary>
    /// Logique d'interaction pour EditorView.xaml
    /// </summary>
    public partial class EditorView : UserControl, InstructionBlock
    {
        public static UIElement draggedElement;

        public EditorView()
        {
            InitializeComponent();
            instructions.Tag = this;
            EndInsturction = new Editor.DummyInstruction();
        }

        public Editor.DummyInstruction EndInsturction { get; set; }

        public void UpdateInternalInstructions()
        {
            //instructions.Children stuff
        }

        private void instructions_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
        }

        private void instructions_Drop(object sender, DragEventArgs e)
        {
        }
    }
}