using FightClub.DTOs.Trainers;
using FightClub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/trainers")]
public class TrainersController : ControllerBase
{
    private readonly ITrainerService _service;

    public TrainersController(ITrainerService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(TrainerCreateDto dto)
    { 
        return Ok(await _service.CreateAsync(dto)); 
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);

        return result is null 
            ? NotFound() 
            : Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] TrainerQueryDto query)
    { 
        return Ok(await _service.GetAsync(query)); 
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, TrainerUpdateDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);

        return result is null 
            ? NotFound() 
            : Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}