using FightClub.Application.Common.Interfaces.Authentication;

namespace FightClub.Infrastructure.Authentication;

internal sealed class PasswordHasher : IPasswordHasher
{
    // workFactor определяет сложность хеширования
    // 12 = 2^12 итераций = 4096 итераций
    // Почему 12: баланс между безопасностью и скоростью
    // - меньше 10 - слишком быстро, уязвимо к brute-force
    // - больше 14 - слишком медленно для production
    // На современном железе 12 = ~250ms на хеш (приемлемо для логина)
    private const int WorkFactor = 12;

    public string HashPassword(string password)
    {
        // BCrypt.Net автоматически:
        // 1. Генерирует случайную соль (salt)
        // 2. Комбинирует пароль + соль
        // 3. Прогоняет через алгоритм Blowfish 2^workFactor раз
        // 4. Возвращает строку вида: $2a$12$[22 символа соли][31 символ хеша]
        //
        // Соль хранится прямо в результате - это нормально и безопасно!
        // Соль не секретная информация, она нужна чтобы одинаковые пароли
        // имели разные хеши
        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        // BCrypt.Net.BCrypt.Verify:
        // 1. Извлекает соль из passwordHash
        // 2. Хеширует введенный password с этой солью
        // 3. Сравнивает результат с хешем из passwordHash
        //
        // Почему не просто ==: хеши нужно сравнивать через timing-safe compare
        // чтобы избежать timing attacks
        // BCrypt.Verify делает это автоматически
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        catch
        {
            // Если passwordHash некорректный формат - вернем false
            return false;
        }
    }
}
