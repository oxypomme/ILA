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

namespace ilaGUI
{
    /// <summary>
    /// Logique d'interaction pour EditMember.xaml
    /// </summary>
    public partial class EditMember : Window
    {
        public EditMember(string name, ILANET.VarType type)
        {
            InitializeComponent();
            Background = App.DarkBackground;
            memberName.Text = name;
            memberName.Focus();
            memberName.SelectAll();
            memberType.Items.Add(new ToStringOverrider(ILANET.GenericType.Int, () => "entier"));
            memberType.Items.Add(new ToStringOverrider(ILANET.GenericType.Float, () => "reel"));
            memberType.Items.Add(new ToStringOverrider(ILANET.GenericType.Char, () => "caractere"));
            memberType.Items.Add(new ToStringOverrider(ILANET.GenericType.Bool, () => "booleen"));
            memberType.Items.Add(new ToStringOverrider(ILANET.GenericType.String, () => "chaine"));
            foreach (var item in App.CurrentILAcode.Declarations)
                if (item is ILANET.TypeDeclaration td)
                    memberType.Items.Add(new ToStringOverrider(td.CreatedType, () => td.CreatedType.Name));
            foreach (var item in memberType.Items)
            {
                if (((item as ToStringOverrider).Content as ILANET.VarType) == type)
                {
                    memberType.SelectedItem = item;
                    break;
                }
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                DialogResult = false;
        }
    }
}