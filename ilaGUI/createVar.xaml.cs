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
using System.Windows.Shapes;
using System.IO;

namespace ilaGUI
{
    /// <summary>
    /// Logique d'interaction pour createVar.xaml
    /// </summary>
    public partial class createVar : Window
    {
        public createVar(VariableDeclaration v, bool edit = false)
        {
            InitializeComponent();
            if (edit)
                validateBtn.Content = "Modifier";
            varName.Text = v.CreatedVariable.Name;
            Background = App.DarkBackground;
            if (v.AboveComment != null)
                comments.Text = v.AboveComment.Message;
            inlineComm.Text = v.InlineComment;
            if (v.CreatedVariable.Constant)
            {
                varConst.IsChecked = true;
                string val = "";
                using (var sw = new StringWriter())
                {
                    v.CreatedVariable.ConstantValue.WriteILA(sw);
                    val = sw.ToString();
                }
                constValue.Text = val;
            }
            else
            {
                varConst.IsChecked = false;
                constValue.IsEnabled = false;
            }
            if (v.CreatedVariable.Type == GenericType.Int)
            {
                varType.SelectedIndex = varType.Items.Add("entier");
                varType.IsEnabled = false;
            }
            else if (v.CreatedVariable.Type == GenericType.Float)
            {
                varType.SelectedIndex = varType.Items.Add("reel");
                varType.IsEnabled = false;
            }
            else if (v.CreatedVariable.Type == GenericType.Char)
            {
                varType.SelectedIndex = varType.Items.Add("caractere");
                varType.IsEnabled = false;
            }
            else if (v.CreatedVariable.Type == GenericType.Bool)
            {
                varType.SelectedIndex = varType.Items.Add("booleen");
                varType.IsEnabled = false;
            }
            else if (v.CreatedVariable.Type == GenericType.String)
            {
                varType.SelectedIndex = varType.Items.Add("chaine");
                varType.IsEnabled = false;
            }
            else
            {
                foreach (var item in App.CurrentILAcode.Declarations)
                {
                    if (item is TypeDeclaration td)
                    {
                        if (td.CreatedType == v.CreatedVariable.Type)
                            varType.SelectedIndex = varType.Items.Add(new ToStringOverrider(td.CreatedType, () => td.CreatedType.Name));
                        else
                            varType.Items.Add(new ToStringOverrider(td.CreatedType, () => td.CreatedType.Name));
                    }
                }
                varConst.IsEnabled = false;
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void validateBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void varConst_Click(object sender, RoutedEventArgs e)
        {
            constValue.IsEnabled = varConst.IsChecked.Value;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                DialogResult = false;
        }
    }
}