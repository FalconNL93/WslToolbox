using System;
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
    }
}