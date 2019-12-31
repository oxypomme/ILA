using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ilaGUI.Editor;

namespace ilaGUI
{
    public partial class ResourcesDic
    {
        private void deleteMenu_click(object sender, RoutedEventArgs e)
        {
            if (GetCaller(sender) is Button)
            {
                var parent = ((GetCaller(sender) as Button).Parent as Grid).Parent;
                if (parent is TreeElement)
                    (parent as TreeElement).deleteButton_Click(sender, e);
            }
            else if (GetCaller(sender) is Grid)
            {
                var parent = ((GetCaller(sender) as Grid).Parent as Grid).Parent;
                //if (parent is Assign)
                //    (parent as Assign).deleteButton_Click(sender, e);
                //else if (parent is Comment)
                //    (parent as Comment).deleteButton_Click(sender, e);
                //else if (parent is DoWhile)
                //    (parent as DoWhile).deleteButton_Click(sender, e);
                //else if (parent is For)
                //    (parent as For).deleteButton_Click(sender, e);
                //else if (parent is If)
                //    (parent as If).deleteButton_Click(sender, e);
                //else if (parent is ModuleCall)
                //    (parent as ModuleCall).deleteButton_Click(sender, e);
                //else if (parent is Return)
                //    (parent as Return).deleteButton_Click(sender, e);
                //else if (parent is Switch)
                //    (parent as Switch).deleteButton_Click(sender, e);
                //else if (parent is While)
                //    (parent as While).deleteButton_Click(sender, e);
            }
        }

        private void editMenu_click(object sender, RoutedEventArgs e)
        {
            if (GetCaller(sender) is Button)
            {
                var parent = ((GetCaller(sender) as Button).Parent as Grid).Parent;
                if (parent is TreeElement)
                    (parent as TreeElement).editButton_Click(sender, e);
            }
            else if (GetCaller(sender) is Grid)
            {
                var parent = ((GetCaller(sender) as Grid).Parent as Grid).Parent;
                //if (parent is Assign)
                //    (parent as Assign).editButton_Click(sender, e);
                //else if (parent is Comment)
                //    (parent as Comment).editButton_Click(sender, e);
                //else if (parent is DoWhile)
                //    (parent as DoWhile).editButton_Click(sender, e);
                //else if (parent is For)
                //    (parent as For).editButton_Click(sender, e);
                //else if (parent is If)
                //    (parent as If).editButton_Click(sender, e);
                //else if (parent is ModuleCall)
                //    (parent as ModuleCall).editButton_Click(sender, e);
                //else if (parent is Return)
                //    (parent as Return).editButton_Click(sender, e);
                //else if (parent is Switch)
                //    (parent as Switch).editButton_Click(sender, e);
                //else if (parent is While)
                //    (parent as While).editButton_Click(sender, e);
            }
        }

        private UIElement GetCaller(object menuItem) => ((menuItem as MenuItem).Parent as ContextMenu).PlacementTarget;

        private void copyMenu_Click(object sender, RoutedEventArgs e)
        {
        }

        private void cutMenu_Click(object sender, RoutedEventArgs e)
        {
        }

        private void undoMenu_Click(object sender, RoutedEventArgs e)
        {
        }

        private void redoMenu_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}