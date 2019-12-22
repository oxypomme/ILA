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
            varName.Focus();
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
                    v.CreatedVariable.ConstantValue?.WriteILA(sw);
                    val = sw.ToString();
                }
                constValue.Text = val;
            }
            else
            {
                varConst.IsChecked = false;
                constValue.IsEnabled = false;
            }
            {
                var title = edit ? "Editer " : "Créer ";
                if (v.CreatedVariable.Type == GenericType.Int)
                    title += "un entier";
                else if (v.CreatedVariable.Type == GenericType.Float)
                    title += "un réel";
                else if (v.CreatedVariable.Type == GenericType.Char)
                    title += "un caractère";
                else if (v.CreatedVariable.Type == GenericType.Bool)
                    title += "un booléen";
                else if (v.CreatedVariable.Type == GenericType.String)
                    title += "une chaine";
                else
                    title += "une variable";
                Title = title;
            }
            if (v.CreatedVariable.Type is Native)
            {
                varType.Visibility = Visibility.Collapsed;
                typeLabel.Visibility = Visibility.Collapsed;
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
            varName.SelectAll();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void validateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (varConst.IsChecked == true && constValue.Text.Trim().Length == 0)
            {
                MessageBox.Show(this, "Aucune valeur constante entrée", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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