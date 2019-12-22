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
using System.Windows.Input;
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
        public static readonly Brush DarkFontColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));

        public App()
        {
            ILAcodes = new List<Program>();
            CurrentILAcode = null;
            Workspaces = new List<string>();
            Console.ActiveConsoles = new List<Console>();
            Console.StandardOutput = new StreamWriter(new Console.ConsoleStream(), Encoding.UTF8) { AutoFlush = true };
#if RELEASE
            System.Console.SetOut(Console.StandardOutput);
            System.Console.SetError(Console.StandardOutput);
#endif
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

        public static BitmapImage ConvertBitmapToWPF(System.Drawing.Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            src.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        public static void createModule(bool isFct)
        {
            Module created;
            bool create = false;
            if (isFct)
            {
                var func = new Function();
                created = func;
                func.Name = "nouvelle_fonction";
                func.ReturnType = GenericType.Bool;
                do
                {
                    var dialog = new createModule(func);
                    dialog.Owner = MainDialog;
                    if (dialog.ShowDialog() == true)
                    {
                        func.Name = dialog.modName.Text;
                        if (!isNameConventionnal(func.Name))
                            MessageBox.Show("nom non conventionnel !", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        else if (!isNameAvailable(func.Name))
                            MessageBox.Show("nom déjà utilisé !", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            create = true;
                            func.ReturnType = ((ToStringOverrider)dialog.returnType.SelectedItem).Content as VarType;
                            var comm = new Comment();
                            comm.MultiLine = dialog.comments.Text.Contains('\n');
                            comm.Message = dialog.comments.Text;
                            func.AboveComment = comm;
                            func.InlineComment = dialog.inlineComm.Text;
                            func.Parameters.Clear();
                            foreach (var item in dialog.paramList.Children)
                                if (item is Parameter p)
                                    func.Parameters.Add(p.Link as ILANET.Parameter);
                        }
                    }
                    else
                        break;
                } while (!(isNameAvailable(func.Name) && isNameConventionnal(func.Name)));
            }
            else
            {
                var mod = new Module();
                created = mod;
                mod.Name = "nouveau_module";
                do
                {
                    var dialog = new createModule(mod);
                    dialog.Owner = MainDialog;
                    if (dialog.ShowDialog() == true)
                    {
                        mod.Name = dialog.modName.Text;
                        if (!isNameConventionnal(mod.Name))
                            MessageBox.Show(dialog, "nom non conventionnel !", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        else if (!isNameAvailable(mod.Name))
                            MessageBox.Show(dialog, "nom déjà utilisé !", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            create = true;
                            var comm = new Comment();
                            comm.MultiLine = dialog.comments.Text.Contains('\n');
                            comm.Message = dialog.comments.Text;
                            mod.AboveComment = comm;
                            mod.InlineComment = dialog.inlineComm.Text;
                            mod.Parameters.Clear();
                            foreach (var item in dialog.paramList.Children)
                                if (item is Parameter p)
                                    mod.Parameters.Add(p.Link as ILANET.Parameter);
                        }
                    }
                    else
                        break;
                } while (!(isNameAvailable(mod.Name) && isNameConventionnal(mod.Name)));
            }
            if (create)
            {
                CurrentILAcode.Methods.Add(created);
                CurrentExecutable = created;
                UpdateTree();
                UpdateEditor();
                UpdateLexic();
            }
        }

        public static Parameter createParameter(Module scope)
        {
            var param = new ILANET.Parameter
            {
                ImportedVariable = new Variable
                {
                    Name = "nouveau_parametre",
                    Type = GenericType.Int
                },
                Mode = ILANET.Parameter.Flags.INPUT
            };
            var uiParam = new Parameter(param, scope);
            return new ParameterEdition(uiParam, scope).ShowDialog() == true ? uiParam : null;
        }

        public static void editType(TypeDeclaration type)
        {
            var copy = new TypeDeclaration();
            copy.AboveComment = new Comment
            {
                Message = type.AboveComment?.Message,
                MultiLine = type.AboveComment != null ? type.AboveComment.MultiLine : false
            };
            copy.InlineComment = type.InlineComment;

            if (type.CreatedType is StructType st)
            {
                var structCopy = new StructType
                {
                    Name = st.Name,
                    Members = new SortedDictionary<string, VarType>()
                };
                foreach (var item in st.Members)
                    structCopy.Members.Add(item.Key, item.Value);
                copy.CreatedType = structCopy;
                var dialog = new createStruct(copy);
                dialog.Owner = MainDialog;
                if (dialog.ShowDialog() == true)
                {
                    var comm = new Comment();
                    comm.MultiLine = dialog.comments.Text.Contains('\n');
                    comm.Message = dialog.comments.Text;
                    type.AboveComment = comm;
                    type.InlineComment = dialog.inlineComm.Text;
                    (type.CreatedType as StructType).Members = structCopy.Members;
                    type.CreatedType.Name = dialog.typeName.Text;
                }
            }
            else if (type.CreatedType is TableType tt)
            {
                var dialog = new createTable(type, true);
                dialog.Owner = MainDialog;
                if (dialog.ShowDialog() == true)
                {
                    var comm = new Comment();
                    comm.MultiLine = dialog.comments.Text.Contains('\n');
                    comm.Message = dialog.comments.Text;
                    type.AboveComment = comm;
                    type.InlineComment = dialog.inlineComm.Text;
                    tt.Name = dialog.typeName.Text;
                    tt.DimensionsSize.Clear();
                    foreach (dimension item in dialog.dimList.Children)
                    {
                        tt.DimensionsSize.Add(new ILANET.Range(
                            ILANET.Parser.Parser.ParseValue(item.minValue.Text, CurrentILAcode, CurrentExecutable, true),
                             ILANET.Parser.Parser.ParseValue(item.maxValue.Text, CurrentILAcode, CurrentExecutable, true)
                           ));
                    }
                }
            }
        }

        public static TypeDeclaration createType(int type) //0 = struct, 1 = table, 2 = enum
        {
            if (type == 0)
            {
                var tmp = new StructType();
                tmp.Name = "nouvelle_structure";
                var dialog = new createStruct(new TypeDeclaration() { CreatedType = tmp });
                dialog.Owner = MainDialog;
                if (dialog.ShowDialog() == true)
                {
                    var decl = new TypeDeclaration();
                    var comm = new Comment();
                    comm.MultiLine = dialog.comments.Text.Contains('\n');
                    comm.Message = dialog.comments.Text;
                    decl.AboveComment = comm;
                    decl.InlineComment = dialog.inlineComm.Text;
                    decl.CreatedType = tmp;
                    tmp.Name = dialog.typeName.Text;
                    return decl;
                }
            }
            else if (type == 1)
            {
                var tmp = new TableType();
                tmp.Name = "nouveau_tableau";
                tmp.DimensionsSize.Add(new ILANET.Range(new ConstantInt() { Value = 1 }, new ConstantInt() { Value = 10 }));
                var dialog = new createTable(new TypeDeclaration() { CreatedType = tmp }, false);
                dialog.Owner = MainDialog;
                if (dialog.ShowDialog() == true)
                {
                    var decl = new TypeDeclaration();
                    var comm = new Comment();
                    comm.MultiLine = dialog.comments.Text.Contains('\n');
                    comm.Message = dialog.comments.Text;
                    decl.AboveComment = comm;
                    decl.InlineComment = dialog.inlineComm.Text;
                    decl.CreatedType = tmp;
                    tmp.Name = dialog.typeName.Text;
                    tmp.DimensionsSize.Clear();
                    foreach (dimension item in dialog.dimList.Children)
                    {
                        tmp.DimensionsSize.Add(new ILANET.Range(
          ILANET.Parser.Parser.ParseValue(item.minValue.Text, CurrentILAcode, CurrentExecutable, true),
           ILANET.Parser.Parser.ParseValue(item.maxValue.Text, CurrentILAcode, CurrentExecutable, true)
         ));
                    }
                    return decl;
                }
            }
            return null;
        }

        public static void createVar(int type, IExecutable scope) //0 = int, 1 = float, 2 = char, 3 = bool, 4 = string, 5 = custom
        {
            var decl = new VariableDeclaration();
            var variable = new Variable();
            decl.CreatedVariable = variable;
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
                var dialog = new createVar(decl);
                dialog.Owner = MainDialog;
                if (dialog.ShowDialog() == true)
                {
                    variable.Name = dialog.varName.Text;
                    variable.Constant = dialog.varConst.IsChecked.Value;
                    if (!isNameConventionnal(variable.Name))
                        MessageBox.Show(dialog, "nom non conventionnel !", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (!isNameAvailable(variable.Name))
                        MessageBox.Show(dialog, "nom déjà utilisé !", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        if (!(variable.Type is Native) && dialog.varType.SelectedIndex == -1)
                        {
                            MessageBox.Show(MainDialog, "Aucun type choisi", "erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                            variable.Name = "";
                            continue;
                        }
                        if (!(variable.Type is Native))
                            variable.Type = (VarType)((ToStringOverrider)dialog.varType.SelectedItem).Content;
                        var comm = new Comment
                        {
                            Message = dialog.comments.Text,
                            MultiLine = dialog.comments.Text.Contains('\n')
                        };
                        decl.InlineComment = dialog.inlineComm.Text;
                        decl.AboveComment = comm;
                        if (scope is Module m)
                            m.Declarations.Add(decl);
                        else if (scope is Program p)
                            p.Declarations.Add(decl);
                        return;
                    }
                }
                else
                    break;
            } while (!(isNameAvailable(variable.Name) && isNameConventionnal(variable.Name)));
        }

        public static void editAlgo(Program algo) => new editAlgo { Owner = MainDialog }.ShowDialog();

        public static void editModule(Module m)
        {
            Module copy;
            Function func;
            string choosenName = m.Name;
            if (m is Function f)
            {
                copy = new Function();
                ((Function)copy).ReturnType = f.ReturnType;
            }
            else
                copy = new Module();
            copy.Name = m.Name.Clone() as string;
            copy.AboveComment = m.AboveComment == null ? null :
                new Comment
                {
                    MultiLine = m.AboveComment.MultiLine,
                    Message = m.AboveComment.Message.Clone() as string
                };
            copy.InlineComment = m.InlineComment.Clone() as string;
            foreach (var item in m.Parameters)
            {
                var param = new ILANET.Parameter();
                var variable = new Variable();
                param.ImportedVariable = variable;
                param.Mode = item.Mode;
                variable.Name = item.ImportedVariable.Name;
                variable.Type = item.ImportedVariable.Type;
                copy.Parameters.Add(param);
            }
            m.Name = "";
            if (copy is Function fcopy)
            {
                func = m as Function;
                do
                {
                    var dialog = new createModule(fcopy, true);
                    dialog.Owner = MainDialog;
                    if (dialog.ShowDialog() == true)
                    {
                        fcopy.Name = dialog.modName.Text;
                        if (!isNameConventionnal(dialog.modName.Text))
                            MessageBox.Show(dialog, "nom non conventionnel !", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        else if (!isNameAvailable(dialog.modName.Text))
                            MessageBox.Show(dialog, "nom déjà utilisé !", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            choosenName = dialog.modName.Text;
                            func.ReturnType = ((ToStringOverrider)dialog.returnType.SelectedItem).Content as VarType;
                            var comm = new Comment();
                            comm.MultiLine = dialog.comments.Text.Contains('\n');
                            comm.Message = dialog.comments.Text;
                            func.AboveComment = comm;
                            func.InlineComment = dialog.inlineComm.Text;
                            func.Parameters.Clear();
                            foreach (var item in dialog.paramList.Children)
                                if (item is Parameter p)
                                    func.Parameters.Add(p.Link as ILANET.Parameter);
                        }
                    }
                    else
                        break;
                } while (!(isNameAvailable(fcopy.Name) && isNameConventionnal(fcopy.Name)));
            }
            else
            {
                do
                {
                    var dialog = new createModule(copy, true);
                    dialog.Owner = MainDialog;
                    if (dialog.ShowDialog() == true)
                    {
                        copy.Name = dialog.modName.Text;
                        if (!isNameConventionnal(dialog.modName.Text))
                            MessageBox.Show(dialog, "nom non conventionnel !", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        else if (!isNameAvailable(dialog.modName.Text))
                            MessageBox.Show(dialog, "nom déjà utilisé !", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            choosenName = dialog.modName.Text;
                            var comm = new Comment();
                            comm.MultiLine = dialog.comments.Text.Contains('\n');
                            comm.Message = dialog.comments.Text;
                            m.AboveComment = comm;
                            m.InlineComment = dialog.inlineComm.Text;
                            m.Parameters.Clear();
                            foreach (var item in dialog.paramList.Children)
                                if (item is Parameter p)
                                    m.Parameters.Add(p.Link as ILANET.Parameter);
                        }
                    }
                    else
                        break;
                } while (!(isNameAvailable(copy.Name) && isNameConventionnal(copy.Name)));
            }
            m.Name = choosenName;
            UpdateTree();
            UpdateEditor();
        }

        public static void editParameter(Parameter param, Module scope) => new ParameterEdition(param, scope, true).ShowDialog();

        public static void editVar(VariableDeclaration variable, IExecutable scope)
        {
            string oldName = variable.CreatedVariable.Name, newName;
            bool oldCstState = variable.CreatedVariable.Constant;
            do
            {
                var dialog = new createVar(variable, true);
                dialog.Owner = MainDialog;
                if (dialog.ShowDialog() == true)
                {
                    newName = dialog.varName.Text;
                    variable.CreatedVariable.Name = "";
                    variable.CreatedVariable.Constant = dialog.varConst.IsChecked.Value;
                    if (!isNameConventionnal(newName))
                        MessageBox.Show(dialog, "nom non conventionnel !", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (!isNameAvailable(newName))
                        MessageBox.Show(dialog, "nom déjà utilisé !", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        if (variable.CreatedVariable.Constant)
                        {
                            IValue cstValule;
                            try
                            {
                                cstValule = ILANET.Parser.Parser.ParseValue(dialog.constValue.Text, CurrentILAcode, scope, true);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(MainDialog, e.Message, "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                variable.CreatedVariable.Name = "";
                                continue;
                            }
                            variable.CreatedVariable.ConstantValue = cstValule;
                        }
                        if (!(variable.CreatedVariable.Type is Native))
                        {
                            int i = 0;
                            foreach (var item in CurrentILAcode.Declarations)
                            {
                                if (item is TypeDeclaration td)
                                {
                                    if (i == dialog.varType.SelectedIndex)
                                        variable.CreatedVariable.Type = td.CreatedType;
                                    i++;
                                }
                            }
                        }
                        var comm = new Comment
                        {
                            Message = dialog.comments.Text,
                            MultiLine = dialog.comments.Text.Contains('\n')
                        };
                        variable.AboveComment = comm;
                        variable.InlineComment = dialog.inlineComm.Text;
                        variable.CreatedVariable.Name = newName;
                        return;
                    }
                }
                else
                {
                    variable.CreatedVariable.Name = oldName;
                    variable.CreatedVariable.Constant = oldCstState;
                    break;
                }
            } while (!(isNameAvailable(variable.CreatedVariable.Name) && isNameConventionnal(variable.CreatedVariable.Name)));
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
            foreach (var item in ilacode.Declarations)
            {
                if (item is VariableDeclaration vd)
                    if (vd.CreatedVariable.Name == name)
                        return false;
                if (item is TypeDeclaration td)
                {
                    if (td.CreatedType.Name == name)
                        return false;
                    if (td.CreatedType is EnumType et)
                        foreach (var item2 in et.Values)
                            if (item2 == name)
                                return false;
                }
            }
            foreach (var item in ilacode.Methods)
            {
                if (item.Name == name)
                    return false;
            }
            if (source is Module m)
            {
                foreach (var item in m.Parameters)
                    if (item.ImportedVariable.Name == name)
                        return false;
                foreach (var item in m.Declarations)
                    if (item.CreatedVariable.Name == name)
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
                if (!(char.IsLetterOrDigit(item) || item == '_') || "éàèêëîïôöûüçÈÉÊËÂÎÇÀÏÔÖÛÜ".Contains(item))
                    return false;
            return true;
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
                Tabs.Items.Add(new TabItem() { Header = item.Name, Foreground = DarkFontColor });
        }

        public static void UpdateTree()
        {
            Tree.TreeList.Children.Clear();
            if (CurrentILAcode != null)
            {
                Tree.operations.IsEnabled = true;
                Tree.TreeList.Children.Add(new TreeElement(CurrentILAcode));
                foreach (var item in CurrentILAcode.Methods.Where(m => !(m is Native)))
                    Tree.TreeList.Children.Add(new TreeElement(item));
                foreach (var item in CurrentILAcode.Declarations.Where(d => !(d is Native)))
                    Tree.TreeList.Children.Add(new TreeElement(item));
                UpdateTreeColor();
            }
            else
                Tree.operations.IsEnabled = false;
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