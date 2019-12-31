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
            if (GetCaller(sender).PlacementTarget is Button)
            {
                var parent = ((GetCaller(sender).PlacementTarget as Button).Parent as Grid).Parent;
                if (parent is TreeElement)
                    (parent as TreeElement).deleteButton_Click(sender, e);
            }
            else if (GetCaller(sender).PlacementTarget is Grid)
            {
                (((sender as MenuItem).Parent as ContextMenu).Tag as IDropableInstruction).Remove();
            }
        }

        private void editMenu_click(object sender, RoutedEventArgs e)
        {
            if (GetCaller(sender).PlacementTarget is Button)
            {
                var parent = ((GetCaller(sender).PlacementTarget as Button).Parent as Grid).Parent;
                if (parent is TreeElement)
                    (parent as TreeElement).editButton_Click(sender, e);
            }
            else if (GetCaller(sender).PlacementTarget is Grid)
            {
                //(((sender as MenuItem).Parent as ContextMenu).Tag as IDropableInstruction).Edit();
            }
        }

        private ContextMenu GetCaller(object menuItem) => (menuItem as MenuItem).Parent as ContextMenu;

        private void copyMenu_Click(object sender, RoutedEventArgs e)
        {
        }

        private void cutMenu_Click(object sender, RoutedEventArgs e)
        {
        }

        private void undoMenu_Click(object sender, RoutedEventArgs e)
        {
            App.Undo();
        }

        private void redoMenu_Click(object sender, RoutedEventArgs e)
        {
            App.Redo();
        }

        private void pasteMenu_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}