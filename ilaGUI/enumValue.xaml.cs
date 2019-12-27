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

namespace ilaGUI
{
    /// <summary>
    /// Logique d'interaction pour enumValue.xaml
    /// </summary>
    public partial class enumValue : UserControl
    {
        private createEnum main;

        public enumValue(string name, createEnum main)
        {
            InitializeComponent();
            (removeBtn.Content as Image).Source = App.MakeDarkTheme((removeBtn.Content as Image).Source as BitmapSource);
            valueName.Text = name;
            this.main = main;
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            main.removeChild(this);
        }
    }
}