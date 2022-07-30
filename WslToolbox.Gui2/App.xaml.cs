using System;
using System.IO;
using System.Reflection;
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

namespace WslToolbox.Gui2;

public partial class App
{
    private static readonly IHost Host = Microsoft.Extensions.Hosting.Host
        .CreateDefaultBuilder()
        .ConfigureAppConfiguration(c =>
        {
            c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location));
            c.AddJsonFile(SaveConfigurationExtension.FileName, true);
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

            services.AddSingleton<DistributionService>();

            services.AddScoped<INavigationWindow, Container>();
            services.AddScoped<ContainerViewModel>();

            services.AddScoped<Dashboard>();
            services.AddScoped<DashboardViewModel>();
            services.AddScoped<Settings>();
            services.AddScoped<SettingsViewModel>();

            services.AddAutoMapper(typeof(App));

            services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
        })
        .UseSerilog()
        .Build();

    public static T? GetService<T>()
        where T : class
    {
        return Host.Services.GetService(typeof(T)) as T;
    }

    private async void OnStartup(object sender, StartupEventArgs e)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.File($"{AppDomain.CurrentDomain.FriendlyName}.log")
            .CreateLogger();

        await Host.StartAsync();
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