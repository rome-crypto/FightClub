using FightClub.Application.Common.Interfaces.Authentication;
using FightClub.Application.Common.Options;
using FightClub.Application.DTOs.Auth;
using FightClub.Application.Exceptions;
using FightClub.Application.Interfaces;
using FightClub.Application.Specifications.Auth;
using FightClub.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FightClub.Application.Services;

public sealed class AuthService(
    IRepository<User> userRepository,
    IRepository<RefreshToken> refreshTokenRepository,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider,
    IOptions<JwtOptions> jwtOptions) : IAuthService
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IRepository<RefreshToken> _refreshTokenRepository = refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<AuthResponseDto> RegisterAsync(
        RegisterDto registerDto,
        CancellationToken cancellationToken = default)
    {
        // Проверяем что email уникальный
        // Почему AnyAsync: эффективнее чем FirstOrDefault - не загружает entity, только проверяет существование
        var emailExists = await _userRepository
            .Query(new UserByEmailSpecification(registerDto.Email))
            .AnyAsync(cancellationToken);

        if (emailExists)
        {
            throw new Exception("User with this email already exists");
        }

        // Хешируем пароль
        // Почему до сохранения: если что-то пойдет не так, пароль не попадет в БД в открытом виде
        var passwordHash = _passwordHasher.HashPassword(registerDto.Password);

        // Создаем пользователя
        var user = new User(
            registerDto.UserName,
            registerDto.Email,
            passwordHash);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        // Генерируем токены
        return await GenerateAuthResponse(user, cancellationToken);
    }

    public async Task<AuthResponseDto> LoginAsync(
        LoginDto loginDto,
        CancellationToken cancellationToken = default)
    {
        // Ищем пользователя по email
        // Почему не говорим "email not found": защита от enumeration attacks
        // Атакующий не должен знать существует ли email в системе
        User user = await _userRepository
            .Query(new UserByEmailSpecification(loginDto.Email))
            .Include(u => u.Roles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(cancellationToken) 
            ?? throw new Exception("Invalid email or password");

        // Проверяем активен ли пользователь
        if (!user.IsActive)
        {
            throw new Exception("User account is deactivated");
        }

        // Проверяем пароль
        var isValidPassword = _passwordHasher.VerifyPassword(
            loginDto.Password,
            user.PasswordHash);

        if (!isValidPassword)
        {
            throw new Exception("Invalid email or password");
        }

        // Генерируем токены
        return await GenerateAuthResponse(user, cancellationToken);
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(
        RefreshTokenDto refreshTokenDto,
        CancellationToken cancellationToken = default)
    {
        // Ищем refresh token в БД
        var refreshToken = await _refreshTokenRepository
            .Query(new RefreshTokenByTokenSpecification(refreshTokenDto.RefreshToken))
            .Include(rt => rt.UserId) // Почему Include: нам нужен User для генерации нового access token
            .FirstOrDefaultAsync(cancellationToken);

        if (refreshToken == null)
        {
            throw new Exception("Invalid refresh token");
        }

        // Проверяем что токен активен (не истек и не отозван)
        if (!refreshToken.IsActive)
        {
            throw new Exception("Refresh token is expired or revoked");
        }

        // Проверяем что пользователь активен
        var user = await _userRepository.GetByIdAsync(refreshToken.UserId);
        if (user == null || !user.IsActive)
        {
            throw new Exception("User account is deactivated");
        }

        // Отзываем старый refresh token
        // Почему отзываем: refresh token rotation - каждый refresh token можно использовать только раз
        // Это защищает от replay attacks
        refreshToken.Revoke(DateTime.UtcNow);
        _refreshTokenRepository.Update(refreshToken);

        // Генерируем новые токены
        return await GenerateAuthResponse(user, cancellationToken);
    }

    public async Task RevokeTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var token = await _refreshTokenRepository
            .Query(new RefreshTokenByTokenSpecification(refreshToken))
            .FirstOrDefaultAsync(cancellationToken);

        if (token == null)
        {
            throw new Exception("Invalid refresh token");
        }

        if (!token.IsActive)
        {
            throw new Exception("Refresh token is already revoked or expired");
        }

        token.Revoke(DateTime.UtcNow);
        _refreshTokenRepository.Update(token);
        await _refreshTokenRepository.SaveChangesAsync();
    }

    // Приватный метод для генерации токенов
    // Почему отдельный метод: DRY - используется в Register, Login и RefreshToken
    private async Task<AuthResponseDto> GenerateAuthResponse(
        User user,
        CancellationToken cancellationToken)
    {
        // Генерируем access token (JWT)
        var accessToken = _jwtProvider.GenerateAccessToken(user);

        // Генерируем refresh token (случайная строка)
        var refreshTokenString = _jwtProvider.GenerateRefreshToken();

        // Создаем entity для refresh token
        var refreshToken = new RefreshToken(
            user.Id,
            refreshTokenString,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays));

        // Сохраняем refresh token в БД
        await _refreshTokenRepository.AddAsync(refreshToken);
        await _refreshTokenRepository.SaveChangesAsync();

        // Возвращаем оба токена клиенту
        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenString,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes)
        };
    }
}
