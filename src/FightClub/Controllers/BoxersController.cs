using FightClub.DTOs.Boxers;
using FightClub.DTOs.Common;
using FightClub.Services.Interfaces;
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id) 
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("GET /api/boxers/{Id} called", id);
        }

        var result = await _boxerService.GetByIdAsync(id);
        
        return Ok(result);
    } 
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BoxerCreateDto data)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(
                "POST /api/boxers called with data: {FirstName} {LastName}, Age: {Age}, WeightCategory: {WeightCategory}",
                data.FirstName,
                data.LastName,
                data.Age,
                data.WeightCategory);
        }

        var boxer = await _boxerService.CreateAsync(data);

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Boxer created with id {Id}",
                boxer.Id);
        }

        return CreatedAtAction(nameof(GetById), new {id = boxer.Id}, boxer);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(Guid id, [FromBody] BoxerUpdateDto data)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(
                "PATCH /api/boxers/{Id} called with data: {FirstName} {LastName}, Age: {Age}, WeightCategory: {WeightCategory}",
                id,
                data.FirstName,
                data.LastName,
                data.Age,
                data.WeightCategory);
        }

        _ = await _boxerService.UpdateAsync(id, data);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Deleting boxer with id {id}", id);
        }
            
        await _boxerService.DeleteAsync(id);
        
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] BoxerQueryDto query)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("GET /api/boxers called with weight={WeightCategory}, minAge={MinAge}, maxAge={MaxAge}",
                query.WeightCategory, 
                query.MinAge, 
                query.MaxAge);
        }

        var result = await _boxerService.GetPagedAsync(query);
        
        return Ok(result);
    }
}
