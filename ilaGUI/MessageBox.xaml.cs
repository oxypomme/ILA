using System;
using System.Collections.Generic;
using System.IO;
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
    /// Logique d'interaction pour MessageBox.xaml
    /// </summary>
    public partial class MessageBox : Window
    {
        private static readonly ImageSource ErrorIcon = App.ConvertBitmapToWPF(Properties.Resources.error_icon);

        private static readonly ImageSource InfoIcon = App.ConvertBitmapToWPF(Properties.Resources.info);

        private static readonly ImageSource QuestionIcon = App.ConvertBitmapToWPF(Properties.Resources.question_icon);

        private static readonly ImageSource WarningIcon = App.ConvertBitmapToWPF(Properties.Resources.warning_icon);

        private MessageBoxButton button;
        private bool managed;
        private MessageBoxResult result;

        private MessageBox()
        {
            InitializeComponent();
            Background = App.DarkBackground;
            managed = false;
        }

        public static MessageBoxResult Show(string messageBoxText, string caption = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None)
            => Show(null, messageBoxText, caption, button, icon, defaultResult);

        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None)
        {
            var dialog = new MessageBox();
            //dialog.Owner = owner;
            dialog.Title = caption;
            dialog.button = button;
            dialog.text.Text = messageBoxText;
            switch (icon)
            {
                case MessageBoxImage.Error:
                    System.Media.SystemSounds.Hand.Play();
                    dialog.iconImg.Source = ErrorIcon;
                    break;

                case MessageBoxImage.Question:
                    System.Media.SystemSounds.Question.Play();
                    dialog.iconImg.Source = QuestionIcon;
                    break;

                case MessageBoxImage.Exclamation:
                    System.Media.SystemSounds.Exclamation.Play();
                    dialog.iconImg.Source = WarningIcon;
                    break;

                case MessageBoxImage.Asterisk:
                    System.Media.SystemSounds.Asterisk.Play();
                    dialog.iconImg.Source = InfoIcon;
                    break;
            }
            switch (button)
            {
                case MessageBoxButton.OK:
                    {
                        var tmp = new Button
                        {
                            Content = "OK",
                            Margin = new Thickness(10),
                            Padding = new Thickness(10),
                            Style = dialog.FindResource("neutralBtnStyle") as Style,
                            MinWidth = 100,
                            Foreground = App.DarkFontColor,
                            IsDefault = true
                        };
                        tmp.Click += (sender, e) =>
                        {
                            dialog.result = MessageBoxResult.OK;
                            dialog.managed = true;
                            dialog.Close();
                        };
                        dialog.buttons.Children.Add(tmp);
                    }
                    break;

                case MessageBoxButton.OKCancel:
                    {
                        var tmp = new Button
                        {
                            Content = "OK",
                            Margin = new Thickness(10),
                            Padding = new Thickness(10),
                            Style = dialog.FindResource("neutralBtnStyle") as Style,
                            MinWidth = 100,
                            Foreground = App.DarkFontColor
                        };
                        tmp.Click += (sender, e) =>
                        {
                            dialog.result = MessageBoxResult.OK;
                            dialog.managed = true;
                            dialog.Close();
                        };
                        if (defaultResult == MessageBoxResult.None || defaultResult == MessageBoxResult.OK)
                            tmp.IsDefault = true;
                        dialog.buttons.Children.Add(tmp);
                    }
                    {
                        var tmp = new Button
                        {
                            Content = "Annuler",
                            Margin = new Thickness(10),
                            Padding = new Thickness(10),
                            Style = dialog.FindResource("cancelBtnStyle") as Style,
                            MinWidth = 100,
                            Foreground = App.DarkFontColor
                        };
                        tmp.Click += (sender, e) =>
                        {
                            dialog.result = MessageBoxResult.Cancel;
                            dialog.managed = true;
                            dialog.Close();
                        };
                        if (defaultResult == MessageBoxResult.Cancel)
                            tmp.IsDefault = true;
                        dialog.buttons.Children.Add(tmp);
                    }
                    break;

                case MessageBoxButton.YesNoCancel:
                    {
                        var tmp = new Button
                        {
                            Content = "Oui",
                            Margin = new Thickness(10),
                            Padding = new Thickness(10),
                            Style = dialog.FindResource("validateBtnStyle") as Style,
                            MinWidth = 100,
                            Foreground = App.DarkFontColor
                        };
                        tmp.Click += (sender, e) =>
                        {
                            dialog.result = MessageBoxResult.Yes;
                            dialog.managed = true;
                            dialog.Close();
                        };
                        if (defaultResult == MessageBoxResult.None || defaultResult == MessageBoxResult.Yes)
                            tmp.IsDefault = true;
                        dialog.buttons.Children.Add(tmp);
                    }
                    {
                        var tmp = new Button
                        {
                            Content = "Non",
                            Margin = new Thickness(10),
                            Padding = new Thickness(10),
                            Style = dialog.FindResource("cancelBtnStyle") as Style,
                            MinWidth = 100,
                            Foreground = App.DarkFontColor
                        };
                        tmp.Click += (sender, e) =>
                        {
                            dialog.result = MessageBoxResult.No;
                            dialog.managed = true;
                            dialog.Close();
                        };
                        if (defaultResult == MessageBoxResult.No)
                            tmp.IsDefault = true;
                        dialog.buttons.Children.Add(tmp);
                    }
                    {
                        var tmp = new Button
                        {
                            Content = "Annuler",
                            Margin = new Thickness(10),
                            Padding = new Thickness(10),
                            Style = dialog.FindResource("neutralBtnStyle") as Style,
                            MinWidth = 100,
                            Foreground = App.DarkFontColor
                        };
                        tmp.Click += (sender, e) =>
                        {
                            dialog.result = MessageBoxResult.Cancel;
                            dialog.managed = true;
                            dialog.Close();
                        };
                        if (defaultResult == MessageBoxResult.Cancel)
                            tmp.IsDefault = true;
                        dialog.buttons.Children.Add(tmp);
                    }
                    break;

                case MessageBoxButton.YesNo:
                    {
                        var tmp = new Button
                        {
                            Content = "Oui",
                            Margin = new Thickness(10),
                            Padding = new Thickness(10),
                            Style = dialog.FindResource("validateBtnStyle") as Style,
                            MinWidth = 100,
                            Foreground = App.DarkFontColor
                        };
                        tmp.Click += (sender, e) =>
                        {
                            dialog.result = MessageBoxResult.Yes;
                            dialog.managed = true;
                            dialog.Close();
                        };
                        if (defaultResult == MessageBoxResult.None || defaultResult == MessageBoxResult.Yes)
                            tmp.IsDefault = true;
                        dialog.buttons.Children.Add(tmp);
                    }
                    {
                        var tmp = new Button
                        {
                            Content = "Non",
                            Margin = new Thickness(10),
                            Padding = new Thickness(10),
                            Style = dialog.FindResource("cancelBtnStyle") as Style,
                            MinWidth = 100,
                            //Foreground = App.DarkFontColor
                        };
                        tmp.Click += (sender, e) =>
                        {
                            dialog.result = MessageBoxResult.No;
                            dialog.managed = true;
                            dialog.Close();
                        };
                        if (defaultResult == MessageBoxResult.No)
                            tmp.IsDefault = true;
                        dialog.buttons.Children.Add(tmp);
                    }
                    break;
            }
            dialog.ShowDialog();
            return dialog.result;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (!managed)
                switch (button)
                {
                    case MessageBoxButton.OK:
                        result = MessageBoxResult.None;
                        break;

                    case MessageBoxButton.OKCancel:
                        result = MessageBoxResult.Cancel;
                        break;

                    case MessageBoxButton.YesNoCancel:
                        result = MessageBoxResult.Cancel;
                        break;

                    case MessageBoxButton.YesNo:
                        result = MessageBoxResult.No;
                        break;
                }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                switch (button)
                {
                    case MessageBoxButton.OK:
                        result = MessageBoxResult.None;
                        break;

                    case MessageBoxButton.OKCancel:
                        result = MessageBoxResult.Cancel;
                        break;

                    case MessageBoxButton.YesNoCancel:
                        result = MessageBoxResult.Cancel;
                        break;

                    case MessageBoxButton.YesNo:
                        result = MessageBoxResult.No;
                        break;
                }
                Close();
            }
        }
    }
}