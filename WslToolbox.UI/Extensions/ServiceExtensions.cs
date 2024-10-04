using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace WslToolbox.UI.Extensions;

public static class ServiceExtensions
{
    public static void AddTransientPage<T, T2>(this IServiceCollection service) where T : ObservableRecipient where T2 : Page
    {
        service.AddTransient<T>();
        service.AddTransient<T2>();
    }

    public static void AddSingletonPage<T, T2>(this IServiceCollection service) where T : ObservableRecipient where T2 : Page
    {
        service.AddSingleton<T>();
        service.AddSingleton<T2>();
    }
}