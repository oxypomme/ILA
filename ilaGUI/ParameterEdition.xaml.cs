using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ilaGUI
{
    /// <summary>
    /// Logique d'interaction pour ParameterEdition.xaml
    /// </summary>
    public partial class ParameterEdition : Window
    {
        private ILANET.Module scope;
        private Parameter selectedParam;

        public ParameterEdition(Parameter param, ILANET.Module scope, bool edit = false)
        {
            InitializeComponent();
            varName.Focus();
            if (edit)
                validateBtn.Content = "Modifier";
            this.scope = scope;
            selectedParam = param;
            var linked = param.Link as ILANET.Parameter;
            Background = App.DarkBackground;
            varName.Text = linked.ImportedVariable.Name;
            varType.Items.Add(new ToStringOverrider(ILANET.GenericType.Int, () => "entier"));
            varType.Items.Add(new ToStringOverrider(ILANET.GenericType.Float, () => "reel"));
            varType.Items.Add(new ToStringOverrider(ILANET.GenericType.Char, () => "caractere"));
            varType.Items.Add(new ToStringOverrider(ILANET.GenericType.Bool, () => "booleen"));
            varType.Items.Add(new ToStringOverrider(ILANET.GenericType.String, () => "chaine"));
            foreach (var item in App.CurrentILAcode.Declarations)
                if (item is ILANET.TypeDeclaration td)
                    varType.Items.Add(new ToStringOverrider(td.CreatedType, () => td.CreatedType.Name));
            foreach (ToStringOverrider item in varType.Items)
                if (item.Content == linked.ImportedVariable.Type)
                    varType.SelectedItem = item;
            if (scope is ILANET.Function)
            {
                varOutput.Visibility = Visibility.Collapsed;
                varInput.Visibility = Visibility.Collapsed;
            }
            varInput.IsChecked = (linked.Mode & ILANET.Parameter.Flags.INPUT) == ILANET.Parameter.Flags.INPUT;
            varOutput.IsChecked = (linked.Mode & ILANET.Parameter.Flags.OUTPUT) == ILANET.Parameter.Flags.OUTPUT;
            varName.SelectAll();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void validateBtn_Click(object sender, RoutedEventArgs e)
        {
            var linked = selectedParam.Link as ILANET.Parameter;
            var oldName = linked.ImportedVariable.Name;
            linked.ImportedVariable.Name = "";
            if (!(App.isNameConventionnal(varName.Text) && App.isNameAvailable(varName.Text, scope)))
            {
                MessageBox.Show(this, "Impossible d'utiliser ce nom", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                linked.ImportedVariable.Name = oldName;
                return;
            }
            if (varType.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Impossible d'utiliser ce type", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (varOutput.IsChecked == false && varInput.IsChecked == false)
            {
                MessageBox.Show(this, "Un paramètre doit être en entrée et/ou sortie", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            linked.ImportedVariable.Name = varName.Text;
            linked.ImportedVariable.Constant = false;
            linked.ImportedVariable.Type = ((ToStringOverrider)varType.SelectedItem).Content as ILANET.VarType;
            linked.Mode = (varOutput.IsChecked == true ? ILANET.Parameter.Flags.OUTPUT : 0) |
                          (varInput.IsChecked == true ? ILANET.Parameter.Flags.INPUT : 0);
            selectedParam.paramName.Text = linked.ImportedVariable.Name;
            selectedParam.dbPoints.Visibility = Visibility.Visible;
            if (linked.Mode == ILANET.Parameter.Flags.OUTPUT)
                selectedParam.prefixName.Text = "s";
            else if (linked.Mode == ILANET.Parameter.Flags.IO)
                selectedParam.prefixName.Text = "es";
            else if (linked.Mode == ILANET.Parameter.Flags.INPUT)
            {
                selectedParam.prefixName.Text = "";
                selectedParam.dbPoints.Visibility = Visibility.Collapsed;
            }
            if (linked.ImportedVariable.Type == ILANET.GenericType.Int)
            {
                selectedParam.iconType.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.int_var));
                selectedParam.paramType.Text = "entier";
            }
            else if (linked.ImportedVariable.Type == ILANET.GenericType.Float)
            {
                selectedParam.iconType.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.float_var));
                selectedParam.paramType.Text = "reel";
            }
            else if (linked.ImportedVariable.Type == ILANET.GenericType.Char)
            {
                selectedParam.iconType.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources._char));
                selectedParam.paramType.Text = "caractere";
            }
            else if (linked.ImportedVariable.Type == ILANET.GenericType.Bool)
            {
                selectedParam.iconType.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources._bool));
                selectedParam.paramType.Text = "booleen";
            }
            else if (linked.ImportedVariable.Type == ILANET.GenericType.String)
            {
                selectedParam.iconType.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources._string));
                selectedParam.paramType.Text = "chaine";
            }
            else if (linked.ImportedVariable.Type is ILANET.StructType st)
            {
                selectedParam.iconType.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources._struct));
                selectedParam.paramType.Text = st.Name;
            }
            else if (linked.ImportedVariable.Type is ILANET.EnumType et)
            {
                selectedParam.iconType.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources._enum));
                selectedParam.paramType.Text = et.Name;
            }
            else if (linked.ImportedVariable.Type is ILANET.TableType tt)
            {
                selectedParam.iconType.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.table));
                selectedParam.paramType.Text = tt.Name;
            }
            else
            {
                selectedParam.iconType.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.custom_var));
                selectedParam.paramType.Text = linked.ImportedVariable.Type.Name;
            }
            DialogResult = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                DialogResult = false;
        }
    }
}