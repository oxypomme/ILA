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
    /// Logique d'interaction pour TableCall.xaml
    /// </summary>
    public partial class TableCall : UserControl, Linked
    {
        public readonly Color ParenthesisColor;

        public TableCall(ILANET.TableCall value, Color c)
        {
            InitializeComponent();
            InternalValue = value;
            ParenthesisColor = c;
            variable.Content = App.GetValueControl(value.Table, c);
            for (int i = 0; i < value.DimensionsIndex.Count; i++)
            {
                parameters.Children.Add(App.GetValueControl(value.DimensionsIndex[i], c));
                if (i < value.DimensionsIndex.Count - 1)
                    parameters.Children.Add(new TextBlock()
                    {
                        Foreground = App.SymbolColorBrush,
                        FontFamily = linkToFont.FontFamily,
                        Text = ", "
                    });
            }
        }

        public ILANET.TableCall InternalValue { get; set; }

        IBaseObject Linked.Link => InternalValue;
    }
}