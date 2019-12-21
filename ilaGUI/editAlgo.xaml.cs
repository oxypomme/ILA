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
    /// Logique d'interaction pour editAlgo.xaml
    /// </summary>
    public partial class editAlgo : Window
    {
        public editAlgo()
        {
            InitializeComponent();
            algonameTB.Text = App.CurrentILAcode.Name;
            Background = App.DarkBackground;
            algonameTB.Focus();
            algonameTB.SelectAll();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!App.isNameConventionnal(algonameTB.Text))
            {
                System.Windows.MessageBox.Show(this, "Nom non conventionnel", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            App.CurrentILAcode.Name = algonameTB.Text;
            int selection = App.Tabs.SelectedIndex;
            App.UpdateTabs();
            App.Tabs.SelectedIndex = selection;

            DialogResult = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                DialogResult = false;
        }
    }
}