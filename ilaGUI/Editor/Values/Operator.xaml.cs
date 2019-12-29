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
    /// Logique d'interaction pour Operator.xaml
    /// </summary>
    public partial class Operator : UserControl, Linked
    {
        public Operator(ILANET.Operator value)
        {
            InitializeComponent();
            InternalValue = value;
            if (value.Left != null)
                leftOperand.Content = App.GetValueControl(value.Left);
            rightOperand.Content = App.GetValueControl(value.Right);
            switch (value.OperatorType)
            {
                case ILANET.Operator.Tag.MINUS:
                    operatorType.Text = " -";
                    break;

                case ILANET.Operator.Tag.ADD:
                    operatorType.Text = " + ";
                    break;

                case ILANET.Operator.Tag.SUB:
                    operatorType.Text = " - ";
                    break;

                case ILANET.Operator.Tag.DIV:
                    operatorType.Text = " / ";
                    break;

                case ILANET.Operator.Tag.MULT:
                    operatorType.Text = " * ";
                    break;

                case ILANET.Operator.Tag.INT_DIV:
                    operatorType.Text = " div ";
                    operatorType.Foreground = new SolidColorBrush(Colors.LightBlue);
                    break;

                case ILANET.Operator.Tag.MOD:
                    operatorType.Text = " mod ";
                    operatorType.Foreground = new SolidColorBrush(Colors.LightBlue);
                    break;

                case ILANET.Operator.Tag.AND:
                    operatorType.Text = " et ";
                    operatorType.Foreground = new SolidColorBrush(Colors.LightBlue);
                    break;

                case ILANET.Operator.Tag.OR:
                    operatorType.Text = " ou ";
                    operatorType.Foreground = new SolidColorBrush(Colors.LightBlue);
                    break;

                case ILANET.Operator.Tag.NOT:
                    operatorType.Text = " non ";
                    operatorType.Foreground = new SolidColorBrush(Colors.LightBlue);
                    break;

                case ILANET.Operator.Tag.EQUAL:
                    operatorType.Text = " = ";
                    break;

                case ILANET.Operator.Tag.DIFFRENT:
                    operatorType.Text = " != ";
                    break;

                case ILANET.Operator.Tag.BIGGER:
                    operatorType.Text = " > ";
                    break;

                case ILANET.Operator.Tag.BIGGER_EQUAL:
                    operatorType.Text = " >= ";
                    break;

                case ILANET.Operator.Tag.SMALLER:
                    operatorType.Text = " < ";
                    break;

                case ILANET.Operator.Tag.SMALLER_EQUAL:
                    operatorType.Text = " <= ";
                    break;
            }
        }

        public ILANET.Operator InternalValue { get; set; }

        IBaseObject Linked.Link => InternalValue;
    }
}