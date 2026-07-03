using FightClub.DTOs.Fights;
using FightClub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FightClub.Controllers;

[ApiController]
[Route("api/fights")]
public class FightController : ControllerBase
{
    private readonly ILogger<FightController> _logger;
    private readonly IFightService _service;

    public FightController(IFightService service, ILogger<FightController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] FightQueryDto query)
    {
        var result = await _service.GetPagedAsync(query);
        return Ok(result);
    }

    [HttpGet("boxer/{boxerId:guid}")]
    public async Task<IActionResult> GetByBoxer(Guid boxerId)
    {
        var result = await _service.GetByBoxerIdAsync(boxerId);

        return Ok(result);
    }
}