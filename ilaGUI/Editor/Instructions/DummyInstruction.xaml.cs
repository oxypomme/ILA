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

            for (int i = 0; i < hitbox.ContextMenu.Items.Count; i++)
            {
                MenuItem menuItem;
                try
                {
                    menuItem = (MenuItem)hitbox.ContextMenu.Items[i];
                }
                catch (InvalidCastException) { i++; menuItem = (MenuItem)hitbox.ContextMenu.Items[i]; }
                if (menuItem.Icon != null)
                    (menuItem.Icon as Image).Source = App.MakeDarkTheme((menuItem.Icon as Image).Source as BitmapSource);

                for (int j = 0; j < menuItem.Items.Count; j++)
                {
                    MenuItem subMenuItem;
                    try
                    {
                        subMenuItem = (MenuItem)menuItem.Items[j];
                    }
                    catch (InvalidCastException) { j++; subMenuItem = (MenuItem)menuItem.Items[j]; }
                    if (subMenuItem.Icon != null)
                        (subMenuItem.Icon as Image).Source = App.MakeDarkTheme((subMenuItem.Icon as Image).Source as BitmapSource);
                }
            }
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

        public bool MovingVisual { get; set; }

        public void UpdateVisuals()
        {
        }

        private void UserControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat) && (string)e.Data.GetData(DataFormats.StringFormat) == "")
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
            if (e.Data.GetDataPresent(DataFormats.StringFormat) && (string)e.Data.GetData(DataFormats.StringFormat) == "")
                e.Effects = DragDropEffects.Move;
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            DropVisual = false;
            (App.Dragged as IDropableInstruction).MovingVisual = false;
            if (App.Dragged is InstructionBlock block && App.recursiveSearch(block.Instructions, this))
                return;
            if (e.Data.GetDataPresent(DataFormats.StringFormat) && (string)e.Data.GetData(DataFormats.StringFormat) == "")
                (this as IDropableInstruction).DropRecieved(App.Dragged as IDropableInstruction);
        }
    }
}