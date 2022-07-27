using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using Wpf.Ui.Mvvm.Contracts;
using WslToolbox.Core;
using WslToolbox.Gui2.Models;
using WslToolbox.Gui2.Views;

namespace WslToolbox.Gui2.Services;

/// <summary>
///     Managed host of the application.
/// </summary>
public class ApplicationHostService : IHostedService
{
    private readonly INavigationService _navigationService;
    private readonly IPageService _pageService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITaskBarService _taskBarService;
    private readonly IMapper _mapper;
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

        var test = new DistributionClass
        {
            IsDefault = false,
            IsInstalled = false,
            Name = null,
            State = null,
            Version = 0,
            Guid = null,
            BasePath = null,
            BasePathLocal = null,
            DefaultUid = 0,
            Size = 0
        };
        
        var distribution = _mapper.Map<DistributionModel>(test);

        var bb = distribution;

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