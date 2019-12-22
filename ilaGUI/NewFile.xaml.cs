using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace ilaGUI
{
    /// <summary>
    /// Logique d'interaction pour NewFile.xaml
    /// </summary>
    public partial class NewFileDialog : Window
    {
        public NewFileDialog()
        {
            InitializeComponent();
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
                MessageBox.Show(this, "Nom non conventionnel", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var newAlgo = new ILANET.Program();
            newAlgo.Name = algonameTB.Text;
            App.ILAcodes.Add(newAlgo);
            App.Workspaces.Add("");

            App.UpdateTabs();
            App.Tabs.SelectedIndex = App.ILAcodes.Count - 1;
            Console.WriteLine("algo '" + algonameTB.Text + "' créé");

            DialogResult = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // this set some proprety in main window to be able to reopen the new_file window
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
                DialogResult = false;
        }
    }
}