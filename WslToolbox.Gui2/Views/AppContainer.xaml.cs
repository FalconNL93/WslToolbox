using System.Windows;
using WPFUI.Appearance;

namespace WslToolbox.Gui2.Views;

public partial class AppContainer : Window
{
    public AppContainer()
    {
        InitializeComponent();
        
        Loaded += (_, _) =>
        {
            Watcher.Watch(this);
        };
    }
}