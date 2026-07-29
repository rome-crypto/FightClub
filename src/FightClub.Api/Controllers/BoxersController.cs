using FightClub.Application.DTOs.Boxers;
using FightClub.Application.DTOs.Common;
using FightClub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FightClub.Api.Controllers;


/// <summary>
/// Контроллер управления боксёрами
/// </summary>
/// <remarks>
/// Предоставляет CRUD-операции для боксёров с разграничением доступа по ролям.
/// 
/// Уровни доступа:
/// - Admin: Полный доступ (CRUD)
/// - Manager: Чтение, создание, обновление
/// - User: Только чтение
/// 
/// Все методы требуют аутентификации, если не указано иное.
/// </remarks>
[ApiController]
[Route("api/boxers")]
[Authorize]
public class BoxersController(IBoxerService service) : ControllerBase
{
    private readonly IBoxerService _boxerService = service;

    /// <summary>
    /// Получить боксёра по ID
    /// </summary>
    /// <remarks>
    /// Возвращает информацию о конкретном боксёре.
    /// 
    /// Пример запроса:
    /// 
    ///     GET /api/boxers/6d0e2f5a-3b8c-4d9e-8f7a-6b5c4d3e2f1a
    ///     Authorization: Bearer {access_token}
    /// 
    /// Пример ответа (200 OK):
    /// 
    ///     {
    ///         "id": "6d0e2f5a-3b8c-4d9e-8f7a-6b5c4d3e2f1a",
    ///         "firstName": "Майк",
    ///         "lastName": "Тайсон",
    ///         "fullName": "Майк Тайсон",
    ///         "age": 58,
    ///         "weight": 100,
    ///         "weightCategory": "Heavyweight",
    ///         "trainerId": "7e1f3a6b-4c8d-5e9f-8g7b-6c5d4e3f2g1b",
    ///         "eloRating": 1850,
    ///         "wins": 50,
    ///         "losses": 6,
    ///         "draws": 0,
    ///         "winRate": 89.29
    ///     }
    /// 
    /// Коды ошибок:
    /// - 401 Unauthorized: Отсутствует токен
    /// - 404 Not Found: Боксёр не найден
    /// </remarks>
    /// <param name="id">ID боксёра</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Данные боксёра</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<BoxerResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        BoxerResponseDto result = await _boxerService.GetByIdAsync(id, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Создать нового боксёра
    /// </summary>
    /// <remarks>
    /// Добавляет нового боксёра в систему.
    /// 
    /// Требуемые права: Admin или Manager
    /// 
    /// Пример запроса:
    /// 
    ///     POST /api/boxers
    ///     Authorization: Bearer {access_token}
    ///     {
    ///         "firstName": "Мухаммед",
    ///         "lastName": "Али",
    ///         "birthDate": "1942-01-17T00:00:00Z",
    ///         "weight": 95,
    ///         "trainerId": "7e1f3a6b-4c8d-5e9f-8g7b-6c5d4e3f2g1b"
    ///     }
    /// 
    /// Пример ответа (201 Created):
    /// 
    ///     {
    ///         "id": "8f2a4b7c-5d9e-6f0a-9h8c-7d6e5f4g3h2c",
    ///         "firstName": "Мухаммед",
    ///         "lastName": "Али",
    ///         "fullName": "Мухаммед Али",
    ///         "age": 82,
    ///         "weight": 95,
    ///         "weightCategory": "Heavyweight",
    ///         "trainerId": "7e1f3a6b-4c8d-5e9f-8g7b-6c5d4e3f2g1b",
    ///         "eloRating": 1500,
    ///         "wins": 0,
    ///         "losses": 0,
    ///         "draws": 0,
    ///         "winRate": 0
    ///     }
    /// 
    /// Правила валидации:
    /// - Имя: 2–50 символов
    /// - Фамилия: 2–50 символов
    /// - Возраст: 18–80 лет
    /// - Вес: 30–200 кг
    /// 
    /// Коды ошибок:
    /// - 401 Unauthorized: Отсутствует токен
    /// - 403 Forbidden: Недостаточно прав (роль User)
    /// - 400 Bad Request: Ошибка валидации
    /// </remarks>
    /// <param name="data">Данные для создания боксёра</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Созданный боксёр с заголовком Location</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<BoxerResponseDto>> Post([FromBody] BoxerCreateDto data, CancellationToken cancellationToken)
    {
        BoxerResponseDto boxer = await _boxerService.CreateAsync(data, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = boxer.Id }, boxer);
    }

    /// <summary>
    /// Обновить данные боксёра
    /// </summary>
    /// <remarks>
    /// Частичное обновление информации о боксёре. Все поля необязательны.
    /// 
    /// Требуемые права: Admin или Manager
    /// 
    /// Пример запроса:
    /// 
    ///     PATCH /api/boxers/6d0e2f5a-3b8c-4d9e-8f7a-6b5c4d3e2f1a
    ///     Authorization: Bearer {access_token}
    ///     {
    ///         "firstName": "Майкл",
    ///         "weight": 105,
    ///         "trainerId": "9g3b5c8d-6e0f-7a1b-9h8c-8d7e6f5g4h3d"
    ///     }
    /// 
    /// Пример ответа:
    /// - 204 No Content (успешно)
    /// 
    /// Коды ошибок:
    /// - 401 Unauthorized: Отсутствует токен
    /// - 403 Forbidden: Недостаточно прав (роль User)
    /// - 404 Not Found: Боксёр не найден
    /// - 400 Bad Request: Ошибка валидации
    /// </remarks>
    /// <param name="id">ID боксёра</param>
    /// <param name="data">Данные для обновления (все поля опциональны)</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>204 No Content при успехе</returns>
    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Patch(Guid id, [FromBody] BoxerUpdateDto data, CancellationToken cancellationToken)
    {
        await _boxerService.UpdateAsync(id, data, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Удалить боксёра
    /// </summary>
    /// <remarks>
    /// Безвозвратно удаляет боксёра из системы.
    /// 
    /// Требуемые права: только Admin
    /// 
    /// Пример запроса:
    /// 
    ///     DELETE /api/boxers/6d0e2f5a-3b8c-4d9e-8f7a-6b5c4d3e2f1a
    ///     Authorization: Bearer {access_token}
    /// 
    /// Пример ответа:
    /// - 204 No Content (успешно)
    /// 
    /// Коды ошибок:
    /// - 401 Unauthorized: Отсутствует токен
    /// - 403 Forbidden: Недостаточно прав (Manager или User)
    /// - 404 Not Found: Боксёр не найден
    /// 
    /// Внимание: Операция необратима.
    /// </remarks>
    /// <param name="id">ID боксёра</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>204 No Content при успехе</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _boxerService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Получить список боксёров с пагинацией и фильтрацией
    /// </summary>
    /// <remarks>
    /// Возвращает страницу боксёров с возможностью поиска и фильтрации.
    /// 
    /// Пример запроса:
    /// 
    ///     GET /api/boxers?page=1&pageSize=10&search=тайсон&weightCategory=Heavyweight&minAge=20&maxAge=40&sortBy=elo&sortOrder=Desc
    ///     Authorization: Bearer {access_token}
    /// 
    /// Параметры запроса:
    /// - page: Номер страницы (по умолчанию: 1)
    /// - pageSize: Количество элементов на странице, максимум 100 (по умолчанию: 10)
    /// - search: Поиск по имени и фамилии
    /// - weightCategory: Фильтр по весовой категории (Lightweight, Middleweight, Heavyweight)
    /// - minAge: Минимальный возраст
    /// - maxAge: Максимальный возраст
    /// - sortBy: Поле для сортировки (firstname, lastname, age, weight, elo, wins)
    /// - sortOrder: Порядок сортировки Asc или Desc (по умолчанию: Asc)
    /// 
    /// Пример ответа (200 OK):
    /// 
    ///     {
    ///         "items": [
    ///             {
    ///                 "id": "6d0e2f5a-3b8c-4d9e-8f7a-6b5c4d3e2f1a",
    ///                 "firstName": "Майк",
    ///                 "lastName": "Тайсон",
    ///                 "fullName": "Майк Тайсон",
    ///                 "age": 58,
    ///                 "weight": 100,
    ///                 "weightCategory": "Heavyweight",
    ///                 "trainerId": "7e1f3a6b-4c8d-5e9f-8g7b-6c5d4e3f2g1b",
    ///                 "eloRating": 1850,
    ///                 "wins": 50,
    ///                 "losses": 6,
    ///                 "draws": 0,
    ///                 "winRate": 89.29
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
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Страница боксёров</returns>
    [HttpGet]
    public async Task<ActionResult<PagedResult<BoxerResponseDto>>> Get([FromQuery] BoxerQueryDto query, CancellationToken cancellationToken)
    {
        PagedResult<BoxerResponseDto> result = await _boxerService.GetPagedAsync(query, cancellationToken);

        return Ok(result);
    }
}
