using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;
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
            services.AddScoped<Dashboard>();
            services.AddScoped<DashboardViewModel>();
        }).Build();

    public static T? GetService<T>()
        where T : class
    {
        return Host.Services.GetService(typeof(T)) as T;
    }

    /// <summary>
    ///     Occurs when the application is loading.
    /// </summary>
    private async void OnStartup(object sender, StartupEventArgs e)
    {
        await Host.StartAsync();
    }

    /// <summary>
    ///     Occurs when the application is closing.
    /// </summary>
    private async void OnExit(object sender, ExitEventArgs e)
    {
        await Host.StopAsync();

        Host.Dispose();
    }

    /// <summary>
    ///     Occurs when an exception is thrown by an application but not handled.
    /// </summary>
    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
    }
}