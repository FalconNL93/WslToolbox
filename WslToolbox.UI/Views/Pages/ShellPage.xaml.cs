using Windows.System;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Helpers;
using WslToolbox.UI.Messengers;
using WslToolbox.UI.ViewModels;
using WslToolbox.UI.Views.Modals;

namespace WslToolbox.UI.Views.Pages;

public sealed partial class ShellPage : Page
{
    public ShellPage(ShellViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationViewService.Initialize(NavigationViewControl);

        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        App.MainWindow.Activated += MainWindow_Activated;
        AppTitleBarText.Text = App.Name;

        // WeakReferenceMessenger.Default.Register<InputDialogMessage>(this, OnShowInputDialog);
        // WeakReferenceMessenger.Default.Register<SimpleDialogShowMessage>(this, OnShowSimpleDialog);
        // WeakReferenceMessenger.Default.Register<UpdateDialogMessage>(this, OnShowUpdateDialog);
    }

    public ShellViewModel ViewModel { get; }
    public bool IsDeveloper { get; } = App.IsDeveloper;

    private void OnShowInputDialog(object recipient, InputDialogMessage message)
    {
        var contentDialog = new InputDialog(message.ViewModel)
        {
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            XamlRoot = XamlRoot
        };

        message.Reply(contentDialog);
    }

    private void OnShowUpdateDialog(object recipient, UpdateDialogMessage message)
    {
        var contentDialog = new UpdateDialog(message.ViewModel)
        {
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            XamlRoot = XamlRoot
        };

        message.Reply(contentDialog.ShowAsync().AsTask());
    }


    private void OnShowSimpleDialog(object recipient, SimpleDialogShowMessage message)
    {
        var contentDialog = new SimpleDialog(message.ViewModel)
        {
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            XamlRoot = XamlRoot
        };

        message.Reply(contentDialog.ShowAsync().AsTask());
    }


    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(RequestedTheme);

        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));
    }

    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        var resource = args.WindowActivationState == WindowActivationState.Deactivated ? "WindowCaptionForegroundDisabled" : "WindowCaptionForeground";

        AppTitleBarText.Foreground = (SolidColorBrush) Application.Current.Resources[resource];
    }

    private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        AppTitleBar.Margin = new Thickness
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = AppTitleBar.Margin.Top,
            Right = AppTitleBar.Margin.Right,
            Bottom = AppTitleBar.Margin.Bottom
        };
    }

    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator {Key = key};

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }
}