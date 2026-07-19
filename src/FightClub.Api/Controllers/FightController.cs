using FightClub.Application.DTOs.Common;
using FightClub.Application.DTOs.Fights;
using FightClub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FightClub.Api.Controllers;


/// <summary>
/// CRUD-контроллер для fight + запуск боя
/// </summary>
/// <remarks>
/// Внедрение зависимостей
/// </remarks>
/// <param name="service">Реализация сервиса для CRUD боя</param>
/// <param name="simulation">Реализация сервиса для бизнес-процесса боя</param>
[ApiController]
[Route("api/fights")]
public class FightController(
    IFightService service,
    IFightSimulationService simulation) : ControllerBase
{
    private readonly IFightService _service = service;
    private readonly IFightSimulationService _simulation = simulation;


    /// <summary>
    /// Получение данных по запросу с пагинацией
    /// </summary>
    /// <param name="query">Запрос</param>
    /// <returns>HTTP 200 и страницы результата</returns>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] FightQueryDto query)
    {
        PagedResult<FightResponseDto> result = await _service.GetPagedAsync(query);

        return Ok(result);
    }
    

    /// <summary>
    /// Создание нового объекта 
    /// </summary>
    /// <param name="data">Данные для создания</param>
    /// <returns>HTTP 201 и созданный объект</returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] FightCreateDto data)
    {
        FightResponseDto fight = await _service.CreateAsync(data);

        return CreatedAtAction(nameof(GetById), new { id = fight.Id }, fight);
    }


    /// <summary>
    /// Запуск боя
    /// </summary>
    /// <param name="id">ID боя</param>
    /// <returns>HTTP 204</returns>
    [HttpPost("{id}/execute")]
    public async Task<IActionResult> Execute(Guid id)
    {
        await _simulation.ExecuteAsync(id);

        return NoContent();
    }


    /// <summary>
    /// Получение боя по ID
    /// </summary>
    /// <remarks>Гарантируется уникальность ID</remarks>
    /// <param name="id">ID боя</param>
    /// <returns>HTTP 200</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }


    /// <summary>
    /// Отмена боя
    /// </summary>
    /// <param name="id">ID боя</param>
    /// <returns>HTTP 204</returns>
    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelFight(Guid id)
    {
        await _simulation.CancelAsync(id);

        return NoContent();
    }

    /// <summary>
    /// Удаление боя
    /// </summary>
    /// <param name="id">ID боя</param>
    /// <returns>HTTP 204</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id) 
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}
