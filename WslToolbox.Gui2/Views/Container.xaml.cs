using System;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using WslToolbox.Gui2.ViewModels;
using WslToolbox.Gui2.Views.Pages;

namespace WslToolbox.Gui2.Views;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class Container : INavigationWindow
{
    private readonly ITaskBarService _taskBarService;
    private readonly IThemeService _themeService;

    public Container(
        ContainerViewModel viewModel,
        INavigationService navigationService,
        IPageService pageService,
        IThemeService themeService,
        ITaskBarService taskBarService,
        ISnackbarService snackbarService,
        IDialogService dialogService)
    {
        ViewModel = viewModel;
        DataContext = this;
        _themeService = themeService;
        _taskBarService = taskBarService;
        InitializeComponent();
        SetPageService(pageService);

        dialogService.SetDialogControl(FormDialog);
        snackbarService.SetSnackbarControl(MessageSnackbar);
        navigationService.SetNavigationControl(RootNavigation);

        Loaded += (_, _) => LoadDefaultPage();
    }

    public ContainerViewModel ViewModel { get; }

    public Frame GetFrame()
    {
        return RootFrame;
    }

    public INavigation GetNavigation()
    {
        return RootNavigation;
    }

    public bool Navigate(Type pageType)
    {
        return RootNavigation.Navigate(pageType);
    }

    public void SetPageService(IPageService pageService)
    {
        RootNavigation.PageService = pageService;
    }

    public void ShowWindow()
    {
        Show();
    }

    public void CloseWindow()
    {
        Close();
    }

    private async void LoadDefaultPage()
    {
        await Dispatcher.InvokeAsync(() => { Navigate(typeof(Dashboard)); });
    }

    private void RootNavigation_OnNavigated(INavigation sender, RoutedNavigationEventArgs e)
    {
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        Application.Current.Shutdown();
    }
}