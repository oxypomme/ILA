using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
using ILANET;

namespace ilaGUI
{
    /// <summary>
    /// Logique d'interaction pour TreeElement.xaml
    /// </summary>
    public partial class TreeElement : UserControl, Linked
    {
        public TreeElement(IBaseObject linkedTo)
        {
            InitializeComponent();
            Link = linkedTo;
            if (linkedTo is Program pr)
            {
                Icon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.algo));
                Title.Content = pr.Name;
            }
            else if (linkedTo is Function fct)
            {
                Icon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.function));
                Title.Content = fct.Name;
            }
            else if (linkedTo is ILANET.Module mod)
            {
                Icon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.module));
                Title.Content = mod.Name;
            }
            Title.Foreground = App.DarkFontColor;
        }

        public IBaseObject Link { get; private set; }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }
    }
}