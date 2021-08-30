using System;
using System.IO;
using System.Windows;

namespace WslToolbox.Views
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

        public void WriteOutput(string OutputContent, bool NewLine = true)
        {
            OutputBlock.Text = OutputBlock.Text
                + OutputContent
                + (NewLine
                    ? Environment.NewLine
                    : String.Empty);
        }

        public new void Show()
        {
            base.Show();
            _ = Focus();
            WindowState = WindowState.Normal;
        }

        private void SaveLog_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ClearLog_Click(object sender, RoutedEventArgs e)
        {
            OutputBlock.Text = string.Empty;
        }

        private void SaveLog_Click_1(object sender, RoutedEventArgs e)
        {
            string dateTime = DateTime.Now.ToString("yyy-MM-d");

            try
            {
                if (!Directory.Exists("logs"))
                {
                    _ = Directory.CreateDirectory("logs");
                    WriteOutput("Logs directory created.");
                }

                File.AppendAllText(
                    $"logs/output-{dateTime}.txt",
                    $"[{dateTime} {DateTime.Now:HH:mm:ss}]{Environment.NewLine}{OutputBlock.Text}{Environment.NewLine}"
                );
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message);
            }
        }
    }
}