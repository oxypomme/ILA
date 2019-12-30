using ILANET;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Logique d'interaction pour DoWhile.xaml
    /// </summary>
    public partial class DoWhile : UserControl, Linked, InstructionBlock, IDropableInstruction
    {
        public DoWhile()
        {
            InitializeComponent();
            EndInstruction = new DummyInstruction();
            icon.Source = App.MakeDarkTheme(icon.Source as BitmapSource);
            EndInstruction = new DummyInstruction();
            instructions.Tag = this;
            hitbox.ContextMenu.Tag = this;
        }

        public bool DropVisual
        {
            set
            {
                if (value)
                    insertHere.Visibility = Visibility.Visible;
                else
                    insertHere.Visibility = Visibility.Collapsed;
            }
        }

        public DummyInstruction EndInstruction { get; set; }
        public IDropableInstruction[] Instructions => instructions.Children.Cast<IDropableInstruction>().ToArray();
        public ILANET.DoWhile InternalInstruction { get; set; }
        IBaseObject Linked.Link => InternalInstruction;

        public bool MovingVisual
        {
            set
            {
                if (value)
                    Background = new SolidColorBrush(Color.FromArgb(75, 50, 150, 250));
                else
                    Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
            }
        }

        public void UpdateInternalInstructions()
        {
            InternalInstruction.Instructions.Clear();
            foreach (var item in instructions.Children)
                if (!(item is DummyInstruction))
                    InternalInstruction.Instructions.Add((item as Linked).Link as Instruction);
        }

        public void UpdateVisuals()
        {
            comment.Text = string.IsNullOrEmpty(InternalInstruction.Comment) ? "" : "//" + InternalInstruction.Comment;
            endComment.Text = string.IsNullOrEmpty(InternalInstruction.EndComment) ? "" : "//" + InternalInstruction.EndComment;
            condHolder.Content = App.GetValueControl(InternalInstruction.Condition);
        }

        private void hitbox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat) && (string)e.Data.GetData(DataFormats.StringFormat) == "" && App.Dragged != this)
            {
                DropVisual = true;
                (App.Dragged as IDropableInstruction).MovingVisual = true;
            }
        }

        private void hitbox_DragLeave(object sender, DragEventArgs e)
        {
            DropVisual = false;
            (App.Dragged as IDropableInstruction).MovingVisual = false;
        }

        private void hitbox_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.StringFormat) && (string)e.Data.GetData(DataFormats.StringFormat) == "" && App.Dragged != this)
            {
                DropVisual = true;
                (App.Dragged as IDropableInstruction).MovingVisual = true;
                e.Effects = DragDropEffects.Move;
            }
        }

        private void hitbox_Drop(object sender, DragEventArgs e)
        {
            DropVisual = false;
            (App.Dragged as IDropableInstruction).MovingVisual = false;
            if (App.Dragged is InstructionBlock block && App.recursiveSearch(block.Instructions, this))
                return;
            if (e.Data.GetDataPresent(DataFormats.StringFormat) && (string)e.Data.GetData(DataFormats.StringFormat) == "" && App.Dragged != this)
                (this as IDropableInstruction).DropRecieved(App.Dragged as IDropableInstruction);
        }

        private void hitbox_MouseEnter(object sender, MouseEventArgs e)
        {
            hitbox.Background = new SolidColorBrush(Color.FromArgb(64, 255, 255, 255));
        }

        private void hitbox_MouseLeave(object sender, MouseEventArgs e)
        {
            hitbox.Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
        }

        private void hitbox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                App.Dragged = this;
                DragDrop.DoDragDrop(this, "", DragDropEffects.Move);
            }
        }
    }
}