using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ilaGUI
{
    /// <summary>
    /// Logique d'interaction pour Console.xaml
    /// </summary>
    public partial class Console : UserControl
    {
        private static bool ConsolesUnlocked = false;

        public Console()
        {
            InitializeComponent();
            (newConsole.Content as Image).Source = App.MakeDarkTheme((newConsole.Content as Image).Source as BitmapSource);
            ActiveConsoles.Add(this);
            inputTB.IsEnabled = ConsolesUnlocked;
            if (ConsolesUnlocked)
            {
                upperSeparator.Background = new SolidColorBrush(Colors.DodgerBlue);
                lowerSeparator.Background = new SolidColorBrush(Colors.DodgerBlue);
                inputSign.Foreground = new SolidColorBrush(Colors.DodgerBlue);
            }
            else
            {
                upperSeparator.Background = new SolidColorBrush(Colors.Gray);
                lowerSeparator.Background = new SolidColorBrush(Colors.Gray);
                inputSign.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        public static TextWriter RuntimeInput { get; set; }

        public static TextWriter StandardOutput { get; set; }

        internal static List<Console> ActiveConsoles { get; set; }

        public static void LockConsoles()
        {
            ConsolesUnlocked = false;
            foreach (var item in ActiveConsoles)
                item.Dispatcher.Invoke(() =>
                {
                    item.inputTB.IsEnabled = false;
                    item.upperSeparator.Background = new SolidColorBrush(Colors.Gray);
                    item.lowerSeparator.Background = new SolidColorBrush(Colors.Gray);
                    item.inputSign.Foreground = new SolidColorBrush(Colors.White);
                });
        }

        public static void UnlockConsoles()
        {
            ConsolesUnlocked = true;
            foreach (var item in ActiveConsoles)
                item.Dispatcher.Invoke(() =>
                {
                    item.inputTB.IsEnabled = true;
                    item.upperSeparator.Background = new SolidColorBrush(Colors.DodgerBlue);
                    item.lowerSeparator.Background = new SolidColorBrush(Colors.DodgerBlue);
                    item.inputSign.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                });
        }

        public static void Write(object obj)
        {
            if (obj != null)
                StandardOutput.Write(obj.ToString());
        }

        public static void WriteLine(object obj)
        {
            if (obj != null)
                StandardOutput.WriteLine(obj.ToString());
        }

        public static void WriteLine() => StandardOutput.WriteLine();

        private static void ScrollConsolesDown()
        {
            foreach (var item in ActiveConsoles)
                item.Dispatcher.Invoke(() => item.consoleScroll.ScrollToVerticalOffset(double.MaxValue));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Window();
            dialog.Title = "Console";
            dialog.Background = App.DarkBackground;
            dialog.Width = 600;
            dialog.Height = 300;
            dialog.Owner = App.MainDialog;
            var content = new Console();
            dialog.Content = content;
            dialog.Closed += (sender, e) => ActiveConsoles.Remove(content);
            dialog.Show();
        }

        private void inputTB_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Return:
                    RuntimeInput.WriteLine(inputTB.Text);
                    RuntimeInput.Flush();
                    StandardOutput.WriteLine(inputTB.Text);
                    inputTB.Text = "";
                    break;
            }
        }

        public class ConsoleOutputStream : Stream
        {
            public override bool CanRead => false;

            public override bool CanSeek => false;

            public override bool CanWrite => true;

            public override long Length => throw new NotSupportedException();

            public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

            public override void Flush()
            {
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException();
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                var content = Encoding.UTF8.GetString(buffer, offset, count);
                foreach (var item in ActiveConsoles)
                    item.Dispatcher.Invoke(() => item.outputTB.Text += content);
                ScrollConsolesDown();
            }
        }
    }
}