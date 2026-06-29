using FightClub.DTOs.Fights;
using FightClub.Services;
using Microsoft.AspNetCore.Mvc;

namespace FightClub.Controllers;

[ApiController]
[Route("api/fights")]
public class FightController : Controller
{
    private readonly FightService _service;

    public FightController(FightService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<FightResponseDto>>> Get([FromQuery] FightQueryDto query)
    {
        var result = await _service.GetAsync(query);
        return Ok(result);
    }

    [HttpGet("boxer/{boxerId:guid}")]
    public async Task<ActionResult<List<FightResponseDto>>> GetByBoxer(Guid boxerId)
    {
        var result = await _service.GetByBoxerAsync(boxerId);
        return Ok(result);
    }
}
