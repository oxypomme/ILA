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
            App.Tree = TreePannel;
            TreeGrid.Children.Add(TreePannel);

            Console = new Console();
            ConsoleGrid.Children.Add(Console);

            Background = App.DarkBackground;
            App.Tabs = algoList;
            App.UpdateTree();
            App.UpdateEditor();
            App.UpdateLexic();
            App.UpdateTabs();
        }

        private Console Console { get; set; }
        private Tree TreePannel { get; set; }

        private void algoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.CurrentILAcode = App.ILAcodes[algoList.SelectedIndex];
            App.CurrentExecutable = App.CurrentILAcode;
            App.CurrentWorkspace = App.Workspaces[algoList.SelectedIndex];
            App.UpdateTree();
            App.UpdateEditor();
            App.UpdateLexic();
        }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new NewFileDialog();
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }

        private void runBtn_Click(object sender, RoutedEventArgs e)
        {
            //Console.WriteInConsole($"ila {App.WorkspacePath + App.CurrentILAcode.Name}.ila");
        }
    }
}