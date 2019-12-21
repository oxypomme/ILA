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
        public createStruct(TypeDeclaration type)
        {
            var structtype = type.CreatedType as StructType;
            InitializeComponent();
            Background = App.DarkBackground;
            typeName.Text = type.CreatedType.Name;
            comments.Text = type.AboveComment?.Message;
            inlineComm.Text = type.InlineComment;
            typeName.Focus();
            typeName.SelectAll();
            {
                foreach (var item in structtype.Members)
                    membersList.Children.Add(new StructMember(structtype, item.Key));
                var addbutton = new Button();
                addbutton.Content = new Image
                {
                    Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.add_field)),
                    Stretch = Stretch.None,
                    Margin = new Thickness(.1, 0, 0, .1)
                };
                addbutton.Click += (sender, e) =>
                {
                    var dialog = new EditMember("nouveau_membre", GenericType.Int);
                    dialog.Owner = this;
                    if (dialog.ShowDialog() == true)
                    {
                        if (!App.isNameConventionnal(dialog.memberName.Text))
                        {
                            MessageBox.Show("Nom non conventionnel", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (structtype.Members.ContainsKey(dialog.memberName.Text))
                        {
                            MessageBox.Show("Nom déjà utilisé", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        structtype.Members.Add(dialog.memberName.Text, (dialog.memberType.SelectedItem as ToStringOverrider).Content as VarType);
                        while (membersList.Children[0] is StructMember)
                            membersList.Children.RemoveAt(0);
                        foreach (var item in structtype.Members)
                            membersList.Children.Insert(membersList.Children.Count - 1, new StructMember(structtype, item.Key));
                    }
                };
                membersList.Children.Add(addbutton);
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