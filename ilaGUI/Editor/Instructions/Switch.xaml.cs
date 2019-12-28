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
    /// Logique d'interaction pour Case.xaml
    /// </summary>
    public partial class Switch : UserControl, Linked, InstructionBlock, IDropableInstruction
    {
        public Switch()
        {
            InitializeComponent();
            EndInstruction = new DummyInstruction();
            icon.Source = App.MakeDarkTheme(icon.Source as BitmapSource);
            EndInstruction = new DummyInstruction();
            defaultInstructions.Tag = this;
            Cases = new List<Case>();
        }

        public List<Case> Cases { get; set; }

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

        public IDropableInstruction[] Instructions
        {
            get
            {
                var list = new List<IDropableInstruction>();
                foreach (var item in Cases)
                    list.AddRange(item.instructions.Children.Cast<IDropableInstruction>());
                list.AddRange(defaultInstructions.Children.Cast<IDropableInstruction>());
                return list.ToArray();
            }
        }

        public ILANET.Switch InternalInstruction { get; set; }

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
        }

        public void UpdateVisuals()
        {
            comment.Text = InternalInstruction.Comment;
            endComment.Text = InternalInstruction.EndComment;
            for (int i = 0; i < InternalInstruction.Cases.Count; i++)
            {
                Cases[i].conditions.Children.Clear();
                for (int j = 0; j < InternalInstruction.Cases[i].Item1.Count; j++)
                {
                    Cases[i].conditions.Children.Add(App.GetValueControl(InternalInstruction.Cases[i].Item1[j]));
                    if (j < InternalInstruction.Cases[i].Item1.Count - 1)
                        Cases[i].conditions.Children.Add(new TextBlock { Text = ", ", Foreground = new SolidColorBrush(Colors.OrangeRed), FontFamily = comment.FontFamily });
                }
            }
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

        private void hitbox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                App.Dragged = this;
                DragDrop.DoDragDrop(this, "", DragDropEffects.Move);
            }
        }

        public struct Case
        {
            public StackPanel conditions;
            public DummyInstruction EndInstruction;
            public StackPanel instructions;
        }
    }
}