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
        .ConfigureServices((_, service) =>
        {
            service.AddHostedService<ApplicationHostService>();

            service.AddSingleton<IThemeService, ThemeService>();
            service.AddSingleton<ITaskBarService, TaskBarService>();
            service.AddSingleton<ISnackbarService, SnackbarService>();
            service.AddSingleton<IDialogService, DialogService>();
            service.AddSingleton<IPageService, PageService>();
            service.AddSingleton<INavigationService, NavigationService>();

            service.AddScoped<INavigationWindow, Container>();
            service.AddScoped<ContainerViewModel>();
            service.AddScoped<Dashboard>();
            service.AddScoped<DashboardViewModel>();
        }).Build();

    public static T? GetService<T>()
        where T : class
    {
        return Host.Services.GetService(typeof(T)) as T;
    }

    private async void OnStartup(object sender, StartupEventArgs e)
    {
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