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
using System.Windows.Shapes;

namespace ilaGUI
{
    /// <summary>
    /// Logique d'interaction pour createStruct.xaml
    /// </summary>
    public partial class createStruct : Window, Linked
    {
        public createStruct()
        {
            InitializeComponent();
        }

        public IBaseObject Link { get; set; }
    }
}