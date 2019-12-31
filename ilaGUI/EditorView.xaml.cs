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
            EndInstruction = new Editor.DummyInstruction() { Height = 150 };

            //foreach (MenuItem menuItem in ContextMenu.Items)
            //{
            //    (menuItem.Icon as Image).Source = App.MakeDarkTheme((menuItem.Icon as Image).Source as BitmapSource);
            //}
        }

        public Editor.DummyInstruction EndInstruction { get; set; }

        public IDropableInstruction[] Instructions => throw new NotImplementedException();

        public void UpdateInternalInstructions()
        {
            var instrus = new List<ILANET.Instruction>();
            {
                if (App.CurrentExecutable is ILANET.Program prog)
                    prog.Instructions.Clear();
                else if (App.CurrentExecutable is ILANET.Module mod)
                    mod.Instructions.Clear();
            }
            foreach (var item in instructions.Children)
                if (!(item is Editor.DummyInstruction))
                    instrus.Add((item as Linked).Link as ILANET.Instruction);
            {
                if (App.CurrentExecutable is ILANET.Program prog)
                    prog.Instructions = instrus;
                else if (App.CurrentExecutable is ILANET.Module mod)
                    mod.Instructions = instrus;
            }
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