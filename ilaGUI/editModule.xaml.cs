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
using System.IO;

namespace ilaGUI
{
    /// <summary>
    /// Logique d'interaction pour editModule.xaml
    /// </summary>
    public partial class createModule : Window
    {
        private UIElement _dummyDragSource = new UIElement();
        private bool _isDown;
        private bool _isDragging;
        private UIElement _realDragSource;
        private Point _startPoint;
        private Button AddParamButton;

        public createModule(Module mod, bool edit = false)
        {
            InitializeComponent();
            modName.Focus();
            AddParamButton = new Button();
            AddParamButton.Height = 20;
            AddParamButton.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            AddParamButton.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            {
                var img = new Image();
                img.Source = App.GetBitmapImage(new MemoryStream(Properties.Resources.add));
                img.Stretch = Stretch.None;
                AddParamButton.Content = img;
            }
            AddParamButton.Click += (object sender, RoutedEventArgs e) =>
            {
                var p = App.createParameter(mod);
                if (p != null)
                {
                    paramList.Children.Add(p);
                    mod.Parameters.Add(p.Link as ILANET.Parameter);
                    paramList.Children.Remove(AddParamButton);
                    paramList.Children.Add(AddParamButton);
                }
            };
            Background = App.DarkBackground;
            modName.Text = mod.Name;
            if (edit)
            {
                Title = "Editer ";
                validateBtn.Content = "Modifier";
            }
            else
                Title = "Créer ";
            if (mod is Function)
                Title += "une fonction";
            else
                Title += "un module";
            if (mod is Function f)
            {
                returnType.Items.Add(new ToStringOverrider(GenericType.Int, () => "entier"));
                returnType.Items.Add(new ToStringOverrider(GenericType.Float, () => "reel"));
                returnType.Items.Add(new ToStringOverrider(GenericType.Char, () => "caractere"));
                returnType.Items.Add(new ToStringOverrider(GenericType.Bool, () => "booleen"));
                returnType.Items.Add(new ToStringOverrider(GenericType.String, () => "chaine"));
                foreach (var item in App.CurrentILAcode.Declarations)
                    if (item is TypeDeclaration td)
                        returnType.Items.Add(new ToStringOverrider(td.CreatedType, () => td.CreatedType.Name));
                foreach (var item in returnType.Items)
                {
                    if (f.ReturnType == ((ToStringOverrider)item).Content)
                    {
                        returnType.SelectedItem = item;
                        break;
                    }
                }
            }
            else
                fctOnly.Visibility = Visibility.Collapsed;
            foreach (var item in mod.Parameters)
                paramList.Children.Add(new Parameter(item, mod));
            paramList.Children.Add(AddParamButton);
            if (mod.AboveComment != null)
                comments.Text = mod.AboveComment.Message;
            inlineComm.Text = mod.InlineComment;
            modName.SelectAll();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void paramList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("UIElement"))
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        private void paramList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("UIElement"))
            {
                UIElement droptarget = e.Source as UIElement;
                int droptargetIndex = -1, i = 0;
                foreach (UIElement element in paramList.Children)
                {
                    if (element.Equals(droptarget))
                    {
                        droptargetIndex = i;
                        break;
                    }
                    i++;
                }
                if (droptargetIndex != -1)
                {
                    paramList.Children.Remove(_realDragSource);
                    paramList.Children.Insert(droptargetIndex, _realDragSource);
                    paramList.Children.Remove(AddParamButton);
                    paramList.Children.Add(AddParamButton);
                }

                _isDown = false;
                _isDragging = false;
                _realDragSource.ReleaseMouseCapture();
            }
        }

        private void paramList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == paramList)
            {
            }
            else
            {
                _isDown = true;
                _startPoint = e.GetPosition(paramList);
            }
        }

        private void paramList_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDown = false;
            _isDragging = false;
            _realDragSource?.ReleaseMouseCapture();
        }

        private void paramList_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDown && e.Source is Parameter)
            {
                if ((_isDragging == false) && ((Math.Abs(e.GetPosition(paramList).X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(e.GetPosition(paramList).Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                {
                    _isDragging = true;
                    _realDragSource = e.Source as UIElement;
                    _realDragSource.CaptureMouse();
                    DragDrop.DoDragDrop(_dummyDragSource, new DataObject("UIElement", e.Source, true), DragDropEffects.Move);
                }
            }
        }

        private void validateBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                DialogResult = false;
        }
    }
}