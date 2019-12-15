using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ILANET;

namespace ilaGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly Brush DarkBackground = new SolidColorBrush(Color.FromRgb(45, 42, 46));
        public static readonly Brush DarkFontColor = new SolidColorBrush(Color.FromRgb(230, 230, 230));

        public App()
        {
            ILAcodes = new List<Program>();
            CurrentILAcode = new Program();
            Workspaces = new List<string>();
            Console.ActiveConsoles = new List<Console>();
            Console.StandardOutput = new StreamWriter(new Console.ConsoleStream(), Encoding.UTF8);
            System.Console.SetOut(Console.StandardOutput);
            System.Console.SetError(Console.StandardOutput);
            CurrentILAcode.Name = "main";
            ILAcodes.Add(CurrentILAcode);
            Workspaces.Add("");
            CurrentExecutable = CurrentILAcode;
        }

        public static IExecutable CurrentExecutable { get; set; }
        public static Program CurrentILAcode { get; set; }
        public static string CurrentWorkspace { get; set; }
        public static List<Program> ILAcodes { get; set; }
        public static Window MainDialog { get; set; }
        public static TabControl Tabs { get; set; }
        public static Tree Tree { get; set; }
        public static List<string> Workspaces { get; set; }

        public static void createFunction()
        {
        }

        public static void createModule()
        {
        }

        public static VarType createType(int type) //0 = struct, 1 = table, 2 = enum
        {
            return null;
        }

        public static void createVar(int type, IExecutable scope) //0 = int, 1 = float, 2 = char, 3 = bool, 4 = string, 5 = custom
        {
            var variable = new Variable();
            variable.Name = "nouvelle_variable";
            switch (type)
            {
                case 0:
                    variable.Type = GenericType.Int;
                    break;

                case 1:
                    variable.Type = GenericType.Float;
                    break;

                case 2:
                    variable.Type = GenericType.Char;
                    break;

                case 3:
                    variable.Type = GenericType.Bool;
                    break;

                case 4:
                    variable.Type = GenericType.String;
                    break;

                case 5:
                    {
                        foreach (var item in CurrentILAcode.Declarations)
                        {
                            if (item is TypeDeclaration td)
                            {
                                variable.Type = td.CreatedType;
                                break;
                            }
                        }
                        break;
                    }
            }
            do
            {
                var dialog = new createVar(variable);
                dialog.Owner = MainDialog;
                if (dialog.ShowDialog() == true)
                {
                    variable.Name = dialog.varName.Text;
                    variable.Constant = dialog.varConst.IsChecked.Value;
                    if (!isNameConventionnal(variable.Name))
                        MessageBox.Show("nom non conventionnel !", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (!isNameAvailable(variable.Name))
                        MessageBox.Show("nom déjà utilisé !", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        if (variable.Constant)
                        {
                            IValue cstValule;
                            try
                            {
                                cstValule = ILANET.Parser.Parser.ParseValue(dialog.constValue.Text, CurrentILAcode, scope, true);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(MainDialog, e.Message, "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                variable.Name = "";
                                continue;
                            }
                            variable.ConstantValue = cstValule;
                        }
                        if (type == 5)
                        {
                            int i = 0;
                            foreach (var item in CurrentILAcode.Declarations)
                            {
                                if (item is TypeDeclaration td)
                                {
                                    if (i == dialog.varType.SelectedIndex)
                                        variable.Type = td.CreatedType;
                                    i++;
                                }
                            }
                        }
                        if (scope is Module m)
                            m.Declarations.Add(new VariableDeclaration() { CreatedVariable = variable });
                        else if (scope is Program p)
                            p.Declarations.Add(new VariableDeclaration() { CreatedVariable = variable });
                        return;
                    }
                }
                else
                    break;
            } while (!(isNameAvailable(variable.Name) && isNameConventionnal(variable.Name)));
        }

        public static BitmapImage GetBitmapImage(Stream stream)
        {
            //https://stackoverflow.com/a/9564425
            var image = new BitmapImage();
            stream.Position = 0;
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = null;
            image.StreamSource = stream;
            image.EndInit();
            image.Freeze();
            return image;
        }

        public static bool isNameAvailable(string name, IExecutable source, Program ilacode)
        {
            foreach (var item in source.Declarations)
            {
                if (item is VariableDeclaration vd)
                    if (vd.CreatedVariable.Name == name)
                        return false;
                if (item is TypeDeclaration td)
                    if (td.CreatedType.Name == name)
                        return false;
            }
            foreach (var item in ilacode.Declarations)
            {
                if (item is VariableDeclaration vd)
                    if (vd.CreatedVariable.Name == name)
                        return false;
                if (item is TypeDeclaration td)
                    if (td.CreatedType.Name == name)
                        return false;
            }
            return true;
        }

        public static bool isNameAvailable(string name) => isNameAvailable(name, CurrentExecutable, CurrentILAcode);

        public static bool isNameAvailable(string name, IExecutable source) => isNameAvailable(name, source, CurrentILAcode);

        public static bool isNameConventionnal(string name)
        {
            if (name.Length == 0)
                return false;
            if (!char.IsLetter(name.First()))
                return false;
            foreach (var item in name)
                if (!(char.IsLetterOrDigit(item) || item == '_'))
                    return false;
            return true;
        }

        public static void ParseEntireProgram()
        {
        }

        public static void UpdateEditor()
        {
        }

        public static void UpdateLexic()
        {
        }

        public static void UpdateTabs()
        {
            Tabs.Items.Clear();
            foreach (var item in ILAcodes)
                Tabs.Items.Add(new TabItem() { Header = item.Name });
        }

        public static void UpdateTree()
        {
            Tree.TreeList.Children.Clear();
            Tree.TreeList.Children.Add(new TreeElement(CurrentILAcode));
            foreach (var item in CurrentILAcode.Methods.Where(m => !(m is Native)))
                Tree.TreeList.Children.Add(new TreeElement(item));
            foreach (var item in CurrentILAcode.Declarations.Where(d => !(d is Native)))
                Tree.TreeList.Children.Add(new TreeElement(item));
            UpdateTreeColor();
        }

        public static void UpdateTreeColor()
        {
            foreach (TreeElement item in Tree.TreeList.Children)
                if (item.Link == CurrentExecutable)
                    item.Background = new SolidColorBrush(Color.FromArgb(128, 60, 100, 160));
                else
                    item.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                ILAcodes.Clear();
                foreach (var item in e.Args)
                {
                    using (var sr = new StreamReader(item))
                        ILAcodes.Add(ILANET.Parser.Parser.Parse(sr.ReadToEnd()));
                }
                CurrentILAcode = ILAcodes.First();
                CurrentExecutable = CurrentILAcode;
            }
        }
    }
}