using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            App.MainDialog = this;

            TreePannel = new Tree();
            TreeGrid.Children.Add(TreePannel);

            Console = new Console();
            ConsoleGrid.Children.Add(Console);

            Background = App.DarkBackground;
        }

        private Tree TreePannel { get; set; }
        private Console Console { get; set; }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new NewFileDialog();
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }

        private void algoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.CurrentILAcode = App.ILAcodes[algoList.SelectedIndex];
            App.CurrentExecutable = App.CurrentILAcode;
            App.UpdateTree();
            App.UpdateEditor();
            App.UpdateLexic();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
    }
}