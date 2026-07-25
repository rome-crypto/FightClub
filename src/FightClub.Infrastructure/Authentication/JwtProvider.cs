using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FightClub.Application.Common.Interfaces.Authentication;
using FightClub.Application.Common.Options;
using FightClub.Application.Interfaces;
using FightClub.Domain.Entities.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FightClub.Infrastructure.Authentication;

internal sealed class JwtProvider(IOptions<JwtOptions> options, IDateTimeProvider dateTimeProvider) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public string GenerateAccessToken(User user)
    {
        // Создаем claims (утверждения) - это данные которые будут храниться в токене
        // Почему именно эти claims:
        // - NameIdentifier (sub) - стандартный claim для user ID
        // - Name - имя пользователя, удобно для отображения
        // - Email - email пользователя
        // - Role - роли пользователя (можно несколько)
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Name, user.UserName),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };

        // Добавляем роли в claims
        // Это нужно чтобы в контроллерах использовать [Authorize(Roles = "Admin")]
        // UserRole - это связующая таблица, в ней только RoleId
        // Для полноценной работы нужно будет загружать сами Role через Include
        foreach (UserRole userRole in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name ?? string.Empty));
        }

        // SigningCredentials - ключ для подписи токена
        // Используем HMAC-SHA256 (симметричное шифрование)
        // Почему HmacSha256: быстрый, безопасный, стандарт индустрии
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        // Создаем токен
        var token = new JwtSecurityToken(
            issuer: _options.Issuer,           // кто выдал токен (наш сервер)
            audience: _options.Audience,       // для кого токен (наше приложение)
            claims: claims,                     // данные в токене
            expires: _dateTimeProvider.UtcNow.AddMinutes(_options.ExpirationMinutes), // когда истекает
            signingCredentials: signingCredentials); // подпись

        // Преобразуем токен в строку
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        // Генерируем случайную строку для refresh token
        // Почему RandomNumberGenerator: криптографически стойкий генератор
        // Не используем Random или Guid - они предсказуемы
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        // Параметры валидации токена
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,              // проверяем issuer
            ValidateAudience = true,            // проверяем audience
            ValidateLifetime = true,            // проверяем срок действия
            ValidateIssuerSigningKey = true,    // проверяем подпись
            ValidIssuer = _options.Issuer,
            ValidAudience = _options.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_options.SecretKey)),
            ClockSkew = TimeSpan.Zero           // убираем допуск по времени (по умолчанию 5 минут)
        };

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out SecurityToken validatedToken);

            // Проверяем что это именно JWT токен с нашим алгоритмом
            if (validatedToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            // Токен невалидный - истек, подпись не совпадает, etc.
            return null;
        }
    }
}
