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
        public static readonly Color DarkThemeColor = Color.FromRgb(45, 42, 46);
        public static readonly Brush DarkBackground = new SolidColorBrush(DarkThemeColor);
        public static readonly Brush DarkFontColor = new SolidColorBrush(Colors.White);
        public static Control Dragged { get; set; }

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
        public static EditorView Editor { get; set; }
        public static List<string> Workspaces { get; set; }

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
                        else if (!isNameAvailable(func.Name, CurrentILAcode))
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
                } while (!(isNameAvailable(func.Name, CurrentILAcode) && isNameConventionnal(func.Name)));
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
                        else if (!isNameAvailable(mod.Name, CurrentILAcode))
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
                } while (!(isNameAvailable(mod.Name, CurrentILAcode) && isNameConventionnal(mod.Name)));
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
                            ILANET.Parser.Parser.ParseValue(item.minValue.Text, CurrentILAcode, CurrentILAcode, true),
                             ILANET.Parser.Parser.ParseValue(item.maxValue.Text, CurrentILAcode, CurrentILAcode, true)
                           ));
                    }
                }
            }
            else if (type.CreatedType is EnumType et)
            {
                var dialog = new createEnum(type, true);
                dialog.Owner = MainDialog;
                if (dialog.ShowDialog() == true)
                {
                    var comm = new Comment();
                    comm.MultiLine = dialog.comments.Text.Contains('\n');
                    comm.Message = dialog.comments.Text;
                    type.AboveComment = comm;
                    type.InlineComment = dialog.inlineComm.Text;
                    et.Name = dialog.typeName.Text;
                    et.Values.Clear();
                    foreach (enumValue item in dialog.valList.Children)
                        et.Values.Add(item.valueName.Text);
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
          ILANET.Parser.Parser.ParseValue(item.minValue.Text, CurrentILAcode, CurrentILAcode, true),
           ILANET.Parser.Parser.ParseValue(item.maxValue.Text, CurrentILAcode, CurrentILAcode, true)
         ));
                    }
                    return decl;
                }
            }
            else if (type == 2)
            {
                var tmp = new EnumType();
                tmp.Name = "nouvelle_enumeration";
                tmp.Values.Add("VALEUR1");
                var dialog = new createEnum(new TypeDeclaration() { CreatedType = tmp }, false);
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
                    tmp.Values.Clear();
                    foreach (enumValue item in dialog.valList.Children)
                        tmp.Values.Add(item.valueName.Text);
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
                    else if (!isNameAvailable(variable.Name, scope))
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
            } while (!(isNameAvailable(variable.Name, scope) && isNameConventionnal(variable.Name)));
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
                        else if (!isNameAvailable(dialog.modName.Text, CurrentILAcode))
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
                } while (!(isNameAvailable(fcopy.Name, CurrentILAcode) && isNameConventionnal(fcopy.Name)));
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
                        else if (!isNameAvailable(dialog.modName.Text, CurrentILAcode))
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
                } while (!(isNameAvailable(copy.Name, CurrentILAcode) && isNameConventionnal(copy.Name)));
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
                    else if (!isNameAvailable(newName, scope))
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
            } while (!(isNameAvailable(variable.CreatedVariable.Name, scope) && isNameConventionnal(variable.CreatedVariable.Name)));
        }

        public static void UpdateEditor()
        {
            if (CurrentExecutable != null)
            {
                if (CurrentExecutable is Program prog)
                {
                    Editor.exeIcon.Source = MakeDarkTheme(GetBitmapImage(new MemoryStream(ilaGUI.Properties.Resources.algo)));
                    Editor.exeType.Text = "algo";
                    Editor.exeName.Text = prog.Name;
                    Editor.leftParenthesis.Visibility = Visibility.Collapsed;
                    Editor.moduleParams.Children.Clear();
                    Editor.rightParenthesis.Visibility = Visibility.Collapsed;
                    Editor.dbPoint.Visibility = Visibility.Collapsed;
                    Editor.fctReturnType.Text = "";
                }
                else if (CurrentExecutable is Function fct)
                {
                    Editor.exeIcon.Source = MakeDarkTheme(GetBitmapImage(new MemoryStream(ilaGUI.Properties.Resources.function)));
                    Editor.exeType.Text = "fonction";
                    Editor.exeName.Text = fct.Name;
                    Editor.leftParenthesis.Visibility = Visibility.Visible;
                    Editor.moduleParams.Children.Clear();
                    for (int i = 0; i < fct.Parameters.Count; i++)
                    {
                        var item = fct.Parameters[i];
                        var sw = new StringWriter();
                        item.ImportedVariable.WriteILA(sw);
                        Editor.moduleParams.Children.Add(new TextBlock
                        {
                            Text = sw.ToString(),
                            VerticalAlignment = VerticalAlignment.Bottom,
                            Foreground = new SolidColorBrush(Colors.White)
                        });
                        sw.GetStringBuilder().Clear();
                        Editor.moduleParams.Children.Add(new TextBlock
                        {
                            Text = ":",
                            VerticalAlignment = VerticalAlignment.Bottom,
                            Foreground = new SolidColorBrush(Colors.OrangeRed)
                        });
                        item.ImportedVariable.Type.WriteILA(sw);
                        Editor.moduleParams.Children.Add(new TextBlock
                        {
                            Text = sw.ToString(),
                            VerticalAlignment = VerticalAlignment.Bottom,
                            Foreground = new SolidColorBrush(Colors.LightBlue)
                        });
                        if (i < fct.Parameters.Count - 1)
                            Editor.moduleParams.Children.Add(new TextBlock
                            {
                                Text = ", ",
                                VerticalAlignment = VerticalAlignment.Bottom,
                                Foreground = new SolidColorBrush(Colors.OrangeRed)
                            });
                    }
                    Editor.rightParenthesis.Visibility = Visibility.Visible;
                    Editor.dbPoint.Visibility = Visibility.Visible;
                    {
                        var sw = new StringWriter();
                        fct.ReturnType.WriteILA(sw);
                        Editor.fctReturnType.Text = sw.ToString();
                    }
                }
                else if (CurrentExecutable is Module mod)
                {
                    Editor.exeIcon.Source = MakeDarkTheme(GetBitmapImage(new MemoryStream(ilaGUI.Properties.Resources.module)));
                    Editor.exeType.Text = "module";
                    Editor.exeName.Text = mod.Name;
                    Editor.leftParenthesis.Visibility = Visibility.Visible;
                    Editor.moduleParams.Children.Clear();
                    for (int i = 0; i < mod.Parameters.Count; i++)
                    {
                        var item = mod.Parameters[i];
                        if (item.Mode == ILANET.Parameter.Flags.OUTPUT)
                            Editor.moduleParams.Children.Add(new TextBlock
                            {
                                Text = "s",
                                VerticalAlignment = VerticalAlignment.Bottom,
                                Foreground = new SolidColorBrush(Colors.White)
                            });
                        if (item.Mode == ILANET.Parameter.Flags.IO)
                            Editor.moduleParams.Children.Add(new TextBlock
                            {
                                Text = "es",
                                VerticalAlignment = VerticalAlignment.Bottom,
                                Foreground = new SolidColorBrush(Colors.White)
                            });
                        if (item.Mode != ILANET.Parameter.Flags.INPUT)
                            Editor.moduleParams.Children.Add(new TextBlock
                            {
                                Text = "::",
                                VerticalAlignment = VerticalAlignment.Bottom,
                                Foreground = new SolidColorBrush(Colors.OrangeRed)
                            });
                        var sw = new StringWriter();
                        item.ImportedVariable.WriteILA(sw);
                        Editor.moduleParams.Children.Add(new TextBlock
                        {
                            Text = sw.ToString(),
                            VerticalAlignment = VerticalAlignment.Bottom,
                            Foreground = new SolidColorBrush(Colors.White)
                        });
                        sw.GetStringBuilder().Clear();
                        Editor.moduleParams.Children.Add(new TextBlock
                        {
                            Text = ":",
                            VerticalAlignment = VerticalAlignment.Bottom,
                            Foreground = new SolidColorBrush(Colors.OrangeRed)
                        });
                        item.ImportedVariable.Type.WriteILA(sw);
                        Editor.moduleParams.Children.Add(new TextBlock
                        {
                            Text = sw.ToString(),
                            VerticalAlignment = VerticalAlignment.Bottom,
                            Foreground = new SolidColorBrush(Colors.LightBlue)
                        });
                        if (i < mod.Parameters.Count - 1)
                            Editor.moduleParams.Children.Add(new TextBlock
                            {
                                Text = ", ",
                                VerticalAlignment = VerticalAlignment.Bottom,
                                Foreground = new SolidColorBrush(Colors.OrangeRed)
                            });
                    }
                    Editor.rightParenthesis.Visibility = Visibility.Visible;
                    Editor.dbPoint.Visibility = Visibility.Collapsed;
                    Editor.fctReturnType.Text = "";
                }
                Editor.instructions.Children.Clear();

                /////////////////////////
                //dummy additions, for tests
                {
                    var assign = new Editor.Assign();
                    var grid1 = assign.leftGrid;
                    var grid2 = assign.rightGrid;
                    grid1.Children.Add(new TextBlock() { Text = "left", Foreground = DarkFontColor });
                    grid2.Children.Add(new TextBlock() { Text = "right", Foreground = DarkFontColor });
                    Editor.instructions.Children.Add(assign);
                }
                {
                    var instru = new Editor.Comment();
                    instru.comment.Text = "//commentaire";
                    Editor.instructions.Children.Add(instru);
                }
                {
                    var instru = new Editor.Return();
                    instru.comment.Text = "//commentaire";
                    instru.fctName.Text = "nom_fct";
                    instru.valueGrid.Children.Add(new TextBlock() { Text = "variable", Foreground = DarkFontColor });
                    Editor.instructions.Children.Add(instru);
                }
                {
                    var instru = new Editor.ModuleCall();
                    instru.moduleName.Text = "module";
                    instru.icon.Source = MakeDarkTheme(GetBitmapImage(new MemoryStream(ilaGUI.Properties.Resources.module)));
                    instru.parameters.Children.Add(GetValueControl(new ConstantChar() { Value = 'a' }));
                    Editor.instructions.Children.Add(instru);
                }
                {
                    var instru = new Editor.DoWhile();
                    instru.instructions.Children.Add(instru.EndInstruction);
                    instru.condGrid.Children.Add(new TextBlock() { Text = "faux", Foreground = DarkFontColor });
                    Editor.instructions.Children.Add(instru);
                }
                {
                    var instru = new Editor.Switch();
                    instru.defaultInstructions.Children.Add(instru.EndInstruction);
                    instru.varGrid.Children.Add(new TextBlock() { Text = "variable", Foreground = DarkFontColor });
                    Editor.instructions.Children.Add(instru);
                }
                {
                    var instru = new Editor.If();
                    instru.conditionGrid.Children.Add(new TextBlock() { Text = "faux", Foreground = DarkFontColor });
                    Editor.instructions.Children.Add(instru);
                    {
                        var instru2 = new Editor.ModuleCall();
                        instru2.moduleName.Text = "fonction";
                        instru2.icon.Source = MakeDarkTheme(GetBitmapImage(new MemoryStream(ilaGUI.Properties.Resources.function)));
                        instru2.parameters.Children.Add(GetValueControl(new ConstantString() { Value = "test" }));
                        instru.ifInstructions.Children.Add(instru2);
                    }
                    instru.ifInstructions.Children.Add(instru.EndInstruction);
                    instru.elseInstructions.Children.Add(instru.ElseEndInstruction);
                }
                {
                    var instru = new Editor.For();
                    instru.instructions.Children.Add(instru.EndInstruction);
                    instru.varGrid.Children.Add(new TextBlock() { Text = "variable", Foreground = DarkFontColor });
                    instru.infGrid.Children.Add(new TextBlock() { Text = "1", Foreground = new SolidColorBrush(Colors.Plum) });
                    instru.supGrid.Children.Add(new TextBlock() { Text = "10", Foreground = new SolidColorBrush(Colors.Plum) });
                    instru.stepGrid.Children.Add(new TextBlock() { Text = "1", Foreground = new SolidColorBrush(Colors.Plum) });
                    Editor.instructions.Children.Add(instru);
                }
                {
                    var loop = new Editor.While();
                    loop.conditionGrid.Children.Add(new TextBlock() { Text = "vrai", Foreground = DarkFontColor });
                    var assign = new Editor.Assign();
                    loop.instructions.Children.Add(assign);
                    loop.instructions.Children.Add(loop.EndInstruction);
                    var grid1 = assign.leftGrid;
                    var grid2 = assign.rightGrid;
                    grid1.Children.Add(new TextBlock() { Text = "variable", Foreground = DarkFontColor });
                    grid2.Children.Add(new TextBlock() { Text = "value", Foreground = DarkFontColor });
                    Editor.instructions.Children.Add(loop);
                }

                ////////////////////////
                Editor.instructions.Children.Add(Editor.EndInstruction);
            }
            else
            {
                Editor.exeIcon.Source = null;
                Editor.exeType.Text = "";
                Editor.exeName.Text = "";
                Editor.leftParenthesis.Visibility = Visibility.Collapsed;
                Editor.moduleParams.Children.Clear();
                Editor.rightParenthesis.Visibility = Visibility.Collapsed;
                Editor.dbPoint.Visibility = Visibility.Collapsed;
                Editor.fctReturnType.Text = "";
                Editor.instructions.Children.Clear();
            }
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