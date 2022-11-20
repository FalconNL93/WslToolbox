using Serilog;
using Serilog.Events;

namespace WslToolbox.Web;

public class WebAdmin
{
    public WebApplication Server()
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddControllersWithViews();

        var app = builder.Build();
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        return app;
    }
}