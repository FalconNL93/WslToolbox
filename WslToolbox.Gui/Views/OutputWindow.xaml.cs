using System;
using System.IO;
using System.Windows;

namespace WslToolbox.Gui.Views
{
    /// <summary>
    /// Interaction logic for OutputWindow.xaml
    /// </summary>
    public partial class OutputWindow : Window
    {
        public OutputWindow()
        {
            InitializeComponent();
        }

        public new void Show()
        {
            base.Show();
            _ = Focus();
            WindowState = WindowState.Normal;
        }

        public void WriteOutput(string OutputContent, bool NewLine = true)
        {
            string dateTime = DateTime.Now.ToString("yyy-MM-dd HH:mm:ss");
            OutputBlock.Text = OutputBlock.Text
                + $"[{dateTime}] "
                + OutputContent
                + (NewLine
                    ? Environment.NewLine
                    : string.Empty);
        }

        private void ClearLog_Click(object sender, RoutedEventArgs e)
        {
            OutputBlock.Text = string.Empty;
        }

        private void SaveLog_Click(object sender, RoutedEventArgs e)
        {
            string dateTime = DateTime.Now.ToString("yyy-MM-dd");

            try
            {
                if (!Directory.Exists("logs"))
                {
                    _ = Directory.CreateDirectory("logs");
                    WriteOutput("Logs directory created.");
                }

                File.AppendAllText(
                    $"logs/output-{dateTime}.txt",
                    OutputBlock.Text + Environment.NewLine
                );
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message);
            }
        }
    }
}