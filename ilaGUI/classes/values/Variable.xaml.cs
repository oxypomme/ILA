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
    /// Logique d'interaction pour Variable.xaml
    /// </summary>
    public partial class Variable : UserControl, Linked
    {
        public Variable()
        {
            InitializeComponent();
        }

        public ILANET.Variable InternalValue { get; set; }
        IBaseObject Linked.Link => InternalValue;
    }
}