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

            App.DarkmodeUrMenus(hitbox.ContextMenu.Items);

            hitbox.ContextMenu.Items.Remove(hitbox.ContextMenu.Items.GetItemAt(5)); // Remove Cut from contextmenu
            hitbox.ContextMenu.Items.Remove(hitbox.ContextMenu.Items.GetItemAt(4)); // Remove Copy from contextmenu
            hitbox.ContextMenu.Items.Remove(hitbox.ContextMenu.Items.GetItemAt(3)); // Remove a separator from contextmenu
            hitbox.ContextMenu.Items.Remove(hitbox.ContextMenu.Items.GetItemAt(2)); // Remove Delete from contextmenu
            hitbox.ContextMenu.Items.Remove(hitbox.ContextMenu.Items.GetItemAt(0)); // Remove Edit from contextmenu
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