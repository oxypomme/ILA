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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ilaGUI.Editor
{
    /// <summary>
    /// Logique d'interaction pour StructCall.xaml
    /// </summary>
    public partial class StructCall : UserControl, Linked
    {
        public StructCall(ILANET.StructCall value)
        {
            InitializeComponent();
            InternalValue = value;
            memberName.Text = value.Name;
            structValue.Content = App.GetValueControl(value.Struct, App.SymbolColorBrush.Color);
        }

        public ILANET.StructCall InternalValue { get; set; }

        IBaseObject Linked.Link => InternalValue;
    }
}