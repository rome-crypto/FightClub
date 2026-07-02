using FightClub.DTOs.Fights;
using FightClub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
    public async Task<ActionResult<List<FightResponseDto>>> Get([FromQuery] FightQueryDto query)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("GET /api/fights called with from={From}, to={To}, boxerId={BoxerId}, winnerId={WinnerId}",
                query.From, query.To, query.BoxerId, query.WinnerId);
        }

        var result = await _service.GetAsync(query);
        return Ok(result);
    }

    [HttpGet("boxer/{boxerId:guid}")]
    public async Task<ActionResult<List<FightResponseDto>>> GetByBoxer(Guid boxerId)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("GET /api/fights/boxers/{Id} called", boxerId);
        }

        var result = await _service.GetByIdAsync(boxerId);
        return Ok(result);
    }
}
