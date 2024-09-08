using Markdig;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views.Modals;

public sealed partial class WhatsNewDialog : ContentDialog
{
    public WhatsNewDialog(WhatsNewDialogViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }

    public WhatsNewDialogViewModel ViewModel { get; }

    /// <summary>
    /// Temporary method for testing
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void WhatsNewDialog_OnLoaded(object sender, RoutedEventArgs e)
    {
        var viewer = WhatsNewViewer;
        var htmlPage = "<style>:root {color-scheme: light dark;}</style>";
        htmlPage += Markdown.ToHtml("This is a text with some *emphasis*");
        await viewer.EnsureCoreWebView2Async();
        var core = viewer.CoreWebView2;
        viewer.NavigateToString(htmlPage);
    }
}