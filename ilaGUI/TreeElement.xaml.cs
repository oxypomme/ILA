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
                deleteButton.Visibility = Visibility.Collapsed;
                Icon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.algo));
                Title.Text = "algo";
            }
            else if (linkedTo is Function fct)
            {
                Icon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.function));
                Title.Text = fct.Name;
            }
            else if (linkedTo is ILANET.Module mod)
            {
                Icon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.module));
                Title.Text = mod.Name;
            }
            else if (linkedTo is VariableDeclaration vd)
            {
                editButton.Visibility = Visibility.Collapsed;
                if (vd.CreatedVariable.Type is IGenericType gt)
                {
                    if (gt == GenericType.String)
                        Icon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources._string));
                    else if (gt == GenericType.Int)
                        Icon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.int_var));
                    else if (gt == GenericType.Char)
                        Icon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources._char));
                    else if (gt == GenericType.Bool)
                        Icon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources._bool));
                    else if (gt == GenericType.Float)
                        Icon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.float_var));
                }
                else
                    Icon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.custom_var));
                Title.Text = vd.CreatedVariable.Name;
            }
            else if (linkedTo is TypeDeclaration td)
            {
                editButton.Visibility = Visibility.Collapsed;
                if (td.CreatedType is StructType)
                    Icon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources._struct));
                else if (td.CreatedType is TableType)
                    Icon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.table));
                else if (td.CreatedType is EnumType)
                    Icon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources._enum));
            }
            Title.Foreground = App.DarkFontColor;
        }

        public IBaseObject Link { get; private set; }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(App.MainDialog, "Voulez-vous vraiment supprimer \"" + Title.Text + "\" ?", "supprimer", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                return;
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
                if (Link is VariableDeclaration vd)
                {
                    App.editVar(vd.CreatedVariable, App.CurrentILAcode);
                    App.UpdateTree();
                    App.UpdateEditor();
                    App.UpdateLexic();
                }
            }
        }
    }
}