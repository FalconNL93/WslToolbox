using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using Wpf.Ui.Mvvm.Contracts;
using WslToolbox.Gui2.Views;

namespace WslToolbox.Gui2.Services;

/// <summary>
///     Managed host of the application.
/// </summary>
public class ApplicationHostService : IHostedService
{
    private readonly IMapper _mapper;
    private readonly INavigationService _navigationService;
    private readonly IPageService _pageService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITaskBarService _taskBarService;
    private readonly IThemeService _themeService;
    private INavigationWindow? _navigationWindow;

    public ApplicationHostService(
        IServiceProvider serviceProvider,
        INavigationService navigationService,
        IPageService pageService,
        IThemeService themeService,
        ITaskBarService taskBarService,
        IMapper mapper
    )
    {
        _serviceProvider = serviceProvider;
        _navigationService = navigationService;
        _pageService = pageService;
        _themeService = themeService;
        _taskBarService = taskBarService;
        _mapper = mapper;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        PrepareNavigation();

        await HandleActivationAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    private async Task HandleActivationAsync()
    {
        await Task.CompletedTask;

        if (!Application.Current.Windows.OfType<Container>().Any())
        {
            _navigationWindow = _serviceProvider.GetService(typeof(INavigationWindow)) as INavigationWindow;
            _navigationWindow!.ShowWindow();
        }

        await Task.CompletedTask;
    }

    private void PrepareNavigation()
    {
        _navigationService.SetPageService(_pageService);
    }
}