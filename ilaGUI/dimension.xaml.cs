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
using System.IO;

namespace ilaGUI
{
    /// <summary>
    /// Logique d'interaction pour dimension.xaml
    /// </summary>
    public partial class dimension : UserControl
    {
        private ILANET.Range range;
        private ILANET.TableType table;

        public dimension(ILANET.Range range, ILANET.TableType type)
        {
            this.range = range;
            table = type;
            InitializeComponent();
            (removeBtn.Content as Image).Source = App.MakeDarkTheme((removeBtn.Content as Image).Source as BitmapSource);
            var sw = new StringWriter();
            range.Min.WriteILA(sw);
            minValue.Text = sw.ToString();
            sw.GetStringBuilder().Clear();
            range.Max.WriteILA(sw);
            maxValue.Text = sw.ToString();
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            table.DimensionsSize.Remove(range);
            var pannel = Parent as StackPanel;
            pannel.Children.Remove(this);
            if (pannel.Children.Count == 1)
                (pannel.Children[0] as dimension).removeBtn.Visibility = Visibility.Collapsed;
        }
    }
}