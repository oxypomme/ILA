using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ILANET;

namespace ilaGUI
{
    /// <summary>
    /// Logique d'interaction pour createEnum.xaml
    /// </summary>
    public partial class createEnum : Window
    {
        private bool edit;
        private EnumType enumtype;
        private string originalName;
        private string[] originalValues;

        public createEnum(TypeDeclaration enumdecl, bool edit)
        {
            this.edit = edit;
            enumtype = enumdecl.CreatedType as EnumType;
            InitializeComponent();
            Background = App.DarkBackground;
            originalName = enumtype.Name;
            typeName.Text = enumtype.Name;
            comments.Text = enumdecl.AboveComment?.Message;
            inlineComm.Text = enumdecl.InlineComment;
            typeName.Focus();
            typeName.SelectAll();
            originalValues = enumtype.Values.ToArray();
            for (int i = 0; i < enumtype.Values.Count; i++)
                valList.Children.Add(new enumValue(enumtype.Values[i], this));
            if (valList.Children.Count == 1)
                (valList.Children[0] as enumValue).removeBtn.Visibility = Visibility.Collapsed;
        }

        internal void removeChild(enumValue value)
        {
            valList.Children.Remove(value);
            if (valList.Children.Count == 1)
                (valList.Children[0] as enumValue).removeBtn.Visibility = Visibility.Collapsed;
        }

        private void addVal_Click(object sender, RoutedEventArgs e)
        {
            var evalue = new enumValue("", this);
            valList.Children.Add(evalue);
            (valList.Children[0] as enumValue).removeBtn.Visibility = Visibility.Visible;
            //https://stackoverflow.com/a/56289573
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                evalue.valueName.Focus();
            }), System.Windows.Threading.DispatcherPriority.Render);
            evalue.valueName.SelectAll();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void validateBtn_Click(object sender, RoutedEventArgs e)
        {
            var valuesUsed = new List<string>();
            foreach (enumValue item in valList.Children)
            {
                if ((!originalValues.Contains(item.valueName.Text) || !edit) && !App.isNameAvailable(item.valueName.Text, App.CurrentILAcode))
                {
                    MessageBox.Show("Nom d'une valeur déjà utilisé", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!App.isNameConventionnal(item.valueName.Text))
                {
                    MessageBox.Show("Valeur non conentionnel", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (valuesUsed.Contains(item.valueName.Text))
                {
                    MessageBox.Show("Plusieurs occurences d'une même valeur", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                valuesUsed.Add(item.valueName.Text);
            }
            if ((originalName != typeName.Text || !edit) && !App.isNameAvailable(typeName.Text, App.CurrentILAcode))
            {
                MessageBox.Show("Nom déjà utilisé", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!App.isNameConventionnal(typeName.Text))
            {
                MessageBox.Show("Nom non conentionnel", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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