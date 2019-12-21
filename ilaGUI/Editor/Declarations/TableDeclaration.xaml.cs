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
    /// Logique d'interaction pour TableDeclaration.xaml
    /// </summary>
    public partial class TableDeclaration : UserControl, Linked
    {
        public TableDeclaration()
        {
            InitializeComponent();
        }

        public ILANET.TypeDeclaration InternalDeclaration { get; set; }

        IBaseObject Linked.Link => InternalDeclaration;
    }
}