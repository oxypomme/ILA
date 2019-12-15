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
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            var newAlgo = new ILANET.Program();
            newAlgo.Name = algonameTB.Text;
            App.ILAcodes.Add(newAlgo);
            App.CurrentILAcode = newAlgo;
            App.CurrentExecutable = App.CurrentILAcode;

            App.UpdateTabs();
            App.UpdateTree();
            App.UpdateEditor();
            App.UpdateLexic();

            DialogResult = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // this set some proprety in main window to be able to reopen the new_file window
        }
    }
}