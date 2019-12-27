using ILANET;
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
    /// Logique d'interaction pour createStruct.xaml
    /// </summary>
    public partial class createStruct : Window
    {
        private readonly string originalName;
        private readonly StructType structtype;

        public createStruct(TypeDeclaration type)
        {
            originalName = type.CreatedType.Name;
            structtype = type.CreatedType as StructType;
            InitializeComponent();
            (addMember.Content as Image).Source = App.MakeDarkTheme((addMember.Content as Image).Source as BitmapSource);
            Background = App.DarkBackground;
            typeName.Text = type.CreatedType.Name;
            comments.Text = type.AboveComment?.Message;
            inlineComm.Text = type.InlineComment;
            typeName.Focus();
            typeName.SelectAll();
            foreach (var item in structtype.Members)
                membersList.Children.Add(new StructMember(structtype, item.Key));
        }

        private void addMember_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new EditMember("nouveau_membre", GenericType.Int, structtype, false);
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                structtype.Members.Add(dialog.memberName.Text, (dialog.memberType.SelectedItem as ToStringOverrider).Content as VarType);
                membersList.Children.Clear();
                foreach (var item in structtype.Members)
                    membersList.Children.Add(new StructMember(structtype, item.Key));
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void validateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (originalName != typeName.Text && !App.isNameAvailable(typeName.Text, App.CurrentILAcode))
            {
                MessageBox.Show("Nom déjà utilisé", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!App.isNameConventionnal(typeName.Text))
            {
                MessageBox.Show("Nom non conventionnel", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
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