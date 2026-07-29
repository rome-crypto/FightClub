using FightClub.Application.Specifications.Common;
using FightClub.Domain.Identity;

namespace FightClub.Application.Specifications.Auth;

public sealed class UserByEmailSpecification : BaseSpecification<User>
{
    public UserByEmailSpecification(string email)
    {
        // AddCriteria задает WHERE условие в SQL запросе
        // Это будет: WHERE Email = @email
        AddCriteria(u => u.Email == email);

        // AddInclude делает EAGER loading связанных данных
        // Это будет JOIN с таблицами Roles и их Permissions
        // Почему нужно: для генерации JWT токена нужны роли пользователя
        AddInclude(u => u.Roles);
    }
}
