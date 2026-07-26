using FightClub.Application.DTOs.Auth;
using FightClub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FightClub.Api.Controllers;

/// <summary>
/// Контроллер аутентификации и авторизации
/// </summary>
/// <remarks>
/// Управляет регистрацией, входом, обновлением токенов и выходом из системы.
/// Использует JWT (JSON Web Tokens) для stateless-аутентификации с ротацией refresh-токенов.
/// 
/// Схема работы:
/// 1. Пользователь регистрируется или входит -> получает Access Token (15 мин) и Refresh Token (7 дней)
/// 2. Access Token используется для запросов к API (передаётся в заголовке Authorization)
/// 3. При истечении Access Token клиент использует Refresh Token для получения новой пары
/// 4. Refresh Token отзывается при выходе или при использовании в ротации
/// 
/// Особенности безопасности:
/// - Хеширование паролей с помощью BCrypt
/// - Ротация Refresh Token (каждый токен можно использовать только один раз)
/// - Access Token — stateless JWT с коротким временем жизни
/// - Защита от перебора (одинаковая ошибка для неверного email/пароля)
/// </remarks>
[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    /// <summary>
    /// Регистрация нового пользователя
    /// </summary>
    /// <remarks>
    /// Создаёт учётную запись и возвращает токены доступа.
    /// 
    /// Требования к паролю:
    /// - Минимум 6 символов
    /// - Максимум 100 символов
    /// - Как минимум одна буква (A-Z или a-z)
    /// - Как минимум одна цифра (0-9)
    /// 
    /// Пример запроса:
    /// 
    ///     POST /api/auth/register
    ///     {
    ///         "userName": "john_doe",
    ///         "email": "john@example.com",
    ///         "password": "SecurePass123"
    ///     }
    /// 
    /// Пример ответа (200 OK):
    /// 
    ///     {
    ///         "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    ///         "refreshToken": "a8f9d7e6c5b4a3f2e1d0c9b8a7f6e5d4c3b2a1f0e9d8c7b6a5f4e3d2c1b0a9f8e7d6",
    ///         "expiresAt": "2026-07-21T18:15:00.000Z"
    ///     }
    /// 
    /// Коды ошибок:
    /// - 400 Bad Request: Ошибка валидации (неверный email, слабый пароль и т.д.)
    /// - 409 Conflict: Пользователь с таким email уже существует
    /// </remarks>
    /// <param name="registerDto">Данные для регистрации</param>
    /// <returns>Токены доступа (access + refresh)</returns>
    [HttpPost("register")]
    [AllowAnonymous] 
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        AuthResponseDto result = await _authService.RegisterAsync(registerDto);
        return Ok(result);
    }

    /// <summary>
    /// Вход в систему
    /// </summary>
    /// <remarks>
    /// Проверяет учётные данные и возвращает токены доступа.
    /// 
    /// Пример запроса:
    /// 
    ///     POST /api/auth/login
    ///     {
    ///         "email": "john@example.com",
    ///         "password": "SecurePass123"
    ///     }
    /// 
    /// Пример ответа (200 OK):
    /// 
    ///     {
    ///         "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    ///         "refreshToken": "a8f9d7e6c5b4a3f2e1d0c9b8a7f6e5d4c3b2a1f0e9d8c7b6a5f4e3d2c1b0a9f8e7d6",
    ///         "expiresAt": "2026-07-21T18:15:00.000Z"
    ///     }
    /// 
    /// Коды ошибок:
    /// - 400 Bad Request: Ошибка валидации
    /// - 401 Unauthorized: Неверный email или пароль
    /// - 403 Forbidden: Учётная запись деактивирована
    /// 
    /// Примечание: Возвращается одинаковый код для неверного email/пароля,
    /// чтобы предотвратить перебор (enumeration attacks).
    /// </remarks>
    /// <param name="loginDto">Учётные данные</param>
    /// <returns>Токены доступа (access + refresh)</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        AuthResponseDto result = await _authService.LoginAsync(loginDto);
        return Ok(result);
    }

    /// <summary>
    /// Обновление access-токена с помощью refresh-токена
    /// </summary>
    /// <remarks>
    /// Когда access-токен истекает (через 15 минут), используйте этот метод для получения новой пары.
    /// 
    /// Как это работает:
    /// 1. Клиент отправляет валидный refresh-токен
    /// 2. Сервер проверяет его и убеждается, что он активен
    /// 3. Старый refresh-токен отзывается (ротация)
    /// 4. Выдаются новый access-токен и новый refresh-токен
    /// 
    /// Пример запроса:
    /// 
    ///     POST /api/auth/refresh
    ///     {
    ///         "refreshToken": "a8f9d7e6c5b4a3f2e1d0c9b8a7f6e5d4c3b2a1f0e9d8c7b6a5f4e3d2c1b0a9f8e7d6"
    ///     }
    /// 
    /// Пример ответа (200 OK):
    /// 
    ///     {
    ///         "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    ///         "refreshToken": "b9a8c7d6e5f4g3h2i1j0k9l8m7n6o5p4q3r2s1t0u9v8w7x6y5z4a3b2c1d0e9f8g7h6",
    ///         "expiresAt": "2026-07-21T18:30:00.000Z"
    ///     }
    /// 
    /// Коды ошибок:
    /// - 400 Bad Request: Ошибка валидации
    /// - 401 Unauthorized: Неверный или истёкший refresh-токен
    /// - 403 Forbidden: Учётная запись деактивирована
    /// 
    /// Безопасность: Ротация refresh-токенов предотвращает атаки повторного воспроизведения.
    /// Каждый refresh-токен можно использовать только один раз.
    /// </remarks>
    /// <param name="refreshTokenDto">Refresh-токен</param>
    /// <returns>Новая пара токенов (access + refresh)</returns>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        AuthResponseDto result = await _authService.RefreshTokenAsync(refreshTokenDto);
        return Ok(result);
    }

    /// <summary>
    /// Выход из системы (отзыв refresh-токена)
    /// </summary>
    /// <remarks>
    /// Отзывает переданный refresh-токен, чтобы его нельзя было использовать в дальнейшем.
    /// 
    /// Важные замечания:
    /// - Access-токен остаётся действительным до истечения срока (15 минут)
    /// - Access-токены — stateless JWT, их нельзя отозвать без дополнительной инфраструктуры
    /// - При выходе отзывается только refresh-токен
    /// - Клиент должен самостоятельно удалить оба токена на своей стороне
    /// 
    /// Пример запроса:
    /// 
    ///     POST /api/auth/logout
    ///     {
    ///         "refreshToken": "a8f9d7e6c5b4a3f2e1d0c9b8a7f6e5d4c3b2a1f0e9d8c7b6a5f4e3d2c1b0a9f8e7d6"
    ///     }
    /// 
    /// Пример ответа:
    /// - 204 No Content (успешно)
    /// 
    /// Коды ошибок:
    /// - 400 Bad Request: Ошибка валидации
    /// - 401 Unauthorized: Неверный refresh-токен
    /// 
    /// Примечание: Access-токен остаётся валидным до истечения срока, даже после выхода.
    /// Для полного выхода клиент должен удалить оба токена.
    /// </remarks>
    /// <param name="refreshTokenDto">Refresh-токен для отзыва</param>
    /// <returns>204 No Content при успехе</returns>
    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenDto refreshTokenDto)
    {
        await _authService.RevokeTokenAsync(refreshTokenDto.RefreshToken);
        return NoContent();
    }

    /// <summary>
    /// Получение информации о текущем пользователе
    /// </summary>
    /// <remarks>
    /// Возвращает данные о пользователе, извлечённые из JWT-токена.
    /// 
    /// Пример запроса:
    /// 
    ///     GET /api/auth/me
    ///     Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
    /// 
    /// Пример ответа (200 OK):
    /// 
    ///     {
    ///         "userId": "6d0e2f5a-3b8c-4d9e-8f7a-6b5c4d3e2f1a",
    ///         "userName": "john_doe",
    ///         "email": "john@example.com",
    ///         "claims": [
    ///             { "type": "sub", "value": "6d0e2f5a-3b8c-4d9e-8f7a-6b5c4d3e2f1a" },
    ///             { "type": "name", "value": "john_doe" },
    ///             { "type": "email", "value": "john@example.com" },
    ///             { "type": "role", "value": "Admin" },
    ///             { "type": "role", "value": "Manager" }
    ///         ]
    ///     }
    /// 
    /// Коды ошибок:
    /// - 401 Unauthorized: Отсутствует или истёк JWT-токен
    /// </remarks>
    /// <returns>Информация о текущем пользователе</returns>
    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        // User.Claims приходит из JWT токена
        // HttpContext.User заполняется автоматически JWT middleware
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

        return Ok(new
        {
            UserId = userId,
            UserName = userName,
            Email = email,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
}
