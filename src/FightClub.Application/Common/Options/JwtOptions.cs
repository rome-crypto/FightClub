namespace FightClub.Application.Common.Options;

public sealed class JwtOptions
{
    public string SecretKey { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int ExpirationMinutes { get; init; }
    public int RefreshTokenExpirationDays { get; init; }
}
