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
    /// Logique d'interaction pour Comment.xaml
    /// </summary>
    public partial class Comment : UserControl, Linked
    {
        public Comment()
        {
            InitializeComponent();
        }

        public ILANET.Comment InternalComment { get; set; }

        IBaseObject Linked.Link => InternalComment;
    }
}