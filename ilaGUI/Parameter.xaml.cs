﻿using ILANET;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ilaGUI
{
    /// <summary>
    /// Logique d'interaction pour Parameter.xaml
    /// </summary>
    public partial class Parameter : UserControl, Linked
    {
        private Module scope;

        public Parameter(ILANET.Parameter param, Module scope)
        {
            InitializeComponent();
            (removeParam.Content as Image).Source = App.MakeDarkTheme((removeParam.Content as Image).Source as BitmapSource);
            (editParam.Content as Image).Source = App.MakeDarkTheme((editParam.Content as Image).Source as BitmapSource);
            this.scope = scope;
            Link = param;
            paramName.Text = param.ImportedVariable.Name;
            if (param.Mode == ILANET.Parameter.Flags.OUTPUT)
                prefixName.Text = "s";
            else if (param.Mode == ILANET.Parameter.Flags.IO)
                prefixName.Text = "es";
            else if (param.Mode == ILANET.Parameter.Flags.INPUT)
                dbPoints.Visibility = Visibility.Collapsed;
            if (param.ImportedVariable.Type == GenericType.Int)
            {
                iconType.Source = App.MakeDarkTheme(App.GetBitmapImage(new MemoryStream(Properties.Resources.int_var)));
                paramType.Text = "entier";
            }
            else if (param.ImportedVariable.Type == GenericType.Float)
            {
                iconType.Source = App.MakeDarkTheme(App.GetBitmapImage(new MemoryStream(Properties.Resources.float_var)));
                paramType.Text = "reel";
            }
            else if (param.ImportedVariable.Type == GenericType.Char)
            {
                iconType.Source = App.MakeDarkTheme(App.GetBitmapImage(new MemoryStream(Properties.Resources._char)));
                paramType.Text = "caractere";
            }
            else if (param.ImportedVariable.Type == GenericType.Bool)
            {
                iconType.Source = App.MakeDarkTheme(App.GetBitmapImage(new MemoryStream(Properties.Resources._bool)));
                paramType.Text = "booleen";
            }
            else if (param.ImportedVariable.Type == GenericType.String)
            {
                iconType.Source = App.MakeDarkTheme(App.GetBitmapImage(new MemoryStream(Properties.Resources._string)));
                paramType.Text = "chaine";
            }
            else if (param.ImportedVariable.Type is StructType st)
            {
                iconType.Source = App.MakeDarkTheme(App.GetBitmapImage(new MemoryStream(Properties.Resources._struct)));
                paramType.Text = st.Name;
            }
            else if (param.ImportedVariable.Type is EnumType et)
            {
                iconType.Source = App.MakeDarkTheme(App.GetBitmapImage(new MemoryStream(Properties.Resources._enum)));
                paramType.Text = et.Name;
            }
            else if (param.ImportedVariable.Type is TableType tt)
            {
                iconType.Source = App.MakeDarkTheme(App.GetBitmapImage(new MemoryStream(Properties.Resources.table)));
                paramType.Text = tt.Name;
            }
            else
            {
                iconType.Source = App.MakeDarkTheme(App.GetBitmapImage(new MemoryStream(Properties.Resources.custom_var)));
                paramType.Text = param.ImportedVariable.Type.Name;
            }
        }

        public IBaseObject Link { get; set; }

        private void editParam_Click(object sender, RoutedEventArgs e)
        {
            App.editParameter(this, scope);
        }

        private void removeParam_Click(object sender, RoutedEventArgs e)
        {
            ((StackPanel)Parent).Children.Remove(this);
            scope.Parameters.Remove(Link as ILANET.Parameter);
        }
    }
}