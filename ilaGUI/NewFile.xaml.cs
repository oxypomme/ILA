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
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // this set some proprety in main window to be able to reopen the new_file window
        }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog
            {
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\"
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // somewhat useless vars
                string nameFile = algonameTB.Text;
                string filePath = dialog.SelectedPath;

                File.WriteAllText(Path.Combine(filePath, nameFile + ".ila"), "algo " + nameFile + "{\n\n}");
            }
            Close();
        }
    }
}