using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;
using Windows.ApplicationModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;
using Serilog;
using WslToolbox.UI.Activation;
using WslToolbox.UI.Attributes;
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
    private ILogger<App> _logger;

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

    public App()
    {
        InitializeComponent();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Toolbox.UserConfiguration, optional: true, reloadOnChange: true)
            .AddJsonFile(Toolbox.LogConfiguration, optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .ReadFrom.Configuration(configuration)
            .WriteTo.File(Toolbox.LogFile)
            .CreateLogger();
        
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

                // Services
                services.AddSingleton<AppNotificationService>();
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

                services.AddSingleton<StartupDialogViewModel>();

                // Configuration
                services.Configure<UserOptions>(context.Configuration.GetSection(nameof(UserOptions)));
                services.Configure<NotificationOptions>(context.Configuration.GetSection(nameof(NotificationOptions)));
                services.Configure<AppCenterOptions>(c => c.IsAvailable = false);
            }).Build();


        _logger = GetService<ILogger<App>>();
        try
        {
            var appCenterInit = InitializeAppCenter();
            var appCenter = GetService<IOptions<AppCenterOptions>>();
            appCenter.Value.IsAvailable = appCenterInit is AppCenterStates.IsAvailable or AppCenterStates.IsEnabled;
            _logger.LogDebug("AppType: {AppType}", Toolbox.GetAppType());
            _logger.LogDebug("AppCenterState: {AppCenter}", appCenterInit);
        }
        catch (Exception e)
        {
            _logger.LogDebug(e, "Failed to initialize App Center");
        }
        
        GetService<AppNotificationService>().Initialize();

        UnhandledException += App_UnhandledException;
    }

    private IHost Host { get; }

    public static WindowEx MainWindow { get; } = new MainWindow();

    private AppCenterStates InitializeAppCenter()
    {
        try
        {
            var secret = Environment.GetEnvironmentVariable("APPCENTER_KEY");
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null && entryAssembly.GetCustomAttribute<AppCenterAttribute>() != null)
            {
                var secretKey = entryAssembly.GetCustomAttribute<AppCenterAttribute>()?.AppCenterKey;
                if (!string.IsNullOrEmpty(secretKey))
                {
                    secret = secretKey;
                }
            }

            if (secret == null)
            {
                return AppCenterStates.IsUnavailable;
            }

            var runConfiguration = GetService<IOptions<UserOptions>>().Value;
            if (!runConfiguration.Analytics)
            {
                return AppCenterStates.IsAvailable;
            }

            AppCenter.Start(secret,
                typeof(Analytics),
                typeof(Crashes)
            );

            return AppCenterStates.IsEnabled;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to initialize AppCenter");
            return AppCenterStates.IsUnavailable;
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
        }
        catch (Exception)
        {
            throw new Exception($"Unexpected error {e.Message}");
        }

#if DEBUG
        throw new Exception($"Unexpected error {e.Message}");
#endif
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        await GetService<IActivationService>().ActivateAsync(args);
    }
}