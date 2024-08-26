using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WslToolbox.Api;

public class Api
{
    public static void Main(string[]? args)
    {
        args ??= [];

        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.UseUrls("https://*:3250");

        builder.Host.UseWindowsService();


        builder.Services.AddControllers()
            .AddApplicationPart(Assembly.Load(AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly().Location)));

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.MapControllers();
        app.Run();
    }
}