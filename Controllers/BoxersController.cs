using FightClub.Models;
using FightClub.Services;
using Microsoft.AspNetCore.Mvc;

namespace FightClub.Controllers;

[ApiController]
[Route("api/boxers")]
public class BoxersController : ControllerBase
{
    private readonly ILogger<BoxersController> _logger;

    private readonly IBoxerService _boxerService;

    public BoxersController(ILogger<BoxersController> logger, IBoxerService service)
    {
        _logger = logger;
        _boxerService = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Boxer>>> Get()
    {
        return Ok(await _boxerService.GetAllBoxers());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Boxer>> GetById(Guid id) 
    {
        var boxer = await _boxerService.GetBoxerById(id);
        return boxer is null ? NotFound() : Ok(boxer);
    } 
    
    [HttpPost]
    public async Task<ActionResult<Boxer>> Post([FromBody] BoxerCreateDto data)
    {
        var boxer = await _boxerService.CreateBoxer(data);

        return CreatedAtAction(nameof(GetById), new {id = boxer.Id}, boxer);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<Boxer>> Patch(Guid id, BoxerUpdateDto data)
    {
        var boxer = await _boxerService.UpdateBoxer(id, data);

        return boxer is null ? NotFound() : Ok(boxer);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Boxer>> Delete(Guid id)
    {
        await _boxerService.DeleteBoxer(id);
        return Ok();
    }
}
