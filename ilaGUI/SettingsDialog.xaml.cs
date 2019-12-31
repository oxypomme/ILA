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
            // TODO: Non hard-coded background color
            themeSelector.Background = new SolidColorBrush(Color.FromRgb(0x41, 0x3c, 0x42));
            moduleSelector.Background = new SolidColorBrush(Color.FromRgb(0x41, 0x3c, 0x42));

            App.Settings.GenConfig();
        }

        private void validateBtn_Click(object sender, RoutedEventArgs e)
        {
            App.Settings.settings.editor_font = editorFontTB.Text;
            App.Settings.settings.code_font = codeFontTB.Text;
            App.Settings.settings.font_size = int.Parse(fontSizeTB.Text);
            App.Settings.settings.intr_icons = iconsCB.IsChecked.Value;

            App.Settings.UpdateConfigJSON();

            DialogResult = true;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }

    public class Setting
    {
        public struct Settings
        {
            public string editor_font;
            public string code_font;
            public int font_size;
            public bool intr_icons;
        }

        public Settings settings = new Settings();

        public void GenConfig()
        {
            if (!File.Exists("settings.json"))
            {
                File.Create("settings.json").Close();

                settings.editor_font = "Roboto";
                settings.code_font = "FiraCode";
                settings.font_size = 12;
                settings.intr_icons = false;

                UpdateConfigJSON();
            }
            else
            {
                Setting sets;
                using (StreamReader file = new StreamReader("settings.json"))
                {
                    sets = JsonConvert.DeserializeObject<ilaGUI.Setting>(file.ReadToEnd());
                    file.Close();
                }

                settings.editor_font = sets.settings.editor_font;
                settings.code_font = sets.settings.code_font;
                settings.font_size = sets.settings.font_size;
                settings.intr_icons = sets.settings.intr_icons;
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