namespace FightClub.Application.Exceptions;

public class ValidationException : AppException
{
    public ValidationException(string message) 
        : base(message, (int)HttpStatusCode.BadRequest)
    {
    }
}
