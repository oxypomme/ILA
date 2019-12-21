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
using ILANET;
using System.IO;

namespace ilaGUI
{
    /// <summary>
    /// Logique d'interaction pour createTable.xaml
    /// </summary>
    public partial class createTable : Window
    {
        private bool editing;
        private TableType table;
        private TypeDeclaration Type;

        public createTable(TypeDeclaration type, bool edit)
        {
            editing = edit;
            Type = type;
            table = type.CreatedType as TableType;
            InitializeComponent();
            Background = App.DarkBackground;
            typeName.Text = table.Name;
            comments.Text = type.AboveComment?.Message;
            inlineComm.Text = type.InlineComment;
            foreach (var item in table.DimensionsSize)
                dimList.Children.Add(new dimension(item, table));
            typeName.Focus();
            typeName.SelectAll();
        }

        private void addDim_Click(object sender, RoutedEventArgs e)
        {
            var range = new ILANET.Range(new ConstantInt { Value = 1 }, new ConstantInt { Value = 10 });
            var dim = new dimension(range, table);
            dimList.Children.Add(dim);
            //https://stackoverflow.com/a/56289573
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                dim.maxValue.Focus();
            }), System.Windows.Threading.DispatcherPriority.Render);
            dim.maxValue.SelectAll();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void validateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!App.isNameConventionnal(typeName.Text))
            {
                MessageBox.Show(this, "Nom non conventionnel", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if ((table.Name != typeName.Text || !editing) && !App.isNameAvailable(typeName.Text))
            {
                MessageBox.Show(this, "Nom non conventionnel", "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            foreach (dimension item in dimList.Children)
            {
                try
                {
                    ILANET.Parser.Parser.ParseValue(item.minValue.Text, App.CurrentILAcode, App.CurrentExecutable, true);
                    ILANET.Parser.Parser.ParseValue(item.maxValue.Text, App.CurrentILAcode, App.CurrentExecutable, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            DialogResult = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                DialogResult = false;
        }
    }
}