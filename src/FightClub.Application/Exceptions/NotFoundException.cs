global using System.Net;

namespace FightClub.Application.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string message) 
        : base(message, (int)HttpStatusCode.NotFound)
    {
    }
}
