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

        public void UpdateConfigJSON()
        {
            using StreamWriter file = new StreamWriter("settings.json");
            file.Write(JsonConvert.SerializeObject(this));
            file.Close();
        }
    }
}