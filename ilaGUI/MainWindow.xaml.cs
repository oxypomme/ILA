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

            (newAlgoMenu.Icon as Image).Source = App.MakeDarkTheme((newAlgoMenu.Icon as Image).Source as BitmapSource);
            (openAlgoMenu.Icon as Image).Source = App.MakeDarkTheme((openAlgoMenu.Icon as Image).Source as BitmapSource);
            (addFieldMenu.Icon as Image).Source = App.MakeDarkTheme((addFieldMenu.Icon as Image).Source as BitmapSource);
            (closeAlgoMenu.Icon as Image).Source = App.MakeDarkTheme((closeAlgoMenu.Icon as Image).Source as BitmapSource);
            (saveMenu.Icon as Image).Source = App.MakeDarkTheme((saveMenu.Icon as Image).Source as BitmapSource);
            (undoMenu.Icon as Image).Source = App.MakeDarkTheme((undoMenu.Icon as Image).Source as BitmapSource);
            (redoMenu.Icon as Image).Source = App.MakeDarkTheme((redoMenu.Icon as Image).Source as BitmapSource);
            (cutMenu.Icon as Image).Source = App.MakeDarkTheme((cutMenu.Icon as Image).Source as BitmapSource);
            (copyMenu.Icon as Image).Source = App.MakeDarkTheme((copyMenu.Icon as Image).Source as BitmapSource);
            (pasteMenu.Icon as Image).Source = App.MakeDarkTheme((pasteMenu.Icon as Image).Source as BitmapSource);
            (buildMenu.Icon as Image).Source = App.MakeDarkTheme((buildMenu.Icon as Image).Source as BitmapSource);
            (startMenu.Icon as Image).Source = App.MakeDarkTheme((startMenu.Icon as Image).Source as BitmapSource);
            (stopMenu.Icon as Image).Source = App.MakeDarkTheme((stopMenu.Icon as Image).Source as BitmapSource);
            (settingsMenu.Icon as Image).Source = App.MakeDarkTheme((settingsMenu.Icon as Image).Source as BitmapSource);
            (helpMenu.Icon as Image).Source = App.MakeDarkTheme((helpMenu.Icon as Image).Source as BitmapSource);

            (newBtn.Content as Image).Source = App.MakeDarkTheme((newBtn.Content as Image).Source as BitmapSource);
            (openBtn.Content as Image).Source = App.MakeDarkTheme((openBtn.Content as Image).Source as BitmapSource);
            (saveBtn.Content as Image).Source = App.MakeDarkTheme((saveBtn.Content as Image).Source as BitmapSource);
            (closeAlgo.Content as Image).Source = App.MakeDarkTheme((closeAlgo.Content as Image).Source as BitmapSource);
            (runBtn.Content as Image).Source = App.MakeDarkTheme((runBtn.Content as Image).Source as BitmapSource);
            (stopBtn.Content as Image).Source = App.MakeDarkTheme((stopBtn.Content as Image).Source as BitmapSource);
            (buildBtn.Content as Image).Source = App.MakeDarkTheme((buildBtn.Content as Image).Source as BitmapSource);
            (settingsBtn.Content as Image).Source = App.MakeDarkTheme((settingsBtn.Content as Image).Source as BitmapSource);
            (wikiBtn.Content as Image).Source = App.MakeDarkTheme((wikiBtn.Content as Image).Source as BitmapSource);
            App.MainDialog = this;

            TreePannel = new Tree();
            App.Tree = TreePannel;
            TreeHolder.Content = TreePannel;

            Editor = new EditorView();
            App.Editor = Editor;
            EditorHolder.Content = Editor;

            Console = new Console();
            ConsoleHolder.Content = Console;

            Background = App.DarkBackground;
            App.Tabs = algoList;
            App.UpdateTree();
            App.UpdateEditor();
            App.UpdateLexic();
            App.UpdateTabs();

            InitShortcuts();
        }

        private Console Console { get; set; }
        private EditorView Editor { get; set; }
        private Tree TreePannel { get; set; }

        private void algoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (algoList.SelectedIndex != -1)
            {
                App.CurrentILAcode = App.ILAcodes[algoList.SelectedIndex];
                App.CurrentExecutable = App.CurrentILAcode;
                App.CurrentWorkspace = App.Workspaces[algoList.SelectedIndex];
                closeAlgo.IsEnabled = true;
                saveMenu.IsEnabled = true;
                saveAsMenu.IsEnabled = true;
                closeAlgoMenu.IsEnabled = true;
                addFieldMenu.IsEnabled = true;
                buildMenu.IsEnabled = true;
                startMenu.IsEnabled = true;
                App.UpdateTree();
                App.UpdateEditor();
                App.UpdateLexic();
            }
            else
            {
                closeAlgo.IsEnabled = false;
                saveMenu.IsEnabled = false;
                saveAsMenu.IsEnabled = false;
                closeAlgoMenu.IsEnabled = false;
                addFieldMenu.IsEnabled = false;
                buildMenu.IsEnabled = false;
                startMenu.IsEnabled = false;
                App.CurrentILAcode = null;
                App.CurrentExecutable = null;
                App.CurrentWorkspace = null;
                App.UpdateTree();
                App.UpdateEditor();
                App.UpdateLexic();
            }
        }

        private void buildBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void closeAlgo_Click(object sender, RoutedEventArgs e)
        {
            App.ILAcodes.Remove(App.CurrentILAcode);
            App.Workspaces.Remove(App.CurrentWorkspace);
            App.UpdateTabs();
            App.Tabs.SelectedIndex = -1;
        }

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

            /* CLOSE ALGO SHORTCUT (Ctrl+W) */
            keyShortcut = new RoutedCommand();
            keyShortcut.InputGestures.Add(new KeyGesture(Key.W, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(keyShortcut, closeAlgo_Click));

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

            /**/

            /* CHANGE ALGO */
            keyShortcut = new RoutedCommand();
            keyShortcut.InputGestures.Add(new KeyGesture(Key.Tab, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(keyShortcut, (sender, e) =>
            App.Tabs.SelectedIndex = (App.Tabs.SelectedIndex + 1) % App.Tabs.Items.Count
            ));
            /* CHANGE ALGO² */
            keyShortcut = new RoutedCommand();
            keyShortcut.InputGestures.Add(new KeyGesture(Key.Tab, ModifierKeys.Control | ModifierKeys.Shift));
            CommandBindings.Add(new CommandBinding(keyShortcut, (sender, e) =>
            App.Tabs.SelectedIndex = (App.Tabs.SelectedIndex - 1 + App.Tabs.Items.Count) % App.Tabs.Items.Count
            ));
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

        private void quitBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void runBtn_Click(object sender, RoutedEventArgs e)
        {
            //Console.WriteInConsole($"ila {App.WorkspacePath + App.CurrentILAcode.Name}.ila");
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void wikiBtn_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}