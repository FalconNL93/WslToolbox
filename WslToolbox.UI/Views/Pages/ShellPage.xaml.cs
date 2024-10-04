using Windows.System;
using CommunityToolkit.Mvvm.Messaging;
using H.NotifyIcon;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using WslToolbox.Core.Legacy.Commands.Service;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Helpers;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Helpers;
using WslToolbox.UI.Messengers;
using WslToolbox.UI.ViewModels;
using WslToolbox.UI.Views.Modals;

namespace WslToolbox.UI.Views.Pages;

public sealed partial class ShellPage : Page
{
    private readonly IMessenger _messenger;
    public TaskbarIcon? TaskbarIcon { get; set; }


    public ShellPage(ShellViewModel viewModel, IMessenger messenger)
    {
        _messenger = messenger;
        ViewModel = viewModel;
        InitializeComponent();

        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationViewService.Initialize(NavigationViewControl);

        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        App.MainWindow.Activated += MainWindow_Activated;
        AppTitleBarText.Text = $"{App.Name} - {Toolbox.Version}";

        WeakReferenceMessenger.Default.Register<InputDialogMessage>(this, OnShowInputDialog);
        WeakReferenceMessenger.Default.Register<SimpleDialogShowMessage>(this, OnShowSimpleDialog);
        WeakReferenceMessenger.Default.Register<UpdateDialogMessage>(this, OnShowUpdateDialog);
        WeakReferenceMessenger.Default.Register<StartupDialogShowMessage>(this, OnShowStartupDialog);
        WeakReferenceMessenger.Default.Register<ImportDialogMessage>(this, OnShowImportDialog);
        WeakReferenceMessenger.Default.Register<MoveDialogMessage>(this, OnShowMoveDialog);
        WeakReferenceMessenger.Default.Register<UserOptionsChanged>(this, OnUserOptionsChanged);
        WeakReferenceMessenger.Default.Register<HideTrayIcon>(this, OnHideTrayIcon);

        RegisterTrayCommands();
    }

    private void OnHideTrayIcon(object recipient, HideTrayIcon message)
    {
        App.HandleClosedEvents = false;
        App.MainWindow.DispatcherQueue.TryEnqueue(DestroyTrayIcon);
    }

    private void InitializeTrayIcon()
    {
        if (TaskbarIcon != null)
        {
            return;
        }

        TaskbarIcon = (TaskbarIcon) Application.Current.Resources["TrayIcon"];
        TaskbarIcon.IconSource = new BitmapImage(new Uri(Path.Combine(AppContext.BaseDirectory, "Assets/app.ico")));
        TaskbarIcon.ToolTipText = $"{App.Name}";

        TaskbarIcon.ForceCreate();
        App.HandleClosedEvents = true;
    }

    private void RegisterTrayCommands()
    {
        var showWindowCommand = (XamlUICommand) Application.Current.Resources["ShowWindowCommand"];
        var exitApplicationCommand = (XamlUICommand) Application.Current.Resources["ExitApplicationCommand"];
        var restartWslServiceCommand = (XamlUICommand) Application.Current.Resources["RestartWslServiceCommand"];

        showWindowCommand.ExecuteRequested += ShowHideWindowCommandOnExecuteRequested;
        exitApplicationCommand.ExecuteRequested += ExitApplicationCommandOnExecuteRequested;
        restartWslServiceCommand.ExecuteRequested += RestartWslServiceCommandOnExecuteRequested;
    }

    private async void RestartWslServiceCommandOnExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
    {
        await StopServiceCommand.Execute();
        await StartServiceCommand.Execute();
    }

    private static void ExitApplicationCommandOnExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
    {
        App.HandleClosedEvents = false;
        Application.Current.Exit();
    }

    private static void ShowHideWindowCommandOnExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
    {
        App.MainWindow.Activate();
    }

    private void DestroyTrayIcon()
    {
        App.HandleClosedEvents = false;
        TaskbarIcon?.Dispose();
    }

    private void ApplyUserConfiguration(UserOptions userOptions)
    {
        if (userOptions.UseSystemTray)
        {
            App.MainWindow.DispatcherQueue.TryEnqueue(InitializeTrayIcon);
        }
        else
        {
            App.MainWindow.DispatcherQueue.TryEnqueue(DestroyTrayIcon);
        }
    }

    private void OnUserOptionsChanged(object recipient, UserOptionsChanged options)
    {
        ApplyUserConfiguration(options.UserOptions);
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

        message.Reply(contentDialog.ShowInput());
    }

    private void OnShowImportDialog(object recipient, ImportDialogMessage message)
    {
        var contentDialog = new ImportDistribution(message.ViewViewModel)
        {
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            XamlRoot = XamlRoot
        };

        message.Reply(contentDialog.ShowInput());
    }

    private void OnShowMoveDialog(object recipient, MoveDialogMessage message)
    {
        var contentDialog = new MoveDialog(message.ViewViewModel)
        {
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            XamlRoot = XamlRoot
        };

        message.Reply(contentDialog.ShowInput());
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

    private void OnShowStartupDialog(object recipient, StartupDialogShowMessage message)
    {
        var contentDialog = new StartupDialog(message.ViewModel)
        {
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            XamlRoot = XamlRoot
        };

        message.Reply(contentDialog.ShowAsync().AsTask());
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));

        var options = _messenger.RequestUserOptions();
        ApplyUserConfiguration(options.Response);
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