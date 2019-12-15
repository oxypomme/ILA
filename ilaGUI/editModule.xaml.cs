﻿using ILANET;
using System;
using System.Collections.Generic;
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
    /// Logique d'interaction pour editModule.xaml
    /// </summary>
    public partial class createModule : Window
    {
        public createModule(Module mod, bool edit = false)
        {
            InitializeComponent();
            Background = App.DarkBackground;
            if (edit)
                Title = "Editer ";
            else
                Title = "Créer ";
            if (mod is Function)
                Title += "une fonction";
            else
                Title += "un module";
            if (mod is Function f)
            {
                returnType.Items.Add(new ToStringOverrider(GenericType.Int, () => "entier"));
                returnType.Items.Add(new ToStringOverrider(GenericType.Float, () => "reel"));
                returnType.Items.Add(new ToStringOverrider(GenericType.Char, () => "caractere"));
                returnType.Items.Add(new ToStringOverrider(GenericType.Bool, () => "booleen"));
                returnType.Items.Add(new ToStringOverrider(GenericType.String, () => "chaine"));
                foreach (var item in App.CurrentILAcode.Declarations)
                    if (item is TypeDeclaration td)
                        returnType.Items.Add(new ToStringOverrider(td.CreatedType, () => td.CreatedType.Name));
                foreach (var item in returnType.Items)
                {
                    if (f.ReturnType == ((ToStringOverrider)item).Content)
                    {
                        returnType.SelectedItem = item;
                        break;
                    }
                }
            }
            else
                returnType.Visibility = Visibility.Collapsed;
            paramList.Children.Add(new Parameter(new ILANET.Parameter
            {
                ImportedVariable = new Variable
                {
                    Name = "test",
                    Type = GenericType.String
                },
                Mode = ILANET.Parameter.Flags.IO
            }));
            paramList.Children.Add(new Parameter(new ILANET.Parameter
            {
                ImportedVariable = new Variable
                {
                    Name = "result",
                    Type = GenericType.Bool
                },
                Mode = ILANET.Parameter.Flags.OUTPUT
            }));
            foreach (var item in mod.Parameters)
                paramList.Children.Add(new Parameter(item));
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void validateBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                DialogResult = false;
        }
    }
}