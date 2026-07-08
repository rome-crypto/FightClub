using FightClub.DTOs.Fights;
using FightClub.Services;
using FightClub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FightClub.Controllers;

[ApiController]
[Route("api/fights")]
public class FightController : ControllerBase
{
    private readonly IFightService _service;

    public FightController(
        IFightService service)
    {
        _service = service;
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
    
    [HttpPost("")]
    public async Task<IActionResult> Post([FromBody] FightCreateDto data)
    {
        var fight = await _service.CreateAndExecuteAsync(data);

        return CreatedAtAction(nameof(GetById), new { id = fight.Id }, fight);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id) 
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}