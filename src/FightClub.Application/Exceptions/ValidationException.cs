namespace FightClub.Application.Exceptions;

public class ValidationException(string message) : AppException(message, (int)HttpStatusCode.BadRequest)
{
}
