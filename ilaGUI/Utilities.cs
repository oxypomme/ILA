﻿using ILANET;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ilaGUI
{
    public partial class App
    {
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

        public static System.Drawing.Bitmap ConvertWPFToBitmap(BitmapSource src)
        {
            //https://stackoverflow.com/a/6484754
            using var outStream = new MemoryStream();
            var enc = new PngBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(src));
            enc.Save(outStream);
            var bitmap = new System.Drawing.Bitmap(outStream);

            return new System.Drawing.Bitmap(bitmap);
        }

        public static void CopyInstruction(IEnumerable<IDropableInstruction> toCopy)
        {
            using var sw = new StringWriter();
            foreach (var item in toCopy)
            {
                var instru = (item as Linked).Link as Instruction;
                instru.WriteILA(sw);
            }
            Clipboard.SetText(sw.ToString());
        }

        /// <summary>
        /// 0 = assign; 1 = comment; 2 = while/dowhile; 3 = for; 4 = if; 5 = module call; 6 =
        /// return; 7 = switch;
        /// </summary>
        public static IDropableInstruction createInstruction(int type)
        {
            switch (type)
            {
                case 0:
                    {
                        var result = new Editor.Assign();
                        if (EditInstruction(result))
                            return result;
                        else
                            return null;
                    }

                case 1:
                    {
                        var result = new Editor.Comment();
                        if (EditInstruction(result))
                            return result;
                        else
                            return null;
                    }

                case 2:
                    {
                        var result = new Editor.While();
                        if (EditInstruction(result))
                            return result;
                        else
                            return null;
                    }

                case 3:
                    {
                        var result = new Editor.For();
                        if (EditInstruction(result))
                            return result;
                        else
                            return null;
                    }

                case 4:
                    {
                        var result = new Editor.If();
                        if (EditInstruction(result))
                            return result;
                        else
                            return null;
                    }

                case 5:
                    {
                        var result = new Editor.ModuleCall();
                        if (EditInstruction(result))
                            return result;
                        else
                            return null;
                    }

                case 6:
                    {
                        var result = new Editor.Return();
                        if (EditInstruction(result))
                            return result;
                        else
                            return null;
                    }

                case 7:
                    {
                        var result = new Editor.Switch();
                        if (EditInstruction(result))
                            return result;
                        else
                            return null;
                    }
                default:
                    return null;
            }
        }

        public static void CutInstruction(IEnumerable<IDropableInstruction> toCut)
        {
            CopyInstruction(toCut);
            foreach (var item in toCut)
                item.Remove();
        }

        public static void DarkmodeUrBtns(UIElementCollection headerItems)
        {
            for (int i = 0; i < headerItems.Count; i++)
            {
                Button menuItem;
                try
                {
                    menuItem = (Button)headerItems[i];
                }
                catch (InvalidCastException) { i++; menuItem = (Button)headerItems[i]; }
                if (menuItem.Content != null)
                    (menuItem.Content as Image).Source = MakeDarkTheme((menuItem.Content as Image).Source as BitmapSource);
            }
        }

        public static void DarkmodeUrMenus(ItemCollection headerItems)
        {
            for (int i = 0; i < headerItems.Count; i++)
            {
                MenuItem menuItem;
                try
                {
                    menuItem = (MenuItem)headerItems[i];
                }
                catch (InvalidCastException) { i++; menuItem = (MenuItem)headerItems[i]; }
                if (menuItem.Icon != null)
                    (menuItem.Icon as Image).Source = MakeDarkTheme((menuItem.Icon as Image).Source as BitmapSource);

                if (menuItem.Items.Count > 0)
                    DarkmodeUrMenus(menuItem.Items);
            }
        }

        public static bool EditInstruction(IDropableInstruction instruction)
        {
            if (instruction is Editor.Assign assign)
            {
            }
            else if (instruction is Editor.Comment comment)
            {
            }
            return true;
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

        public static System.Drawing.Color GetDarkThemeColor(System.Drawing.Color c)
        {
            var wpfColor = Color.FromArgb(c.A, c.R, c.G, c.B);
            wpfColor = GetDarkThemeColor(wpfColor);
            return ToClassic(GetDarkThemeColor(ToWPF(c)));
        }

        public static Color GetDarkThemeColor(Color c)
        {
            var hsl = GetHSL(c);
            double luminosity = 1 - hsl.Item3;
            double haloLuminosity = 1 - GetHSL(Color.FromArgb(255, 240, 240, 240)).Item3;
            double themeBackgroundLuminosity = GetHSL(DarkThemeColor).Item3;

            if (luminosity < haloLuminosity)
                return GetRGB(c.A, hsl.Item1, hsl.Item2, themeBackgroundLuminosity * luminosity / haloLuminosity);
            else
                return GetRGB(c.A, hsl.Item1, hsl.Item2, (1.0 - themeBackgroundLuminosity) * (luminosity - 1.0) / (1.0 - haloLuminosity) + 1.0);
        }

        public static (double, double, double) GetHSL(Color c)
        {
            double r = c.R / 255.0;
            double g = c.G / 255.0;
            double b = c.B / 255.0;
            double max = Math.Max(Math.Max(r, g), b);
            double min = Math.Min(Math.Min(r, g), b);
            double lum = (max + min) / 2;
            double satur, hue;
            if (min == max)
                satur = 0;
            else if (lum < .5)
                satur = (max - min) / (max + min);
            else
                satur = (max - min) / (2.0 - max - min);
            if (r == max)
                hue = (g - b) / (max - min);
            else if (g == max)
                hue = 2.0 + (b - r) / (max - min);
            else
                hue = 4.0 + (r - g) / (max - min);
            hue = (hue * 60 + 360) % 360;
            return (hue, satur, lum);
        }

        public static Control GetInstructionControl(Instruction instru)
        {
            if (instru is Assign)
            {
                var res = new Editor.Assign();
                res.InternalInstruction = instru as Assign;
                res.UpdateVisuals();
                return res;
            }
            else if (instru is Comment)
            {
                var res = new Editor.Comment();
                res.InternalComment = instru as Comment;
                res.UpdateVisuals();
                return res;
            }
            else if (instru is DoWhile)
            {
                var loop = instru as DoWhile;
                var res = new Editor.DoWhile();
                res.InternalInstruction = loop;
                foreach (var item in loop.Instructions)
                    res.instructions.Children.Add(GetInstructionControl(item));
                res.instructions.Children.Add(res.EndInstruction);
                res.UpdateVisuals();
                return res;
            }
            else if (instru is For)
            {
                var loop = instru as For;
                var res = new Editor.For();
                res.InternalInstruction = loop;
                foreach (var item in loop.Instructions)
                    res.instructions.Children.Add(GetInstructionControl(item));
                res.instructions.Children.Add(res.EndInstruction);
                res.UpdateVisuals();
                return res;
            }
            else if (instru is If)
            {
                var block = instru as If;
                var res = new Editor.If();
                var font = res.comment.FontFamily;
                res.InternalInstruction = block;
                foreach (var item in block.IfInstructions)
                    res.ifInstructions.Children.Add(GetInstructionControl(item));
                res.ifInstructions.Children.Add(res.EndInstruction);
                foreach (var item in block.ElseInstructions)
                    res.elseInstructions.Children.Add(GetInstructionControl(item));
                res.elseInstructions.Children.Add(res.ElseEndInstruction);
                foreach (var elif in block.Elif)
                {
                    var elifStruct = new Editor.If.Elif();
                    elifStruct.EndInstruction = new Editor.DummyInstruction();
                    var firstLine = new StackPanel { Orientation = Orientation.Horizontal, IsHitTestVisible = false };
                    firstLine.Children.Add(new TextBlock
                    {
                        Text = "sinon si ",
                        Foreground = App.KeywordColorBrush,
                        FontFamily = font,
                        Margin = new System.Windows.Thickness(21, 0, 0, 0)
                    });
                    elifStruct.condition = new ContentControl();
                    firstLine.Children.Add(elifStruct.condition);
                    firstLine.Children.Add(new TextBlock
                    {
                        Text = " alors",
                        Foreground = App.KeywordColorBrush,
                        FontFamily = font
                    });
                    elifStruct.comment = new TextBlock
                    {
                        Foreground = commentsColorBrush,
                        FontFamily = font,
                        FontStyle = System.Windows.FontStyles.Italic,
                        Margin = new System.Windows.Thickness(5, 0, 0, 0)
                    };
                    firstLine.Children.Add(elifStruct.comment);
                    var mainFlow = new StackPanel();
                    mainFlow.Children.Add(firstLine);
                    var instruGrid = new Grid();
                    mainFlow.Children.Add(instruGrid);
                    instruGrid.Children.Add(new System.Windows.Shapes.Rectangle
                    {
                        Width = 1,
                        Fill = new SolidColorBrush(Colors.Gray),
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                        Margin = new System.Windows.Thickness(12, 0, 0, 0)
                    });
                    elifStruct.instructions = new StackPanel { Margin = new System.Windows.Thickness(25, 0, 0, 0) };
                    elifStruct.instructions.Tag = res;
                    foreach (var item in elif.Item2)
                        elifStruct.instructions.Children.Add(GetInstructionControl(item));
                    elifStruct.instructions.Children.Add(elifStruct.EndInstruction);
                    instruGrid.Children.Add(elifStruct.instructions);
                    res.elifs.Children.Add(mainFlow);
                    res.elifList.Add(elifStruct);
                }
                res.UpdateVisuals();
                return res;
            }
            else if (instru is ModuleCall)
            {
                var res = new Editor.ModuleCall();
                res.InternalInstruction = instru as ModuleCall;
                res.UpdateVisuals();
                return res;
            }
            else if (instru is Return)
            {
                var res = new Editor.Return();
                res.InternalInstruction = instru as Return;
                res.UpdateVisuals();
                return res;
            }
            else if (instru is ILANET.Switch)
            {
                var block = instru as ILANET.Switch;
                var res = new Editor.Switch();
                var font = res.comment.FontFamily;
                res.InternalInstruction = block;
                foreach (var item in block.Default)
                    res.defaultInstructions.Children.Add(GetInstructionControl(item));
                res.defaultInstructions.Children.Add(res.EndInstruction);
                foreach (var cas in block.Cases)
                {
                    var caseStruct = new Editor.Switch.Case();
                    caseStruct.EndInstruction = new Editor.DummyInstruction();
                    var firstLine = new DockPanel { Margin = new System.Windows.Thickness(25, 0, 0, 0) };
                    caseStruct.conditions = new StackPanel
                    {
                        IsHitTestVisible = false,
                        Orientation = Orientation.Horizontal
                    };
                    firstLine.Children.Add(caseStruct.conditions);
                    firstLine.Children.Add(new TextBlock
                    {
                        Text = " : ",
                        Foreground = App.SymbolColorBrush,
                        FontFamily = font
                    });
                    caseStruct.instructions = new StackPanel();
                    firstLine.Children.Add(caseStruct.instructions);
                    var mainGrid = new Grid();
                    mainGrid.Children.Add(firstLine);
                    mainGrid.Children.Add(new System.Windows.Shapes.Rectangle
                    {
                        Width = 1,
                        Fill = new SolidColorBrush(Colors.Gray),
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                        Margin = new System.Windows.Thickness(30, 16, 0, 0)
                    });
                    caseStruct.instructions.Tag = res;
                    foreach (var item in cas.Item2)
                        caseStruct.instructions.Children.Add(GetInstructionControl(item));
                    caseStruct.instructions.Children.Add(caseStruct.EndInstruction);
                    res.casesList.Children.Add(mainGrid);
                    res.Cases.Add(caseStruct);
                }
                res.UpdateVisuals();
                return res;
            }
            else if (instru is While)
            {
                var loop = instru as While;
                var res = new Editor.While();
                res.InternalInstruction = loop;
                foreach (var item in loop.Instructions)
                    res.instructions.Children.Add(GetInstructionControl(item));
                res.instructions.Children.Add(res.EndInstruction);
                res.UpdateVisuals();
                return res;
            }
            else
                return null;
        }

        public static Color GetRGB(byte a, (double, double, double) c) => GetRGB(a, c.Item1, c.Item2, c.Item3);

        public static Color GetRGB((double, double, double) c) => GetRGB(255, c);

        public static Color GetRGB(byte alpha, double hue, double satur, double lum)
        {
            double v;
            hue /= 360;
            double r, g, b;
            r = lum;   // default to gray
            g = lum;
            b = lum;
            v = (lum <= 0.5) ? (lum * (1.0 + satur)) : (lum + satur - lum * satur);
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;
                m = lum + lum - v;
                sv = (v - m) / v;
                hue *= 6.0;
                sextant = (int)hue;
                fract = hue - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;

                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;

                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;

                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;

                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;

                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            Color rgb;
            rgb.R = Convert.ToByte(r * 255.0f);
            rgb.G = Convert.ToByte(g * 255.0f);
            rgb.B = Convert.ToByte(b * 255.0f);
            rgb.A = alpha;
            return rgb;
        }

        public static Control GetValueControl(IValue value, Color currentParenthesisTheme)
        {
            if (value is ConstantBool)
                return new Editor.ConstantBool(value as ConstantBool);
            else if (value is ConstantChar)
                return new Editor.ConstantChar(value as ConstantChar);
            else if (value is ConstantFloat)
                return new Editor.ConstantFloat(value as ConstantFloat);
            else if (value is ConstantInt)
                return new Editor.ConstantInt(value as ConstantInt);
            else if (value is ConstantString)
                return new Editor.ConstantString(value as ConstantString);
            else if (value is EnumCall)
                return new Editor.EnumCall(value as EnumCall);
            else if (value is FunctionCall)
                return new Editor.FunctionCall(value as FunctionCall, currentParenthesisTheme);
            else if (value is Operator)
                return new Editor.Operator(value as Operator, currentParenthesisTheme);
            else if (value is StructCall)
                return new Editor.StructCall(value as StructCall);
            else if (value is TableCall)
                return new Editor.TableCall(value as TableCall, currentParenthesisTheme);
            else if (value is Variable)
                return new Editor.Variable(value as Variable);
            else
                return null;
        }

        public static Color HashColor(Color c)
        {
            var colorList = new List<Color>()
            {
                SymbolColorBrush.Color,
                Color.FromArgb(255, 255, 153, 0),
                Color.FromArgb(255, 255, 20, 147),
                Color.FromArgb(255, 154, 205, 50),
                Color.FromArgb(255, 148, 0, 211)
            };
            for (int i = 0; i < colorList.Count - 1; i++)
            {
                if (c == colorList[i])
                    return colorList[i + 1];
            }
            var hsl = GetHSL(c);
            hsl.Item1 = (120 + hsl.Item1) % 360;
            return GetRGB(hsl);
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

        public static BitmapSource MakeDarkTheme(BitmapSource image)
        {
            var src = ConvertWPFToBitmap(image);
            MakeDarkTheme(src);
            return ConvertBitmapToWPF(MakeDarkTheme(src));
        }

        public static System.Drawing.Bitmap MakeDarkTheme(System.Drawing.Bitmap bitmap)
        {
            bitmap = new System.Drawing.Bitmap(bitmap);
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    bitmap.SetPixel(x, y, GetDarkThemeColor(bitmap.GetPixel(x, y)));
                }
            }
            return bitmap;
        }

        public static void NewFile()
        {
            var dialog = new NewFileDialog();
            dialog.Owner = Current.MainWindow;
            dialog.ShowDialog();
        }

        public static void OpenFile()
        {
            string path = CurrentWorkspace != null ? CurrentWorkspace : "";
            var dialog = new OpenFileDialog
            {
                FileName = path,
                Title = "Ouvrir un fichier",
                Filter = "Algo|*.ila|Tous les fichiers|*.*",
                Multiselect = true
            };
            if (dialog.ShowDialog().Value)
            {
                foreach (var item in dialog.FileNames)
                {
                    var workspace = item;
                    using var sr = new StreamReader(item);
                    Program prog;
                    try
                    {
                        prog = ILANET.Parser.Parser.Parse(sr.ReadToEnd(), true);
                        Console.WriteLine("'" + prog.Name + "' chargé avec succès");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ILAcodes.Add(prog);
                    Workspaces.Add(workspace);
                    UpdateTabs();
                    Tabs.SelectedIndex = Tabs.Items.Count - 1;
                }
            }
        }

        public static void OpenSettings()
        {
            var dialog = new SettingsDialog();
            dialog.Owner = Current.MainWindow;
            dialog.ShowDialog();
        }

        public static void PasteInstruction(IDropableInstruction insertHere)
        {
            var clipContent = Clipboard.GetText().Trim();
            if (!string.IsNullOrEmpty(clipContent))
            {
                var instrus = new List<Instruction>();
                try
                {
                    int index = 0;
                    while (index < clipContent.Length)
                    {
                        var tmp = ILANET.Parser.Parser.ParseInstruction(clipContent, CurrentILAcode, CurrentExecutable, ref index, true);
                        if (tmp != null)
                            instrus.Add(tmp);
                    }
                }
                catch (ILANET.Parser.Parser.ILAException e)
                {
                    MessageBox.Show(e.Message, "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception)
                { }
                foreach (var instru in instrus)
                {
                    var control = GetInstructionControl(instru);
                    insertHere.DropRecieved(control as IDropableInstruction);
                }
            }
        }

        public static bool recursiveSearch(IEnumerable<IDropableInstruction> array, IDropableInstruction toFind)
        {
            foreach (var item in array)
            {
                if (item == toFind)
                    return true;
                if (item is InstructionBlock ib)
                    if (recursiveSearch(ib.Instructions, toFind))
                        return true;
            }
            return false;
        }

        public static void RunCurrentILA()
        {
            if (CurrentILAcode == null)
                MessageBox.Show("Veuillez selectionner un programme à lancer", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (Executing != null)
                MessageBox.Show("Veuillez fermer le programme actuel", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (SaveCurrent())
            {
                {
                    using var stream = new StreamReader(CurrentWorkspace);
                    try
                    {
                        var prog = ILANET.Parser.Parser.Parse(stream.ReadToEnd());
                    }
                    catch (ILANET.Parser.Parser.ILAException e)
                    {
                        MessageBox.Show(e.Message, "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                var proc = new Process();
                proc.Exited += (sender, e) =>
                {
                    Console.LockConsoles();
                    Executing = null;
                    MainDialog.Dispatcher.Invoke(() => MainDialog.stopBtn.IsEnabled = false);
                    Console.WriteLine("Programme terminé");
                };
                proc.StartInfo = new ProcessStartInfo
                {
                    Arguments = '"' + CurrentWorkspace + '"',
                    FileName = "ila.exe",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = false
                };
                proc.EnableRaisingEvents = true;
                proc.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                proc.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);
                Executing = proc;
                proc.Start();
                MainDialog.stopBtn.IsEnabled = true;
                proc.BeginOutputReadLine();
                Console.RuntimeInput = proc.StandardInput;
                Console.UnlockConsoles();
                Console.ActiveConsoles.Last().inputTB.Focus();
            }
        }

        public static bool SaveCurrent()
        {
            if (CurrentILAcode == null)
                return false;
            string path = CurrentWorkspace != null ? CurrentWorkspace : "";
            if (path == "")
            {
                var dialog = new SaveFileDialog
                {
                    Title = "Sauvegarder un fichier",
                    Filter = "Algo|*.ila|Tous les fichiers|*.*"
                };
                if (dialog.ShowDialog().Value)
                {
                    CurrentWorkspace = dialog.FileName;
                    using var sr = new StreamWriter(CurrentWorkspace);
                    CurrentILAcode.WriteILA(sr);
                    return true;
                }
                else
                    return false;
            }
            else
            {
                using var sr = new StreamWriter(CurrentWorkspace);
                CurrentILAcode.WriteILA(sr);
                return true;
            }
        }

        public static void Undo()
        {
        }

        public static void Redo()
        {
        }

        public static System.Drawing.Color ToClassic(Color c) => System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B);

        public static Color ToWPF(System.Drawing.Color c) => Color.FromArgb(c.A, c.R, c.G, c.B);
    }
}