using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ilaGUI
{
    public interface IDropableInstruction
    {
        bool DropVisual { set; }
        bool MovingVisual { set; }

        public void DropRecieved(IDropableInstruction droppedOnto)
        {
            var this_control = this as Control;
            var this_pannel = this_control.Parent as StackPanel;
            var this_parent = this_pannel.Tag as InstructionBlock;
            var other_control = droppedOnto as Control;
            var other_pannel = other_control.Parent as StackPanel;
            var other_parent = other_pannel.Tag as InstructionBlock;

            other_pannel.Children.Remove(other_control);
            other_parent.UpdateInternalInstructions();

            this_pannel.Children.Insert(this_pannel.Children.IndexOf(this_control), other_control);
            this_parent.UpdateInternalInstructions();
        }

        void UpdateVisuals();
    }
}