using FightClub.Application.DTOs.Boxers;
using FightClub.Application.DTOs.Common;
using FightClub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FightClub.Api.Controllers;


/// <summary>
/// CRUD-контроллер для boxer
/// </summary>
/// <remarks>
/// Внедрение зависимостей
/// </remarks>
/// <param name="service">Реализация сервиса для CRUD боксера</param>
[ApiController]
[Route("api/boxers")]
public class BoxersController(IBoxerService service) : ControllerBase
{
    private readonly IBoxerService _boxerService = service;

    /// <summary>
    /// Поиск по ID
    /// </summary>
    /// <remarks>Гарантируется уникальность ID</remarks>
    /// <param name="id">ID боксера</param>
    /// <returns>HTTP 200 и результат поиска</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        BoxerResponseDto result = await _boxerService.GetByIdAsync(id);

        return Ok(result);
    }

    /// <summary>
    /// Создание нового объекта
    /// </summary>
    /// <param name="data">Данные для создания</param>
    /// <returns>HTTP 201 и новый объект</returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BoxerCreateDto data)
    {
        BoxerResponseDto boxer = await _boxerService.CreateAsync(data);

        return CreatedAtAction(nameof(GetById), new { id = boxer.Id }, boxer);
    }

    /// <summary>
    /// Обновление боксера
    /// </summary>
    /// <param name="id">ID боксера</param>
    /// <param name="data">Данные для изменения</param>
    /// <returns>HTTP 204</returns>
    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(Guid id, [FromBody] BoxerUpdateDto data)
    {
        await _boxerService.UpdateAsync(id, data);

        return NoContent();
    }

    /// <summary>
    /// Удаление по ID
    /// </summary>
    /// <param name="id">ID боксера</param>
    /// <returns>HTTP 204</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _boxerService.DeleteAsync(id);

        return NoContent();
    }

    /// <summary>
    /// Получение по запросу с пагинацией
    /// </summary>
    /// <param name="query"></param>
    /// <returns>HTTP 200 и страницы результата</returns>
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] BoxerQueryDto query)
    {
        PagedResult<BoxerResponseDto> result = await _boxerService.GetPagedAsync(query);

        return Ok(result);
    }
}
