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

            InitShortcuts();
        }

        private Console Console { get; set; }
        private Tree TreePannel { get; set; }

        private void InitShortcuts()
        {
            // inspired by https://stackoverflow.com/a/33450624
            RoutedCommand keyShortcut = new RoutedCommand();

            /* NEW_ALGO SHORTCUT (Ctrl+N) */
            keyShortcut.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(keyShortcut, newBtn_Click));

            /* OPEN SHORTCUT (Ctrl+O) */
            keyShortcut = new RoutedCommand();
            keyShortcut.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(keyShortcut, newBtn_Click));

            /* SAVE SHORTCUT (Ctrl+S) */
            keyShortcut = new RoutedCommand();
            keyShortcut.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(keyShortcut, saveBtn_Click));

            /**/

            /* RUN SHORTCUT (F5) */
            keyShortcut = new RoutedCommand();
            keyShortcut.InputGestures.Add(new KeyGesture(Key.F5));
            CommandBindings.Add(new CommandBinding(keyShortcut, newBtn_Click));

            /* BUILD SHORTCUT (Ctrl+B) */
            keyShortcut = new RoutedCommand();
            keyShortcut.InputGestures.Add(new KeyGesture(Key.B, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(keyShortcut, buildBtn_Click));

            /**/

            /* HELP SHORTCUT (F1) */
            keyShortcut = new RoutedCommand();
            keyShortcut.InputGestures.Add(new KeyGesture(Key.F1));
            CommandBindings.Add(new CommandBinding(keyShortcut, wikiBtn_Click));
        }

        private void algoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (algoList.SelectedIndex != -1)
            {
                App.CurrentILAcode = App.ILAcodes[algoList.SelectedIndex];
                App.CurrentExecutable = App.CurrentILAcode;
                App.CurrentWorkspace = App.Workspaces[algoList.SelectedIndex];
                App.UpdateTree();
                App.UpdateEditor();
                App.UpdateLexic();
            }
        }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new NewFileDialog();
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }

        private void openBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void runBtn_Click(object sender, RoutedEventArgs e)
        {
            //Console.WriteInConsole($"ila {App.WorkspacePath + App.CurrentILAcode.Name}.ila");
        }

        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void buildBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void wikiBtn_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}