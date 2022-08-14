using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;
using WslToolbox.Gui2.Extensions;
using WslToolbox.Gui2.Models;
using WslToolbox.Gui2.Services;
using WslToolbox.Gui2.ViewModels;
using WslToolbox.Gui2.Views;
using WslToolbox.Gui2.Views.Pages;
using static System.Reflection.Assembly;

namespace WslToolbox.Gui2;

public partial class App
{
    public static readonly string? AppDirectory = Path.GetDirectoryName(GetEntryAssembly()?.Location);
    public static readonly string? AssemblyVersionFull = GetExecutingAssembly().GetName().Version?.ToString();

    private static readonly IHost Host = Microsoft.Extensions.Hosting.Host
        .CreateDefaultBuilder()
        .ConfigureAppConfiguration(c =>
        {
            c.SetBasePath(AppDirectory);
            c.AddJsonFile(SaveConfigurationExtension.FileName, true, true);
        })
        .ConfigureServices((context, services) =>
        {
            services.AddHostedService<ApplicationHostService>();

            services.AddSingleton<IThemeService, ThemeService>();
            services.AddSingleton<ITaskBarService, TaskBarService>();
            services.AddSingleton<ISnackbarService, SnackbarService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddScoped<INavigationWindow, Container>();
            services.AddScoped<ContainerViewModel>();

            services.AddSingleton<DistributionService>();

            services.AddPage<Dashboard, DashboardViewModel>();
            services.AddPage<Settings, SettingsViewModel>();
            services.AddPage<Information, InformationViewModel>();

            services.AddAutoMapper(typeof(App));

            services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
        })
        .UseSerilog()
        .Build();

    private async void OnStartup(object sender, StartupEventArgs e)
    {
        ConfigureLogger();
        await Host.StartAsync();
    }

    private static void ConfigureLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.File($"{AppDirectory}/logs/{AppDomain.CurrentDomain.FriendlyName}.log")
            .CreateLogger();
    }

    private async void OnExit(object sender, ExitEventArgs e)
    {
        await Host.StopAsync();

        Host.Dispose();
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
    }
}