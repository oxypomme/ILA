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
using ILANET;
using System.IO;
using System.Linq;

namespace ilaGUI
{
    /// <summary>
    /// Logique d'interaction pour StructMember.xaml
    /// </summary>
    public partial class StructMember : UserControl
    {
        public string Member;
        public StructType Type;

        public StructMember(StructType type, string member)
        {
            Type = type;
            Member = member;
            InitializeComponent();
            memberName.Text = member;
            if (type.Members[member] == GenericType.Int)
            {
                memberIcon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.int_var));
                memberType.Text = "entier";
            }
            else if (type.Members[member] == GenericType.Float)
            {
                memberIcon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.float_var));
                memberType.Text = "reel";
            }
            else if (type.Members[member] == GenericType.Char)
            {
                memberIcon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources._char));
                memberType.Text = "caractere";
            }
            else if (type.Members[member] == GenericType.Bool)
            {
                memberIcon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources._bool));
                memberType.Text = "booleen";
            }
            else if (type.Members[member] == GenericType.String)
            {
                memberIcon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources._string));
                memberType.Text = "chaine";
            }
            else if (type.Members[member] is StructType st)
            {
                memberIcon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources._struct));
                memberType.Text = st.Name;
            }
            else if (type.Members[member] is TableType tt)
            {
                memberIcon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources._struct));
                memberType.Text = tt.Name;
            }
            else if (type.Members[member] is EnumType et)
            {
                memberIcon.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources._struct));
                memberType.Text = et.Name;
            }
        }

        private void editMember_Click(object sender, RoutedEventArgs e)
        {
            StackPanel parent = Parent as StackPanel;
            var dialog = new EditMember(Member, Type.Members[Member], Type, true);
            if (dialog.ShowDialog() == true)
            {
                Type.Members.Remove(Member);
                Type.Members.Add(dialog.memberName.Text, (dialog.memberType.SelectedItem as ToStringOverrider).Content as VarType);
                while (parent.Children[0] is StructMember)
                    parent.Children.RemoveAt(0);
                foreach (var item in Type.Members)
                    parent.Children.Insert(parent.Children.Count - 1, new StructMember(Type, item.Key));
                Member = dialog.memberName.Text;
            }
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            StackPanel parent = Parent as StackPanel;
            Type.Members.Remove(Member);
            parent.Children.Remove(this);
        }
    }
}