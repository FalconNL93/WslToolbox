using Microsoft.UI.Xaml.Controls;

namespace WslToolbox.UI.Extensions;

public static class TextBoxExtensions
{
    public static void WriteLine(this TextBox textBox, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        if (string.IsNullOrEmpty(textBox.Text))
        {
            textBox.Text = $"{text}{Environment.NewLine}";
            return;
        }

        textBox.Text += $"{text}{Environment.NewLine}";
    }
}