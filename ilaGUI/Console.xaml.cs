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
                // Credits to https://stackoverflow.com/a/3308697
                CmdProcess = new Process();

                CmdProcess.StartInfo.FileName = "cmd.exe";

                // Set UseShellExecute to false for redirection.
                CmdProcess.StartInfo.UseShellExecute = false;

                // Redirect the standard output of the sort command. This stream is read
                // asynchronously using an event handler.
                CmdProcess.StartInfo.RedirectStandardOutput = true;

                CmdOutput = new StringBuilder("");

                // Set our event handler to asynchronously read the sort output.
                CmdProcess.OutputDataReceived += CmdOutputHandler;

                // Redirect standard input as well. This stream is used synchronously.
                CmdProcess.StartInfo.RedirectStandardInput = true;
                CmdProcess.Start();

                // Use a stream writer to synchronously write the sort input.
                CmdInput = CmdProcess.StandardInput;

                // Start the asynchronous read of the cmd output stream.
                CmdProcess.BeginOutputReadLine();

                outputTB.Text = CmdOutput.ToString();
            }
        }

        private StreamWriter CmdInput { get; set; }
        private StringBuilder CmdOutput { get; set; }
        private Process CmdProcess { get; set; }

        private void CmdOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            // Collect the sort command output.
            if (!string.IsNullOrEmpty(outLine.Data))
            {
                // Add the text to the collected output.
                CmdOutput.Append(Environment.NewLine + $"{outLine.Data}");
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => outputTB.Text = CmdOutput.ToString()));
            }
        }

        private void inputTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key is Key.Return)
            {
                WriteInConsole(inputTB.Text);
                inputTB.Text = "";
                if (inputTB.Text == "cls")
                {
                    CmdOutput = new StringBuilder("");
                    outputTB.Text = "";
                }
                consoleScroll.ScrollToVerticalOffset(consoleScroll.ScrollableHeight);
            }
        }

        public void WriteInConsole(string message)
        {
            if (!string.IsNullOrEmpty(message))
                CmdInput.WriteLine(message);
        }
    }
}