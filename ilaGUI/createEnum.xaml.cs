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
using ILANET;

namespace ilaGUI
{
    /// <summary>
    /// Logique d'interaction pour createEnum.xaml
    /// </summary>
    public partial class createEnum : Window
    {
        private EnumType enumtype;

        public createEnum(TypeDeclaration enumdecl)
        {
            enumtype = enumdecl.CreatedType as EnumType;
            InitializeComponent();
            Background = App.DarkBackground;

            typeName.Text = enumtype.Name;
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
            valList.Children.Add(new enumValue("", this));
            (valList.Children[0] as enumValue).removeBtn.Visibility = Visibility.Visible;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void validateBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                DialogResult = false;
        }
    }
}