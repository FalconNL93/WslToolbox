using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WslToolbox.Web.Models;

namespace WslToolbox.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var dists = await Core.Commands.Service.ListServiceCommand.ListDistributions(true);

        ViewData["Dists"] = dists;
        
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}