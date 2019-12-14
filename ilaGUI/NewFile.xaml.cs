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
                App.CurrentILAcode.Name = algonameTB.Text;
                App.ILAcodes.Add(App.CurrentILAcode);
                App.CurrentExecutable = App.CurrentILAcode;

                App.UpdateTree();
                App.UpdateEditor();
                App.UpdateLexic();

                File.WriteAllText(Path.Combine(dialog.SelectedPath, algonameTB.Text + ".ila"), App.ILAcodes);
            }
            Close();
        }
    }
}