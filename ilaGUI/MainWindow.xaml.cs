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
        private List<(bool isShowed, Window window)> subWindows = new List<(bool, Window)>(); // Désolé mais je sais pas utiliser les Tuples<>

        public MainWindow()
        {
            InitializeComponent();
            TreePannel = new Tree();
            TreeGrid.Children.Add(TreePannel);
            Background = App.DarkBackground;
        }

        private Tree TreePannel { get; set; }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            /*
             * created : is the new_file window is already set
             * index : the index of the new_file window in the list
             * showed : is the new_file window is showed
            */
            (bool created, int index, bool showed) = (false, -1, false);

            for (int i = 0; i < subWindows.Count; i++)
            {
                // we check if the new_file window exist
                if (subWindows[i].window is NewFile)
                {
                    created = true;
                    index = i;
                    showed = subWindows[i].isShowed;
                    break; // only one sub-window of each type can exist, so we found it
                }
            }

            if (!showed)
            {
                if (!created)
                {
                    index = subWindows.Count - 1;
                    subWindows.Add((true, new NewFile()));
                }
                subWindows[index].window.Show();
            }
            else
            {
                subWindows[index].window.Focus();
            }
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