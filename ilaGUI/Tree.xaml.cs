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

namespace ilaGUI
{
    /// <summary>
    /// Logique d'interaction pour Tree.xaml
    /// </summary>
    public partial class Tree : UserControl
    {
        public Tree()
        {
            InitializeComponent();
            Title.Foreground = App.DarkFontColor;
            TreeList.Children.Add(new TreeElement(new ILANET.Program() { Name = "algo test" }));
            TreeList.Children.Add(new TreeElement(new ILANET.Module() { Name = "module test" }));
            TreeList.Children.Add(new TreeElement(new ILANET.Function() { Name = "function test" }));
        }
    }
}