using Microsoft.Extensions.DependencyInjection;

namespace WslToolbox.Gui2.Extensions;

public static class PageServiceExtension
{
    public static void AddPage<T1, T2>(this IServiceCollection services)
        where T1 : class where T2 : class
    {
        services.AddScoped<T1>();
        services.AddScoped<T2>();
    }
}