using FightClub.Application.DTOs.Trainers;
using FightClub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FightClub.Api.Controllers;

/// <summary>
/// Контроллер управления тренерами
/// </summary>
/// <remarks>
/// Предоставляет CRUD-операции для тренеров с разграничением доступа по ролям.
/// 
/// Уровни доступа:
/// - Admin: Полный доступ (CRUD)
/// - Manager: Чтение, создание, обновление
/// - User: Только чтение
/// 
/// Все методы требуют аутентификации.
/// </remarks>
[ApiController]
[Route("api/trainers")]
[Authorize]
public class TrainersController(ITrainerService service) : ControllerBase
{
    private readonly ITrainerService _service = service;

    /// <summary>
    /// Создать нового тренера
    /// </summary>
    /// <remarks>
    /// Добавляет нового тренера в систему.
    /// 
    /// Требуемые права: Admin или Manager
    /// 
    /// Пример запроса:
    /// 
    ///     POST /api/trainers
    ///     Authorization: Bearer {access_token}
    ///     {
    ///         "firstName": "Константин",
    ///         "lastName": "Цзю",
    ///         "birthDate": "1969-09-19T00:00:00Z"
    ///     }
    /// 
    /// Пример ответа (201 Created):
    /// 
    ///     {
    ///         "id": "9h4c6d8e-7f0a-1b2c-3d4e-5f6g7h8i9j0k",
    ///         "firstName": "Константин",
    ///         "lastName": "Цзю",
    ///         "fullName": "Константин Цзю",
    ///         "age": 56
    ///     }
    /// 
    /// Правила валидации:
    /// - Имя: 2–50 символов
    /// - Фамилия: 2–50 символов
    /// - Возраст: 18–100 лет
    /// 
    /// Коды ошибок:
    /// - 401 Unauthorized: Отсутствует токен
    /// - 403 Forbidden: Недостаточно прав (роль User)
    /// - 400 Bad Request: Ошибка валидации
    /// </remarks>
    /// <param name="dto">Данные для создания тренера</param>
    /// <returns>Созданный тренер с заголовком Location</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Post(TrainerCreateDto dto)
    {
        TrainerResponseDto result = await _service.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Получить тренера по ID
    /// </summary>
    /// <remarks>
    /// Возвращает информацию о конкретном тренере.
    /// 
    /// Пример запроса:
    /// 
    ///     GET /api/trainers/9h4c6d8e-7f0a-1b2c-3d4e-5f6g7h8i9j0k
    ///     Authorization: Bearer {access_token}
    /// 
    /// Пример ответа (200 OK):
    /// 
    ///     {
    ///         "id": "9h4c6d8e-7f0a-1b2c-3d4e-5f6g7h8i9j0k",
    ///         "firstName": "Константин",
    ///         "lastName": "Цзю",
    ///         "fullName": "Константин Цзю",
    ///         "age": 56
    ///     }
    /// 
    /// Коды ошибок:
    /// - 401 Unauthorized: Отсутствует токен
    /// - 404 Not Found: Тренер не найден
    /// </remarks>
    /// <param name="id">ID тренера</param>
    /// <returns>Данные тренера</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>
    /// Получить список тренеров с пагинацией и фильтрацией
    /// </summary>
    /// <remarks>
    /// Возвращает страницу тренеров с возможностью поиска и фильтрации.
    /// 
    /// Пример запроса:
    /// 
    ///     GET /api/trainers?page=1&pageSize=10&search=цзю&minAge=40&maxAge=60&sortBy=lastname&sortOrder=Asc
    ///     Authorization: Bearer {access_token}
    /// 
    /// Параметры запроса:
    /// - page: Номер страницы (по умолчанию: 1)
    /// - pageSize: Количество элементов на странице, максимум 100 (по умолчанию: 10)
    /// - search: Поиск по имени и фамилии
    /// - minAge: Минимальный возраст
    /// - maxAge: Максимальный возраст
    /// - sortBy: Поле для сортировки (firstname, lastname, age)
    /// - sortOrder: Порядок сортировки Asc или Desc (по умолчанию: Asc)
    /// 
    /// Пример ответа (200 OK):
    /// 
    ///     {
    ///         "items": [
    ///             {
    ///                 "id": "9h4c6d8e-7f0a-1b2c-3d4e-5f6g7h8i9j0k",
    ///                 "firstName": "Константин",
    ///                 "lastName": "Цзю",
    ///                 "fullName": "Константин Цзю",
    ///                 "age": 56
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
    /// <returns>Страница тренеров</returns>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] TrainerQueryDto query)
    {
        return Ok(await _service.GetPagedAsync(query));
    }

    /// <summary>
    /// Обновить данные тренера
    /// </summary>
    /// <remarks>
    /// Частичное обновление информации о тренере. Все поля необязательны.
    /// 
    /// Требуемые права: Admin или Manager
    /// 
    /// Пример запроса:
    /// 
    ///     PATCH /api/trainers/9h4c6d8e-7f0a-1b2c-3d4e-5f6g7h8i9j0k
    ///     Authorization: Bearer {access_token}
    ///     {
    ///         "firstName": "Константин",
    ///         "lastName": "Цзю-старший"
    ///     }
    /// 
    /// Пример ответа:
    /// - 204 No Content (успешно)
    /// 
    /// Коды ошибок:
    /// - 401 Unauthorized: Отсутствует токен
    /// - 403 Forbidden: Недостаточно прав (роль User)
    /// - 404 Not Found: Тренер не найден
    /// - 400 Bad Request: Ошибка валидации
    /// </remarks>
    /// <param name="id">ID тренера</param>
    /// <param name="dto">Данные для обновления (все поля опциональны)</param>
    /// <returns>204 No Content при успехе</returns>
    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Patch(Guid id, TrainerUpdateDto dto)
    {
        await _service.UpdateAsync(id, dto);

        return NoContent();
    }

    /// <summary>
    /// Удалить тренера
    /// </summary>
    /// <remarks>
    /// Безвозвратно удаляет тренера из системы.
    /// 
    /// Требуемые права: только Admin
    /// 
    /// Пример запроса:
    /// 
    ///     DELETE /api/trainers/9h4c6d8e-7f0a-1b2c-3d4e-5f6g7h8i9j0k
    ///     Authorization: Bearer {access_token}
    /// 
    /// Пример ответа:
    /// - 204 No Content (успешно)
    /// 
    /// Коды ошибок:
    /// - 401 Unauthorized: Отсутствует токен
    /// - 403 Forbidden: Недостаточно прав (Manager или User)
    /// - 404 Not Found: Тренер не найден
    /// 
    /// Внимание: Операция необратима.
    /// </remarks>
    /// <param name="id">ID тренера</param>
    /// <returns>204 No Content при успехе</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}
