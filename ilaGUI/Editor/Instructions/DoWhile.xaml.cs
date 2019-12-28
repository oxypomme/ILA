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
    /// Logique d'interaction pour DoWhile.xaml
    /// </summary>
    public partial class DoWhile : UserControl, Linked, InstructionBlock, IDropableInstruction
    {
        public DoWhile()
        {
            InitializeComponent();
            EndInsturction = new DummyInstruction();
            //Tag
        }

        public bool DropVisual { set => throw new NotImplementedException(); }
        public DummyInstruction EndInsturction { get; set; }
        public IDropableInstruction[] Instructions => throw new NotImplementedException();
        public ILANET.DoWhile InternalInstruction { get; set; }
        IBaseObject Linked.Link => InternalInstruction;
        public bool MovingVisual { set => throw new NotImplementedException(); }

        public void UpdateInternalInstructions()
        {
        }

        public void UpdateVisuals()
        {
        }
    }
}