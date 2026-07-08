using FightClub.Application.DTOs.Fights;
using FightClub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FightClub.Api.Controllers;

[ApiController]
[Route("api/fights")]
public class FightController : ControllerBase
{
    private readonly IFightService _service;
    private readonly IFightSimulationService _simulation;

    public FightController(
        IFightService service,
        IFightSimulationService simulation)
    {
        _service = service;
        _simulation = simulation;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] FightQueryDto query)
    {
        var result = await _service.GetPagedAsync(query);
        return Ok(result);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> Post([FromBody] FightCreateDto data)
    {
        var fight = await _service.CreateAsync(data);

        return CreatedAtAction(nameof(GetById), new { id = fight.Id }, fight);
    }

    [HttpPost("{id}/execute")]
    public async Task<IActionResult> Execute(Guid id)
    {
        var result = await _simulation.ExecuteAsync(id);

        return Ok(result);
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