using FightClub.Application.DTOs.Common;
using FightClub.Application.DTOs.Fights;
using FightClub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FightClub.Api.Controllers;


/// <summary>
/// Контроллер управления боями
/// </summary>
/// <remarks>
/// Предоставляет операции по управлению боями: создание, выполнение, отмена и просмотр.
/// 
/// Уровни доступа:
/// - Admin: Полный доступ (создание, выполнение, отмена, удаление)
/// - Manager: Создание, выполнение, отмена, просмотр
/// - User: Только чтение
/// 
/// Жизненный цикл боя:
/// 1. Created — бой создан, но не запланирован
/// 2. Scheduled — бой запланирован на определённую дату
/// 3. InProgress — бой выполняется в данный момент
/// 4. Finished — бой завершён с результатом
/// 5. Cancelled — бой отменён
/// </remarks>
[ApiController]
[Route("api/fights")]
[Authorize]
public class FightController(
    IFightService service,
    IFightSimulationService simulation) : ControllerBase
{
    private readonly IFightService _service = service;
    private readonly IFightSimulationService _simulation = simulation;


    /// <summary>
    /// Получить список боёв с пагинацией и фильтрацией
    /// </summary>
    /// <remarks>
    /// Возвращает страницу боёв с возможностью фильтрации по статусу, боксёру и дате.
    /// 
    /// Пример запроса:
    /// 
    ///     GET /api/fights?page=1&pageSize=10&status=Scheduled&boxerId=6d0e2f5a-3b8c-4d9e-8f7a-6b5c4d3e2f1a&from=2026-07-01&to=2026-07-31
    ///     Authorization: Bearer {access_token}
    /// 
    /// Параметры запроса:
    /// - page: Номер страницы (по умолчанию: 1)
    /// - pageSize: Количество элементов на странице, максимум 100 (по умолчанию: 10)
    /// - status: Фильтр по статусу (Created, Scheduled, InProgress, Finished, Cancelled)
    /// - boxerId: Фильтр по участию конкретного боксёра
    /// - winnerId: Фильтр по победителю
    /// - from: Фильтр по дате начала (включительно)
    /// - to: Фильтр по дате окончания (включительно)
    /// - sortBy: Поле для сортировки (date, status)
    /// - sortOrder: Порядок сортировки Asc или Desc (по умолчанию: Asc)
    /// 
    /// Пример ответа (200 OK):
    /// 
    ///     {
    ///         "items": [
    ///             {
    ///                 "id": "8f2a4b7c-5d9e-6f0a-9h8c-7d6e5f4g3h2c",
    ///                 "boxerAId": "6d0e2f5a-3b8c-4d9e-8f7a-6b5c4d3e2f1a",
    ///                 "boxerBId": "7e1f3a6b-4c8d-5e9f-8g7b-6c5d4e3f2g1b",
    ///                 "winnerId": null,
    ///                 "status": "Scheduled",
    ///                 "endType": null,
    ///                 "plannedRounds": 12,
    ///                 "actualRounds": 0,
    ///                 "totalScoreA": 0,
    ///                 "totalScoreB": 0,
    ///                 "rounds": []
    ///             }
    ///         ],
    ///         "page": 1,
    ///         "pageSize": 10,
    ///         "totalCount": 1,
    ///         "totalPages": 1
    ///     }
    /// 
    /// Коды ошибок:
    /// - 401 Unauthorized: Отсутствует токен
    /// - 400 Bad Request: Некорректные параметры запроса
    /// </remarks>
    /// <param name="query">Параметры фильтрации и пагинации</param>
    /// <returns>Страница боёв</returns>
    [HttpGet]
    public async Task<ActionResult<PagedResult<FightResponseDto>>> Get([FromQuery] FightQueryDto query, CancellationToken cancellationToken)
    {
        PagedResult<FightResponseDto> result = await _service.GetPagedAsync(query, cancellationToken);

        return Ok(result);
    }


    /// <summary>
    /// Создать новый бой
    /// </summary>
    /// <remarks>
    /// Создаёт бой между двумя боксёрами.
    /// 
    /// Требуемые права: Admin или Manager
    /// 
    /// Пример запроса:
    /// 
    ///     POST /api/fights
    ///     Authorization: Bearer {access_token}
    ///     {
    ///         "boxerAId": "6d0e2f5a-3b8c-4d9e-8f7a-6b5c4d3e2f1a",
    ///         "boxerBId": "7e1f3a6b-4c8d-5e9f-8g7b-6c5d4e3f2g1b",
    ///         "rounds": 12
    ///     }
    /// 
    /// Пример ответа (201 Created):
    /// 
    ///     {
    ///         "id": "8f2a4b7c-5d9e-6f0a-9h8c-7d6e5f4g3h2c",
    ///         "boxerAId": "6d0e2f5a-3b8c-4d9e-8f7a-6b5c4d3e2f1a",
    ///         "boxerBId": "7e1f3a6b-4c8d-5e9f-8g7b-6c5d4e3f2g1b",
    ///         "winnerId": null,
    ///         "status": "Created",
    ///         "endType": null,
    ///         "plannedRounds": 12,
    ///         "actualRounds": 0,
    ///         "totalScoreA": 0,
    ///         "totalScoreB": 0,
    ///         "rounds": []
    ///     }
    /// 
    /// Правила валидации:
    /// - ID боксёров должны различаться
    /// - Оба боксёра должны существовать
    /// - Количество раундов: от 1 до 12
    /// 
    /// Коды ошибок:
    /// - 401 Unauthorized: Отсутствует токен
    /// - 403 Forbidden: Недостаточно прав (роль User)
    /// - 400 Bad Request: Ошибка валидации
    /// - 404 Not Found: Один или оба боксёра не найдены
    /// </remarks>
    /// <param name="data">Данные для создания боя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Созданный бой с заголовком Location</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<FightResponseDto>> Post([FromBody] FightCreateDto data, CancellationToken cancellationToken)
    {
        FightResponseDto fight = await _service.CreateAsync(data, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = fight.Id }, fight);
    }


    /// <summary>
    /// Запустить бой (симуляция)
    /// </summary>
    /// <remarks>
    /// Выполняет симуляцию боя раунд за раундом.
    /// 
    /// Требуемые права: Admin или Manager
    /// 
    /// Процесс выполнения:
    /// 1. Бой должен быть в статусе Scheduled
    /// 2. Каждый раунд симулируется со случайными очками (7–12)
    /// 3. После всех раундов победитель определяется по сумме очков
    /// 4. Обновляются рейтинги (ELO) и статистика боксёров
    /// 
    /// Пример запроса:
    /// 
    ///     POST /api/fights/8f2a4b7c-5d9e-6f0a-9h8c-7d6e5f4g3h2c/execute
    ///     Authorization: Bearer {access_token}
    /// 
    /// Пример ответа:
    /// - 204 No Content (успешно)
    /// 
    /// Коды ошибок:
    /// - 401 Unauthorized: Отсутствует токен
    /// - 403 Forbidden: Недостаточно прав (роль User)
    /// - 404 Not Found: Бой или боксёр не найдены
    /// - 400 Bad Request: Бой не запланирован или уже завершён
    /// 
    /// Примечание: После выполнения обновляются статистика и ELO боксёров.
    /// </remarks>
    /// <param name="id">ID боя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>204 No Content при успехе</returns>
    [HttpPost("{id}/execute")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Execute(Guid id, CancellationToken cancellationToken)
    {
        await _simulation.ExecuteAsync(id, cancellationToken);

        return NoContent();
    }


    /// <summary>
    /// Получить бой по ID
    /// </summary>
    /// <remarks>
    /// Возвращает полную информацию о бое, включая раунды и события.
    /// 
    /// Пример запроса:
    /// 
    ///     GET /api/fights/8f2a4b7c-5d9e-6f0a-9h8c-7d6e5f4g3h2c
    ///     Authorization: Bearer {access_token}
    /// 
    /// Пример ответа (200 OK) для завершённого боя:
    /// 
    ///     {
    ///         "id": "8f2a4b7c-5d9e-6f0a-9h8c-7d6e5f4g3h2c",
    ///         "boxerAId": "6d0e2f5a-3b8c-4d9e-8f7a-6b5c4d3e2f1a",
    ///         "boxerBId": "7e1f3a6b-4c8d-5e9f-8g7b-6c5d4e3f2g1b",
    ///         "winnerId": "6d0e2f5a-3b8c-4d9e-8f7a-6b5c4d3e2f1a",
    ///         "status": "Finished",
    ///         "endType": "Decision",
    ///         "plannedRounds": 12,
    ///         "actualRounds": 12,
    ///         "totalScoreA": 118,
    ///         "totalScoreB": 112,
    ///         "rounds": [
    ///             {
    ///                 "number": 1,
    ///                 "scoreA": 10,
    ///                 "scoreB": 9,
    ///                 "events": [
    ///                     {
    ///                         "type": "Punch",
    ///                         "boxerId": "6d0e2f5a-3b8c-4d9e-8f7a-6b5c4d3e2f1a",
    ///                         "occurredAt": "2026-07-21T18:00:00Z"
    ///                     }
    ///                 ]
    ///             }
    ///         ]
    ///     }
    /// 
    /// Коды ошибок:
    /// - 401 Unauthorized: Отсутствует токен
    /// - 404 Not Found: Бой не найден
    /// </remarks>
    /// <param name="id">ID боя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Детали боя с раундами и событиями</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<FightResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await _service.GetByIdAsync(id, cancellationToken));
    }


    /// <summary>
    /// Отменить бой
    /// </summary>
    /// <remarks>
    /// Отменяет запланированный бой.
    /// 
    /// Требуемые права: Admin или Manager
    /// 
    /// Пример запроса:
    /// 
    ///     POST /api/fights/8f2a4b7c-5d9e-6f0a-9h8c-7d6e5f4g3h2c/cancel
    ///     Authorization: Bearer {access_token}
    /// 
    /// Пример ответа:
    /// - 204 No Content (успешно)
    /// 
    /// Коды ошибок:
    /// - 401 Unauthorized: Отсутствует токен
    /// - 403 Forbidden: Недостаточно прав (роль User)
    /// - 404 Not Found: Бой не найден
    /// - 400 Bad Request: Бой не запланирован или уже завершён
    /// </remarks>
    /// <param name="id">ID боя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>204 No Content при успехе</returns>
    [HttpPost("{id}/cancel")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> CancelFight(Guid id, CancellationToken cancellationToken)
    {
        await _simulation.CancelAsync(id, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Удалить бой
    /// </summary>
    /// <remarks>
    /// Безвозвратно удаляет бой из системы.
    /// 
    /// Требуемые права: только Admin
    /// 
    /// Ограничения:
    /// - Нельзя удалять бои в статусе InProgress
    /// - Нельзя удалять завершённые бои (Finished)
    /// 
    /// Пример запроса:
    /// 
    ///     DELETE /api/fights/8f2a4b7c-5d9e-6f0a-9h8c-7d6e5f4g3h2c
    ///     Authorization: Bearer {access_token}
    /// 
    /// Пример ответа:
    /// - 204 No Content (успешно)
    /// 
    /// Коды ошибок:
    /// - 401 Unauthorized: Отсутствует токен
    /// - 403 Forbidden: Недостаточно прав (Manager или User)
    /// - 404 Not Found: Бой не найден
    /// - 400 Bad Request: Бой нельзя удалить (в процессе или завершён)
    /// </remarks>
    /// <param name="id">ID боя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>204 No Content при успехе</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
}
