using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ilaGUI
{
    public partial class ResourcesDic
    {
        private void deleteMenu_click(object sender, RoutedEventArgs e)
        {
        }

        private void editMenu_click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(GetCaller(sender));
        }

        private IDropableInstruction GetCaller(object menuItem) => ((menuItem as MenuItem).Parent as ContextMenu).Tag as IDropableInstruction;
    }
}