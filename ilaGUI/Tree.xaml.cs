using ILANET;
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

        private void newBoolBtn_Click(object sender, RoutedEventArgs e)
        {
            App.createVar(3, App.CurrentILAcode);
            App.UpdateTree();
            App.UpdateLexic();
        }

        private void newCharBtn_Click(object sender, RoutedEventArgs e)
        {
            App.createVar(2, App.CurrentILAcode);
            App.UpdateTree();
            App.UpdateLexic();
        }

        private void newCustomBtn_Click(object sender, RoutedEventArgs e)
        {
            App.createVar(5, App.CurrentILAcode);
            App.UpdateTree();
            App.UpdateLexic();
        }

        private void newFloatBtn_Click(object sender, RoutedEventArgs e)
        {
            App.createVar(1, App.CurrentILAcode);
            App.UpdateTree();
            App.UpdateLexic();
        }

        private void newFncBtn_Click(object sender, RoutedEventArgs e)
        {
            App.createModule(true);
            App.UpdateTree();
            App.UpdateLexic();
            App.UpdateEditor();
        }

        private void newIntBtn_Click(object sender, RoutedEventArgs e)
        {
            App.createVar(0, App.CurrentILAcode);
            App.UpdateTree();
            App.UpdateLexic();
        }

        private void newModBtn_Click(object sender, RoutedEventArgs e)
        {
            App.createModule(false);
            App.UpdateTree();
            App.UpdateLexic();
            App.UpdateEditor();
        }

        private void newStringBtn_Click(object sender, RoutedEventArgs e)
        {
            App.createVar(4, App.CurrentILAcode);
            App.UpdateTree();
            App.UpdateLexic();
        }

        private void newStructBtn_Click(object sender, RoutedEventArgs e)
        {
            var created = App.createType(0);
            if (created != null)
            {
                App.CurrentILAcode.Declarations.Add(created);
                App.UpdateTree();
                App.UpdateLexic();
            }
        }

        private void newTabBtn_Click(object sender, RoutedEventArgs e)
        {
            var created = App.createType(1);
            if (created != null)
            {
                App.CurrentILAcode.Declarations.Add(created);
                App.UpdateTree();
                App.UpdateLexic();
            }
        }
    }
}