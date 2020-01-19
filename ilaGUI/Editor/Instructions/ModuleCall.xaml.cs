using ILANET;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Logique d'interaction pour ModuleCall.xaml
    /// </summary>
    public partial class ModuleCall : UserControl, Linked, IDropableInstruction
    {
        public ModuleCall()
        {
            InitializeComponent();
            App.DarkmodeUrMenus(mainGrid.ContextMenu.Items);
            mainGrid.ContextMenu.Tag = this;
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

        public ILANET.ModuleCall InternalInstruction { get; set; }
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

        public void UpdateVisuals()
        {
            if (InternalInstruction.CalledModule is Function)
                icon.Source = App.MakeDarkTheme(App.GetBitmapImage(new MemoryStream(Properties.Resources.function)));
            else
                icon.Source = App.MakeDarkTheme(App.GetBitmapImage(new MemoryStream(Properties.Resources.module)));
            moduleName.Text = InternalInstruction.CalledModule.Name;
            comment.Text = string.IsNullOrEmpty(InternalInstruction.Comment) ? "" : "//" + InternalInstruction.Comment;
            parameters.Children.Clear();
            for (int i = 0; i < InternalInstruction.Args.Count; i++)
            {
                parameters.Children.Add(App.GetValueControl(InternalInstruction.Args[i]));
                if (i < InternalInstruction.Args.Count - 1)
                    parameters.Children.Add(new TextBlock
                    {
                        Text = ", ",
                        Foreground = App.SymbolColorBrush,
                        FontFamily = comment.FontFamily
                    });
            }
        }

        private void UserControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat) && (string)e.Data.GetData(DataFormats.StringFormat) == "" && App.Dragged != this)
            {
                DropVisual = true;
                (App.Dragged as IDropableInstruction).MovingVisual = true;
            }
        }

        private void UserControl_DragLeave(object sender, DragEventArgs e)
        {
            DropVisual = false;
            (App.Dragged as IDropableInstruction).MovingVisual = false;
        }

        private void UserControl_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.StringFormat) && (string)e.Data.GetData(DataFormats.StringFormat) == "" && App.Dragged != this)
                e.Effects = DragDropEffects.Move;
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            DropVisual = false;
            (App.Dragged as IDropableInstruction).MovingVisual = false;
            if (App.Dragged is InstructionBlock block && App.recursiveSearch(block.Instructions, this))
                return;
            if (e.Data.GetDataPresent(DataFormats.StringFormat) && (string)e.Data.GetData(DataFormats.StringFormat) == "" && App.Dragged != this)
                (this as IDropableInstruction).DropRecieved(App.Dragged as IDropableInstruction);
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Background = new SolidColorBrush(Color.FromArgb(64, 255, 255, 255));
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                App.Dragged = this;
                DragDrop.DoDragDrop(this, "", DragDropEffects.Move);
            }
        }
    }
}