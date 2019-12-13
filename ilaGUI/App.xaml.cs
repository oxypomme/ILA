using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
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
        public static List<Program> ILAcodes;

        public App()
        {
            ILAcodes = new List<Program>();
            CurrentILAcode = new Program();
            CurrentILAcode.Name = "main";
            ILAcodes.Add(CurrentILAcode);
            CurrentExecutable = CurrentILAcode;
        }

        public static IExecutable CurrentExecutable { get; set; }
        public static Program CurrentILAcode { get; set; }
        public static TabControl Tabs { get; set; }
        public static Tree Tree { get; set; }

        public static VarType createType(int type) //0 = struct, 1 = table, 2 = enum
        {
            return null;
        }

        public static Variable createVar(int type) //0 = int, 1 = float, 2 = char, 3 = bool, 4 = string, 5 = custom
        {
            return null;
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