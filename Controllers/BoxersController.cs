using FightClub.DTOs;
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

    [HttpGet("{id}")]
    public async Task<ActionResult<BoxerResponseDto>> GetById(Guid id) 
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("GET /api/boxers/{Id} called", id);
        }

        var result = await _boxerService.GetByIdAsync(id);
        
        return Ok(result);
    } 
    
    [HttpPost]
    public async Task<ActionResult<BoxerResponseDto>> Post([FromBody] BoxerCreateDto data)
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

        _logger.LogInformation("Boxer created with id {Id}", boxer.Id);

        return CreatedAtAction(nameof(GetById), new {id = boxer.Id}, boxer);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<BoxerResponseDto>> Patch(Guid id, [FromBody] BoxerUpdateDto data)
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


        var result = await _boxerService.UpdateAsync(id, data);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Deleting boxer with id {Id}", id);
        }
            
        await _boxerService.DeleteAsync(id);
        
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        string? weightCategory,
        int? minAge,
        int? maxAge)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("GET /api/boxers called with weight={Weight}, minAge={MinAge}, maxAge={MaxAge}", 
                weightCategory, minAge, maxAge);
        }

        var result = await _boxerService.GetFilteredAsync(weightCategory, minAge, maxAge);
        
        return Ok(result);
    }
}
