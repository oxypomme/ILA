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
        }

        private void newFncBtn_Click(object sender, RoutedEventArgs e)
        {
            var f = new ILANET.Function() { Name = "nouvelle fonction" };
            App.CurrentILAcode.Methods.Add(f);
            App.CurrentExecutable = f;
            App.UpdateTree();
            App.UpdateEditor();
            App.UpdateLexic();
        }

        private void newModBtn_Click(object sender, RoutedEventArgs e)
        {
            var m = new ILANET.Module() { Name = "nouveau module" };
            App.CurrentILAcode.Methods.Add(m);
            App.CurrentExecutable = m;
            App.UpdateTree();
            App.UpdateEditor();
            App.UpdateLexic();
        }
    }
}