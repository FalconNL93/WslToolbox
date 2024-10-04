using System.Diagnostics;
using System.Net.Http.Headers;
using Windows.ApplicationModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;
using Serilog;
using WslToolbox.UI.Activation;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Configurations;
using WslToolbox.UI.Core.Contracts.Services;
using WslToolbox.UI.Core.Helpers;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Core.Services;
using WslToolbox.UI.Core.Sinks;
using WslToolbox.UI.Extensions;
using WslToolbox.UI.Helpers;
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
    private readonly ILogger<App> _logger;
    public static bool HandleClosedEvents { get; set; }

    public readonly LoggerConfiguration LogConfiguration = new();

    public App()
    {
        try
        {
            Directory.CreateDirectory(Toolbox.AppDirectory);
            Toolbox.CopyOldConfiguration();
        }
        catch (Exception e)
        {
            throw;
        }


        InitializeComponent();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Toolbox.UserConfiguration, true, true)
            .AddJsonFile(Toolbox.LogConfiguration, true, true)
            .AddEnvironmentVariables()
            .Build();

        LogConfiguration = LogConfiguration
            .MinimumLevel.Information()
            .ReadFrom.Configuration(configuration)
            .WriteTo.Sink<EventSink>()
            .WriteTo.File(Toolbox.LogFile);

        Log.Logger = LogConfiguration.CreateLogger();

        Log.Logger.Debug("Logger initialized");
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(builder =>
            {
                builder.AddConfiguration(configuration);
            })
            .UseContentRoot(AppContext.BaseDirectory)
            .UseSerilog()
            .ConfigureServices((context, services) =>
            {
                services.AddAutoMapper(typeof(AutoMapperProfiles));

                // Default Activation Handler
                services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

                // Clients
                services.AddHttpClient<DownloadService>(c =>
                {
                    c.BaseAddress = Toolbox.GitHubDownloadUrl;
                    c.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue {NoCache = true};
                });
                services.AddHttpClient<UpdateService>(c =>
                {
                    c.BaseAddress = Toolbox.GitHubManifestFile;
                    c.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue {NoCache = true};
                });

                services.AddSingleton<WslConfigurationService>();

                // Services
                services.AddSingleton<AppNotificationService>();
                services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
                services.AddTransient<INavigationViewService, NavigationViewService>();
                services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
                services.AddSingleton<LogService>();

                services.AddSingleton<IActivationService, ActivationService>();
                services.AddSingleton<IPageService, PageService>();
                services.AddSingleton<INavigationService, NavigationService>();

                // Core Services
                services.AddSingleton<IDistributionDataService, DistributionDataService>();
                services.AddSingleton<IFileService, FileService>();
                services.AddSingleton<IConfigurationService, ConfigurationService>();
                services.AddSingleton<DistributionService>();

                // Views and ViewModels
                services.AddTransientPage<ShellViewModel, ShellPage>();
                services.AddTransientPage<DashboardViewModel, DashboardPage>();
                services.AddTransientPage<SettingsViewModel, SettingsPage>();
                services.AddTransientPage<NotificationViewModel, NotificationModal>();
                services.AddTransientPage<DeveloperViewModel, DeveloperPage>();
                services.AddTransientPage<LogViewModel, LogPage>();
                services.AddTransientPage<WslSettingsViewModel, WslSettingsPage>();

                services.AddSingleton<StartupDialogViewModel>();

                // Configuration
                services.Configure<UserOptions>(context.Configuration.GetSection(nameof(UserOptions)));
                services.Configure<NotificationOptions>(context.Configuration.GetSection(nameof(NotificationOptions)));
            }).Build();


        _logger = GetService<ILogger<App>>();

        GetService<AppNotificationService>().Initialize();

#if DEBUG
        UnhandledException += App_DebugUnhandledException;
#else
        UnhandledException += App_UnhandledException;
#endif
    }

    private IHost Host { get; }

    public static UserOptions GetUserOptions()
    {
        var optionsClass = GetService<IOptions<UserOptions>>();

        return optionsClass.Value;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static bool IsPackage()
    {
        try
        {
            return Package.Current.Id != null;
        }
        catch (Exception e)
        {
            return false;
        }
    }

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
            _logger.LogError(e.Exception, "An UI exception has occurred: {Message}", e.Message);
            ShowExceptionDialog(e);
        }
        catch (Exception)
        {
            throw new Exception($"Unexpected error {e.Message}");
        }
    }

#if DEBUG
    private void App_DebugUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        _logger.LogError(e.Exception, "An UI exception has occurred: {Message}", e.Message);
        ShowExceptionDialog(e);
    }
#endif

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        if (MainWindow is MainWindow mainWindow)
        {
            mainWindow.Closed += OnMainWindowClosed;
        }

        await GetService<IActivationService>().ActivateAsync(args);
    }

    private void OnMainWindowClosed(object sender, WindowEventArgs args)
    {
        if (!HandleClosedEvents)
        {
            _logger.LogInformation("Application exited");
            Environment.Exit(0);
        }

        args.Handled = true;
        MainWindow.Hide();

        _logger.LogInformation("Application minimized");
    }

    private void ShowExceptionDialog(UnhandledExceptionEventArgs e)
    {
        _logger.LogError(e.Exception, "An UI exception has occurred: {Message}", e.Message);
        var errorMessage = $"An unhandled exception has occurred and the application must close.{Environment.NewLine}{Environment.NewLine}Log file is written to:{Environment.NewLine}{Toolbox.LogFile}, open log file?";

        var messageboxResult = MessageBoxHelper.ShowError(errorMessage);
        if (messageboxResult == MessageBoxResult.Yes)
        {
            ShellHelper.OpenFile(Toolbox.LogFile);
        }
    }
}