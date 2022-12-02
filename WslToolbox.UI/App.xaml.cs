using System.Diagnostics;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Serilog;
using Serilog.Events;
using Windows.ApplicationModel;
using WslToolbox.UI.Activation;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Configurations;
using WslToolbox.UI.Core.Contracts.Services;
using WslToolbox.UI.Core.Helpers;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Core.Services;
using WslToolbox.UI.Extensions;
using WslToolbox.UI.Services;
using WslToolbox.UI.ViewModels;
using WslToolbox.UI.Views.Modals;
using WslToolbox.UI.Views.Pages;
using UnhandledExceptionEventArgs = Microsoft.UI.Xaml.UnhandledExceptionEventArgs;

namespace WslToolbox.UI;

public partial class App : Application
{
    public const string Name = "WSL Toolbox";
    public static readonly bool IsDeveloper = Debugger.IsAttached;
   
    public static bool IsPackage()
    {
        try
        {
            return Package.Current.Id != null;
        }
        catch
        {
            return false;
        }
    }

    public App()
    {
        InitializeComponent();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(Toolbox.LogFile, LogEventLevel.Debug)
            .CreateLogger();

        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(c =>
            {
                c.AddJsonFile(Toolbox.UserConfiguration, true);
            })
            .UseContentRoot(AppContext.BaseDirectory)
            .UseSerilog()
            .ConfigureServices((context, services) =>
            {
                services.AddAutoMapper(typeof(AutoMapperProfiles));

                // Default Activation Handler
                services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

                // Other Activation Handlers
                services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();

                // Clients
                services.AddHttpClient<UpdateService>(c =>
                {
                    c.BaseAddress = new Uri("https://raw.githubusercontent.com/FalconNL93/manifests/main/");
                });

                // Services
                services.AddSingleton<IAppNotificationService, AppNotificationService>();
                services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
                services.AddTransient<INavigationViewService, NavigationViewService>();
                services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

                services.AddSingleton<IActivationService, ActivationService>();
                services.AddSingleton<IPageService, PageService>();
                services.AddSingleton<INavigationService, NavigationService>();

                // Core Services
                services.AddSingleton<IDistributionDataService, DistributionDataService>();
                services.AddSingleton<IFileService, FileService>();
                services.AddSingleton<IConfigurationService, ConfigurationService>();
                services.AddSingleton<DistributionService>();

                // Views and ViewModels
                services.AddPage<ShellViewModel, ShellPage>();
                services.AddPage<DashboardViewModel, DashboardPage>();
                services.AddPage<SettingsViewModel, SettingsPage>();
                services.AddPage<NotificationViewModel, NotificationModal>();
                services.AddPage<DeveloperViewModel, DeveloperPage>();

                // Configuration
                services.Configure<UserOptions>(context.Configuration.GetSection(nameof(UserOptions)));
            }).Build();

        GetService<IAppNotificationService>().Initialize();

        UnhandledException += App_UnhandledException;
    }

    private IHost Host { get; }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static T GetService<T>()
        where T : class
    {
        if ((Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        try
        {
            var logger = GetService<ILogger<App>>();
            logger.LogError(e.Exception, "An UI exception has occurred: {Message}", e.Message);
        }
        catch (Exception)
        {
            throw new Exception($"Unexpected error {e.Message}");
        }

        throw new Exception($"Unexpected error {e.Message}");
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        await GetService<IActivationService>().ActivateAsync(args);
    }
}