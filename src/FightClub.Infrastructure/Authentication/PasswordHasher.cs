using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FightClub.Infrastructure.Authentication;

internal class PasswordHasher
{
    public string HashPassword(string password)
    {
        // Implement your password hashing logic here
        // For example, you can use a library like BCrypt or PBKDF2
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
    }
}
