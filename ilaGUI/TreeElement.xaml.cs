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
                deleteIcon.Visibility = Visibility.Collapsed;
                Icon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.algo));
                Title.Content = "algo";
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
            else if (linkedTo is VariableDeclaration vd)
            {
                if (vd.CreatedVariable.Type is GenericType gt)
                {
                }
                else
                    Title.Content = vd.CreatedVariable.Name;
            }
            Title.Foreground = App.DarkFontColor;
        }

        public IBaseObject Link { get; private set; }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Link is ILANET.Module)
            {
                App.CurrentILAcode.Methods.Remove(Link as ILANET.Module);
                App.UpdateTree();
                if (App.CurrentExecutable == Link)
                {
                    App.CurrentExecutable = App.CurrentILAcode;

                    App.UpdateEditor();
                    App.UpdateLexic();
                    App.UpdateTreeColor();
                }
            }
            else
            {
                App.CurrentILAcode.Declarations.Remove(Link as IDeclaration);
                App.UpdateTree();
                App.UpdateLexic();
                App.ParseEntireProgram();
            }
        }

        private void globalButton_Click(object sender, RoutedEventArgs e)
        {
            if (Link is IExecutable exe)
            {
                App.CurrentExecutable = exe;
                App.UpdateEditor();
                App.UpdateLexic();
                App.UpdateTreeColor();
            }
            else
            {
                //edit declaration
            }
        }
    }
}