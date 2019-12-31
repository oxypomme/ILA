using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ilaGUI
{
    /// <summary>
    /// Logique d'interaction pour Settings.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        public SettingsDialog()
        {
            InitializeComponent();
            Background = App.DarkBackground;
        }
    }

    public class Settings
    {
        public string EditorFont { get; set; }
        public string CodeFont { get; set; }

        public void GenConfig()
        {
            if (!File.Exists("settings.json"))
            {
                File.Create("settings.json").Close();

                EditorFont = "Roboto";
                CodeFont = "FiraCode";

                UpdateConfigJSON();
            }
            else
            {
                Settings settings;
                using (StreamReader file = new StreamReader("settings.json"))
                {
                    settings = JsonConvert.DeserializeObject<Settings>(file.ReadToEnd());
                    file.Close();
                }

                EditorFont = settings.EditorFont;
                CodeFont = settings.CodeFont;
            }
        }

        public void UpdateConfigJSON()
        {
            using StreamWriter file = new StreamWriter("settings.json");
            file.Write(JsonConvert.SerializeObject(this));
            file.Close();
        }
    }
}