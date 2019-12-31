using ILANET;
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
    /// Logique d'interaction pour FunctionCall.xaml
    /// </summary>
    public partial class FunctionCall : UserControl, Linked
    {
        public FunctionCall(ILANET.FunctionCall value)
        {
            InitializeComponent();
            InternalValue = value;
            fctName.Text = value.CalledFunction.Name;
            for (int i = 0; i < value.Args.Count; i++)
            {
                parameters.Children.Add(App.GetValueControl(value.Args[i]));
                if (i < value.Args.Count - 1)
                    parameters.Children.Add(new TextBlock()
                    {
                        Text = ", ",
                        Foreground = App.SymbolColorBrush,
                        FontFamily = fctName.FontFamily
                    });
            }
        }

        public ILANET.FunctionCall InternalValue { get; set; }
        IBaseObject Linked.Link => InternalValue;
    }
}