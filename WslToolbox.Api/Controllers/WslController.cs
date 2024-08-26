using Microsoft.AspNetCore.Mvc;
using WslToolbox.Core.Legacy.Commands.Distribution;
using WslToolbox.Core.Legacy.Commands.Service;

namespace WslToolbox.Api.Controllers;

[Route("/")]
[ApiController]
public class WslController : ControllerBase
{
    public async Task<IActionResult> Get()
    {
        return new JsonResult(await ListServiceCommand.ListDistributions());
    }

    [Route("/restart/{DistroId}")]
    public async Task<IActionResult> Restart(string distroId)
    {
        var list = await ListServiceCommand.ListDistributions();
        var distro = list.FirstOrDefault(x => x.Guid == $"{{{distroId}}}");

        if (distro == null)
        {
            return NotFound();
        }

        try
        {
            var stopCommand = await TerminateDistributionCommand.Execute(distro);
            var startCommand = await StartDistributionCommand.Execute(distro);
        }
        catch (Exception e)
        {
            return BadRequest();
        }

        return Ok();
    }
}