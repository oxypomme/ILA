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
        public Console()
        {
            InitializeComponent();
            {
                ActiveConsoles.Add(this);
                // Credits to https://stackoverflow.com/a/3308697
                CmdProcess = new Process();

                CmdProcess.StartInfo.FileName = "cmd.exe";

                // Set UseShellExecute to false for redirection.
                CmdProcess.StartInfo.UseShellExecute = false;

                // Redirect the standard output of the sort command. This stream is read
                // asynchronously using an event handler.
                CmdProcess.StartInfo.RedirectStandardOutput = true;

                // Set our event handler to asynchronously read the sort output.
                CmdProcess.OutputDataReceived += CmdOutputHandler;
                CmdProcess.ErrorDataReceived += CmdOutputHandler;

                // Redirect standard input as well. This stream is used synchronously.
                CmdProcess.StartInfo.RedirectStandardInput = true;
                CmdProcess.Start();

                // Use a stream writer to synchronously write the sort input.
                CmdInput = CmdProcess.StandardInput;

                // Start the asynchronous read of the cmd output stream.
                CmdProcess.BeginOutputReadLine();
            }
        }

        public static TextWriter StandardOutput { get; internal set; }

        public TextWriter CmdInput { get; private set; }

        internal static List<Console> ActiveConsoles { get; set; }

        private Process CmdProcess { get; set; }

        public void WriteInConsole(string message)
        {
            if (!string.IsNullOrEmpty(message))
                CmdInput.WriteLine(message);
        }

        private void CmdOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            // Collect the sort command output.
            if (!string.IsNullOrEmpty(outLine.Data))
            {
                // Add the text to the collected output.
                Dispatcher.Invoke(() =>
                {
                    StandardOutput.Write(Environment.NewLine + $"{outLine.Data}");
                    StandardOutput.Flush();
                    consoleScroll.ScrollToVerticalOffset(9999999);
                });
            }
        }

        private void inputTB_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Return:
                    WriteInConsole(inputTB.Text);
                    if (inputTB.Text == "cls")
                    {
                        outputTB.Text = "";
                    }
                    inputTB.Text = "";
                    break;
            }
        }

        public class ConsoleStream : Stream
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
                    item.outputTB.Text += content;
            }
        }
    }
}