using FightClub.Application.DTOs.Trainers;
using FightClub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FightClub.Api.Controllers;

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
    public async Task<IActionResult> Post(TrainerCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] TrainerQueryDto query)
    {
        return Ok(await _service.GetPagedAsync(query));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(Guid id, TrainerUpdateDto dto)
    {
        _ = await _service.UpdateAsync(id, dto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}