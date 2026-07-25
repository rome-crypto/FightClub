using FightClub.Application.DTOs.Auth;
using FightClub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FightClub.Api.Controllers;

/// <summary>
/// Контроллер для аутентификации и авторизации
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Регистрация нового пользователя
    /// </summary>
    /// <remarks>
    /// Создает нового пользователя и возвращает access + refresh токены
    ///
    /// Требования к паролю:
    /// - Минимум 6 символов
    /// - Хотя бы одна буква и одна цифра
    /// </remarks>
    /// <param name="registerDto">Данные для регистрации (username, email, password)</param>
    /// <returns>HTTP 200 и токены авторизации</returns>
    [HttpPost("register")]
    [AllowAnonymous] // Почему AllowAnonymous: любой может зарегистрироваться
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);
        return Ok(result);
    }

    /// <summary>
    /// Вход в систему
    /// </summary>
    /// <remarks>
    /// Проверяет email и пароль, возвращает access + refresh токены
    /// </remarks>
    /// <param name="loginDto">Данные для входа (email, password)</param>
    /// <returns>HTTP 200 и токены авторизации</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);
        return Ok(result);
    }

    /// <summary>
    /// Обновление access токена с помощью refresh токена
    /// </summary>
    /// <remarks>
    /// Когда access token истекает (15 минут), клиент использует refresh token
    /// чтобы получить новую пару токенов без повторного ввода пароля.
    ///
    /// Refresh Token Rotation: старый refresh token отзывается, выдается новый.
    /// Это защищает от replay attacks - каждый refresh token можно использовать только раз.
    /// </remarks>
    /// <param name="refreshTokenDto">Refresh token</param>
    /// <returns>HTTP 200 и новые токены авторизации</returns>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        var result = await _authService.RefreshTokenAsync(refreshTokenDto);
        return Ok(result);
    }

    /// <summary>
    /// Выход из системы (отзыв refresh токена)
    /// </summary>
    /// <remarks>
    /// Отзывает refresh token чтобы его нельзя было больше использовать.
    /// Access token при этом остается валидным до истечения срока (15 минут).
    ///
    /// Почему так: access token - это JWT, он stateless, сервер не может его "отозвать"
    /// без сложной инфраструктуры (redis blacklist). Поэтому мы отзываем только refresh token.
    /// </remarks>
    /// <param name="refreshTokenDto">Refresh token для отзыва</param>
    /// <returns>HTTP 204 No Content</returns>
    [HttpPost("logout")]
    [AllowAnonymous] // Почему AllowAnonymous: можно логаутиться даже если токен истек
    public async Task<IActionResult> Logout([FromBody] RefreshTokenDto refreshTokenDto)
    {
        await _authService.RevokeTokenAsync(refreshTokenDto.RefreshToken);
        return NoContent();
    }

    /// <summary>
    /// Тестовый endpoint для проверки авторизации
    /// </summary>
    /// <remarks>
    /// Доступен только авторизованным пользователям.
    /// Используй для проверки что JWT токен работает.
    ///
    /// Отправь в заголовке: Authorization: Bearer {твой_access_token}
    /// </remarks>
    /// <returns>HTTP 200 и информация о текущем пользователе</returns>
    [HttpGet("me")]
    [Authorize] // Почему Authorize: только для авторизованных пользователей
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
