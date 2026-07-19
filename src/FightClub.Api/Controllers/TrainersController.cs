using FightClub.Application.DTOs.Trainers;
using FightClub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FightClub.Api.Controllers;

/// <summary>
/// CRUD контроллер для сущности тренера
/// </summary>
/// <remarks>
/// Внедрение зависимостей
/// </remarks>
/// <param name="service">Реализация сервиса для CRUD тренера</param>
[ApiController]
[Route("api/trainers")]
public class TrainersController(ITrainerService service) : ControllerBase
{
    private readonly ITrainerService _service = service;

    /// <summary>
    /// Создание нового объекта
    /// </summary>
    /// <param name="dto">Данные для создания</param>
    /// <returns>HTTP 201 и результат</returns>
    [HttpPost]
    public async Task<IActionResult> Post(TrainerCreateDto dto)
    {
        TrainerResponseDto result = await _service.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Получение по ID
    /// </summary>
    /// <remarks>Гарантируется уникальность ID</remarks>
    /// <param name="id">ID тренера</param>
    /// <returns>HTTP 200 и результат</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>
    /// Получение по запросу + пагинация
    /// </summary>
    /// <param name="query">Запрос</param>
    /// <returns>HTTP 200 и страницы результата</returns>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] TrainerQueryDto query)
    {
        return Ok(await _service.GetPagedAsync(query));
    }

    /// <summary>
    /// Обновление по ID
    /// </summary>
    /// <param name="id">ID искомого</param>
    /// <param name="dto">Данные для изменения</param>
    /// <returns>HTTP 204</returns>
    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(Guid id, TrainerUpdateDto dto)
    {
        await _service.UpdateAsync(id, dto);

        return NoContent();
    }

    /// <summary>
    /// Удаление по ID
    /// </summary>
    /// <param name="id">ID для удаления</param>
    /// <returns>HTTP 204</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}
